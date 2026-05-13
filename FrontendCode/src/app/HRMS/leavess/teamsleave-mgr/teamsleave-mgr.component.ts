import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccessPolicyRoutingModule } from "../../access-policy/access-policy-routing.module";
import { leavesService } from '../../service/leaves.service';
import { forkJoin } from 'rxjs';
import { of } from 'rxjs';
import { switchMap, delay, map } from 'rxjs/operators';
import { SettingsService } from '../../service/settings.service';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-teamsleave-mgr',
  standalone: true,
  imports: [ToastMessageComponent, SharedModule, CommonModule, ReactiveFormsModule, NgxPaginationModule, AccessPolicyRoutingModule],
  templateUrl: './teamsleave-mgr.component.html',
  styleUrl: './teamsleave-mgr.component.scss'
})
export class TeamsleaveMgrComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalApprove') closeModalApprove!: ElementRef;
  @ViewChild('closeModalReject') closeModalReject!: ElementRef;

  isSpinner: boolean = false;
  employeeDetails;
  currentDate: Date = new Date();
  weeks: string[][] = [];
  weekDays: string[] = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  statCards: { title: string; value: string; icon: string; bgGradient: string }[] = [];

  // ************This is for HR things *****************//
  showAllFiltersHR: boolean = false;

  rowsAppliedHR: any = [];
  originalRowsAppliedHR: any = [];
  isTableDataAppliedHR: any = false;
  errorMessageAppliedHR: any;
  pageAppliedHR1 = 1;
  pageSizeAppliedHR1 = 10;
  pageSizesAppliedHR1 = [10, 50, 100, 500];
  rowsAllListHR: any = [];
  originalRowsAllListHR: any = [];
  isTableDataAllListHR: any = false;
  errorMessageAllListHR: any;
  pageAllListHR = 1;
  pageSizeAllListHR = 10;
  pageSizesAllListHR = [10, 50, 100, 500];
  selectAllHR: boolean = false;
  selectedRowsHR: any[] = [];
  fromDate: string = '';
  toDate: string = '';
  status: string = '';
  combinedLeaveDates = new Set<string>(); // Holds yyyy-MM-dd strings for quick date mark lookup
  selectedDate: string = ''; // clicked date in yyyy-MM-dd format
  selectedDateLeaves: any[] = []; // leave rows for clicked date
  isRecordDeleted: boolean = false;


  constructor(private leaveSerive: leavesService, private readonly settingService: SettingsService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.generateCalendar(this.currentDate);
  }

  ngOnInit() {
    setTimeout(() => {
      this.loadAllHRLeaveData();
    }, 1000);
    this.employeeGetAllHolidays()
  }

  ///////////// This is for HR Purpose//////////////////////////////////////////////////////
  formatJsonDate(jsonDate: string | null | undefined): string {
    if (!jsonDate) return ''; // handle null or undefined
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (!match) return ''; // malformed string
    const timestamp = +match[1];
    const date = new Date(timestamp);
    // Format to dd-MM-yyyy in local time
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Jan = 0
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  }

  // loadAllHRLeaveData() {
  //   this.isSpinner = true;
  //   const reqBody = {
  //     LoginId: this.employeeDetails[0].LoginId,
  //     EmpId: this.employeeDetails[0].EmpId,
  //   };

  //   forkJoin({
  //     applied: this.leaveSerive.GetAllApplyManagerLeave(reqBody),
  //     others: this.leaveSerive.GetAllManagerLeave(reqBody),
  //   }).subscribe({
  //     next: ({ applied, others }: any) => {
  //       const formatDates = (list: any[]) =>
  //         list.map(item => ({
  //           ...item,
  //           StartDate: this.formatJsonDate(item.StartDate),
  //           EndDate: this.formatJsonDate(item.EndDate),
  //           AppliedDate: this.formatJsonDate(item.AppliedDate),
  //         }));

  //       const appliedList = Array.isArray(applied) ? formatDates(applied) : [];
  //       const otherList = Array.isArray(others) ? formatDates(others) : [];

  //       // ✅ Table 1: APPLIED only
  //       this.rowsAppliedHR = [...appliedList];
  //       this.originalRowsAppliedHR = [...appliedList];
  //       this.isTableDataAppliedHR = appliedList.length === 0;
  //       this.errorMessageAppliedHR = appliedList.length === 0 ? "No Data Found" : "";

  //       // ✅ Table 2: All other statuses
  //       this.rowsAllListHR = [...otherList];
  //       this.originalRowsAllListHR = [...otherList];
  //       this.isTableDataAllListHR = otherList.length === 0;
  //       this.errorMessageAllListHR = otherList.length === 0 ? "No Data Found" : "";

  //       this.generateCombinedLeaveDates();
  //       this.generateCalendar(this.currentDate);

  //       // ✅ Dashboard Stats (combined statuses)
  //       const counts = {
  //         applied: appliedList.filter(x => x.Status === 'APPLIED').length,
  //         approved: otherList.filter(x => x.Status?.includes('APPROVED')).length,
  //         rejected: otherList.filter(x => x.Status?.includes('REJECTED')).length,
  //         cancelled: otherList.filter(x => x.Status === 'CANCELLED').length,
  //       };

  //       this.statCards = [
  //         { title: 'APPLIED', value: counts.applied.toString(), icon: 'fas fa-hourglass-half', bgGradient: '#072F5F' },
  //         { title: 'APPROVED', value: counts.approved.toString(), icon: 'fas fa-check-circle', bgGradient: '#072F5F' },
  //         { title: 'REJECTED', value: counts.rejected.toString(), icon: 'fas fa-times-circle', bgGradient: '#072F5F' },
  //         { title: 'CANCELLED', value: counts.cancelled.toString(), icon: 'fas fa-calendar-times', bgGradient: '#072F5F' },
  //       ];

  //       this.isSpinner = false;
  //     },
  //     error: (err) => {
  //       console.error("Error fetching HR leave data", err);
  //       this.rowsAppliedHR = [];
  //       this.rowsAllListHR = [];
  //       this.isTableDataAppliedHR = true;
  //       this.isTableDataAllListHR = true;
  //       this.errorMessageAppliedHR = "Internal Server Error";
  //       this.errorMessageAllListHR = "Internal Server Error";
  //       this.statCards = [];
  //       this.isSpinner = false;
  //     }
  //   });
  // }

  holidayDates = new Set<string>();
  holidayDetailsMap = new Map<string, { title: string, type: string }>();

  formatDateToYMD(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  parseDotNetDate(dotNetDateStr: string): Date {
    const timestamp = parseInt(dotNetDateStr?.replace(/[^0-9]/g, ''), 10);
    return new Date(timestamp);
  }
  isToday(day: string): boolean {
    if (!day) return false;
    const today = new Date();
    const current = new Date(this.currentDate); // current displayed month
    const year = current.getFullYear();
    const month = current.getMonth(); // 0-based

    return (
      parseInt(day, 10) === today.getDate() &&
      month === today.getMonth() &&
      year === today.getFullYear()
    );
  }

  employeeGetAllHolidays() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    };
    this.settingService.employeeGetAllHolidays(reqBody).subscribe({
      next: (res: any) => {
        if (Array.isArray(res)) {
          this.holidayDates.clear();
          this.holidayDetailsMap.clear();

          res.forEach(holiday => {
            const holidayDate = this.parseDotNetDate(holiday.Date);
            const formattedDate = this.formatDateToYMD(holidayDate);
            this.holidayDates.add(formattedDate);
            this.holidayDetailsMap.set(formattedDate, {
              title: holiday.Title,
              type: holiday.HolidayType
            });
          });
          this.generateCalendar(this.currentDate);
        }
      },
      error: (err: any) => {
        console.error("Error fetching holidays", err);
      }
    });
  }

  loadAllHRLeaveData() {
    this.isSpinner = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.leaveSerive.GetAllApplyManagerLeave(reqBody).pipe(
      switchMap(applied =>
        of(null).pipe(
          delay(1000), // ⏱️ Wait for 1 second
          switchMap(() =>
            this.leaveSerive.GetAllManagerLeave(reqBody).pipe(
              map(others => ({ applied, others }))
            )
          )
        )
      )
    ).subscribe({
      next: ({ applied, others }: any) => {
        const formatDates = (list: any[]) =>
          list.map(item => ({
            ...item,
            StartDate: this.formatJsonDate(item.StartDate),
            EndDate: this.formatJsonDate(item.EndDate),
            AppliedDate: this.formatJsonDate(item.AppliedDate),
          }));

        const appliedList = Array.isArray(applied) ? formatDates(applied) : [];
        const otherList = Array.isArray(others) ? formatDates(others) : [];

        // ✅ Table 1: APPLIED only
        this.rowsAppliedHR = [...appliedList];
        this.originalRowsAppliedHR = [...appliedList];
        this.isTableDataAppliedHR = appliedList.length === 0;
        this.errorMessageAppliedHR = appliedList.length === 0 ? "No Data Found" : "";

        // ✅ Table 2: All other statuses
        // Define custom status priority
        const statusPriority: { [key: string]: number } = {
          'APPROVED': 1,
          'REJECTED': 2,
          'CANCELLED': 3,
          'WITHDRAWN': 4
        };

        // Sort by status priority
        otherList.sort((a, b) => {
          const aStatus = Object.keys(statusPriority).find(key => a.Status?.includes(key)) || '';
          const bStatus = Object.keys(statusPriority).find(key => b.Status?.includes(key)) || '';
          return (statusPriority[aStatus] || 99) - (statusPriority[bStatus] || 99);
        });
        this.rowsAllListHR = [...otherList];
        this.originalRowsAllListHR = [...otherList];
        this.isTableDataAllListHR = otherList.length === 0;
        this.errorMessageAllListHR = otherList.length === 0 ? "No Data Found" : "";
        this.generateCombinedLeaveDates();
        this.generateCalendar(this.currentDate);
        const counts = {
          applied: appliedList.filter(x => x.Status === 'APPLIED').length,
          approved: otherList.filter(x => x.Status?.includes('APPROVED')).length,
          rejected: otherList.filter(x => x.Status?.includes('REJECTED')).length,
          cancelled: otherList.filter(x => x.Status === 'CANCELLED').length,
          withdrawn: otherList.filter(x => x.Status === 'WITHDRAWN').length
        };
        this.statCards = [
          { title: 'APPLIED', value: counts.applied.toString(), icon: 'fas fa-hourglass-half', bgGradient: '#072F5F' },
          { title: 'APPROVED', value: counts.approved.toString(), icon: 'fas fa-check-circle', bgGradient: '#072F5F' },
          { title: 'REJECTED', value: counts.rejected.toString(), icon: 'fas fa-times-circle', bgGradient: '#072F5F' },
          { title: 'CANCELLED', value: counts.cancelled.toString(), icon: 'fas fa-calendar-times', bgGradient: '#072F5F' },
          { title: 'WITHDRAWN', value: counts.withdrawn.toString(), icon: 'fas fa-undo-alt', bgGradient: '#072F5F' }
        ];
        this.isSpinner = false;
      },
      error: (err) => {
        console.error("Error fetching HR leave data", err);
        this.rowsAppliedHR = [];
        this.rowsAllListHR = [];
        this.isTableDataAppliedHR = true;
        this.isTableDataAllListHR = true;
        this.errorMessageAppliedHR = "Internal Server Error";
        this.errorMessageAllListHR = "Internal Server Error";
        this.statCards = [];
        this.isSpinner = false;
      }
    });
  }

  applyFilterPending(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rowsAppliedHR = this.originalRowsAppliedHR.filter((row: any) => {
        const LeaveType = row.LeaveType?.toUpperCase() || '';
        const EmpName = row.EmpName?.toUpperCase() || '';
        const EmpCode = row.EmpCode?.toUpperCase() || '';
        const Approver = row.Approver?.toString().toUpperCase() || '';
        return (
          LeaveType.includes(filterValue) ||
          EmpName.includes(filterValue) ||
          EmpCode.includes(filterValue) ||
          Approver.includes(filterValue)

        );
      });
    } else {
      this.rowsAppliedHR = [...this.originalRowsAppliedHR];
      this.isTableDataAppliedHR = false;
    }
    if (this.rowsAppliedHR.length === 0) {
      this.isTableDataAppliedHR = true;
      this.errorMessageAppliedHR = 'No Records Found for Searched Data';
      this.rowsAppliedHR = [...this.originalRowsAppliedHR];
    } else {
      this.isTableDataAppliedHR = false;
      this.errorMessageAppliedHR = null;
    }
  }

   applyFilterAll(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rowsAllListHR = this.originalRowsAllListHR.filter((row: any) => {
        const LeaveType = row.LeaveType?.toUpperCase() || '';
        const EmpName = row.EmpName?.toUpperCase() || '';
        const EmpCode = row.EmpCode?.toUpperCase() || '';
        const Approver = row.Approver?.toString().toUpperCase() || '';
        return (
          LeaveType.includes(filterValue) ||
          EmpName.includes(filterValue) ||
          EmpCode.includes(filterValue) ||
          Approver.includes(filterValue)

        );
      });
    } else {
      this.rowsAllListHR = [...this.originalRowsAllListHR];
      this.isTableDataAllListHR = false;
    }
    if (this.rowsAllListHR.length === 0) {
      this.isTableDataAllListHR = true;
      this.errorMessageAllListHR = 'No Records Found for Searched Data';
      this.rowsAllListHR = [...this.originalRowsAllListHR];
    } else {
      this.isTableDataAllListHR = false;
      this.errorMessageAllListHR = null;
    }
  }

  prevMonth() {
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();
    this.currentDate = new Date(year, month - 1, 1);
    this.generateCalendar(this.currentDate);
  }
  nextMonth() {
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();
    this.currentDate = new Date(year, month + 1, 1);
    this.generateCalendar(this.currentDate);
  }
  generateCalendar(date: Date) {
    const year = date.getFullYear();
    const month = date.getMonth();
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const weeks: string[][] = [];
    let week: string[] = [];
    for (let i = 0; i < firstDay.getDay(); i++) {
      week.push('');
    }
    for (let day = 1; day <= lastDay.getDate(); day++) {
      week.push(day.toString());
      if (week.length === 7) {
        weeks.push(week);
        week = [];
      }
    }
    if (week.length > 0) {
      while (week.length < 7) {
        week.push('');
      }
      weeks.push(week);
    }
    this.weeks = weeks;
  }

  parseDateDMY(dateStr: string): Date {
    const parts = dateStr.split('-');
    if (parts.length !== 3) return new Date('');
    const day = parseInt(parts[0], 10);
    const month = parseInt(parts[1], 10) - 1;
    const year = parseInt(parts[2], 10);
    return new Date(year, month, day);
  }


  generateCombinedLeaveDates() {
    this.combinedLeaveDates.clear();
    const allLeaves = [...this.rowsAppliedHR, ...this.rowsAllListHR];

    allLeaves.forEach(leave => {
      let current = this.parseDateDMY(leave.StartDate);
      const end = this.parseDateDMY(leave.EndDate);

      while (current <= end) {
        const formatted = this.formatCalendarDate(new Date(current));
        this.combinedLeaveDates.add(formatted);
        current.setDate(current.getDate() + 1);
      }
    });
  }

  getDateObject(day: string): Date {
    const dayNum = parseInt(day, 10);
    if (isNaN(dayNum)) return new Date('');
    return new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), dayNum);
  }

  formatCalendarDate(date: Date): string {
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    return `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
  }

  formatDateToDMY(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  }
  selectedDateDisplay: any;

  onCalendarDateClick(day: string) {
    if (!day) return;
    const clickedDate = this.getDateObject(day);
    this.selectedDate = this.formatCalendarDate(clickedDate);  // yyyy-MM-dd, used for filtering
    this.selectedDateDisplay = this.formatDateToDMY(clickedDate); // dd-MM-yyyy, used for showing
    const allLeaves = [...this.rowsAppliedHR, ...this.rowsAllListHR];
    const convertToISO = (dateStr: string): string => {
      if (!dateStr) return '';
      const parts = dateStr.split('-'); // [dd, MM, yyyy]
      if (parts.length !== 3) return '';
      return `${parts[2]}-${parts[1]}-${parts[0]}`; // yyyy-MM-dd
    };
    this.selectedDateLeaves = allLeaves.filter(leave => {
      const startISO = convertToISO(leave.StartDate);
      const endISO = convertToISO(leave.EndDate);
      return startISO <= this.selectedDate && endISO >= this.selectedDate;
    });
  }

  getLeaveStatusCountForDate(dateStr: string): {
    approved: number;
    applied: number;
    total: number;
  } {
    let approved = 0;
    let applied = 0;
    let total = 0;
    const allLeaves = [...this.rowsAppliedHR, ...this.rowsAllListHR];
    const currentDate = this.parseDateDMY(this.formatDateToDMY(new Date(dateStr)));
    allLeaves.forEach(leave => {
      const start = this.parseDateDMY(leave.StartDate);
      const end = this.parseDateDMY(leave.EndDate);

      if (start <= currentDate && end >= currentDate) {
        total++;
        if (leave.Status?.includes('APPROVED')) {
          approved++;
        } else if (leave.Status === 'APPLIED') {
          applied++;
        }
        // You can also track rejected/cancelled/withdrawn if needed
      }
    });

    return { approved, applied, total };
  }

  getCircleClass(status: string): string {
    switch (status.toUpperCase()) {
      case 'APPLIED':
        return 'circle-orange';
      case 'APPROVED':
        return 'circle-green';
      case 'REJECTED':
        return 'circle-red';
      case 'CANCELLED':
        return 'circle-gray';
      case 'WITHDRAWN':
        return 'circle-brown';
      default:
        return '';
    }
  }

  // toggleAllSelectionHR() {
  //   this.rowsAppliedHR.forEach((row: any) => row.selected = this.selectAllHR);
  //   this.updateSelectedRows();
  // }
   toggleAllSelectionHR() {
    const startIndex = (this.pageAppliedHR1 - 1) * this.pageSizeAppliedHR1;
    const endIndex = startIndex + this.pageSizeAppliedHR1;
    const currentPageRows = this.rowsAppliedHR.slice(startIndex, endIndex);
    currentPageRows.forEach((row: any) => row.selected = this.selectAllHR);
    this.updateSelectedRows();
  }
  // updateSelection() {
  //   this.selectAllHR = this.rowsAppliedHR.every((row: any) => row.selected);
  //   this.updateSelectedRows();
  // }
  updateSelection() {
    const startIndex = (this.pageAppliedHR1 - 1) * this.pageSizeAppliedHR1;
    const endIndex = startIndex + this.pageSizeAppliedHR1;
    const currentPageRows = this.rowsAppliedHR.slice(startIndex, endIndex);
    this.selectAllHR = currentPageRows.every((row: any) => row.selected);
    this.updateSelectedRows();
  }
  updateSelectedRows() {
    this.selectedRowsHR = this.rowsAppliedHR.filter((row: any) => row.selected);
  }

  approvalRemarks: string = '';
  rejectRemarks: string = '';

  approveSelectedByHR() {
    if (this.selectedRowsHR.length === 0) {
      this.triggerToast('Please select at least one record.', 'No Selection', 'warning');
      return;
    }
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      lstofLevAppId: this.selectedRowsHR.map(row => ({
        LeaveAppId: row.LeaveAppId,
        Remarks: this.approvalRemarks || 'Leave Approved',
      }))
    };
    console.log(reqBody);
    this.isSpinner = true;
    this.leaveSerive.ApproveLeaveByManager(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Approved Successfully', 'success');
          this.isRecordDeleted = true;
          setTimeout(() => {
            this.closeModalApprove.nativeElement?.click();
            this.selectAllHR = false;
            this.resetRemarksApprove();
            this.loadAllHRLeaveData()
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
          this.isSpinner = false;
        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.isSpinner = false;
        }

      },
      error: (err: any) => {
        this.triggerToast(err['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
      },
      complete: () => {
        this.isSpinner = false;
      }
    });
  }
  resetRemarksApprove(): void {
    this.approvalRemarks = ''; 
  }

  rejectSelectedByHR() {
    if (this.selectedRowsHR.length === 0) {
      this.triggerToast('Please select at least one record.', 'No Selection', 'warning');
      return;
    }
    if (!this.rejectRemarks || this.rejectRemarks.trim() === '') {
      this.triggerToast('Remarks are required to reject the records.', 'Missing Remarks', 'warning');
      return;
    }
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      lstofLevAppId: this.selectedRowsHR.map(row => ({
        LeaveAppId: row.LeaveAppId,
        Remarks: this.rejectRemarks || 'Leave Rejected',
      }))
    };
    this.isSpinner = true;
    this.leaveSerive.RejectLeaveByManager(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Rejected Successfully', 'success');
          this.isRecordDeleted = true;
          setTimeout(() => {
            this.closeModalReject.nativeElement?.click();
            this.resetRemarksReject();
            this.loadAllHRLeaveData()
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
          this.isSpinner = false;
        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.isSpinner = false;
        }
      },
      error: (err: any) => {
        this.triggerToast(err['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
      },
      complete: () => {
        this.isSpinner = false;
      }
    });
  }
  resetRemarksReject(){
    this.rejectRemarks = '';
  }

  onStatCardClick(status: string) {
    this.pageAppliedHR1 = 1;
    this.pageAllListHR = 1;
    this.selectAllHR = false;
    this.selectedRowsHR = [];
    if (status.toUpperCase() === 'APPLIED') {
      this.showAllFiltersHR = false;
      this.status = ''; // clear status filter for pending list
    } else {
      this.showAllFiltersHR = true;
      this.status = status.toLowerCase();  // Set status filter for all list
      this.filterAllListByStatus(this.status);
    }
  }

  // Filter allListHR by status
  filterAllListByStatus(status: string) {
    if (!status) {
      this.rowsAllListHR = [...this.originalRowsAllListHR];
    } else {
      this.rowsAllListHR = this.originalRowsAllListHR.filter((row: any) =>
        row.Status?.toLowerCase().includes(status)
      );
    }
  }
  filterAllListHR() {
    this.rowsAllListHR = this.originalRowsAllListHR.filter((item: any) => {
      const parseDMY = (dateStr: string): Date => {
        const [dd, mm, yyyy] = dateStr.split('-').map(Number);
        return new Date(yyyy, mm - 1, dd); // JS Date: yyyy, MM (0-indexed), dd
      };
      const from = this.fromDate ? new Date(this.fromDate) : null; // yyyy-MM-dd
      const to = this.toDate ? new Date(this.toDate) : null;       // yyyy-MM-dd
      const itemStart = parseDMY(item.StartDate);
      const itemEnd = parseDMY(item.EndDate);
      // Strip time part to ensure consistent comparison
      const normalize = (d: Date) => new Date(d.getFullYear(), d.getMonth(), d.getDate());
      const matchesDate =
        (!from || !to) ||
        (normalize(itemStart) >= normalize(from) && normalize(itemEnd) <= normalize(to));
      const matchesStatus =
        !this.status || this.status === '' ||
        item.Status?.toLowerCase().includes(this.status.toLowerCase());
      return matchesDate && matchesStatus;
    });
    this.isTableDataAllListHR = this.rowsAllListHR.length === 0;
    this.errorMessageAllListHR = this.isTableDataAllListHR ? "No Data Found" : "";
  }

  dropdownVisible = false;

  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
  }
  // Listen for clicks anywhere in the document
  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const isDropdown = target.closest('.dropdown-content') !== null;
    const isButton = target.matches('.export-button');
    if (!isDropdown && !isButton) {
      this.dropdownVisible = false;
    }
    if (!target.closest('.custom-dropdown') && !target.closest('th')) {
      this.showStartDateInput = false;
      this.showEndDateInput = false;
      this.showStatusDropdown = false;
      this.showLeaveTypeMenu = false;
    }
  }

  exportFile(format: string) {
    const dataToExport = this.rowsAllListHR;
    if (!dataToExport || dataToExport.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return; // Prevent further execution
    }
    if (format === 'excel') {
      this.dropdownVisible = false;
      this.exportToExcel(dataToExport);
    }
    if (format === 'pdf') {
      this.exportToPdf(dataToExport);
      this.dropdownVisible = false;
    }
  }

  exportToExcel(data: any[]) {
    if (!data || data.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
    }
    const formattedData = data.map((item: any) => ({
      'Leave Type': item.LeaveType,
      'Employee Code': item.EmpCode,
      'Employee Name': item.EmpName,
      'Start Date': item.StartDate,
      'End Date': item.EndDate,
      'Period (days)': item.Duration,
      'Status': item.Status,
      'Approver': item.Approver,
      'Remarks': item.Remarks
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(formattedData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Leave Report': worksheet },
      SheetNames: ['Leave Report']
    };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob: Blob = new Blob([excelBuffer], { type: 'application/octet-stream' });

    FileSaver.saveAs(blob, 'Leave_Report.xlsx');
    console.log('Exported Excel with rows:', formattedData.length);
  }

  exportToPdf(data: any[]) {
    if (!data || data.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
    }
    const doc = new jsPDF();
    const columns = [
      { header: 'Leave Type', dataKey: 'LeaveType' },
      { header: 'Employee Code', dataKey: 'EmpCode' },
      { header: 'Employee Name', dataKey: 'EmpName' },
      { header: 'Leave Type', dataKey: 'LeaveType' },
      { header: 'Start Date', dataKey: 'StartDate' },
      { header: 'End Date', dataKey: 'EndDate' },
      { header: 'Duration', dataKey: 'Duration' },
      { header: 'Status', dataKey: 'Status' },
      { header: 'Approver', dataKey: 'Approver' },
      { header: 'Remarks', dataKey: 'Remarks' }
    ];
    const rows = data.map(item => ({
      ...item,
      StartDate: item.StartDate || '',
      EndDate: item.EndDate || '',
      Remarks: item.Remarks || ''
    }));

    doc.setFontSize(16);
    doc.text('Leave Application Report', 14, 22);
    autoTable(doc, {
      columns,
      body: rows,
      startY: 30,
      styles: { fontSize: 8 },
      headStyles: { fillColor: [41, 128, 185] },
      theme: 'grid'
    });
    doc.save('Leave_Report.pdf');
  }

  //   resetFiltersHR() {
  //   this.fromDate = '';
  //   this.toDate = '';
  //   this.status = '';
  //   this.selectAllHR = false;
  //   this.selectedRowsHR = [];
  //   this.pageAppliedHR1 = 1;
  //   this.pageAllListHR = 1;
  //   if (this.showAllFiltersHR) {
  //     this.rowsAllListHR = [...this.originalRowsAllListHR];
  //     this.isTableDataAllListHR = this.rowsAllListHR.length === 0;
  //     this.errorMessageAllListHR = this.rowsAllListHR.length === 0 ? "No Data Found" : "";
  //   } else {
  //     this.rowsAppliedHR = [...this.originalRowsAppliedHR];
  //     this.isTableDataAppliedHR = this.rowsAppliedHR.length === 0;
  //     this.errorMessageAppliedHR = this.rowsAppliedHR.length === 0 ? "No Data Found" : "";
  //   }
  // }

  onModalClose() {
    const originalDate: Date = new Date();
    this.currentDate = new Date(originalDate);
    this.generateCalendar(this.currentDate);
    this.selectedDate = '';
    this.selectedDateLeaves = [];
  }

  // Dropdown visibility
  showLeaveTypeMenu = false;

  // Selected filters
  filteredLeaveType: string = '';
  filteredStatus: string = '';

  statusOptions = ['APPROVED', 'REJECTED', 'CANCELLED', 'WITHDRAWN'];

  // Toggle methods
  toggleLeaveTypeMenu() {
    this.showLeaveTypeMenu = !this.showLeaveTypeMenu;
    this.showStatusDropdown = false;
  }


  // Set filters
  setFilterLeaveType(type: string) {
    this.filteredLeaveType = type;
    this.showLeaveTypeMenu = false;
    this.filterAllListHR();
  }

  showStartDateInput = false;
  showEndDateInput = false;

  clearStartDateFilter() {
    this.fromDate = '';
    this.filterAllListHR();
  }
  clearEndDateFilter() {
    this.toDate = '';
    this.filterAllListHR();
  }

  showStartDateDropdown = false;
  showEndDateDropdown = false;
  showStatusDropdown = false;


  toggleStartDateInput() {
    this.closeAllDropdowns();
    this.showStartDateInput = !this.showStartDateInput;
  }

  toggleEndDateInput() {
    this.closeAllDropdowns();
    this.showEndDateInput = !this.showEndDateInput;
  }

  toggleStatusDropdown() {
    this.closeAllDropdowns();
    this.showStatusDropdown = !this.showStatusDropdown;
  }

  closeAllDropdowns() {
    this.showStartDateInput = false;
    this.showEndDateInput = false;
    this.showStatusDropdown = false;
    this.showLeaveTypeMenu = false;
  }

  setFilterStatus(value: string) {
    this.status = value;
    this.showStatusDropdown = false;
    this.filterAllListHR();
    this.clearEndDateFilter()

  }
  ///////////// This is for HR Purpose//////////////////////////////////////////////////////

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
