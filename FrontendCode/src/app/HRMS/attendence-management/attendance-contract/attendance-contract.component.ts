import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ValidationErrors } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { catchError, map, tap } from 'rxjs/operators';
import { forkJoin, Observable, of } from 'rxjs';
import * as XLSX from 'xlsx-js-style';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-attendance-contract',
  standalone: true,
  imports: [FormsModule, CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule],
  templateUrl: './attendance-contract.component.html',
  styleUrl: './attendance-contract.component.scss'
})
export class AttendanceContractComponent implements OnInit {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  employeeDetails;
  accessPolicy: any
  controlAccessPage: any
  isSpinner: boolean = false;
  searchValue: any;
  contractAttendanceForm: any = FormGroup;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  isTableData: boolean = false;
  getLocationData: any;
  locationMessage: any;
  visibleForm: boolean = false;
  locationUrl: any;
  today = new Date().toISOString().split('T')[0];
  minDate: string | undefined;
  getDDProjectList: any = [];
  getVendorList: any = [];
  dropdownVisible = false;


  constructor(
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly employeeService: EmployeeModuleService,
    private readonly fb: FormBuilder,
    private readonly hrmsMainService: HrmsServiceService,
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Contract Attendance'
      );
    });

  }
  ngOnInit(): void {
    this.contractAttendanceForm = this.fb.group({
      date_from: [''],
      date_to: [''],
      project: [''],
      vendar: [''],
      status: [''],
    }, { validators: this.dateRangeValidator })
    this.getLocation();

    setTimeout(() => {
      setTimeout(() => {
        this.dropdownProjectList();
      }, 100);
      setTimeout(() => {
        this.dropdownVendorList();
      }, 100);
      this.ContractAttendanceManager();
    }, 100);
  }
  onFromDate(): void {
    if (this.contractAttendanceForm.get('date_from')?.value) {
      this.minDate = this.contractAttendanceForm.get('date_from')?.value;
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
    return this.contractAttendanceForm.hasError('dateRange');
  }
  getLocation(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const latitude = position.coords.latitude;
          const longitude = position.coords.longitude;
          this.getPlaceDetails(latitude, longitude);
          this.locationMessage = null;
          this.visibleForm = true;
        },
        (error) => {
          this.locationMessage = this.getErrorMessage(error.code);
          const latitude = null;
          const longitude = null;
          this.visibleForm = false;
        }
      );
    } else {
      this.locationMessage = 'Geolocation is not supported by this browser.';
      this.visibleForm = false;
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

  getPlaceDetails(latitude: number, longitude: number) {
    const geocodeUrl = `https://nominatim.openstreetmap.org/reverse?lat=${latitude}&lon=${longitude}&format=json`;

    fetch(geocodeUrl)
      .then(response => response.json())
      .then(data => {
        if (data && data.address) {
          this.getLocationData = data;

          this.locationUrl = `https://www.openstreetmap.org/?mlat=${latitude}&mlon=${longitude}#map=18/${latitude}/${longitude}`;
        }
      })
      .catch(error => {
        console.error('Error with reverse geocoding:', error);
      });
  }

  dropdownProjectList() {
    this.isSpinner = true;
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.hrmsMainService.DDProjectList(reqBody).subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.getDDProjectList = res;
        } else {
          this.triggerToast('', 'No Data To Load Project List', 'warning');
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Failed To Load Project List', 'danger');
        this.isSpinner = false;
      }
    })
  }

  dropdownVendorList() {
    this.isSpinner = true;
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.hrmsMainService.DDVendorList(reqBody).subscribe({
      // this.hrmsMainService.erpContractAttendanceVendor().subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.getVendorList = res;
        } else {
          this.triggerToast('', 'No Data To Load Vendor List', 'warning');
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Failed To Load Vendor List', 'danger');
        this.isSpinner = false;
      }
    })
  }
  formatTime(time: any): string {
    const hours = String(time.Hours).padStart(2, '0');
    const minutes = String(time.Minutes).padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  ContractAttendanceManager() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      FromDate: "",
      ToDate: "",
      ProjectId: 0
    };
    this.isSpinner = true;
    this.hrmsMainService.ContractAttendanceManager(reqBody).subscribe({
      next: (res: any) => {
        if (res && res.length >= 1) {
          this.rows = res.map((row: any) => {

            if (row.Date) {
              const timestamp = Number(row.Date.replace(/[^0-9]/g, ''));
              row.Date = new Date(timestamp);
            }

            if (row.LoginTime) {
              row.LoginTimeFormatted = this.formatTime(row.LoginTime);
            }

            if (row.LogoutTime) {
              row.LogoutTimeFormatted = this.formatTime(row.LogoutTime);
            }

            if (row.Activehrs) {
              row.ActivehrsFormatted = this.formatTime(row.Activehrs);
            }
            if (row.Approvedhrs) {
              row.ApprovedhrsFormatted = this.formatTime(row.Approvedhrs);
            }

            return row;
          });
          this.originalRows = res;
          this.isTableData = false;
        } else {
          this.errorMessage = "No records found";
          this.isTableData = true;
        }
        this.isSpinner = false;
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Failed To Load The Data', "danger");
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
        this.page = 1;
        this.rows = [];
      }
    });
  }

  submitFilterData() {
    const fromDate = this.contractAttendanceForm.get('date_from')?.value || "";
    const toDate = this.contractAttendanceForm.get('date_to')?.value || "";
    const projectId = this.contractAttendanceForm.get('project')?.value || 0;
    const vendarId = this.contractAttendanceForm.get('vendar')?.value || 0;
    const status = this.contractAttendanceForm.get('status')?.value || '';
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      FromDate: fromDate,
      ToDate: toDate,
      ProjectId: projectId,
      VendorId: vendarId,
      Status: status
    };
    this.isSpinner = true;
    this.hrmsMainService.ContractAttendanceManager(reqBody).subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
          this.isSpinner = false;
          this.errorMessage = "No records found";
          this.isTableData = true;
        } else {
          if (res && res.length >= 1) {
            this.rows = res.map((row: any) => {

              if (row.Date) {
                const timestamp = Number(row.Date.replace(/[^0-9]/g, ''));
                row.Date = new Date(timestamp);
              }

              if (row.LoginTime) {
                row.LoginTimeFormatted = this.formatTime(row.LoginTime);
              }

              if (row.LogoutTime) {
                row.LogoutTimeFormatted = this.formatTime(row.LogoutTime);
              }

              if (row.Activehrs) {
                row.ActivehrsFormatted = this.formatTime(row.Activehrs);
              }
              if (row.Approvedhrs) {
                row.ApprovedhrsFormatted = this.formatTime(row.Approvedhrs);
              }

              return row;
            });
            this.originalRows = res;
            this.isTableData = false;
          } else {
            this.errorMessage = "No records found";
            this.isTableData = true;
          }
          this.isSpinner = false;
        }
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Failed To Load The Data', "danger");
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
        this.page = 1;
        this.rows = [];
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

  exportFile(format: string) {
    if (format === 'excel') {
      this.exportToExcel();
    }

  }
  exportToExcel(): void {
    if (!this.rows || this.rows.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }

    const formatTime = (timeObj: any) => {
      if (!timeObj) return '-';
      const h = timeObj.Hours.toString().padStart(2, '0');
      const m = timeObj.Minutes.toString().padStart(2, '0');
      return `${h}:${m}`;
    };

    const excelRows = this.rows.map((item, index) => ({
      "Sl No.": index + 1,
      "Date": item.Date,
      "Employee Name": `${item.EmpName} (${item.EmpCode})`,
      "Project": item.Project,
      "Manager": item.ManagerName,
      "Mobile": item.Mobile,
      "Login": formatTime(item.LoginTime),
      "Logout": item.IsLogout ? 'Logged Out' : (item.LogoutTime ? formatTime(item.LogoutTime) : 'Logout'),
      "Active Hours": item.Activehrs
        ? `${item.Activehrs.Hours.toString().padStart(2, '0')}:${item.Activehrs.Minutes.toString().padStart(2, '0')}`
        : '-',
      "Adjust Hour": item.Approvedhrs
        ? `${item.Approvedhrs.Hours.toString().padStart(2, '0')}:${item.Approvedhrs.Minutes.toString().padStart(2, '0')}`
        : '00:00',
      "Status": item.IsApproved ? 'Approved' : 'Pending'
    }));

    const worksheet = XLSX.utils.json_to_sheet(excelRows);
    const range = XLSX.utils.decode_range(worksheet['!ref']!);

    for (let C = range.s.c; C <= range.e.c; C++) {
      const headerCell = XLSX.utils.encode_cell({ r: 0, c: C });
      const headerValue = worksheet[headerCell]?.v;

      for (let R = 1; R <= range.e.r; R++) {
        const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
        if (!worksheet[cellRef]) continue;
        const cellValue = worksheet[cellRef].v;

        // Logout coloring
        if (headerValue === 'Logout') {
          if (cellValue === 'Logged Out') worksheet[cellRef].s = { fill: { fgColor: { rgb: 'D3D3D3' } } };
          else if (cellValue === 'Logout') worksheet[cellRef].s = { fill: { fgColor: { rgb: 'FFB3B3' } } };
        }

        // Status coloring
        if (headerValue === 'Status') {
          if (cellValue === 'Approved') worksheet[cellRef].s = { fill: { fgColor: { rgb: 'B3FFB3' } } };
          else if (cellValue === 'Pending') worksheet[cellRef].s = { fill: { fgColor: { rgb: 'FFDAB3' } } };
        }

        // Active Hours <1h warning
        if (headerValue === 'Active Hours' && cellValue && cellValue !== '-') {
          const [h, m] = cellValue.split(':').map(Number);
          if (h === 0 && m < 60) worksheet[cellRef].s = { fill: { fgColor: { rgb: 'FFCCCC' } } };
        }
      }
    }

    const workbook: XLSX.WorkBook = { Sheets: { 'Attendance': worksheet }, SheetNames: ['Attendance'] };
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'ContractAttendance.xlsx');
    this.dropdownVisible = false;
  }

  resetData() {
    this.contractAttendanceForm?.reset();
    this.ContractAttendanceManager();
  }

  isAllSelected: boolean = false;
  toggleAllSelection() {
    this.rows.forEach(row => {
      if (!row.IsApproved) {         // Only select unapproved rows
        row.isSelected = this.isAllSelected;
      }
    });

    this.logSelectedRows();
  }

  onRowSelectionChange() {
    // Only consider unapproved rows for header checkbox
    this.isAllSelected = this.rows
      .filter(row => !row.IsApproved)
      .every(row => row.isSelected);

    this.logSelectedRows();
  }
  getSelectedValue: any = [];
  logSelectedRows() {
    const selectedRows = this.rows.filter(row => row.isSelected);
    this.getSelectedValue = selectedRows;
  }

  approve() {
    // Filter only selected rows
    const selectedRows = this.rows.filter(row => row.isSelected);
    if (selectedRows.length === 0) {
      alert('Please select at least one row to approve.');
      return;
    }
    // Map to required payload format
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      lstofCantractIId: selectedRows.map(row => ({ CId: row.CId }))
    };
    console.log('Payload to send:', payload);
    // Call API
    this.isSpinner = true;
    this.employeeService.ApprovedbyManager(payload).subscribe({
      next: (res: any) => {
        if (res['msg'] === 'Approved Successfully') {
          selectedRows.forEach(row => row.IsApproved = true);
          this.isAllSelected = false;
          this.ContractAttendanceManager();
          this.logSelectedRows();
          this.isSpinner = false;
          this.triggerToast(res['msg'], '', 'success')
        } else if (res['Message']) {
          this.triggerToast(res['Message'], '', 'success');
        }
        this.isSpinner = false;
      },
      error: (err: any) => {
        console.error('Approval failed', err);
        this.isSpinner = false;
        this.triggerToast('Internal Server Error', 'Failed To Approve The Data Try Again!', 'danger')
      }
    });
  }


  viewdata: any;
  onModalView(data: any) {
    this.viewdata = data;
    console.log(data);
  }

  onLogout(row: any) {
    const confirmLogout = confirm('Are you sure you want to logout this employee?');
    if (!confirmLogout) {
      return; // If user clicks "Cancel", stop here
    }
    console.log('Logout clicked for:', row);
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      CId: row.CId,
      LogoutLatitude: this.getLocationData?.lat,
      LogoutLonqitude: this.getLocationData?.lon,
      LogoutAddress: this.getLocationData?.display_name,
      Description: ""
    };
    this.isSpinner = true;
    this.employeeService.LogoutbyManager(payload).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], '', '');
          this.ContractAttendanceManager();
          this.logSelectedRows();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
        }
        this.isSpinner = false;
      },
      error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Failed To Logout Try Again!', 'danger');
        this.isSpinner = false;
      }
    });
  }

  enableEdit(row: any) {
    row.isEditing = true;
  }
  saveAdjustHour(row: any) {
    row.isEditing = false;
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      CId: row.CId,
      Approvedhrs: row.AdjustHour
    };
    console.log("Sending:", payload);
    this.isSpinner = true;
    this.employeeService.ApprovedHrbyManager(payload).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], '', 'success');
          this.ContractAttendanceManager();
          this.logSelectedRows();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
        }
        this.isSpinner = false;
      },
      error: () => {
        this.isSpinner = false;
        this.triggerToast('Internal Server Error', 'Failed To Update Hour Try Again!', 'danger');
      }
    });
  }

  formatToHHMM(time: any): string {
    const hours = String(time?.Hours || 0).padStart(2, '0');
    const minutes = String(time?.Minutes || 0).padStart(2, '0');
    return `${hours}:${minutes}`;
  }
  applyFilter() {
    const val = this.searchValue?.toLowerCase().trim() || '';
    if (!this.originalRows || this.originalRows.length === 0) {
      this.rows = [];
      this.isTableData = true;
      this.errorMessage = `No record found for "${this.searchValue}"`;
      return;
    }
    this.rows = this.originalRows.filter((row: any) => {
      const searchableText = `
      ${row.Date || ''}
      ${row.EmpName || ''}
      ${row.EmpCode || ''}
      ${row.Project || ''}
      ${row.ProjectCode || ''}
      ${row.ManagerName || ''}
      ${row.Mobile || ''}
      ${row.Status ?? ''}
      ${row.LoginStatus || ''}
    `.toLowerCase();
      return searchableText.includes(val);
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

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
