import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { payRollService } from '../../service/payroll.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-mapping',
  standalone: true,
  imports: [CommonModule, SharedModule, ToastMessageComponent, ReactiveFormsModule,
    NgxPaginationModule
  ],
  templateUrl: './mapping.component.html',
  styleUrl: './mapping.component.scss'
})
export class MappingComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;

  isSpinner: boolean = false;
  mappingForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  getDD_grade: any;
  getDDOfPayoutType: any = [];
  isEdited: boolean = false;
  errorMessage: any;
  isTableData: boolean = false;
  isSpinner1: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  getAllPayoutMasterList: any = [];
  originalAllPayoutMasterList: any[] = [];
  isRecordDeletedCommon: boolean = false;
  searchValue: string = '';
  isCardOpen = false;
  constructor(private payrollService: payRollService,
    private readonly hrmsService: HrmsServiceService,
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
    this.DDGrade();
    setTimeout(() => {
      this.dropdownPayoutType();
      this.getAllPayoutMappingMaster();
    }, 100);
    this.mappingForm = this.fb.group({
      grade: ['', [Validators.required]],
      payoutType: ['', [Validators.required]],
    })
  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  DDGrade() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      GradeId: 0
      // EmpId: 110
    }
    this.hrmsService.access_DD_Grade(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDD_grade = res;
      } else {
        this.getDD_grade = []
      }
    }, error => {
      this.getDD_grade = [];
      this.triggerToast('Internal Server Error', 'To Grade List', 'danger')
    })
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
      this.triggerToast('Payout Type', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }

  getAllPayoutMappingMaster() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.GetAllPayoutMappingMaster(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getAllPayoutMasterList = res;
        this.originalAllPayoutMasterList = res;
        this.isSpinner = false;
        this.errorMessage = '';
        this.isTableData = false;
      } else {
        this.isSpinner = false;
        this.errorMessage = 'No Data Found';
        this.isTableData = true;

      }
    }, error => {
      this.triggerToast('', 'Internal Server Error', 'danger');
      this.isSpinner = false;
      this.errorMessage = 'Internal Server Error';
      this.isTableData = true;
    })
  }

  getPayoutMappingMaster() {
    const reqBody = {
      "LoginId": 149,
      "MapId": 1
    }
  }

  applyFilter() {
    const val = this.searchValue.toLowerCase().trim();
    this.getAllPayoutMasterList = this.originalAllPayoutMasterList.filter((row: any) => {
      return (
        row.Grade?.toLowerCase().includes(val) ||
        row.PayoutTypeName?.toLowerCase().includes(val)
      );
    });
    // Reset pagination on new search
    this.page = 1;
  }
  submitForm() {
    if (this.mappingForm.valid) {
      const grade = this.mappingForm?.get('grade').value;
      const payout = this.mappingForm?.get('payoutType').value
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        GradeId: grade.GradeId,
        Grade: grade.Grade,
        PayoutTypeId: payout.PayoutTypeId,
        PayoutTypeName: payout.PayoutTypeName,
      }
      this.isSpinner = true;
      this.payrollService.AddPayoutMappingMaster(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg']) {
            this.triggerToast('', res['msg'], "");
            this.resetData();
            this.getAllPayoutMappingMaster();
          } else if (res['Message']) {
            this.triggerToast(res['Message'], "Failed To Add", "warning");
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'To Add Record', 'danger');
          this.isSpinner = false;

        }
      })
    } else {
      this.isFormSubmitted = true;
    }
  }
  patchValuesData: any;
  patchVlaues(data: any, edited: boolean) {
    this.isEdited = edited;
    const selectedGrade = this.getDD_grade.find((x: any) => x.GradeId === data.GradeId);
    const selectedPayoutType = this.getDDOfPayoutType.find((x: any) => x.PayoutTypeId === data.PayoutTypeId);
    this.mappingForm.patchValue({
      grade: selectedGrade,
      payoutType: selectedPayoutType
    });
    this.patchValuesData = data;
    this.isCardOpen = true;
  }
  updateForm() {
    if (this.mappingForm.valid) {
      const grade = this.mappingForm?.get('grade').value;
      const payout = this.mappingForm?.get('payoutType').value
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        MapId: this.patchValuesData.MapId,
        GradeId: grade.GradeId,
        Grade: grade.Grade,
        PayoutTypeId: payout.PayoutTypeId,
        PayoutTypeName: payout.PayoutTypeName,
      }
      this.isSpinner = true;
      this.payrollService.UpdatePayoutMappingMaster(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], 'success');
            this.isSpinner = false;
            this.getAllPayoutMappingMaster();
            this.resetData();
          } else if (res['Message']) {
            this.triggerToast(res['Message'], "Something went wrong", "warning");
            this.isSpinner = false;
          }
        }, error: (err: any) => {
          this.triggerToast('', 'Internal Server Error', 'danger');
          this.isSpinner = false;
        }
      })
    } else {
      this.isFormSubmitted = true;
    }
  }
  getDeleteData: any;
  confirmDelete(row: any) {
    console.log(row);
    this.getDeleteData = row;
  }

  deleteRecord() {
    this.isSpinner1 = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      MapId: this.getDeleteData.MapId,
    }
    this.isSpinner1 = true;
    this.payrollService.DeletePayoutMappingMaster(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isRecordDeletedCommon = true;
          setTimeout(() => {
            this.closeModalDelete.nativeElement?.click();
            this.getAllPayoutMappingMaster();
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
    })
  }

  resetData() {
    this.mappingForm.reset();
    this.isFormSubmitted = false;
    this.isEdited = false
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
