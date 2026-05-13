// angular imports
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { ApiService } from './api.service'; // Import your API service
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from 'src/app/HRMS/hrms-service.service';
import { VerifyOtpComponent } from 'src/app/HRMS/verify-otp/verify-otp.component';
import { environment } from 'src/assets/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EmployeeModuleService } from 'src/app/HRMS/service/employee.service';
import { EntityStateService } from 'src/app/HRMS/service/entity-state.service';

@Component({
  standalone: true,
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  imports: [SharedModule, ToastMessageComponent,RouterModule],
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('secondModal') secondModal: any = ElementRef;
  @ViewChild('closeChangeModal') closeChangeModal: any = ElementRef;

  loginForm: any = FormGroup; // Form group for login form
  errorMessage: string | undefined; // Variable to hold error message
  spinner: boolean = false;
  loading: boolean = false;
  showPassword = false;
  loginUserData: any;
  accessDataUrl: any;
  verifyOtpData: any;

  forgotPasswordForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isValidOtp: boolean = false;

  changePasswordForm: any = FormGroup;
  newPasswordFieldType: string = 'password';
  confirmPasswordFieldType: string = 'password';

  constructor(
    private readonly apiService: ApiService,
    private readonly router: Router,
    private readonly fb: FormBuilder,
    private readonly hrmsService: HrmsServiceService,
    private readonly http: HttpClient,
    private readonly employeeModuleService: EmployeeModuleService,
    private entityStateService: EntityStateService
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required], // Username field with required validation
      password: ['', Validators.required], // Password field with required validation
    });
    this.forgotPasswordForm = this.fb.group({
      username: ['', [Validators.required]],
      emailId: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      otp: ['']
    });
    this.changePasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,16}$")]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validator: this.passwordMatchValidator
    })

  }

  passwordMatchValidator(formGroup: FormGroup) {
    const newPassword = formGroup.get('newPassword')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    if (newPassword && confirmPassword && newPassword !== confirmPassword) {
      return { mismatch: true };
    }
    return null;
  }

  onPaste(event: ClipboardEvent): void {
    event.preventDefault();
  }

  toggleNewPasswordVisibility(): void {
    this.newPasswordFieldType = this.newPasswordFieldType === 'password' ? 'text' : 'password';
  }

  toggleConfirmPasswordVisibility(): void {
    this.confirmPasswordFieldType = this.confirmPasswordFieldType === 'password' ? 'text' : 'password';
  }


  ngOnInit(): void {
    if (sessionStorage.getItem('shouldRefresh')) {
      window.location.reload();
      sessionStorage.removeItem('shouldRefresh');
    }
    if (this.router.url.includes('/auth/signin')) {
      sessionStorage.removeItem('accessPolicy');
      sessionStorage.removeItem('employeeDetails');
      sessionStorage.removeItem('token');
      sessionStorage.removeItem('userAuth');
      sessionStorage.removeItem('userdata');
      // sessionStorage.removeItem('SelectedLEId');
    }
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  submitForm() {
    this.spinner = true;
    this.loading = true;
    if (this.loginForm.valid) {
      const reqbody = {
        UserName: this.loginForm?.get('username')?.value,
        Password: this.loginForm?.get('password')?.value
      };
      this.apiService.login(reqbody).subscribe({
        next: (res: any) => {
          if (res['msg'] === null) {
            console.log(res);
            this.loginUserData = res;
            sessionStorage.setItem('userdata', JSON.stringify(res));
            this.loginCheckAuth();
          } else if (res['Message']) {
            this.spinner = false;
            this.loading = false;
            this.triggerToast(res['Message'], "Try Again", "warning");
          }
        },
        error: (err) => {
          console.log(err);
          this.spinner = false;
          this.loading = false;
          this.triggerToast("Internal Server Error", "Login...Try Again", "danger");
        },
        complete: () => {
          this.spinner = false;
          this.loading = false;
        }
      });
    } else {
      this.spinner = false;
      this.triggerToast("Invalid", "Please Enter valid Credentials ", "danger");
      this.loading = false;
      this.router.navigate(['/auth/signin'], { replaceUrl: true });
    }
  }

  loginCheckAuth() {
    const reqBody = {
      UserName: this.loginForm?.get('username')?.value,
    }
    this.spinner = true;
    this.loading = true;
    this.hrmsService.LoginCheckAuth(reqBody).subscribe({
      next: (res: any) => {
        if (res['AuthKey'] === 'Success' && res['TokenId'] === 'Success') {
          this.getEmployeeDetails();
        } else {
          this.router.navigate(['/auth/signin'], { replaceUrl: true });
        }
        this.spinner = false;
        this.loading = false;
      }, error: (err) => {
        this.spinner = false;
        this.loading = false;
        this.triggerToast("Someting Went Wrong", "Internal Server Error.. Try Again", "danger");
      }
    })
  }

  getFinancialYear: any;
  financialyear() {
    const reqBody = {
      EmpId: this.loginUserData.EmpId,
    };
    this.apiService.getFYearDetails(reqBody).subscribe({
      next: (res: any) => {
        // console.log(res);
        sessionStorage.setItem('financialYearDetails', JSON.stringify(res));
      }, error: (err: any) => {
        console.log(err);
      }
    })
  }


  storeGetEmployeeDetails: any;
  showEntityDropdown: boolean = false;
  getLegalEntity: any;
  // getEmployeeDetails() {
  //   const reqBody = {
  //     EmpId: this.loginUserData.EmpId,
  //     UserName: this.loginUserData.UserName
  //   };
  //   this.hrmsService.GetEmployeeDetails(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res) {
  //         this.storeGetEmployeeDetails = res;
  //         sessionStorage.setItem('employeeDetails', JSON.stringify(res));
  //         this.spinner = false;
  //         this.loading = false;

  //         //  if (res[0].CPwd === true) {
  //         //     this.router.navigate(['/change_password']);
  //         //   } else {
  //         //     this.router.navigate(['/dashboard']);
  //         //     console.log('dashboard');
  //         //     this.financialyear();
  //         //   }

  //         console.log(res[0].AuthorisedEntity !== null && res[0].AuthorisedEntity !== '');
  //         if (res[0].AuthorisedEntity !== null && res[0].AuthorisedEntity !== '') {
  //           this.showEntityDropdown = true;
  //           this.loginForm.get('username')?.disable();
  //           this.loginForm.get('password')?.disable();
  //           this.callAuthorizedEntityAPI();
  //         } else {
  //           if (res[0].CPwd === true) {
  //             this.router.navigate(['/change_password']);
  //           } else {
  //             this.router.navigate(['/dashboard']);
  //             console.log('dashboard');
  //             this.financialyear();
  //           }
  //         }
  //       } else {
  //         console.log('still login page');
  //       }
  //     },
  //     error: (err) => {
  //       this.spinner = false;
  //       this.loading = false;
  //       this.triggerToast("Internal Server Error", "Get EmployeeDetails", "danger");
  //     },
  //     complete: () => {
  //       // Optional: Code to execute on completion, if any
  //       this.spinner = false;
  //       this.loading = false;
  //     }
  //   });
  // }
 callAuthorizedEntityAPI() {
    const reqBody = {
      EmpId: this.storeGetEmployeeDetails[0].EmpId,
      CompId: 1,
      AuthorisedEntity: this.storeGetEmployeeDetails[0].AuthorisedEntity,
    };
    this.employeeModuleService.employeeDDLegalEntity(reqBody).subscribe({
      next: (res: any) => {
        this.getLegalEntity = res;
      }, error: (err: any) => {
        alert("Internal Server Error! Please try again.");
      }
    })
  }

  selectedEntity: any;
  onEntityChange(value: any) {
    this.selectedEntity = value.target.value;
    console.log("Dropdown changed:", this.selectedEntity); // <-- this works
  }
  getEmployeeDetails() {
    const reqBody = {
      EmpId: this.loginUserData.EmpId,
      UserName: this.loginUserData.UserName
    };

    this.hrmsService.GetEmployeeDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res && res.length > 0) {
          this.storeGetEmployeeDetails = res;
          sessionStorage.setItem('employeeDetails', JSON.stringify(res));

          this.spinner = false;
          this.loading = false;

          const authorisedEntity = res[0].AuthorisedEntity;

          // CASE 1: Multiple authorised entities → show dropdown
          if (
            authorisedEntity &&
            authorisedEntity.split(',').map((e: any) => e.trim()).length > 1
          ) {
            this.showEntityDropdown = true;
            this.loginForm.get('username')?.disable();
            this.loginForm.get('password')?.disable();
            this.callAuthorizedEntityAPI();
          }

          // CASE 2: Single / null / empty authorised entity → auto-set if single & navigate
          else {
            // Auto-set entity when exactly ONE exists (eg: "1")
            if (authorisedEntity && authorisedEntity.split(',').length === 1) {
              const singleEntityId = Number(authorisedEntity.trim());

              if (!isNaN(singleEntityId)) {
                this.entityStateService.setEntityId(singleEntityId);
                console.log('Auto-selected Entity:', singleEntityId);
              }
            }

            // Normal navigation flow
            if (res[0].CPwd === true) {
              this.router.navigate(['/change_password']);
            } else {
              this.router.navigate(['/dashboard']);
              console.log('dashboard');
              this.financialyear();
            }
          }
        } else {
          console.log('still login page');
        }
      },

      error: (err) => {
        this.spinner = false;
        this.loading = false;
        this.triggerToast(
          "Internal Server Error",
          "Get EmployeeDetails",
          "danger"
        );
      },

      complete: () => {
        this.spinner = false;
        this.loading = false;
      }
    });
  }

 

  finalSubmit() {
    if (!this.selectedEntity || this.selectedEntity === '') {
      window.alert("Please select a Legal Entity before submitting.");
      return;
    }
    this.entityStateService.setEntityId(Number(this.selectedEntity));
    if (this.storeGetEmployeeDetails[0].CPwd === true) {
      this.router.navigate(['/change_password']);
    } else {
      this.router.navigate(['/dashboard']);
      console.log('dashboard');
      this.financialyear();
    }
  }


  getOtp() {
    this.isFormSubmitted = true;
    if (this.forgotPasswordForm.valid) {
      const reqBody = {
        UserName: this.forgotPasswordForm?.get('username')?.value,
        Email: this.forgotPasswordForm?.get('emailId')?.value
      };
      this.apiService.LoginForgetPassword(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg'] === 'OTP Send successfully') {
            this.isValidOtp = true;
            this.forgotPasswordForm?.get('username').disable();
            this.forgotPasswordForm?.get('emailId').disable();
            this.triggerToast('success', 'OTP Has Sent Successfully To Your Provide Mail Id Please Check', 'success')
          } else if (res['StatusCode'] === 404) {
            this.triggerToast('warning', res['Message'], 'warning');
            this.isValidOtp = false;
            this.forgotPasswordForm?.get('username').enable();
            this.forgotPasswordForm?.get('emailId').enable();
          }
        }, error: (error: any) => {
          console.log(error);
          this.triggerToast('Something Went Wrong', 'Internal Server Error.. Try Again', 'danger');
        }
      })
    } else {
      this.isValidOtp = false;
    }
  }

  forgotFormSubmit() {
    const reqBody = {
      UserName: this.forgotPasswordForm?.get('username')?.value,
      Email: this.forgotPasswordForm?.get('emailId')?.value,
      Otp: this.forgotPasswordForm?.get('otp')?.value
    };
    this.apiService.LoginFPwdVerify(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res['msg']) {
          this.verifyOtpData = res
          this.secondModal?.nativeElement?.click();
        } else if (res['Message']) {
          this.triggerToast('warning', res['Message'], 'warning');
        }
      }, error: (error: any) => {
        console.log(error);
        this.triggerToast('Something Went Wrong', 'Internal Server Error.. Try Again', 'danger');
      }
    })
  }
  changeSubmitForm() {
    if (this.changePasswordForm.valid) {
      const reqBody = {
        EmpId: this.verifyOtpData.EmpId,
        EmpCode: this.verifyOtpData.EmpCode,
        FPwd: true,
        CPwd: false,
        OldPassword: "",
        NewPassword: this.changePasswordForm?.get('confirmPassword').value,
      }
      this.hrmsService.LoginChangePassword(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg']) {
            this.closeChangeModal.nativeElement?.click();
          } else if (res['Message']) {
            this.triggerToast('warning', res['Message'], 'warning');
          }
        }, error: (error: any) => {
          this.triggerToast('Something Went Wrong', 'Internal Server Error.. Try Again', 'danger');
        }
      })
    } else {
      this.isFormSubmitted = true;
    }
  }
  onModalClose() {
    this.forgotPasswordForm.reset();
    this.isFormSubmitted = false;
    this.isValidOtp = false;
  }

  onModalCloseSecond() {
    this.changePasswordForm.reset();
    window.location.reload()
  }



  triggerToast(header: any, body: any, mess: any) {
    // const header = 'Toast Header';
    // const body = 'This is a toast message.';
    this.toastMessageComponent.showToast(header, body, mess);
  }

  handleAlphaChar(event: any) {
    if (
      (event.charCode > 32 && event.charCode < 48) ||
      (event.charCode > 57 && event.charCode < 127)
    ) {
      event.preventDefault();
    }
  }
}