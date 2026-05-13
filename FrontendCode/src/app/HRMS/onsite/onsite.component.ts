import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AttendenceModuleService } from '../service/attendence.service';
import { HrmsServiceService } from '../hrms-service.service';

@Component({
  selector: 'app-onsite',
  standalone: true,
  imports: [FormsModule, CommonModule, SharedModule, ToastMessageComponent, NgxPaginationModule],
  templateUrl: './onsite.component.html',
  styleUrl: './onsite.component.scss'
})
export class OnsiteComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  addOnSiteForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  today: any;
  errorMessage: any;
  locationMessage: any;
  isTableData: boolean = false;
  isSpinner: boolean = false;
  getLocationData: any;
  visibleForm: boolean = false;

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsServiceAttendance: AttendenceModuleService,
    private readonly htmsMainservice: HrmsServiceService,
    private http: HttpClient) {

  }

  ngOnInit(): void {
    this.addOnSiteForm = this.fb.group({
      employee_name: ['', [Validators.required]],
      employee_code: ['', [Validators.required]],
      date: ['', [Validators.required]],
      company: ['', [Validators.required]],
      workStatus: ['', [Validators.required]]
    });
    const now = new Date();
    this.today = now.toISOString().split('T')[0];
    this.getLocation();
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
          console.log(data);
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
      const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
      const year = date.getFullYear();
      return `${day}-${month}-${year}`;
    };
    const fromDate = parseDate(fromDateValue);
    const fromOnly = formatDate(fromDate);
    const formData = {
      LoginId: 149,
      EmpName: this.addOnSiteForm.get('employee_name').value,
      EmpCode: this.addOnSiteForm.get('employee_code').value,
      Date: fromOnly,
      Company: this.addOnSiteForm.get('company').value,
      WorkStatus: this.addOnSiteForm.get('workStatus').value,
      Latitude: this.getLocationData?.lat,
      Longitude: this.getLocationData?.lon,
      Address: this.getLocationData?.display_name,
      City: this.getLocationData?.address['city']
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
          this.triggerToast('Added Details', res['msg'], 'success');
          this.isSpinner = false;
          this.isTableData = false;
        }

      }, error: (error: any) => {
        this.isSpinner = false;
        this.isTableData = true;
      }
    })
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
