import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-page-module',
  standalone: true,
  imports: [CardComponent, CommonModule, NgxPaginationModule, SharedModule,
    ReactiveFormsModule, ToastMessageComponent, RouterModule],
  templateUrl: './page-module.component.html',
  styleUrl: './page-module.component.scss'
})
export class PageModuleComponent implements OnInit {
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;
  pageModuleForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails: any;
  accessPolicy: any;
  getPageModuleAccess: any
  controlAccessPage: any
  isSpinner: boolean = false;
  rows: any[] = [];
  errorMessage: any;
  isEdited: boolean = false;
  viewdata: any;
  getEditdata: any;
  getDepartementModule: any;
  getDepartementSubModule: any;
  originalRows: any;
  isTableData: boolean = false;
  page = 1;
pageSize = 20;
  pageSizes = [20, 50, 100];
  isRecordDeleted: boolean = false;
  isCardOpen = false;

  constructor(private readonly fb: FormBuilder, private readonly hrmsService: HrmsServiceService,
    private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; // safety check
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Page Module List'
      );
    });
  }


  ngOnInit(): void {
    this.pageModuleForm = this.fb.group({
      module: ['', [Validators.required]],
      sub_module: ['', [Validators.required]],
      pagemodule_name: ['', [Validators.required]],
    });
    this.pageModuleForm?.get('module').valueChanges.subscribe((val: any) => {
      this.pageModuleForm?.get('sub_module').reset();
      this.getDepartementSubModule = [];
    })
    setTimeout(() => {
      this.access_Module();
      setTimeout(() => {
        this.getAllPagemoduleList();
      }, 100);
    }, 100);

  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  access_Module() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.hrmsService.access_Module(reqBody).subscribe((res: any) => {
      if (res) {
        this.getDepartementModule = res;
      }
    }, error => {
      this.triggerToast("Internal Server Error", "To Load Module List", "danger");
      this.isSpinner = false;
    })
  }

  callSubModule() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      ModuleId: Number(this.pageModuleForm.get('module')?.value)
    }
    this.hrmsService.access_Sub_Module(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDepartementSubModule = res;
      } else {
        this.getDepartementSubModule = []
      }
    }, error => {
      this.triggerToast("Internal Server Error", "To Load Sub Module List", "danger");
      this.isSpinner = false;
    })
  }


  getAllPagemoduleList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.getAllPagemodule(reqBody).subscribe((res: any) => {
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
    if (this.pageModuleForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId ? this.employeeDetails[0].EmpId : '',
        ModuleId: Number(this.pageModuleForm?.get('module').value) ? Number(this.pageModuleForm?.get('module').value) : '',
        SubModuleId: Number(this.pageModuleForm?.get('sub_module').value) ? Number(this.pageModuleForm?.get('sub_module').value) : '',
        PageModuleName: this.pageModuleForm?.get('pagemodule_name').value ? this.pageModuleForm?.get('pagemodule_name').value : ''
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.addPagemodule(reqBody).subscribe((res: any) => {
        if (res["msg"] === "Added") {
          this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
          this.getAllPagemoduleList();
          this.isSpinner = false;
          this.pageModuleForm.reset();
          this.isFormSubmitted = false;
        } else if (res["Message"]) {
          this.triggerToast(res['Message'], "Duplicate Access Name is not allowed", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Add Records', "danger");
        this.isSpinner = false;
      })
    } else {
      // this.triggerToast("Invalid", "Please Enter valid Credentials ", "danger");
      this.isSpinner = false;
      // this.loading = false
    }
  }

  editData(data: any, edited: boolean) {
    // console.log(data);
    this.getEditdata = data;
    this.isCardOpen = true;
    this.isEdited = edited;
    this.pageModuleForm?.get('module').patchValue(data.ModuleId);
    // this.pageModuleForm?.get('sub_module').patchValue(data.SubModuleName);
    this.pageModuleForm?.get('pagemodule_name').patchValue(data.PageModuleName);
    this.callSubModule()
    setTimeout(() => {
      this.pageModuleForm?.get('sub_module').patchValue(data.SubModuleId);
      // this.pageModuleForm?.get('sub_module').patchValue(data.SubModuleName);
    }, 100);
  }

  updatePageModuleForm() {
    if (this.pageModuleForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        ModuleId: Number(this.pageModuleForm?.get('module').value) ? Number(this.pageModuleForm?.get('module').value) : '',
        SubModuleId: Number(this.pageModuleForm?.get('sub_module').value) ? Number(this.pageModuleForm?.get('sub_module').value) : '',
        PageModuleId: Number(this.getEditdata.PageModuleId) ? Number(this.getEditdata.PageModuleId) : '',
        PageModuleName: this.pageModuleForm?.get('pagemodule_name').value ? this.pageModuleForm?.get('pagemodule_name').value : '',
      }
      // console.log(reqBody);
      this.isSpinner = true
      this.hrmsService.updatePagemodule(reqBody).subscribe((res: any) => {
        if (res['msg'] === "Updated") {
          this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
          this.getAllPagemoduleList();
          this.isSpinner = false;
          this.pageModuleForm.reset();
          this.isEdited = false;
          this.isFormSubmitted = false;
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
        this.isSpinner = false;
      })
    }
  }

  onView(data: any) {
    // console.log(data);
    this.viewdata = data
  }

  deletePageModuleData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      PageModuleId: this.viewdata.PageModuleId,
      PageModuleName: this.viewdata.PageModuleName
    }
    this.isSpinner = true;
    this.hrmsService.deletePagemodule(reqBody).subscribe((res: any) => {
      if (res['msg'] === "Deleted") {
        this.isSpinner = false;
        this.isRecordDeleted = true;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getAllPagemoduleList();
        setTimeout(() => {
          this.closeModal.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
      } else {
        this.triggerToast(res['Message'], 'Something went wrong', 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast(error['Message'], 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }

  resetData() {
    this.pageModuleForm.reset();
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
