import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HrmsServiceService } from 'src/app/HRMS/hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgApexchartsModule } from "ng-apexcharts";
import { DashboardService } from 'src/app/HRMS/service/dashboard.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from 'bootstrap';
import { EChartsOption } from 'echarts';
import { leavesService } from 'src/app/HRMS/service/leaves.service';
import { Router, RouterModule } from '@angular/router';
import { SettingsService } from 'src/app/HRMS/service/settings.service';
import { NgxEchartsModule } from 'ngx-echarts';
import { AccessPolicyStoreService } from 'src/app/HRMS/service/accessPolicayApi.service';
import { AttendenceModuleService } from 'src/app/HRMS/service/attendence.service';
import { NotificationService } from 'src/app/HRMS/service/notification.service';
import { EntityStateService } from 'src/app/HRMS/service/entity-state.service';


type DayStatus =
  | 'none'
  | 'absent'
  | 'rh-holiday'
  | 'general-holiday';


interface CalendarDay {
  date: Date | null;
  dayNumber: number | null;
  status: DayStatus;
}

interface HolidayInfo {
  name: string;
  type: string;
}


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, ToastMessageComponent, NgApexchartsModule,
    NgbModule, RouterModule, NgxEchartsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  @ViewChild('workLocationChart') chart: any;
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  employeeDetails;
  isSpinner: boolean = false;
  attendanceDashboard: any = FormGroup;
  isFormSubmitted: boolean = false;
  today = new Date().toISOString().split('T')[0];
  getLocations: any[] = [];
  errMsgLocation: any;
  employees: any[] = [];
  errorMessageEmpName: any;
  cardErrorMsg: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;
  initalyToday: any;
  isSelfView = true;
  isSelfSelected = true;
  getLast7daysDahboardData: any;
  locationMessage: any;
  getLocationData: any;
  dashbooardLogActivity: any;
  isLoggedIn: boolean = false;
  isSubmitOverAll: any;
  tooltipContent: string | null = null;
  accessPolicy: any;
  controlAccessPage: any;

  // Increment per tick (adjust if needed)
  duration: number = 2000;
  finalDeviceCheckInCount: number = 0;
  finalAppCheckInCount: number = 0;
  finalOnSiteCheckInCount: number = 0;
  finaltotalEmployeesCount: number = 0

  yesterdayPresentCount: number = 0;
  yesterdayAbsentCount: number = 0;
  yesterdayLeaveCount: number = 0;
  yesterdayWFHCount: number = 0;
  yesterdayOnsiteCount: number = 0;

  listOfHoliday: any[] = [];
  listOfEmp: any[] = [];
  listOfBirthdayToday: any[] = [];
  listOfBirthdayWeek: any[] = [];
  listOfBirthdayMonth: any[] = [];
  holidaysList: any[] = [];
  nextHoliday: any = null;
  isHolidayLoader: boolean = true;
  isTeamsMemberLoader: boolean = true;
  todayVisitors: any[] = [];


  pendingLeaves: any[] = [];
  pendingLeavesCount = 0;
  allLeaves: any[] = [];
  allLeavesCount = 0;
  compOffList: any[] = [];
  compOffCount = 0;

  selectedModalType: string = '';
  modalData: any[] = [];
  selectedEmployeeType: string = '';
  modalDataEmployeeverView: any[] = [];

  employeeOverview = {
    newJoiners: [] as any[],
    exits: [] as any[],
    deactive: [] as any[]
  };

  shiftManagementList: any[] = [];
  unmappedEmployeesCount: number = 0;


  absenceDays: number = 0;
  finalCountAbsdays: number = 175;
  incrementAbsdays: number = 1;

  // charts
  errorConsolidatedAttendanceRes: any;
  chartOptionsWorkedHour: any;
  isShowLocationBreakDownChart = false
  workedHours: any;
  maxHours: any;
  workedHourChart: any;
  barChartOptions: any;


  chartOptions: any;
  gaugeOptions: any;
  attendanceOption: any;

  departmentWiseOptions: EChartsOption = {};
  private data: number[] = [];
  private departments = ['Management', 'HR', 'Marketing', 'Development', 'Testing', 'Others'];


  //This is for birthday and anniversady /////////
  selectedTab: 'yesterday' | 'today' | 'tomorrow' = 'today';

  constructor(
    private fb: FormBuilder,
    private readonly hrmsService: HrmsServiceService,
    private readonly dashboardService: DashboardService,
    private cdr: ChangeDetectorRef,
    private leaveSerive: leavesService,
    private router: Router,
    private readonly settingService: SettingsService,
    private readonly attendanceService: AttendenceModuleService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private notificationService: NotificationService,
    private entityStateService: EntityStateService,
    private eRef: ElementRef
  ) {
    this.attendanceDashboard = this.fb.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      location: [''],
      emloyee: [''],
    });
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    if (this.employeeDetails[0]?.OnSiteStatus === 'LOGIN' && this.employeeDetails) {
      this.isLoggedIn = true;
    }

    if (this.employeeDetails && this.employeeDetails[0].JoiningDate) {
      const timestamp = parseInt(this.employeeDetails[0].JoiningDate.replace('/Date(', '').replace(')/', ''));
      const joiningDate = new Date(timestamp);
      this.employeeDetails[0].formattedJoiningDate = joiningDate.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    }
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Dashboard'
      );
    });
    console.log(this.controlAccessPage);

  }

  ngOnInit(): void {
    // this.getEmployeeEventsSelf();
    const today = new Date();
    this.initalyToday = today.toISOString().split('T')[0];
    const fromDate = new Date();
    fromDate.setDate(today.getDate() - 7);
    const fromDateStr = fromDate.toISOString().split('T')[0];
    this.attendanceDashboard.patchValue({
      fromDate: fromDateStr,
      toDate: this.initalyToday,
    });
    this.toggleSelection('self');
    this.getLocation();
    // this.getUnreadCount();
    setTimeout(() => {
      this.getAllLeave();
    }, 100);
    this.individualLeaveCount();
    this.generateCalendar(this.currentYear, this.currentMonth);
    setTimeout(() => {
      this.attendanceDetails();
      this.departmentWiseDetails();
    }, 100);
  }

  getUnreadCount() {
    this.isSpinner = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.notificationService.GetUnreadCount(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', '', 'danger')
      }
    })
  }


  getTotalAttendance(attendanceData: any) {
    // Pull values from your API response
    const officeCount = attendanceData?.DeviceCheckInCount || 0;
    const onsiteCount = attendanceData?.OnSiteCount || 0;
    const wfhCount = attendanceData?.WFHCount || 0;
    // Prepare data for the ECharts pie chart
    const pieData = [
      { value: officeCount, name: 'Office Check-In' },
      { value: onsiteCount, name: 'On-Site' },
      { value: wfhCount, name: 'Work From Home' }
    ];
    const total = officeCount + onsiteCount + wfhCount;
    // Set up the chart options (design unchanged)
    this.chartOptions = {
      tooltip: {
        trigger: 'item',
        formatter: '{b}: {c} ({d}%)'
      },
      legend: {
        top: '5%',
        left: 'center',
        textStyle: { fontSize: 12 }
      },
      series: [
        {
          name: 'Work Location',
          type: 'pie',
          radius: ['40%', '70%'],
          avoidLabelOverlap: false,
          itemStyle: {
            borderRadius: 10,
            borderColor: '#fff',
            borderWidth: 2
          },
          label: { show: false, position: 'center' },
          emphasis: {
            label: {
              show: true,
              fontSize: 22,
              fontWeight: 'bold'
            }
          },
          labelLine: { show: false },
          data: pieData
        }
      ],
      graphic: [
        {
          type: 'text',
          left: 'center',
          top: '45%',
          style: {
            text: 'Total',
            textAlign: 'center',
            fontSize: 14,
            fill: '#888'
          }
        },
        {
          type: 'text',
          left: 'center',
          top: '55%',
          style: {
            text: total.toString(),
            textAlign: 'center',
            fontSize: 24,
            fontWeight: 'bold',
            fill: '#000'
          }
        }
      ],
      color: ['#37647D', '#80A473', '#DB9B4D']
    };
  }

  totalWorkHourvsWorked(totalHoursStr: string, workedHoursStr: string) {

    // const convertToHours = (time: string): number => {
    //   if (!time) return 0;
    //   const [h, m, s] = time.split(':').map(Number);
    //   return h + (m / 60) + (s / 3600);
    // };
    const convertToHours = (time: any): number => {
      if (!time) return 0;
      if (typeof time !== 'string') return 0;
      const [h = 0, m = 0, s = 0] = time.split(':').map(Number);

      return h + (m / 60) + (s / 3600);
    };
    const totalHours = convertToHours(totalHoursStr);
    const workedHours = convertToHours(workedHoursStr);

    this.gaugeOptions = {
      tooltip: {
        formatter: () => `Worked: ${workedHoursStr}` // ✅ original
      },
      series: [
        {
          name: 'Work Hours',
          type: 'gauge',
          startAngle: 180,
          endAngle: 0,
          center: ['50%', '75%'],
          min: 0,
          max: totalHours,
          radius: '100%',

          progress: {
            show: true,
            width: 18,
            itemStyle: { color: '#4CAF50' }
          },

          axisLine: {
            lineStyle: {
              width: 18,
              color: [[1, '#E0E0E0']]
            }
          },

          axisTick: { show: true },
          splitLine: { show: true },
          axisLabel: { show: false },
          pointer: { show: true },

          detail: {
            show: true,
            offsetCenter: [0, '-20%'],
            formatter: () => `${workedHoursStr}`, // ✅ EXACT API VALUE
            fontSize: 20,
            fontWeight: 'bold',
            color: '#333'
          },

          data: [{ value: workedHours }]
        },
        {
          type: 'gauge',
          startAngle: 180,
          endAngle: 0,
          center: ['50%', '75%'],
          radius: '100%',

          pointer: { show: false },
          progress: { show: false },
          axisLine: { show: false },
          axisTick: { show: false },
          splitLine: { show: false },
          axisLabel: { show: false },

          detail: {
            show: true,
            offsetCenter: [0, '30%'],
            formatter: () => `Total: ${totalHoursStr}`, // ✅ EXACT API VALUE
            fontSize: 14,
            color: '#888'
          },

          data: [{ value: 0 }]
        }
      ]
    };
  }


  attendanceDetails() {
    this.attendanceOption = {

      tooltip: {
        trigger: 'item',
        formatter: '{a} <br/>{b} : {c} ({d}%)'
      },
      legend: {
        bottom: 10,
        left: 'center',
        data: ['On-Time Check-In', 'Late Check-In', 'LOP', 'Leaves']
      },
      series: [
        {
          name: 'Attendance Details',
          type: 'pie',
          radius: '65%',
          center: ['50%', '50%'],
          data: [
            { value: 120, name: 'On-Time Check-In' },
            { value: 45, name: 'Late Check-In' },
            { value: 20, name: 'LOP' },
            { value: 30, name: 'Leaves' }
          ],
          emphasis: {
            itemStyle: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            }
          }
        }
      ]
    };
  }
  departmentWiseDetails() {
    this.data = this.departments.map(() => Math.round(Math.random() * 200));
    this.departmentWiseOptions = {
      xAxis: {
        max: 'dataMax'
      },
      yAxis: {
        type: 'category',
        data: this.departments,
        inverse: true,
        animationDuration: 300,
        animationDurationUpdate: 300,
        max: 3 // Only top 3 departments
      },
      series: [
        {
          realtimeSort: true,
          name: 'Department Stats',
          type: 'bar',
          data: this.data,
          label: {
            show: true,
            position: 'right',
            valueAnimation: true
          }
        }
      ],
      legend: {
        show: true
      },
      animationDuration: 0,
      animationDurationUpdate: 3000,
      animationEasing: 'linear',
      animationEasingUpdate: 'linear'
    };

    // 3. Start real-time update loop
    setInterval(() => {
      for (let i = 0; i < this.data.length; ++i) {
        this.data[i] += Math.random() > 0.9
          ? Math.round(Math.random() * 2000)
          : Math.round(Math.random() * 200);
      }

      // 4. Update chart series only
      this.departmentWiseOptions = {
        ...this.departmentWiseOptions,
        series: [
          {
            type: 'bar',
            realtimeSort: true,
            data: [...this.data],
            label: {
              show: true,
              position: 'right',
              valueAnimation: true
            }
          }
        ]
      };
    }, 3000);
  }

  goToApplyLeave() {
    this.router.navigate(['/leave'], { queryParams: { openModal: true } });
  }

  goToAddHoliday() {
    this.router.navigate(['/holidays'], { queryParams: { openModal: true } });
  }

  // ************ This is for Calendar purpose including holiday and leave *************//
  currentMonth = new Date().getMonth();
  currentYear = new Date().getFullYear();
  weeks: CalendarDay[][] = [];
  selectedDay: CalendarDay | null = null;
  selectedDayDetails: any = null;
  holidayMap = new Map<string, HolidayInfo>();

  months = [
    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
  ];
  generateCalendar(year: number, month: number) {
    this.weeks = [];
    const firstDayIndex = new Date(year, month, 1).getDay();
    const totalDays = new Date(year, month + 1, 0).getDate();
    let dayCounter = 1 - firstDayIndex;

    for (let w = 0; w < 6; w++) {
      const week: CalendarDay[] = [];
      for (let d = 0; d < 7; d++, dayCounter++) {
        if (dayCounter < 1 || dayCounter > totalDays) {
          week.push({ date: null, dayNumber: null, status: 'none' });
        } else {
          const dateObj = new Date(year, month, dayCounter);
          const status = this.computeStatusForDay(dateObj);
          week.push({ date: dateObj, dayNumber: dayCounter, status });
        }
      }
      this.weeks.push(week);
    }
  }
  prevMonth() {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }
    this.generateCalendar(this.currentYear, this.currentMonth);
    this.selectedDay = null;
  }

  nextMonth() {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.generateCalendar(this.currentYear, this.currentMonth);
    this.selectedDay = null;
  }
  isToday(date: Date | null): boolean {
    if (!date) return false;
    const today = new Date();
    return date.getFullYear() === today.getFullYear() &&
      date.getMonth() === today.getMonth() &&
      date.getDate() === today.getDate();
  }

  selectDay(day: CalendarDay) {
    if (!day?.date) return;

    const dateStr = this.formatDateToYMD(day.date);
    const holidayObj = this.holidayMap.get(dateStr);
    const leave = this.leaveMap.get(dateStr);

    // Determine what to display
    let reason = '—';
    let hours = '';
    let leaveReason = '';
    let isHoliday = false;
    let isLeave = false;

    if (holidayObj && leave) {
      reason = `${holidayObj.name} + ${leave.LeaveType}`;
      hours = '00:00';
      leaveReason = leave.Reason || '';
      isHoliday = true;
      isLeave = true;

    } else if (holidayObj) {
      reason = holidayObj.name;
      hours = 'Holiday';
      isHoliday = true;

    } else if (leave) {
      reason = leave.LeaveType;
      hours = '00:00';
      leaveReason = leave.Reason || '';
      isLeave = true;
    }

    this.selectedDay = day;
    this.selectedDayDetails = {
      dateStr: day.date.toDateString(),
      shift: '',
      reason,
      hours,
      leaveReason,
      isLeave,
      isHoliday
    };
  }


  selectDayByNumber(dayNum: number) {
    for (const w of this.weeks) {
      for (const d of w) {
        if (d.dayNumber === dayNum) { this.selectDay(d); return; }
      }
    }
  }
  computeStatusForDay(date: Date): DayStatus {
    const dateStr = this.formatDateToYMD(date);

    if (this.leaveMap.has(dateStr)) return 'absent';

    const holiday = this.holidayMap.get(dateStr);
    if (holiday) {
      if (holiday.type?.includes('RH')) return 'rh-holiday';
      return 'general-holiday';
    }

    return 'none';
  }


  parseDotNetDate(dotNetDateString: string): Date {
    const timestamp = parseInt(dotNetDateString.match(/\d+/)?.[0] || '0', 10);
    return new Date(timestamp);
  }

  formatDateToYMD(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  // ************ This is for Calendar purpose including holiday and leave *************//

  // ************ This is for leave API used for calendar  ************//
  getEmployeeLeave: any = [];
  leaveMap = new Map<string, any>();
  leaveBalances: { label: string, value: string, cls: string }[] = [];
  isloaderLeaveBalance: boolean = true;
  getAllLeave() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.isSpinner = true;
    this.leaveSerive.GetAllLeave(reqBody).subscribe({
      next: (res: any[]) => {
        for (const leave of res) {
          const start = this.parseDotNetDate(leave.StartDate);
          const end = this.parseDotNetDate(leave.EndDate);
          let loop = new Date(start);
          while (loop <= end) {
            const ymd = this.formatDateToYMD(loop);
            this.leaveMap.set(ymd, leave);
            loop.setDate(loop.getDate() + 1);
          }
        }
        this.generateCalendar(this.currentYear, this.currentMonth);
        this.isSpinner = false;
      },
      error: (err: any) => {
        this.isSpinner = false;
      }

    });
  }
  // ************ This is for leave API used for calendar  ************//


  // ************ This is for individualLeaveCount(Leave Card) API ************//
  individualLeaveCount() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.isloaderLeaveBalance = true;
    this.leaveSerive.IndividualLeaveCount(reqBody).subscribe({
      next: (res: any) => {
        const allLeaveItems: any[] = [];
        for (const key in res) {
          if (Array.isArray(res[key])) {
            allLeaveItems.push(...res[key]);
          }
        }
        this.leaveBalances = allLeaveItems.map(item => {
          const shortLabel = this.getShortLeaveType(item.LeaveType);
          return {
            label: shortLabel ? shortLabel : 'NA',
            value: item.ClosingBalance != null ? item.ClosingBalance.toFixed(2) : 'NA',
            cls: this.getLeaveClass(shortLabel)
          };
        });
        this.isloaderLeaveBalance = false;
      },
      error: (err: any) => {
        console.error("Error fetching leave balances", err);
        this.isloaderLeaveBalance = false;
        this.triggerToast('Internal Server Error', "For Leave Balance!", "danger");
      }
    });
  }

  getShortLeaveType(leaveType: string): string {
    return leaveType?.split('-')[0]?.trim() || 'Unknown';
  }
  getLeaveClass(leaveType: string): string {
    const type = leaveType.toLowerCase();
    if (type.includes('cl')) return 'lb-casual';
    if (type.includes('el')) return 'lb-earned';
    if (type.includes('rh')) return 'lb-rh';
    if (type.includes('pl')) return 'lb-paternity';
    if (type.includes('ml')) return 'lb-maternity';
    return 'lb-default';
  }
  // ************ This is for individualLeaveCount(Leave Card) API ************//


  //////////// This is Geolocation //////////////
  getLocation(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const latitude = position.coords.latitude;
          const longitude = position.coords.longitude;
          this.getPlaceDetails(latitude, longitude);
          this.locationMessage = null;
        },
        (error) => {
          this.locationMessage = this.getErrorMessage(error.code);
        }
      );
    } else {
      this.locationMessage = 'Geolocation is not supported by this browser.';
    }
  }
  private getErrorMessage(errorCode: number): string {
    switch (errorCode) {
      case 1:
        return 'User denied the request for Geolocation.';
      case 2:
        return 'Location information is unavailable.';
      case 3:
        return 'The request to get user location timed out.';
      default:
        return 'An unknown error occurred.';
    }
  }
  getLoginDashboardLocation: any;
  getPlaceDetails(latitude: number, longitude: number) {
    const geocodeUrl = `https://nominatim.openstreetmap.org/reverse?lat=${latitude}&lon=${longitude}&format=json`;
    fetch(geocodeUrl)
      .then(response => response.json())
      .then(data => {
        if (data && data.address) {
          // console.log(data);
          this.getLocationData = data;
          this.getLoginDashboardLocation = data;
        } else {
          console.error('Unable to retrieve location data');
        }
      })
      .catch(error => {
        console.error('Error with reverse geocoding:', error);
      });
  }
  //////////// This is Geolocation //////////////

  /////////// This is For toggleing Self And All /////////
  toggleSelection(value: string) {
    this.isSelfSelected = value === 'self';
    this.isSelfView = this.isSelfSelected;
    const isOverall = !this.isSelfSelected;

    this.getInitial7DaysDashboardData(isOverall);

    this.isSubmitOverAll = value;

    const today = new Date();
    this.initalyToday = today.toISOString().split('T')[0];

    const fromDate = new Date();
    fromDate.setDate(today.getDate() - 7);
    const fromDateStr = fromDate.toISOString().split('T')[0];

    this.attendanceDashboard.patchValue({
      fromDate: fromDateStr,
      toDate: this.initalyToday,
    });

    if (value == 'all') {
      this.attendanceDashboard?.get('location').patchValue('');
      this.attendanceDashboard?.get('emloyee').patchValue('');
      this.selectedEmployee = '';
      this.filteredEmployees = [];

      setTimeout(() => {
        this.employeeGetDDLocationApi();
        setTimeout(() => {
          this.getEmployeeSelectEmployee();
        }, 100);
      }, 100);

    } else {
      // ✅ RESET employee when switching to SELF
      this.attendanceDashboard?.get('emloyee')?.patchValue('');
      this.selectedEmployee = '';
      this.searchText = '';
      this.filteredEmployees = [];
      this.isValidEmployee = false;

      // OPTIONAL: clear validation errors
      this.attendanceDashboard.get('emloyee')?.setErrors(null);

      // ✅ IMPORTANT: SELF DATA CALL
      this.getEmployeeEventsSelf();
    }

    this.workedHours = '';
    this.maxHours = '';
    this.errorConsolidatedAttendanceRes = '';
    this.cardErrorMsg = '';
  }
  /////////// This is For toggleing Self And All /////////

  /////////// This is For inital 7 days Self And All /////////
  convertTimeToHours(time: string): number {
    if (!time) return 0;
    const [hours, minutes, seconds] = time.split(':').map(Number);
    return +(hours + (minutes / 60) + (seconds / 3600)).toFixed(2);
  }
  getShiftClassByIndex(index: number): string {
    switch (index) {
      case 0:
        return 'morning';   // color 1
      case 1:
        return 'evening';   // color 2
      case 2:
        return 'night';     // color 3
      case 3:
        return 'general';   // color 4 (create this class)
      default:
        return 'default-shift'; // बाकी shifts
    }
  }
  getInitial7DaysDashboardData(isOverall: boolean) {
    const today = new Date();
    const startDate = new Date(today);
    startDate.setDate(today.getDate() - 7);
    const endDate = new Date(today);
    const startDateStr = startDate.toISOString().split('T')[0];
    const endDateStr = endDate.toISOString().split('T')[0];

    this.isSpinner = true;

    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: 0,
      LocationId: 0,
      IsOverall: isOverall,
      StartDate: startDateStr,
      EndDate: endDateStr,
      LEId: Number(this.entityStateService.getEntityId()),
    };

    this.dashboardService.GetConsolidatedAttendanceData(reqBody).subscribe({
      next: (res: any) => {

        this.getLast7daysDahboardData = res;
        this.isShowLocationBreakDownChart = true;

        // ================= EXISTING =================

        const attendanceData = res.find((item: any) => item.AttendanceSource)?.AttendanceSource;
        if (attendanceData) {
          this.finalDeviceCheckInCount = attendanceData.DeviceCheckInCount;
          this.finalOnSiteCheckInCount = attendanceData.OnSiteCount;
          this.finalAppCheckInCount = attendanceData.WFHCount;
          this.finaltotalEmployeesCount = attendanceData.TotalEmployeeCount;
        }

        const workedHoursData = res.find((item: any) => item.CurrentMonthWorkedHours)?.CurrentMonthWorkedHours;
        if (workedHoursData) {
          this.workedHours = workedHoursData.TotalWH;
          this.maxHours = workedHoursData.MaxWH;
        }

        this.totalWorkHourvsWorked(this.maxHours, this.workedHours);

        const attendanceDataChart = res.find((item: any) => item.AttendanceSource)?.AttendanceSource;
        this.getTotalAttendance(attendanceDataChart);

        const onTimeCheckInData = res.find((item: any) => item.OnTimeCheckIn)?.OnTimeCheckIn;
        if (onTimeCheckInData) {
          const labels = onTimeCheckInData.map((item: any) => item.Date);
          const onTimeCounts = onTimeCheckInData.map((item: any) => item.OnTimeCheckInCount);
          const lateCheckInCounts = onTimeCheckInData.map((item: any) => item.LateCheckInCount);
          this.initializeBarChart(labels, onTimeCounts, lateCheckInCounts);
        }

        // ================= NEW CODE =================

        // ✅ Leave Management
        // Pending Leaves
        const pendingLeaves = res.find((item: any) => item.PendingLeaves)?.PendingLeaves;
        this.pendingLeaves = pendingLeaves ? pendingLeaves : [];
        this.pendingLeavesCount = this.pendingLeaves.length;

        // All Leaves
        const allLeaves = res.find((item: any) => item.AllLeaves)?.AllLeaves;
        this.allLeaves = allLeaves ? allLeaves : [];
        this.allLeavesCount = this.allLeaves.length;

        // Comp Off
        const compOff = res.find((item: any) => item.CompOffList)?.CompOffList;
        this.compOffList = compOff ? compOff : [];
        this.compOffCount = this.compOffList.length;

        // ✅ Visitors
        const visitors = res.find((item: any) => item.GetvisitorToday)?.GetvisitorToday;
        this.todayVisitors = visitors ? visitors : [];

        // ✅ Employee Overview
        const empList = res.find((item: any) => item.CurrentmonthemployeeList)?.CurrentmonthemployeeList;

        if (empList && empList.length > 0) {
          const currentMonth = new Date().getMonth();
          const currentYear = new Date().getFullYear();

          this.employeeOverview.newJoiners = empList.filter((emp: any) => {
            if (!emp.JoiningDate) return false;
            const joinDate = new Date(parseInt(emp.JoiningDate.replace(/[^0-9]/g, '')));
            return joinDate.getMonth() === currentMonth && joinDate.getFullYear() === currentYear;
          });

          this.employeeOverview.exits = empList.filter((emp: any) =>
            emp.EmpStatus === 'Inactive' || emp.EmpStatus === 'Exit'
          );

          this.employeeOverview.deactive = empList.filter((emp: any) =>
            emp.EmpStatus === 'Deactive'
          );
        } else {
          this.employeeOverview = { newJoiners: [], exits: [], deactive: [] };
        }

        // ✅ Shift Management (Dynamic)
        const shiftData = res.find((item: any) => item.ShiftManagement)?.ShiftManagement;
        this.shiftManagementList = shiftData ? shiftData : [];
        // Unmapped calculation (same logic)
        const totalMapped = this.shiftManagementList
          .reduce((sum: number, s: any) => sum + (s.ShiftEmpCount || 0), 0);

        this.unmappedEmployeesCount =
          (this.finaltotalEmployeesCount || 0) - totalMapped;

        // ✅ Yesterday Attendance (NEW - without affecting existing code)
        const yesterdayData = res.find((item: any) => item.YesterdayAttendanceDetails)?.YesterdayAttendanceDetails;

        if (yesterdayData) {
          this.yesterdayPresentCount = yesterdayData.PresentYesterday || 0;
          this.yesterdayAbsentCount = yesterdayData.AbsentYesterday || 0;
          this.yesterdayLeaveCount = yesterdayData.OnLeaveYesterday || 0;
          this.yesterdayWFHCount = yesterdayData.WFHYesterday || 0;
          this.yesterdayOnsiteCount = yesterdayData.ONSITEYesterday || 0;
        }

        this.isSpinner = false;
      },

      error: () => {
        this.isSpinner = false;
        this.errorConsolidatedAttendanceRes = 'Internal Server Error';
        this.cardErrorMsg = 'Internal Server Error';
      }
    });
  }
  /////////// This is For inital 7 days Self And All /////////

  /////// This is Location APi///////////
  employeeGetDDLocationApi() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.dashboardService.employeeGetLocation(reqBody).subscribe({
      next: (res: any) => {
        // console.log(res);
        this.getLocations = res
        this.isSpinner = false;
        this.errMsgLocation = ''
      },
      error: (error: any) => {
        this.errMsgLocation = 'Error loading data. Please try again.';
        this.isSpinner = false;
      }
    });
  }
  /////// This is Location APi///////////

  /////// This is Location Based Employee///////////
  locationBasedEmployee() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      LocationId: Number(this.attendanceDashboard?.get('location').value)
    }
    this.dashboardService.DDselectEmployee(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length >= 1) {
          this.employees = res;
        } else {
          this.triggerToast('', 'Employee Details Not Found for the given Location or EmpId', 'warning');
          this.employees = [];
          this.attendanceDashboard?.get('emloyee').reset();
        }
      },
      error: (error: any) => {
        this.triggerToast('Intaernal Server Error', '', 'danger')
        this.employees = [];
      }
    });
  }

  //this is second code for contact person
  getEmployeeSelectEmployee() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.dashboardService.employeeSelectEmployee(reqBody).subscribe({
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
        employee.FirstName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        employee.EmpCode.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employees];
    }
  }
  selectEmployeee(employee: any) {
    this.searchText = employee.FirstName;
    this.selectedEmployee = employee.EmpId;
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.FirstName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.attendanceDashboard.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.attendanceDashboard.get('emloyee')?.setErrors(null);
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
  @HostListener('document:click', ['$event'])
  clickOutside(event: Event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isDropdownOpen = false;
    }
  }
  //this is second code for contact person
  showTooltip(): void {
    this.tooltipContent = `<strong>Login Time : </strong>${this.employeeDetails[0]?.OnSiteLogInTime?.Hours}:${this.employeeDetails[0]?.OnSiteLogInTime?.Minutes}`;
  }

  hideTooltip(): void {
    this.tooltipContent = null;
  }


  LoginDashboardTime: any;
  LoginDashboardtoday: any;
  loginFunction(): void {
    const now = new Date();
    this.LoginDashboardtoday = now.toISOString().split('T')[0];
    this.LoginDashboardTime = now.toTimeString().split(' ')[0];
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpName: this.employeeDetails[0].FirstName,
      EmpCode: this.employeeDetails[0].EmpCode,
      LoginDate: this.LoginDashboardtoday,
      LoginTime: this.LoginDashboardTime,
      Company: 'WebApp',
      WorkStatus: "Login",
      LoginLatitude: this.getLocationData?.lat,
      LoginLongitude: this.getLocationData?.lon,
      LoginAddress: this.getLocationData?.display_name,
      LoginCity: this.getLocationData?.address['city'],
      Purpose: "",
      LogoutLatitude: '',
      LogoutLongitude: '',
      LogoutAddress: '',
      LogoutCity: '',
      LogOutTime: '',
      LogoutDescription: '',
    };
    console.log(reqBody);
    this.isSpinner = true;
    this.attendanceService.EmployeeAddOnSiteData(reqBody).subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('Login Failed', res['Message'], 'warning')
          this.isSpinner = false;
        } else {
          this.isLoggedIn = true;
          this.triggerToast('', 'Login Success', 'success');
          this.isSpinner = false;
          this.getEmployeeDetails();
        }
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Login Failed! Please Try Again', 'danger');
        this.isSpinner = false;
      }
    })
  }
  logoutFunction(): void {
    const now = new Date();
    const today = now.toISOString().split('T')[0];
    const currentTime = now.toTimeString().split(' ')[0];

    const formData = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpName: this.employeeDetails[0].FirstName,
      EmpCode: this.employeeDetails[0].EmpCode,
      Company: "WebApp",
      WorkStatus: "Logout",

      LoginLatitude: this.getLoginDashboardLocation?.lat,
      LoginLongitude: this.getLoginDashboardLocation?.lon,
      LoginAddress: this.getLoginDashboardLocation?.display_name,
      LoginCity: this.getLoginDashboardLocation?.address?.city,

      LoginDate: this.LoginDashboardtoday,
      LoginTime: this.LoginDashboardTime,

      Id: this.employeeDetails[0].OnSiteLogInId,
      LogoutDate: today,
      LogoutTime: currentTime,
      LogoutLatitude: this.getLocationData?.lat,
      LogoutLongitude: this.getLocationData?.lon,
      LogoutAddress: this.getLocationData?.display_name,
      LogoutCity: this.getLocationData?.address?.city,

      Purpose: "",
      Description: ""
    };
    console.log(formData);
    this.isSpinner = true;
    this.attendanceService.EmployeeAddOnSiteData(formData).subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('Login Failed', res['Message'], 'warning')
          this.isSpinner = false;
        } else {
          this.isLoggedIn = true;
          this.triggerToast('', 'Login Success', 'success')
          this.isSpinner = false;
          this.getEmployeeDetails();
        }
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Login Failed! Please Try Again', 'danger');
        this.isSpinner = false;
      }
    })

  }

  getEmployeeDetails() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      UserName: this.employeeDetails[0].UserName
    };
    this.hrmsService.GetEmployeeDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res) {
          sessionStorage.setItem('employeeDetails', JSON.stringify(res));
          window.location.reload();
        }
      },
      error: (err) => {
        this.triggerToast("Internal Server Error", "Get EmployeeDetails", "danger");
      },
      complete: () => {
        // Optional: Code to execute on completion, if any
      }
    });
  }

  getLoginLogsSelf() {
    const reqBody = {
      LoginId: this.employeeDetails[0]?.LoginId,
    }
    this.dashboardService.GetLoginLogs(reqBody).subscribe({
      next: (res: any) => {
        if (res) {
          console.log(res)
        }
      },
      error: (err) => {
        this.triggerToast("Internal Server Error", "Get EmployeeDetails", "danger");
      },
      complete: () => {

      }
    })
  }

  getAllLoginLogs() {
    const reqBody = {
      LoginId: this.employeeDetails[0]?.LoginId,
    }
    this.dashboardService.GetAllLoginLogs(reqBody).subscribe({
      next: (res: any) => {
        if (res) {
          console.log(res)
        }
      },
      error: (err) => {
        this.triggerToast("Internal Server Error", "Get EmployeeDetails", "danger");
      },
      complete: () => {
      }
    })
  }


  getDrilloutWorkLocationBreakdown() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.dashboardService.DashboardDetails(reqBody).subscribe({
      next: (res: any) => {
        console.log(res)
      }, error: (err: any) => {
        console.log(err)
      }
    })
  }

  onChartClick(config: any) {
    const clickedLabel = config.w.config.labels[config.dataPointIndex]; // Get the label of the clicked point
    const clickedValue = config.w.config.series[config.dataPointIndex]; // Correct way to access the clicked value
    console.log("Chart clicked!", clickedLabel, "Value:", clickedValue);
    this.getDrilloutWorkLocationBreakdown();
    const myModal = new Modal(document.getElementById('empWorkLocationBreakdownModal')!);
    myModal.show();
  }

  formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    const options: Intl.DateTimeFormatOptions = {
      day: '2-digit',
      month: 'short',
      year: '2-digit',
    };
    return date.toLocaleDateString('en-GB', options);
  }
  initializeBarChart(labels: string[], onTimeCounts: number[], lateCheckInCounts: number[]) {
    const formattedLabels = labels.map((label) => this.formatDate(label));
    const adjustedLateCheckInCounts = lateCheckInCounts.map(count => count * -1);
    // console.log(adjustedLateCheckInCounts);
    const maxOnTime = Math.max(...onTimeCounts);
    const minLate = Math.min(...adjustedLateCheckInCounts);
    const maxLate = Math.abs(minLate);
    const maxVal = Math.max(maxOnTime, maxLate);
    const minVal = -maxVal;
    this.barChartOptions = {
      series: [
        {
          name: 'On Time Check In',
          data: onTimeCounts,
          color: '#10B981'
        },
        {
          name: 'Late Check In',
          data: adjustedLateCheckInCounts,
          color: '#EF4444'
        }
      ],
      chart: {
        type: 'bar',
        height: 350,
        events: {
          dataPointSelection: (event: any, chartContext: any, config: any) => {
            this.onChartOnTineVsLate(config);
          }
        }
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '50%',
          endingShape: 'rounded',
          grouped: true
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
      },
      xaxis: {
        categories: formattedLabels,
      },
      yaxis: {
        title: {
          text: 'Check In Count'
        },
        min: minVal, // Ensure negative values are below 0
        max: maxVal // Ensure positive values are above 0
      },
      fill: {
        opacity: 1
      },
      legend: {
        position: 'top',
        horizontalAlign: 'center'
      },
    };
  }
  onChartOnTineVsLate(config: any) {
    const clickedLabel = config.w.config.labels[config.dataPointIndex]; // Get the label of the clicked point
    const clickedValue = config.w.config.series[config.dataPointIndex]; // Correct way to access the clicked value
    console.log("Chart clicked!", clickedLabel, "Value:", clickedValue);
    const myModal = new Modal(document.getElementById('empWorkLocationBreakdownModal')!);
    myModal.show();
  }
  //initalize charts Ends


  submitFilterData() {
    const fromDate = this.attendanceDashboard?.get('fromDate').value;
    const toDate = this.attendanceDashboard?.get('toDate').value;

    if (fromDate && toDate) {
      const from = new Date(fromDate);
      const to = new Date(toDate);
      const diffInTime = to.getTime() - from.getTime();
      const diffInDays = diffInTime / (1000 * 3600 * 24);

      if (diffInDays > 30) {
        this.isFormSubmitted = true;
        this.triggerToast('Invalid', 'The "To Date" must be within 31 days of the "From Date".', 'warning');
        return;
      }
    }

    if (this.attendanceDashboard.valid) {

      this.isFormSubmitted = false;

      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        StartDate: fromDate,
        EndDate: toDate,
        LocationId: Number(this.attendanceDashboard?.get('location').value) || 0,
        EmpId: this.selectedEmployee ? this.selectedEmployee : 0,
        IsOverall: this.isSubmitOverAll === 'all',
      };

      this.isSpinner = true;
      this.workedHours = '';
      this.maxHours = '';

      this.dashboardService.GetConsolidatedAttendanceData(reqBody).subscribe({
        next: (res: any) => {

          this.getLast7daysDahboardData = res;
          this.isShowLocationBreakDownChart = true;

          // ================= EXISTING =================

          const attendanceData = res.find((item: any) => item.AttendanceSource)?.AttendanceSource;
          if (attendanceData) {
            this.finalDeviceCheckInCount = attendanceData.DeviceCheckInCount;
            this.finalOnSiteCheckInCount = attendanceData.OnSiteCount;
            this.finalAppCheckInCount = attendanceData.WFHCount;
            this.finaltotalEmployeesCount = attendanceData.TotalEmployeeCount;
          }

          const workedHoursData = res.find((item: any) => item.CurrentMonthWorkedHours)?.CurrentMonthWorkedHours;
          if (workedHoursData) {
            this.workedHours = workedHoursData.TotalWH;
            this.maxHours = workedHoursData.MaxWH;
          }

          this.totalWorkHourvsWorked(this.maxHours, this.workedHours);

          const attendanceDataChart = res.find((item: any) => item.AttendanceSource)?.AttendanceSource;
          this.getTotalAttendance(attendanceDataChart);

          const onTimeCheckInData = res.find((item: any) => item.OnTimeCheckIn)?.OnTimeCheckIn;
          if (onTimeCheckInData) {
            const labels = onTimeCheckInData.map((item: any) => item.Date);
            const onTimeCounts = onTimeCheckInData.map((item: any) => item.OnTimeCheckInCount);
            const lateCheckInCounts = onTimeCheckInData.map((item: any) => item.LateCheckInCount);
            this.initializeBarChart(labels, onTimeCounts, lateCheckInCounts);
          }

          // ================= NEW CODE =================

          // ✅ Leave Management
          // Pending Leaves
          const pendingLeaves = res.find((item: any) => item.PendingLeaves)?.PendingLeaves;
          this.pendingLeaves = pendingLeaves ? pendingLeaves : [];
          this.pendingLeavesCount = this.pendingLeaves.length;

          // All Leaves
          const allLeaves = res.find((item: any) => item.AllLeaves)?.AllLeaves;
          this.allLeaves = allLeaves ? allLeaves : [];
          this.allLeavesCount = this.allLeaves.length;

          // Comp Off
          const compOff = res.find((item: any) => item.CompOffList)?.CompOffList;
          this.compOffList = compOff ? compOff : [];
          this.compOffCount = this.compOffList.length;

          // ✅ Visitors
          const visitors = res.find((item: any) => item.GetvisitorToday)?.GetvisitorToday;
          this.todayVisitors = visitors ? visitors : [];

          // ✅ Employee Overview
          const empList = res.find((item: any) => item.CurrentmonthemployeeList)?.CurrentmonthemployeeList;

          if (empList && empList.length > 0) {
            const currentMonth = new Date().getMonth();
            const currentYear = new Date().getFullYear();

            this.employeeOverview.newJoiners = empList.filter((emp: any) => {
              if (!emp.JoiningDate) return false;
              const joinDate = new Date(parseInt(emp.JoiningDate.replace(/[^0-9]/g, '')));
              return joinDate.getMonth() === currentMonth && joinDate.getFullYear() === currentYear;
            });

            this.employeeOverview.exits = empList.filter((emp: any) =>
              emp.EmpStatus === 'Inactive' || emp.EmpStatus === 'Exit'
            );

            this.employeeOverview.deactive = empList.filter((emp: any) =>
              emp.EmpStatus === 'Deactive'
            );
          } else {
            this.employeeOverview = { newJoiners: [], exits: [], deactive: [] };
          }

          // ===== ✅ Shift Management (NEW) =====
          // ✅ Shift Management (Dynamic)
          const shiftData = res.find((item: any) => item.ShiftManagement)?.ShiftManagement;

          this.shiftManagementList = shiftData ? shiftData : [];

          // Unmapped calculation (same logic)
          const totalMapped = this.shiftManagementList
            .reduce((sum: number, s: any) => sum + (s.ShiftEmpCount || 0), 0);

          this.unmappedEmployeesCount =
            (this.finaltotalEmployeesCount || 0) - totalMapped;

          // ✅ Yesterday Attendance (NEW - without affecting existing code)
          const yesterdayData = res.find((item: any) => item.YesterdayAttendanceDetails)?.YesterdayAttendanceDetails;

          if (yesterdayData) {
            this.yesterdayPresentCount = yesterdayData.PresentYesterday || 0;
            this.yesterdayAbsentCount = yesterdayData.AbsentYesterday || 0;
            this.yesterdayLeaveCount = yesterdayData.OnLeaveYesterday || 0;
            this.yesterdayWFHCount = yesterdayData.WFHYesterday || 0;
            this.yesterdayOnsiteCount = yesterdayData.ONSITEYesterday || 0;
          }

          this.isSpinner = false;
        },

        error: () => {
          this.isSpinner = false;
          this.errorConsolidatedAttendanceRes = 'Internal Server Error';
          this.cardErrorMsg = 'Internal Server Error';
        }
      });

    } else {
      this.isFormSubmitted = true;
      this.isSpinner = false;
    }
  }

  resetFormData() {
    this.attendanceDashboard.reset();
    this.toggleSelection('self');
    this.selectedEmployee = '';

  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }



  getVisitorStatus(visitor: any) {
    if (!visitor.Accept && !visitor.Approved) {
      return { label: 'Invited', class: 'status-invited' };
    } else if (visitor.Accept && !visitor.Approved) {
      return { label: 'Accepted', class: 'status-accepted' };
    } else if (visitor.Accept && visitor.Approved) {
      return { label: 'Check-in/out', class: 'status-checked' };
    } else {
      return { label: 'Unknown', class: '' };
    }
  }

  getEmployeeEventsSelf() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
    };
    this.dashboardService.GetEmployeeEvents(reqBody).subscribe({
      next: (res: any) => {
        console.log('Full Response:', res);

        // Store raw emplList and other data
        this.listOfEmp = res.lstofemp || [];

        // Store raw emplList and other data
        const birthdays = res.lstofbirthday || {};
        this.listOfBirthdayToday = birthdays.lstofdaybirthday || [];
        this.listOfBirthdayWeek = birthdays.lstofweekbirthday || [];
        this.listOfBirthdayMonth = birthdays.lstofmonthbirthday || [];

        // Store raw holidays and other data
        this.holidayMap.clear();
        this.listOfHoliday = res.lstofholiday || [];

        // Process & parse holidays for display
        this.holidaysList = this.listOfHoliday.map((h: any) => {
          const [day, month, year] = h.Date.split('-').map(Number);
          const dateObj = new Date(year, month - 1, day);
          return {
            name: h.Title,
            date: h.Date,
            dateObj,
            type: h.HolidayType,
            location: h.Location
          };
        });
        // Sort holidays chronologically
        this.holidaysList.sort((a, b) => a.dateObj.getTime() - b.dateObj.getTime());

        // Find next upcoming holiday (or fallback to first)
        const today = new Date();
        this.nextHoliday =
          this.holidaysList.find(h => h.dateObj >= today) || this.holidaysList[0];
        for (const holiday of this.holidaysList) {
          const [day, month, year] = holiday.date.split('-').map(Number);
          const dateObj = new Date(year, month - 1, day);
          const key = this.formatDateToYMD(dateObj).trim(); // trim just in case
          // this.holidayMap.set(key, holiday.name);
          this.holidayMap.set(key, {
            name: holiday.name,
            type: holiday.type   // "RH Holidays" | "General Holidays"
          });

        }
        this.generateCalendar(this.currentYear, this.currentMonth);
        this.isHolidayLoader = false;
        this.isTeamsMemberLoader = false;
      },
      error: (err: any) => {
        console.error('Error fetching employee events:', err);
        this.isHolidayLoader = false;
        this.isTeamsMemberLoader = false;
      }
    });
  }

  getPeople(tab: string): any[] {
    if (!tab) return [];
    switch (tab) {
      case 'today':
        return Array.isArray(this.listOfBirthdayToday) ? this.listOfBirthdayToday : [];
      case 'yesterday':
        return Array.isArray(this.listOfBirthdayWeek) ? this.listOfBirthdayWeek : [];
      case 'tomorrow':
        return Array.isArray(this.listOfBirthdayMonth) ? this.listOfBirthdayMonth : [];
      default:
        return [];
    }
  }

  getBirthdayPersonImage(person: any) {
    if (person.image) {
      return person.image;
    }

    if (person.Gender?.toLowerCase() === 'male') {
      return 'assets/3135715.png';
    } else if (person.Gender?.toLowerCase() === 'female') {
      return 'assets/8077066.jpg';
    }

    return 'assets/NoImage.jpg'; // fallback
  }

  formatDOB(dob: string): string {
    if (!dob) return '';
    const [day, month, year] = dob.split('-').map(Number);
    const date = new Date(year, month - 1, day);

    // Get month short name
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
      "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    return `${day < 10 ? '0' + day : day} ${monthNames[date.getMonth()]}`;
  }

  // getEmployeeImage(emp: any) {
  //   return emp.ImageUrl || `https://i.pravatar.cc/40?u=${emp.EmpId}`;
  // }
  getEmployeeImage(emp: any) {
    // if (emp.ImageUrl) {
    //   return emp.ImageUrl;
    // https://www.w3schools.com/howto/img_avatar.png
    // }
    if (emp.Gender?.toLowerCase() === 'male') {
      return 'assets/3135715.png';
    } else if (emp.Gender?.toLowerCase() === 'female') {
      return 'assets/8077066.jpg';
    }

    return 'assets/NoImage.jpg';
  }
  openTeamsChat(email: string): void {
    if (!email) {
      alert("No email found for this employee.");
      return;
    }
    const teamsUrl = `msteams://teams.microsoft.com/l/chat/0/0?users=${email}`;
    window.open(teamsUrl, '_blank', 'noopener,noreferrer');
  }

  openLeaveModal(type: string) {
    this.selectedModalType = type;

    if (type === 'pending') {
      this.modalData = this.pendingLeaves;
    } else if (type === 'all') {
      this.modalData = this.allLeaves;
    } else {
      this.modalData = this.compOffList;
    }
    // ✅ OPEN BOOTSTRAP MODAL
    const modal = new Modal(document.getElementById('leaveDetailsModal')!);
    modal.show();
  }



  openEmployeeModal(type: string) {

    console.log('TYPE:', type); // ✅ DEBUG

    this.selectedEmployeeType = type;

    if (type === 'new') {
      this.modalDataEmployeeverView = this.employeeOverview.newJoiners;
    } else if (type === 'exit') {
      this.modalDataEmployeeverView = this.employeeOverview.exits;
    } else {
      this.modalDataEmployeeverView = this.employeeOverview.deactive;
    }

    const modal = new Modal(document.getElementById('employeeModal')!);
    modal.show();
  }






  // Add these properties to your component

  // Add this method to open the holidays popup
  openHolidaysPopup() {
    const modal = new Modal(document.getElementById('upcomingHolidaysModal')!);
    modal.show();
  }


  // Add these methods for quick actions
  goToAddEmployee() {
    // Navigate to add employee page
    this.router.navigate(['/employees/add']);
  }
  goToViewVisitorPage() {
    this.router.navigate(['/view_visitor'])
  }

  goToApproveLeave() {
    // Navigate to leave approval page
    this.router.navigate(['/leave/approvals']);
  }

  goToUploadAttendance() {
    // Navigate to upload attendance page
    this.router.navigate(['/attendance/upload']);
  }

  // Add this property for unmapped shift count
  unmappedShiftCount = 6; // This should come from your API/service
  // Add this method to your DashboardComponent class
  getAttendancePercentage(): number {
    const totalPresent = (this.finalDeviceCheckInCount || 0) +
      (this.finalAppCheckInCount || 0) +
      (this.finalOnSiteCheckInCount || 0);
    const totalEmployees = this.finaltotalEmployeesCount || 1; // Avoid division by zero

    return Math.min(Math.round((totalPresent / totalEmployees) * 100), 100);
  }

}


