import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { Router, RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-module',
  standalone: true,
  imports: [CommonModule, ToastMessageComponent, CardComponent, ReactiveFormsModule,
    NgxPaginationModule, SharedModule, RouterModule],
  templateUrl: './module.component.html',
  styleUrl: './module.component.scss'
})
export class ModuleComponent implements OnInit {
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;

  moduleForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails: any;
  accessPolicy: any;
  controlAccessPage: any;
  getModuleListAccess: any
  isSpinner: boolean = false;
  rows: any[] = [];
  errorMessage: any;
  isEdited: boolean = false;
  viewdata: any;
  getEditdata: any;
  originalRows: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 20;
  pageSizes = [20, 50, 100];
  isRecordDeleted: boolean = false;
  isCardOpen = false;
  tabs: any[] = [];

  allTabs = [
    { id: 'submodule', title: 'SubModule List', type: 'item', url: '/subModule', icon: 'feather icon-grid' },
    { id: 'pagemodule', title: 'Page Module List', type: 'item', url: '/pageModule', icon: 'feather icon-file-text' }
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }
  constructor(private readonly fb: FormBuilder, private readonly hrmsService: HrmsServiceService,
    private router: Router, private accessPolicyStoreService: AccessPolicyStoreService,
  ) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // console.log('this.accessPolicy=>', this.accessPolicy);
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Module List'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log('this.controlAccessPage=>', this.controlAccessPage);
  }

  ngOnInit(): void {
    this.moduleForm = this.fb.group({
      module_name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
    });
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; // ✅ Guard clause
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Module List'
      );
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((p: any) => p.PageName === tab.title && p.ViewAccess)
      );
    });
    this.getAllModule();
  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  getAllModule() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.getAllModule(reqBody).subscribe((res: any) => {
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
    if (this.moduleForm.valid) {
      this.isSpinner = true;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId || '',
        ModuleName: this.moduleForm?.get('module_name')?.value || '',
      };
      this.hrmsService.addModuleData(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast(res['Message'], '', 'warning');
          } else if (res['msg'] === 'Added') {
            this.triggerToast(res['msg'], 'Module Added Successfully', 'success');
            this.rows = res;
            this.getAllModule();
            this.moduleForm.reset();
            this.isFormSubmitted = false;
          }
          this.isSpinner = false;
        },
        error: () => {
          this.triggerToast('Internal Server Error', 'Failed To Add Records', 'danger');
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
    this.moduleForm?.get('module_name').patchValue(data.ModuleName);
  }

  updateModuleForm() {
    this.isFormSubmitted = true;
    if (this.moduleForm.valid) {
      this.isSpinner = true;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId || '',
        ModuleId: this.getEditdata.ModuleId || '',
        ModuleName: this.moduleForm?.get('module_name')?.value || '',
      };
      this.hrmsService.updateModuleData(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === "Updated") {
            this.triggerToast(res['msg'], 'Module Name Updated Successfully', 'success');
            this.getAllModule();
            this.moduleForm.reset();
            this.isEdited = false;
            this.isFormSubmitted = false;
          } else {
            this.triggerToast(res['Message'], 'Something went wrong', 'danger');
          }
          this.isSpinner = false;
        },
        error: (error) => {
          this.triggerToast(error['Message'] || 'Internal Server Error', 'Failed to Update Records', 'danger');
          this.isEdited = false;
          this.isSpinner = false;
        }
      });
    } else {
      this.isSpinner = false;
    }
  }


  onView(data: any) {
    this.viewdata = data
  }

  deleteModuleData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      ModuleId: this.viewdata.ModuleId,
      ModuleName: this.viewdata.ModuleName,
    };
    this.isSpinner = true;
    this.hrmsService.deleteModuleData(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg'] === "Deleted") {
          this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
          this.isRecordDeleted = true;
          this.getAllModule();
          setTimeout(() => {
            this.closeModal.nativeElement?.click();
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
        } else if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning');
        } else {
          this.triggerToast(res['msg'] || 'Unexpected Error', 'Something went wrong', 'warning');
        }
        this.isSpinner = false;
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Failed to Delete Record', 'danger');
        this.isSpinner = false;
      },
    });
  }

  resetData() {
    this.moduleForm.reset();
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
