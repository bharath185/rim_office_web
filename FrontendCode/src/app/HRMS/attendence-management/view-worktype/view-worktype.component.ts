import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { AttendenceModuleService } from '../../service/attendence.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
@Component({
  selector: 'app-view-worktype',
  standalone: true,
  imports: [CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule,
    RouterModule
  ],
  templateUrl: './view-worktype.component.html',
  styleUrl: './view-worktype.component.scss'
})
export class ViewWorktypeComponent implements OnInit, OnDestroy {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue: any = ElementRef;
  @ViewChild('closeModal') closeModal: any = ElementRef;


  constructor(private readonly fb: FormBuilder, private readonly attendenceService: AttendenceModuleService,
    private readonly cdr: ChangeDetectorRef, private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    // console.log('Employee Details', this.employeeDetails);

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // // console.log(this.accessPolicy);
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'View Work Type'
    // );
    // this.controlAccessPage=viewEmployeeAccess;
    // console.log(this.controlAccessPage);

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'View Work Type'
      );
    });
  }
 
  accessPolicy:any;
  controlAccessPage:any
  viewWorkTypeForm: any = FormGroup;
  actionForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  minDate: string | undefined;
  maxDate: string | undefined;
  today = new Date().toISOString().split('T')[0];
  isSpinner: boolean = false;
  employeeDetails: any;
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  patchValue: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100,500];;
  showDropdown: boolean = false;
  selectedEmpId: any;
  isApproveMode: boolean = true;
  employees: any[] = [];
  isValidEmployee: boolean = true;
  searchText: string = '';
  isDropdownOpen = false;
  filteredEmployees: any[] = [];
  dropdownVisible = false;  // Track visibility of the dropdown
  dropdownTarget: HTMLElement | null = null;

  ngOnInit(): void {
    setTimeout(() => {
      this.GetAllApproverWorkType();
      setTimeout(() => {
        this.getDDEmployeeName();
      }, 100);
    }, 100);
    this.viewWorkTypeFormVal();
    this.actionForm = this.fb.group({
      description: ['']
    });
    document.addEventListener('click', this.closeDropdownNew.bind(this));
  }


  viewWorkTypeFormVal() {
    this.viewWorkTypeForm = this.fb.group({
      employeeName: [''],
      employee_code: [''],
      date_from: [''],
      date_to: [''],
      staus: ['']

    }, { validators: this.dateRangeValidator });
    this.viewWorkTypeForm?.get('date_from')?.valueChanges.subscribe((value: any) => {
      if (value) {
        this.viewWorkTypeForm?.get('date_to')?.setValidators([Validators.required]);
      } else {
        this.viewWorkTypeForm?.get('date_to')?.clearValidators();
      }
      this.viewWorkTypeForm?.get('date_to')?.updateValueAndValidity();
    });

  }

  GetAllApproverWorkType() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.attendenceService.EmployeeGetAllApproverWorkType(reqBody).subscribe((res: any) => {
      if (res && res.length >= 1) {
        this.rows = res;
        setTimeout(() => {
          const formattedData = res.map((item: any) => ({
            ...item,
            StartDate: this.formatDate(this.parseJsonDate(item.StartDate)),
            EndDate: this.formatDate(this.parseJsonDate(item.EndDate)),
          }));
          this.rows = [...formattedData];
          this.originalRows = [...formattedData]; // Ensure originalRows has formatted dates too
        }, 100);
        this.isSpinner = false;
        this.isTableData = false;
      } else {
        this.errorMessage = "No records found";
        this.isSpinner = false;
        this.isTableData = true;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Failed To Load The Approver Data', "danger");
      this.errorMessage = "Internal Server Error";
      this.isSpinner = false;
      this.isTableData = true;
    });
  }

  // This is the Second code of Employee Name
  getDDEmployeeName() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.attendenceService.EmployeeDDEmployeeApprover(reqBody).subscribe((res: any) => {
      if (res && res.length >= 1) {
        this.employees = res;
        this.isSpinner = false;
      } else {
        this.isSpinner = false;
        this.triggerToast('No data Found ', 'To Load The Employee Name', "warning");
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Failed To Load The Employee Name', "danger");
      this.isSpinner = false;
    });
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

  selectEmployee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmpId = employee.EmpId;
    this.viewWorkTypeForm.get('employee_code')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }

  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.viewWorkTypeForm.get('employeeName')?.setErrors({ invalidEmployee: true });
    } else {
      this.viewWorkTypeForm.get('employeeName')?.setErrors(null);
    }
  }
  // This is the Second code of Employee Name

  submitFormData() {
    this.isFormSubmitted = true;
    if (this.viewWorkTypeForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.selectedEmpId ? this.selectedEmpId : 0,
        FromDate: this.viewWorkTypeForm?.get('date_from').value ? this.viewWorkTypeForm?.get('date_from').value : '',
        ToDate: this.viewWorkTypeForm?.get('date_to').value ? this.viewWorkTypeForm?.get('date_to').value : '',
        Status: this.viewWorkTypeForm?.get('staus').value ? this.viewWorkTypeForm?.get('staus').value : ''
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.attendenceService.EmployeeGetAllWorkTypeFilter(reqBody).subscribe((res: any) => {
        if (res && res.length >= 1) {
          this.rows = res;
          this.isTableData = false;
          setTimeout(() => {
            const formattedData = res.map((item: any) => ({
              ...item,
              StartDate: this.formatDate(this.parseJsonDate(item.StartDate)),
              EndDate: this.formatDate(this.parseJsonDate(item.EndDate)),
            }));
            this.rows = [...formattedData];
          }, 100);
          this.isSpinner = false;
          this.isTableData = false;
        } else {
          this.errorMessage = "No records found";
          this.isSpinner = false;
          this.isTableData = true;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Load The Approver Data', "danger");
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
      });
    }
  }

  openModal(action: string, row: any) {
    this.patchValue = row;
    if (action === 'approve' && !row.IsApproved && !row.IsRejected) {
      this.isApproveMode = true;
    } else if (action === 'reject' && !row.IsApproved && !row.IsRejected) {
      this.isApproveMode = false;
    }
  }

  approve() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      WorkTypeId: this.patchValue.WorkTypeId,
      EmpId: this.patchValue.EmpId,
      EmpCode: this.patchValue.EmpCode,
      ApproverDescription: this.actionForm?.get('description').value ? this.actionForm?.get('description').value : ''
    }
    this.isSpinner = true;
    this.attendenceService.EmployeeApproveWorkType(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.isSpinner = false;
        this.closeModal.nativeElement.click();
        this.GetAllApproverWorkType();
        this.triggerToast(res['msg'], 'Status Updated Successfully', 'success');
      } else if (res['Message']) {
        this.isSpinner = false;
        this.triggerToast(res['msg'], res['msg'], 'warning');
      }
    }, error => {
      this.isSpinner = false;
      this.triggerToast('Internal Server Error', 'Failed To Change The Status', 'danger');
    })
  }

  reject() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      WorkTypeId: this.patchValue.WorkTypeId,
      EmpId: this.patchValue.EmpId,
      EmpCode: this.patchValue.EmpCode,
      ApproverDescription: this.actionForm?.get('description').value ? this.actionForm?.get('description').value : ''
    }
    this.isSpinner = true;
    this.attendenceService.EmployeeRejectWorkType(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.isSpinner = false;
        this.closeModal.nativeElement.click();
        this.GetAllApproverWorkType();
        this.triggerToast(res['msg'], 'Status Updated Successfully', 'success');
      } else if (res['Message']) {
        this.isSpinner = false;
        this.triggerToast(res['msg'], res['msg'], 'warning');
      }
    }, error => {
      this.isSpinner = false;
      this.triggerToast('Internal Server Error', 'Failed To Change The Status', 'danger');
    })
  }

   // This is second code for action
   ngOnDestroy(): void {
    document.removeEventListener('click', this.closeDropdownNew.bind(this));
  }

  toggleDropdownNew(event: MouseEvent, row:any): void {
    this.patchValue = row;
    this.dropdownVisible = !this.dropdownVisible;
    event.stopPropagation();
    this.dropdownTarget = event.target as HTMLElement;
  }

  approveAction(): void {
    this.dropdownVisible = false;  
    this.approve();
  }

  rejectAction(): void {
    this.dropdownVisible = false;  
    this.reject();
  }

  closeDropdownNew(event: MouseEvent): void {
    if (this.dropdownVisible && this.dropdownTarget !== event.target) {
      this.dropdownVisible = false;
    }
  }
