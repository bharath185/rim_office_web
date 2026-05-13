import { CommonModule } from '@angular/common';
import { Component, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { AttendenceModuleService } from 'src/app/HRMS/service/attendence.service';
import { EmployeeModuleService } from 'src/app/HRMS/service/employee.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from 'src/app/HRMS/service/accessPolicayApi.service';
import { EntityStateService } from 'src/app/HRMS/service/entity-state.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-employee-shift-mapping',
  standalone: true,
  imports: [CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule,],
  templateUrl: './employee-shift-mapping.component.html',
  styleUrl: './employee-shift-mapping.component.scss'
})
export class EmployeeShiftMappingComponent {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue: any = ElementRef;

  employeeDetails;
  employeeShiftMappingForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isSpinner: boolean = false;
  getDDCompany: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  isTableShow: boolean = false;
  // getShifts: any = [];
  getShifts: { ShiftId: number; ShiftName: string; }[] = [];
  isTableData: boolean = false;
  errorMessage: any;
  errorMessageShiftEmployees: any;
  errorMessageNonShiftEmployees: any;
  page = 1;
  pageSize = 5;
  pageSizes = [5, 10, 15, 20];
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;

  shiftEmployeeSearch: string = '';
  nonShiftEmployeeSearch: string = '';
  filteredShiftEmployees: any[] = [];
  filteredNonShiftEmployees: any[] = [];
  sortDirectionShift: boolean = true;
  sortDirectionNonShift: boolean = true;
  getEmployeeShiftsDetails: any;
  selectedShifts: any;
  selectedNonShifts: any;
  selectedShiftName: any;
  onViewShiftData: any;
  getEntityName: any;
  accessPolicy: any
  controlAccessPage: any

