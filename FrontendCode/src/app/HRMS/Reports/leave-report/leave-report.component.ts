import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { payRollService } from '../../service/payroll.service';
import { NgxPaginationModule } from 'ngx-pagination';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { EntityStateService } from '../../service/entity-state.service';
@Component({
  selector: 'app-leave-report',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, NgxPaginationModule],
  templateUrl: './leave-report.component.html',
  styleUrl: './leave-report.component.scss'
})
export class LeaveReportComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  leaveReportForm: any = FormGroup;
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

  rows = [
    {
      name: 'John Doe',
      code: '1234',
      division: 'Sales',
      department: 'Marketing',
      location: 'New York',
      totalEL: 12,
      totalCL: 8,
      leavesUtilized: [1, 0, 2, 1, 1, 0, 1, 0, 1, 1, 1, 1],
      leaveBalanceCL: 2,
      leaveBalanceEL: 5
    },
    {
      name: 'Jane Smith',
      code: '5678',
      division: 'Finance',
      department: 'Accounts',
      location: 'London',
      totalEL: 15,
      totalCL: 10,
      leavesUtilized: [2, 1, 1, 2, 1, 0, 1, 1, 0, 2, 1, 1],
      leaveBalanceCL: 4,
      leaveBalanceEL: 11
    },
    {
      name: 'Michael Lee',
      code: '0001',
      division: 'IT',
      department: 'Development',
      location: 'Tokyo',
      totalEL: 18,
      totalCL: 12,
      leavesUtilized: [1, 2, 1, 2, 1, 2, 1, 1, 2, 1, 1, 2],
      leaveBalanceCL: 10,
      leaveBalanceEL: 8
    }
  ];

  constructor(
    private accessPolicyStoreService: AccessPolicyStoreService, private payrollService: payRollService,
    private readonly fb: FormBuilder, private readonly hrmsService: HrmsServiceService,
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
    }, 200);
    setTimeout(() => {
      this.getEmployeeSelectEmployee();
    }, 300);
    this.leaveReportForm = this.fb.group({
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
    this.leaveReportForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.leaveReportForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.leaveReportForm.get('emloyee')?.setErrors(null);
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
      AuthorisedEntity: this.entityStateService.getEntityId(),
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

  exportToExcel(): void {
    if (this.rows.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }

    // Flatten leavesUtilized into month columns
    const filteredData = this.rows.map((item: any) => {
      const monthLeaves: any = {};
      const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
      months.forEach((month, idx) => {
        monthLeaves[month] = item.leavesUtilized[idx] || 0;
      });

      return {
        "Name": item.name,
        "Code": item.code,
        "Division": item.division,
        "Department": item.department,
        "Location": item.location,
        "Total EL": item.totalEL,
        "Total CL": item.totalCL,
        ...monthLeaves,
        "CL Balance": item.leaveBalanceCL,
        "EL Balance": item.leaveBalanceEL
      };
    });

    // Create empty worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.aoa_to_sheet([]);

    // Add header rows
    XLSX.utils.sheet_add_aoa(worksheet, [
      ["Name", "Code", "Division", "Department", "Location", "Total Leaves", "", "Leaves Utilized", "", "", "", "", "", "", "", "", "", "", "", "Leave Balance", ""],
      ["", "", "", "", "", "EL", "CL", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "CL", "EL"]
    ], { origin: 'A1' });

    // Add JSON data starting from row 3
    XLSX.utils.sheet_add_json(worksheet, filteredData, { origin: 'A3', skipHeader: true });

    // Merge cells like your HTML table
    worksheet['!merges'] = [
      { s: { r: 0, c: 5 }, e: { r: 0, c: 6 } },   // Total Leaves (EL, CL)
      { s: { r: 0, c: 7 }, e: { r: 0, c: 18 } },  // Leaves Utilized (Jan–Dec)
      { s: { r: 0, c: 19 }, e: { r: 0, c: 20 } }  // Leave Balance (CL, EL)
    ];

    // Create workbook and save
    const workbook: XLSX.WorkBook = { Sheets: { 'Leave Summary': worksheet }, SheetNames: ['Leave Summary'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'Leave Summary.xlsx');
    this.dropdownVisible = false;
  }

  exportToPDF(): void {
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
  }


  submitFilterData() {

  }

  applyFilter() {
  }
  resetData() {
    this.isFormSubmitted = false;
    this.leaveReportForm.reset();
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
