import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { AttendenceModuleService } from 'src/app/HRMS/service/attendence.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from 'src/app/HRMS/service/accessPolicayApi.service';


@Component({
  selector: 'app-add-shifts-settings',
  standalone: true,
  imports: [ToastMessageComponent, SharedModule, NgxPaginationModule, NgbTimepickerModule, RouterModule],
  templateUrl: './add-shifts-settings.component.html',
  styleUrl: './add-shifts-settings.component.scss'
})
export class AddShiftsSettingsComponent {
@ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue: any = ElementRef;
  @ViewChild('startTimeInput', { static: true }) startTimeInput!: ElementRef;
  @ViewChild('endTimeInput', { static: true }) endTimeInput!: ElementRef;
  @ViewChild('closeChangeModal') closeChangeModal: any = ElementRef;

  addShiftForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  clockedHoursArray = ["7:00", "7:30", "8:00", "8:30", "9:00", "9:30", "10:00"];
  startTimeHour = [
    "1", "2", "3", "4", "5", "6", "7", "8",
    "9", "10", "11", "12", "13", "14", "15", "16",
    "17", "18", "19", "20", "21", "22", "23", "24"
  ]
  startMinutes = ["00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
    "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
    "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
    "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
    "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
    "51", "52", "53", "54", "55", "56", "57", "58", "59"]
  time: any;
  workedDaysArray = [4, 5, 6, 7];
  isEdited: boolean = false;
  isSpinner: boolean = false;
  employeeDetails;
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  viewdata: any;
  getEditdata: any;
  patchValue: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 5;
  pageSizes = [5, 10, 15, 20];
  previousValue: string = '';
  previousEndTimeValue: string = '';
  controlAccessPage:any;
  accessPolicy:any
  isRecordDeleted:boolean=false;
isCardOpen=false;

  constructor(private readonly fb: FormBuilder, private readonly attendenceService: AttendenceModuleService,
    private readonly activateRoute: ActivatedRoute, private readonly route:Router,
    private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Add Shifts'
    // );
    // this.controlAccessPage=viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Add Shifts'
      );
    });
  }

  ngOnInit(): void {
    this.addShiftForm = this.fb.group({
      shiftName: ['', [Validators.required]],
      startTime: ['', [Validators.required]],
      // startTimeAMPM: ['', [Validators.required]],
      // startMinutes:['', [Validators.required]],
      endTime: ['', [Validators.required]],
      // endTimeAMPM: ['', [Validators.required]],
      // endMinutes:['', [Validators.required]],
      clockHours: ['', [Validators.required]],
      workDays: ['', [Validators.required]],


      newTime: []
    });
    this.getAllShift();
  }


   toggleButton(){
    this.isCardOpen = !this.isCardOpen
  }

  onTimeInputChange() {
    const currentValue = this.startTimeInput.nativeElement.value;
    if (currentValue.length === 5 && this.previousValue) {
      const previousMinutes = this.previousValue.slice(3, 5);
      const currentMinutes = currentValue.slice(3, 5);
      if (currentMinutes !== previousMinutes) {
        this.startTimeInput.nativeElement.blur();
      }
    }
    this.previousValue = currentValue;
  }


  getAllShift() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.attendenceService.ShiftGetAllShift(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.rows = res;
        this.originalRows = res;
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

  getShift() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      "ShiftId": 1
    }
    this.isSpinner = true;
    this.attendenceService.ShiftGetShift(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.rows = res;
        this.originalRows = res;
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
  getStartTime(): string {
    const startTime = this.addShiftForm.get('startTime')?.value;
    // const ampm = this.addShiftForm.get('startTimeAMPM')?.value;
    //  const startMinutes = this.addShiftForm.get('startMinutes')?.value;
    return `${startTime}:000`;
  }
  getEndTime(): string {
    const endTime = this.addShiftForm.get('endTime')?.value;
    // const endMinutes = this.addShiftForm.get('endMinutes')?.value;
    return `${endTime}:000`;
  }
  submitFormData() {
    this.isFormSubmitted = true;
    if (this.addShiftForm?.valid) {
      this.isFormSubmitted = false;
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        ShiftName: this.addShiftForm?.get('shiftName').value ? this.addShiftForm?.get('shiftName').value : '',
        StartTime: this.getStartTime(),
        EndTime: this.getEndTime(),
        ClkHrs: `${this.addShiftForm?.get('clockHours').value}:000`,
        Days: this.addShiftForm?.get('workDays').value ? this.addShiftForm?.get('workDays').value : ''
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.attendenceService.ShiftAddShift(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.getAllShift();
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
    this.addShiftForm.reset();
    this.isFormSubmitted = false;
    this.isEdited = false;
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
    // this.rows = [...this.originalRows];
  }

  editPatchData(data: any, edited: boolean) {
    // console.log(data);
    this.patchValue = data;
    this.isCardOpen=true;
    // Extracting the hours and minutes
    const startHours = this.patchValue.StartTime.Hours.toString().padStart(2, '0');
    const startMinutes = this.patchValue.StartTime.Minutes.toString().padStart(2, '0');
    const endHours = this.patchValue.EndTime.Hours.toString().padStart(2, '0');
    const endMinutes = this.patchValue.EndTime.Minutes.toString().padStart(2, '0');
    // Create formatted time strings
    const formattedStartTime = `${startHours}:${startMinutes}`;
    const formattedEndTime = `${endHours}:${endMinutes}`;
    //  formattedClkHrs 
    const formattedClkHrs = this.patchValue.ClkHrs.split(":")[0] + ":" + this.patchValue.ClkHrs.split(":")[1];
    this.addShiftForm.patchValue({
      shiftName: this.patchValue.ShiftName,
      startTime: formattedStartTime,
      endTime: formattedEndTime,
      clockHours: formattedClkHrs,
      workDays: this.patchValue.Days
    })
    this.isEdited = edited;
  }

  updateShiftType() {
    this.isFormSubmitted = true;
    if (this.addShiftForm?.valid) {
      this.isFormSubmitted = false;
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        ShiftId: this.patchValue.ShiftId,
        ShiftName: this.addShiftForm?.get('shiftName').value ? this.addShiftForm?.get('shiftName').value : '',
        StartTime: this.getStartTime(),
        EndTime: this.getEndTime(),
        ClkHrs: `${this.addShiftForm?.get('clockHours').value}:000`,
        Days: this.addShiftForm?.get('workDays').value ? this.addShiftForm?.get('workDays').value : ''
      }
      // console.log(reqBody);

      this.isSpinner = true
      this.attendenceService.ShiftUpdateShift(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
          this.isSpinner = false;
          this.getAllShift();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
        this.isSpinner = false;
      })
    }
  }

  onView(data: any) {
    // console.log(data);
    this.getEditdata = data;
  }
  deleteShiftTypeData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      ShiftId: this.getEditdata.ShiftId,
    }
    // console.log(reqBody);
    this.attendenceService.ShiftDeleteShift(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], "Record Deleted Successfully", "success");
        this.isSpinner = false;

        this.isRecordDeleted = true;
         this.getAllShift();
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
  onEndTimeInputChange() {
    const currentValue = this.endTimeInput.nativeElement.value;
    if (currentValue.length === 5 && this.previousEndTimeValue) {
      const previousMinutes = this.previousEndTimeValue.slice(3, 5);
      const currentMinutes = currentValue.slice(3, 5);

      if (currentMinutes !== previousMinutes) {
        this.endTimeInput.nativeElement.blur();
      }
    }
    this.previousEndTimeValue = currentValue;
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
