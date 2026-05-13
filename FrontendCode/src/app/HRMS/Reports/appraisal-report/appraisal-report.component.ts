import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HrmsServiceService } from '../../hrms-service.service';
import { payRollService } from '../../service/payroll.service';
import { EntityStateService } from '../../service/entity-state.service';

@Component({
  selector: 'app-appraisal-report',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, NgxPaginationModule],
  templateUrl: './appraisal-report.component.html',
  styleUrl: './appraisal-report.component.scss'
})
export class AppraisalReportComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  appraisalReportForm: any = FormGroup;
  employeeDetails;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any
  getLocations: any;
  errorMessageLocation: any
  getDepartementName = [];
  searchValue: string = '';
  dropdownVisible = false;
  isTableData: boolean = false;
  errorMessage: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  // rows: any[] = [];


  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;

  constructor(
    private accessPolicyStoreService: AccessPolicyStoreService, private payrollService: payRollService,
    private readonly fb: FormBuilder, 
    private readonly hrmsService: HrmsServiceService,
  private entityStateService: EntityStateService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Reports'
      );
    });
  }
  ngOnInit(): void {
    setTimeout(() => {
      this.payrollDropdwonLocation();
      this.dropdwon_department();
    }, 100);
    setTimeout(() => {
      this.getEmployeeSelectEmployee();
    }, 300);
    this.appraisalReportForm = this.fb.group({
      emloyee: ['',],
      emloyeeCode: ['',],
      division: ['',],
      DeptName: ['',],
      Location: ['',],
    })
  }

  //this is Employee list
  getEmployeeSelectEmployee() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsService.visitorAccessDDEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.errorMessageEmpName = ''
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
          this.errorMessageEmpName = 'No Data Found.'
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessageEmpName = 'Error loading data. Please try again.';
        this.isSpinner = false;
      }
    });
  }
  filterEmployees() {
    if (this.searchText) {
      this.filteredEmployees = this.employees.filter((employee: any) =>
        employee.EmpName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        employee.EmpCode.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employees];
    }
  }
  selectEmployeee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.appraisalReportForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.appraisalReportForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.appraisalReportForm.get('emloyee')?.setErrors(null);
    }
  }
  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  openDropdown() {
    this.isDropdownOpen = true;
    this.filteredEmployees = [...this.employees];
  }
  closeDropdown() {
    setTimeout(() => {
      this.isDropdownOpen = false;
    }, 200);
  }
  //this is Employee list

  payrollDropdwonLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity:this.entityStateService.getEntityId(),
    }
    this.isSpinner = true;
    this.getLocations = [];
    setTimeout(() => {
      this.payrollService.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.getLocations = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = []
        }
      },
        error => {
          this.errorMessageLocation = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner = false;
        })
    }, 100);
  }

  dropdwon_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;
    this.hrmsService.access_DD_department(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDepartementName = res;
        } else {
          this.triggerToast('', 'Record Not Found', 'Warning');
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Department List', 'danger');
        this.isSpinner = false;
      }
    });
  }

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const isDropdown = target.closest('.dropdown-content') !== null;
    const isButton = target.matches('.export-button');
    if (!isDropdown && !isButton) {
      this.dropdownVisible = false;
    }
  }
  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
  }
  // Listen for clicks anywhere in the document

  exportFile(format: string) {
    if (format === 'excel') {
      this.exportToExcel();
    }
    if (format === 'pdf') {
      this.exportToPDF()
    }
  }

  exportToExcel(){

  }

  exportToPDF(){
    
  }

  submitFilterData() {

  }

  applyFilter() {
  }
  resetData() {
    this.isFormSubmitted = false;
    this.appraisalReportForm.reset();
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
