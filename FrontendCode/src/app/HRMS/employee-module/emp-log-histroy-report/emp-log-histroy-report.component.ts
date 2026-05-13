import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/assets/environment';
import { trigger, style, animate, transition } from '@angular/animations';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs';
import { payRollService } from '../../service/payroll.service';
import { NgxPaginationModule } from 'ngx-pagination';


@Component({
  selector: 'app-emp-log-histroy-report',
  standalone: true,
  imports: [CommonModule, SharedModule, ReactiveFormsModule, ToastMessageComponent,
    RouterModule, NgxPaginationModule
  ],
  templateUrl: './emp-log-histroy-report.component.html',
  styleUrl: './emp-log-histroy-report.component.scss'
})
export class EmpLogHistroyReportComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  errorMessage: any;
  viewdata: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 500];
  employeeDetails;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any;
  employeeLogHistroyForm: any = FormGroup;
  isTableData: boolean = false;
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  rows: any[] = [];
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  getDropdownReporter: any[] = [];
  getDropdownEmployee: any[] = [];

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsEmployeeModuleService: EmployeeModuleService,
    private readonly fromQueryParams: ActivatedRoute,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly cdr: ChangeDetectorRef,
    private eRef: ElementRef,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private entityStateService: EntityStateService,
    private readonly payrollLocationDD: payRollService,
  ) {
    const accessPolicy = sessionStorage.getItem('accessPolicy');
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Employee Probation Report'
      );
    });

    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

  }

  ngOnInit(): void {
    this.employeeLogHistroyForm = this.fb.group({
      BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      reporter_name: [''],
      employee_name: [''],
    });
    setTimeout(() => {
      this.getBusinessUnit();
      setTimeout(() => {
        this.callLocation()
        setTimeout(() => {
          this.access_DD_department();
          setTimeout(() => {
            this.getAllEmployeeLogHistory();
          }, 200);
        }, 200);
      }, 200);
    }, 200);
    
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;

        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.getBusinessUnit();
          this.callLocation();
          setTimeout(() => {
            this.resetData();
          }, 200);
        }
        this.currentEntityId = newEntityId;
      });
  }
  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }

  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getBusinessUnitlist = [];
    setTimeout(() => {
      this.hrmsEmployeeModuleService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.getBusinessUnitlist = res;
          this.isSpinner = false;
        } else {
          this.isSpinner = false;
          this.getBusinessUnitlist = [];
        }
      },
        error => {
          this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
          this.isSpinner = false;
        })
    }, 100);
  }

  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getLocations = []
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.getLocations = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = []
        }
      },
        error => {
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner = false;
          this.getLocations = []
        })
    }, 100);
  }
  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;

    this.hrmsServiceMain.access_DD_department(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDepartementName = res;
        } else {
          this.triggerToast('', 'Record Not Found', 'Warning');
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Department List', 'danger');
        this.isSpinner = false;
      }
    });
  }
  callDDDesignation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.employeeLogHistroyForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
    this.employeeLogHistroyForm.patchValue({
      reporter_name: ''
    });
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe({
      next: (res: any) => {
        this.getDepartementRole = res;
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error loading Designation', 'danger');
        this.isSpinner = false;
      }
    });
  }

  callDDEmployee() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.employeeLogHistroyForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.employeeLogHistroyForm?.get('Location').value || 0),
      DeptId: Number(this.employeeLogHistroyForm?.get('DeptName').value || 0),
      DesignationId: Number(this.employeeLogHistroyForm?.get('Designation').value || 0),
      ReporterId: Number(this.employeeLogHistroyForm?.get('reporter_name').value || 0), 
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.DDEmployeeList(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length > 0) {
          this.getDropdownEmployee = res;
        } else {
          this.getDropdownEmployee = [];
          this.triggerToast('', 'No Data Found For Employee Name', 'warning')
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'No Data Found For Employee Name', 'danger');
        this.getDropdownEmployee = [];
        this.isSpinner = false;
      }
    })
  }

  callReporter() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.employeeLogHistroyForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.employeeLogHistroyForm?.get('Location').value || 0),
      DeptId: Number(this.employeeLogHistroyForm?.get('DeptName').value || 0),
      DesignationId: Number(this.employeeLogHistroyForm?.get('Designation').value || 0),
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.DDReporterList(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length > 0) {
          this.getDropdownReporter = res;
        } else {
          this.getDropdownReporter = [];
          this.triggerToast('', 'No Data Found For Reporter Name', 'warning')
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'No Data Found For Reporter Name', 'danger');
        this.getDropdownReporter = [];
        this.isSpinner = false;
      }
    })
  }

  formatDotNetDate(jsonDate: string | null): string {
    if (!jsonDate) return '-';

    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      const date = new Date(parseInt(match[1], 10));
      const day = date.getDate().toString().padStart(2, '0');
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const year = date.getFullYear();
      return `${day}-${month}-${year}`;
    }

    return '-';
  }
  getAllEmployeeLogHistory() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocId: 0,
      DeptId: 0,
      DesignationId: 0,
      ReporterId: 0
    };

    this.isSpinner = true;

    this.hrmsEmployeeModuleService
      .GetAllEmployeeLogHistory(reqBody)
      .subscribe({
        next: (res: any) => {
          if (res && res.length > 0) {
            this.rows = res;              // ✅ bind data to table
            this.isTableData = false;     // ✅ show table
          } else {
            this.rows = [];
            this.isTableData = true;      // ✅ show "no data"
            this.errorMessage = 'No records found';
          }

          this.isSpinner = false;
        },
        error: () => {
          this.isSpinner = false;
        }
      });
  }

  selectedEmployee: any = null;

  onView(row: any) {
    this.selectedEmployee = row;
  }

  resetData() {
    this.employeeLogHistroyForm.reset();
  }

  submitFilterData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.employeeLogHistroyForm?.get('BusinessUnit').value || 0),
      LocId: Number(this.employeeLogHistroyForm?.get('Location').value || 0),
      DeptId: Number(this.employeeLogHistroyForm?.get('DeptName').value || 0),
      DesignationId: Number(this.employeeLogHistroyForm?.get('Designation').value || 0),
      ReporterId: Number(this.employeeLogHistroyForm?.get('reporter_name')?.value || 0),
      EmpId: Number(this.employeeLogHistroyForm?.get('employee_name')?.value || 0)
    };

    this.isSpinner = true;

    this.hrmsEmployeeModuleService.GetAllEmployeeLogHistory(reqBody).subscribe({
      next: (res: any) => {

        console.log('API Response:', res);

        if (res && res.length > 0) {
          this.rows = res;              // ✅ bind data to table
          this.isTableData = false;     // ✅ show table
        } else {
          this.rows = [];
          this.isTableData = true;      // ✅ show "no data"
          this.errorMessage = 'No records found';
        }

        this.isSpinner = false;
      },

      error: (err: any) => {
        this.rows = [];
        this.isTableData = true;
        this.errorMessage = 'Error fetching data';
        this.isSpinner = false;
      }
    });
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
