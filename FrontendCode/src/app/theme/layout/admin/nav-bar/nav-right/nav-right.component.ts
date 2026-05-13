// Angular Import
import { Component, OnInit, ViewChild } from '@angular/core';
import { animate, style, transition, trigger } from '@angular/animations';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { HrmsServiceService } from 'src/app/HRMS/hrms-service.service';
import { EmployeeModuleService } from 'src/app/HRMS/service/employee.service';
import { NavigationItem } from '../../navigation/navigation';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EntityStateService } from 'src/app/HRMS/service/entity-state.service';
import { AttendenceModuleService } from 'src/app/HRMS/service/attendence.service';

@Component({
  selector: 'app-nav-right',
  templateUrl: './nav-right.component.html',
  styleUrls: ['./nav-right.component.scss'],
  providers: [NgbDropdownConfig],
  animations: [
    trigger('slideInOutLeft', [
      transition(':enter', [style({ transform: 'translateX(100%)' }), animate('300ms ease-in', style({ transform: 'translateX(0%)' }))]),
      transition(':leave', [animate('300ms ease-in', style({ transform: 'translateX(100%)' }))])
    ]),
    trigger('slideInOutRight', [
      transition(':enter', [style({ transform: 'translateX(-100%)' }), animate('300ms ease-in', style({ transform: 'translateX(0%)' }))]),
      transition(':leave', [animate('300ms ease-in', style({ transform: 'translateX(-100%)' }))])
    ])
  ]
})
export class NavRightComponent implements OnInit {
  changePasswordForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  newPasswordFieldType: string = 'password';
  confirmPasswordFieldType: string = 'password';

  visibleUserList: boolean;
  chatMessage: boolean;
  friendId!: number;
  userDetails: any;
  loggedInuser: String | undefined;
  looggedInuserName: any;
  designation: any
  employeeDetails: any;
  navigations: NavigationItem[] = [];
  userDet1: any;
  UserDet2: any;
  getLegalEntity: any;
  getLocationData: any;
  isSpinner: boolean = false;
  locationMessage: any;
  isLoggedIn: boolean = false;


