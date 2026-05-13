import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { Modal } from 'bootstrap';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { combineLatest, startWith } from 'rxjs';
import { filter, debounceTime } from 'rxjs/operators';
import { environment } from 'src/assets/environment';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { leavesService } from '../../service/leaves.service';
import { SettingsService } from '../../service/settings.service';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-leaves',
  standalone: true,
  imports: [ToastMessageComponent, SharedModule, CommonModule,
    ReactiveFormsModule, NgxPaginationModule, RouterModule],
  templateUrl: './leaves.component.html',
  styleUrl: './leaves.component.scss'
})
export class LeavesComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;
  @ViewChild('closeModalwithdraw') closeModalwithdraw!: ElementRef;
  @ViewChild('deleteModal') deleteModal!: ElementRef;
  @ViewChild('compOffModalClose') compOffModalClose!: ElementRef;
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild('closeModal') closeModal!: ElementRef;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  [x: string]: any;
  applyLeaveForm: any = FormGroup;
  compOffForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  today = new Date().toISOString().split('T')[0];
  minDate: string | undefined;
  maxDate: string | undefined;
  isEdited: boolean = false;
  durationOptions: string[] = ['Full Day', 'Half Day'];
  filteredDurationOptions: string[] = [...this.durationOptions];
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  isSpinner: boolean = false;
  dropdownLeaveType: any = [];
  errorMessage: any;
  errorMessageCompOff: any;
  rows: any;
  originalRows: any;
  isTableData: boolean = false;
  isTableDataCompOff: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100];
  patchValue: any;
  isRecordDeleted: boolean = false;
  viewdata: any
  filterLeaveType: string = '';
  filterApprovedBy: string = '';
  filterStatus: string = '';
  filterDateRange: string = '';
  currentDate: Date = new Date();
  weeks: string[][] = [];
  weekDays: string[] = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  clickedDatesCount: number = 0;
  selectedStartDate: string | null = null;
  selectedEndDate: string | null = null;
  appliedLeaveDatesMap: { [date: string]: { leaveType: string; status: string } } = {};
  isCompOffSelected: boolean = false;
  getCurrentLeaves: any;
  selectedFiles: File[] = [];
  filePreviewUrls: string[] = [];
  getLeaveDocPath: any;
  leavesCard: any[] = [];
  minStartDate!: string;
  maxStartDate!: string;

  tabs: any[] = [];

  allTabs = [
    { id: 'create_leave_type', title: 'Create Leave Types', type: 'item', url: '/create_leave_type', icon: 'feather icon-folder' },
    { id: 'leave_balance_report', title: 'Leave Balance Report', type: 'item', url: '/leave_balance_report', icon: 'feather icon-clipboard' },
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }

  constructor(private fb: FormBuilder, private leaveSerive: leavesService,
    private router: Router, private readonly settingService: SettingsService,
    private route: ActivatedRoute, private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Leave'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; // ✅ Guard clause
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Leave'
      );
      console.log( this.controlAccessPage)
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((p: any) => p.PageName === tab.title && p.ViewAccess)
      );
    });
  }
  draftLeaveDatesSet = new Set();

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['openModal'] === 'true') {
        setTimeout(() => {
          this.openModal();
        }, 0);
      }
    });
    this.applyLeaveForm = this.fb.group({
      leaveParticulars: ['', [Validators.required]],
      date_from: ['', [Validators.required, this.sundayValidator]],
      date_to: ['', [Validators.required, this.sundayValidator]],
      noOfDays: ['', [Validators.required, Validators.min(0.5), Validators.max(999)]],
      leaveType: ['', [Validators.required]],
      leaveDay: ['', [Validators.required]],
      attachments: [''],
      compOffDate: [''],
      compOffReason: [''],
    });
    this.compOffForm = this.fb.group({
      projectName: ['', [Validators.required]],
      task: ['', [Validators.required]],
      date: ['', [Validators.required]],
      hours: ['', [Validators.required]],
      managerList: ['', [Validators.required]],
    })

    this.individualLeaveCount();
    this.employeeGetAllHolidays();
    const today = new Date();
    const applyDate = today.toISOString().split('T')[0];
    this.applyLeaveForm.patchValue({ date: applyDate });

    this.applyLeaveForm.get('date_from')?.valueChanges
      .pipe(debounceTime(200))
      .subscribe((value: any) => {
        this.handleDraftDateChange(value);
      });

    this.applyLeaveForm.get('date_to')?.valueChanges
      .pipe(debounceTime(200))
      .subscribe((value: any) => {
        this.handleDraftDateChange(value);
      });

    this.applyLeaveForm.get('leaveDay')?.valueChanges.subscribe(() => {
      this.updateNoOfDays();
    });
    this.generateCalendar(this.currentDate);
    setTimeout(() => {
      setTimeout(() => {
        this.getAllLeave();
      }, 1000);
      this.dropdownDDLeaveType();
    }, 100);
    this.applyLeaveForm.get('leaveType')?.valueChanges.subscribe((value: any) => {
      this.onLeaveTypeChange(value);
    });
    const currentDate = new Date();
    this.minStartDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, currentDate.getDate()));
    const nextYear = new Date(currentDate.getFullYear() + 1, currentDate.getMonth(), currentDate.getDate());
    this.maxStartDate = this.formatDate(nextYear);
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  hasShownDraftToast = false;
  handleDraftDateChange(value: any) {
    if (!this.isEdited && this.draftLeaveDatesSet.has(value)) {
      const draft = this.rows.find(
        (item: any) =>
          item.Status === 'DRAFT' &&
          value >= this.convertToISO(item.StartDate) &&
          value <= this.convertToISO(item.EndDate)
      );
      if (draft) {
        this.patchValue = draft;
        this.patchFormValues();
        if (!this.hasShownDraftToast) {
          this.triggerToast('Selected date is already in draft', 'Values patched', '');
          this.hasShownDraftToast = true;
        }
      }
    } else {
      this.tryLoadLeaveTypes();
      this.updateNoOfDays();
    }
  }
  onDateChange(event: any) {
    const selectedDate = event.target.value;
    if (selectedDate) {
      this.getCompOffHour(selectedDate);
    }
  }

  getCompOffAccutalHour: any;
  getCompOffHour(date: string) {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      EmpCode: this.employeeDetails[0].EmpCode,
      Date: date
    };

    this.isSpinner = true;
    this.leaveSerive.CompOffHours(reqBody).subscribe({
      next: (res: any) => {
        this.getCompOffAccutalHour = res;
        console.log(res);
        this.isSpinner = false;
      },
      error: () => {
        this.isSpinner = false;
        this.triggerToast(
          'Failed To Load The CompOff Hours',
          'Internal Server Error',
          'danger'
        );
      }
    });
  }

  // individualLeaveCount() {
  //   this.isSpinner = true;
  //   const reqBody = {
  //     LoginId: this.employeeDetails[0].LoginId,
  //     EmpId: this.employeeDetails[0].EmpId,
  //   };
  //   this.leaveSerive.IndividualLeaveCount(reqBody).subscribe({
  //     next: (res: any) => {
  //       this.getCurrentLeaves = res;
  //       const allLeaves: any[] = [];
  //       for (const key in res) {
  //         if (key.endsWith('Counts') && Array.isArray(res[key])) {
  //           allLeaves.push(...res[key]);
  //         }
  //       }
  //       const colorPalette = [
  //         '#cfa3ec', // Light purple
  //         '#ffa3d1', // Light pink
  //         '#a3c5ff', // Light blue
  //         '#ffd280', // Light orange
  //         '#ffcce6'  // Light rose
  //       ];
  //       const defaultColor = '#cccccc';
  //       this.leavesCard = allLeaves.map((leave: any, index: number) => {
  //         const total = leave.OpeningBalance || 0;
  //         const used = leave.Availed || 0;
  //         const remaining = leave.ClosingBalance || 0;
  //         return {
  //           type: leave.LeaveType,
  //           total,
  //           used,
  //           remaining,
  //           cardColor: colorPalette[index] || defaultColor
  //         };
  //       });
  //       this.isSpinner = false;
  //     },
  //     error: (err: any) => {
  //       console.error(err);
  //       this.isSpinner = false;
  //     }
  //   });
  // }

  individualLeaveCount() {
    this.isSpinner = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    };
    const EmpType = this.employeeDetails[0]?.EmpType;
    this.leaveSerive.IndividualLeaveCount(reqBody).subscribe({
      next: (res: any) => {
        this.getCurrentLeaves = res;
        const allLeaves: any[] = [];
        for (const key in res) {
          if (key.endsWith('Counts') && Array.isArray(res[key])) {
            allLeaves.push(...res[key]);
          }
        }
        // ✅ Hide EL for specific employee
        let filteredLeaves = allLeaves;
        if (EmpType === 'Contract') {
          filteredLeaves = allLeaves.filter((leave: any) => leave.LeaveType !== 'EL');
        }
        const colorPalette = [
          '#cfa3ec',
          '#ffa3d1',
          '#a3c5ff',
          '#ffd280',
          '#ffcce6'
        ];
        const defaultColor = '#cccccc';
        this.leavesCard = filteredLeaves.map((leave: any, index: number) => {
          const total = leave.OpeningBalance || 0;
          const used = leave.Availed || 0;
          const remaining = leave.ClosingBalance || 0;
          return {
            type: leave.LeaveType,
            total,
            used,
            remaining,
            cardColor: colorPalette[index] || defaultColor
          };
        });
        this.isSpinner = false;
      },
      error: (err: any) => {
        console.error(err);
        this.isSpinner = false;
      }
    });
  }

  getCardColor(leaveType: string): string {
    // Extract abbreviation from leaveType like "Earned Leave - (EL)" => "EL"
    const matchAbbr = leaveType.match(/\(([^)]+)\)$/);
    const typeAbbr = matchAbbr ? matchAbbr[1] : leaveType;

    const match = this.leavesCard?.find(card => card.type === typeAbbr);
    return match?.cardColor || '#9e4747ff'; // fallback color
  }

  navigateViewLeaveEmp() {
    if (this.employeeDetails[0].Authorised == true || this.employeeDetails[0].DeptName === ('Human Resource')) {
      this.router.navigate(['/teams_leaves']);
    } if (this.employeeDetails[0].Authorised == true && this.employeeDetails[0].DeptName != ('Human Resource')) {
      this.router.navigate(['/teams_leave']);
    }
  }
  viewCompOffRequest() {
    this.router.navigate(['/compoff_request']);
  }
  tryLoadLeaveTypes() {
    const start = this.applyLeaveForm.get('date_from')?.value;
    const end = this.applyLeaveForm.get('date_to')?.value;
    if (start && end) {
      this.dropdownDDLeaveType();
    }
  }
  onLeaveTypeChange(selectedLeaveTypeId: string): void {
    const selectedLeaveType = this.dropdownLeaveType.find(
      (type: any) => type.LeaveTypeId === +selectedLeaveTypeId
    );
    if (selectedLeaveType?.LeaveType === 'Compensatory off - (Comp Off)') {
      this.isCompOffSelected = true;
      this.applyLeaveForm.get('compOffDate')?.setValidators([Validators.required]);
      this.applyLeaveForm.get('compOffReason')?.setValidators([
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(50)
      ]);
    } else {
      this.isCompOffSelected = false;
      this.applyLeaveForm.get('compOffDate')?.clearValidators();
      this.applyLeaveForm.get('compOffReason')?.clearValidators();
      this.applyLeaveForm.get('compOffDate')?.setValue('');
      this.applyLeaveForm.get('compOffReason')?.setValue('');
    }
    this.applyLeaveForm.get('compOffDate')?.updateValueAndValidity();
    this.applyLeaveForm.get('compOffReason')?.updateValueAndValidity();
  }
  dropdownDDLeaveType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      StartDate: this.applyLeaveForm?.get('date_from').value ? this.applyLeaveForm?.get('date_from').value : '',
      EndDate: this.applyLeaveForm?.get('date_to').value ? this.applyLeaveForm?.get('date_to').value : '',
    }
    this.isSpinner = true;
    this.leaveSerive.DDLeaveType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.isSpinner = false;
        this.dropdownLeaveType = res;
      } else {
        this.isSpinner = false;
      }
    }, error => {
      this.isSpinner = false;
      this.triggerToast('Something Went Wrong', 'Failed to load LeaveType dropdown', 'danger');
    })
  }
  // this is for calendar purpose
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
  formatDateToLocalIso(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  formatJsonDate(jsonDate: string | null | undefined): string {
    if (!jsonDate || typeof jsonDate !== 'string') {
      return '';
    }
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (!match) {
      return '';
    }
    const timestamp = +match[1];
    const date = new Date(timestamp);
    return `${date.getDate().toString().padStart(2, '0')}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getFullYear()}`;
  }
  onCalendarDateClick(day: string): void {
    if (!day) return;
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth(); // 0-based
    const selectedDate = new Date(year, month, parseInt(day));
    const isoDate = this.formatDateToLocalIso(selectedDate);
    if (this.clickedDatesCount === 0) {
      this.selectedStartDate = isoDate;
      this.applyLeaveForm.patchValue({ date_from: isoDate });
      this.clickedDatesCount = 1;
      this.minDate = isoDate;
    } else if (this.clickedDatesCount === 1) {
      this.selectedEndDate = isoDate;
      this.applyLeaveForm.patchValue({ date_to: isoDate });
      this.maxStartDate = isoDate;
      this.clickedDatesCount = 0;
      // this.updateNoOfDays();
    } else {
      this.clickedDatesCount = 0;
      this.selectedStartDate = null;
      this.selectedEndDate = null;
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
  onModalClose() {
    const originalDate: Date = new Date();
    this.currentDate = new Date(originalDate);
    this.generateCalendar(this.currentDate);
  }
  getDayClass(day: string): string {
    if (!day) return '';
    const date = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), +day);
    const iso = this.formatDateToLocalIso(date);
    const leaveInfo = this.appliedLeaveDatesMap[iso];
    if (leaveInfo) {
      const status = leaveInfo.status?.toUpperCase();
      const statusClassMap: { [key: string]: string } = {
        'APPROVED': 'status-approved-hr-calendar',
        'REJECTED': 'status-rejected-mgr-calendar',
        'CANCELLED': 'status-cancelled-calendar',
        'DRAFT': 'status-draft-calendar',
        'WITHDRAWN': 'status-withdraw-calendar',
        'APPLIED': 'status-applied-calendar'
      };
      return `${statusClassMap[status] || ''}`;
    }
    return '';
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

  getLeaveLabel(day: string): string | null {
    if (!day) return null;
    const date = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), +day);
    const iso = this.formatDateToLocalIso(date);
    const leaveInfo = this.appliedLeaveDatesMap[iso];
    if (leaveInfo) {
      return `${leaveInfo.leaveType} - ${leaveInfo.status}`;
    }
    return null;
  }
  truncateText(text: string | null, maxLength: number): string {
    if (!text) return '';
    return text.length > maxLength ? text.slice(0, maxLength) + '...' : text;
  }
  getLeaveInfo(day: string): { leaveType: string; status: string } | null {
    if (!day) return null;
    const date = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), +day);
    const iso = this.formatDateToLocalIso(date);
    return this.appliedLeaveDatesMap[iso] || null;
  }

  getLeaveAbbr(day: string): string | null {
    if (!day) return null;
    const date = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), +day);
    const iso = this.formatDateToLocalIso(date);
    const leaveInfo = this.appliedLeaveDatesMap[iso];
    if (leaveInfo) {
      const normalizedLeaveType = leaveInfo.leaveType
        .replace(/\s*-\s*\([A-Z]+\)(\s*-\s*\([A-Z]+\))?$/, '')
        .trim();
      const leaveTypeShortMap: { [key: string]: string } = {
        'Sick Leave': 'SL',
        'Casual Leave': 'CL',
        'Earned Leave': 'EL',
        'Medical Leave': 'ML',
        'Reserved Holiday': 'RH',
        'Compensatory off': 'CO',
        'LOP': 'LOP'
      };
      for (const key in leaveTypeShortMap) {
        if (normalizedLeaveType.includes(key)) {
          return leaveTypeShortMap[key];
        }
      }
      return 'LV';
    }
    return null;
  }
  getHoliday(day: string): any | null {
    if (!day) return null;
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();
    const date = new Date(year, month, +day);
    const iso = `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
    return this.holidayDates.get(iso) || null;
  }
  // this is for calendar purpose

  // this is for File upload purpose
  onFileSelected(event: any) {
    const files: FileList = event.target.files;

    const allowedTypes = [
      'application/pdf',
      'image/png',
      'image/jpeg',
      'image/jpg',
      'image/webp'
    ];

    const validFiles: File[] = [];

    for (let i = 0; i < files.length; i++) {
      const file = files[i];

      if (!allowedTypes.includes(file.type)) {
        this.triggerToast(
          'Invalid file type',
          'Only PDF and image files are allowed',
          'warning'
        );
        continue;
      }

      validFiles.push(file);
    }

    if (validFiles.length === 0) {
      return;
    }

    this.selectedFiles.push(...validFiles);
    this.applyLeaveForm.get('attachments')?.setValue(this.selectedFiles);

    validFiles.forEach(file => {
      this.filePreviewUrls.push(URL.createObjectURL(file));
    });

    // ✅ Upload ONCE instead of per file
    this.isSpinner = true;
    this.leaveSerive.UploadFileLeave(
      this.employeeDetails[0].EmpId,
      'Leave',
      this.selectedFiles
    ).subscribe({
      next: (res) => {
        this.getLeaveDocPath = res.path;
        console.log('Upload success:', res);
        this.isSpinner = false;
      },
      error: (err) => {
        this.isSpinner = false;
        console.error('Upload failed:', err);
      }
    });
  }

  removeFile(index: number) {
    URL.revokeObjectURL(this.filePreviewUrls[index]);
    this.selectedFiles.splice(index, 1);
    this.filePreviewUrls.splice(index, 1);
    this.applyLeaveForm.get('attachments')?.setValue(this.selectedFiles);
    if (this.selectedFiles.length === 0) {
      this.getLeaveDocPath = '';
    }
  }
  isImage(file: File): boolean {
    return file.type.startsWith('image/');
  }
  isPdf(file: File): boolean {
    return file.type === 'application/pdf';
  }
  openFile(index: number) {
    const file = this.selectedFiles[index];
    const url = this.filePreviewUrls[index];
    if (this.isPdf(file) || this.isImage(file)) {
      window.open(url, '_blank');
    } else {
      alert('Preview not available for this file type.');
    }
  }
  //  This is for Uplaod File Purpose
  getAllLeave() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    };

    this.isSpinner = true;
    this.leaveSerive.GetAllLeave(reqBody).subscribe({
      next: (res: any) => {
        this.draftLeaveDatesSet.clear();

        if (res.length >= 1) {
          const formattedData = res.map((item: any) => {
            const startDate = this.formatJsonDate(item.StartDate);
            const endDate = this.formatJsonDate(item.EndDate);
            let normalizedStatus = item.Status;
            if (item.Status?.includes('APPROVED')) {
              normalizedStatus = 'APPROVED';
            } else if (item.Status?.includes('REJECTED')) {
              normalizedStatus = 'REJECTED';
            }
            if (item.Status === 'DRAFT') {
              let start = new Date(startDate.split('-').reverse().join('-'));
              const end = new Date(endDate.split('-').reverse().join('-'));
              while (start <= end) {
                const iso = this.formatDateToLocalIso(start); // yyyy-MM-dd
                this.draftLeaveDatesSet.add(iso);
                start.setDate(start.getDate() + 1);
              }
            }
            const start = new Date(startDate.split('-').reverse().join('-'));
            const end = new Date(endDate.split('-').reverse().join('-'));
            while (start <= end) {
              const iso = this.formatDateToLocalIso(start);
              this.appliedLeaveDatesMap[iso] = {
                leaveType: item.LeaveType,
                status: normalizedStatus,
              };
              start.setDate(start.getDate() + 1);
            }
            return {
              ...item,
              StartDate: startDate,
              EndDate: endDate,
              CreatedDate: this.formatJsonDate(item.CreatedDate),
              Status: normalizedStatus,
            };
          });
          const statusOrder = ['DRAFT', 'APPLIED', 'APPROVED', 'REJECTED', 'CANCELLED', 'WITHDRAWN'];
          formattedData.sort((a: any, b: any) => {
            return statusOrder.indexOf(a.Status) - statusOrder.indexOf(b.Status);
          });
          this.rows = [...formattedData];
          this.originalRows = [...formattedData];
          this.isSpinner = false;
          this.isTableData = false;
        } else {
          this.errorMessage = 'No records found';
          this.isSpinner = false;
          this.isTableData = true;
        }
      },
      error: () => {
        this.errorMessage = 'Internal Server Error';
        this.isSpinner = false;
        this.isTableData = true;
      }
    });
  }
  isTableDataEmpty: any;
  onFilterChange() {
    const leaveType = this.filterLeaveType;
    const approvedBy = this.filterApprovedBy;
    const status = this.filterStatus;
    const daysRange = this.filterDateRange;
    const now = new Date();
    this.rows = this.originalRows.filter((row: any) => {
      let isMatch = true;
      if (leaveType && row.LeaveTypeId != leaveType) {
        isMatch = false;
      }
      if (approvedBy && row.Approver !== approvedBy) {
        isMatch = false;
      }
      if (status && row.Status !== status) {
        isMatch = false;
      }
      if (daysRange) {
        const days = parseInt(daysRange, 10);
        const cutoffDate = new Date(now);
        cutoffDate.setDate(now.getDate() - days);
        const [day, month, year] = row.StartDate.split('-').map(Number);
        const leaveDate = new Date(year, month - 1, day);
        if (leaveDate < cutoffDate) {
          isMatch = false;
        }
      }
      return isMatch;
    });
    this.isTableDataEmpty = this.rows.length === 0;
    this.errorMessage = this.isTableDataEmpty ? 'No Data Found' : '';
  }
  showLeaveTypeMenu = false;
  toggleLeaveTypeMenu() {
    this.showLeaveTypeMenu = !this.showLeaveTypeMenu;
  }
  setFilterLeaveType(value: string) {
    this.filterLeaveType = value;
    this.showLeaveTypeMenu = false;
    this.onFilterChange();
  }
  showStatusDropdown = false;
  statusOptions: string[] = [
    'APPROVED',
    'REJECTED',
    'APPLIED',
    'CANCELLED',
    'DRAFT',
    'WITHDRAWN'
  ];

  toggleStatusDropdown() {
    this.showStatusDropdown = !this.showStatusDropdown;
  }

  setFilterStatus(value: string) {
    this.filterStatus = value;
    this.showStatusDropdown = false;
    this.onFilterChange();
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) => {
        const leaveType = row.LeaveType?.toString().toUpperCase() || ''; // Replace with LeaveType name if available
        const fromDate = this.formatJsonDate(row.StartDate).toUpperCase();
        const toDate = this.formatJsonDate(row.EndDate).toUpperCase();
        const duration = row.Duration?.toString().toUpperCase() || '';
        const approvedBy = row.Approver?.toUpperCase() || '';
        const status = row.Status?.toUpperCase() || '';
        return (
          leaveType.includes(filterValue) ||
          fromDate.includes(filterValue) ||
          toDate.includes(filterValue) ||
          duration.includes(filterValue) ||
          approvedBy.includes(filterValue) ||
          status.includes(filterValue)
        );
      });
    } else {
      this.rows = [...this.originalRows];
      this.isTableData = false;
    }
    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = 'No Records Found for Searched Data';
      this.rows = [...this.originalRows];
    } else {
      this.isTableData = false;
      this.errorMessage = null;
    }
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
      this.showLeaveTypeMenu = false;
      this.showStatusDropdown = false;
    }
  }

  exportFile(format: string) {
    const dataToExport = this.rows;
    if (format === 'excel') {
      this.exportToExcel(dataToExport);
      this.dropdownVisible = false;
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
      { header: 'Start Date', dataKey: 'StartDate' },
      { header: 'End Date', dataKey: 'EndDate' },
      { header: 'Period(days)', dataKey: 'Duration' },
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
  clearFilters() {
    this.filterLeaveType = '';
    this.filterApprovedBy = '';
    this.filterStatus = '';
    this.filterDateRange = '';
    this.rows = [...this.originalRows];
    this.page = 1
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
  }
  updateNoOfDays() {
    const fromDate = this.applyLeaveForm.get('date_from')?.value;
    const toDate = this.applyLeaveForm.get('date_to')?.value;
    const duration = this.applyLeaveForm.get('leaveDay')?.value;
    if (fromDate && toDate) {
      const start = new Date(fromDate);
      const end = new Date(toDate);
      if (end >= start) {
        let diffDays = Math.floor((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24)) + 1;
        if (diffDays > 1) {
          this.filteredDurationOptions = ['Full Day'];
          if (duration === 'Half Day') {
            this.applyLeaveForm.patchValue({ leaveDay: 'Full Day' });
          }
        } else {
          this.filteredDurationOptions = ['Full Day', 'Half Day'];
        }
        if (!duration) {
          this.applyLeaveForm.patchValue({ leaveDay: 'Full Day' });
        }
        if (diffDays === 1 && this.applyLeaveForm.get('leaveDay')?.value === 'Half Day') {
          diffDays = 0.5;
        }
        this.applyLeaveForm.patchValue({ noOfDays: diffDays });
        if (this.applyLeaveForm.hasError('dateRange')) {
          this.applyLeaveForm.setErrors(null);
        }
      } else {
        this.applyLeaveForm.patchValue({ noOfDays: '' });
        this.applyLeaveForm.setErrors({ dateRange: true });
        this.filteredDurationOptions = ['Full Day', 'Half Day']; // Reset in case of error
      }
    }
  }
  disableSundays = (date: NgbDate, current?: { year: number; month: number }): boolean => {
    const jsDate = new Date(date.year, date.month - 1, date.day);
    return jsDate.getDay() === 0; // Disable Sundays
  };
  sundayValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value) {
      const day = new Date(value).getDay();
      if (day === 0) {
        return { sundayNotAllowed: true };
      }
    }
    return null;
  }
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }
  onFromDate(): void {
    const fromDate = this.applyLeaveForm.get('date_from')?.value;
    if (fromDate) {
      this.minDate = fromDate;
      this.updateNoOfDays();
    }
  }
  onToDate(): void {
    const toDate = this.applyLeaveForm.get('date_to')?.value;
    if (toDate) {
      this.maxStartDate = toDate;
      this.updateNoOfDays();
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.applyLeaveForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.applyLeaveForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.applyLeaveForm.hasError('dateRange');
  }
  holidayDates = new Map<string, string>();
  employeeGetAllHolidays() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    };
    this.settingService.employeeGetAllHolidays(reqBody).subscribe({
      next: (res: any) => {
        if (Array.isArray(res)) {
          this.parseAndStoreHolidayDates(res);
          this.generateCalendar(this.currentDate);
        }
      },
      error: (err: any) => {
        console.error("Error fetching holidays", err);
      }
    });
  }
  parseAndStoreHolidayDates(holidays: any[]) {
    this.holidayDates.clear();
    holidays.forEach(holiday => {
      if (holiday.Date) {
        const timestamp = Number(holiday.Date.match(/\d+/)[0]);
        const holidayDate = new Date(timestamp);
        // Convert to local date string without timezone conversion
        const isoDate = `${holidayDate.getFullYear()}-${(holidayDate.getMonth() + 1).toString().padStart(2, '0')}-${holidayDate.getDate().toString().padStart(2, '0')}`;
        this.holidayDates.set(isoDate, holiday);
      }
    });
  }
  checkLeaveDatesForHolidays(startDate: string, endDate: string): boolean {
    const start = new Date(startDate);
    const end = new Date(endDate);

    for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
      const dateStr = d.toISOString().split('T')[0];
      if (this.holidayDates.has(dateStr)) {
        return true;  // holiday date found in range
      }
    }
    return false;
  }

  saveAsDraft() {
    // const startDate = this.applyLeaveForm?.get('date_from')?.value;
    // const endDate = this.applyLeaveForm?.get('date_to')?.value;

    // if (!startDate || !endDate) {
    //   this.triggerToast('Please select valid start and end dates.', '', 'warning');
    //   return;
    // }

    // if (this.checkLeaveDatesForHolidays(startDate, endDate)) {
    //   this.triggerToast('You cannot apply leave on a holiday.', '', 'warning');
    //   return;
    // }
    const noOfDays = this.applyLeaveForm?.get('noOfDays')?.value;
    if (noOfDays > 10 && !this.getLeaveDocPath) {
      this.triggerToast('Document is required for leave requests exceeding 10 days.', '', 'warning');
      return;
    }
    if (this.applyLeaveForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.employeeDetails[0].EmpCode,
        StartDate: this.applyLeaveForm?.get('date_from').value,
        EndDate: this.applyLeaveForm?.get('date_to').value,
        Duration: this.applyLeaveForm?.get('noOfDays').value,
        NoOfDays: this.applyLeaveForm?.get('leaveDay').value,
        LeaveTypeId: Number(this.applyLeaveForm?.get('leaveType').value),
        // AppliedDate: this.applyLeaveForm?.get('date').value,
        Reason: this.applyLeaveForm?.get('leaveParticulars').value,
        CompOffDate: this.applyLeaveForm?.get('compOffDate').value ? this.applyLeaveForm?.get('compOffDate').value : '',
        CompOffReason: this.applyLeaveForm?.get('compOffReason').value ? this.applyLeaveForm?.get('compOffReason').value : '',
        DocName: this.getLeaveDocPath ? this.getLeaveDocPath : '',
      }
      this.isSpinner = true;
      this.leaveSerive.DraftLeave(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg']) {
            this.triggerToast(res['msg'], 'Record Saved As Draft Successfully', 'success');
            setTimeout(() => {
              this.closeModal.nativeElement?.click();
              this.getAllLeave();
            }, 100);
            this.isFormSubmitted = false;
            this.resetData();
            this.isSpinner = false;
          }
          else if (res['Message']) {
            this.triggerToast(res['Message'], 'Failed To Saved As Draft', 'warning');
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.triggerToast('Something Went Wrong', 'Failed', 'danger');
          this.isSpinner = false;
        }
      })
    } else {
      this.isFormSubmitted = true;
    }
  }
  applyLeave() {
    const noOfDays = this.applyLeaveForm?.get('noOfDays')?.value;
    if (noOfDays > 10 && !this.getLeaveDocPath) {
      this.triggerToast('Document is required for leave requests exceeding 10 days.', '', 'warning');
      return;
    }
    if (this.applyLeaveForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.employeeDetails[0].EmpCode,
        StartDate: this.applyLeaveForm?.get('date_from').value,
        EndDate: this.applyLeaveForm?.get('date_to').value,
        Duration: this.applyLeaveForm?.get('noOfDays').value,
        NoOfDays: this.applyLeaveForm?.get('leaveDay').value,
        LeaveTypeId: Number(this.applyLeaveForm?.get('leaveType').value),
        Reason: this.applyLeaveForm?.get('leaveParticulars').value,
        CompOffDate: this.applyLeaveForm?.get('compOffDate').value ? this.applyLeaveForm?.get('compOffDate').value : '',
        CompOffReason: this.applyLeaveForm?.get('compOffReason').value ? this.applyLeaveForm?.get('compOffReason').value : '',
        DocName: this.getLeaveDocPath ? this.getLeaveDocPath : '',
        IsLOP: false,
      };
      this.isSpinner = true;
      this.leaveSerive.ApplyLeave(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['Message']) {
            this.isSpinner = false;
            const confirmLOP = window.confirm(res['Message']);
            if (confirmLOP) {
              const lopReqBody = { ...reqBody, IsLOP: true };
              console.log('Resending API with IsLOP:true', lopReqBody);
              this.isSpinner = true;
              this.leaveSerive.ApplyLeave(lopReqBody).subscribe({
                next: (res2: any) => {
                  this.isSpinner = false;
                  console.log('Resend response:', res2);
                  if (res2['msg']) {
                    this.triggerToast(res2['msg'], 'Record Added Successfully', 'success');
                    setTimeout(() => {
                      this.closeModal.nativeElement?.click();
                      this.getAllLeave();
                      this.individualLeaveCount();
                    }, 100);
                    this.resetData();
                    this.isFormSubmitted = false;
                  } else if (res2['Message']) {
                    this.triggerToast(res2['Message'], 'Failed To Add The Leaves', 'warning');
                    this.isFormSubmitted = false;
                  }
                },
                error: (err2: any) => {
                  console.log('Resend API error:', err2);
                  this.isSpinner = false;
                  this.triggerToast('Something Went Wrong', 'Failed To Add The Leaves', 'danger');
                }
              });
            } else {
              this.triggerToast('Leave request cancelled.', '', 'info');
              this.isFormSubmitted = false;
            }
          }
          else if (res['msg']) {
            this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
            setTimeout(() => {
              this.closeModal.nativeElement?.click();
              this.getAllLeave();
              this.individualLeaveCount();
            }, 100);
            this.resetData();
            this.isFormSubmitted = false;
            this.isSpinner = false;
          }
          else if (res['Message']) {
            this.triggerToast(res['Message'], 'Failed To Add The Leaves', 'warning');
            this.isFormSubmitted = false;
            this.isSpinner = false;
          } else {
            this.isSpinner = false;
          }
        },
        error: (err: any) => {
          console.log(err);
          this.triggerToast('Something Went Wrong', 'Failed To Add The Leaves', 'danger');
          this.isSpinner = false;
        }
      });
    } else {
      this.isFormSubmitted = true;
    }
  }


  openModal(): void {
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
  }
  convertToISO(dateStr: string): string {
    const [day, month, year] = dateStr.split('-');
    return `${year}-${month}-${day}`;
  }
  convertDotNetDate(dotNetDate: string): string | null {
    if (!dotNetDate) return null;
    const timestamp = Number(dotNetDate.replace(/\/Date\((\d+)\)\//, '$1'));
    if (isNaN(timestamp)) return null;
    const date = new Date(timestamp);
    return date.toISOString().substring(0, 10);  // e.g. "2025-09-18"
  }
  editData(data: any, edited: boolean) {
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
    this.patchValue = data;
    this.isEdited = edited;
    console.log(data);
    if (this.dropdownLeaveType && this.dropdownLeaveType.length) {
      this.patchFormValues();
    } else {
      // If dropdownLeaveType not ready, wait and retry
      const interval = setInterval(() => {
        if (this.dropdownLeaveType && this.dropdownLeaveType.length) {
          clearInterval(interval);
          this.patchFormValues();
        }
      }, 100);
    }
  }
  patchFormValues() {
    this.applyLeaveForm.patchValue({
      date_from: this.convertToISO(this.patchValue.StartDate),
      date_to: this.convertToISO(this.patchValue.EndDate),
      leaveDay: this.patchValue.Duration,
      leaveType: this.patchValue.LeaveTypeId,
      leaveParticulars: this.patchValue.Reason,
      compOffReason: this.patchValue.CompOffReason,
      compOffDate: this.convertDotNetDate(this.patchValue.CompOffDate),
      attachments: [],
      existingAttachments: [{
        name: this.getFileNameFromPath(this.patchValue.DocName),
        url: this.convertServerPathToUrl(this.patchValue.DocName)
      }]
    });
  }

  deleteRowId: any;
  deleteData(data: any) {
    this.deleteRowId = data;
  }

  deleteLeave() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      LeaveAppId: this.deleteRowId.LeaveAppId
    }
    this.isSpinner = true;
    this.leaveSerive.DeleteDraftLeave(reqBody).subscribe({
      next: (res: any) => {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.isRecordDeleted = true;
        setTimeout(() => {
          this.deleteModal.nativeElement?.click();
          this.getAllLeave();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
        this.isSpinner = false;
      }, error: (err: any) => {
        console.log(err);
        this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
        this.isSpinner = false;
      }
    })
  }
  updateDraftSave() {
    const noOfDays = this.applyLeaveForm?.get('noOfDays')?.value;
    if (noOfDays > 10 && !this.getLeaveDocPath) {
      this.triggerToast('Document is required for leave requests exceeding 10 days.', '', 'warning');
      return;
    }
    if (this.applyLeaveForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.employeeDetails[0].EmpCode,
        StartDate: this.applyLeaveForm?.get('date_from').value,
        EndDate: this.applyLeaveForm?.get('date_to').value,
        Duration: this.applyLeaveForm?.get('noOfDays').value,
        NoOfDays: this.applyLeaveForm?.get('leaveDay').value,
        LeaveTypeId: Number(this.applyLeaveForm?.get('leaveType').value),
        Reason: this.applyLeaveForm?.get('leaveParticulars').value,
        CompOffDate: this.applyLeaveForm?.get('compOffDate').value || '',
        CompOffReason: this.applyLeaveForm?.get('compOffReason').value || '',
        DocName: this.getLeaveDocPath || '',
        LeaveAppId: this.patchValue.LeaveAppId,
        IsLOP: false,
      };
      this.isSpinner = true;
      this.leaveSerive.DraftApplyLeave(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          // Handle LOP confirmation
          if (res['Message']) {
            this.isSpinner = false;
            const confirmLOP = window.confirm(res['Message']);
            if (confirmLOP) {
              const lopReqBody = { ...reqBody, IsLOP: true };
              this.isSpinner = true;
              this.leaveSerive.DraftApplyLeave(lopReqBody).subscribe({
                next: (res2: any) => {
                  this.isSpinner = false;
                  console.log('Resend response:', res2);
                  if (res2['msg']) {
                    this.triggerToast(res2['msg'], 'Record Added Successfully', 'success');
                    setTimeout(() => {
                      this.closeModal.nativeElement?.click();
                      this.getAllLeave();
                      this.individualLeaveCount();
                    }, 100);
                    this.resetData();
                    this.isFormSubmitted = false;
                  } else if (res2['Message']) {
                    this.triggerToast(res2['Message'], 'Failed To Add The Leaves', 'warning');
                    this.isFormSubmitted = false;
                  }
                },
                error: (err2: any) => {
                  console.log('Resend API error:', err2);
                  this.isSpinner = false;
                  this.triggerToast('Something Went Wrong', 'Failed To Add The Leaves', 'danger');
                }
              });
            } else {
              this.triggerToast('Leave request cancelled.', '', 'info');
              this.isFormSubmitted = false;
            }
          }
          // Successful message without LOP
          else if (res['msg']) {
            this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
            setTimeout(() => {
              this.closeModal.nativeElement?.click();
              this.getAllLeave();
              this.individualLeaveCount();
            }, 100);
            this.resetData();
            this.isFormSubmitted = false;
            this.isSpinner = false;
          }

          // Failed message
          else if (res['Message']) {
            this.triggerToast(res['Message'], 'Failed To Add The Leaves', 'warning');
            this.isFormSubmitted = false;
            this.isSpinner = false;
          } else {
            this.isSpinner = false;
          }
        },
        error: (err: any) => {
          console.log(err);
          this.triggerToast('Something Went Wrong', 'Failed To Add The Leaves', 'danger');
          this.isSpinner = false;
        }
      });
    } else {
      this.isFormSubmitted = true;
    }
  }

  getFileNameFromPath(fullPath: string): string {
    if (!fullPath) return '';
    const parts = fullPath.split(/[\\/]/); // split by \ or /
    return parts[parts.length - 1];
  }
  convertServerPathToUrl(path: string): string {
    if (!path) return '';
    // Adjust base URL accordingly:
    const baseUrl = `${environment.baseUrl}`;
    const fileName = this.getFileNameFromPath(path);
    return baseUrl + fileName;
  }
  removeExistingFile(fileToRemove: { name: string; url: string }) {
    const existingFiles = this.applyLeaveForm.get('existingAttachments')?.value || [];
    const updatedFiles = existingFiles.filter((file: any) => file.url !== fileToRemove.url);
    this.applyLeaveForm.get('existingAttachments')?.setValue(updatedFiles);
    // If no more files, reset doc path
    if (updatedFiles.length === 0 && this.selectedFiles.length === 0) {
      this.getLeaveDocPath = '';
    }
  }
  onViewCancel(data: any) {
    console.log(data);
    this.viewdata = data
  }
  cancleRecord() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      LeaveAppId: this.viewdata.LeaveAppId
    }
    this.isSpinner = true;
    this.leaveSerive.CancelLeave(reqBody).subscribe({
      next: (res: any) => {
        this.triggerToast(res['msg'], 'Record Cancelled Successfully', 'success');
        this.isRecordDeleted = true;
        setTimeout(() => {
          this.closeModalDelete.nativeElement?.click();
          this.getAllLeave();
          this.individualLeaveCount();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
        this.isSpinner = false;
      }
    })
  }

  viewDataWithdrawn: any;
  onWithdraw(data: any) {
    this.viewDataWithdrawn = data;
  }

  withdrawYes() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      LeaveAppId: this.viewDataWithdrawn.LeaveAppId
    }
    this.isSpinner = true;
    this.leaveSerive.WithDrawLeave(reqBody).subscribe({
      next: (res: any) => {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.isRecordDeleted = true;
        setTimeout(() => {
          this.closeModalwithdraw.nativeElement?.click();
          this.getAllLeave();
          this.individualLeaveCount();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
        this.isSpinner = false;
      }
    })
  }
  dropdownApproverList: any = [];
  compOffRequestData: any = [];
  callApiList() {
    this.getApproverListCompOff();
    setTimeout(() => {
      this.getAllCompOffRequest();
    }, 100);
  }
  getApproverListCompOff() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.leaveSerive.DDApproveManager(reqBody).subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.dropdownApproverList = res;
        } else {
          this.triggerToast('No Data Found', 'For Approver List', '');
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
        this.isSpinner = false;
      }
    })
  }

  getAllCompOffRequest() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.leaveSerive.GetAllEmpCompOffLeave(reqBody).subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.compOffRequestData = res;
          this.isTableDataCompOff = false;
          this.isSpinner = false;
        } else {
          this.errorMessageCompOff = 'No records found';
          this.isSpinner = false;
          this.isTableDataCompOff = true;
        }
      }, error: (err: any) => {
        this.errorMessageCompOff = 'Internal Server Error';
        this.isSpinner = false;
        this.isTableDataCompOff = true;
      }
    })
  }

  submitCompOffForm() {
    if (this.compOffForm.valid) {
      this.isFormSubmitted = false;
      const selectedManager = this.compOffForm?.get('managerList')?.value;

      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.employeeDetails[0].EmpCode,
        ManagerId: selectedManager?.ManagerId,
        ManagerCode: selectedManager?.ManagerCode,
        Date: this.compOffForm?.get('date')?.value,
        ProjectId: 0,
        Project: this.compOffForm?.get('projectName')?.value,
        TaskId: 0,
        Task: this.compOffForm?.get('task')?.value,
        Hrs: this.compOffForm?.get('hours')?.value,
        ActualHrs: this.getCompOffAccutalHour?.ActualHrs ? this.getCompOffAccutalHour?.ActualHrs : 0,
        WorkMode: this.getCompOffAccutalHour?.WorkMode ? this.getCompOffAccutalHour?.WorkMode : "-",
      };
      this.leaveSerive.CompOffLeave(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], '');
            this.compOffModalClose.nativeElement?.click();
          } else if (res['Message']) {
            this.triggerToast(res['Message'], res['Message'], '');
          }
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
        }
      })
    } else {
      this.isFormSubmitted = true;
    }
  }
  resetCompOffForm() {
    this.compOffForm.reset();
    this.isFormSubmitted = false;
    this.getCompOffAccutalHour = ''
  }
  resetData() {
    this.applyLeaveForm.reset();
    this.isEdited = false;
    this.isFormSubmitted = false;
    this.minDate = undefined;
    this.maxDate = undefined;
    this.maxStartDate = '';
    // Clear selected files and revoke preview URLs to free memory
    this.selectedFiles.forEach((file, index) => {
      URL.revokeObjectURL(this.filePreviewUrls[index]);
    });
    this.selectedFiles = [];
    this.filePreviewUrls = [];
    // Reset date as before
    const today = new Date();
    const applyDate = today.toISOString().split('T')[0];
    this.applyLeaveForm.patchValue({ date: applyDate });
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
  allowOnlyNumberAndSymbols(event: KeyboardEvent) {
    const char = String.fromCharCode(event.keyCode || event.which);
    const allowedPattern = /^[0-9:,.]$/;
    if (!allowedPattern.test(char)) {
      event.preventDefault();
    }
  }

}
