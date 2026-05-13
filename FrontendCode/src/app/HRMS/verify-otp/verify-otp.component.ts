import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgOtpInputModule } from 'ng-otp-input';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Config } from 'ng-otp-input/lib/models/config';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { HrmsServiceService } from '../hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [SharedModule, CommonModule, NgOtpInputModule, ToastMessageComponent],
  templateUrl: './verify-otp.component.html',
  styleUrl: './verify-otp.component.scss'
})
export class VerifyOtpComponent implements OnInit, OnDestroy {
  config: Config = {
    length: 6,
    allowNumbersOnly: true,
    isPasswordInput: false,
    inputStyles: {
      width: '25px',
      height: '30px',
      'border-radius': '3px',
      'font-size': '15px',
      'cursor': 'pointer',
      'margin-right': '2px',
      'margin-top': '10px',
    },
  };
  otpForm: any = FormGroup;
  otp: FormControl = new FormControl('', [Validators.required]);
  otpSubscription: any = Subscription;
  isSpinner: boolean = false;
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  constructor(private readonly hrmsService: HrmsServiceService, private readonly route: Router) { }

  ngOnInit(): void {
    this.otpForm = new FormGroup({
      otp: this.otp,
    });
    this.otpSubscription = this.otpForm.get('otp').valueChanges.subscribe((value: any) => {
      this.checkOtpValidity(value);
    });
  }

  checkOtpValidity(value: string) {
    if (value && value.length === 6) {
      this.otpForm.get('otp').setErrors(null);
    } else {
      this.otpForm.get('otp').setErrors({ 'invalid': true });
    }
  }

  onSubmit() {
    if (this.otpForm.valid) {
      const reqBody = {
        otp: this.otpForm.get('otp').value
      }
      this.isSpinner = true;
      this.hrmsService.visitorVerifyOTP(reqBody).subscribe((res: any) => {
        if ((res['msg'] === 'OTP Verified Successfully')) {
          this.route.navigate(['visitor_page'], {
            queryParams: res
          });
          this.isSpinner = false;
        } 
        else if (this.otpForm.valid) {
          this.hrmsService.visitorVerifyOTPCheckIn(reqBody).subscribe((res: any) => {
            if ((res['msg'] === 'OTP Verified Successfully') && ((res['VisitorCheckIn'] === false || res['VisitorCheckOut'] === false))) {
              this.route.navigate(['visitor_checkin'], {
                queryParams: res
              });
              this.isSpinner = false;
            } else {
              if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], "warning");
                this.isSpinner = false;
              }
            }
          }, error => {
            this.triggerToast(error['Message'], 'Internal Server Error', "danger");
            this.isSpinner = false;
          })
        } 
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner = false;
      })

    } else {
      this.triggerToast("Invalid", "Please Enter valid Credentials ", "danger");
      this.isSpinner = false;
    }
  }

  ngOnDestroy() {
    if (this.otpSubscription) {
      this.otpSubscription.unsubscribe();
    }
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