  constructor(private readonly route: Router,
    private readonly hrmsService: HrmsServiceService,
    private readonly fb: FormBuilder,
    private readonly employeeModuleService: EmployeeModuleService,
    private entityStateService: EntityStateService,
    private readonly attendanceService: AttendenceModuleService,
  ) {
    this.visibleUserList = false;
    this.chatMessage = false;


  }
  ngOnInit(): void {
    const storedEmployeeData = sessionStorage.getItem('userdata');
    this.userDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    const employeeDetails = sessionStorage.getItem('employeeDetails');
    if (employeeDetails) {
      try {
        this.employeeDetails = JSON.parse(employeeDetails);
      } catch (error) {
        console.error('Error parsing JSON:', error);
      }
    } else {
      console.warn('No employee details provided');
    }
    // if (this.employeeDetails[0]?.OnSiteStatus === 'LOGIN' && this.employeeDetails) {
    //   this.isLoggedIn = true;
    // }
    this.isLoggedIn = this.employeeDetails?.[0]?.OnSiteStatus === 'LOGIN';
    setTimeout(() => {
      this.calllegalEntity();
    }, 100);
    if (this.userDetails != null || undefined || '') {
      this.loggedInuser = this.userDetails.EmpCode;
      this.looggedInuserName = this.userDetails['FirstName'] + "," + this.userDetails['UserName'];
      this.userDet1 = this.userDetails['FirstName'];
      this.UserDet2 = this.userDetails['UserName'];
      this.designation = this.userDetails['Designation'];
    }

    this.changePasswordForm = this.fb.group({
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,16}$")]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validator: this.passwordMatchValidator
    })
  }
  selectedEntityId: number | null = null;
  calllegalEntity() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: 1,
      AuthorisedEntity: this.employeeDetails[0].AuthorisedEntity
    };
    if (this.employeeDetails[0].AuthorisedEntity !== '' && this.employeeDetails[0].AuthorisedEntity !== null) {
      this.employeeModuleService.employeeDDLegalEntity(reqBody).subscribe((res: any[]) => {
        if (res && res.length > 0) {
          this.getLegalEntity = res;
          const lastId = this.entityStateService.getEntityId() || sessionStorage.getItem('SelectedLEId');
          if (lastId) {
            this.selectedEntityId = +lastId;
            this.entityStateService.setEntityId(this.selectedEntityId);
          }
        }
      });
    }
  }

  //////////// This is Geolocation //////////////
  getLocation(callback?: () => void): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const latitude = position.coords.latitude;
          const longitude = position.coords.longitude;

          this.getPlaceDetails(latitude, longitude, () => {
            if (callback) callback(); // ✅ always call
          });

          this.locationMessage = null;
        },
        (error) => {
          this.locationMessage = this.getErrorMessage(error.code);

          // ✅ IMPORTANT: fallback (do not block login/logout)
          this.getLocationData = {
            lat: '',
            lon: '',
            display_name: '',
            address: {}
          };

          if (callback) callback(); // ✅ still continue
        },
        {
          enableHighAccuracy: true,
          timeout: 10000
        }
      );
    } else {
      this.locationMessage = 'Geolocation is not supported by this browser.';

      // ✅ fallback
      this.getLocationData = {
        lat: '',
        lon: ''
      };

      if (callback) callback();
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
  getPlaceDetails(latitude: number, longitude: number, callback?: () => void) {
    const geocodeUrl = `https://nominatim.openstreetmap.org/reverse?lat=${latitude}&lon=${longitude}&format=json`;
    fetch(geocodeUrl)
      .then(response => response.json())
      .then(data => {
        if (data && data.address) {
          this.getLocationData = data;
          this.getLoginDashboardLocation = data;

          // ✅ Run callback if provided
          if (callback) callback();
        } else {
          console.error('Unable to retrieve location data');
        }
      })
      .catch(error => {
        console.error('Error with reverse geocoding:', error);
      });
  }
  //////////// This is Geolocation //////////////
  LoginDashboardTime: any;
  LoginDashboardtoday: any;
  loginDetailsData: any

  onLoginClick(): void {
    this.getLocation(() => {
      this.loginFunction();
    });
  }

  // Call logout after getting location
  onLogoutClick(): void {
    this.getLocation(() => {
      this.logoutFunction();
    });
  }

  // Login function (unchanged payload, just waits for location)
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
      LoginCity: this.getLocationData?.address?.city,
      Purpose: "",
      LogoutLatitude: '',
      LogoutLongitude: '',
      LogoutAddress: '',
      LogoutCity: '',
      LogOutTime: '',
      LogoutDescription: '',
    };

    console.log('Login Payload:', reqBody);
    this.isSpinner = true;

    this.attendanceService.EmployeeAddOnSiteData(reqBody).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (!res['Message']) {
          this.isLoggedIn = true;
          this.loginDetailsData = res;
          this.getEmployeeDetails();
        }
      },
      error: (err: any) => {
        this.isSpinner = false;
        console.error('Login error:', err);
      }
    });
  }

  // Logout function (unchanged payload, just waits for location)
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

    console.log('Logout Payload:', formData);
    this.isSpinner = true;

    this.attendanceService.EmployeeAddOnSiteData(formData).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (!res['Message']) {
          this.isLoggedIn = false;
          this.getEmployeeDetails();
        }
      },
      error: (err: any) => {
        this.isSpinner = false;
        console.error('Logout error:', err);
      }
    });
  }


  // getEmployeeDetails() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     UserName: this.employeeDetails[0].UserName
  //   };
  //   this.hrmsService.GetEmployeeDetails(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res) {
  //         sessionStorage.setItem('employeeDetails', JSON.stringify(res));
  //         window.location.reload();
  //       }
  //     },
  //     error: (err) => {
  //     },
  //     complete: () => {
  //       // Optional: Code to execute on completion, if any
  //     }
  //   });
  // }

  getEmployeeDetails() {
  const reqBody = {
    EmpId: this.employeeDetails[0].EmpId,
    UserName: this.employeeDetails[0].UserName
  };

  this.hrmsService.GetEmployeeDetails(reqBody).subscribe({
    next: (res: any) => {
      if (res) {
        // ✅ save
        sessionStorage.setItem('employeeDetails', JSON.stringify(res));

        // ✅ update instantly (NO reload)
        this.employeeDetails = res;
        this.isLoggedIn = res[0]?.OnSiteStatus === 'LOGIN';
      }
    }
  });
}

  onEntityChange(leId: any) {
    console.log('Selected Legal Entity ID:', leId);
    this.entityStateService.setEntityId(leId);
    // this.route.navigate(['/dashboard']);
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const newPassword = formGroup.get('newPassword')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    if (newPassword && confirmPassword && newPassword !== confirmPassword) {
      return { mismatch: true };
    }
    return null;
  }

  toggleNewPasswordVisibility(): void {
    this.newPasswordFieldType = this.newPasswordFieldType === 'password' ? 'text' : 'password';
  }

  toggleConfirmPasswordVisibility(): void {
    this.confirmPasswordFieldType = this.confirmPasswordFieldType === 'password' ? 'text' : 'password';
  }

  submitForm() {
    this.isFormSubmitted = true;
  }

  onModalClose() {
    this.changePasswordForm.reset();
    this.isFormSubmitted = false;
  }

  // public method
  onChatToggle(friendID: number) {
    this.friendId = friendID;
    this.chatMessage = !this.chatMessage;
  }
  logout() {
    const reqbody = {
      UserName: this.userDetails.UserName,
      TokenId: this.userDetails.TokenId,
      AuthKey: this.userDetails.UserAuth,
      RoleId: this.employeeDetails[0].DesignationId
    }
    console.log(reqbody);
    this.hrmsService.logoutApi(reqbody).subscribe((res: any) => {
      if (res['TokenId'] === 'Expired') {
        sessionStorage.removeItem('accessPolicy');
        sessionStorage.removeItem('employeeDetails');
        sessionStorage.removeItem('token');
        sessionStorage.removeItem('userAuth');
        sessionStorage.removeItem('userdata');
        sessionStorage.removeItem('financialYearDetails');
        sessionStorage.setItem('shouldRefresh', 'true');
        // sessionStorage.removeItem('isOnsiteVisit');
        // sessionStorage.clear();
        setTimeout(() => {
          this.route.navigate(['/auth/signin']);
        }, 100);
      } else if (res['Message']) {
        window.alert('Sorry Something went wrong');
      }
    }, error => {
      window.alert('Internal Server Error')
    })

  }


  nagivatePageNew() {
    this.route.navigate(['/holidays'])
  }


}
