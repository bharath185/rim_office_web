import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { Router, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { environment } from 'src/assets/environment';
import { forkJoin } from 'rxjs';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-invite-page',
  standalone: true,
  imports: [ToastMessageComponent, CommonModule, SharedModule, FormsModule,
    ReactiveFormsModule, NgxPaginationModule, RouterModule],
  templateUrl: './invite-page.component.html',
  styleUrl: './invite-page.component.scss'
})
export class InvitePageComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue!: ElementRef;
  baseUrl: string = environment.baseUrl;
  @ViewChild('timeInput') timeInput: any = ElementRef;


  invitePageForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isEdited: boolean = false;
  employeeDetails: any;
  controlAccessPage: any;
  getFacalities: any[] = [];
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  selectedEmployee: any = null;
  today: any;
  hours: string[] = [];
  minutes: string[] = [];
  ampm: string[] = ['AM', 'PM'];
  employees: any[] = [];
  searchTerm: string = '';
  filteredSuggestions: any[] = [];
  approverEmployeeNames: any[] = [];
  filteredEmployeeNames: any[] = [];
  errorMessage: any;
  errorMessageFacility: any;
  errorMessageGetAllInvite: any;
  accessPolicy: any;
  invitePageAccess: any;
  imageSrc: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;
  ImagePath: any;
  isUploadSuccess: boolean = false;
  isSuggestionSelected: boolean = false;
  page = 1;
  pageSize = 15;
  pageSizes = [15, 50, 100, 500];
  isTableData: boolean = false;
  rows: any[] = [];
  originalRows: any;
  viewdata: any;
  showDropdown: boolean = false;
  errorMessageEmpName: any;
  selectedEmpId: any;
  patchPhotoUrl: any;
  isValidPhoto: any;
  previousTimeValue: string = '';
  checkOutTime: any
  checkInTime: any
  searchText: string = '';
  isDropdownOpen = false;
  filteredEmployees: any[] = [];
  isValidEmployee: boolean = true;
  isCardOpen = false;
  tabs: any[] = [];

  allTabs = [
    { id: 'view_visitor', title: 'View Visitor', type: 'item', url: '/view_visitor', icon: 'feather icon-eye' },
    { id: 'direct_checkin', title: 'Direct Checkin', type: 'item', url: '/direct_checkin', icon: 'feather icon-log-in' }
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }

  constructor(private readonly fb: FormBuilder, private readonly hrmsService: HrmsServiceService, private readonly route: Router,
    private readonly cdr: ChangeDetectorRef, private router: Router, private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // console.log('this.accessPolicy=>', this.accessPolicy);

    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Visitor'
    // );

    // this.controlAccessPage = viewEmployeeAccess;
    // console.log('this.controlAccessPage=>', this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return; // ✅ Guard clause
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Visitor'
      );
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((p: any) => p.PageName === tab.title && p.ViewAccess)
      );
    });
  }


  ngOnInit(): void {
    this.invitePageForm = this.fb.group({
      contact_name: ['', [Validators.required, Validators.pattern(/^[A-Za-z -]*$/), Validators.minLength(2), Validators.maxLength(30)]],
      designation: ['',],
      company: ['', []],
      purpose: [''],
      // per_email_id: ['', [Validators.required,  Validators.pattern('[a-zA-Z0-9+_.-.]+@[a-zA-Z0-9-]+.[a-z]{2,7}')]],
      official_email: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      mobile_number: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      alter_mobile_number: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      meet_to: ['', [Validators.required]],
      facilities: ['', [Validators.required]],
      uplaodImage: [''],
      date: ['', [Validators.required]],
      Time: ['', [Validators.required]]

    });
    this.populateHours();
    this.populateMinutes();
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = ('0' + (currentDate.getMonth() + 1)).slice(-2); // Add leading zero for months
    const day = ('0' + currentDate.getDate()).slice(-2); // Add leading zero for days

    this.today = `${year}-${month}-${day}`;

    setTimeout(() => {
      this.visitorGetAllEmployeeInviteApi();
      setTimeout(() => {
        this.getVisitorAccessDDCompanyApi();
        setTimeout(() => {
          this.visitorAccessDDEmployeeApi();
        }, 200)
      }, 200)
    }, 200)
  }
  convertToUppercase(event: any, controlName: string) {
    const value = event.target.value.toUpperCase();
    this.invitePageForm.get(controlName)?.setValue(value, { emitEvent: false });
  }
  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  getVisitorAccessDDCompanyApi() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;
    this.hrmsService.visitorAccessDDCompany(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getFacalities = res;
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessageFacility = 'Error loading data. Try Again.';
        this.triggerToast('Internal Server Error', 'Error Loading In Facility Data, Please Refresh Once', "danger");
        this.isSpinner = false;
      }
    });
  }

  visitorAccessDDEmployeeApi() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsService.visitorAccessDDEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessageEmpName = 'Error loading data. Please try again.';
        this.triggerToast('Internal Server Error', 'Error Loading Contact Person Please Refresh Once', "danger");
        this.isSpinner = false;
      }
    });
  }

  visitorGetAllEmployeeInviteApi() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner1 = true;
    this.hrmsService.VisitorGetAllEmployeeInvite(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        setTimeout(() => {
          this.rows = res;
          this.rows = res.map((item: any) => ({
            ...item,
            Date: this.formatDate(this.parseJsonDate(item.Date)),
            CheckIn: this.formatDate(this.parseJsonDate(item.CheckIn), true),
            CheckOut: this.formatDate(this.parseJsonDate(item.CheckOut), true)
          }));
          this.originalRows = [...this.rows];
          this.isSpinner1 = false;
        }, 1000);
      } else {
        this.errorMessageGetAllInvite = "No records found";
        this.isSpinner1 = false;
        this.isTableData = true;
      }
    }, error => {
      this.errorMessageGetAllInvite = "Internal Server Error";
      this.isSpinner1 = false;
      this.isTableData = true;
    })
  }
  onTimeInputChange() {
    const currentValue = this.timeInput.nativeElement.value;
    if (currentValue.length === 5 && this.previousTimeValue) {
      const previousMinutes = this.previousTimeValue.slice(3, 5);
      const currentMinutes = currentValue.slice(3, 5);
      if (currentMinutes !== previousMinutes) {
        this.timeInput.nativeElement.blur();
      }
    }
    this.previousTimeValue = currentValue;
  }

  preventKeyboardInput(event: KeyboardEvent) {
    event.preventDefault(); // Prevents any keyboard input
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault(); // Prevents paste input
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return `${dd}-${mm}-${yyyy}`;
  }

  populateHours(): void {
    this.hours = [];
    for (let i = 1; i <= 12; i++) {
      this.hours.push(i < 10 ? '0' + i : i.toString());
    }
  }

  populateMinutes(): void {
    this.minutes = [];
    for (let i = 0; i < 60; i++) {
      this.minutes.push(i < 10 ? '0' + i : i.toString());
    }
  }

  getFormattedTime(): string {
    const hour = this.invitePageForm.get('hour')?.value;
    const minute = this.invitePageForm.get('minute')?.value;
    const ampm = this.invitePageForm.get('ampm')?.value;
    return `${hour}:${minute} ${ampm}`;
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

  selectEmployeee(employee: any) {
    console.log(employee);

    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.invitePageForm.get('meet_to')?.setErrors({ invalidEmployee: true });
    } else {
      this.invitePageForm.get('meet_to')?.setErrors(null);
    }
  }
  //this is second code for contact person
  formatDate(date: Date | null, includeTime: boolean = false): string {
    if (!date) return '';
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
    const year = date.getFullYear();
    const formattedDay = day;
    const formattedMonth = month;
    const formattedYear = year;
    if (includeTime) {
      const hours = date.getHours().toString().padStart(2, '0');
      const minutes = date.getMinutes().toString().padStart(2, '0');
      return `${hours}:${minutes}`;
    }
    return `${formattedYear}-${formattedMonth}-${formattedDay}`; // To match 'today' format (yyyy-mm-dd)
  }
  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      return new Date(parseInt(match[1], 10));
    }
    return null;
  }

  deleteInvite(data: any) {
    console.log(data);
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      VisitId: data.VisitId
    }
    this.isSpinner = true;
    this.hrmsService.VisitorCancelInvite(reqBody).subscribe((res: any) => {
      if (res['msg'] === 'Invite Cancelled') {
        this.visitorGetAllEmployeeInviteApi();
        this.isSpinner = false;
        this.triggerToast(res['msg'], 'Records Removed From The Table', 'success');
      } else if (res['Message']) {
        this.triggerToast(res['Message'], res['Message'], 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.isSpinner = false;
      this.triggerToast('Internal Server Error', 'Something went wrong', 'danger');
    })
  }
  onView(data: any) {
    console.log(data);
    this.viewdata = data;
    const photo = this.viewdata.Photo;
    this.isValidPhoto = photo !== null && photo !== undefined && photo !== '';
    const getPhotoUrl = this.viewdata?.Photo.replace(/\\/g, "\\\\");
    this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;
  }
  filterSuggestions() {
    const control = this.invitePageForm.get('meet_to');
    if (this.searchTerm === '') {
      this.filteredSuggestions = [];
      this.errorMessage = null;
      this.isSuggestionSelected = false;
      control?.setErrors(null); // Clear errors
    } else {
      this.filteredSuggestions = this.employees.filter(employee =>
        employee.EmpName.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
      if (this.filteredSuggestions.length === 0) {
        this.errorMessage = `No results found for "${this.searchTerm}".`;
      } else {
        this.errorMessage = null;
      }
      this.isSuggestionSelected = false;
    }
    control?.updateValueAndValidity();
  }

  selectSuggestion(employee: any) {
    this.searchTerm = employee.EmpName;
    this.selectedEmployee = employee;
    this.filteredSuggestions = [];
    this.errorMessage = null;
    this.isSuggestionSelected = true;
    this.invitePageForm.get('meet_to')?.setValue(employee.EmpName);
    this.invitePageForm.get('meet_to')?.setErrors(null);
    this.invitePageForm.get('meet_to')?.updateValueAndValidity();
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (this.invitePageForm?.get('contact_name').valid) {
      if (input.files && input.files.length > 0) {
        const file = input.files[0];
        if (!file.type.match(/image\/(jpg|jpeg)/)) {
          alert('Only JPG and JPEG files are allowed.');
          input.value = ''; // Clear the input
          return;
        }
        if (file.size > 5 * 1024 * 1024) { // 5 MB
          alert('File size should not exceed 5 MB.');
          input.value = ''; // Clear the input
          return;
        }
        const reader = new FileReader();
        reader.onload = () => {
          this.imageSrc = reader.result;
        };
        reader.readAsDataURL(file);
        this.selectedFile = file;
        this.uploadImage();
      }
    } else {
      this.invitePageForm?.get('contact_name').reset();
      this.triggerToast('Invalid', 'Please Fill The Name', 'warning');
      input.value = '';
    }
  }

  uploadImage() {
    if (!this.selectedFile) {
      alert('No file selected.');
      return;
    }
    if (this.selectedFile) {
      this.isSpinner = true
      this.hrmsService.visitorFileUploadImage(this.invitePageForm?.get('contact_name').value, this.selectedFile).subscribe((res: any) => {
        if (res) {
          this.ImagePath = res.path;
          this.isSpinner = false;
          this.triggerToast(res['msg'], 'Profile Picture Uploaded', "success");
          this.isUploadSuccess = true;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner = false;
        this.isUploadSuccess = false;
      })
    }

  }

  submitForm() {
    this.isFormSubmitted = true;
    console.log(this.invitePageForm.valid, this.invitePageForm);
    // this.invitePageForm.get('meet_to')?.updateValueAndValidity();
    if (this.invitePageForm.valid) {
      const isPhotoRequired = this.invitePageForm?.get('uplaodImage')?.value;
      if (isPhotoRequired && !this.ImagePath) {
        this.triggerToast("Photo Required", "Please click send photo before submitting.", "danger");
        return; // Exit the function if a required photo is missing
      }
      const formattedTime = this.getFormattedTime();
      const dateObject = new Date(this.invitePageForm?.get('date').value);
      const year = dateObject.getFullYear();
      const month = String(dateObject.getMonth() + 1).padStart(2, '0'); // getMonth() is zero-based
      const day = String(dateObject.getDate()).padStart(2, '0');
      const dateOnly = `${day}-${month}-${year}`;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        Name: this.invitePageForm?.get('contact_name').value,
        Designation: this.invitePageForm?.get('designation').value,
        Company: this.invitePageForm?.get('company').value,
        Purpose: this.invitePageForm?.get('purpose').value,
        // PMail: this.invitePageForm?.get('per_email_id').value,
        OMail: this.invitePageForm?.get('official_email').value,
        Mobile: this.invitePageForm?.get('mobile_number').value,
        AMobile: this.invitePageForm?.get('alter_mobile_number').value,
        Photo: this.ImagePath ? this.ImagePath : '',
        CompId: this.invitePageForm?.get('facilities').value,
        // WhomtoMeet: this.selectedEmployee ? this.selectedEmployee.EmpId : '',
        WhomtoMeet: this.selectedEmployee,
        Date: dateOnly,
        // Time: formattedTime,
        Time: this.invitePageForm?.get('Time').value

      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.visitorInvitevisit(reqBody).subscribe((res: any) => {
        if (res["msg"]) {
          this.triggerToast(res['msg'], 'Mail Sent Successfully', 'success');
          this.isSpinner = false;
          this.visitorGetAllEmployeeInviteApi();
          this.resetFormData();
          this.isFormSubmitted = false;
          this.visitorAccessDDEmployeeApi();
        } else if (res["Message"]) {
          this.triggerToast(res['Message'], "Sorry Something went wrong", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
    }
    else {
      // this.triggerToast("Invalid", "Please Enter valid Credentials ", "danger");
      this.isSpinner = false;
    }
  }

  resetFormData() {
    this.invitePageForm.reset();
    this.isUploadSuccess = false;
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.imageSrc = '';
    this.selectedFile = null;
    this.ImagePath = '';
    this.invitePageForm.value = '';
    this.isEdited = false;
    this.isFormSubmitted = false;
    this.errorMessageEmpName = '';
    setTimeout(() => {
      if (this.inputValue?.nativeElement) {
        this.inputValue.nativeElement.value = null;
        const event = new KeyboardEvent('keyup', { bubbles: true });
        this.inputValue.nativeElement.dispatchEvent(event);
        this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));  // Ensure this method handles its own logic
      }
    }, 100);
  }

  pageChange(event: any) {
    this.page = event
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
      this.errorMessageGetAllInvite = 'No Records Found for Searched Data';
      this.rows = [...this.originalRows];
    } else {
      this.isTableData = false;
      this.errorMessageGetAllInvite = null;
    }
  }
  handleAlphaChar(event: any) {
    if (
      (event.charCode > 32 && event.charCode < 48) ||
      (event.charCode > 57 && event.charCode < 127)
    ) {
      event.preventDefault();
    }
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  onFocus(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  onBlur(event: FocusEvent) {
    setTimeout(() => {
      this.showDropdown = false;
    }, 300);
    // this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  setFloatingLabel(selectElement: HTMLSelectElement) {
    const label = selectElement.nextElementSibling as HTMLElement;
    if (selectElement.value) {
      label.classList.add('floating');
    } else {
      label.classList.remove('floating');
    }
  }

}
