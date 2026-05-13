import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { AppraisalReportComponent } from '../appraisal-report/appraisal-report.component';
import { LeaveReportComponent } from '../leave-report/leave-report.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { LeaveBalanceComponent } from '../leave-balance/leave-balance.component';
import { WorkingDaysReportsComponent } from '../working-days-reports/working-days-reports.component';

@Component({
  selector: 'app-emp-mas-report',
  standalone: true,
  imports: [SharedModule, CommonModule, ToastMessageComponent, 
    AppraisalReportComponent, LeaveReportComponent],
  templateUrl: './emp-mas-report.component.html',
  styleUrl: './emp-mas-report.component.scss'
})
export class EmpMasReportComponent implements OnInit {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  isSpinner: boolean = false;
  empMasterReportForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  searchValue: string = '';
  dropdownVisible = false;
  today = new Date().toISOString().split('T')[0];


  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;

  constructor(private readonly fb: FormBuilder,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly hrmsService: HrmsServiceService) {
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
      this.dropdwon_department();
    }, 150);
    setTimeout(() => {
      this.getEmployeeSelectEmployee();
    }, 200);
    
    this.empMasterReportForm = this.fb.group({
      emloyee: ['',],
      emloyeeCode: ['',],
      gender: ['',],
      EmailId: ['',],
      DOJ: ['',],
      DeptName: ['',],
      Designation: ['',],
    })
  }

  selectedTab = 0;
  selectTab(index: number) {
    this.selectedTab = index;
    if (index === 0) {
      this.getEmployeeSelectEmployee();
      this.callDDDesignation();
    }
  }
  tabs = [
    {
      label: 'Employee Master Report',
      icon: 'feather icon-users'
    },
    {
      label: 'Appraisal Report',
      icon: 'feather icon-award'
    },
    {
      label: 'Leave Report',
      icon: 'feather icon-calendar'
    },
    // {
    //   label: 'Leave Balance Report',
    //   icon: 'feather icon-clipboard'
    // },
    // {
    //   label: 'Option Reports',
    //   icon: 'feather icon-clipboard'
    // }
  ];

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
    this.empMasterReportForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.empMasterReportForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.empMasterReportForm.get('emloyee')?.setErrors(null);
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

  callDDDesignation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.empMasterReportForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
    this.hrmsService.access_DDDesignation(reqBody).subscribe({
      next: (res: any) => {
        this.getDepartementRole = res;
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error loading Designation', 'danger');
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

  applyFilter() {

  }

  submitFilterData() {

  }

  resetData() {

  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
