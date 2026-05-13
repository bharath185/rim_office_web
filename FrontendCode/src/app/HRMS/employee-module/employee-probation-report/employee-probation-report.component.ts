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

@Component({
  selector: 'app-employee-probation-report',
  standalone: true,
  imports: [CommonModule, SharedModule, ReactiveFormsModule, ToastMessageComponent,
    RouterModule
  ],
  templateUrl: './employee-probation-report.component.html',
  styleUrl: './employee-probation-report.component.scss'
})
export class EmployeeProbationReportComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalReject') closeModalReject!: ElementRef;


  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  employeeDetails;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any;
  employeeProbationForm: any = FormGroup;
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;

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
    this.employeeProbationForm = this.fb.group({
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
            this.getAllEmpProbationTrackingHistory();
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
      DeptId: this.employeeProbationForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
    this.employeeProbationForm.patchValue({
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
      BUId: Number(this.employeeProbationForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.employeeProbationForm?.get('Location').value || 0),
      DeptId: Number(this.employeeProbationForm?.get('DeptName').value || 0),
      DesignationId: Number(this.employeeProbationForm?.get('Designation').value || 0),
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
      BUId: Number(this.employeeProbationForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.employeeProbationForm?.get('Location').value || 0),
      DeptId: Number(this.employeeProbationForm?.get('DeptName').value || 0),
      DesignationId: Number(this.employeeProbationForm?.get('Designation').value || 0),
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

  resetData() {
    this.employeeProbationForm.reset();
  }


  submitFilterData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.employeeProbationForm?.get('BusinessUnit').value || 0),
      LocId: Number(this.employeeProbationForm?.get('Location').value || 0),
      DeptId: Number(this.employeeProbationForm?.get('DeptName').value || 0),
      DesignationId: Number(this.employeeProbationForm?.get('Designation').value || 0),
      ReporterId: Number(this.employeeProbationForm?.get('reporter_name')?.value || 0),
      EmpId: Number(this.employeeProbationForm?.get('employee_name')?.value || 0)
    };

    this.isSpinner = true;
    this.hrmsEmployeeModuleService.GetAllEmpProbationTrackingHistory(reqBody).subscribe({
      next: (res: any) => {
        this.pendingList = res?.PendingProbationList || [];
        this.historyList = res?.ProbationHistoryList || [];
        this.isSpinner = false;
      }, error: (err: any) => {
        this.isSpinner = false;
      }

    })
  }

  pendingList: any[] = [];
  historyList: any[] = [];
  activeTab: string = 'pending';

  getAllEmpProbationTrackingHistory() {
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
      .GetAllEmpProbationTrackingHistory(reqBody)
      .subscribe({
        next: (res: any) => {
          this.pendingList = res?.PendingProbationList || [];
          this.historyList = res?.ProbationHistoryList || [];
          this.isSpinner = false;
        },
        error: () => {
          this.isSpinner = false;
        }
      });
  }

  selectedEmployees: any[] = [];
  selectAll: boolean = false;


  // Select / Unselect All
  toggleSelectAll() {
    this.pendingList.forEach(emp => {
      emp.selected = this.selectAll;
    });

    this.updateSelection();
  }


  // Update selected list
  updateSelection() {
    this.selectedEmployees = this.pendingList.filter(emp => emp.selected);

    // Update header checkbox state
    this.selectAll = this.pendingList.length > 0 &&
      this.pendingList.every(emp => emp.selected);
  }


  // Confirm button click
  confirmSelection() {
    if (this.selectedEmployees.length === 0) {
      this.triggerToast('Please select at least one record.', 'No Selection', 'warning');
      return;
    }
    // Open modal manually
    const modal = new (window as any).bootstrap.Modal(
      document.getElementById('rejectModal')
    );
    modal.show();
  }


  // Date formatter
  formatDate(dateStr: string): string {
    if (!dateStr) return '-';
    const timestamp = parseInt(dateStr.replace(/\/Date\((\d+)\)\//, '$1'), 10);
    const date = new Date(timestamp);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}-${month}-${year}`; // 👉 07-04-2026
  }
  rejectRemarks: string = '';
  isRecordDeleted: boolean = false;


  resetRemarksReject() {
    this.rejectRemarks = '';
  }

  rejectSelectedByHR() {
    if (this.selectedEmployees.length === 0) {
      this.triggerToast('Please select at least one record.', 'No Selection', 'warning');
      return;
    }

    if (!this.rejectRemarks || this.rejectRemarks.trim() === '') {
      this.triggerToast('Remarks are required.', 'Missing Remarks', 'warning');
      return;
    }

    const selectedIds = this.selectedEmployees.map(emp => emp.EmpId);

    // 👉 Check single vs multiple
    const empIdPayload = selectedIds.length === 1
      ? selectedIds[0]      // single
      : selectedIds;        // multiple

    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: empIdPayload,   // ✅ dynamic
      Remarks: this.rejectRemarks
    };

    console.log('Final Payload:', reqBody);
    console.log(reqBody)
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.ConfirmProbation(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], '', '');
          this.isRecordDeleted = true;
          this.isSpinner = false;
          setTimeout(() => {
            this.closeModalReject.nativeElement?.click();;
            this.resetRemarksReject();
            this.getAllEmpProbationTrackingHistory();
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
          this.isSpinner = false;
        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.isSpinner = false;
        }
      },
      error: (err: any) => {
        this.triggerToast(err['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
      },
      complete: () => {
        this.isSpinner = false;
      }
    });
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