// This is second code for action

  resetData() {
    this.viewWorkTypeForm.reset();
    this.isFormSubmitted = false;
    this.minDate = undefined;
    this.maxDate = undefined;
    this.searchText = '';
    this.selectedEmpId = '';
    // this.viewWorkTypeForm?.updateValueAndValidity();
    setTimeout(() => {
      if (this.inputValue?.nativeElement) {
        this.inputValue.nativeElement.value = null;
        const event = new KeyboardEvent('keyup', { bubbles: true });
        this.inputValue.nativeElement.dispatchEvent(event);
        this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));  // Ensure this method handles its own logic
      }
    }, 100);
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
      // this.getAllInviteList();
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

  onBlur(event: any) {
    setTimeout(() => {
      this.showDropdown = false; // Hide the dropdown on blur
    }, 200);
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
    if (this.viewWorkTypeForm.get('date_from')?.value) {
      this.minDate = this.viewWorkTypeForm.get('date_from')?.value;
    }
  }
  onToDate(): void {
    if (this.viewWorkTypeForm.get('date_to')?.value) {
      this.maxDate = this.viewWorkTypeForm.get('date_to')?.value;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.viewWorkTypeForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.viewWorkTypeForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.viewWorkTypeForm.hasError('dateRange');
  }
  // preventKeyboardInput(event: KeyboardEvent) {
  //   event.preventDefault();
  // }
  // preventPaste(event: ClipboardEvent) {
  //   event.preventDefault();
  // }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  onFocus(event: FocusEvent) {
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
  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      return new Date(parseInt(match[1], 10));
    }
    return null;
  }
  formatDate(date: Date | null): string {
    if (!date) return '';
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
    // return `${year}-${month}-${day}`;
  }
}
