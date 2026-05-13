import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { AttendenceModuleService } from '../../service/attendence.service';
import { Router, RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-self-attendance',
  standalone: true,
  imports: [FormsModule, CommonModule, SharedModule, NgxPaginationModule, ToastMessageComponent,
    RouterModule],
  templateUrl: './self-attendance.component.html',
  styleUrl: './self-attendance.component.scss'
})
export class SelfAttendanceComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  employeeDetails;
  isSpinner: boolean = false;
  attendanceData: any[] = []
  isTableData: boolean = false;
  errorMessage: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  selectedEndMonth: string;
  months: string[] = [];
  selectedMonthEndDate: string = '';
  selectedMonthStartDate: string = '';
  years: number[] = [];
  currentYear;
  isdefalutMonth = true;
  allMonths: string[] = [];
  selectedYear: number = new Date().getFullYear();
  accessPolicy: any[] = [];
  controlAccessPage: any;
  tabs: any[] = [];
  dropdownVisible = false;


  allTabs = [
    { id: 'employee_attendance', title: 'Employee Attendance', type: 'item', url: '/employee_attendance', icon: 'feather icon-user-check' },
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }

  constructor(private readonly hrmsEmpAttendance: AttendenceModuleService,
    private router: Router, private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
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

    const currentYear = new Date().getFullYear();
    this.currentYear = currentYear
    for (let year = 2020; year <= currentYear; year++) {
      this.years.push(year);
    }
  }

  ngOnInit(): void {
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Self Attendance'
      );
      // Move tab filtering here
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((policy: any) =>
          policy.PageName === tab.title && policy.ViewAccess
        )
      );
    });
    this.getEachEmployeeAttendance();
  }

  yesterdayAttendance: any

  // getEachEmployeeAttendance() {
  //   const formatDate = (date: Date): string => {
  //     const year = date.getFullYear();
  //     const month = (date.getMonth() + 1).toString().padStart(2, '0');
  //     const day = date.getDate().toString().padStart(2, '0');
  //     return `${year}-${month}-${day}`;
  //   };

  //   const generateFullMonthAttendance = (startDate: string, endDate: string, apiData: any[]) => {
  //     const start = new Date(startDate);
  //     const end = new Date(endDate);
  //     const fullList: any[] = [];

  //     const formatDateLocal = (date: Date): string => {
  //       const year = date.getFullYear();
  //       const month = (date.getMonth() + 1).toString().padStart(2, '0');
  //       const day = date.getDate().toString().padStart(2, '0');
  //       return `${year}-${month}-${day}`;
  //     };

  //     while (start <= end) {
  //       const dateStr = formatDateLocal(start);
  //       const existing = apiData.find(d => d.AttendaceDate === dateStr);

  //       if (existing) {
  //         fullList.push(existing);
  //       } else {
  //         fullList.push({
  //           AttendaceDate: dateStr,
  //           lstofAttendance: [
  //             {
  //               LogInTime: "",
  //               LogOutTime: "",
  //               BreakTime: "",
  //               WorkingHours: "",
  //               WorkType: "",
  //             }
  //           ]
  //         });
  //       }

  //       start.setDate(start.getDate() + 1);
  //     }

  //     return fullList;
  //   };

  //   const currentDate = new Date();
  //   const startDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);

  //   const yesterday = new Date(currentDate);
  //   yesterday.setDate(currentDate.getDate() - 1);
  //   const formattedYesterday = formatDate(yesterday);

  //   const formattedStartDate = formatDate(startDate);
  //   const formattedEndDate = formatDate(currentDate);

  //   const reqBody = {
  //     LoginId: this.employeeDetails[0].LoginId,
  //     StartDate: formattedStartDate,
  //     EndDate: formattedEndDate
  //   };

  //   this.isSpinner = true;
  //   this.hrmsEmpAttendance.EmployeeEachEmployeeAttendance(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res.length >= 1) {
  //         this.attendanceData = generateFullMonthAttendance(
  //           formattedStartDate,
  //           formattedEndDate,
  //           res
  //         );
  //         const yesterdayAttendance = this.attendanceData.filter(
  //           (attendance: any) => attendance.AttendaceDate === formattedYesterday
  //         );

  //         if (yesterdayAttendance.length > 0) {
  //           this.yesterdayAttendance = yesterdayAttendance[0].lstofAttendance[0];
  //         }
  //         this.isTableData = false;
  //         this.page = 1;
  //       } else {
  //         this.isTableData = true;
  //         this.errorMessage = "No Data Found";
  //       }
  //       this.isSpinner = false;
  //     },
  //     error: (error: any) => {
  //       this.errorMessage = "Internal Server Error";
  //       this.isTableData = true;
  //       this.isSpinner = false;
  //     }
  //   });
  // }

  getEachEmployeeAttendance() {
    const formatDate = (date: Date): string => {
      const year = date.getFullYear();
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    };

    const generateFullMonthAttendance = (startDate: string, endDate: string, apiData: any[]) => {
      const start = new Date(startDate);
      const end = new Date(endDate);
      const fullList: any[] = [];

      const formatDateLocal = (date: Date): string => {
        const year = date.getFullYear();
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        return `${year}-${month}-${day}`;
      };

      while (start <= end) {
        const dateStr = formatDateLocal(start);
        const existing = apiData.find(d => d.AttendaceDate === dateStr);

        if (existing) {
          fullList.push(existing);
        } else {
          fullList.push({
            AttendaceDate: dateStr,
            lstofAttendance: [
              {
                LogInTime: "",
                LogOutTime: "",
                BreakTime: "",
                WorkingHours: "",
                WorkType: "",
              }
            ]
          });
        }

        start.setDate(start.getDate() + 1);
      }

      return fullList;
    };
    const currentDate = new Date();

    const startDate = new Date(
      currentDate.getFullYear(),
      currentDate.getMonth(),
      1
    );

    const formattedStartDate = formatDate(startDate);

    // 🔹 Get yesterday (with weekend handling)
    let yesterdayDate = new Date(currentDate);
    const day = currentDate.getDay();

    if (day === 0) {
      // Sunday → Friday
      yesterdayDate.setDate(currentDate.getDate() - 2);
    } else if (day === 1) {
      // Monday → Friday
      yesterdayDate.setDate(currentDate.getDate() - 3);
    } else {
      // Normal → yesterday
      yesterdayDate.setDate(currentDate.getDate() - 1);
    }

    const formattedYesterday = formatDate(yesterdayDate);

    // ✅ Use yesterday here
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      StartDate: formattedStartDate,
      EndDate: formattedYesterday
    };

    this.isSpinner = true;
    this.hrmsEmpAttendance.EmployeeEachEmployeeAttendance(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          // Generate the full month attendance
          this.attendanceData = generateFullMonthAttendance(
            formattedStartDate,
            formattedYesterday,
            res
          );

          this.attendanceData.sort((a, b) => {
            return new Date(b.AttendaceDate).getTime() - new Date(a.AttendaceDate).getTime();
          });
          // Filter out yesterday's attendance (based on our adjusted "yesterday" logic)
          const yesterdayAttendance = this.attendanceData.filter(
            (attendance: any) => attendance.AttendaceDate === formattedYesterday
          );

          if (yesterdayAttendance.length > 0) {
            this.yesterdayAttendance = yesterdayAttendance[0].lstofAttendance[0];
            // Convert the LogDate from '/Date(...)' to JavaScript Date object
            if (this.yesterdayAttendance.LogDate) {
              const timestamp = parseInt(this.yesterdayAttendance.LogDate.replace("/Date(", "").replace(")/", ""), 10);
              this.yesterdayAttendance.LogDate = new Date(timestamp);
            }
            console.log(this.yesterdayAttendance); // Log the result to check
          }

          this.isTableData = false;
          this.page = 1;
        } else {
          this.isTableData = true;
          this.errorMessage = "No Data Found";
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessage = "Internal Server Error";
        this.isTableData = true;
        this.isSpinner = false;
      }
    });
  }

  tooltipContent: any;
  hoveredCellId: string | null = null;

  isValidTime(time: string): boolean {
    if (!time) return false;
    return time !== '00:00:00';
  }

  setDayTooltip(attendance: any, date: string) {
    if (!attendance) return;

    let tooltip = `
    <i class="fas fa-user"></i> Name &nbsp;:&nbsp; <strong>${attendance.EmpName}</strong><br>
    <i class="fas fa-briefcase"></i> Work Mode &nbsp;:&nbsp; <strong>${attendance.WorkType || '-'}</strong><br>
    <i class="fas fa-sun"></i> Shift &nbsp;:&nbsp; <strong>${attendance.ShiftName || '-'}</strong><br>
    <i class="fas fa-clock"></i> Active Hours &nbsp;:&nbsp; <strong>${attendance.ActiveHours}</strong><br>
  `;

    /* ================= ESSL ================= */
    if (
      this.isValidTime(attendance.ESSLLogInTime) ||
      this.isValidTime(attendance.ESSLLogOutTime) ||
      this.isValidTime(attendance.ESSLActiveHours)
    ) {
      tooltip += `
      <strong class="essl-title">ESSL</strong><br>
      ${this.isValidTime(attendance.ESSLLogInTime) ? `Login &nbsp;:&nbsp; <strong>${attendance.ESSLLogInTime}</strong><br>` : ''}
      ${this.isValidTime(attendance.ESSLLogOutTime) ? `Logout &nbsp;:&nbsp; <strong>${attendance.ESSLLogOutTime}</strong><br>` : ''}
      ${this.isValidTime(attendance.ESSLActiveHours) ? `Active &nbsp;:&nbsp; <strong>${attendance.ESSLActiveHours}</strong><br>` : ''}
    `;
    }

    /* ================= WFH ================= */
    if (
      this.isValidTime(attendance.WFHLogInTime) ||
      this.isValidTime(attendance.WFHLogOutTime) ||
      this.isValidTime(attendance.WFHActiveHours)
    ) {
      tooltip += `
      <strong class="wfh-title">WFH</strong><br>
      ${this.isValidTime(attendance.WFHLogInTime) ? `Login &nbsp;:&nbsp; <strong>${attendance.WFHLogInTime}</strong><br>` : ''}
      ${this.isValidTime(attendance.WFHLogOutTime) ? `Logout &nbsp;:&nbsp; <strong>${attendance.WFHLogOutTime}</strong><br>` : ''}
      ${this.isValidTime(attendance.WFHActiveHours) ? `Active &nbsp;:&nbsp; <strong>${attendance.WFHActiveHours}</strong><br>` : ''}
    `;
    }

    /* ================= ONSITE ================= */
    if (
      this.isValidTime(attendance.ONSITELogInTime) ||
      this.isValidTime(attendance.ONSITELogOutTime) ||
      this.isValidTime(attendance.ONSITEActiveHours)
    ) {
      tooltip += `
      <strong class="onsite-title">Onsite</strong><br>
      ${this.isValidTime(attendance.ONSITELogInTime) ? `Login &nbsp;:&nbsp; <strong>${attendance.ONSITELogInTime}</strong><br>` : ''}
      ${this.isValidTime(attendance.ONSITELogOutTime) ? `Logout &nbsp;:&nbsp; <strong>${attendance.ONSITELogOutTime}</strong><br>` : ''}
      ${this.isValidTime(attendance.ONSITEActiveHours) ? `Active &nbsp;:&nbsp; <strong>${attendance.ONSITEActiveHours}</strong><br>` : ''}
    `;
    }

    this.tooltipContent = tooltip;
    this.hoveredCellId = date;
  }


  clearTooltipContent() {
    this.tooltipContent = '';
    this.hoveredCellId = null;
  }


  getSelectedYear(event: any) {
    const selectedYear = parseInt(event.target.value, 10);
    if (selectedYear === this.currentYear) {
      this.isdefalutMonth = true;
    } else {
      this.isdefalutMonth = false;
      this.allMonths = [
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
    }
    this.selectedYear = selectedYear;
  }
  getSelectedMonth(event: any) {
    const formatDate = (date: Date): string => {
      const year = date.getFullYear();
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    };
    const selectedMonth = event.target.value.trim();
    if (!selectedMonth) {
      console.error('No month selected');
      return;
    }
    const monthIndex = new Date(`${selectedMonth} 1, ${this.selectedYear}`).getMonth(); // Use the selectedYear here
    if (isNaN(monthIndex)) {
      console.error('Invalid month selection:', selectedMonth);
      return;
    }
    const startDate = new Date(this.selectedYear, monthIndex, 1);
    const endDate = new Date(this.selectedYear, monthIndex + 1, 0);
    this.selectedMonthStartDate = formatDate(startDate);
    this.selectedMonthEndDate = formatDate(endDate);
    this.isSpinner = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      StartDate: this.selectedMonthStartDate,
      EndDate: this.selectedMonthEndDate
    };
    console.log(reqBody);
    this.hrmsEmpAttendance.EmployeeEachEmployeeAttendance(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.attendanceData = res;
          this.isTableData = false;
          this.page = 1;
        } else {
          this.isTableData = true;
          this.errorMessage = "No Data Found";
        }
        this.isSpinner = false;
      }, error: (error: any) => {
        this.errorMessage = "Internal Server Error";
        this.isTableData = true;
        this.isSpinner = false;
      }
    })
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
  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const isDropdown = target.closest('.dropdown-content') !== null;
    const isButton = target.matches('.export-button');
    if (!isDropdown && !isButton) {
      this.dropdownVisible = false;
    }
  }

  exportToExcel(): void {
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
    const filteredData = this.attendanceData.map((item: any) => {
      const details = item.lstofAttendance?.[0] || {};
      return {
        "Attendace Date": item.AttendaceDate,
        "Name": details.EmpName,
        "Code": details.EmpCode,
        "Shift": details.ShiftName,
        "Holiday": details.HolidayName,
        "LogIn Time": details.LogInTime,
        "LogOut Time": details.LogOutTime,
        "Active Hours": details.ActiveHours,
        "Working Hours": details.WorkingHours,
      };
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(filteredData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Self Attendance': worksheet },
      SheetNames: ['Self Attendance']
    };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'SelfAttendance.xlsx');
    this.dropdownVisible = false;
  }


  exportToPDF(): void {
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
    const filteredData = this.attendanceData.map((item: any) => {
      const details = item.lstofAttendance?.[0] || {};
      return [
        item.AttendaceDate,
        details.EmpName,
        details.EmpCode,
        details.ShiftName,
        details.LogInTime,
        details.LogOutTime,
        details.WorkingHours,
        details.ActiveHours,
        details.WorkType,
        details.HolidayName ?? "",
      ];
    });
    const headers = [
      'Attendance Date',
      'Name',
      'Code',
      'Shift',
      'LogIn Time',
      'LogOut Time',
      'Working Hours',
      'Active Hours',
      'Work Type',
      'Holiday',
    ];
    // PDF (Landscape, A3)
    const doc = new jsPDF({
      orientation: 'landscape',
      unit: 'pt',
      format: 'a3',
    });
    autoTable(doc, {
      head: [headers],
      body: filteredData,
      styles: {
        fontSize: 7,
        cellPadding: 3,
        overflow: 'linebreak',
        halign: 'left',
      },
      headStyles: {
        fillColor: [7, 47, 95],
        textColor: 255,
        fontStyle: 'bold',
        fontSize: 7,
      },
      alternateRowStyles: { fillColor: [245, 245, 245] },

      didDrawPage: (data) => {
        doc.setFontSize(14);
        doc.text('Self Attendance Report', data.settings.margin.left, 25);
      },
    });

    doc.save('SelfAttendance.pdf');
    this.dropdownVisible = false;
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
