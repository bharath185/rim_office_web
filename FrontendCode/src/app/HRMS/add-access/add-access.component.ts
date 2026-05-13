import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { HrmsServiceService } from '../hrms-service.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccessPolicyStoreService } from '../service/accessPolicayApi.service';


@Component({
  selector: 'app-add-access',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule, CommonModule, ToastMessageComponent, NgxPaginationModule],
  providers: [NgbDropdownConfig],
  templateUrl: './add-access.component.html',
  styleUrl: './add-access.component.scss'
})
export class AddAccessComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('exampleModal') modalElementRef!: ElementRef;
  modalInstance: any;
  employeeDetails: any;
  accessPolicy: any;
  controlAccessPage: any;
  addAccessForm: any = FormGroup;
  modalForm: any = FormGroup;
  getDepartementRole: any;
  getDepartementName: any;
  getDepartementModule: any;
  isFormSubmitted: boolean = false;
  pagesData: any;
  selectAll: boolean = false;
  selectAllView: boolean = false;
  selectAllAdd: boolean = false;
  selectAllDelete: boolean = false;
  selectAllUpdate: boolean = false;

  constructor(private hrmsService: HrmsServiceService, private fb: FormBuilder,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Add Access'
      );
    });
  }

  ngOnInit(): void {
    this.addAccessForm = this.fb.group({
      department: ['', [Validators.required]],
      role: ['', [Validators.required]],
      access_name: ['', [Validators.required]],
    });
    this.modalForm = this.fb.group({
      modalModuleName: ['', [Validators.required]]
    });

    this.addAccessForm?.get('department').valueChanges.subscribe((val: any) => {
      this.addAccessForm?.get('role').reset();
      this.getDepartementRole = [];
      this.addAccessForm?.updateValueAndValidity();
    });

    this.addAccessForm?.get('role').valueChanges.subscribe((val: any) => {
      if (val && this.addAccessForm?.get('role')?.valid) {
        this.GetAllPagesAccessList();
      } else {
      }
      this.addAccessForm?.updateValueAndValidity();
    });


    setTimeout(() => {
      this.access_DD_department();
      this.GetAllPagesAccessList();
    }, 100);
  }

  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.hrmsService.access_DD_department(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDepartementName = res;

      } else {
        this.getDepartementName = []
      }
    }, error => {
      this.getDepartementName = [];
      this.triggerToast('Internal Server Error', 'To Load Department List', 'danger')
    })
  }

  callRoleApi() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.addAccessForm?.get('department')?.value,
    }
    this.hrmsService.access_DD_Role(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDepartementRole = res;
      } else {
        this.getDepartementRole = [];
      }
    }, error => {
      this.getDepartementRole = [];
      this.triggerToast('Internal Server Error', 'To Load Role/Designation List', 'danger')
    })
  }
  toggleAll() {
    this.pagesData.forEach((page: any) => {
      page.selected = this.selectAll;
      page.canView = this.selectAllView;
      page.canAdd = this.selectAllAdd;
      page.canDelete = this.selectAllDelete;
      page.canUpdate = this.selectAllUpdate;
    });
  }

  toggleRowPermissions(page: any) {
    const isSelected = page.selected;
    page.canView = isSelected;
    page.canAdd = isSelected;
    page.canDelete = isSelected;
    page.canUpdate = isSelected;
  }
  toggleView() {
    this.pagesData.forEach((page: any) => {
      page.canView = this.selectAllView;
    });
  }

  toggleAdd() {
    this.pagesData.forEach((page: any) => {
      page.canAdd = this.selectAllAdd;
    });
  }

  toggleDelete() {
    this.pagesData.forEach((page: any) => {
      page.canDelete = this.selectAllDelete;
    });
  }

  toggleUpdate() {
    this.pagesData.forEach((page: any) => {
      page.canUpdate = this.selectAllUpdate;
    });
  }

  GetAllPagesAccessList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: Number(this.addAccessForm?.get('department')?.value),
      RoleId: Number(this.addAccessForm?.get('role')?.value),
    };

    this.hrmsService.GetAllPages(reqBody).subscribe(
      (res: any) => {
        if (res.length >= 1) {
          this.pagesData = res.map((page: any) => ({
            ...page,
            canView: page.ViewAccess,
            canAdd: page.AddAccess,
            canDelete: page.DeleteAccess,
            canUpdate: page.UpdateAccess,
            selected: page.ViewAccess || page.AddAccess || page.DeleteAccess || page.UpdateAccess
          }));
          this.originalPagesData = [...this.pagesData];
        }
      },
      (error) => {
        this.triggerToast('Internal Server Error', 'Get All Access List', 'danger');
      }
    );
  }

  originalPagesData: any[] = [];
  globalFilter: string = '';

  applyGlobalFilter() {
    const filterValue = this.globalFilter.toLowerCase();
    this.pagesData = this.originalPagesData.filter((page: any) =>
      page.ModuleName.toLowerCase().includes(filterValue) ||
      page.SubModuleName.toLowerCase().includes(filterValue) ||
      page.PageName.toLowerCase().includes(filterValue)
    );
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  submitSelected() {
    const selectedPages = this.pagesData.filter((page: any) => page.selected);
    if (selectedPages.length === 0) {
      this.triggerToast('No rows selected', 'Please select at least one row.', 'warning');
      return;
    }
    console.log('Selected Rows:', selectedPages);
    const payload = selectedPages.map((page: any) => ({
      EmpId: Number(this.employeeDetails[0].EmpId),
      DeptId: Number(this.addAccessForm?.get('department').value),
      RoleId: Number(this.addAccessForm?.get('role').value),
      ModuleId: Number(page.ModuleId),
      SubModuleId: Number(page.SubModuleId),
      PageModuleId: Number(page.PageModuleId),
      AddAccess: !!page.canAdd,
      UpdateAccess: !!page.canUpdate,
      DeleteAccess: !!page.canDelete,
      ViewAccess: !!page.canView,
      CreatedBy: ""
    }));
    console.log(payload);
    this.hrmsService.SubmitAccessControls(payload).subscribe(
      (res: any) => {
        console.log(res);
        if (res['msg']) {
          this.addAccessForm.reset();
          window.location.reload();
          this.GetAllPagesAccessList();
          this.triggerToast(res['msg'], res['msg'], '');
        } else {
          this.triggerToast(res['msg'], res['msg'], 'info');
        }
      },
      (error) => {
        this.triggerToast('Error', 'Failed to submit selected pages.', 'danger');
      }
    );
  }

  editAccess() {
    const selectedPages = this.pagesData.filter((page: any) => page.selected);
    if (selectedPages.length === 0) {
      this.triggerToast('No rows selected', 'Please select at least one row.', 'warning');
      return;
    }
    console.log('Selected Rows:', selectedPages);
    const payload = selectedPages.map((page: any) => ({
      EmpId: Number(this.employeeDetails[0].EmpId),
      DeptId: Number(this.addAccessForm?.get('department').value),
      RoleId: Number(this.addAccessForm?.get('role').value),
      ModuleId: Number(page.ModuleId),
      SubModuleId: Number(page.SubModuleId),
      PageModuleId: Number(page.PageModuleId),
      AddAccess: !!page.canAdd,
      UpdateAccess: !!page.canUpdate,
      DeleteAccess: !!page.canDelete,
      ViewAccess: !!page.canView,

    }));
    console.log(payload);
    this.hrmsService.UpdatePageModule(payload).subscribe(
      (res: any) => {
        if (res['message'][0].msg === 'Updated') {
          this.triggerToast(res['message'][0].msg, 'Pages Updated successfully!', 'success');
          this.GetAllPagesAccessList();
        } else {
          this.triggerToast('Failed', res['message'][0].msg, 'warning');
        }
      },
      (error) => {
        this.triggerToast('Error', 'Failed to submit selected pages.', 'danger');
      }
    );
  }

  deleteAccess() {
    const selectedPages = this.pagesData.filter((page: any) => page.selected);
    if (selectedPages.length === 0) {
      this.triggerToast('No rows selected', 'Please select at least one row.', 'warning');
      return;
    }
    const payload = selectedPages.map((page: any) => ({
      EmpId: Number(this.employeeDetails[0].EmpId),
      DeptId: Number(this.addAccessForm?.get('department').value),
      RoleId: Number(this.addAccessForm?.get('role').value),
      ModuleId: Number(page.ModuleId),
      SubModuleId: Number(page.SubModuleId),
      PageModuleId: Number(page.PageModuleId),
    }));
    console.log(payload);
    this.hrmsService.DeletePageModule(payload).subscribe(
      (res: any) => {
        console.log();
        if (res['message'][0].msg === 'Deleted') {
          this.triggerToast(res['message'][0].msg, 'Deleted', 'success');
          this.GetAllPagesAccessList();
        } else {
          this.triggerToast('Failed', res['message'][0].msg, 'warning');
        }
      },
      (error) => {
        this.triggerToast('Error', 'Failed to submit selected pages.', 'danger');
      }
    );
  }

  onModuleSelect(event: Event) {
    console.log(event.target);

  }
}
