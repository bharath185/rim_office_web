import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { payRollService } from '../../service/payroll.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-pay-slip-section',
  standalone: true,
  imports: [SharedModule, CommonModule, ToastMessageComponent,
    ReactiveFormsModule, NgxPaginationModule],
  animations: [
    trigger('stepAnimation', [
      state('hidden', style({ opacity: 0, transform: 'translateX(-20px)', display: 'none' })),
      state('visible', style({ opacity: 1, transform: 'translateX(0)', display: 'block' })),
      transition('hidden => visible', [style({ display: 'block' }), animate('300ms ease-in')]),
      transition('visible => hidden', [animate('300ms ease-out', style({ opacity: 0, transform: 'translateX(20px)' }))]),
    ]),
  ],
  templateUrl: './pay-slip-section.component.html',
  styleUrl: './pay-slip-section.component.scss'
})
export class PaySlipSectionComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;

  employeeDetails;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any
  isSpinner1: boolean = false;
  getDDOfPayoutType: any = [];
  getRowsTableData: any = [];
  earningForm: any = FormGroup;
  deductionForm: any = FormGroup;
  summaryForm: any = FormGroup;
  payoutForm: any = FormGroup;
  currentStep = 1;
  isEditable = true;
  getDDOfComponent: any = [];
  selectedPayoutType: any = null;
  selectedPayout: any;
  minStartDate!: string;
  maxStartDate!: string;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  isTableData: boolean = false;
  errorMessage: any;
  isEdited: boolean = false;
  isRecordDeletedCommon: boolean = false;

  constructor(private payrollService: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly fb: FormBuilder) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Salary Management'
      );
    });
    const currentDate = new Date();
    this.minStartDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, currentDate.getDate()));
    const nextYear = new Date(currentDate.getFullYear() + 1, currentDate.getMonth(), currentDate.getDate());
    this.maxStartDate = this.formatDate(nextYear);
  }
  ngOnInit(): void {
    this.dropdownPayoutType();
    setTimeout(() => {
      this.getAllPayslipSectionComponent();
    }, 100);
    this.earningForm = this.fb.group({
      components: this.fb.array([]),
    });
    this.deductionForm = this.fb.group({
      components: this.fb.array([]),
    });
    this.summaryForm = this.fb.group({
      components: this.fb.array([]),
    });
    this.payoutForm = this.fb.group({
      PayoutTypeId: [''],
      date_from: [''],
      date_to: [''],
    })
    this.addComponentRow('earning');
    this.addComponentRow('deduction');
    this.addComponentRow('summary');
  }
  onPayoutTypeChange(value: any) {
    this.selectedPayoutType = value;
    this.getDDPayrollComponent();
  }
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }
  onFromDate(): void {
    const fromDate = this.payoutForm.get('date_from')?.value;
    if (fromDate) {
      this.minStartDate = fromDate;
    }
  }
  onToDate(): void {
    const toDate = this.payoutForm.get('date_to')?.value;
    if (toDate) {
      this.maxStartDate = toDate;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.payoutForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.payoutForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.payoutForm.hasError('dateRange');
  }
  getComponents(step: string): FormArray {
    switch (step) {
      case 'earning':
        return this.earningForm.get('components') as FormArray;
      case 'deduction':
        return this.deductionForm.get('components') as FormArray;
      case 'summary':
        return this.summaryForm.get('components') as FormArray;
    }
    return this.fb.array([]);
  }
  createComponentRow(seq: number): FormGroup {
    return this.fb.group({
      componentDD: ['', Validators.required],
      SequenceNo: [seq, Validators.required]
    });
  }
  addComponentRow(step: string) {
    const arr = this.getComponents(step);
    if (arr.length >= 20) {
      alert('Maximum 20 components allowed.');
      return;
    }
    const seq = arr.length + 1;
    arr.push(this.createComponentRow(seq));
  }
  removeRow(step: string, i: number) {
    const arr = this.getComponents(step);
    arr.removeAt(i);
    arr.controls.forEach((ctrl, idx) => {
      ctrl.get('SequenceNo')?.setValue(idx + 1);
    });
  }
  isComponentDisabled(step: string, component: any, rowIndex: number): boolean {
    const arr = this.getComponents(step);
    const selectedValues = arr.controls.map((ctrl, idx) =>
      idx !== rowIndex ? Number(ctrl.get('componentDD')?.value) : null
    );
    return selectedValues.includes(Number(component.ComponentId));
  }

  nextStep() {
    if (this.currentStep === 1 && this.earningForm.invalid) {
      this.isFormSubmitted = true;
      return;
    }
    if (this.currentStep === 2 && this.deductionForm.invalid) {
      this.isFormSubmitted = true;
      return;
    }
    if (this.currentStep === 3 && this.summaryForm.invalid) {
      this.isFormSubmitted = true;
      return;
    }
    this.currentStep++;
    this.isFormSubmitted = false;
  }
  prevStep() {
    this.currentStep--;
  }
  submitFinalForm() {
    const getId = (comp: any) =>
      typeof comp.componentDD === 'number'
        ? comp.componentDD
        : comp.componentDD?.ComponentId;

    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: this.payoutForm.value.PayoutTypeId,
      EffectiveDateFrom: this.payoutForm.value.date_from,
      EffectiveDateTo: this.payoutForm.value.date_to,

      Sections: [
        {
          SectionName: "EARNINGS",
          Components: this.earningForm.getRawValue().components.map((c: any) => ({
            ComponentId: getId(c),
            SequenceNo: c.SequenceNo
          }))
        },
        {
          SectionName: "DEDUCTIONS",
          Components: this.deductionForm.getRawValue().components.map((c: any) => ({
            ComponentId: getId(c),
            SequenceNo: c.SequenceNo
          }))
        },
        {
          SectionName: "SUMMARY",
          Components: this.summaryForm.getRawValue().components.map((c: any) => ({
            ComponentId: getId(c),
            SequenceNo: c.SequenceNo
          }))
        }
      ]
    };
    console.log(payload);
    this.isSpinner = true;
    this.payrollService.AddPayslipSectionComponent(payload).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isSpinner = false;
          this.getAllPayslipSectionComponent();
          this.resetStepper();
          this.selectedPayoutType = false;
          this.payoutForm.reset();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
          this.isSpinner = false;
        }
      }, error: (err: any) => {
        this.triggerToast('Failed To Add Record', 'Internal Server Error', 'danger');
        this.isSpinner = false;
      }
    })
  }

  hasComponents(payout: any): boolean {
    return payout?.Sections?.some((sec: any) => sec?.Components?.length > 0);
  }


  editPayout(payout: any, edited: boolean) {
    console.log('Edit Payout:', payout); // ✅ For debugging
    this.isEdited = edited;
    // Patch payout form
    this.payoutForm.patchValue({
      PayoutTypeId: payout.PayoutTypeId,
      date_from: this.formatDate(new Date(parseInt(payout.Sections[0].Components[0].EffectiveFrom.replace(/\D/g, '')))),
      date_to: this.formatDate(new Date(parseInt(payout.Sections[0].Components[0].EffectiveTo.replace(/\D/g, ''))))
    });

    this.selectedPayoutType = payout.PayoutTypeId;
    this.getDDPayrollComponent();

    // Reset FormArrays
    ['earning', 'deduction', 'summary'].forEach(section => {
      const formArray = this.getComponents(section);
      formArray.clear();
    });

    // Patch components to FormArrays
    payout.Sections.forEach((section: any) => {
      const sectionName = section.SectionName.toLowerCase();
      section.Components.forEach((comp: any, index: number) => {
        const formArray = this.getComponents(sectionName === 'earnings' ? 'earning' : sectionName === 'deductions' ? 'deduction' : 'summary');
        formArray.push(this.fb.group({
          SectionComponentId: [comp.SectionComponentId],
          componentDD: [comp.ComponentId, Validators.required],
          SequenceNo: [comp.SequenceNo, Validators.required]
        }));
      });
    });
    this.currentStep = 1; // reset stepper to first step
  }
  updateAddForm() {
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: this.payoutForm.value.PayoutTypeId,
      EffectiveDateFrom: this.payoutForm.value.date_from,
      EffectiveDateTo: this.payoutForm.value.date_to,

      Sections: [
        {
          SectionName: "EARNINGS",
          Components: this.earningForm.getRawValue().components.map((c: any) => ({
            SectionComponentId: c.SectionComponentId,
            ComponentId: Number(c.componentDD),
            SequenceNo: Number(c.SequenceNo)
          }))
        },
        {
          SectionName: "DEDUCTIONS",
          Components: this.deductionForm.getRawValue().components.map((c: any) => ({
            SectionComponentId: c.SectionComponentId,
            ComponentId: Number(c.componentDD),
            SequenceNo: Number(c.SequenceNo)
          }))
        },
        {
          SectionName: "SUMMARY",
          Components: this.summaryForm.getRawValue().components.map((c: any) => ({
            SectionComponentId: c.SectionComponentId,
            ComponentId: Number(c.componentDD),
            SequenceNo: Number(c.SequenceNo)
          }))
        }
      ]
    };
    this.isSpinner = true;
    this.payrollService.UpdatePayslipSectionComponent(payload).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isSpinner = false;
          this.getAllPayslipSectionComponent();
          this.resetStepper();
        } else {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
          this.isSpinner = false;
        }
      }, error: (err: any) => {
        this.triggerToast('', 'Internal Server Error', 'danger');
        this.isSpinner = false;
      }
    })
    // console.log(payload);
  }


  deletePayout(payout: any) {
    console.log(payout);
    this.selectedPayout = payout;
  }
  convertDate(dotNetDate: string): string {
    const timestamp = Number(dotNetDate.replace('/Date(', '').replace(')/', ''));
    const dateObj = new Date(timestamp);
    return dateObj.toISOString().split('T')[0]; // YYYY-MM-DD
  }

  deleteRecord() {
    if (!this.selectedPayout) {
      this.triggerToast('No record selected', 'Error', 'danger');
      return;
    }
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: this.selectedPayout.PayoutTypeId,
      EffectiveDateFrom: this.convertDate(this.selectedPayout.Sections[0].Components[0].EffectiveFrom),
      EffectiveDateTo: this.convertDate(this.selectedPayout.Sections[0].Components[0].EffectiveTo),
      Sections: this.selectedPayout.Sections.map((sec: any) => ({
        SectionName: sec.SectionName,
        Components: sec.Components.map((c: any) => ({
          SectionComponentId: c.SectionComponentId
        }))
      }))
    };
    // console.log("DELETE PAYLOAD", payload);
    this.isSpinner1 = true;
    this.payrollService.DeletePayslipSectionComponent(payload).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isRecordDeletedCommon = true;
          setTimeout(() => {
            this.closeModalDelete.nativeElement?.click();
            this.getAllPayslipSectionComponent();
            setTimeout(() => this.isRecordDeletedCommon = false, 1100);
          }, 1000);
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
        }
        this.isSpinner1 = false;
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
        this.isSpinner1 = false;
      }
    });
  }

  resetStepper() {
    this.currentStep = 1;
    [this.earningForm, this.deductionForm, this.summaryForm].forEach((form) =>
      form.reset({ components: [] })
    );
    this.addComponentRow('earning');
    this.addComponentRow('deduction');
    this.addComponentRow('summary');
  }
  dropdownPayoutType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.DDPayrollPayoutType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfPayoutType = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Payout Type", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('PayoutType', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }
  getDDPayrollComponent() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: this.payoutForm?.get('PayoutTypeId').value,
    }
    this.isSpinner = true;
    this.payrollService.DDPayrollComponent(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfComponent = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Payout Type", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Component Value', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }

  getAllPayslipSectionComponent() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.payrollService.GetAllPayslipSectionComponent(reqBody).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (res && res.length > 0) {
          this.getRowsTableData = res;
          this.isTableData = false;
        } else {
          this.isTableData = true;
          this.errorMessage = "No Data Found";
        }
      },
      error: () => {
        this.isSpinner = false;
        this.isTableData = true;
        this.errorMessage = "Internal Server Error";
      }
    });
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}







