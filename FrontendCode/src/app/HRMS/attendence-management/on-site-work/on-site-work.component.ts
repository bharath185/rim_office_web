import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { CommonModule, DatePipe  } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AttendenceModuleService } from '../../service/attendence.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';


@Component({
  selector: 'app-on-site-work',
  standalone: true,
  providers: [DatePipe],
  imports: [FormsModule, CommonModule, SharedModule, ToastMessageComponent, NgxPaginationModule, NgbModule],
  templateUrl: './on-site-work.component.html',
  styleUrl: './on-site-work.component.scss'
})
export class OnSiteWorkComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal:any= ElementRef;

  addOnSiteForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  today: any;
  employeeDetails
  rows: any[] = [];
  errorMessage: any;
  locationMessage: any;
  isTableData: boolean = false;
  isSpinner: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100,500];;
  getLocationData: any;
  visibleForm: boolean = false;
  isCheckedOut: boolean = false;
  accessPolicy:any
  controlAccessPage:any

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsServiceAttendance: AttendenceModuleService,
    private readonly datePipe: DatePipe,
    private readonly htmsMainservice: HrmsServiceService,
     private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'On Site'
      );
    });

    
  }

  ngOnInit(): void {
    this.addOnSiteForm = this.fb.group({
      date: ['', [Validators.required]],
      company: ['', [Validators.required]],
      // workStatus: ['', [Validators.required]],
      purpose: ['', [Validators.required]]
    });
    const now = new Date();
    this.today = now.toISOString().split('T')[0];
    this.getLocation();
    this.getOnSiteData();
  }

  getTodayDate(): string {
    return this.datePipe.transform(new Date(), 'dd-MM-yyyy')!;
  }
  getOnSiteData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    };
    this.isSpinner = true;
    this.hrmsServiceAttendance.EmployeeGetOnSiteData(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          setTimeout(() => {
            const formattedData = res.map((item: any) => {
              const parsedDate = this.parseJsonDate(item.LoginDate);
              const parsedDateLogout = this.parseJsonDate(item.LogoutDate);
              return {
                ...item,
                LoginDate: parsedDate ? this.formatDate(parsedDate) : '',  
                LogoutDate: parsedDateLogout ? this.formatDate(parsedDateLogout) : '',
              };
            });
            this.rows = [...formattedData];
            this.originalRows = [...formattedData];  
          }, 100);
        } else {
          this.rows = res;
          this.originalRows = res;
        }
        this.isSpinner = false;
        this.isTableData = false;
      },
      error: (err) => {
        this.errorMessage = 'Internal Server Error';
        this.isSpinner = false;
        this.isTableData = true;
      }
    });
  }

  
  tooltipContent: string | null = null;
  showTooltip(column: string, row: any): void {
    if (column === 'logintime') {
      this.tooltipContent = `<strong>Login Address</strong> : ${row.LoginAddress ? row.LoginAddress.slice(0, 30) : ''}<br>
      <strong>Login Date</strong> : ${row.LoginDate ? row.LoginDate : ''} <br>
      <strong>Purpose</strong> : ${row.Purpose ? row.Purpose : ''}`
    } else if (column === 'logouttime') {
      this.tooltipContent = `<strong>Logout Address</strong> : ${row.LogoutAddress ? row.LogoutAddress.slice(0, 30) : ''}<br>
      <strong>Logout Date</strong> : ${row.LogoutDate ? row.LogoutDate : ''} <br>
      <strong>Description</strong> : ${row.Description ? row.Description : ''}`
    } else if (column === 'company') {
      this.tooltipContent = `<strong>Company Details</strong> : ${row.Company ? row.Company : ''}`
    } else {
      this.tooltipContent = null; 
    }
  }

  // This method is triggered when the mouse leaves the cell
  hideTooltip(): void {
    this.tooltipContent = null;
  }

  viewdata: any;
  onView(data: any) {
    // console.log(data);
    this.viewdata = data;
  }
  originalRows: any[] = [];
  applyFilter(event: any): void {
    const query = event.target.value.toLowerCase();
    const filteredRows = this.originalRows.filter((row) => {
      const formattedDate = row.LoginDate ? row.LoginDate.toLowerCase() : '';
      return (
        (row.EmpCode && row.EmpCode.toLowerCase().includes(query)) ||  // Filter by Employee Code
        (row.Company && row.Company.toLowerCase().includes(query)) ||  // Filter by Company
        (row.Address && row.Address.toLowerCase().includes(query)) ||  // Filter by Address
        (row.City && row.City.toLowerCase().includes(query)) ||
        (formattedDate.includes(query))        // Filter by Date
      );
    });
    this.rows = filteredRows;
    if (filteredRows.length === 0) {
      this.isTableData = true;
      this.errorMessage = 'No Records Found for Searched Data';
    } else {
      this.isTableData = false;
    }
  }
  sortDirection: { [key: string]: boolean } = { Date: true };
  sortKey: string = 'Date';
  sortData(column: string): void {
    if (this.sortKey === column) {
      this.sortDirection[column] = !this.sortDirection[column];
    } else {
      this.sortKey = column;
      this.sortDirection[column] = true;
    }

    this.rows.sort((a, b) => {
      let valA = a[column];
      let valB = b[column];

      if (valA == null) valA = '';
      if (valB == null) valB = '';

      if (typeof valA === 'string' && typeof valB === 'string') {
        valA = valA.toLowerCase();
        valB = valB.toLowerCase();
      }

      if (this.sortDirection[column]) {
        return valA > valB ? 1 : valA < valB ? -1 : 0;
      } else {
        return valA < valB ? 1 : valA > valB ? -1 : 0;
      }
    });
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
        } else {
          console.error('Unable to retrieve location data');
        }
      })
      .catch(error => {
        console.error('Error with reverse geocoding:', error);
      });
  }

  submitFormData() {
    const isOnsiteVisit = sessionStorage.getItem('isOnsiteVisit');
    const lastLoginDate = sessionStorage.getItem('lastLoginDate');  
    const currentDate = new Date().toISOString().split('T')[0]; 
    if (isOnsiteVisit === 'true' && lastLoginDate === currentDate) {
      this.triggerToast('Check-Out Required', 'You must check-out from your previous check-in before checking in again.', 'warning');
      return;
    }
    
    const fromDateValue = this.addOnSiteForm?.get('date')?.value;
    const parseDate = (date: any): Date | null => {
      if (date === null || date === undefined) return null;
      if (typeof date === 'string') return new Date(date);
      if (date instanceof Date) return date;
      return null;
    };
    
    const formatDate = (date: Date | null): string => {
      if (!date) return '';
      const day = date.getDate().toString().padStart(2, '0');
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const year = date.getFullYear();
      return `${year}-${month}-${day}`; // Return in 'YYYY-MM-DD' format for easier comparison
    };
    
    const fromDate = parseDate(fromDateValue);
    const fromOnly = formatDate(fromDate);
    
    const formData = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpName: this.employeeDetails[0].FirstName,
      EmpCode: this.employeeDetails[0].EmpCode,
      LoginDate: fromOnly,
      Company: this.addOnSiteForm.get('company').value,
      WorkStatus: "Login",
      LoginLatitude: this.getLocationData?.lat,
      LoginLongitude: this.getLocationData?.lon,
      LoginAddress: this.getLocationData?.display_name,
      LoginCity: this.getLocationData?.address['city'],
      Purpose: this.addOnSiteForm.get('purpose').value,
      LogoutLatitude: '',
      LogoutLongitude: '',
      LogoutAddress: '',
      LogoutCity: '',
      LogOutTime: '',
      LogoutDescription: '',
    };
    
    console.log(formData);
    this.isSpinner = true;
    this.hrmsServiceAttendance.EmployeeAddOnSiteData(formData).subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('Something Went Wrong', res['Message'], 'warning');
          this.isSpinner = false;
          this.isTableData = false;
        } else {
          this.triggerToast('Check-In Details', res['msg'], 'success');
          sessionStorage.setItem('isOnsiteVisit', 'true');
          sessionStorage.setItem('lastLoginDate', fromOnly);  // Store the check-in date
          this.getOnSiteData();
          this.isSpinner = false;
          this.isTableData = false;
        }
      }, 
      error: (error: any) => {
        this.isSpinner = false;
        this.isTableData = true;
      }
    });
  }
  getCheckoutData: any
  checkModalView(data: any) {
    // console.log(data);
    this.getCheckoutData = data;
  }
  description: string = ''; 
  checkoutFromOffice() {
    const formData = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpName: this.employeeDetails[0].FirstName,
      EmpCode: this.getCheckoutData?.EmpCode,
      Company: this.getCheckoutData?.Company,
      WorkStatus: "Logout",
      LoginAddress: this.getCheckoutData?.LoginAddress,
      LoginLatitude: this.getCheckoutData?.LoginLatitude,
      LoginLongitude: this.getCheckoutData?.LoginLongitude,
      LoginCity: this.getCheckoutData?.LoginCity,
      Id: this.getCheckoutData?.Id,
      LoginDate: this.getCheckoutData?.LoginDate,
      LogInTDate: this.getCheckoutData?.LoginDate,
      Logoutime: this.getCheckoutData?.LogInTime,
      LogoutLatitude: this.getLocationData?.lat,
      LogoutLongitude: this.getLocationData?.lon,
      LogoutAddress: this.getLocationData?.display_name,
      LogoutCity: this.getLocationData?.address['city'],
      Purpose: this.getCheckoutData?.LoginPurpose,
      Description: this.description
    };
    
    console.log(formData);
    this.isSpinner = true;
    this.hrmsServiceAttendance.EmployeeAddOnSiteData(formData).subscribe({
      next: (res: any) => {
        if ( res['msg'] == 'Updated') {
          this.triggerToast('Check-Out', res['msg'], 'success');
          sessionStorage.removeItem('isOnsiteVisit');
          sessionStorage.removeItem('lastLoginDate');  
          this.getOnSiteData();
          this.isSpinner = false;
          this.isTableData = false;
          this.isCheckedOut = true;
          this.description = '';
          setTimeout(() => {
            this.closeModal.nativeElement?.click();
            setTimeout(() => {
              this.isCheckedOut = false;
            }, 1100);
          }, 1000);
        } else if (res['Message']) {
          this.triggerToast('Something Went Wrong', res['Message'], 'warning');
          this.isSpinner = false;
          this.isTableData = false;
        }
      }, 
      error: (error: any) => {
        this.isSpinner = false;
        this.isTableData = true;
      }
    });
  }
 resetValOnCloseModal(){
  this.description = '';
 }

  formatDate(date: Date): string {
    const day = String(date.getDate()).padStart(2, '0'); // Ensure 2 digits
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  }

  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      return new Date(parseInt(match[1], 10));
    }
    return null;
  }

  onBlur(event: any) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  onFocus(event: FocusEvent) {
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
    event.preventDefault(); // Prevents any keyboard input
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault(); // Prevents paste input
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


}
