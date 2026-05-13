import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { AttendenceModuleService } from '../../service/attendence.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule } from '@angular/router';
import { ChartComponent } from "ng-apexcharts";
import { NgApexchartsModule } from "ng-apexcharts";
import {
  ApexChart,
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexPlotOptions,
  ApexDataLabels,
  ApexLegend,  // Import ApexLegend for the legend property
} from "ng-apexcharts";
import { SafeHtml } from '@angular/platform-browser';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: string[];
  dataLabels?: ApexDataLabels;
  plotOptions?: ApexPlotOptions;
  legend?: ApexLegend;
};

interface AttendanceRecord {
  EmpId: number;
  EmpCode: string;
  EmpName: string;
  WorkingHours: number;
  LogInTime: string;
  LogOutTime: string;
  IsWorkFromHome: boolean;
  LogDate: string;
  WorkType: string;
  DaysPresent: number;
  LeaveType: string;
}

@Component({
  selector: 'app-reporting-emp-attendance',
  standalone: true,
  imports: [FormsModule, CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule, RouterModule, NgApexchartsModule],
  templateUrl: './reporting-emp-attendance.component.html',
  styleUrls: ['./reporting-emp-attendance.component.scss']
})
export class ReportingEmpAttendanceComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild("chart") chart!: ChartComponent;
  chartOptions!: Partial<ChartOptions>;
  tooltipContent!: SafeHtml;

  employeeDetails;
  attendanceArray: { [date: string]: AttendanceRecord[] } = {};
  employees: any[] = [];
  dates: string[] = [];
  errorMessage: any;
  isTableData: boolean = false;
  isSpinner: boolean = false;
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
  hoveredCellId: string | null = null;
  selectedYear: number = new Date().getFullYear();
  accessPolicy: any;
  controlAccessPage: any;
  dropdownVisible = false;
  searchValue: string = '';
  filteredEmployees: { EmpId: number; EmpCode: string; EmpName: string }[] = [];

  constructor(private readonly hrmsEmpAttendance: AttendenceModuleService,
    private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Employee Attendance'
      );
    });

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
    this.getReportingEmployeeAttendance();
    this.showPieChart();
  }
  getWorkTypeColor(workType: string, daysPresent: number, leaveType: string): string {
    if (workType === 'Onsite') 'yellow';
    if (workType === 'WFH') 'orange';
    if (leaveType === 'LOP') 'red';
    if (leaveType === 'EL') '#A3C6FF';
    if (leaveType === 'CL') '#B172DB';
    if (daysPresent === 1) 'rgb(201 233 119)'
    return 'transparent';
  }

  getReportingEmployeeAttendance() {
    const formatDate = (date: Date): string => {
      const year = date.getFullYear();
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    };

    // ⭐ NEW — generate all dates from start to end
    const generateFullDateList = (startDate: string, endDate: string) => {
      const datesList: string[] = [];
      const start = new Date(startDate);
      const end = new Date(endDate);
      while (start <= end) {
        datesList.push(formatDate(start));
        start.setDate(start.getDate() + 1);
      }
      return datesList;
    };
    const firstDayOfCurrentMonth = new Date();
    firstDayOfCurrentMonth.setDate(1);
    const yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);

    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      StartDate: formatDate(firstDayOfCurrentMonth),
      EndDate: formatDate(yesterday)
    };
    this.isSpinner = true;
    this.hrmsEmpAttendance.EmployeeReportingEmployeeAttendance(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.attendanceArray = {};
          this.employees = [];
          // ⭐ Replace pushing only API-dates with complete month dates
          this.dates = generateFullDateList(
            formatDate(firstDayOfCurrentMonth),
            formatDate(yesterday)
          );

          res.forEach((attendanceRecord: any) => {
            const attendanceDate = attendanceRecord.AttendaceDate;

            if (attendanceRecord.lstofAttendance.length >= 1) {
              attendanceRecord.lstofAttendance.forEach((attendance: AttendanceRecord) => {

                if (!this.employees.some(emp => emp.EmpId === attendance.EmpId)) {
                  this.employees.push({
                    EmpId: attendance.EmpId,
                    EmpCode: attendance.EmpCode,
                    EmpName: attendance.EmpName
                  });
                }

                if (!this.attendanceArray[attendanceDate]) {
                  this.attendanceArray[attendanceDate] = [];
                }

                this.attendanceArray[attendanceDate].push(attendance);
              });
            }
          });
          this.filteredEmployees = [...this.employees];
          this.isTableData = false;
          this.isSpinner = false;
        } else {
          this.isSpinner = false;
          this.isTableData = true;
          this.errorMessage = "No Data Found";
        }
      },
      error: (error: any) => {
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
        this.isSpinner = false;
      }
    });
  }

  applyFilter() {
    const val = this.searchValue.toLowerCase().trim();

    if (!val) {
      this.filteredEmployees = [...this.employees];
      this.isTableData = false;
      this.errorMessage = '';
      return;
    }

    this.filteredEmployees = this.employees.filter(emp => {
      // Match employee name or code
      const matchesEmployee =
        emp.EmpName?.toLowerCase().includes(val) ||
        emp.EmpCode?.toLowerCase().includes(val);
      const matchesAttendance = this.dates.some(date =>
        this.attendanceArray[date]?.some(record =>
          record.EmpId === emp.EmpId && (
            record.WorkingHours?.toString().includes(val) ||
            record.LogInTime?.toLowerCase().includes(val) ||
            record.LogOutTime?.toLowerCase().includes(val) ||
            record.WorkType?.toLowerCase().includes(val)
          )
        )
      );

      return matchesEmployee || matchesAttendance;
    });

    if (this.filteredEmployees.length === 0) {
      this.isTableData = true;
      this.errorMessage = `No record found for "${this.searchValue}"`;
    } else {
      this.isTableData = false;
      this.errorMessage = '';
    }

    this.page = 1; // Reset pagination to first page
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
  private flattenAttendance() {
    const result: any[] = [];
    Object.keys(this.attendanceArray).forEach(date => {
      const employees = this.attendanceArray[date];

      employees.forEach((emp: any) => {
        result.push({
          AttendanceDate: date,
          ...emp
        });
      });
    });
    return result;
  }


  // exportToExcel(): void {
  //   const flatData = this.flattenAttendance();
  //   if (flatData.length === 0) {
  //     this.triggerToast('Sorry', 'No data to export!', 'info');
  //     return;
  //   }
  //   const exportRows = flatData.map(d => ({
  //     "Attendance Date": d.AttendanceDate,
  //     "Emp Code": d.EmpCode,
  //     "Employee Name": d.EmpName,
  //     "Department": d.DeptName,
  //     "Designation": d.Designation,
  //     "Shift": d.ShiftName,
  //     "LogIn Time": d.LogInTime,
  //     "LogOut Time": d.LogOutTime,
  //     "Working Hours": d.WorkingHours,
  //     "Active Hours": d.ActiveHours,
  //     "Break Time": d.BreakTime,
  //     "Work Type": d.WorkType,
  //     "Holiday": d.HolidayName ?? ""
  //   }));
  //   const worksheet = XLSX.utils.json_to_sheet(exportRows);
  //   const workbook: XLSX.WorkBook = {
  //     Sheets: { 'Reporting Attendance': worksheet },
  //     SheetNames: ['Reporting Attendance']
  //   };
  //   const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
  //   const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
  //   FileSaver.saveAs(blobData, 'Reporting_Attendance.xlsx');
  //   this.dropdownVisible = false;
  // }

  private flattenAttendanceFromApi(
    attendanceObj: { [date: string]: AttendanceRecord[] }
  ): any[] {

    const result: any[] = [];

    Object.keys(attendanceObj).forEach(date => {
      const employees = attendanceObj[date];

      employees.forEach(emp => {
        result.push({
          AttendanceDate: date,
          ...emp
        });
      });
    });

    return result;
  }


  private groupByCompany(data: any[]) {
    return data.reduce((groups, item) => {
      const comp = item.CompName || 'Unknown';

      if (!groups[comp]) {
        groups[comp] = [];
      }

      groups[comp].push(item);
      return groups;
    }, {} as { [key: string]: any[] });
  }


  exportToExcel(): void {
    const flatData = this.flattenAttendanceFromApi(this.attendanceArray);

    if (flatData.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }

    const groupedByCompany = this.groupByCompany(flatData);

    // 🔥 ONE workbook
    const workbook: XLSX.WorkBook = {
      Sheets: {},
      SheetNames: []
    };

    // 🔁 Create one sheet per company
    Object.keys(groupedByCompany).forEach(companyName => {

      const exportRows = groupedByCompany[companyName].map((d: any) => ({
        "Attendance Date": d.AttendanceDate,
        "Emp Code": d.EmpCode,
        "Employee Name": d.EmpName,
        "Department": d.DeptName,
        "Designation": d.Designation,
        "Shift": d.ShiftName,
        "LogIn Time": d.LogInTime,
        "LogOut Time": d.LogOutTime,
        "Working Hours": d.WorkingHours,
        "Active Hours": d.ActiveHours,
        "Break Time": d.BreakTime,
        "Work Type": d.WorkType,
        "Holiday": d.HolidayName ?? ""
      }));

      const worksheet = XLSX.utils.json_to_sheet(exportRows);

      // Excel sheet name max length = 31 chars
      const safeSheetName = companyName.substring(0, 31);

      workbook.Sheets[safeSheetName] = worksheet;
      workbook.SheetNames.push(safeSheetName);
    });

    // ✅ Save only ONCE
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });

    FileSaver.saveAs(blobData, 'Reporting_Attendance.xlsx');

    this.dropdownVisible = false;
  }




  exportToPDF(): void {
    const flatData = this.flattenAttendance();
    if (flatData.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
    const tableData = flatData.map(d => [
      d.AttendanceDate,
      d.EmpCode,
      d.EmpName,
      d.DeptName,
      d.Designation,
      d.ShiftName,
      d.LogInTime,
      d.LogOutTime,
      d.WorkingHours,
      d.ActiveHours,
      d.BreakTime,
      d.WorkType,
      d.HolidayName ?? ""
    ]);
    const headers = [
      "Attendance Date",
      "Emp Code",
      "Employee Name",
      "Department",
      "Designation",
      "Shift",
      "LogIn Time",
      "LogOut Time",
      "Working Hours",
      "Active Hours",
      "Break Time",
      "Work Type",
      "Holiday"
    ];
    const doc = new jsPDF({
      orientation: 'landscape',
      unit: 'pt',
      format: 'a3'
    });
    autoTable(doc, {
      head: [headers],
      body: tableData,
      styles: { fontSize: 7 },
      headStyles: { fillColor: [7, 47, 95], textColor: 255 }
    });
    doc.save("Reporting_Attendance.pdf");
    this.dropdownVisible = false;
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

    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      StartDate: this.selectedMonthStartDate,
      EndDate: this.selectedMonthEndDate
    };
    console.log(reqBody);

    this.isSpinner = true;
    this.hrmsEmpAttendance.EmployeeReportingEmployeeAttendance(reqBody).subscribe({
      next: (res: any) => {
        this.attendanceArray = {};
        this.employees = [];
        this.dates = [];
        res.forEach((attendanceRecord: any) => {
          const attendanceDate = attendanceRecord.AttendaceDate;
          this.dates.push(attendanceDate);
          if (attendanceRecord.lstofAttendance.length >= 1) {
            attendanceRecord.lstofAttendance.forEach((attendance: AttendanceRecord) => {
              if (!this.employees.some(emp => emp.EmpId === attendance.EmpId)) {
                this.employees.push({
                  EmpId: attendance.EmpId,
                  EmpCode: attendance.EmpCode,
                  EmpName: attendance.EmpName
                });
              }
              if (!this.attendanceArray[attendanceDate]) {
                this.attendanceArray[attendanceDate] = [];
              }
              this.attendanceArray[attendanceDate].push(attendance);
            });
          }
        });
        this.filteredEmployees = [...this.employees];
        this.isTableData = false;
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
        this.isSpinner = false;
      }
    });
  }


  formatTime(time: string): string {
    if (time) {
      return time.slice(0, 5);
    }
    return '00:00';
  }


  setTooltipContent(employee: any, record: any, date: string) {
    // console.log(record);
    this.hoveredCellId = `${employee.EmpId}-${date}`;
    const formattedLogInTime = this.formatTime(record.LogInTime);
    const formattedLogOutTime = this.formatTime(record.LogOutTime);
    const formattedWorkedHours = this.formatTime(record.WorkingHours);
    const formattedBreakTime = this.formatTime(record.BreakTime);
    const formattedOverTime = this.formatTime(record.OverTime);
    this.tooltipContent = `
      <strong>Name:</strong> ${employee.EmpName}<br>
      <strong>Login Time:</strong> ${formattedLogInTime}<br>
      <strong>Logout Time:</strong> ${formattedLogOutTime || 'N/A'}<br>
      <strong>Worked Hours:</strong> ${formattedWorkedHours}<br>
      <strong>Work Mode:</strong> ${record.WorkType}<br>
      <strong>Shift Name:</strong> ${record.ShiftName}<br>
      <strong>Break Time:</strong> ${formattedBreakTime}<br>
      <strong>Over Time:</strong> ${formattedOverTime}<br>
    `;
  }

  showPieChart() {
    this.chartOptions = {
      series: [44, 55, 13, 43, 22],
      chart: {
        type: "donut",
        width: 300,
        height: 300,
      },
      labels: ["Team A", "Team B", "Team C", "Team D", "Team E"],
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: { width: 1000 },
            legend: { position: "bottom" }
          }
        }
      ],
      dataLabels: {
        enabled: true,
        formatter: (val: any) => `${val}%`,
        style: {
          fontSize: "14px",
          fontWeight: "bold",
          colors: ["#fff"]
        }
      },
      plotOptions: {
        pie: { donut: { size: "50%" } }
      },
      legend: {
        position: "bottom",
        labels: {
          useSeriesColors: true
        }
      }
    };
  }

  clearTooltipContent(): void {
    this.tooltipContent = '';
    this.hoveredCellId = null;
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
