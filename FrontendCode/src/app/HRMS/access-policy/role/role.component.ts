import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { payRollService } from '../../service/payroll.service';
@Component({
  selector: 'app-role',
  standalone: true,
  imports: [CommonModule, ToastMessageComponent, CardComponent, ReactiveFormsModule, SharedModule, NgxPaginationModule],
  templateUrl: './role.component.html',
  styleUrl: './role.component.scss'
})
export class RoleComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild('closeModal') closeModal!: ElementRef;

  roleForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails: any;
  accessPolicy: any;
  controlAccessPage: any;
  isSpinner: boolean = false;
  rows: any[] = [];
  errorMessage: any;
  isEdited: boolean = false;
  viewdata: any;
  getDepartementName: any;

  roleListAccess: any;
  originalRows: any;
  isTableData: boolean = false;
  getEditdata: any;
  page = 1;
  pageSize = 20;
  pageSizes = [20, 50, 100];
  isRecordDeleted: boolean = false;
  isCardOpen = false;

  constructor(private readonly fb: FormBuilder, 
    private readonly hrmsService: HrmsServiceService,
   private accessPolicyStoreService: AccessPolicyStoreService,
  private payrollService: payRollService) {

    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
     this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; 
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Role List'
      );
    });
  }

  ngOnInit(): void {
    this.roleForm = this.fb.group({
      role_Name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      // Validators.pattern('^[a-zA-Z ]*$'),
      department: ['', [Validators.required]],
      grade: ['', [Validators.required]],
    });
    setTimeout(() => {
      this.access_DD_department();
      this.DDGrade();
      setTimeout(() => {
        this.getAllRoleData();
      }, 100);
    }, 1000);

  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  access_DD_department() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsService.access_DD_department(reqBody).subscribe({
      next: (res: any) => {
        if (Array.isArray(res) && res.length > 0) {
          this.getDepartementName = res;
        } else {
          this.triggerToast('', 'Record Not Found', 'warning');
        }
        this.isSpinner = false;
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Failed to Load the Department List', 'danger');
        this.isSpinner = false;
      },
    });
  }
  getDD_grade: any;

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

  getAllRoleData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.getAllRoleData(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        setTimeout(() => {
          this.rows = res;
          this.originalRows = res;
          // this.totalPages = Math.ceil(this.rows.length / this.itemsPerPage);
          // this.updatePaginatedRows();
          this.isSpinner = false;
          this.isTableData = false;
        }, 1000);
      } else {
        this.errorMessage = "No records found";
        this.triggerToast('', 'No records found', 'danger');
        this.isSpinner = false;
        this.isTableData = true;
      }
    }, error => {
      this.errorMessage = "Internal Server Error";
      this.triggerToast('Internal Server Error', 'To Load The Role Data', 'danger');
      this.isSpinner = false;
      this.isTableData = true;

    })
  }

  selectedGradeData: any; // holds the full selected grade

  submitFormdata() {
    this.isFormSubmitted = true;
    if (this.roleForm.valid) {
      const selectedGrade = this.roleForm.get('grade').value;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId ? this.employeeDetails[0].EmpId : '',
        GradeId: selectedGrade?.GradeId || '',
        Grade: selectedGrade?.Grade || '',
        RoleName: this.roleForm?.get('role_Name').value ? this.roleForm?.get('role_Name').value : '',
        DeptId: Number(this.roleForm?.get('department').value) ? Number(this.roleForm?.get('department').value) : ''
      }
      // console.log(reqBody);
      this.isSpinner = true
      this.hrmsService.addRoleData(reqBody).subscribe((res: any) => {
        if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
          this.isSpinner = false;
        } else if (res['msg'] === 'Added') {
          this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
          this.rows = res;
          this.getAllRoleData();
          this.roleForm.reset();
          this.isSpinner = false;
          this.isFormSubmitted = false;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
      })
    } else {
      // this.triggerToast('Invalid', 'Please Fill All Details', 'danger');
      this.isSpinner = false;
    }
  }

  editData(data: any, edited: boolean) {
    this.getEditdata = data
    this.isEdited = edited;
    this.isCardOpen = true;
    this.roleForm?.get('department').patchValue(data.DeptId);
    this.roleForm?.get('role_Name').patchValue(data.RoleName);
    // Find and patch the full grade object
    const selectedGrade = this.getDD_grade.find((grade: any) => grade.GradeId === data.GradeId);
    this.roleForm?.get('grade').patchValue(selectedGrade || null);
  }
  updateRoleData() {
    if (this.roleForm.valid) {
      const selectedGrade = this.roleForm?.get('grade')?.value;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        DeptId: Number(this.roleForm?.get('department').value) ? Number(this.roleForm?.get('department').value) : '',
        RoleId: Number(this.getEditdata.RoleId) ? Number(this.getEditdata.RoleId) : '',
        RoleName: this.roleForm?.get('role_Name').value ? this.roleForm?.get('role_Name').value : '',
        GradeId: selectedGrade?.GradeId || '',
        Grade: selectedGrade?.Grade || ''
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.updateRoleData(reqBody).subscribe((res: any) => {
        if (res['msg'] === "Updated") {
          this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
          this.getAllRoleData();
          this.isSpinner = false;
          this.roleForm.reset();
          this.isEdited = false;
          this.isFormSubmitted = false;
        } else if (res['Message']) {
          this.triggerToast(res['Message'], 'Something went wrong', 'danger');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Something went wrong', 'danger');
        this.isSpinner = false;
      })
    }
  }

  onView(data: any) {
    this.viewdata = data;
  }
  deleteRoleData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      RoleId: this.viewdata.RoleId,
      RoleName: this.viewdata.RoleName
    }
    this.isSpinner = true;
    this.hrmsService.deleteRoleData(reqBody).subscribe((res: any) => {
      if (res['msg'] === "Deleted") {
        this.isSpinner = false;
        this.isRecordDeleted = true;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getAllRoleData();
        setTimeout(() => {
          this.closeModal.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
      } else if (res['Message']) {
        this.triggerToast(res['Message'], res['Message'], 'warning');
        this.isSpinner = false;
      }

    }, error => {
      this.triggerToast('Internal Server Error', 'Failed To Delete Records', 'danger');
      this.isSpinner = false;
    })
  }
  resetData() {
    this.roleForm.reset();
    this.isEdited = false;
    this.isFormSubmitted = false;
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
  }

  onFocus(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  onBlur(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  setFloatingLabel(selectElement: HTMLSelectElement) {
    const label = selectElement.nextElementSibling as HTMLElement;
    if (selectElement.value) {
      label.classList.add('floating');
    } else {
      label.classList.remove('floating');
    }
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rows = this.rows.filter((row: any) =>
        Object.values(row).some(val =>
          String(val).toUpperCase().includes(filterValue)
        )
      );
    } else {
      this.isTableData = false;
      this.rows = [...this.originalRows];
      this.rows = this.rows
    }
    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = 'No Records Found for Searched Data';
      this.rows = [...this.originalRows];
    } else {
      this.isTableData = false;
      this.errorMessage = null;
    }
  }

}