  constructor(private readonly fb: FormBuilder, 
    private readonly attendenceService: AttendenceModuleService,
    private readonly hrmsService: EmployeeModuleService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private entityStateService: EntityStateService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    console.log('Employee Details', this.employeeDetails);

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Employee Shifts Mapping'
    // );
    // this.controlAccessPage=viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Employee Shifts Mapping'
      );
    });
  }
  ngOnInit(): void {
    this.employeeShiftMappingForm = this.fb.group({
      // Company: ['', [Validators.required]],
      // LegalEntity: ['', [Validators.required]],
      BusinessUnit: [''],
      Location: [''],
      shifts: ['', [Validators.required]]
    });
    // this.employee_DD_Employee();
    this.getBusinessUnit();
    // this.employeeShiftMappingForm?.get('Company').valueChanges.subscribe((val: any) => {
    //   this.getBusinessUnitlist = [];
    //   this.getLocations = [];
    // });

    this.employeeShiftMappingForm?.get('BusinessUnit').valueChanges.subscribe((val: any) => {
      this.employeeShiftMappingForm?.get('Location').reset();
      this.employeeShiftMappingForm?.get('shifts').reset();
      this.getLocations = [];
      this.getShifts = [];
    });

    this.employeeShiftMappingForm?.get('Location').valueChanges.subscribe((val: any) => {
      this.employeeShiftMappingForm?.get('shifts').reset();
      this.getShifts = [];
    });

    // this.employeeShiftMappingForm?.get('LegalEntity').valueChanges.subscribe((val: any) => {
    //   this.employeeShiftMappingForm?.get('BusinessUnit').reset();
    //   this.employeeShiftMappingForm?.get('Location').reset();
    //   this.employeeShiftMappingForm?.get('shifts').reset();
    //   this.getBusinessUnitlist = [];
    //   this.getLocations = [];
    //   this.getShifts = [];
    // });

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
  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }
  isEnableBusiness(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    // this.getDeptDataID = selectElement.value;
    this.getEntityName = selectElement.options[selectElement.selectedIndex].text;
    console.log(this.getEntityName)
    if (this.getEntityName === 'RIM India Pvt Ltd') {
      this.employeeShiftMappingForm?.get('BusinessUnit').disable();
      this.employeeShiftMappingForm?.get('Location').disable();
    } else {
      this.employeeShiftMappingForm?.get('BusinessUnit').enable();
      this.employeeShiftMappingForm?.get('Location').enable();
      this.employeeShiftMappingForm.get('BusinessUnit')?.updateValueAndValidity();
      this.employeeShiftMappingForm.get('Location')?.updateValueAndValidity();
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
  //     CompId: Number(this.employeeShiftMappingForm?.get('Company').value)
  //   }
  //   this.isSpinner = true;
  //   this.hrmsService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
  //     setTimeout(() => {
  //       this.employeeShiftMappingForm?.get('LegalEntity').reset();
  //       this.employeeShiftMappingForm?.get('BusinessUnit').reset();
  //       this.employeeShiftMappingForm?.get('Location').reset();
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
        this.employeeShiftMappingForm?.get('BusinessUnit').reset();
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
      CompId:1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.employeeShiftMappingForm?.get('BusinessUnit').value),
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDLocation(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.employeeShiftMappingForm?.get('Location').reset();
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

  callDDShifts() {
    this.getDDShifts();
  }

  getDDShifts() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: Number(this.employeeShiftMappingForm?.get('BusinessUnit').value) ? Number(this.employeeShiftMappingForm?.get('BusinessUnit').value) : 0,
      LocationId: Number(this.employeeShiftMappingForm?.get('Location').value) ? Number(this.employeeShiftMappingForm?.get('Location').value) : 0
    }
    this.isSpinner = true;
    this.attendenceService.ShiftDDShift(reqBody).subscribe((res: any) => {
      console.log(res);
      if (res.length >= 1) {
        this.getShifts = res
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


  callEmployeeShifts() {
    if (this.employeeShiftMappingForm.valid) {
      const selectedShift = this.employeeShiftMappingForm.get('shifts')?.value;
      if (selectedShift) {
        this.selectedShiftName = selectedShift.ShiftName;
      } else {
        this.selectedShiftName = '';
      }
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        CompId: 1,
        LEId: Number(this.entityStateService.getEntityId()),
        BUId: Number(this.employeeShiftMappingForm?.get('BusinessUnit').value),
        LocationId: Number(this.employeeShiftMappingForm?.get('Location').value),
        ShiftId: selectedShift.ShiftId,
      };
      this.isSpinner = true;
      this.attendenceService.ShiftGetAllShiftEmployee(reqBody).subscribe(
        (res: any) => {
          console.log(res);
          if (res && (res.ShiftEmployee.length > 0 || res.NonShiftEmployee.length > 0)) {
            this.getEmployeeShiftsDetails = res;
            this.filteredShiftEmployees = res.ShiftEmployee;
            this.filteredNonShiftEmployees = res.NonShiftEmployee;
            this.isSpinner = false;
            this.isTableData = false;
          } else {
            this.errorMessage = "No records found";
            this.filteredShiftEmployees = [];
            this.filteredNonShiftEmployees = [];
            this.isSpinner = false;
            this.isTableData = true;
          }
        },
        (error) => {
          this.errorMessage = "Internal Server Error";
          this.isSpinner = false;
          this.isTableData = true;
        }
      );
    }
  }

  filterShiftEmployees() {
    this.filteredShiftEmployees = this.getEmployeeShiftsDetails.ShiftEmployee.filter((employee: any) =>
      employee.FirstName.toLowerCase().includes(this.shiftEmployeeSearch.toLowerCase()) ||
      employee.EmpCode.toLowerCase().includes(this.shiftEmployeeSearch.toLowerCase())
    );
    // Check if the result is empty and set error message
    if (this.filteredShiftEmployees.length === 0) {
      this.errorMessageShiftEmployees = "No records found";
    } else {
      this.errorMessageShiftEmployees = ""; // Clear the error message if records are found
    }
  }
  filterNonShiftEmployees() {
    this.filteredNonShiftEmployees = this.getEmployeeShiftsDetails.NonShiftEmployee.filter((employee: any) =>
      employee.FirstName.toLowerCase().includes(this.nonShiftEmployeeSearch.toLowerCase()) ||
      employee.EmpCode.toLowerCase().includes(this.nonShiftEmployeeSearch.toLowerCase())
    );
    // Check if the result is empty and set error message
    if (this.filteredNonShiftEmployees.length === 0) {
      this.errorMessageNonShiftEmployees = "No records found";
    } else {
      this.errorMessageNonShiftEmployees = ""; // Clear the error message if records are found
    }
  }
  // Sorting logic (toggle between ascending/descending)
  sortShift(property: string) {
    const direction = this.sortDirectionShift ? 1 : -1;
    this.filteredShiftEmployees.sort((a, b) =>
      (a[property] > b[property] ? 1 : -1) * direction
    );
    this.sortDirectionShift = !this.sortDirectionShift;
  }
  sortNonShift(property: string) {
    const direction = this.sortDirectionNonShift ? 1 : -1;
    this.filteredNonShiftEmployees.sort((a, b) =>
      (a[property] > b[property] ? 1 : -1) * direction
    );
    this.sortDirectionNonShift = !this.sortDirectionNonShift;
  }

  // Select All functionality
  selectAllShiftEmployees(event: any) {
    const selectAllShift = (event.target as HTMLInputElement).checked;
    this.filteredShiftEmployees.forEach(employee => employee.selected = selectAllShift);
    const selectedEmployees = this.filteredShiftEmployees.filter(employee => employee.selected);
    // console.log(selectAllShift ? selectedEmployees : []);
    this.selectedShifts = selectedEmployees.filter(shift => shift.selected).map(shift => ({
      EmpId: shift.EmpId,
      EmpCode: shift.EmpCode
    }));
  }
  selectAllNonShiftEmployees(event: any) {
    const selectAllNonShift = (event.target as HTMLInputElement).checked;
    this.filteredNonShiftEmployees.forEach(employee => employee.selected = selectAllNonShift);
    const selectedEmployees = this.filteredNonShiftEmployees.filter(employee => employee.selected);
    // console.log(selectAllNonShift ? selectedEmployees : []);
    this.selectedNonShifts = selectedEmployees.filter(shift => shift.selected).map(shift => ({
      EmpId: shift.EmpId,
      EmpCode: shift.EmpCode
    }));
  }
  onRowShiftSelectChange() {
    const selectedShiftEmployees = this.filteredShiftEmployees.filter(employee => employee.selected);
    // console.log("Selected selectedShiftEmployees:", selectedShiftEmployees);
    this.selectedShifts = selectedShiftEmployees.filter(shift => shift.selected).map(shift => ({
      EmpId: shift.EmpId,
      EmpCode: shift.EmpCode
    }));

  }
  onRowNonShiftSelectChange() {
    const selectedNonShiftEmployees = this.filteredNonShiftEmployees.filter(employee => employee.selected);
    // console.log("Selected Non-Shift Employees:", selectedNonShiftEmployees);
    this.selectedNonShifts = selectedNonShiftEmployees.filter(shift => shift.selected).map(shift => ({
      EmpId: shift.EmpId,
      EmpCode: shift.EmpCode
    }));
  }

  addShifts() {
    if (this.employeeShiftMappingForm.valid) {
      const selectedShift = this.employeeShiftMappingForm.get('shifts')?.value;
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        CompId: 1,
        LEId: Number(this.entityStateService.getEntityId()),
        BUId: Number(this.employeeShiftMappingForm?.get('BusinessUnit').value),
        LocationId: Number(this.employeeShiftMappingForm?.get('Location').value),
        ShiftId: selectedShift.ShiftId,
        ShiftName: selectedShift.ShiftName,
        EmpList: this.selectedNonShifts
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.attendenceService.ShiftAddShiftEmployee(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.callEmployeeShifts();
          // this.resetData();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Add The Data', "danger");
        this.isSpinner = false;
      })
    } else {
      this.isFormSubmitted = true;
    }

  }

  removeShifts() {
    if (this.employeeShiftMappingForm.valid) {
      const selectedShift = this.employeeShiftMappingForm.get('shifts')?.value;
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        CompId: 1,
        LEId: Number(this.entityStateService.getEntityId()),
        BUId: Number(this.employeeShiftMappingForm?.get('BusinessUnit').value),
        LocationId: Number(this.employeeShiftMappingForm?.get('Location').value),
        ShiftId: selectedShift.ShiftId,
        ShiftName: selectedShift.ShiftName,
        EmpList: this.selectedShifts
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.attendenceService.ShiftRemoveShiftEmployee(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], "Data Removed Successfully", "success");
          this.isSpinner = false;
          this.callEmployeeShifts();
          // this.resetData();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Removed The Data', "danger");
        this.isSpinner = false;
      })
    } else {
      this.isFormSubmitted = true;
    }
  }

  resetData() {
    this.employeeShiftMappingForm.reset();
    this.isFormSubmitted = false;
    this.isTableData = true;
  }

  onView(data: any) {
    this.onViewShiftData = data;
    console.log(this.onViewShiftData);
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
}
