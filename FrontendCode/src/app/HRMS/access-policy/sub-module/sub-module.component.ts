import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
@Component({
  selector: 'app-sub-module',
  standalone: true,
  imports: [CardComponent, NgxPaginationModule, SharedModule, CommonModule, ReactiveFormsModule,
    ToastMessageComponent, ReactiveFormsModule, RouterModule],
  templateUrl: './sub-module.component.html',
  styleUrl: './sub-module.component.scss'
})
export class SubModuleComponent implements OnInit {
  @ViewChild('closeModal') closeModal!: ElementRef;
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue!: ElementRef;

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsService: HrmsServiceService,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // console.log('this.accessPolicy=>', this.accessPolicy);
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'SubModule List'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log('this.controlAccessPage=>', this.controlAccessPage);

  }

  subModuleForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails: any;
  accessPolicy: any;
  controlAccessPage: any;
  getSubModuleAccess: any
  isSpinner: boolean = false;
  rows: any[] = [];
  errorMessage: any;
  isEdited: boolean = false;
  viewdata: any;
  getEditdata: any;
  getDepartementModule: any;
  originalRows: any;
  isTableData: boolean = false;
  isRecordDeleted: boolean = false;
  page = 1;
  pageSize = 20;
  pageSizes = [20, 50, 100];
  isCardOpen = false;


  ngOnInit(): void {
    this.subModuleForm = this.fb.group({
      module: ['', [Validators.required]],
      sub_module: ['', [Validators.required]]
    });
    this.getAllSubModuleList();
    this.access_Module();
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; // safety check
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'SubModule List'
      );
    });
  }


  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  access_Module() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.hrmsService.access_Module(reqBody).subscribe((res: any) => {
      this.getDepartementModule = res;
    })
  }

  getAllSubModuleList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.getAllSubModule(reqBody).subscribe((res: any) => {
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
    }, error => {
      this.errorMessage = "Internal Server Error";
      this.isSpinner = false;
      this.isTableData = true;
    })
  }

  submitFormdata() {
    this.isFormSubmitted = true;
    if (this.subModuleForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId ? this.employeeDetails[0].EmpId : '',
        ModuleId: Number(this.subModuleForm?.get('module').value) ? Number(this.subModuleForm?.get('module').value) : '',
        SubModuleName: this.subModuleForm?.get('sub_module').value ? this.subModuleForm?.get('sub_module').value : ''
      }
      this.isSpinner = true
      this.hrmsService.addSubModule(reqBody).subscribe((res: any) => {
        if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
          this.isSpinner = false;
        } else if (res['msg'] === 'Added') {
          this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
          this.getAllSubModuleList();
          this.subModuleForm.reset();
          this.isSpinner = false;
          this.isFormSubmitted = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Add Records', 'warning');
        this.isSpinner = false;
      })
    } else {
      // this.triggerToast('Invalid', 'Please Fill All Details', 'danger');
      this.isSpinner = false;
    }
  }

  editData(data: any, edited: boolean) {
    // console.log(data);
    this.getEditdata = data;
    this.isCardOpen = true;
    this.isEdited = edited;
    this.subModuleForm?.get('module').patchValue(data.ModuleId);
    this.subModuleForm?.get('sub_module').patchValue(data.SubModuleName);
  }


  updatesubModuleForm() {
    if (this.subModuleForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId ? this.employeeDetails[0].EmpId : '',
        ModuleId: Number(this.subModuleForm?.get('module').value) ? Number(this.subModuleForm?.get('module').value) : '',
        SubModuleId: Number(this.getEditdata.SubModuleId) ? Number(this.getEditdata.SubModuleId) : '',
        SubModuleName: this.subModuleForm?.get('sub_module').value ? this.subModuleForm?.get('sub_module').value : '',
      }
      this.isSpinner = true;
      this.hrmsService.updateSubModule(reqBody).subscribe((res: any) => {
        if (res['msg'] === "Updated") {
          this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
          this.getAllSubModuleList();
          this.isSpinner = false;
          this.subModuleForm.reset();
          this.isEdited = false;
          this.isFormSubmitted = false;
        } else {
          this.triggerToast(res['Message'], res['Message'], 'danger');
          this.isEdited = false;
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'warning');
        this.isEdited = false;
        this.isSpinner = false;
      })
    }
  }

  onView(data: any) {
    // console.log(data);
    this.viewdata = data
  }

  deleteSubModuledata() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      SubModuleId: this.viewdata.SubModuleId,
      SubModuleName: this.viewdata.SubModuleName
    }
    this.isSpinner = true;
    this.hrmsService.deleteSubModule(reqBody).subscribe((res: any) => {
      if (res['msg'] === "Deleted") {
        this.isSpinner = false;
        this.isRecordDeleted = true;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getAllSubModuleList();
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
      this.triggerToast('Internal Server Error', 'Failed To Delete Record', 'warning');
      this.isSpinner = false;
    })
  }

  resetData() {
    this.subModuleForm.reset();
    this.isEdited = false;
    this.isFormSubmitted = false;
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
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
