import { Component, ElementRef, HostListener, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { AttendenceModuleService } from '../../service/attendence.service';
import { payRollService } from '../../service/payroll.service';
import { NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { TimepickerComponent } from '../../timepicker/timepicker.component';
import { DatePipe } from '@angular/common';
import { log } from 'console';
import * as XLSX from 'xlsx';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-upload-file',
  standalone: true,
  imports: [FormsModule, CommonModule, ToastMessageComponent, SharedModule, NgxPaginationModule,
    RouterModule, NgbTimepickerModule, TimepickerComponent
  ],
  providers: [DatePipe],
  templateUrl: './upload-file.component.html',
  styleUrl: './upload-file.component.scss'
})
export class UploadFileComponent implements OnInit, OnDestroy {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('startTimeInput', { static: true }) startTimeInput!: ElementRef;
  isUploadSelected = false;
  isManualSelected = false;
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  rows: any = [];
  originalrows: any = [];
  searchValue: string = '';
  isTableData: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100];

  uploadAttenForm: any = FormGroup;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  getDDCompany: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  errorMessage: any;
  today = new Date().toISOString().split('T')[0];
  minDate: string | undefined;
  isTimePickerVisible: boolean = false;
  time: Date = new Date();

  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;

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
  previousValue: string = '';


  constructor(
    private readonly hrmsService: EmployeeModuleService,
    private readonly fb: FormBuilder,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly hrmsEmpAttendance: AttendenceModuleService,
    private readonly payrollLocationDD: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private datePipe: DatePipe,
    private entityStateService: EntityStateService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Self Attendance'
      );
    });
  }

  ngOnInit(): void {
    this.uploadAttenForm = this.fb.group({
      // company: [''],
      // LegalEntity: [''],
      BusinessUnit: [''],
      Location: [''],
      emloyee: ['', [Validators.required]],
      emloyeeCode: ['',],
      date_from: ['', [Validators.required]],
      time: [''],
      startTime: ['', [Validators.required]],
    });
    setTimeout(() => {
      // this.dropdown_Comapny();
      this.getAllManualAttendance();
      setTimeout(() => {
        this.callLocation();
        this.getEmployeeSelectEmployee();
      }, 200);
    }, 100);
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;

        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.callLocation();
          this.resetData();
        }
        this.currentEntityId = newEntityId;
      });
  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }
  onFromDate(): void {
    if (this.uploadAttenForm.get('date_from')?.value) {
      this.minDate = this.uploadAttenForm.get('date_from')?.value;
    }
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

  openTimePicker() {
    this.isTimePickerVisible = true;  // Show time picker when input is focused
  }

  closeTimePicker() {
    this.isTimePickerVisible = false;  // Hide time picker
  }

  onTimeChange(time: Date) {
    console.log('Selected Time:', time);  // Log the Date object

    // Format the Date object to show just the time (hh:mm AM/PM)
    const formattedTime = this.datePipe.transform(time, 'hh:mm a'); // Format the time as 'hh:mm AM/PM'
    console.log('Formatted Time:', formattedTime);  // Log the formatted time

    // Set the formatted time into the form control
    this.uploadAttenForm.get('time')?.setValue(formattedTime);

    // Optionally, update the component's internal time state
    this.time = time;
  }

  // dropdown_Comapny() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId
  //   };
  //   this.isSpinner = true;
  //   this.hrmsService.employeeDDCompany(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res.length >= 1) {
  //         this.getDDCompany = res;
  //       } else {
  //         this.triggerToast(res['Message'], 'Sorry No Data Found', 'warning');
  //       }
  //       this.isSpinner = false;
  //     },
  //     error: (error: any) => {
  //       this.triggerToast('Internal Server Error', 'Error loading Company Name', 'danger');
  //       this.isSpinner = false;
  //     }
  //   });
  // }

  // calllegalEntity(event: any) {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     AuthorisedEntity: this.entityStateService.getEntityId(),
  //     CompId: Number(this.uploadAttenForm?.get('company').value)
  //   }
  //   this.isSpinner = true;
  //   this.getLegalEntity = []
  //   this.hrmsService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
  //     setTimeout(() => {
  //       this.uploadAttenForm?.get('LegalEntity').reset();
  //       this.uploadAttenForm?.get('BusinessUnit').reset();
  //       this.uploadAttenForm?.get('Location').reset();
  //     }, 100);
  //     if (res.length >= 1) {
  //       this.getLegalEntity = res;
  //       this.isSpinner = false;
  //     } else {
  //       this.triggerToast(res['Message'], "No Data Found For Legal Entity", "warning");
  //       this.isSpinner = false;
  //       this.getLegalEntity = []
  //     }
  //   },
  //     error => {
  //       this.errorMessage = 'Error loading data. Please try again.';
  //       this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
  //       this.isSpinner = false;
  //     })
  // }

  // getBusinessUnit() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     AuthorisedEntity: this.entityStateService.getEntityId(),
  //     CompId: Number(this.uploadAttenForm?.get('company').value),
  //     LEId: Number(this.uploadAttenForm?.get('LegalEntity').value),
  //   }
  //   this.isSpinner = true;
  //   this.getBusinessUnitlist = []
  //   setTimeout(() => {
  //     this.hrmsService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
  //       if (res.length >= 1) {
  //         this.uploadAttenForm?.get('BusinessUnit').reset();
  //         this.getBusinessUnitlist = res;
  //         this.isSpinner = false;
  //       } else {
  //         this.isSpinner = false;
  //         this.getBusinessUnitlist = [];
  //         this.getLocations = []
  //       }
  //     },
  //       error => {
  //         this.errorMessage = 'Error loading data. Please try again.';
  //         this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
  //         this.isSpinner = false;
  //       })
  //   }, 100);

  // }

  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getLocations = [];
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.uploadAttenForm?.get('Location').reset();
          this.getLocations = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = []
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner = false;
        })
    }, 100);
  }

  //this is Employee list
  getEmployeeSelectEmployee(compId?: number, leId?: number, locationId?: number) {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      // CompId: (compId ?? this.uploadAttenForm.get('company')?.value) || 0,
      CompId: 1,
      // LEId: (leId ?? this.uploadAttenForm.get('LegalEntity')?.value) || 0,
      LEId:Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocationId: (locationId ?? this.uploadAttenForm.get('Location')?.value) || 0
    };

    this.isSpinner = true;
    this.hrmsEmpAttendance.employeeDDEmpList(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.errorMessageEmpName = '';
        } else {
          this.employees = [];
          this.errorMessageEmpName = 'No Data Found.';
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.employees = [];
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
    this.uploadAttenForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.uploadAttenForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.uploadAttenForm.get('emloyee')?.setErrors(null);
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


  selectedFiles: File[] = [];
  fileUrls: string[] = [];

  enableManualEntry() {
    this.isManualSelected = true;
    this.isUploadSelected = false;
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (!file) {
      return;
    }

    // ✅ Only XLSX MIME type
    const allowedType =
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

    if (file.type !== allowedType) {
      alert('Only Excel (.xlsx) files are allowed');
      event.target.value = '';
      return;
    }

    // Clear previous file
    if (this.fileUrls.length > 0) {
      URL.revokeObjectURL(this.fileUrls[0]);
    }

    this.selectedFiles = [file];
    this.fileUrls = [URL.createObjectURL(file)];

    this.isUploadSelected = true;
    this.isManualSelected = false;
    this.uploadAttenForm.disable();

    event.target.value = '';
  }

  openFile(index: number) {
    const url = this.fileUrls[index];
    window.open(url, '_blank'); // 🔥 opens file
  }

  removeFile(index: number) {
    URL.revokeObjectURL(this.fileUrls[index]);

    this.selectedFiles = [];
    this.fileUrls = [];

    this.isUploadSelected = false;
    this.uploadAttenForm.enable();
  }


  // 🔽 Download Sample File
  downloadSampleFile() {
    const data = [
      ['EmpCode', 'Date', 'Time', 'Status'], // header
      []                                    // empty row
    ];

    const worksheet = XLSX.utils.aoa_to_sheet(data);
    const workbook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(workbook, worksheet, 'Attendance');

    XLSX.writeFile(workbook, 'sample_upload_template.xlsx');
  }


  uploadFiles() {
    if (!this.selectedFiles || this.selectedFiles.length === 0) {
      alert('Please select a file to upload');
      return;
    }
    const file: File = this.selectedFiles[0];
    const allowedType =
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

    if (file.type !== allowedType) {
      alert('Only Excel (.xlsx) files are allowed');
      return;
    }
    const reader = new FileReader();
    reader.onload = (e: any) => {
      const data = new Uint8Array(e.target.result);
      const workbook = XLSX.read(data, { type: 'array' });
      const sheetName = workbook.SheetNames[0];
      const worksheet = workbook.Sheets[sheetName];
      const rows = XLSX.utils.sheet_to_json(worksheet, { defval: '' });
      // ❌ No data rows
      if (!rows || rows.length === 0) {
        alert('Please fill all data before uploading');
        return;
      }
      // ❌ All rows empty
      const hasValidRow = rows.some((row: any) =>
        Object.values(row).some(value => value !== '')
      );
      if (!hasValidRow) {
        alert('Please fill all data before uploading');
        return;
      }
      // ✅ Valid → upload
      const loginId = this.employeeDetails[0].LoginId;
      const empId = this.employeeDetails[0].EmpId;

      this.uploadToApi(loginId, empId, file.name, file);
    };

    reader.readAsArrayBuffer(file);
  }


  uploadToApi(loginId: number, empId: number, fileName: string, file: File) {
    this.isSpinner = true;
    this.hrmsEmpAttendance.employeeUploadAttendance(loginId, empId, fileName, file)
      .subscribe({
        next: (res: any) => {
          this.triggerToast('', 'File uploaded successfully', '')
          this.removeFile(0);
          this.isSpinner = false;
          this.getAllManualAttendance();
        },
        error: (err: any) => {
          this.triggerToast('', 'File upload failed', 'danger');
          this.isSpinner = false;
        }
      });
  }


  resetData() {
    if (this.fileUrls.length > 0) {
      URL.revokeObjectURL(this.fileUrls[0]);
    }

    this.selectedFiles = [];
    this.fileUrls = [];

    // Reset state flags
    this.isManualSelected = false;
    this.isUploadSelected = false;
    this.isFormSubmitted = false;

    // Reset & enable form
    this.uploadAttenForm.reset();
    this.uploadAttenForm.enable();

    // Reset file input value
    const fileInput = document.getElementById('fileUpload') as HTMLInputElement;
    if (fileInput) fileInput.value = '';

    // 🔄 Reload all employees after reset
    this.getEmployeeSelectEmployee();
  }

  getAllManualAttendance() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.hrmsEmpAttendance.employeeGetAllManualAttendance(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.rows = res;
          this.originalrows = res;
          this.errorMessage = '';
          this.isTableData = false;
        }
        else if (res['Message']) {
          this.errorMessage = res['Message'];
          this.isTableData = true;
          this.triggerToast('', res['Message'], 'warning');
        }
        else {
          this.errorMessage = 'No Data Found';
          this.isTableData = true;
        }
        this.isSpinner = false;
      },
      error: () => {
        this.errorMessage = 'Internal Server Error';
        this.isSpinner = false;
        this.isTableData = true;
      }
    })
  }


  submitFormData() {
    if (this.uploadAttenForm.valid) {
      this.isFormSubmitted = false;
      const reqBody = {
        // company: this.uploadAttenForm?.get('company').value,
        // LegalEntity: this.uploadAttenForm?.get('LegalEntity').value,
        // BusinessUnit: this.uploadAttenForm?.get('BusinessUnit').value,
        // Location: this.uploadAttenForm?.get('Location').value,
        // emloyee: this.uploadAttenForm?.get('emloyee').value,
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.employeeDetails[0].EmpId,
        EmpCode: this.uploadAttenForm?.get('emloyeeCode').value,
        Date: this.uploadAttenForm?.get('date_from').value,
        Time: this.uploadAttenForm?.get('startTime').value,
        Status: "Active",
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmpAttendance.employeeUploadSingleAttendance(reqBody).subscribe({
        next: (res: any) => {
          this.triggerToast('', 'Data Submitted Successfully', '');
          this.getAllManualAttendance();
          this.isSpinner = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'Failed to submit the data', '');
          this.isSpinner = false;
        }
      })
    } else {
      this.isFormSubmitted = true;
    }

  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


}
