import { Component, HostListener, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { payRollService } from '../../service/payroll.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { EntityStateService } from '../../service/entity-state.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { leavesService } from '../../service/leaves.service';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Subscription } from 'rxjs';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-leave-balance',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, NgxPaginationModule,
    RouterModule
  ],
  templateUrl: './leave-balance.component.html',
  styleUrl: './leave-balance.component.scss'
})
export class LeaveBalanceComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  leaveBalanceForm: any = FormGroup;
  getDDCompany: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;

  employeeDetails;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any
  errorMessageLocation: any
  searchValue: string = '';
  dropdownVisible = false;
  isTableData: boolean = false;
  errorMessage: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  rows: any[] = [];
  originalRows: any[] = [];
  years: number[] = [];
  months: { name: string; value: number }[] = [];

  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;

  constructor(
    private accessPolicyStoreService: AccessPolicyStoreService,
    private hrmsEmployeeService: EmployeeModuleService,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly fb: FormBuilder,
    private readonly payrollLocationDD: payRollService,
    private readonly leavesService: leavesService,
    private entityStateService: EntityStateService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Leave Balance Report'
      );
    });
  }

  allMonths = [
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 }
  ];

  ngOnInit(): void {

    // 1️⃣ Create form FIRST
    this.leaveBalanceForm = this.fb.group({
      emloyee: [''],
      emloyeeCode: [''],
      // company: [''],
      // LegalEntity: [''],
      BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      Year: [''],
      Month: [''],
    });

    // 2️⃣ Generate years once
    this.generateYears();

    // 3️⃣ Listen to Year changes
    this.leaveBalanceForm.get('Year')?.valueChanges.subscribe((year: any) => {
      this.updateMonths(year);
    });

    // 4️⃣ API calls
    setTimeout(() => {
      this.getEmployeeSelectEmployee();
    }, 300);

    setTimeout(() => {
      this.getBusinessUnit();
      setTimeout(() => {
        this.callLocation();
      }, 200);
      setTimeout(() => {
        this.access_DD_department();
      }, 200);
    }, 200);
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;
        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.getBusinessUnit();
          this.callLocation();
          setTimeout(() => {
             this.resetData();
          }, 200);
        }
        this.currentEntityId = newEntityId;
      });
  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }

  generateYears() {
    const startYear = 2020;
    const currentYear = new Date().getFullYear();

    for (let year = startYear; year <= currentYear; year++) {
      this.years.push(year);
    }
  }

  updateMonths(selectedYear: number) {
    if (!selectedYear) {
      this.months = [];
      return;
    }

    const today = new Date();
    const currentYear = today.getFullYear();
    const currentMonth = today.getMonth() + 1; // Jan = 1

    if (+selectedYear === currentYear) {
      // 🔥 Current year → show only up to current month
      this.months = this.allMonths.filter(
        m => m.value <= currentMonth
      );
    } else {
      // 🔁 Past or future year → show all months
      this.months = [...this.allMonths];
    }

    // ❌ Reset month if it becomes invalid
    const selectedMonth = this.leaveBalanceForm.get('Month')?.value;
    if (selectedMonth && !this.months.some(m => m.value === selectedMonth)) {
      this.leaveBalanceForm.get('Month')?.reset();
    }
  }


  //this is Employee list
  getEmployeeSelectEmployee() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsServiceMain.visitorAccessDDEmployee(reqBody).subscribe({
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
        // employee.EmpId.toLowerCase().includes(this.searchText.toLowerCase()) ||
        employee.EmpCode.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employees];
    }
  }
  selectEmployeee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.leaveBalanceForm.get('emloyeeCode')?.patchValue(employee.EmpId);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.leaveBalanceForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.leaveBalanceForm.get('emloyee')?.setErrors(null);
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

  // employee_DD_Comapny() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId
  //   };
  //   this.isSpinner1 = true;
  //   this.hrmsEmployeeService.employeeDDCompany(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res.length >= 1) {
  //         this.getDDCompany = res;
  //       } else {
  //         this.triggerToast(res['Message'], 'Sorry No Data Found', 'warning');
  //       }
  //       this.isSpinner1 = false;
  //     },
  //     error: (error: any) => {
  //       this.triggerToast('Internal Server Error', 'Error loading Company Name', 'danger');
  //       this.isSpinner1 = false;
  //     }
  //   });
  // }

  // calllegalEntity(event: any) {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     AuthorisedEntity: this.entityStateService.getEntityId(),
  //     CompId: Number(this.leaveBalanceForm?.get('company').value)
  //   }
  //   this.isSpinner1 = true;
  //   this.getLegalEntity = []
  //   this.hrmsEmployeeService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
  //     setTimeout(() => {
  //       this.leaveBalanceForm?.get('LegalEntity').reset();
  //       this.leaveBalanceForm?.get('BusinessUnit').reset();
  //       this.leaveBalanceForm?.get('Location').reset();
  //     }, 100);
  //     if (res.length >= 1) {
  //       this.getLegalEntity = res;
  //       this.isSpinner1 = false;
  //     } else {
  //       this.triggerToast(res['Message'], "No Data Found For Legal Entity", "warning");
  //       this.isSpinner1 = false;
  //       this.getLegalEntity = []
  //     }
  //   },
  //     error => {
  //       this.errorMessage = 'Error loading data. Please try again.';
  //       this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
  //       this.isSpinner1 = false;
  //     })
  // }
  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner1 = true;
    // this.getBusinessUnitlist = [];
    // this.getLocations = []
    setTimeout(() => {
      this.hrmsEmployeeService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.leaveBalanceForm?.get('BusinessUnit').reset();
          this.getBusinessUnitlist = res;
          this.isSpinner1 = false;
        } else {
          this.isSpinner1 = false;
          this.getBusinessUnitlist = [];
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
          this.isSpinner1 = false;
        })
    }, 100);

  }

  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.leaveBalanceForm?.get('Location').reset();
          this.getLocations = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = []
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner = false;
          this.getLocations = []
        })
    }, 100);
  }
  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner1 = true;
    this.hrmsServiceMain.access_DD_department(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDepartementName = res;
        } else {
          this.triggerToast('', 'Record Not Found', 'Warning');
        }
        this.isSpinner1 = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Department List', 'danger');
        this.isSpinner1 = false;
      }
    });
  }

  callDDDesignation(event: any) {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.leaveBalanceForm?.get('DeptName')?.value,
    };
    this.isSpinner1 = true;
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe({
      next: (res: any) => {
        this.getDepartementRole = res;
        this.isSpinner1 = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error loading Designation', 'danger');
        this.isSpinner1 = false;
      }
    });
  }

  submitFilterData() {
    if (this.leaveBalanceForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.leaveBalanceForm?.get('emloyeeCode').value || 0,
        CompId: 1,
        LEId: Number(this.entityStateService.getEntityId()),
        BUId: this.leaveBalanceForm?.get('BusinessUnit').value || 0,
        LocationId: this.leaveBalanceForm?.get('Location').value || 0,
        DeptId: this.leaveBalanceForm?.get('DeptName').value || 0,
        DesignationId: this.leaveBalanceForm?.get('Designation').value || 0,
        Year: this.leaveBalanceForm?.get('Year').value || 0,
        Month: this.leaveBalanceForm?.get('Month').value || 0,
      }
      this.isSpinner = true;
      console.log(reqBody);
      this.leavesService.LeaveBalReport(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast(res['Message'], '', 'warning')
          } else {
            this.rows = res;
            this.originalRows = [...res];
            this.triggerToast(res['msg'], '', 'success')
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'Leave Count Details Not Found', 'danger')
        }
      })
    }
  }

  applyFilter() {
    const value = this.searchValue?.toLowerCase().trim();

    if (!value) {
      this.rows = [...this.originalRows];
      return;
    }

    this.rows = this.originalRows.filter((item: any) =>
      item.EmpName?.toLowerCase().includes(value) ||
      item.EmpCode?.toLowerCase().includes(value) ||
      item.Year?.toString().includes(value) ||
      item.Month?.toString().includes(value) ||
      item.DeptId?.toString().includes(value) ||
      item.LocationId?.toString().includes(value)
    );
  }


  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
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

  exportFile(format: string) {
    if (format === 'excel') this.exportToExcel();
    if (format === 'pdf') this.exportToPDF();
  }

  // Use the updated exportToExcel() I gave you

  exportToExcel(): void {
    if (this.rows.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }

    // Map API response to exportable format
    const filteredData = this.rows.map((item: any, index: number) => {
      return {
        "SL No": index + 1,
        "Name": item.EmpName,
        "Code": item.EmpCode,

        // CL
        "CL Total": item.CLOpeningBalance,
        "CL Used": item.CLAvailed,
        "CL Avail": item.CLColsingBalance,
        "CL Carry Forward": item.CLCarryFroward,

        // EL
        "EL Total": item.ELOpeningBalance,
        "EL Used": item.ELAvailed,
        "EL Avail": item.ELColsingBalance,
        "EL Carry Forward": item.ELCarryFroward,

        // RH
        "RH Total": item.RHOpeningBalance,
        "RH Used": item.RHAvailed,
        "RH Avail": item.RHColsingBalance,
        "RH Carry Forward": item.RHCarryFroward,

        // COMPOFF (if needed)
        "COMPOFF Total": item.COMPOFFOpeningBalance,
        "COMPOFF Used": item.COMPOFFAvailed,
        "COMPOFF Avail": item.COMPOFFColsingBalance,
        "COMPOFF Carry Forward": item.COMPOFFCarryFroward,
      };
    });


    // Create empty worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(filteredData);

    // Optional: Set column widths for readability
    worksheet['!cols'] = Array(20).fill({ wpx: 100 });

    // Create workbook and save
    const workbook: XLSX.WorkBook = { Sheets: { 'Leave Summary': worksheet }, SheetNames: ['Leave Summary'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'Leave Summary.xlsx');

    this.dropdownVisible = false;
  }
  exportToPDF() {

  }


  resetData() {
    this.isFormSubmitted = false;
    this.leaveBalanceForm.reset();
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
