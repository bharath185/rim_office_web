import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { HttpClient } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { forkJoin, Observable, of } from 'rxjs';
import { AttendenceModuleService } from '../service/attendence.service';
import { HrmsServiceService } from '../hrms-service.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-contract-attendance',
  standalone: true,
  imports: [FormsModule, CommonModule, SharedModule, ToastMessageComponent, NgxPaginationModule],

  templateUrl: './contract-attendance.component.html',
  styleUrl: './contract-attendance.component.scss'
})
export class ContractAttendanceComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;


  contractAttendanceForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  showFullForm: boolean = false;

  isTableData: boolean = false;
  isSpinner: boolean = false;
  getLocationData: any;
  locationMessage: any;
  visibleForm: boolean = false;
  locationUrl: any;
  getVendorList: any = [];
  getSiteList: any = [];
  getDDProjectList: any = [];
  getLoginStatusValue: any = [];
  showOtherInput: boolean = false;
  storeMobileValue: any;
  CId: any;
  logoutDescription: string = '';

  constructor(private readonly fb: FormBuilder,
    public hrmsMainService: HrmsServiceService,
    private route: Router
  ) { }

  ngOnInit(): void {
    this.getLocation();
    this.contractAttendanceForm = this.fb.group({
      mobile_number: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      employee_name: ['', [Validators.required]],
      vendar: ['', [Validators.required]],
      project: ['', [Validators.required]],
      manager: [''],
      managerCode: [''],
      code: [''],
      site: ['', [Validators.required]],
      otherName: [''],
      skills: [''],
      loginstatus: [''],
      official_email: [''],
    });
  }
