import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { AttendenceModuleService } from '../../service/attendence.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-wfh-mode',
  standalone: true,
  imports: [FormsModule, CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule],
  templateUrl: './wfh-mode.component.html',
  styleUrl: './wfh-mode.component.scss'
})
export class WfhModeComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  wfhModeForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  years: string[] = [];
  months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  categories = ['Permanent', 'Contract', 'Temporary'];
  employeeDetails;
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  isSpinner: boolean = false;
  isTableData: boolean = false;
  getDDCompany: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  searchText: string = '';
  isDropdownOpen = false;
  filteredEmployees: any[] = [];
  employees: any;
  today = new Date().toISOString().split('T')[0];
  minDate: string | undefined;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  isValidEmployee: boolean = true;
  selectedEmployee: any = null;
  viewdata: any = {};
  errMsgLocation: any;
  getLocations: any[] = [];
  accessPolicy: any
  controlAccessPage: any
  searchValue: any;

  constructor(
    private readonly fb: FormBuilder,
    private readonly hrmsService: EmployeeModuleService,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly hrmsServiceAttendance: AttendenceModuleService,
    private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'WFH Mode'
      );
    });

  }
  ngOnInit(): void {
    this.wfhModeForm = this.fb.group({
      // company: [''],
      location: [''],
      DeptName: [''],
      teams: [''],
      Designation: [''],
      date_from: [''],
      date_to: [''],
      employee: ['']
    }, { validators: this.dateRangeValidator });
    setTimeout(() => {
      // this.employee_DD_Company();
    }, 100);
    setTimeout(() => {
      // this.employeeGetDDLocationApi();
      this.access_DD_department();
    }, 200);
    setTimeout(() => {
      this.getAllWFHDetails();
    }, 1000);
    this.initializeYears();
    this.wfhModeForm?.get('date_from')?.valueChanges.subscribe((value: any) => {
      if (value) {
        this.wfhModeForm?.get('date_to')?.setValidators([Validators.required]);
      } else {
        this.wfhModeForm?.get('date_to')?.clearValidators();
      }
      this.wfhModeForm?.get('date_to')?.updateValueAndValidity();
    });
  }
  initializeYears() {
    const currentYear = new Date().getFullYear();
    for (let year = 2010; year <= currentYear; year++) {
      this.years.push(year.toString());
    }
  }
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }

  get dateRangeError(): boolean {
    return this.wfhModeForm.hasError('dateRange');
  }
  // employee_DD_Company() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId
  //   };
  //   this.isSpinner = true;
  //   this.hrmsService.employeeDDCompany(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res.length >= 1) {
  //         this.getDDCompany = res;
  //       } else {
  //         this.triggerToast(res['Message'], 'Sorry No Data Found', 'warning');
  //       }
  //       this.isSpinner = false;
  //     },
  //     error: (error: any) => {
  //       this.triggerToast('Internal Server Error', 'Error loading Company Name', 'danger');
  //       this.isSpinner = false;
  //     }
  //   });
  // }
  employeeGetDDLocationApi() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.hrmsServiceMain.employeeGetLocation(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        this.getLocations = res
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errMsgLocation = 'Error loading data. Please try again.';
        this.isSpinner = false;
      }
    });
  }
  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;
    this.hrmsServiceMain.access_DD_department(reqBody).subscribe({
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

  callDDDesignation(event: any) {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.wfhModeForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe({
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

  callEmplyee() {
    this.wfhModeForm?.get('employee').reset();
    this.accessDDDeptEmployee();
  }
  accessDDDeptEmployee() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      DeptId: Number(this.wfhModeForm?.get('DeptName').value),
      DesignationId: Number(this.wfhModeForm?.get('Designation').value)
    };
    this.isSpinner = true;
    this.hrmsServiceMain.AccessDDDeptEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res && res.length >= 1) {
          this.employees = res;
        } else {
          this.triggerToast('No data Found', 'To Load The Employee Name', 'warning');
          this.employees = [];
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error Loading Contact Person Please Refresh Once', 'danger');
        this.isSpinner = false;
      }
    });
  }
  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      return new Date(parseInt(match[1], 10));
    }
    return null;
  }
  formatDate(date: Date | null): string {
    if (!date) return '';
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
    const year = date.getFullYear();
    // return `${day}-${month}-${year}`;
    return `${year}-${month}-${day}`;
  }

  getAllWFHDetails() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.hrmsServiceAttendance.employeeGetAllWFHDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res && res.length >= 1) {
          this.rows = res.map((row: any) => {
            row.Date = this.formatDate(this.parseJsonDate(row.Date));
            return row;
          });
          this.originalRows = res;
          this.isTableData = false;
        } else {
          this.errorMessage = "No records found";
          this.isTableData = true;
        }
        this.isSpinner = false;
      }, error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Failed To Load The Approver Data', "danger");
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
        this.page = 1;
        this.rows = [];
      }
    })
  }
  applyFilter() {
    const val = this.searchValue.toLowerCase().trim();

    this.rows = this.originalRows.filter((row: any) => {
      return (
        row.Date?.toLowerCase().includes(val) ||
        row.WFHType?.toLowerCase().includes(val) ||
        row.Status?.toLowerCase().includes(val) ||
        row.Remarks?.toLowerCase().includes(val) ||
        row.RequestedBy?.toLowerCase().includes(val) ||
        row.EmpName?.toLowerCase().includes(val) ||
        row.CompName?.toLowerCase().includes(val) ||
        row.DeptName?.toLowerCase().includes(val) ||
        row.Designation?.toLowerCase().includes(val) ||
        row.Designation?.toLowerCase().includes(val) ||

        // Number fields if any
        row.RequestId?.toString().toLowerCase().includes(val)
      );
    });

    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = `No record found for "${this.searchValue}"`;
    } else {
      this.isTableData = false;
      this.errorMessage = '';
    }

    this.page = 1;
  }


  onFromDate(): void {
    if (this.wfhModeForm.get('date_from')?.value) {
      this.minDate = this.wfhModeForm.get('date_from')?.value;
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

  selectEmployee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }

  checkValidEmployee() {
    const isMatch = this.employees.some((employee: any) =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.wfhModeForm.get('employee')?.setErrors({ invalidEmployee: true });
    } else {
      this.wfhModeForm.get('employee')?.setErrors(null);
    }
  }

  submitFilterData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: 1,
      DeptId: Number(this.wfhModeForm.get('DeptName')?.value) ? Number(this.wfhModeForm.get('DeptName')?.value) : 0,
      DesignationId: Number(this.wfhModeForm.get('Designation')?.value) ? Number(this.wfhModeForm.get('Designation')?.value) : 0,
      FromDate: this.wfhModeForm.get('date_from')?.value ? this.wfhModeForm.get('date_from')?.value : '',
      ToDate: this.wfhModeForm.get('date_to')?.value ? this.wfhModeForm.get('date_to')?.value : '',
      EmpId: this.selectedEmployee ? this.selectedEmployee : 0
    };
    this.isSpinner = true;
    this.hrmsServiceAttendance.employeeGetAllWFHFilterDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res && res.length >= 1) {
          this.rows = res.map((row: any) => {
            row.Date = this.formatDate(this.parseJsonDate(row.Date));
            return row;
          });
          this.isTableData = false;
        } else {
          this.errorMessage = "No records found";
          this.isTableData = true;
        }
        this.originalRows = res;
        this.isSpinner = false;
      }, error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Internal Server Error', "danger");
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
        this.page = 1;
      }
    })
  }
  onView(data: any) {
    this.viewdata = data;
    console.log(this.viewdata);

  }

  resetData() {
    this.wfhModeForm.reset();
    this.getDepartementRole = [];
    this.minDate = undefined;
    this.isValidEmployee = true;
    this.employees = [];
    this.page = 1;
    this.getAllWFHDetails();
    this.wfhModeForm?.updateValueAndValidity();
    this.searchValue = ''
  }

  onFocus(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  onBlur(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  setFloatingLabel(selectElement: HTMLSelectElement) {
    const label = selectElement.nextElementSibling as HTMLElement;
    if (selectElement.value) {
      label.classList.add('floating');
    } else {
      label.classList.remove('floating');
    }
  }

  preventKeyboardInput(event: KeyboardEvent) {
    event.preventDefault();
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault();
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
