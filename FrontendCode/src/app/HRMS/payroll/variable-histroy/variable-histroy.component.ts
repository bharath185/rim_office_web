import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { payRollService } from '../../service/payroll.service';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs';
import { Modal } from 'bootstrap';


@Component({
  selector: 'app-variable-histroy',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent,
    NgxPaginationModule],
  templateUrl: './variable-histroy.component.html',
  styleUrl: './variable-histroy.component.scss'
})
export class VariableHistroyComponent implements OnInit {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;

  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  errorMessage: any;
  payrollVariableForm: any = FormGroup;
  addPayrollVariableSalForm: any = FormGroup;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  getDropdownReporter: any[] = [];
  getDropdownEmployee: any[] = [];
  isTableData: boolean = false
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  originalRows: any[] = [];
  rows: any[] = [];
  searchValue: string = '';
  years: number[] = [];
  months: { id: number, name: string }[] = [];
  selectedYear!: number;
  selectedMonth: any;
  getDropdownVariable: any = [];
  isEdited: boolean = false;


  constructor(private payrollService: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly fb: FormBuilder,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly hrmsEmployeeModuleService: EmployeeModuleService,
    private readonly payrollLocationDD: payRollService,

    private entityStateService: EntityStateService) {
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
    this.payrollVariableForm = this.fb.group({
      BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      reporter_name: [''],
      employee_name: [''],
      year: [''],
      month: [''],
    });

    this.addPayrollVariableSalForm = this.fb.group({
      emloyee: ['', [Validators.required]],
      emloyeeCode: ['', [Validators.required]],
      CTC: ['', [Validators.required]],

      variable: [''],
      variableAmt: [''],
      VariableYear: [''],
      VariableMonth: ['']
    });


    setTimeout(() => {
      this.getBusinessUnit();
      setTimeout(() => {
        this.callLocation()
        setTimeout(() => {
          this.access_DD_department();
          setTimeout(() => {
            this.getPayrollVariableHistory();
          }, 200);
        }, 200);
      }, 200);
    }, 200);
    const currentYear = new Date().getFullYear();
    for (let yr = 2020; yr <= currentYear; yr++) {
      this.years.push(yr);
    }

    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;

        if (this.currentEntityId && this.currentEntityId !== newEntityId) {


        }

        this.currentEntityId = newEntityId;
      });
  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }

  getAllMonths() {
    return [
      { id: 1, name: 'January' }, { id: 2, name: 'February' },
      { id: 3, name: 'March' }, { id: 4, name: 'April' },
      { id: 5, name: 'May' }, { id: 6, name: 'June' },
      { id: 7, name: 'July' }, { id: 8, name: 'August' },
      { id: 9, name: 'September' }, { id: 10, name: 'October' },
      { id: 11, name: 'November' }, { id: 12, name: 'December' }
    ];
  }
  getMonthName(monthId: number): string {
    const month = this.getAllMonths().find(m => m.id === monthId);
    return month ? month.name : '';
  }

  onYearChange() {
    this.selectedYear = Number(this.selectedYear);
    this.selectedMonth = '';
    const currentYear = new Date().getFullYear();
    const currentMonth = new Date().getMonth() + 1;
    const allMonths = this.getAllMonths();
    if (this.selectedYear === currentYear) {
      this.months = allMonths.filter(m => m.id <= currentMonth);
    } else {
      this.months = allMonths;
    }
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
      DeptId: this.payrollVariableForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
    this.payrollVariableForm.patchValue({
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
      BUId: Number(this.payrollVariableForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.payrollVariableForm?.get('Location').value || 0),
      DeptId: Number(this.payrollVariableForm?.get('DeptName').value || 0),
      DesignationId: Number(this.payrollVariableForm?.get('Designation').value || 0),
      ReporterId: Number(this.payrollVariableForm?.get('reporter_name').value || 0),
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
      BUId: Number(this.payrollVariableForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.payrollVariableForm?.get('Location').value || 0),
      DeptId: Number(this.payrollVariableForm?.get('DeptName').value || 0),
      DesignationId: Number(this.payrollVariableForm?.get('Designation').value || 0),
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

  openModal(): void {
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
    this.getEmployeeSelectEmployee();
    this.DDPayrollVariable();
  }
  getPayrollVariableHistory() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocationId: 0,
      DeptId: 0,
      DesignationId: 0,
      ReporterId: 0,
      EmpId: 0,
    };

    this.isSpinner = true;

    this.payrollService.PayrollVariableHistory(reqBody).subscribe(
      (res: any) => {
        this.isSpinner = false;

        if (res && res.length > 0) {
          // bind to table
          this.rows = res;
          this.originalRows = this.rows;
          this.isTableData = false;
        } else {
          this.rows = [];
          this.originalRows = [];
          this.isTableData = true;
          this.errorMessage = "No data found";
        }
      },
      error => {
        this.isSpinner = false;
        this.triggerToast('Internal Server Error', 'Error loading List Of Data', "danger");
      }
    );
  }

  resetData() {
    this.payrollVariableForm.reset();
    this.searchValue = '';
    this.getPayrollVariableHistory(); // enough, don't touch isTableData here
  }

  filterData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.payrollVariableForm?.get('BusinessUnit').value || 0),
      LocId: Number(this.payrollVariableForm?.get('Location').value || 0),
      DeptId: Number(this.payrollVariableForm?.get('DeptName').value || 0),
      DesignationId: Number(this.payrollVariableForm?.get('Designation').value || 0),
      ReporterId: Number(this.payrollVariableForm?.get('reporter_name')?.value || 0),
      EmpId: Number(this.payrollVariableForm?.get('employee_name')?.value || 0),
      Year: Number(this.payrollVariableForm?.get('year')?.value || 0),
      Month: Number(this.payrollVariableForm?.get('month')?.value || 0),
    };

    this.isSpinner = true;

    this.payrollService.PayrollVariableHistory(reqBody).subscribe(
      (res: any) => {
        this.isSpinner = false;

        if (res && res.length > 0) {
          this.rows = res;
          this.originalRows = this.rows;
          this.isTableData = false;

        } else {
          this.rows = [];
          this.originalRows = [];

          this.isTableData = true;
          this.errorMessage = "No data found";
        }
      },
      error => {
        this.isSpinner = false;
        this.triggerToast('Internal Server Error', 'Error loading List Of Data', "danger");
      }
    );
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) => {
        const EmpName = row.EmpName?.toUpperCase() || '';
        const EmpCode = row.EmpCode?.toUpperCase() || '';

        return (
          EmpName.includes(filterValue) ||
          EmpCode.includes(filterValue)

        );
      });
    } else {
      this.rows = [...this.originalRows];
      this.isTableData = false;
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


  //this is Employee list
  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;

  getEmployeeSelectEmployee() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsServiceMain.visitorAccessDDEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.errorMessageEmpName = ''
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
          this.errorMessageEmpName = 'No Data Found.'
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessageEmpName = 'Error loading data. Please try again.';
        this.isSpinner = false;
      }
    });
  }
  filterEmployees() {
    if (this.searchText) {
      this.filteredEmployees = this.employees.filter((employee: any) =>
        employee.EmpName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        employee.EmpCode.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employees];
    }
  }
  selectEmployeee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.addPayrollVariableSalForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.addPayrollVariableSalForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.addPayrollVariableSalForm.get('emloyee')?.setErrors(null);
    }
  }
  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  openDropdown() {
    this.isDropdownOpen = true;
    this.filteredEmployees = [...this.employees];
  }
  closeDropdown() {
    setTimeout(() => {
      this.isDropdownOpen = false;
    }, 200);
  }
  //this is Employee list

  DDPayrollVariable() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.payrollService.DDPayrollVariable(reqBody).subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.getDropdownVariable = res;

        } else {
          this.triggerToast('', "No Data For Variable Value", "warning");
        }
        this.isSpinner = false;
      }
    })
  }

  submitAddVarialForm() {
    if (this.addPayrollVariableSalForm.valid) {
      const ctc = Number(this.addPayrollVariableSalForm?.get('CTC').value || 0);
      const selectedVariable = this.getDropdownVariable.find(
        (v: any) => v.VariableCode === this.addPayrollVariableSalForm?.get('variable').value
      );
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.selectedEmployee,
        EmpCode: this.addPayrollVariableSalForm.get('emloyeeCode').value,
        EmpCTC: ctc,
        VariableName: selectedVariable?.VariableName || '',
        VariableCode: selectedVariable?.VariableCode || '',
        VariableId: selectedVariable?.VariableId || 0,
        VariableAmt: Number(this.addPayrollVariableSalForm.get('variableAmt').value),
        Year: this.addPayrollVariableSalForm.get('VariableYear').value,
        Month: Number(this.addPayrollVariableSalForm.get('VariableMonth').value),
      }
      this.isSpinner = true;
      this.payrollService.AddPayrollVariableHistory(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], 'success');
            setTimeout(() => {
              this.getPayrollVariableHistory();
              this.closeModal.nativeElement?.click();
              this.resetDataModal();
            }, 100);
          } else {
            this.triggerToast(res['Message'], "Something went wrong", "warning");
            this.isSpinner = false;
          }
        }, error: (err: any) => {
          this.triggerToast('Failed To Add Record', 'Internal Server Error', 'danger');
          this.isSpinner = false;
        }
      })
    }
  }
  resetDataModal() {
    this.addPayrollVariableSalForm.reset();
  }
  patchEmpSalDetails: any;

  patchVlaues(data: any, edited: boolean) {
    this.getEmployeeSelectEmployee();
    this.DDPayrollVariable();
    this.months = this.getAllMonths();
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();


    this.isEdited = edited;
    this.patchEmpSalDetails = data;

    this.addPayrollVariableSalForm.patchValue({
      emloyee: (data.EmpName || '').replace(/\s+/g, ' ').trim(),
      emloyeeCode: data.EmpCode,
      CTC: data.EmpCTC,

      variable: data.VariableName,
      variableAmt: data.VariableAmt,

      VariableYear: Number(data.Year),
      VariableMonth: Number(data.Month)
    });
  }

  confirmDelete(row: any) {
    const confirmAction = window.confirm(
      `Are you sure you want to delete this record?\n\nEmployee: ${row.EmpName}`
    );

    if (confirmAction) {
      this.deletePayrollVariableHistory(row);
    }
  }
  deletePayrollVariableHistory(row: any) {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      VariableHistoryId: row.VariableHistoryId
    };

    this.isSpinner = true;

    this.payrollService.DeletePayrollVariableHistory(reqBody).subscribe(
      (res: any) => {
        if (res['msg']) {
          // success message
          this.triggerToast(
            'Success',
            'Record deleted successfully',
            'success'
          );

          // refresh table
          this.getPayrollVariableHistory();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], '', 'warning')
        }
        this.isSpinner = false;


      },
      error => {
        this.isSpinner = false;
        this.triggerToast(
          'Internal Server Error',
          'Failed to delete record',
          'danger'
        );
      }
    );
  }

  updateAddVarialForm() {
    if (this.addPayrollVariableSalForm.valid) {
      const ctc = Number(this.addPayrollVariableSalForm?.get('CTC').value || 0);
      const selectedVariable = this.getDropdownVariable.find(
        (v: any) => v.VariableCode === this.addPayrollVariableSalForm?.get('variable').value
      );
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.selectedEmployee || this.patchEmpSalDetails.EmpId,
        EmpCode: this.addPayrollVariableSalForm.get('emloyeeCode').value,
        EmpCTC: ctc,
        VariableHistoryId: this.patchEmpSalDetails.VariableHistoryId,
        VariableName: selectedVariable?.VariableName || '',
        VariableCode: selectedVariable?.VariableCode || '',
        VariableId: selectedVariable?.VariableId || 0,
        VariableAmt: Number(this.addPayrollVariableSalForm.get('variableAmt').value),
        Year: this.addPayrollVariableSalForm.get('VariableYear').value,
        Month: Number(this.addPayrollVariableSalForm.get('VariableMonth').value),
      }
      this.isSpinner = true;
      this.payrollService.UpdatePayrollVariableHistory(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], 'success');
            setTimeout(() => {
              this.getPayrollVariableHistory();
              this.closeModal.nativeElement?.click();
              this.resetDataModal();
            }, 100);
          } else if (res['Message']) {
            this.triggerToast(res['Message'], "", "warning");
            this.isSpinner = false;
          }
        }, error: (err: any) => {
          this.triggerToast('Failed To Add Record', 'Internal Server Error', 'danger');
          this.isSpinner = false;
        }
      })
    }
  }


  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

}