convertToUppercase(event: any, controlName: string) {
    const value = event.target.value.toUpperCase();
    this.contractAttendanceForm.get(controlName)?.setValue(value, { emitEvent: false });
  }
  formatTo12Hour(hours: number, minutes: number): string {
    const ampm = hours >= 12 ? 'PM' : 'AM';
    const formattedHours = hours % 12 || 12; // convert 0 to 12
    const formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
    return `${formattedHours}:${formattedMinutes} ${ampm}`;
  }

  onCheckStatus() {
    if (this.contractAttendanceForm.get('mobile_number')?.valid) {
      const reqBody = {
        LoginId: 149,
        MobileNo: this.contractAttendanceForm.get('mobile_number')?.value,
      }
      this.isSpinner = true;
      this.hrmsMainService.ContractAttendanceChecking(reqBody).subscribe({
        next: (res: any) => {
          this.getLoginStatusValue = res;
          this.showFullForm = true;
          this.isFormSubmitted = false;
          this.storeMobileValue = this.contractAttendanceForm.get('mobile_number')?.value;
          forkJoin({
            vendorList: this.dropdownVendorList(),
            projectList: this.dropdownProjectList(),
            siteList: this.dropdownSiteList()
          }).subscribe({
            next: ({ vendorList, projectList, siteList }) => {
              this.patchValueWhenLogin(res);
              this.isSpinner = false;
            },
            error: () => {
              this.isSpinner = false;
            }
          });
        },
        error: () => {
          this.triggerToast('Internal Server Error', 'Sorry! Failed Try Again', 'danger');
          this.isSpinner = false;
        }
      });
    } else {
      this.isFormSubmitted = true;
    }

  }

  patchValueWhenLogin(res: any) {
    const selectedVendor = this.getVendorList.find((v: any) => v.Vendor === res.Vendor);
    const selectedProject = this.getDDProjectList.find((p: any) => p.Project === res.Project);
    const selectedSite = this.getSiteList.find((p: any) => p.Site === res.Site);
    if (res.SiteDetails !== null && res.SiteDetails !== "") {
      this.showOtherInput = true;
      console.log('entring');
    } else {
      this.showOtherInput = false;
      console.log('not showing otherName');
    }
    this.CId = res.CId;
    this.contractAttendanceForm.patchValue({
      mobile_number: res.Mobile,
      vendar: selectedVendor || null,
      employee_name: res.EmpName,
      code: res.EmpCode,
      project: selectedProject || null,
      site: selectedSite || null,
      otherName: res.SiteDetails,
      manager: res.ManagerName,
      managerCode: res.ManagerEmpCode,
      skills: res.Skill,
      official_email: res.Mail
    });
  }

  dropdownVendorList() {
    this.isSpinner = true;
    const reqBody = { LoginId: 149 };
    return this.hrmsMainService.DDVendorList(reqBody).pipe(
    // return this.hrmsMainService.erpContractAttendanceVendor().pipe(
      tap((res: any) => {
        if (res.length > 0) {
          this.getVendorList = res;
        } else {
          this.triggerToast('', 'No Data To Load Vendor List', 'warning');
        }
        this.isSpinner = false;
      }),
      catchError(err => {
        this.triggerToast('Internal Server Error', 'Failed To Load Vendor List', 'danger');
        this.isSpinner = false;
        return of([]);  // return empty array on error
      })
    );
  }

  dropdownProjectList() {
    this.isSpinner = true;
    const reqBody = { LoginId: 149 };
    return this.hrmsMainService.DDProjectList(reqBody).pipe(
      tap((res: any) => {
        if (res.length > 0) {
          this.getDDProjectList = res;
        } else {
          this.triggerToast('', 'No Data To Load Project List', 'warning');
        }
        this.isSpinner = false;
      }),
      catchError(err => {
        this.triggerToast('Internal Server Error', 'Failed To Load Project List', 'danger');
        this.isSpinner = false;
        return of([]); // return empty array on error
      })
    );
  }
  onProjectChange() {
    const selectedProject = this.contractAttendanceForm.get('project')?.value;

    if (selectedProject) {

      // Patch Manager details
      this.contractAttendanceForm.patchValue({
        manager: selectedProject.ManagerName,
        managerCode: selectedProject.ManagerCode
      });

      // 🔥 NEW LOGIC FOR SITE
      if (selectedProject.Site && selectedProject.SiteId) {
        // Find matching site object from dropdown list
        const matchedSite = this.getSiteList.find(
          (site: any) => site.SiteId === selectedProject.SiteId
        );

        if (matchedSite) {
          this.contractAttendanceForm.patchValue({
            site: matchedSite
          });

          this.contractAttendanceForm.get('site')?.disable(); // Disable dropdown
        }
      } else {
        // If no site in project response
        this.contractAttendanceForm.patchValue({
          site: ''
        });

        this.contractAttendanceForm.get('site')?.enable(); // Enable dropdown
      }

    } else {
      // Reset everything if no project selected
      this.contractAttendanceForm.patchValue({
        manager: '',
        managerCode: '',
        site: ''
      });

      this.contractAttendanceForm.get('site')?.enable();
    }
  }

  onSiteChange() {
    const selectedSite = this.contractAttendanceForm.get('site')?.value;
    if (selectedSite?.Site === 'Others') {
      this.showOtherInput = true;
      this.contractAttendanceForm.get('otherName')?.setValidators([Validators.required]);
    } else {
      this.showOtherInput = false;
      this.contractAttendanceForm.get('otherName')?.clearValidators();
      this.contractAttendanceForm.patchValue({ otherName: '' });
    }
    this.contractAttendanceForm.get('otherName')?.updateValueAndValidity();
  }

  dropdownSiteList() {
    this.isSpinner = true;
    const reqBody = { LoginId: 149 };

    return this.hrmsMainService.DDSiteList(reqBody).pipe(
      tap((res: any) => {
        if (res.length > 0) {
          this.getSiteList = res;
        } else {
          this.triggerToast('', 'No Data To Load Site List', 'warning');
        }
        this.isSpinner = false;
      }),
      catchError(err => {
        this.triggerToast('Internal Server Error', 'Failed To Load Site List', 'danger');
        this.isSpinner = false;
        return of([]); // return empty array on error
      })
    );
  }

  submitLoginForm() {
    this.contractAttendanceForm.get('mobile_number')?.setValue(this.storeMobileValue);
    if (this.contractAttendanceForm?.valid) {
      console.log(this.getLocationData);
      console.log(this.locationUrl);
      const selectedVendor = this.contractAttendanceForm.get('vendar')?.value;
      const selectedProject = this.contractAttendanceForm.get('project')?.value;
      const selectedSite = this.contractAttendanceForm.get('site')?.value;

      const reqBody = {
        LoginId: 149,
        LoginStatus: "No Data",
        Mobile: this.storeMobileValue,
        Mail: this.contractAttendanceForm?.get('official_email').value,
        EmpCode: this.contractAttendanceForm?.get('code').value,
        EmpName: this.contractAttendanceForm?.get('employee_name').value,
        Skill: this.contractAttendanceForm?.get('skills').value || "",
        VendorId: selectedVendor?.VendorId,
        ERPVendorId: selectedVendor?.ERPVendorId,
        Vendor: selectedVendor?.Vendor,
        ProjectId: selectedProject?.ProjectId,
        ERPProjectId: selectedProject?.ERPProjectId,
        ProjectCode: selectedProject?.ProjectCode,
        Project: selectedProject?.Project,
        ManagerId: selectedProject?.ManagerId,
        ManagerEmpCode: selectedProject?.ManagerCode,
        ManagerName: selectedProject?.ManagerName,
        SiteId: selectedSite?.SiteId,
        Site: selectedSite?.Site,
        SiteDetails: this.contractAttendanceForm?.get('otherName').value,
        LoginLatitude: this.getLocationData?.lat,
        LoginLonqitude: this.getLocationData?.lon,
        LoginAddress: this.getLocationData?.display_name,
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsMainService.AddContractAttendance(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === 'Login Successfully') {
            this.triggerToast('', res['msg'], 'success');
            this.route.navigate(['success'], {
              state: { message: 'Login Successfully!' }
            });
          } else if (res['Message']) {
            this.triggerToast('', res['Message'], 'warning');
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'Failed To Login, Try Again', 'danger');
          this.isSpinner = false;
        }
      })
    } else {
      this.isFormSubmitted = true;
      console.log('else');
      Object.keys(this.contractAttendanceForm.controls).forEach(key => {
        const control = this.contractAttendanceForm.get(key);
        if (control?.invalid) {
          console.log(key, control.errors);
        }
      });

    }
  }

  logOut() {
    if (!this.logoutDescription?.trim()) return;
    const reqBody = {
      LoginId: 149,
      LoginStatus: "LOGIN",
      CId: this.CId,
      LogoutLatitude: this.getLocationData?.lat,
      LogoutLonqitude: this.getLocationData?.lon,
      LogoutAddress: this.getLocationData?.display_name,
      Description: this.logoutDescription.trim()
    }
    this.isSpinner = true;
    this.hrmsMainService.AddContractAttendance(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg'] === 'Logout Successfully') {
          this.triggerToast('', res['msg'], 'success');
          this.closeModal.nativeElement?.click();
          this.route.navigate(['success'], {
            state: { message: 'Logout Successfully!' }
          });

        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Failed To Logout, Try Again', 'danger');
        this.isSpinner = false;
      }
    })
  }
  resetFormData() {
    this.isFormSubmitted = false;
    this.contractAttendanceForm.reset();
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


  handleAlphaChar(event: any) {
    if (
      (event.charCode > 32 && event.charCode < 48) ||
      (event.charCode > 57 && event.charCode < 127)
    ) {
      event.preventDefault();
    }
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }



}
