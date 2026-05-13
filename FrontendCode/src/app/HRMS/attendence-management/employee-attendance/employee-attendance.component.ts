import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { AttendenceModuleService } from '../../service/attendence.service';
import { NgApexchartsModule } from "ng-apexcharts";
import { Router, RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { payRollService } from '../../service/payroll.service';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs';
// import * as XLSX from 'xlsx';
import * as XLSX from 'xlsx-js-style';

interface AttendanceRecord {
  EmpId: number;
  EmpCode: string;
  EmpName: string;
  WorkingHours: string;
  LogInTime: string;
  LogOutTime: string;
  IsWorkFromHome: boolean;
  LogDate: string;
  ShiftName: string;
  BreakTime: string,
  OverTime: string,
  WorkType: string,
  ActiveHours: string,
  ESSLLogInTime: string,
  ESSLLogOutTime: string,
  ESSLActiveHours: string,
  WFHLogInTime: string,
  WFHLogOutTime: string,
  WFHActiveHours: string,
  ONSITELogInTime: string,
  ONSITELogOutTime: string,
  ONSITEActiveHours: string,
}

interface DateAttendance {
  [date: string]: AttendanceRecord[];
}

@Component({
  selector: 'app-employee-attendance',
  standalone: true,
  imports: [FormsModule, CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule,
    NgApexchartsModule, RouterModule],
  templateUrl: './employee-attendance.component.html',
  styleUrl: './employee-attendance.component.scss'
})
export class EmployeeAttendanceComponent implements OnInit, OnDestroy {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;

  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  employeeAttendanceForm: any = FormGroup;
  loadAttendanceForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isFormSubmittedModal: boolean = false;
  errorMessageEmpName: any;
  years: string[] = [];
  employeeDetails;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  getDDCompany: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  searchText: string = '';
  searchTextLoadAttendance: string = '';
  isDropdownOpen = false;
  filteredEmployees: any[] = [];
  filteredEmployeesLoadAtten: any[] = [];
  employees: any;
  employeesLoadAttendance: any;
  employeesSerach: any[] = [];
  employeesSerachLoadAttendance: any;
  today = new Date().toISOString().split('T')[0];
  minDate: string | undefined;
  isValidEmployee: boolean = true;
  isValidEmployeeLoadAttendance: boolean = true;
  selectedEmployee: any = null;
  selectedEmployeeLoadAttendance: any = null;
  selectedEndMonth: any;
  months: string[] = [];
  selectedMonthEndDate: string = '';
  selectedMonthStartDate: string = '';
  isMonthSelected: boolean = false;
  tooltipContent: any;
  errorMessage: any;
  isTableData: boolean = false;
  attendanceArray: DateAttendance[] = [];
  rows: any[] = [];
  isClickedFilter: boolean = false;
  dates: any;
  employeeAttendanceData: any;
  accessPolicy: any
  controlAccessPage: any
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  dropdownVisible = false;
  searchValue: string = '';
  isPagination: boolean = true;
  yesterday: string | undefined;
  getEmployeeTypeList: any[] = [];


  editingCell: {
    empName: string;
    date: string;
  } | null = null;

  editedAttendance: {
    [key: string]: any;
  } = {};

  tabs: any[] = [];

  allTabs = [
    { id: 'option_report', title: 'Present-Absent Report', type: 'item', url: '/option_report', icon: 'feather icon-clipboard' },
  ];


  constructor(private readonly hrmsService: EmployeeModuleService,
    private readonly fb: FormBuilder, private readonly hrmsServiceMain: HrmsServiceService,
    private readonly hrmsEmpAttendance: AttendenceModuleService,
    private readonly payrollLocationDD: payRollService,
    private readonly sanitizer: DomSanitizer,
    private readonly cdr: ChangeDetectorRef,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private entityStateService: EntityStateService,
    private router: Router
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Employee Attendance'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Employee Attendance'
      );
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((p: any) => p.PageName === tab.title && p.ViewAccess)
      );
    });

    this.getCurrentMonthList();
    this.calculateTotalPages();

    const yesterdayDate = new Date();
    yesterdayDate.setDate(yesterdayDate.getDate() - 1);
    this.yesterday = yesterdayDate.toISOString().split('T')[0];
  }
  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }
  getCurrentMonthList() {
    const now = new Date();
    const currentMonthIndex = now.getMonth();
    const allMonths = [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December',
    ];
    this.months = allMonths.slice(0, currentMonthIndex + 1);
    this.selectedEndMonth = allMonths[currentMonthIndex];
  }

  ngOnInit(): void {
    this.loadAttendanceForm = this.fb.group({
      attendance_date_from: ['', [Validators.required]],
      // emloyeeLoadAttendance: ['', [Validators.required]],
      // message: ['']
    })
    this.employeeAttendanceForm = this.fb.group({
      // company: [''],
      // LegalEntity: [''],
      BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      date_from: [''],
      date_to: [''],
      employeeType: [],
      employee: ['']
    }, { validators: this.dateRangeValidator });

    setTimeout(() => {
      this.access_DD_department();
      setTimeout(() => {
        this.getBusinessUnit();
        setTimeout(() => {
          this.callLocation();
          setTimeout(() => {
            this.getDDEmpTypeList();
            setTimeout(() => {
              // this.employeeAttendance();
            }, 200);
          }, 200);
        }, 200);
      }, 200);

    }, 200);
    this.initializeYears();
    this.employeeAttendanceForm?.get('date_from')?.valueChanges.subscribe((value: any) => {
      if (value) {
        this.employeeAttendanceForm?.get('date_to')?.setValidators([Validators.required]);
      } else {
        this.employeeAttendanceForm?.get('date_to')?.clearValidators();
      }
      this.employeeAttendanceForm?.get('date_to')?.updateValueAndValidity();
    });
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;
        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.getBusinessUnit();
          this.callLocation();
          this.resetData();
        }
        this.currentEntityId = newEntityId;
      });

  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }

  initializeYears() {
    const currentYear = new Date().getFullYear();
    for (let year = 2010; year <= currentYear; year++) {
      this.years.push(year.toString());
    }
  }

  // this is for modal code
  isFromDateInvalid(): boolean {
    const fromDate = this.loadAttendanceForm.get('attendance_date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isSpinner2:boolean=false;
  submitModalForm() {
    if (this.loadAttendanceForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        // EmpId: this.selectedEmployeeLoadAttendance ? this.selectedEmployeeLoadAttendance : 0,
        EmpId: 0,
        Date: this.loadAttendanceForm?.get('attendance_date_from').value,
        msg: "",
      }
      console.log(reqBody)
      this.isSpinner2 = true;
      this.hrmsService.employeeSPAttendance(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg']) {
            this.triggerToast(res['msg'], '', '')
            this.closeModal.nativeElement?.click();
          } else if (res['Message']) {
            this.triggerToast(res['Message'], '', '')
          }
          this.isSpinner2 = false;
        }, error: (err: any) => {
          this.isSpinner2 = false;
          this.triggerToast('Internal Server Error', '', 'danger')
        }
      })
    } else {
      this.triggerToast('Please Select The Date', '', 'warning')
    }

  }

  resetModalData() {
    this.isValidEmployeeLoadAttendance = true;
    this.employeesSerachLoadAttendance = [];
    this.employeesSerachLoadAttendance = [];
    this.loadAttendanceForm.reset()
  }

  // this is for modal code end 


  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }

  get dateRangeError(): boolean {
    return this.employeeAttendanceForm.hasError('dateRange');
  }

  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      // LEId: Number(this.employeeAttendanceForm?.get('LegalEntity').value),
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getBusinessUnitlist = [];
    this.getLocations = []
    setTimeout(() => {
      this.hrmsService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.employeeAttendanceForm?.get('BusinessUnit').reset();
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

  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.employeeAttendanceForm?.get('Location').reset();
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
      DeptId: this.employeeAttendanceForm?.get('DeptName')?.value,
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
    this.employeeAttendanceForm?.get('employee').reset();
    this.accessDDDeptEmployee();
  }
  getDDEmpTypeList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDEmpType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getEmployeeTypeList = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Employee Type List", "warning");
        this.isSpinner = false;
        this.getEmployeeTypeList = []
      }
    },
      error => {
        this.triggerToast('Internal Server Error', 'To Load Employee Type List', "danger");
        this.isSpinner = false;
      })
  }
  accessDDDeptEmployee() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      DeptId: Number(this.employeeAttendanceForm?.get('DeptName').value),
      DesignationId: Number(this.employeeAttendanceForm?.get('Designation').value)
    };
    this.isSpinner = true;
    this.hrmsServiceMain.AccessDDDeptEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res && res.length >= 1) {
          this.employeesSerach = res;
        } else {
          this.triggerToast('No data Found', 'To Load The Employee Name', 'warning');
          this.employeesSerach = [];
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error Loading Contact Person Please Refresh Once', 'danger');
        this.isSpinner = false;
      }
    });
  }

  onFromDate(): void {
    if (this.employeeAttendanceForm.get('date_from')?.value) {
      this.minDate = this.employeeAttendanceForm.get('date_from')?.value;
    }
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  openDropdown() {
    this.isDropdownOpen = true;
    this.filteredEmployees = Array.isArray(this.employeesSerach)
      ? [...this.employeesSerach]
      : [];
  }

  closeDropdown() {
    setTimeout(() => {
      this.isDropdownOpen = false;
    }, 200);
  }

  filterEmployees() {
    if (!Array.isArray(this.employeesSerach)) return;
    if (this.searchText) {
      this.filteredEmployees = this.employeesSerach.filter((employee: any) =>
        employee.EmpName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        employee.EmpCode.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employeesSerach];
    }
  }

  selectEmployee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }

  checkValidEmployee() {
    const isMatch = this.employeesSerach.some((employee: any) =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.employeeAttendanceForm.get('employee')?.setErrors({ invalidEmployee: true });
    } else {
      this.employeeAttendanceForm.get('employee')?.setErrors(null);
    }
  }

  employeeAttendance() {
    const formatDate = (date: Date): string => {
      const year = date.getFullYear();
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    };
    const firstDayOfCurrentMonth = new Date();
    firstDayOfCurrentMonth.setDate(1);
    const yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);

    this.calculateTotalPages();
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      LoginId: this.employeeDetails[0].LoginId,
      CompId: this.employeeDetails[0].CompId,

      DeptId: this.employeeDetails[0].DeptId,
      DesignationId: this.employeeDetails[0].DesignationId,
      StartDate: formatDate(firstDayOfCurrentMonth),
      EndDate: formatDate(yesterday),
      PageNumber: this.pageNumber,
      PageSize: this.pageSize,
      LocationId: [1, 2, 3, 4]
    };
    this.isSpinner1 = true;
    this.hrmsEmpAttendance.EmployeeEmployeeAttendance(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          const dataArray = Array.isArray(res.Data) ? res.Data : [res.Data];
          this.attendanceArray = dataArray;
          this.totalRecords = res.TotalRecords;
          this.calculateTotalPages();
          this.dates = this.attendanceArray.map((item: any) => item.AttendaceDate);
          this.employeeAttendanceData = {};
          this.attendanceArray.forEach((item: any) => {
            item.lstofAttendance.forEach((attendance: any) => {
              if (!this.employeeAttendanceData[attendance.EmpName]) {
                this.employeeAttendanceData[attendance.EmpName] = {};  // Use EmpName as key
              }
              if (!this.employeeAttendanceData[attendance.EmpName].EmpCode) {
                this.employeeAttendanceData[attendance.EmpName].EmpCode = attendance.EmpCode;  // Store EmpCode
              }
              this.employeeAttendanceData[attendance.EmpName][item.AttendaceDate] = attendance;
            });
          });

          this.employees = [];
          Object.keys(this.employeeAttendanceData).forEach(empName => {
            const empAttendance = this.employeeAttendanceData[empName];
            const attendanceForEmp = this.dates.map((date: any) => {
              return empAttendance[date] || null;
            });
            // take PayDays from first available attendance
            const firstAttendance = attendanceForEmp.find((a: any) => a !== null);

            this.employees.push({
              EmpName: empName,
              EmpCode: empAttendance.EmpCode,
              PayDays: firstAttendance?.PayDays ?? '',
              attendance: attendanceForEmp
            });
          });
          this.isSpinner1 = false;
          this.isTableData = false;
        } else {
          this.isSpinner1 = false;
          this.isTableData = true;
          this.errorMessage = "No Data Found";
        }

      },
      error: (error: any) => {
        this.isSpinner1 = false;
        this.errorMessage = "Internal Server Error";
        this.isTableData = true;
      }
    });
  }

  submitFilterData() {
    this.isFormSubmitted = true;
    if (this.isSpinner1) {
      return;
    }
    if (this.employeeAttendanceForm.valid) {
      this.isFormSubmitted = false;
      this.isClickedFilter = true;
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        // CompId: Number(this.employeeAttendanceForm?.get('company').value || 0),
        CompId: 1,
        // LEId: Number(this.employeeAttendanceForm?.get('LegalEntity').value),
        LEId: this.entityStateService.getEntityId(),
        BUId: Number(this.employeeAttendanceForm?.get('BusinessUnit').value),
        LocId: Number(this.employeeAttendanceForm?.get('Location').value),
        StartDate: this.employeeAttendanceForm?.get('date_from').value || null,
        EndDate: this.employeeAttendanceForm?.get('date_to').value || null,
        DeptId: Number(this.employeeAttendanceForm?.get('DeptName').value || 0),
        DesignationId: Number(this.employeeAttendanceForm?.get('Designation').value || 0),
        EmpTypeId: Number(this.employeeAttendanceForm?.get('employeeType').value || 0),
        EmpId: this.selectedEmployee ? this.selectedEmployee : 0
      };
      console.log('Request Body:', reqBody);
      this.isSpinner1 = true;
      this.isPagination = false;
      this.hrmsEmpAttendance.EmployeeAttendanceFilter(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast('', res['Message'], '');
            this.isSpinner1 = false;
            this.errorMessage = "";
            this.isTableData = true;
          } else {
            const dataArray = Array.isArray(res) ? res : [res];
            console.log('Filtered Data:', dataArray); // Log the response data
            if (dataArray.length >= 1) {
              this.attendanceArray = [];
              this.employees = [];
              this.dates = [];
              this.attendanceArray = dataArray;
              this.totalRecords = res.TotalRecords;
              this.calculateTotalPages();
              this.dates = this.attendanceArray.map((item: any) => item.AttendaceDate);
              this.employeeAttendanceData = {};
              this.attendanceArray.forEach((item: any) => {
                item.lstofAttendance.forEach((attendance: any) => {
                  if (!this.employeeAttendanceData[attendance.EmpName]) {
                    this.employeeAttendanceData[attendance.EmpName] = {};
                  }
                  // Include EmpCode in employeeAttendanceData
                  if (!this.employeeAttendanceData[attendance.EmpName].EmpCode) {
                    this.employeeAttendanceData[attendance.EmpName].EmpCode = attendance.EmpCode;  // Store EmpCode
                  }
                  this.employeeAttendanceData[attendance.EmpName][item.AttendaceDate] = attendance;
                });
              });
              this.employees = [];
              console.log('Unique employees count:', Object.keys(this.employeeAttendanceData).length);
              Object.keys(this.employeeAttendanceData).forEach(empName => {
                const empAttendance = this.employeeAttendanceData[empName];
                const attendanceForEmp = this.dates.map((date: any) => {
                  return empAttendance[date] || null;
                });
                // take PayDays from first available attendance
                const firstAttendance = attendanceForEmp.find((a: any) => a !== null);

                this.employees.push({
                  EmpName: empName,
                  EmpCode: empAttendance.EmpCode,
                  PayDays: firstAttendance?.PayDays ?? '',
                  attendance: attendanceForEmp
                });
              });
              this.isSpinner1 = false;
              this.isTableData = false;
            } else {
              this.isSpinner1 = false;
              this.employees = [];
              this.dates = [];
              this.errorMessage = "No Data Found";
              this.isTableData = true;

            }
          }

        },
        error: (error: any) => {
          this.isSpinner1 = false;
          this.employees = [];
          this.dates = [];
          this.errorMessage = "Internal Server Error";
          this.isTableData = true;
        }
      });
    }
    else {
      this.isFormSubmitted = true;
      this.triggerToast('', 'Please Select End Date When Start Date Selected', '');
    }
  }

  getCellColor(empName: string, date: string): string {
    const empData = this.employeeAttendanceData[empName];
    const attendance = empData?.[date];

    // 1️⃣ LeaveType has highest priority
    if (attendance?.LeaveType) {
      switch (attendance.LeaveType) {
        case 'LOP':
          return 'red';
        case 'EL':
          return '#A3C6FF';
        case 'CL':
          return '#B172DB';
        case 'Holiday':
          return '#9edbcf';
        case 'RH':
          return '#FFE7E9';
        case 'COMP OFF':
          return '#63e5e5'
      }
    }

    // 2️⃣ WorkType background
    if (attendance?.WorkType?.toLowerCase().includes('onsite')) {
      return 'yellow';
    }
    if (attendance?.WorkType === 'WFH' || attendance?.WorkType === 'wfh') {
      return 'orange';
    }
    if (attendance?.WorkType === 'MANUAL' || attendance?.WorkType === 'Manual') {
      return '#c79670';
    }
    if (attendance.WorkType?.includes('+')) {
      return '#b1b1e7ff';
    }
    if (attendance?.DaysPresent === 1) {
      return 'rgb(201 233 119)'
    }

    // 3️⃣ Default
    return 'transparent';
  }

  getLeaveBgColor(attendance: any) {
    if (!attendance || !attendance.LeaveType || attendance.LeaveType.trim() === '') {
      return {}; // Present
    }

    switch (attendance.LeaveType) {
      case 'LOP':
        return { 'background-color': 'red', 'color': 'white' };

      case 'EL':
        return { 'background-color': '#A3C6FF' };

      case 'CL':
        return { 'background-color': '#B172DB', 'color': 'white' };

      default:
        return {};
    }
  }

  getAttendanceDisplay(employee: any, date: string): string {
    const empData = this.employeeAttendanceData[employee.EmpName];
    const attendance = empData?.[date];
    if (!attendance) return '';
    // ⭐ If Weekend but employee worked → show working hours
    if (
      attendance.LeaveType === 'Weekend' &&
      attendance.WorkingHours !== '00:00:00'
    ) {
      return attendance.WorkingHours.slice(0, 5);
    }
    if (attendance?.LeaveType) {
      return attendance.LeaveType;
    }
    // ⭐ Weekdays → always show working hours (even 00:00)
    if (attendance.WorkingHours) {
      return attendance.WorkingHours.slice(0, 5);
    }
    return '';
  }

  // dont remove this code
  // getAttendanceDisplay(employee: any, date: string): string {
  //   const empData = this.employeeAttendanceData[employee.EmpName];
  //   const attendance = empData?.[date];
  //   // If there's a LeaveType, return it (e.g., EL, CL, RH, LOP, Holiday)
  // if (attendance?.LeaveType) {
  //   return attendance.LeaveType;
  // }
  //   // Otherwise, return the WorkingHours
  //   return attendance?.WorkingHours ? attendance.WorkingHours.slice(0, 5) : '';
  // }


  enableEdit(employee: any, date: string, index: number) {
    if (!this.isCellEditable(employee, date, index)) {
      return;
    }
    this.editingCell = {
      empName: employee.EmpName,
      date: date
    };
  }

  isCellEditable(employee: any, date: string, index: number): boolean {
    const attendance = employee.attendance[index];
    if (!attendance) return false;
    // Weekend check
    const day = new Date(date).getDay(); // 0 = Sun, 6 = Sat
    const isWeekend = day === 0 || day === 6;
    // LeaveType check
    const restrictedLeaveTypes = ['LOP', 'EL', 'CL', 'Holiday'];
    const hasRestrictedLeave =
      restrictedLeaveTypes.includes(attendance.LeaveType);
    // ❌ NOT editable if weekend OR leave type
    if (isWeekend || hasRestrictedLeave) {
      return false;
    }
    return true; // ✅ Only weekday + no restricted leave
  }

  isEditing(employee: any, date: string): boolean {
    return (
      this.editingCell?.empName === employee.EmpName &&
      this.editingCell?.date === date
    );
  }

  disableEdit() {
    this.editingCell = null;
  }

  onWorkingHoursChange(value: string, employee: any, date: string, index: number) {
    const formattedValue = value.length === 5 ? value + ':00' : value;
    employee.attendance[index].WorkingHours = formattedValue;
    const key = `${employee.EmpName}_${date}`;
    this.editedAttendance[key] = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: employee.attendance[index].EmpId,
      EmpCode: employee.EmpCode,
      // EmpName: employee.EmpName,
      Date: date,
      Time: formattedValue,
      Status: "Active"
    };
  }

  hasEditedData(): boolean {
    return Object.keys(this.editedAttendance).length > 0;
  }

  submitEditedAttendance() {
    const payload = Object.values(this.editedAttendance);
    console.log('Submitting Payload:', payload);
    this.hrmsEmpAttendance.employeeUploadMultiAttendance(payload).subscribe({
      next: (res) => {
        this.triggerToast('Success', 'Attendance updated successfully', '');
        this.editedAttendance = {};
        this.submitFilterData();
      },
      error: () => {
        this.triggerToast('Error', 'Failed to update attendance', '');
      }
    });
  }

  handleNoData() {
    this.isSpinner1 = false;
    this.employees = [];
    this.dates = [];
    this.errorMessage = 'No Data Found';
    this.isTableData = true;
  }

  handleError() {
    this.isSpinner1 = false;
    this.employees = [];
    this.dates = [];
    this.errorMessage = 'Internal Server Error';
    this.isTableData = true;
  }

  pageSizes: number[] = [10, 50, 100, 500];
  pageSize: number = 10;
  pageNumber: number = 1;
  totalRecords: any;
  totalPages: number = 0;

  onPageSizeChange() {
    this.pageNumber = 1;
    this.calculateTotalPages();
    this.employeeAttendance();
  }

  onPageChange(direction: 'next' | 'prev') {
    if (direction === 'next' && this.pageNumber * this.pageSize < this.totalRecords) {
      this.pageNumber++;
    } else if (direction === 'prev' && this.pageNumber > 1) {
      this.pageNumber--;
    }
    this.employeeAttendance();
  }

  calculateTotalPages() {
    this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
  }


  applyFilter() {
    if (!this.searchValue) {
      this.employees = Object.keys(this.employeeAttendanceData).map(empName => {
        const empAttendance = this.employeeAttendanceData[empName];
        const attendanceForEmp = this.dates.map((date: any) => empAttendance[date] || null);
        const firstAttendance = attendanceForEmp.find((a: any) => a !== null);
        return {
          EmpName: empName,
          EmpCode: empAttendance.EmpCode,
          PayDays: firstAttendance?.PayDays ?? '',
          attendance: this.dates.map((date: any) => empAttendance[date] || null)
        };
      });
      this.isTableData = false;
      return;
    }
    const filterValue = this.searchValue.toLowerCase();
    const filteredEmployees = Object.keys(this.employeeAttendanceData)
      .filter(empName => {
        const empAttendance = this.employeeAttendanceData[empName];
        // Match against EmpName or EmpCode only
        const matchesEmployee =
          empName.toLowerCase().includes(filterValue) ||
          empAttendance.EmpCode?.toLowerCase().includes(filterValue);
        return matchesEmployee; // Only filter by EmpName and EmpCode
      })
      .map(empName => {
        const empAttendance = this.employeeAttendanceData[empName];
        const attendanceForEmp = this.dates.map((date: any) => empAttendance[date] || null);
        const firstAttendance = attendanceForEmp.find((a: any) => a !== null);
        return {
          EmpName: empName,
          EmpCode: empAttendance.EmpCode,
          PayDays: firstAttendance?.PayDays ?? '',
          attendance: this.dates.map((date: any) => empAttendance[date] || null)
        };
      });
    if (filteredEmployees.length > 0) {
      this.employees = filteredEmployees;
      this.isTableData = false;
    } else {
      this.employees = [];
      this.isTableData = true;
      this.errorMessage = "No Data Found";
    }
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
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
    const companyMap: { [key: string]: any[] } = {};
    const allDates = Array.from(
      new Set(this.attendanceArray.map((x: any) => x.AttendaceDate))
    );
    const holidayDates = new Set<string>();
    this.attendanceArray.forEach((item: any) => {
      item.lstofAttendance.forEach((emp: any) => {
        if (emp.LeaveType === 'Holiday') {
          holidayDates.add(item.AttendaceDate);
        }
      });
    });
    this.attendanceArray.forEach((item: any) => {
      const attendanceDate = item.AttendaceDate;
      item.lstofAttendance.forEach((emp: any) => {
        const companyName = emp.CompName || 'Unknown';
        if (!companyMap[companyName]) {
          companyMap[companyName] = [];
        }
        let row = companyMap[companyName].find(
          x => x.Code === emp.EmpCode
        );
        if (!row) {
          row = {
            "Sl No.": companyMap[companyName].length + 1, // Add Sl No. here
            Name: emp.EmpName,
            Code: emp.EmpCode,
            "Pay Days": emp.PayDays,
            // ✅ ADD THESE TWO LINES
            "Login Location": emp.LoginLocation || '-',
            "Logout Location": emp.LogoutLocation || '-'
          };
          allDates.forEach(d => {
            row[d] = "";
          });
          companyMap[companyName].push(row);
        }
        if (emp.LeaveType) {
          row[attendanceDate] = emp.LeaveType;
        } else {
          row[attendanceDate] = emp.WorkingHours; // or ActiveHours
        }
      });
    });
    const workbook: XLSX.WorkBook = {
      Sheets: {},
      SheetNames: []
    };
    Object.keys(companyMap).forEach(companyName => {
      const worksheet = XLSX.utils.json_to_sheet(companyMap[companyName]);
      const range = XLSX.utils.decode_range(worksheet['!ref']!);
      for (let C = range.s.c; C <= range.e.c; C++) {
        const headerCell = XLSX.utils.encode_cell({ r: 0, c: C });
        const headerValue = worksheet[headerCell]?.v;
        if (holidayDates.has(headerValue)) {
          for (let R = 1; R <= range.e.r; R++) {
            const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
            if (!worksheet[cellRef]) continue;
            worksheet[cellRef].s = {
              fill: { fgColor: { rgb: '9EDBCF' } }, // Light Teal for Holiday
            };
          }
        }
        const date = new Date(headerValue);
        const dayOfWeek = date.getDay(); // 0 = Sunday, 6 = Saturday
        if (dayOfWeek === 0 || dayOfWeek === 6) { // Saturday (6) or Sunday (0)
          for (let R = 1; R <= range.e.r; R++) {
            const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
            if (!worksheet[cellRef]) continue;
            worksheet[cellRef].s = {
              fill: { fgColor: { rgb: 'FFCCCC' } }, // Light Red for weekends
            };
          }
        }
        for (let R = 1; R <= range.e.r; R++) {
          const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
          if (!worksheet[cellRef]) continue;
          const cellValue = worksheet[cellRef].v;
          switch (cellValue) {
            case 'EL': // Earned Leave
              worksheet[cellRef].s = {
                fill: { fgColor: { rgb: 'A3C6FF' } }, // Light Blue for EL
              };
              break;
            case 'CL': // Casual Leave
              worksheet[cellRef].s = {
                fill: { fgColor: { rgb: 'B172DB' } }, // Light Purple for CL
              };
              break;
            case 'RH': // Restricted Holiday
              worksheet[cellRef].s = {
                fill: { fgColor: { rgb: 'FFE7E9' } }, // Light Pink for RH
              };
              break;
            case 'LOP': // Loss of Pay
              worksheet[cellRef].s = {
                fill: { fgColor: { rgb: 'FF9999' } }, // Light Red for LOP
              };
              break;
            case 'Holiday': // Holiday
              worksheet[cellRef].s = {
                fill: { fgColor: { rgb: '9EDBCF' } }, // Light Teal for Holiday
              };
              break;
          }
        }
      }

      const safeSheetName = companyName.substring(0, 31);
      workbook.Sheets[safeSheetName] = worksheet;
      workbook.SheetNames.push(safeSheetName);
    });
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'EmployeeAttendance_DateWise.xlsx');
    this.dropdownVisible = false;
  }

  // exportToExcel(): void {
  //   if (this.isTableData === true) {
  //     this.triggerToast('Sorry', 'No data to export!', 'info');
  //     return;
  //   }

  //   const companyMap: { [key: string]: any[] } = {};

  //   const allDates = Array.from(
  //     new Set(this.attendanceArray.map((x: any) => x.AttendaceDate))
  //   );

  //   const holidayDates = new Set<string>();
  //   this.attendanceArray.forEach((item: any) => {
  //     item.lstofAttendance.forEach((emp: any) => {
  //       if (emp.LeaveType === 'Holiday') {
  //         holidayDates.add(item.AttendaceDate);
  //       }
  //     });
  //   });

  //   this.attendanceArray.forEach((item: any) => {
  //     const attendanceDate = item.AttendaceDate;

  //     item.lstofAttendance.forEach((emp: any) => {
  //       const companyName = emp.CompName || 'Unknown';

  //       if (!companyMap[companyName]) {
  //         companyMap[companyName] = [];
  //       }

  //       let row = companyMap[companyName].find(
  //         x => x.Code === emp.EmpCode
  //       );

  //       if (!row) {
  //         row = {
  //           "Sl No.": companyMap[companyName].length + 1,
  //           Name: emp.EmpName,
  //           Code: emp.EmpCode,
  //           "Pay Days": emp.PayDays,
  //           "Login Location": emp.LoginLocation || '-',
  //           "Logout Location": emp.LogoutLocation || '-'
  //         };

  //         allDates.forEach(d => {
  //           row[d] = "";
  //         });

  //         companyMap[companyName].push(row);
  //       }

  //       if (emp.LeaveType) {
  //         row[attendanceDate] = emp.LeaveType;
  //       } else {
  //         row[attendanceDate] = emp.WorkingHours;
  //       }
  //     });
  //   });

  //   const workbook: XLSX.WorkBook = {
  //     Sheets: {},
  //     SheetNames: []
  //   };

  //   Object.keys(companyMap).forEach(companyName => {

  //     // 🔥 ✅ STEP 1: Legend rows (your UI colors)
  //     const legendRows = [
  //       { Name: 'Onsite' },
  //       { Name: 'WFH' },
  //       { Name: 'Manual' },
  //       { Name: 'Comp Off' },
  //       { Name: 'Half Day' },
  //       { Name: 'LOP' },
  //       { Name: 'EL' },
  //       { Name: 'CL' },
  //       { Name: 'RH' },
  //       { Name: 'Holiday' },
  //       {} // gap
  //     ];

  //     const finalData = [...legendRows, ...companyMap[companyName]];

  //     const worksheet = XLSX.utils.json_to_sheet(finalData);

  //     const range = XLSX.utils.decode_range(worksheet['!ref']!);

  //     // 🔥 ✅ STEP 2: Legend Colors
  //     const legendColors: any = {
  //       Onsite: 'FFFF00',
  //       WFH: 'FFA500',
  //       Manual: 'C79670',
  //       'Comp Off': '63E5E5',
  //       'Half Day': 'C9E977',
  //       LOP: 'FF0000',
  //       EL: 'A3C6FF',
  //       CL: 'B172DB',
  //       RH: 'FFE7E9',
  //       Holiday: '9EDBCF'
  //     };

  //     for (let R = 1; R <= 10; R++) {
  //       const cellRef = XLSX.utils.encode_cell({ r: R, c: 0 });

  //       if (!worksheet[cellRef]) continue;

  //       const value = worksheet[cellRef].v;

  //       if (legendColors[value]) {
  //         worksheet[cellRef].s = {
  //           fill: { fgColor: { rgb: legendColors[value] } }
  //         };
  //       }
  //     }

  //     // 🔥 EXISTING LOGIC (UNCHANGED)
  //     for (let C = range.s.c; C <= range.e.c; C++) {
  //       const headerCell = XLSX.utils.encode_cell({ r: 0, c: C });
  //       const headerValue = worksheet[headerCell]?.v;

  //       if (holidayDates.has(headerValue)) {
  //         for (let R = 1; R <= range.e.r; R++) {
  //           const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
  //           if (!worksheet[cellRef]) continue;

  //           worksheet[cellRef].s = {
  //             fill: { fgColor: { rgb: '9EDBCF' } },
  //           };
  //         }
  //       }

  //       const date = new Date(headerValue);
  //       const dayOfWeek = date.getDay();

  //       if (dayOfWeek === 0 || dayOfWeek === 6) {
  //         for (let R = 1; R <= range.e.r; R++) {
  //           const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
  //           if (!worksheet[cellRef]) continue;

  //           worksheet[cellRef].s = {
  //             fill: { fgColor: { rgb: 'FFCCCC' } },
  //           };
  //         }
  //       }

  //       for (let R = 1; R <= range.e.r; R++) {
  //         const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
  //         if (!worksheet[cellRef]) continue;

  //         const cellValue = worksheet[cellRef].v;

  //         switch (cellValue) {
  //           case 'EL':
  //             worksheet[cellRef].s = { fill: { fgColor: { rgb: 'A3C6FF' } } };
  //             break;
  //           case 'CL':
  //             worksheet[cellRef].s = { fill: { fgColor: { rgb: 'B172DB' } } };
  //             break;
  //           case 'RH':
  //             worksheet[cellRef].s = { fill: { fgColor: { rgb: 'FFE7E9' } } };
  //             break;
  //           case 'LOP':
  //             worksheet[cellRef].s = { fill: { fgColor: { rgb: 'FF9999' } } };
  //             break;
  //           case 'Holiday':
  //             worksheet[cellRef].s = { fill: { fgColor: { rgb: '9EDBCF' } } };
  //             break;
  //         }
  //       }
  //     }

  //     const safeSheetName = companyName.substring(0, 31);
  //     workbook.Sheets[safeSheetName] = worksheet;
  //     workbook.SheetNames.push(safeSheetName);
  //   });

  //   const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
  //   const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
  //   FileSaver.saveAs(blobData, 'EmployeeAttendance_DateWise.xlsx');

  //   this.dropdownVisible = false;
  // }

  


  exportToPDF(): void {
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }

    const companyMap: { [key: string]: any[] } = {};

    // 🔥 Collect unique dates
    const allDates = Array.from(
      new Set(this.attendanceArray.map((x: any) => x.AttendaceDate))
    );

    // 🔥 Build company-wise employee rows (same as Excel)
    this.attendanceArray.forEach((item: any) => {
      const attendanceDate = item.AttendaceDate;

      item.lstofAttendance.forEach((emp: any) => {
        const companyName = emp.CompName || 'Unknown';

        if (!companyMap[companyName]) {
          companyMap[companyName] = [];
        }

        let row = companyMap[companyName].find(
          x => x.Code === emp.EmpCode
        );

        if (!row) {
          row = {
            "Sl No.": companyMap[companyName].length + 1,
            Name: emp.EmpName,
            Code: emp.EmpCode,
            "Pay Days": emp.PayDays
          };

          allDates.forEach(d => {
            row[d] = "";
          });

          companyMap[companyName].push(row);
        }

        row[attendanceDate] = emp.LeaveType
          ? emp.LeaveType
          : emp.WorkingHours;
      });
    });

    const doc = new jsPDF({
      orientation: 'landscape',
      unit: 'pt',
      format: 'a3',
    });

    let startY = 40;

    Object.keys(companyMap).forEach((companyName, index) => {

      if (index !== 0) {
        doc.addPage();
        startY = 40;
      }

      doc.setFontSize(14);
      doc.text(`Company: ${companyName}`, 40, 25);

      const headers = [
        "Sl No.",
        "Name",
        "Code",
        "Pay Days",
        ...allDates
      ];

      const body = companyMap[companyName].map(row =>
        headers.map(header => row[header] ?? "")
      );

      autoTable(doc, {
        head: [headers],
        body: body,
        startY: startY,
        styles: {
          fontSize: 6,
          cellPadding: 3,
          halign: 'center'
        },
        headStyles: {
          fillColor: [7, 47, 95],
          textColor: 255,
          fontStyle: 'bold'
        },

        didParseCell: function (data) {

          // Skip header row
          if (data.section === 'head') return;

          const columnIndex = data.column.index;
          const header = headers[columnIndex];

          // 🔹 Apply Weekend Color
          if (allDates.includes(header)) {
            const date = new Date(header);
            const day = date.getDay();

            if (day === 0 || day === 6) {
              data.cell.styles.fillColor = [255, 204, 204]; // Light Red
            }
          }

          const cellValue = data.cell.raw;

          // 🔹 Apply Leave Type Colors
          switch (cellValue) {
            case 'EL':
              data.cell.styles.fillColor = [163, 198, 255]; // Light Blue
              break;

            case 'CL':
              data.cell.styles.fillColor = [177, 114, 219]; // Purple
              break;

            case 'RH':
              data.cell.styles.fillColor = [255, 231, 233]; // Light Pink
              break;

            case 'LOP':
              data.cell.styles.fillColor = [255, 153, 153]; // Light Red
              break;

            case 'Holiday':
              data.cell.styles.fillColor = [158, 219, 207]; // Teal
              break;
          }
        },

        didDrawPage: (data) => {
          doc.setFontSize(16);
          doc.text('Employee Attendance Date-Wise Report', 40, 20);
        }
      });


    });

    doc.save('EmployeeAttendance_DateWise.pdf');
    this.dropdownVisible = false;
  }


  getEmployeeNameById(EmpId: string): string {
    const employee = this.employeeDetails.find((emp: any) => emp.EmpId === EmpId);
    return employee ? employee.EmpName : EmpId;
  }



  resetData() {
    this.months = [];
    this.employeeAttendanceForm?.reset();
    this.getDepartementRole = [];
    this.minDate = undefined;
    this.isValidEmployee = true;
    this.employeesSerach = [];
    this.isClickedFilter = false;
    this.pageSize = 10;
    this.isPagination = true;
    this.pageNumber = 1;
    setTimeout(() => {
      this.getCurrentMonthList()
    }, 200);
    this.employeeAttendance();
    if (this.isMonthSelected === true) {
      window.location.reload();
    }
  }


  formatTime(time: string): string {
    if (time) {
      return time.slice(0, 5);
    }
    return '00:00';
  }
  hoveredCellId: string | null = null;

  isValidTime(time: string): boolean {
    if (!time) return false;
    return time !== '00:00:00';
  }

  setTooltipContent(employee: any, date: any) {
    const attendanceRecord = this.attendanceArray.find(
      (attendance: any) => attendance.AttendaceDate === date
    );


    if (!attendanceRecord) return;

    const empAttendance = attendanceRecord['lstofAttendance']
      .find((attendance: any) => attendance.EmpName === employee.EmpName);
    console.log(empAttendance);
    if (!empAttendance) return;

    let tooltip = `
    <i class="fas fa-user"></i> Name &nbsp;:&nbsp; <strong>${empAttendance.EmpName}</strong><br>
    <i class="fas fa-briefcase"></i> Work Mode &nbsp;:&nbsp; <strong>${empAttendance.WorkType}</strong><br>
    <i class="fas fa-sun"></i> Shift &nbsp;:&nbsp; <strong>${empAttendance.ShiftName}</strong><br>
    <i class="fas fa-clock"></i> Active Hours &nbsp;:&nbsp;<strong>${empAttendance.ActiveHours}</strong><br>
  `;

    /* ================= ESSL ================= */
    if (
      this.isValidTime(empAttendance.ESSLLogInTime) ||
      this.isValidTime(empAttendance.ESSLLogOutTime) ||
      this.isValidTime(empAttendance.ESSLActiveHours)
    ) {
      tooltip += `
      <strong class="essl-title">ESSL</strong><br>
      ${this.isValidTime(empAttendance.ESSLLogInTime) ? `Login &nbsp;:&nbsp; <strong>${empAttendance.ESSLLogInTime}</strong><br>` : ''}
      ${this.isValidTime(empAttendance.ESSLLogOutTime) ? `Logout &nbsp&nbsp; <strong>${empAttendance.ESSLLogOutTime}</strong><br>` : ''}
      ${this.isValidTime(empAttendance.ESSLActiveHours) ? `Active &nbsp;:&nbsp; <strong>${empAttendance.ESSLActiveHours}</strong><br>` : ''}
    `;
    }

    /* ================= WFH ================= */
    if (
      this.isValidTime(empAttendance.WFHLogInTime) ||
      this.isValidTime(empAttendance.WFHLogOutTime) ||
      this.isValidTime(empAttendance.WFHActiveHours)
    ) {
      tooltip += `
      <strong class="wfh-title">WFH</strong><br>
      ${this.isValidTime(empAttendance.WFHLogInTime) ? `Login &nbsp;:&nbsp; <strong>${empAttendance.WFHLogInTime}</strong><br>` : ''}
      ${this.isValidTime(empAttendance.WFHLogOutTime) ? `Logout &nbsp;:&nbsp; <strong>${empAttendance.WFHLogOutTime}</strong><br>` : ''}
      ${this.isValidTime(empAttendance.WFHActiveHours) ? `Active &nbsp;:&nbsp; <strong>${empAttendance.WFHActiveHours}</strong><br>` : ''}
    `;
    }

    /* ================= ONSITE ================= */
    if (
      this.isValidTime(empAttendance.ONSITELogInTime) ||
      this.isValidTime(empAttendance.ONSITELogOutTime) ||
      this.isValidTime(empAttendance.ONSITEActiveHours)
    ) {
      tooltip += `
      <strong class="onsite-title">Onsite</strong><br>
      ${this.isValidTime(empAttendance.ONSITELogInTime) ? `Login &nbsp;:&nbsp; <strong>${empAttendance.ONSITELogInTime}</strong><br>` : ''}
      ${this.isValidTime(empAttendance.ONSITELogOutTime) ? `Logout &nbsp;:&nbsp; <strong>${empAttendance.ONSITELogOutTime}</strong><br>` : ''}
      ${this.isValidTime(empAttendance.ONSITEActiveHours) ? `Active &nbsp;:&nbsp; <strong>${empAttendance.ONSITEActiveHours}</strong><br>` : ''}
    `;
    }

    this.tooltipContent = tooltip;
    this.hoveredCellId = `${employee.EmpName}-${date}`;
  }



  clearTooltipContent(): void {
    this.tooltipContent = '';
    this.hoveredCellId = null;
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

