import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { RouterModule } from '@angular/router';
import { EmployeeModuleService } from '../../service/employee.service';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-nominee-details',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent, NgxPaginationModule, RouterModule],
  templateUrl: './nominee-details.component.html',
  styleUrl: './nominee-details.component.scss'
})
export class NomineeDetailsComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  maxNominees = 3;
  isViewtOthers: boolean[] = [];
  isSpinner: boolean = false;
  nomineeForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  today = new Date().toISOString().split('T')[0];
  showSecondaryNominee = false;
  secondaryNomineeForm: any = FormGroup;
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsService: EmployeeModuleService,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Financial Details'
      );
    });
  }

  ngOnInit(): void {
    this.nomineeForm = this.fb.group({
      nominees: this.fb.array([])
    });

    // Initialize with one nominee
    this.addNominee();
  }

  get nominees(): FormArray {
    return this.nomineeForm.get('nominees') as FormArray;
  }

  createNomineeFormGroup(): FormGroup {
    const group = this.fb.group({
      fullName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      relationshipToEmp: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      DOB: ['', [Validators.required]],
      Gender: ['', [Validators.required]],
      contactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      email: ['', [Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      percentageShare: ['', [Validators.required]],
      identificationProof: [''],
      Others: [''],
      IdentificationProofNum: [''],
    });

    const index = this.nominees.length;
    this.isViewtOthers.push(false);

    const idProofControl = group.get('identificationProof');
    if (idProofControl) {
      idProofControl.valueChanges.subscribe((val: any) => {
        this.isViewtOthers[index] = val === 'Others';
      });
    }
    return group;
  }

  addNominee(): void {
    if (this.nominees.length < this.maxNominees) {
      this.nominees.push(this.createNomineeFormGroup());
    }
  }

  removeNominee(index: number): void {
    if (this.nominees.length > 1) {
      this.nominees.removeAt(index);
      this.isViewtOthers.splice(index, 1);
    }
  }

  convertToUppercase(event: any, controlName: string) {
    const value = event.target.value.toUpperCase();
    this.nomineeForm.get(controlName)?.setValue(value, { emitEvent: false });
  }

  submitForm() {
    this.isFormSubmitted = true;
    const payload = this.nomineeForm.value;
    console.log(payload);
    if (this.nomineeForm.valid) {
      console.log("Submitted Data:", this.nomineeForm.value);
      // You can post this.nomineeForm.value to API here
    } else {
      console.warn("Form is invalid");
    }
  }
  resetData() {
    this.isFormSubmitted = false;
    this.nomineeForm.reset();
  }
  triggerToast(header: any, body: any, mess: any) {
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
