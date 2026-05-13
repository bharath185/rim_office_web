import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { payRollService } from '../../service/payroll.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { HrmsServiceService } from '../../hrms-service.service';
import { Subscription } from 'rxjs';
import { EntityStateService } from '../../service/entity-state.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { Modal } from 'bootstrap';


@Component({
  selector: 'app-add-emp-sal',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent,
    NgxPaginationModule
  ],
  templateUrl: './add-emp-sal.component.html',
  styleUrl: './add-emp-sal.component.scss'
})
export class AddEmpSalComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;
  @ViewChild('closeModal') closeModal!: ElementRef;


  employeeDetails;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  accessPolicy: any;
  controlAccessPage: any
  isSpinner1: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  isTableData: boolean = false;
  isTableDataForVariable: boolean = false;
  errorMessage: any;
  errorMessageForVariable: any;
  addEmployeeSalForm: any = FormGroup;
  getAllEmployeeSalForm: any = FormGroup;
  minStartDate!: string;
  maxStartDate!: string;
  getListOfAllEmployeeSalaryDetails: any = [];
  searchValue: string = '';
  originalEmployeeList: any[] = [];
  isEdited: boolean = false;
  isCardOpen = false;
  patchEmpSalDetails: any;
  isRecordDeletedCommon: boolean = false;

  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;
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

  years: number[] = [];
  months: { id: number, name: string }[] = [];
  selectedYear!: number;
  selectedMonth: any;


  constructor(private payrollService: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly fb: FormBuilder,
    private readonly hrmsService: HrmsServiceService,
    private entityStateService: EntityStateService,
    private readonly hrmsEmployeeModuleService: EmployeeModuleService,
    private readonly payrollLocationDD: payRollService,
    private readonly hrmsServiceMain: HrmsServiceService,


  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Salary Management'
      );
    });
    const currentDate = new Date();
    this.minStartDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, currentDate.getDate()));
    const nextYear = new Date(currentDate.getFullYear() + 1, currentDate.getMonth(), currentDate.getDate());
    this.maxStartDate = this.formatDate(nextYear);
  }

  ngOnInit(): void {

    this.getAllEmployeeSalForm = this.fb.group({
      BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      reporter_name: [''],
      employee_name: [''],
    });

    this.addEmployeeSalForm = this.fb.group({
      emloyee: ['', [Validators.required]],
      emloyeeCode: ['', [Validators.required]],
      CTC: ['', [Validators.required]],
      date_from: ['', [Validators.required]],
      // date_to: ['', [Validators.required]],
      appraised: [false],

      IsArrear: [false],
      salaryType: [''],
      variable: [''],
      variableAmt: [''],
      Period: [''],
      arrearAmt: [''],
      PendingMonth: [''],
      DescriptionforArrear: [''],
      year: [],
      month: [],
    });

    setTimeout(() => {
      this.getBusinessUnit();
      setTimeout(() => {
        this.callLocation()
        setTimeout(() => {
          this.access_DD_department();
          setTimeout(() => {
            this.getAllEmployeeSalaryDetails();
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
          this.getBusinessUnit();
          this.callLocation();
          setTimeout(() => {
            this.resetData();
            this.resetFilterData();
          }, 200);
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
      DeptId: this.getAllEmployeeSalForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
    this.getAllEmployeeSalForm.patchValue({
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
      BUId: Number(this.getAllEmployeeSalForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.getAllEmployeeSalForm?.get('Location').value || 0),
      DeptId: Number(this.getAllEmployeeSalForm?.get('DeptName').value || 0),
      DesignationId: Number(this.getAllEmployeeSalForm?.get('Designation').value || 0),
      ReporterId: Number(this.getAllEmployeeSalForm?.get('reporter_name').value || 0),
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
      BUId: Number(this.getAllEmployeeSalForm?.get('BusinessUnit').value || 0),
      LocationId: Number(this.getAllEmployeeSalForm?.get('Location').value || 0),
      DeptId: Number(this.getAllEmployeeSalForm?.get('DeptName').value || 0),
      DesignationId: Number(this.getAllEmployeeSalForm?.get('Designation').value || 0),
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

  resetFilterData() {
    this.getAllEmployeeSalForm.reset();
    this.getAllEmployeeSalaryDetails();
  }

  submitFilterData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.getAllEmployeeSalForm?.get('BusinessUnit').value || 0),
      LocId: Number(this.getAllEmployeeSalForm?.get('Location').value || 0),
      DeptId: Number(this.getAllEmployeeSalForm?.get('DeptName').value || 0),
      DesignationId: Number(this.getAllEmployeeSalForm?.get('Designation').value || 0),
      ReporterId: Number(this.getAllEmployeeSalForm?.get('reporter_name')?.value || 0),
      EmpId: Number(this.getAllEmployeeSalForm?.get('employee_name')?.value || 0)
    };
    this.isSpinner = true;
    this.payrollService.GetAllEmployeeSalaryDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.originalEmployeeList = res;

          this.getListOfAllEmployeeSalaryDetails = res.map((row: any) => {
            const from = this.parseJsonDate(row.EffectiveFromDate);
            const to = this.parseJsonDate(row.EffectiveToDate);
            row.EffectiveFromDate = from ? this.formatDate(from) : '';
            row.EffectiveToDate = to ? this.formatDate(to) : '';
            return row;
          });
          // Store formatted rows also in original list
          this.originalEmployeeList = JSON.parse(JSON.stringify(this.getListOfAllEmployeeSalaryDetails));
          this.isTableData = false;
        }
        else {
          this.errorMessage = 'No Data Found';
          this.getListOfAllEmployeeSalaryDetails = [];
          this.isTableData = true;
        }
        this.isSpinner = false;
      },
      error: () => {
        this.isSpinner = false;
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
      }
    })
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    // return `${year}-${month}-${day}`;
    return `${day}-${month}-${year}`;
  }
  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      return new Date(parseInt(match[1], 10));
    }
    return null;
  }
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }
  onFromDate(): void {
    const fromDate = this.addEmployeeSalForm.get('date_from')?.value;
    if (fromDate) {
      this.minStartDate = fromDate;
    }
  }
  onToDate(): void {
    const toDate = this.addEmployeeSalForm.get('date_to')?.value;
    if (toDate) {
      this.maxStartDate = toDate;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.addEmployeeSalForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.addEmployeeSalForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.addEmployeeSalForm.hasError('dateRange');
  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  getAllEmployeeSalaryDetails() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.GetAllEmployeeSalaryDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.originalEmployeeList = res;

          this.getListOfAllEmployeeSalaryDetails = res.map((row: any) => {
            const from = this.parseJsonDate(row.EffectiveFromDate);
            const to = this.parseJsonDate(row.EffectiveToDate);
            row.EffectiveFromDate = from ? this.formatDate(from) : '';
            row.EffectiveToDate = to ? this.formatDate(to) : '';
            return row;
          });
          // Store formatted rows also in original list
          this.originalEmployeeList = JSON.parse(JSON.stringify(this.getListOfAllEmployeeSalaryDetails));
          this.isTableData = false;
        }
        else {
          this.errorMessage = 'No Data Found';
          this.getListOfAllEmployeeSalaryDetails = [];
          this.isTableData = true;
        }
        this.isSpinner = false;
      },
      error: () => {
        this.isSpinner = false;
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
      }
    })
  }
  applyFilter() {
    const val = this.searchValue.toLowerCase().trim();
    this.getListOfAllEmployeeSalaryDetails = this.originalEmployeeList.filter((row: any) => {
      return (
        row.FirstName?.toLowerCase().includes(val) ||
        row.EmpCode?.toLowerCase().includes(val) ||
        row.CTC?.toString().includes(val) ||
        row.EffectiveFromDate?.toLowerCase().includes(val) ||
        row.EffectiveToDate?.toLowerCase().includes(val) ||
        (row.IsAppraised ? 'yes' : 'no').includes(val)
      );
    });

    // Reset pagination on new search
    this.page = 1;
  }

  FilterTable() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: ''
    }
    this.isSpinner = true;
    this.payrollService.GetEmployeeSalaryDetails(reqBody).subscribe(({
      next: (res: any) => {
        this.isSpinner = false;
      }, error: (err: any) => {
        this.isSpinner = false;
      }
    }))
  }

  openModal(): void {
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
    this.getEmployeeSelectEmployee();

  }

  getDropdownVariable: any = [];
  onSalaryTypeChange() {
    const type = this.addEmployeeSalForm.get('salaryType')?.value;
    if (type === 'variable') {
      this.addEmployeeSalForm.get('variable')?.addValidators(Validators.required);
      this.addEmployeeSalForm.get('variableAmt')?.addValidators(Validators.required);
      this.addEmployeeSalForm.get('Period')?.addValidators(Validators.required);
      // this.addEmployeeSalForm.get('arrearAmt')?.addValidators(Validators.required);
      this.DDPayrollVariable();
      setTimeout(() => {
        this.GetAllPayrollVariable();
      }, 100);
    } else {
      this.addEmployeeSalForm.get('variable')?.clearValidators();
      this.addEmployeeSalForm.get('variableAmt')?.clearValidators();
      this.addEmployeeSalForm.get('Period')?.clearValidators();
      // this.addEmployeeSalForm.get('arrearAmt')?.clearValidators();
    }

    this.addEmployeeSalForm.get('variable')?.updateValueAndValidity();
    this.addEmployeeSalForm.get('variableAmt')?.updateValueAndValidity();
    this.addEmployeeSalForm.get('Period')?.updateValueAndValidity();
    // this.addEmployeeSalForm.get('arrekarAmt')?.updateValueAndValidity();
  }
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
  gelAllVariableData: any = [];
  GetAllPayrollVariable() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.payrollService.GetAllPayrollVariable(reqBody).subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.gelAllVariableData = res;
          this.isTableDataForVariable = false;
        } else {
          this.errorMessageForVariable = 'No Data Found';
          this.gelAllVariableData = [];
          this.isTableDataForVariable = true;
        }
        this.isSpinner = false;
      }
    })
  }
  modalType: string = '';

  variableObj = {
    VariableId: 0,
    VariableName: '',
    VariableCode: ''
  };

  isEditMode = false;

  openAddModal(type: string) {
    this.modalType = type;
    this.variableObj = {
      VariableId: 0,
      VariableName: '',
      VariableCode: ''
    };
    const modalEl = document.getElementById('inputModal');
    const modal = (window as any).bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
    // 🔥 IMPORTANT: delay focus until modal fully stabilizes
    setTimeout(() => {
      const input = document.getElementById('variableName') as HTMLElement;
      input?.blur(); // remove forced focus trap loop
    }, 300);
  }
  closeModalVariable() {
    const modalEl = document.getElementById('inputModal');
    const modal = (window as any).bootstrap.Modal.getInstance(modalEl);
    modal?.hide();
    // 🔥 destroy internal state
    setTimeout(() => {
      (window as any).bootstrap.Modal.getOrCreateInstance(modalEl).dispose?.();
    }, 300);
  }
  onVariableChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    if (value === 'createNew') {
      this.closeModal.nativeElement?.click();
      setTimeout(() => {
        this.openAddModal('variable');
      }, 200);
    }
  }

  openEditModal(data: any) {
    this.isEditMode = true;
    this.DDPayrollVariable();
    setTimeout(() => {
      this.GetAllPayrollVariable();
    }, 100);
    this.variableObj = {
      VariableId: data.VariableId,
      VariableName: data.VariableName,
      VariableCode: data.VariableCode
    };

    const modalEl = document.getElementById('inputModal');
    const modal = (window as any).bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();

  }
  saveVariable() {

    if (!this.variableObj?.VariableName || !this.variableObj?.VariableCode) {
      return;
    }

    const payload = {
      VariableId: this.variableObj.VariableId,
      VariableName: this.variableObj.VariableName,
      VariableCode: this.variableObj.VariableCode
    };

    if (this.isEditMode) {

      this.payrollService.UpdatePayrollVariable(payload).subscribe({
        next: (res: any) => {
          console.log('Updated successfully', res);
          this.afterSave();
        },
        error: (err: any) => {
          console.error('Update error', err);
        }
      });

    } else {

      this.payrollService.AddPayrollVariable(payload).subscribe({
        next: (res: any) => {
          console.log('Added successfully', res);
          this.afterSave();
        },
        error: (err: any) => {
          console.error('Add error', err);
        }
      });
    }
  }
  afterSave() {
    this.variableObj = {
      VariableId: 0,
      VariableName: '',
      VariableCode: ''
    };
    this.isEditMode = false;
    this.GetAllPayrollVariable();
  }
  deleteVariable(row: any) {

    const confirmDelete = window.confirm(
      'Are you sure you want to delete this variable? This action cannot be undone.'
    );

    if (!confirmDelete) {
      return; // user clicked Cancel
    }

    const reqBody = {
      VariableId: row.VariableId,
      LoginId: this.employeeDetails[0].LoginId
    };

    this.isSpinner1 = true;

    this.payrollService.DeletePayrollVariable(reqBody).subscribe({
      next: (res: any) => {

        if (res?.msg) {
          this.triggerToast('Success', res.msg, '');
          this.GetAllPayrollVariable();
        }

        this.isSpinner1 = false;
      },
      error: (err: any) => {
        this.isSpinner1 = false;
        this.triggerToast('Error', 'Delete failed', 'danger');
      }
    });
  }

  submitAddSalForm() {
    if (this.addEmployeeSalForm.valid) {
      const ctc = Number(this.addEmployeeSalForm?.get('CTC').value || 0);
      const salaryType = this.addEmployeeSalForm?.get('salaryType').value;

      const selectedVariable = this.getDropdownVariable.find(
        (v: any) => v.VariableCode === this.addEmployeeSalForm?.get('variable').value
      );

      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.selectedEmployee,
        EmpCode: this.addEmployeeSalForm.get('emloyeeCode').value,
        CTC: ctc,
        MCTC: 0,
        EffectiveFromDate: this.addEmployeeSalForm?.get('date_from').value,
        EffectiveToDate: null,
        IsAppraised: this.addEmployeeSalForm?.get('appraised').value,
        ArrearAmt: this.addEmployeeSalForm?.get('arrearAmt').value || 0,
        IsArrear: this.addEmployeeSalForm?.get('IsArrear').value || false,

        VariableName: selectedVariable?.VariableName || '',
        VariableCode: selectedVariable?.VariableCode || '',

        Period: this.addEmployeeSalForm?.get('Period').value || '',
        VariableAmt: this.addEmployeeSalForm?.get('variableAmt').value || 0,
        PendingMonth: Number(this.addEmployeeSalForm?.get('PendingMonth').value || 0),
        DescriptionforArrear: this.addEmployeeSalForm?.get('DescriptionforArrear').value || '',

        IsFixed: salaryType === 'fixed',
        IsVariable: salaryType === 'variable',

        ArrearYear: Number(this.addEmployeeSalForm?.get('year').value || 0),
        ArrearMonth: Number(this.addEmployeeSalForm?.get('month').value || 0),

      };
      console.log(reqBody)
      this.isSpinner = true;
      this.payrollService.AddEmployeeSalaryDetails(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], 'success');
            setTimeout(() => {
              this.closeModal.nativeElement?.click();
              this.getAllEmployeeSalaryDetails();
              this.resetData();
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
    } else {
      this.isFormSubmitted = true;
    }
  }

  resetData() {
    this.addEmployeeSalForm.reset();
    this.isFormSubmitted = false;
    this.isEdited = false
  }
  resetFilter() {

  }
  convertToISO(dateStr: string): string | null {
    if (!dateStr) return null;
    const parts = dateStr.split('-');  // ["07","12","2023"]
    if (parts.length !== 3) return null;

    const [day, month, year] = parts;
    return `${year}-${month}-${day}`;   // "2023-12-07"
  }

  patchVlaues(data: any, edited: boolean) {
    console.log(data)
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
    this.isEdited = edited;
    this.patchEmpSalDetails = data;
    // salary type mapping
    let salaryType = '';
    if (this.patchEmpSalDetails.IsFixed) {
      salaryType = 'fixed';
    } else if (this.patchEmpSalDetails.IsVariable) {
      salaryType = 'variable';
      this.DDPayrollVariable();
    }

    const year = Number(this.patchEmpSalDetails.ArrearYear);

    // 1️⃣ set year first
    this.addEmployeeSalForm.patchValue({
      year: year
    });

    // 2️⃣ manually trigger month loading
    this.selectedYear = year;
    this.onYearChange(); // 👈 VERY IMPORTANT

    const monthId = String(this.patchEmpSalDetails.ArrearMonth); // 👈 STRING
    this.selectedMonth = monthId; // for ngModel
    const fromDate = this.convertToISO(this.patchEmpSalDetails.EffectiveFromDate);

    setTimeout(() => {
      this.addEmployeeSalForm.patchValue({
        emloyee: this.patchEmpSalDetails.FirstName,
        emloyeeCode: this.patchEmpSalDetails.EmpCode,
        CTC: this.patchEmpSalDetails.CTC,
        MCTC: this.patchEmpSalDetails.MCTC,
        date_from: fromDate,
        appraised: this.patchEmpSalDetails.IsAppraised,

        PendingMonth: this.patchEmpSalDetails.PendingMonth,
        DescriptionforArrear: this.patchEmpSalDetails.DescriptionforArrear,

        salaryType: salaryType,
        variable: this.patchEmpSalDetails.VariableCode || '',
        Period: this.patchEmpSalDetails.Period || '',
        variableAmt: this.patchEmpSalDetails.VariableAmt || 0,
        arrearAmt: this.patchEmpSalDetails.ArrearAmt || 0,
        IsArrear: this.patchEmpSalDetails.IsArrear || false,

        year: this.patchEmpSalDetails.ArrearYear,
        month: monthId
      });
    }, 100);

    this.isCardOpen = true;
  }

  updateAddForm() {

    const salaryType = this.addEmployeeSalForm?.get('salaryType').value;

    const selectedVariable = this.getDropdownVariable.find(
      (v: any) => v.VariableCode === this.addEmployeeSalForm?.get('variable').value
    );

    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.patchEmpSalDetails.EmpId ? this.patchEmpSalDetails.EmpId : this.selectedEmployee,
      SalaryId: this.patchEmpSalDetails.SalaryId,
      RecordStatus: this.patchEmpSalDetails.RecordStatus,

      EmpCode: this.addEmployeeSalForm.get('emloyeeCode').value,
      CTC: this.addEmployeeSalForm?.get('CTC').value,
      MCTC: this.patchEmpSalDetails.MCTC,
      IncrementPercent: this.patchEmpSalDetails.IncrementPercent,
      PerviousCTC: this.patchEmpSalDetails.PerviousCTC,

      EffectiveFromDate: this.addEmployeeSalForm?.get('date_from').value,
      EffectiveToDate: this.patchEmpSalDetails.EffectiveToDate,

      IsAppraised: this.addEmployeeSalForm?.get('appraised').value,

      // ✅ salary type mapping
      IsFixed: salaryType === 'fixed',
      IsVariable: salaryType === 'variable',

      // ✅ variable details
      VariableName: selectedVariable?.VariableName || '',
      VariableCode: selectedVariable?.VariableCode || '',
      VariableAmt: this.addEmployeeSalForm?.get('variableAmt').value || 0,
      Period: this.addEmployeeSalForm?.get('Period').value || '',
      ArrearAmt: this.addEmployeeSalForm?.get('arrearAmt').value || 0,
      IsArrear: this.addEmployeeSalForm?.get('IsArrear').value || false,
      PendingMonth: Number(this.addEmployeeSalForm?.get('PendingMonth').value || 0),
      DescriptionforArrear: this.addEmployeeSalForm?.get('DescriptionforArrear').value || '',
      ArrearYear: Number(this.addEmployeeSalForm?.get('year').value || 0),
      ArrearMonth: Number(this.addEmployeeSalForm.get('month').value) || 0,
    };

    console.log(reqBody);

    this.isSpinner = true;

    this.payrollService.UpdateEmployeeSalaryDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isSpinner = false;

          setTimeout(() => {
            this.closeModal.nativeElement?.click();
            this.getAllEmployeeSalaryDetails();
            this.resetData();
          }, 100);

        } else if (res['Message']) {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
          this.isSpinner = false;
        }
      },
      error: () => {
        this.triggerToast('', 'Internal Server Error', 'danger');
        this.isSpinner = false;
      }
    });
  }

  getDeleteEmpSalDetail: any
  confirmDelete(row: any) {
    console.log(row);
    this.getDeleteEmpSalDetail = row;
  }

  deleteRecord() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      SalaryId: this.getDeleteEmpSalDetail.SalaryId
    }
    this.isSpinner1 = true;
    this.payrollService.DeleteEmployeeSalaryDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isRecordDeletedCommon = true;
          setTimeout(() => {
            this.closeModalDelete.nativeElement?.click();
            this.getAllEmployeeSalaryDetails();
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


  //this is Employee list
  getEmployeeSelectEmployee() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsService.visitorAccessDDEmployee(reqBody).subscribe({
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
    this.addEmployeeSalForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.addEmployeeSalForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.addEmployeeSalForm.get('emloyee')?.setErrors(null);
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

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
