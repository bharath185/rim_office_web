import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { AttendenceModuleService } from '../../service/attendence.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { Router, RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-add-work-type',
  standalone: true,
  imports: [CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule,
    RouterModule
  ],
  templateUrl: './add-work-type.component.html',
  styleUrl: './add-work-type.component.scss'
})
export class AddWorkTypeComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue: any = ElementRef;
  @ViewChild('closeChangeModal') closeChangeModal: any = ElementRef;

  addWorkTypeForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  minDate: string | undefined;
  maxDate: string | undefined;
  today = new Date().toISOString().split('T')[0];
  isSpinner: boolean = false;
  employeeDetails;
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  isEdited: boolean = false;
  viewdata: any;
  getEditdata: any;
  patchValue: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100,500];
  isRecordDeleted: boolean = false;
  accessPolicy:any;
  controlAccessPage:any;
  isCardOpen = false;
  tabs: any[] = [];

  allTabs = [
    { id: 'view_worktype', title: 'View Work Type', type: 'item', url: '/view_worktype', icon: 'feather icon-clipboard' },
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }

  constructor(private readonly fb: FormBuilder, private readonly attendenceService: AttendenceModuleService,
    private router: Router,private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Add Work Type'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);
  }

  ngOnInit(): void {
    this.addWorkTypeFormVal();

     this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; // ✅ Guard clause
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Add Work Type'
      );
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((p: any) => p.PageName === tab.title && p.ViewAccess)
      );
    });
  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  addWorkTypeFormVal() {
    this.addWorkTypeForm = this.fb.group({
      workType: ['', [Validators.required]],
      date_from: ['', [Validators.required]],
      date_to: ['', [Validators.required]],
      Reason: [''],
    }, { validators: this.dateRangeValidator });
    this.getAllWorkTypeList();
    // this.getWorkType();
  }
  getWorkType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      WorkTypeId: ''
    }
    this.isSpinner = true;
    this.attendenceService.EmployeeGetWorkType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.isSpinner = false;
      } else {
        this.errorMessage = "No records found";
        this.isSpinner = false;
      }
    }, error => {
      this.errorMessage = "Internal Server Error";
      this.isSpinner = false;
    })
  }
  getAllWorkTypeList() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.attendenceService.EmployeeGetAllWorkType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        setTimeout(() => {
          const formattedData = res.map((item: any) => ({
            ...item,
            StartDate: this.formatDate(this.parseJsonDate(item.StartDate)),
            EndDate: this.formatDate(this.parseJsonDate(item.EndDate)),
          }));
          this.rows = [...formattedData];
          this.originalRows = [...formattedData];
        }, 100);
        this.isSpinner = false;
        this.isTableData = false;
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

  submitFormData() {
    this.isFormSubmitted = true;
    const fromDateValue = this.addWorkTypeForm?.get('date_from')?.value;
    const toDateValue = this.addWorkTypeForm?.get('date_to')?.value;
    const parseDate = (date: any): Date | null => {
      if (date === null || date === undefined) return null;
      if (typeof date === 'string') return new Date(date);
      if (date instanceof Date) return date;
      return null;
    };
    const formatDate = (date: Date | null): string => {
      if (!date) return '';
      const day = date.getDate().toString().padStart(2, '0');
      const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
      const year = date.getFullYear();
      return `${day}-${month}-${year}`;
    };
    const fromDate = parseDate(fromDateValue);
    const toDate = parseDate(toDateValue);
    const fromOnly = formatDate(fromDate);
    const toOnly = formatDate(toDate);
    if (this.addWorkTypeForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.employeeDetails[0].EmpCode,
        WorkType: this.addWorkTypeForm?.get('workType').value,
        StartDate: fromOnly,
        EndDate: toOnly,
        Reason: this.addWorkTypeForm?.get('Reason').value
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.attendenceService.employeeAddWorkType(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.getAllWorkTypeList();
          this.resetData();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Add The Data', "danger");
        this.isSpinner = false;
      })
    }
  }

  resetData() {
    this.addWorkTypeForm.reset();
    this.isFormSubmitted = false;
    this.minDate = undefined;
    this.maxDate = undefined;
    this.isEdited = false;
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
    // this.rows = [...this.originalRows];
  }

  onView(data: any) {
    // console.log(data);
    this.getEditdata = data;
  }

  editPatchData(data: any, edited: boolean) {
    // console.log(data);
    this.patchValue = data;
    this.isEdited = edited;
    this.isCardOpen = true;
    const parseDate = (date: any): Date | null => {
      if (date === null || date === undefined) return null;
      if (typeof date === 'string') return new Date(date);
      if (date instanceof Date) return date;
      return null;
    };
    const formatDate = (date: Date | null): string => {
      if (!date) return '';
      const day = date.getDate().toString().padStart(2, '0');
      const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
      const year = date.getFullYear();
      // return `${day}-${month}-${year}`;
      return `${year}-${month}-${day}`;
    };
    const StartDate = this.patchValue.StartDate ? parseDate(this.patchValue.StartDate) : null;
    const formattedStartDate = formatDate(StartDate);
    const EndDate = this.patchValue.EndDate ? parseDate(this.patchValue.EndDate) : null;
    const formattedEndDate = formatDate(EndDate);
    this.addWorkTypeForm.patchValue({
      workType: this.patchValue.WorkType ? this.patchValue.WorkType : this.addWorkTypeForm?.get('workType').value,
      date_from: formattedStartDate ? formattedStartDate : this.addWorkTypeForm?.get('date_from').value,
      date_to: formattedEndDate ? formattedEndDate : this.addWorkTypeForm?.get('date_to').value,
      Reason: this.patchValue.Reason ? this.patchValue.Reason : this.addWorkTypeForm?.get('Reason').value
    })
  }

  updateAddWorkType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      EmpCode: this.employeeDetails[0].EmpCode,
      WorkTypeId: this.patchValue.WorkTypeId,
      WorkType: this.addWorkTypeForm?.get('workType').value,
      StartDate: this.addWorkTypeForm?.get('date_from').value,
      EndDate: this.addWorkTypeForm?.get('date_to').value,
      Reason: this.addWorkTypeForm?.get('Reason').value
    }
    this.isSpinner = true
    this.attendenceService.EmployeeUpdateWorkType(reqBody).subscribe((res: any) => {
      if (res['msg'] === "Updated") {
        this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
        this.isEdited = false;
        this.addWorkTypeForm.reset();
        this.isFormSubmitted = false;
        this.getAllWorkTypeList();
        this.isSpinner = false;
      } else if (res['Message']) {
        this.triggerToast(res['Message'], res['Message'], 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      this.isSpinner = false;
    })
  }
  deleteWorkTypeData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      WorkTypeId: this.getEditdata.WorkTypeId
    }
    console.log(reqBody);
    this.attendenceService.EmployeeDeleteWorkType(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        console.log('1');
        this.triggerToast(res['msg'], "Record Deleted Successfully", "success");
        this.isSpinner = false;
        this.isRecordDeleted = true;
        this.getAllWorkTypeList();
        setTimeout(() => {
          this.closeChangeModal.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
      } else if (res['Message']) {
        this.triggerToast(res['Message'], res['Message'], "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Failed To Remove The Data', "danger");
      this.isSpinner = false;
    })
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

  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }

  onFromDate(): void {
    if (this.addWorkTypeForm.get('date_from')?.value) {
      this.minDate = this.addWorkTypeForm.get('date_from')?.value;
    }
  }
  onToDate(): void {
    if (this.addWorkTypeForm.get('date_to')?.value) {
      this.maxDate = this.addWorkTypeForm.get('date_to')?.value;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.addWorkTypeForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.addWorkTypeForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.addWorkTypeForm.hasError('dateRange');
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
    // return `${day}-${month}-${year}`;
    return `${year}-${month}-${day}`;
  }
}
