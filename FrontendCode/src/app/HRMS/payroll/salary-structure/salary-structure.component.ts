import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { payRollService } from '../../service/payroll.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { Modal } from 'bootstrap';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { number } from 'echarts';


interface DynamicInput {
  type: 'value' | 'symbol' | 'component';
  value: string | number;
}

const deleteKeyMap: any = {
  PayoutType: ['PayoutTypeId'],
  Segment: ['PayoutTypeId', 'SegmentId']
};

@Component({
  selector: 'app-salary-structure',
  standalone: true,
  imports: [SharedModule, CommonModule, ToastMessageComponent,
    ReactiveFormsModule, NgxPaginationModule],
  templateUrl: './salary-structure.component.html',
  styleUrl: './salary-structure.component.scss'
})
export class SalaryStructureComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;

  employeeDetails;
  isSpinner: boolean = false;
  getDDOfPayoutType: any = [];
  getDDOfSegment: any = [];
  getDDymbols: any = [];
  getDDOfFrequency: any = [];
  getDDOfComponent: any = [];
  getListOfPayoutType: any = [];
  getListOfSegment: any = [];
  currentAddType: 'PayoutType' | 'Segment' | null = null;
  addForm: any = FormGroup;
  componetForm: any = FormGroup;
  isFormSubmittedAddForm: boolean = false;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any;
  isEdited: boolean = false;
  isSpinner1: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  disableValueComponent: boolean = false;
  disableSymbol: boolean = true;
  showSymbolButton: boolean = false;
  isTableData: boolean = false;
  rows: any[] = [];
  errorMessage: any;
  getSelectedPayoutTypeValue: any = number;
  getmodalPayoutDDValue: any

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
  }

  ngOnInit(): void {
    this.dropdownPayoutType();
    setTimeout(() => {
      setTimeout(() => {
        this.getAllComponentDetails();
        this.getDropDownSymbols();
      }, 1000);
    }, 100)
    this.addForm = this.fb.group({
      payoutTypeName: ['', [Validators.required]],
      Frequency: ['', [Validators.required]],

      segmentOfPayout: ['', [Validators.required]],
      SegmentName: ['', [Validators.required]]
    })

    this.componetForm = this.fb.group({
      compFormPayoutType: ['', [Validators.required]],
      compFormSegment: ['', [Validators.required]],
      compFormComponetName: ['', [Validators.required]],
      compFormComponentCode: ['', [Validators.required]],

      amount: [null],
      percentage: [null],
      compFormComponetDD: [null,],

      conditions: this.fb.array([])
    })
  }

  get conditions(): FormArray<FormGroup> {
    return this.componetForm.get('conditions') as FormArray<FormGroup>;
  }

  onAmountChange() {
    const amount = this.componetForm.get('amount')?.value;
    if (amount !== null && amount !== '') {
      this.componetForm.get('percentage')?.setValue(null);
    }
  }
  onPercentageChange() {
    const percentage = this.componetForm.get('percentage')?.value;
    if (percentage !== null && percentage !== '') {
      this.componetForm.get('amount')?.setValue(null);
    }
  }

  // Add input
  addInput(type: 'value' | 'symbol' | 'component') {
    if (this.conditions.length >= 9) return;
    this.conditions.push(
      this.fb.group({
        type: [type],
        value: [type === 'value' ? null : '']
      })
    );
    // your button enable/disable rules remain same
    if (type === 'value' || type === 'component') {
      this.disableValueComponent = true;
      this.disableSymbol = false;
      this.showSymbolButton = true;
    } else if (type === 'symbol') {
      this.disableValueComponent = false;
      this.disableSymbol = true;
    }
  }

  // Remove input
  removeInput(i: number) {
    this.conditions.removeAt(i);
    const hasValueOrComponent = this.conditions.value.some(
      (input: any) => input.type === 'value' || input.type === 'component'
    );
    this.showSymbolButton = hasValueOrComponent;
    this.disableSymbol = !hasValueOrComponent;
    this.disableValueComponent = false;
  }

  getConditionString(): string {
    if (!this.conditions || this.conditions.length === 0) return '';

    return this.conditions.value
      .map((item: any) => {
        switch (item.type) {
          case 'value':
            return item.value ?? '';
          case 'symbol':
            const symbolObj = this.getDDymbols.find((s: any) => s.SymbolId == item.value);
            return symbolObj ? symbolObj.Symbol : '';
          case 'component':
            const compObj = this.getDDOfComponent.find((c: any) => c.ComponentId == item.value);
            return compObj ? compObj.ComponentName : '';
          default:
            return '';
        }
      })
      .join(' '); // space between each part
  }
  submit() {
    // Step 1: Form validation
    const isFormValid = this.componetForm.valid;
    this.isFormSubmitted = !isFormValid; // mark form as submitted if invalid

    // Step 2: Conditions validation
    const conditions = this.conditions.value;
    let isConditionValid = true;
    let prevType: 'value' | 'symbol' | 'component' | null = null;

    for (let i = 0; i < conditions.length; i++) {
      const curr = conditions[i];
      if (i === 0 && curr.type === 'symbol') isConditionValid = false; // first cannot be symbol
      if (curr.type === 'symbol' && prevType === 'symbol') isConditionValid = false; // no consecutive symbols
      prevType = curr.type;
    }
    if (conditions.length > 0 && conditions[conditions.length - 1].type === 'symbol') {
      isConditionValid = false; // last cannot be symbol
    }

    // Step 3: If either invalid, show alerts and stop
    if (!isFormValid && !isConditionValid) {
      alert('Form is invalid AND conditions are invalid!');
      return;
    } else if (!isFormValid) {
      alert('Form is invalid!');
      return;
    } else if (!isConditionValid) {
      alert('Conditions are invalid!');
      return;
    }
    const selectedComponent = this.componetForm.get('compFormComponetDD')?.value;
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      LoginId: this.employeeDetails[0].LoginId,

      ComponentName: this.componetForm?.get('compFormComponetName')?.value,
      ComponentCode: this.componetForm?.get('compFormComponentCode')?.value,

      PayoutTypeId: Number(this.componetForm?.get('compFormPayoutType')?.value),
      PayoutTypeName: this.componetForm?.get('compFormPayoutType')?.value?.PayoutTypeName || "",

      SegmentId: Number(this.componetForm?.get('compFormSegment')?.value),
      SegmentName: this.componetForm?.get('compFormSegment')?.value?.SegmentName || "",

      lstofLC: [
        {
          Percentage: this.componetForm?.get('percentage')?.value || 0,
          Value: this.componetForm?.get('amount')?.value || 0,

          ComponentId1: selectedComponent?.ComponentId || 0,
          ComponentName1: selectedComponent?.ComponentName || "",

          EffectiveFrom: "",
          EffectiveTo: "",

          ConditionExpression: this.getConditionString(),
          ConditionResultPFESI: ""
        }
      ]
    };

    console.log(reqBody);
    this.isSpinner = true;
    this.payrollService.AddComponent(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], "");
          this.isSpinner = false;
          this.getDDOfComponent = [];
          this.getAllComponentDetails();
          this.getDDPayrollComponent();
          this.resetData();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], "warning");
          this.isSpinner = false;
        }
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'To Load The Components', 'danger');
        this.isSpinner = false;
      }
    })
  }

  getAllComponentDetails() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.payrollService.GetAllComponentDetails(reqBody).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (res && res.length > 0) {
          this.rows = res;
          this.isTableData = false;
        } else {
          this.isTableData = true;
          this.errorMessage = "No records found";
        }
      },
      error: () => {
        this.isSpinner = false;
        this.isTableData = true;
        this.errorMessage = "Internal Server Error";
      }
    });
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
      this.triggerToast('', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }

  getDropDownSegment() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: Number(this.componetForm?.get('compFormPayoutType').value),
    }
    this.isSpinner = true;
    this.getDDOfSegment = [];
    this.payrollService.DDPayrollSegment(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfSegment = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Segment", "warning");
        this.isSpinner = false;
        this.getDDOfSegment = [];
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Segment List', 'danger');
      this.isSpinner = false;
      this.getDDOfSegment = [];
    })
  }

  getDDPayrollComponent() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: Number(this.getSelectedPayoutTypeValue),
    }
    this.isSpinner = true;
    this.getDDOfComponent = []
    this.payrollService.DDPayrollComponent(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfComponent = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Components Type", "warning");
        this.isSpinner = false;
        this.getDDOfComponent = []
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Component Data', 'danger');
      this.isSpinner = false;
      this.getDDOfComponent = []
    })
  }

  getDropDownSymbols() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.DDPayrollSymbols(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDymbols = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Segment", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Symbol List', 'danger');
      this.isSpinner = false;
    })
  }
  resetData() {
    while (this.conditions.length !== 0) {
      this.conditions.removeAt(0);
    }
    this.componetForm.reset();
    this.addForm.reset();
    // 🔥 FORCE RESET DROPDOWNS TO NULL
    this.componetForm.get('compFormPayoutType')?.setValue(null);
    this.componetForm.get('compFormSegment')?.setValue(null);

    this.isFormSubmitted = false;
    this.disableValueComponent = false;
    this.disableSymbol = true;
    this.showSymbolButton = false;
  }

  // ************this is for modal CRUD Payout *****************
  isTableDataPayout: boolean = false;
  errorMessagePayout: any;
  recordToDelete: any = null;

  isTableDataSegment: boolean = false;
  errorMessageSegment: any;

  deleteType: 'PayoutType' | 'Segment' | null = null;
  isRecordDeletedCommon: boolean = false;
  payoutPatchData: any
  segmentPatchData: any

  // openAddModal(type: 'PayoutType' | 'Segment') {
  //   this.currentAddType = type;
  //   const modalElement = document.getElementById('salaryStructureModal');
  //   if (modalElement) {
  //     const modal = new Modal(modalElement);
  //     modal.show();
  //   }
  // }
  openAddModal(type: 'PayoutType' | 'Segment') {
    this.currentAddType = type;

    if (type === 'PayoutType') {
      this.addForm.get('Frequency')?.enable();
      this.addForm.get('payoutTypeName')?.enable();

      this.addForm.get('segmentOfPayout')?.disable();
      this.addForm.get('SegmentName')?.disable();
    } else {
      this.addForm.get('Frequency')?.disable();
      this.addForm.get('payoutTypeName')?.disable();

      this.addForm.get('segmentOfPayout')?.enable();
      this.addForm.get('SegmentName')?.enable();
    }

    const modalElement = document.getElementById('salaryStructureModal');
    if (modalElement) {
      const modal = new Modal(modalElement);
      modal.show();
    }
  }

  closeModal() {
    const modalElement = document.getElementById('salaryStructureModal');
    if (modalElement) {
      const modal = new Modal(modalElement);
      modal.hide();
    }
  }
  onPayoutChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.getSelectedPayoutTypeValue = selectedValue
    if (selectedValue === 'createNew') {
      this.openAddModal('PayoutType');
      this.getAllPayrollPayoutType();
      this.getFrequencyDDType();
    } else {
      this.getDropDownSegment();
      this.getDDPayrollComponent();
    }
  }
  onSegmentChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    if (selectedValue === 'createNew') {
      // GET the payout type selected from main form
      const payoutType = this.componetForm.get('compFormPayoutType')?.value;
      this.getmodalPayoutDDValue = payoutType;
      // PATCH it into modal form
      this.addForm.patchValue({
        segmentOfPayout: payoutType
      });
      this.openAddModal('Segment');
      setTimeout(() => {
        this.addForm.get('segmentOfPayout')?.disable();
      }, 0);
      this.getAllSegmentType();
    }
  }
  closeModalReset() {
    this.addForm?.reset();
    this.isEdited = false;
    this.isFormSubmittedAddForm = false;
    this.resetData();
  }

  submitAddForm() {
    switch (this.currentAddType) {
      case 'PayoutType':
        if (this.addForm?.get('payoutTypeName').valid && this.addForm?.get('Frequency').valid) {
          this.isFormSubmittedAddForm = false;
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            EmpId: this.employeeDetails[0].EmpId,
            PayoutTypeName: this.addForm?.get('payoutTypeName').value,
            Frequency: this.addForm?.get('Frequency').value
          }
          this.isSpinner1 = true;
          this.payrollService.AddPayrollPayoutType(reqBody).subscribe((res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], 'success');
              this.isSpinner1 = false;
              this.closeModalReset();
              this.getAllPayrollPayoutType();
              this.dropdownPayoutType();
              this.getFrequencyDDType();
              this.resetData();
            } else {
              this.triggerToast(res['Message'], "Something went wrong", "warning");
              this.isSpinner1 = false;
            }
          }, error => {
            this.triggerToast('', 'Internal Server Error', 'danger');
            this.isSpinner1 = false;
          })
        } else {
          this.isFormSubmittedAddForm = true;
        }
        break;

      case 'Segment':
        if (this.addForm?.get('SegmentName').valid) {
          console.log(this.addForm.value, 'valid')
          this.isFormSubmittedAddForm = false;
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            EmpId: this.employeeDetails[0].EmpId,
            PayoutTypeId: Number(this.addForm?.get('segmentOfPayout').value),
            SegmentName: this.addForm?.get('SegmentName').value
          }
          this.isSpinner1 = true;
          this.payrollService.AddPayrollSegment(reqBody).subscribe((res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], 'success');
              this.isSpinner1 = false;
              // this.closeModalReset();
              this.addForm.get('SegmentName')?.reset();
              setTimeout(() => {
                this.getAllSegmentType();
              }, 0);
              // this.getDropDownSegment();
              // this.resetData();
            } else {
              this.triggerToast(res['Message'], "Something went wrong", "warning");
              this.isSpinner1 = false;
            }
          }, error => {
            this.triggerToast('', 'Internal Server Error', 'danger');
            this.isSpinner1 = false;
          })
        } else {
          this.isFormSubmittedAddForm = true;
          console.log('not valid', console.log(this.addForm.value))
        }
        break;
    }
  }
  patchVlaues(data: any, edited: boolean) {
    console.log(data);
    this.isEdited = edited;
    this.payoutPatchData = data;
    this.addForm.patchValue({
      payoutTypeName: data.PayoutTypeName,
      Frequency: data.Frequency,
    });
  }

  patchVlauesSegment(data: any, edited: boolean) {
    console.log(data);
    this.isEdited = edited;
    this.segmentPatchData = data;
    this.addForm.patchValue({
      segmentOfPayout: data.PayoutTypeId,
      SegmentName: data.SegmentName,
    });
  }
  updateAddForm() {
    switch (this.currentAddType) {
      case 'PayoutType':
        if (this.addForm?.get('payoutTypeName').valid && this.addForm?.get('Frequency').valid) {
          this.isFormSubmittedAddForm = false;
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            EmpId: this.employeeDetails[0].EmpId,
            PayoutTypeId: this.payoutPatchData.PayoutTypeId,
            PayoutTypeName: this.addForm?.get('payoutTypeName').value,
            Frequency: this.addForm?.get('Frequency').value
          }
          this.isSpinner1 = true;
          this.payrollService.UpdatePayrollPayoutType(reqBody).subscribe((res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], 'success');
              this.isSpinner1 = false;
              this.getAllPayrollPayoutType();
              this.dropdownPayoutType();
              this.resetData();
              this.closeModalReset();
            } else {
              this.triggerToast(res['Message'], "Something went wrong", "warning");
              this.isSpinner1 = false;
            }
          }, error => {
            this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
            this.isSpinner1 = false;
          })
        } else {
          this.isFormSubmittedAddForm = true;
        }
        break;

      case 'Segment':
        if (this.addForm?.get('SegmentName').valid) {
          this.isFormSubmittedAddForm = false;
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            EmpId: this.employeeDetails[0].EmpId,
            SegmentId: this.segmentPatchData.SegmentId,
            PayoutTypeId: this.addForm?.get('segmentOfPayout').value,
            SegmentName: this.addForm?.get('SegmentName').value
          }
          this.isSpinner1 = true;
          this.payrollService.UpdatePayrollSegment(reqBody).subscribe((res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], 'success');
              this.isSpinner1 = false;
              // this.closeModalReset();
              this.addForm.get('SegmentName')?.reset();
              setTimeout(() => {
                this.getAllSegmentType();
              }, 0);
              // this.getDropDownSegment();
              // this.resetData();
            } else if (res['Message']) {
              this.triggerToast(res['Message'], "Something went wrong", "warning");
              this.isSpinner1 = false;
            }
          }, error => {
            this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
            this.isSpinner1 = false;
          })
        } else {
          this.isFormSubmittedAddForm = true;
        }
        break;
    }
  }
  getAllPayrollPayoutType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner1 = true;
    this.payrollService.GetAllPayrollPayoutType(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getListOfPayoutType = res;
          this.errorMessagePayout = '';
          this.isTableDataPayout = false;
        } else {
          this.errorMessagePayout = 'No Data Found';
          this.getListOfPayoutType = [];
          this.isTableDataPayout = true;
        }
        this.isSpinner1 = false;
      }, error: (err: any) => {
        this.isSpinner1 = false;
        this.errorMessagePayout = 'Internal Server Error';
        this.isTableDataPayout = true;
      }
    })
  }
  getFrequencyDDType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner1 = true;
    this.payrollService.DDPayrollFrequency(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDDOfFrequency = res;
        } else {
          this.triggerToast('', 'No Data Found For Frequency List', '')
        }
        this.isSpinner1 = false;
      }, error: (err: any) => {
        this.isSpinner1 = false;
        this.triggerToast('Internal Server Error', 'To Load Frequency Data', 'danger')
      }
    })
  }

  confirmDelete(row: any) {
    console.log(row)
    this.recordToDelete = row;
    this.deleteType = this.currentAddType;
    // this.isRecordDeleted = false;
  }
  deleteRecord() {
    if (!this.recordToDelete || !this.deleteType) return;
    const key = deleteKeyMap[this.deleteType];
    if (!key) {
      this.triggerToast('Delete Failed', 'Unknown delete type', 'danger');
      return;
    }
    const keys = deleteKeyMap[this.deleteType];
    const reqBody: any = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    };
    keys.forEach((key: string) => {
      reqBody[key] = this.recordToDelete[key];
    });
    switch (this.deleteType) {
      case 'PayoutType':
        this.isSpinner1 = true;
        this.payrollService.DeletePayrollPayoutType(reqBody).subscribe({
          next: (res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], 'success');
              this.isRecordDeletedCommon = true;
              this.getDDOfPayoutType = [];
              this.closeModalReset();
              setTimeout(() => {
                this.closeModalDelete.nativeElement?.click();
                this.getAllPayrollPayoutType();
                this.dropdownPayoutType();
                setTimeout(() => {
                  this.isRecordDeletedCommon = false;
                }, 1100);
              }, 1000);
            } else if (res['Message']) {
              this.triggerToast(res['Message'], res['Message'], 'warning');
              this.getAllPayrollPayoutType();
            }
            this.isSpinner1 = false;
          }, error: (err: any) => {
            this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
            this.isSpinner1 = false;
          }
        });
        break;
      case 'Segment':
        this.isSpinner1 = true;
        this.payrollService.DeletePayrollSegment(reqBody).subscribe({
          next: (res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], 'success');
              this.isRecordDeletedCommon = true;
              this.getDDOfSegment = [];
              setTimeout(() => {
                this.closeModalDelete.nativeElement?.click();
                this.getDDOfSegment = [];
                this.getAllSegmentType();
                this.closeModalReset();
                this.resetData();
                // this.getDropDownSegment();
                setTimeout(() => {
                  this.isRecordDeletedCommon = false;
                }, 1100);
              }, 1000);
            } else if (res['Message']) {
              this.triggerToast(res['Message'], res['Message'], 'warning');
            }
            this.isSpinner1 = false;
          }, error: (err: any) => {
            this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
            this.isSpinner1 = false;
          }
        });
        break;
    }

  }
  toggleIsActive(row: any): void {
    row.IsActive = !row.IsActive;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: row.PayoutTypeId
    }
    this.isSpinner1 = true;
    const apiCall = row.IsActive
      ? this.payrollService.ActivatePayrollPayoutType(reqBody)
      : this.payrollService.DeactivatePayrollPayoutType(reqBody);
    apiCall.subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.getAllPayrollPayoutType();
        } else if (res['msg']) {
          this.getAllPayrollPayoutType();
          this.triggerToast(`${row.IsActive ? 'Activated' : 'Deactivated'} successfully`, `${row.IsActive ? 'Activated' : 'Deactivated'}`, 'success');
        }
        this.isSpinner1 = false;
      },
      error: (err) => {
        this.isSpinner1 = false;
        row.IsActive = !row.IsActive;
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      }
    });
  }



  // getAllSegmentType() {
  //   const reqBody = {
  //     LoginId: this.employeeDetails[0].LoginId
  //   }
  //   this.isSpinner1 = true;
  //   this.payrollService.GetAllPayrollSegment(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res.length >= 1) {
  //         this.getListOfSegment = res;
  //         this.errorMessageSegment = '';
  //         this.isTableDataSegment = false;
  //       } else {
  //         this.errorMessageSegment = 'No Data Found';
  //         this.getListOfSegment = [];
  //         this.isTableDataSegment = true;
  //       }
  //       this.isSpinner1 = false;
  //     }, error: (err: any) => {
  //       this.isSpinner1 = false;
  //       this.errorMessageSegment = 'Internal Server Error';
  //       this.isTableDataSegment = true;
  //     }
  //   })
  // }

  getAllSegmentType() {
    console.log(this.getmodalPayoutDDValue)
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      PayoutTypeId: Number(this.getmodalPayoutDDValue),
    }
    console.log(reqBody)
    this.isSpinner1 = true;
    this.payrollService.GetAllPayrollPayoutTypeSegment(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getListOfSegment = res;
          this.errorMessageSegment = '';
          this.isTableDataSegment = false;
        } else {
          this.errorMessageSegment = 'No Data Found';
          this.getListOfSegment = [];
          this.isTableDataSegment = true;
        }
        this.isSpinner1 = false;
      }, error: (err: any) => {
        this.isSpinner1 = false;
        this.errorMessageSegment = 'Internal Server Error';
        this.isTableDataSegment = true;
      }
    })
  }

  // ************this is for modal open and close*****************

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

}
