import { Component, HostListener, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { payRollService } from '../../service/payroll.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { EntityStateService } from '../../service/entity-state.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { leavesService } from '../../service/leaves.service';
// import * as XLSX from 'xlsx';
import * as XLSX from 'xlsx-js-style';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Subscription } from 'rxjs';
import { RouterModule } from '@angular/router';
import { DashboardService } from '../../service/dashboard.service';

@Component({
  selector: 'app-working-days-reports',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, NgxPaginationModule, RouterModule],
  templateUrl: './working-days-reports.component.html',
  styleUrl: './working-days-reports.component.scss'
})
export class WorkingDaysReportsComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  employeeDetails;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  searchValue: string = '';
  rows: any[] = [];
  originalRows: any[] = [];
  dropdownVisible = false;
  workingDayForm: any = FormGroup;
  getDepartementName = [];
  minDateCareer: string | undefined;
  maxDateCareer: string | undefined;
  yesterday: string | undefined;

  isTableShow: boolean = false;
  errorMessage: any;
  isTableData: boolean = false;
  getBusinessUnitlist: any;
  getLocations: any[] = [];



  constructor(
    private accessPolicyStoreService: AccessPolicyStoreService,
    private hrmsEmployeeService: EmployeeModuleService,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly fb: FormBuilder,
    private readonly leavesService: leavesService,
    private readonly dashboardService: DashboardService,
    private readonly payrollLocationDD: payRollService,
    private entityStateService: EntityStateService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Present-Absent Report'
      );
    }); 
    const yesterdayDate = new Date();
    yesterdayDate.setDate(yesterdayDate.getDate() - 1);
    this.yesterday = yesterdayDate.toISOString().split('T')[0];
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


  ngOnInit() {
    this.workingDayForm = this.fb.group({
      DeptName: ['', []],
      date_from: ['', [Validators.required]],
      date_to: ['', [Validators.required]],
      BusinessUnit: ['', []],
      Location: ['', []],
    }, { validators: this.careerDateValidator.bind(this) })
    setTimeout(() => {
      this.access_DD_department();
      setTimeout(() => {
        this.getBusinessUnit();
        setTimeout(() => {
          this.callLocation();
        }, 200);
      }, 200);
    }, 100);
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


  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getBusinessUnitlist = [];
    setTimeout(() => {
      this.hrmsEmployeeService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.workingDayForm?.get('BusinessUnit').reset();
          this.getBusinessUnitlist = res;
          this.isSpinner = false;
        } else {
          this.isSpinner = false;
          this.getBusinessUnitlist = [];
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
          this.isSpinner = false;
        })
    }, 100);
  }


  // callLocation() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     AuthorisedEntity: Number(this.entityStateService.getEntityId()),
  //     CompId: 1,
  //     LEId: Number(this.entityStateService.getEntityId()),
  //     BUId: Number(this.workingDayForm?.get('BusinessUnit').value) ? Number(this.workingDayForm?.get('BusinessUnit').value) : 0,
  //   }
  //   this.getLocations = []
  //   this.isSpinner = true;
  //   setTimeout(() => {
  //     this.hrmsEmployeeService.employeeDDLocation(reqBody).subscribe((res: any) => {
  //       if (res.length >= 1) {
  //         this.workingDayForm?.get('Location').reset();
  //         this.getLocations = res;
  //         this.isSpinner = false;
  //       } else {
  //         this.triggerToast(res['Message'], "No Data Found For Location", "warning");
  //         this.isSpinner = false;
  //         this.getLocations = []
  //       }
  //     },
  //       error => {
  //         this.errorMessage = 'Error loading data. Please try again.';
  //         this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
  //         this.isSpinner = false;
  //         this.getLocations = []
  //       })
  //   }, 100);
  // }

  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.workingDayForm?.get('Location').reset();
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

  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
  }

  careerDateValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    const errors: any = {};
    // Check: date_from <= date_to
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      errors.dateRange = true;
    }
    return Object.keys(errors).length > 0 ? errors : null;
  }


  onFromDateCareer(): void {
    if (this.workingDayForm.get('date_from')?.value) {
      this.minDateCareer = this.workingDayForm.get('date_from')?.value;
    }
  }
  onToDateCareer(): void {
    if (this.workingDayForm.get('date_to')?.value) {
      this.maxDateCareer = this.workingDayForm.get('date_to')?.value;
    }
  }
  isFromDateInvalid(): boolean {
    const control = this.workingDayForm.get('date_from');
    if (!control) return false;

    return (
      (control.touched || this.isFormSubmitted) &&
      (control.invalid || this.workingDayForm.hasError('careerDateComparison'))
    );
  }

  isToDateInvalid(): boolean {
    const toDate = this.workingDayForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.workingDayForm.hasError('dateRange');
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
  departments: any = [];
  submitFilterData() {
    if (this.workingDayForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        CompId: 1,
        LEId: this.entityStateService.getEntityId(),
        BUId:Number(this.workingDayForm?.get('BusinessUnit').value),
        LocId: Number(this.workingDayForm?.get('Location').value),
        DeptId: Number(this.workingDayForm?.get('DeptName').value),
        StartDate: this.workingDayForm?.get('date_from').value,
        EndDate: this.workingDayForm?.get('date_to').value,
      };
      this.isSpinner = true;
      console.log(reqBody);
      this.hrmsEmployeeService.employeeAttendanceDeptReport(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast(res['Message'], '', 'warning');
            this.isTableShow = false;
            this.isTableData = true;
            this.errorMessage = res['Message']
          } else {
            this.isTableShow = true;
            this.rows = res;
            this.originalRows = [...res];
            if (res.length > 0) {
              this.departments = res[0].lstofDept;
            }
            this.triggerToast(res['msg'], '', 'success')
            this.isTableData = false;
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.isSpinner = false;
          this.isTableShow = false;
          this.isTableData = true;
          this.errorMessage = 'Internal Server Error'
          this.triggerToast('Internal Server Error', 'Leave Count Details Not Found', 'danger')
        }
      })
    } else {
      this.isFormSubmitted = true
    }

  }

  resetData() {
    this.workingDayForm.reset();
    this.isFormSubmitted = false;
    this.minDateCareer = undefined;
    this.maxDateCareer = undefined;
    this.isTableShow = false;
  }

  exportFile(format: string) {
    if (format === 'excel') this.exportToExcel();
  }

  exportToExcel(): void {
    const ws: XLSX.WorkSheet = {};
    const wb: XLSX.WorkBook = {
      SheetNames: ['Attendance Report'],
      Sheets: {}
    };

    const merges: XLSX.Range[] = [];

    const headerRow1: any[] = ['SL No', 'Working Days'];
    const headerRow2: any[] = ['', ''];
    const headerRow3: any[] = ['', ''];

    let colIndex = 2; // Start after SL No & Working Days

    this.departments.forEach((dept: any) => {

      // Row 1 → Dept Name
      headerRow1.push(dept.DeptName, '', '', '');

      // Row 2 → Total + OA %
      headerRow2.push(
        `Total: ${dept.Total}   OA %: ${dept.OverAllAbsentPercentage}`,
        '', '', ''
      );

      // Row 3 → PR AB LV AB%
      headerRow3.push('PR', 'AB', 'LV', 'AB %');

      // Merge Dept Name (Row 1)
      merges.push({
        s: { r: 0, c: colIndex },
        e: { r: 0, c: colIndex + 3 }
      });

      // Merge Total + OA% (Row 2)
      merges.push({
        s: { r: 1, c: colIndex },
        e: { r: 1, c: colIndex + 3 }
      });

      colIndex += 4;
    });

    // Merge SL No (vertical)
    merges.push({ s: { r: 0, c: 0 }, e: { r: 2, c: 0 } });

    // Merge Working Days (vertical)
    merges.push({ s: { r: 0, c: 1 }, e: { r: 2, c: 1 } });

    // Add headers
    XLSX.utils.sheet_add_aoa(ws, [headerRow1], { origin: 'A1' });
    XLSX.utils.sheet_add_aoa(ws, [headerRow2], { origin: 'A2' });
    XLSX.utils.sheet_add_aoa(ws, [headerRow3], { origin: 'A3' });

    // Add data rows
    this.rows.forEach((row, index) => {

      const dataRow: any[] = [
        index + 1,
        `${row.Date}\n${row.Day}`
      ];

      this.departments.forEach((dept: any) => {
        const deptData = row.lstofDept.find(
          (d: any) => d.DeptName.trim() === dept.DeptName.trim()
        );

        if (deptData) {
          dataRow.push(
            deptData.Present,
            deptData.Absent,
            deptData.Leave,
            deptData.AbsentPesent
          );
        } else {
          dataRow.push('', '', '', '');
        }
      });

      XLSX.utils.sheet_add_aoa(ws, [dataRow], { origin: -1 });
    });

    ws['!merges'] = merges;

    // Column width
    ws['!cols'] = [
      { wch: 8 },
      { wch: 18 },
      ...Array(this.departments.length * 4).fill({ wch: 10 })
    ];

    wb.Sheets['Attendance Report'] = ws;

    XLSX.writeFile(wb, 'Attendance_Report.xlsx');
  }



  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


}
