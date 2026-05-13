import { CommonModule } from '@angular/common';
import { Component, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from 'bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccessPolicyStoreService } from 'src/app/HRMS/service/accessPolicayApi.service';
import { AttendenceModuleService } from 'src/app/HRMS/service/attendence.service';
import { EmployeeModuleService } from 'src/app/HRMS/service/employee.service';
import { EntityStateService } from 'src/app/HRMS/service/entity-state.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-shifts-mapping',
  standalone: true,
  imports: [CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule, NgbModule, RouterModule],
  templateUrl: './shifts-mapping.component.html',
  styleUrl: './shifts-mapping.component.scss'
})
export class ShiftsMappingComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue: any = ElementRef;
  @ViewChild('closeModal') closeModal: any = ElementRef;

  employeeDetails;
  shiftGroupingForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isSpinner: boolean = false;
  getDDCompany: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  isTableShow: boolean = false;
  AllShiftsRows: any[] = [];
  rows: any[] = [];
  isTableData: boolean = false;
  isTableDataGetShift: boolean = false;
  errorMessage: any;
  errorMessageGetShift: any;
  selectAll: boolean = false;
  selectedShifts: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  originalRows: any;
  tooltipContent: string = '';
  getEntityName: any;
  accessPolicy: any
  controlAccessPage: any
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;

  constructor(private readonly fb: FormBuilder,
    private readonly attendenceService: AttendenceModuleService,
    private readonly hrmsService: EmployeeModuleService,
    private entityStateService: EntityStateService,
    private readonly router: Router,
    private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Shifts'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Shifts'
      );
    });

  }
  ngOnInit(): void {
    this.getBusinessUnit();
    this.shiftGroupingForm = this.fb.group({
      // Company: ['', [Validators.required]],
      // LegalEntity: ['', [Validators.required]],
      BusinessUnit: [''],
      Location: ['']
    });
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;
        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          console.log('Entity changed → resetting filter form');
          this.getBusinessUnit();
          this.resetData();
        }
        this.currentEntityId = newEntityId;
      });
    // this.employee_DD_Employee();
    this.getAllShiftGrouping();
    // this.shiftGroupingForm?.get('Company').valueChanges.subscribe((val: any) => {
    //   this.getBusinessUnitlist = [];
    //   this.getLocations = [];
    // });
    // this.shiftGroupingForm?.get('LegalEntity').valueChanges.subscribe((val: any) => {
    //   this.isTableShow = false
    // });
    this.shiftGroupingForm.get('BusinessUnit').valueChanges.subscribe((val: any) => {
      const locationControl = this.shiftGroupingForm.get('Location');
      if (val) {
        locationControl.setValidators([Validators.required]);
        this.shiftGroupingForm.get('Location').reset();
        this.getLocations = [];
      } else {
        locationControl.clearValidators();
      }
      locationControl.updateValueAndValidity();
    });

  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }
  // Encryption method using crypo-js
  // encryptData(data: string): string {
  //   const key = 'your-encryption-key';
  //   return CryptoJS.AES.encrypt(data, key).toString();
  // }

  // nagivateToAddSfits() {
  //   const encryptedPath = this.encryptData('/add_shifts');
  //   this.router.navigate(['/add_shifts', encryptedPath]);
  // }
  // Encryption method using crypo-js

  isEnableBusiness(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    // this.getDeptDataID = selectElement.value;
    this.getEntityName = selectElement.options[selectElement.selectedIndex].text;
    if (this.getEntityName === 'RIM India Pvt Ltd') {
      this.shiftGroupingForm?.get('BusinessUnit').disable();
      this.shiftGroupingForm?.get('Location').disable();
    } else {
      this.shiftGroupingForm?.get('BusinessUnit').enable();
      this.shiftGroupingForm?.get('Location').enable();
      this.shiftGroupingForm.get('BusinessUnit')?.updateValueAndValidity();
      this.shiftGroupingForm.get('Location')?.updateValueAndValidity();
    }
  }
  // employee_DD_Employee() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId
  //   }
  //   this.isSpinner = true;
  //   this.hrmsService.employeeDDCompany(reqBody).subscribe((res: any) => {
  //     if (res.length >= 1) {
  //       this.getDDCompany = res;
  //       this.isSpinner = false;
  //     } else {
  //       this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
  //       this.isSpinner = false;
  //     }
  //   },
  //     error => {
  //       // this.errorMessage = 'Error loading data. Please try again later.';
  //       this.triggerToast('Internal Server Error', 'Error loading Company Name', "danger");
  //       this.isSpinner = false;
  //     })
  // }
  // calllegalEntity(event: any) {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     AuthorisedEntity: this.entityStateService.getEntityId(),
  //     CompId: Number(this.shiftGroupingForm?.get('Company').value)
  //   }
  //   this.isSpinner = true;
  //   this.hrmsService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
  //     setTimeout(() => {
  //       this.shiftGroupingForm?.get('LegalEntity').reset();
  //       this.shiftGroupingForm?.get('BusinessUnit').reset();
  //       this.shiftGroupingForm?.get('Location').reset();
  //     }, 100);
  //     if (res) {
  //       this.getLegalEntity = res;
  //       this.isSpinner = false;

  //     } else {
  //       this.triggerToast(res['Message'], "No Data Found For Legal Entity", "warning");
  //       this.isSpinner = false;
  //       this.getLegalEntity = []
  //     }
  //   },
  //     error => {
  //       // this.errorMessage = 'Error loading data. Please try again later.';
  //       this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
  //       this.isSpinner = false;
  //     })
  // }
  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.getBusinessUnitlist = [];
    this.isSpinner = true;
    this.hrmsService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.shiftGroupingForm?.get('BusinessUnit').reset();
        this.getBusinessUnitlist = res;
        this.isSpinner = false;
      } else {
        // this.triggerToast(res['Message'], "No Data Found For Business Unit", "warning");
        this.isSpinner = false;
        this.getBusinessUnitlist = [];
        this.getLocations = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
        this.isSpinner = false;
      })
  }

  callLocation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.shiftGroupingForm?.get('BusinessUnit').value),
    }
    this.getLocations = []
    this.isSpinner = true;
    this.hrmsService.employeeDDLocation(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.shiftGroupingForm?.get('Location').reset();
        this.getLocations = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Location", "warning");
        this.isSpinner = false;
        this.getLocations = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
        this.isSpinner = false;
      })
  }

  getAllShiftsTableData() {
    this.isFormSubmitted = true;
    if (this.shiftGroupingForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
      }
      this.isSpinner = true;
      this.attendenceService.ShiftGetAllShift(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.isSpinner = false;
          this.AllShiftsRows = res;
          this.isTableShow = true;
        } else {
          this.isTableShow = true;
          this.errorMessageGetShift = "No records found";
          this.isSpinner = false;
          this.isTableDataGetShift = true;
        }
      }, error => {
        this.isTableShow = true;
        this.errorMessageGetShift = "Internal Server Error";
        this.isSpinner = false;
        this.isTableDataGetShift = true;
      })
    } else {
      this.triggerToast('Location is mandatory', "Please select loaction", "warning");
    }
  }

  getAllShiftGrouping() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.attendenceService.ShiftGetAllShiftGrouping(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.rows = res;
        // this.originalRows = res;
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

  toggleSelectAll(event: Event) {
    this.selectAll = (event.target as HTMLInputElement).checked;
    this.AllShiftsRows.forEach(row => row.selected = this.selectAll);
    this.getSelectedRows();
  }

  onRowSelectChange() {
    const allSelected = this.AllShiftsRows.every(row => row.selected);
    this.selectAll = allSelected;
    this.getSelectedRows();

  }
  getSelectedRows() {
    const selectedRows = this.AllShiftsRows.filter(row => row.selected);
    this.selectedShifts = selectedRows.filter(shift => shift.selected).map(shift => ({
      ShiftId: shift.ShiftId,
      ShiftName: shift.ShiftName
    }));
  }
  formatTime(time: string): string {
    if (time) {
      return time.slice(0, 4);
    }
    return '00:00';
  }
  setTooltipContent(shift: any): void {
    if (shift) {
      const formattedLogInTime = this.formatTime(shift.ClkHrs);
      this.tooltipContent = `
      Start Time: <strong>${shift.StartTime.Hours}:${shift.StartTime.Minutes}</strong><br>
      End Time: <strong>${shift.EndTime.Hours}:${shift.EndTime.Minutes}</strong><br>
      Days: <strong>${shift.Days}</strong><br>
      Clocked Hours: <strong>${formattedLogInTime}</strong>
    `;
    }

  }

  clearTooltipContent(): void {
    this.tooltipContent = '';
  }

  openModal(): void {
    if (this.shiftGroupingForm.valid) {
      this.getAllShiftsTableData();
      const modalElement = document.getElementById('modal-right');
      const modal = new Modal(modalElement);
      modal.show();
    } else {
      this.isFormSubmitted = true;
    }
  }


  submitFormData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.shiftGroupingForm?.get('BusinessUnit').value),
      LocationId: Number(this.shiftGroupingForm?.get('Location').value),
      lstOfShift: this.selectedShifts
    }
    // console.log(reqBody);
    this.isSpinner = true;
    this.attendenceService.ShiftAddShiftGrouping(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], "Data Added Successfully", "success");
        this.isSpinner = false;
        this.getAllShiftGrouping();
        this.closeModal.nativeElement?.click();
        this.resetData();
      } else if (res['Message']) {
        this.triggerToast(res['Message'], res['Message'], "warning");
        this.isSpinner = false;
      }
    }, (error: any) => {
      this.triggerToast('Internal Server Error', 'Failed To Add The Data', "danger");
      this.isSpinner = false;
    })
  }

  resetData() {
    this.shiftGroupingForm.reset();
    this.isFormSubmitted = false;
    this.isTableShow = false;
    this.shiftGroupingForm?.get('BusinessUnit').enable();
    this.shiftGroupingForm?.get('Location').enable();
    // setTimeout(() => {
    //   this.inputValue.nativeElement.value = null;
    //   let event = new KeyboardEvent('keyup', { 'bubbles': true });
    //   this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    // }, 100);
    // this.rows = [...this.originalRows];
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

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  // applyFilter(event: Event) {
  //   const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
  //   if (filterValue) {
  //     this.rows = this.rows.filter((row: any) =>
  //       Object.values(row).some(val =>
  //         String(val).toUpperCase().includes(filterValue)
  //       )
  //     );
  //   } else {
  //     this.isTableData = false;
  //     this.rows = [...this.originalRows];
  //   }

  //   if (this.rows.length === 0) {
  //     this.isTableData = true;
  //     this.errorMessage = 'No Records Found for Searched Data';
  //     this.rows = [...this.originalRows];
  //   } else {
  //     this.isTableData = false;
  //     this.errorMessage = null;
  //   }
  // }
}
