import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Modal } from 'bootstrap';
import { HrmsServiceService } from '../hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, ToastMessageComponent],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent implements OnInit {
  @ViewChild('changePassword', { static: true }) changePasswordModal: any;
  @ViewChild('closeModal') closeModal: any = ElementRef;
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  changePasswordForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  newPasswordFieldType: string = 'password';
  confirmPasswordFieldType: string = 'password';
  oldPasswordFieldType: string = 'password'
  modalElement: any;
  employeeDetails;

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsService: HrmsServiceService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
  }
  ngOnInit(): void {
    this.changePasswordForm = this.fb.group({
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,16}$")]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validator: this.passwordMatchValidator
    });
  }

  ngAfterViewInit(): void {
    this.modalElement = new Modal(this.changePasswordModal.nativeElement);
    this.modalElement.show();
  }
  passwordMatchValidator(formGroup: FormGroup) {
    const newPassword = formGroup.get('newPassword')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    if (newPassword && confirmPassword && newPassword !== confirmPassword) {
      return { mismatch: true };
    }
    return null;
  }

  changeSubmitForm() {
    if (this.changePasswordForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.employeeDetails[0].EmpCode,
        FPwd: false,
        CPwd: true,
        OldPassword: this.changePasswordForm?.get('oldPassword').value,
        NewPassword: this.changePasswordForm?.get('confirmPassword').value,
      }
      this.hrmsService.LoginChangePassword(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg'] === 'Password Changed') {
            this.closeModal.nativeElement?.click();
            // window.alert('Password Changed Successfully');
            this.triggerToast('Success','Password Changed Successfully','success');
          } else if (res['Message']) {
            this.triggerToast('Failed',res['Message'],'warning');
          }
        }, error: (error: any) => {
          this.triggerToast('Something Went Wrong', 'Internal Server Error, Try Again', 'danger');
        }
      })
    } else {
      this.isFormSubmitted = true;
    }
  }

  toggleOldPasswordVisibility(): void {
    this.oldPasswordFieldType = this.oldPasswordFieldType === 'password' ? 'text' : 'password';
  }

  toggleNewPasswordVisibility(): void {
    this.newPasswordFieldType = this.newPasswordFieldType === 'password' ? 'text' : 'password';
  }

  toggleConfirmPasswordVisibility(): void {
    this.confirmPasswordFieldType = this.confirmPasswordFieldType === 'password' ? 'text' : 'password';
  }

  onPaste(event: ClipboardEvent): void {
    event.preventDefault();
  }


  onModalClose() {
    this.changePasswordForm.reset();
    this.isFormSubmitted = false;

  }

  ngOnDestroy(): void {
    // Close the modal if it's open when navigating away from the component
    if (this.modalElement) {
      this.modalElement?.hide();
    }
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
