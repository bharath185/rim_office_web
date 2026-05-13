import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
@Component({
  selector: 'app-department',
  standalone: true,
  imports: [CardComponent, SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent, NgxPaginationModule],
  templateUrl: './department.component.html',
  styleUrl: './department.component.scss'
})
export class DepartmentComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild('closeModal') closeModal!: ElementRef;

  accessPolicy: any;
  controlAccessPage: any;
  departmentForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails: any;
  isSpinner: boolean = false;
  rows: any;
  errorMessage: any;
  isEdited: boolean = false;
  viewdata: any;
  getEditdata: any;
  originalRows: any;
  departmentListAccess: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 20;
  pageSizes = [20, 50, 100];
  isRecordDeleted: boolean = false;
  isCardOpen = false;

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsService: HrmsServiceService,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Department List'
      );
    });
  }

  ngOnInit(): void {
    this.departmentForm = this.fb.group({
      short_Name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      department_Name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      // Validators.pattern('^[a-zA-Z ]*$'), 
    });
    this.getAllDepartmentData();
  }
  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }


  getAllDepartmentData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.isSpinner = true;
    this.hrmsService.getAllDepartmentData(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          setTimeout(() => {
            this.rows = res;
            this.originalRows = res;
            this.isSpinner = false;
            this.isTableData = false;
          }, 1000);
        } else {
          this.errorMessage = "No records found";
          this.isSpinner = false;
          this.isTableData = true;
        }
      },
      error: (error: any) => {
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
      },
      complete: () => {
      },
    });
  }

  submitFormdata() {
    this.isFormSubmitted = true;

    if (this.departmentForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId || '',
        DeptName: this.departmentForm?.get('department_Name')?.value || '',
        DeptShortName: this.departmentForm?.get('short_Name')?.value || '',
      };
      this.isSpinner = true;
      this.hrmsService.addDepartmentData(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast(res['Message'], '', 'warning');
          } else if (res['msg'] === 'Added') {
            this.getAllDepartmentData();
            this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
            this.departmentForm?.reset();
            this.isFormSubmitted = false;
          }
          this.isSpinner = false;
        },
        error: () => {
          this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
          this.isSpinner = false;
        }
      });
    } else {
      this.isSpinner = false;
    }
  }


  editData(data: any, edited: boolean) {
    this.getEditdata = data;
    this.isCardOpen = true;
    this.isEdited = edited;
    this.departmentForm?.get('department_Name').patchValue(this.getEditdata.DeptName);
    this.departmentForm?.get('short_Name').patchValue(data.DeptShortName)
  }

  updateDepartmentForm() {
    this.isFormSubmitted = true;
    if (this.departmentForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId || '',
        DeptId: this.getEditdata.DeptId || '',
        DeptName: this.departmentForm?.get('department_Name')?.value || '',
        DeptShortName: this.departmentForm?.get('short_Name')?.value || '',
      };
      this.isSpinner = true;
      this.hrmsService.updateDepartmentData(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === "Updated") {
            this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
            this.getAllDepartmentData();
            this.departmentForm.reset();
            this.isEdited = false;
            this.isFormSubmitted = false;
          } else {
            this.triggerToast(res['Message'], 'Something went wrong', 'danger');
          }
          this.isSpinner = false;
        },
        error: (err) => {
          this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
          this.isSpinner = false;
        }
      });
    }
  }


  onView(data: any) {
    this.viewdata = data
  }

  deleteDepartmentData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.viewdata.DeptId,
      DeptName: this.viewdata.DeptName,
    };
    this.isSpinner = true;
    this.hrmsService.deleteDepartmentData(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg'] === "Deleted") {
          this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
          this.isRecordDeleted = true;
          this.getAllDepartmentData();
          setTimeout(() => {
            this.closeModal.nativeElement?.click();
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
        } else if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
        } else {
          this.triggerToast(res['msg'], 'Something went wrong', 'warning');
        }
        this.isSpinner = false;
      },
      error: () => {
        this.triggerToast('Internal Server Error', '', 'danger');
        this.isSpinner = false;
      }
    });
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  resetData() {
    this.departmentForm.reset();
    this.isEdited = false;
    this.isFormSubmitted = false;
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) =>
        Object.values(row).some(val =>
          String(val).toUpperCase().includes(filterValue)
        )
      );
      if (this.rows.length === 0) {
        this.isTableData = true;
        this.errorMessage = 'No Records Found for Searched Data';
      } else {
        this.isTableData = false;
        this.errorMessage = null;
      }
    } else {
      this.rows = [...this.originalRows];
      this.isTableData = false;
      this.errorMessage = null;
    }
  }



}
