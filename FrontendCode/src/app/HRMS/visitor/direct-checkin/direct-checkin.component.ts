import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { Router, RouterModule } from '@angular/router';
import { WebcamImage, WebcamModule } from 'ngx-webcam';
import { CameraComponent } from '../../camera/camera.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-direct-checkin',
  standalone: true,
  imports: [ToastMessageComponent, CommonModule, SharedModule, FormsModule,
    ReactiveFormsModule, WebcamModule, CameraComponent, RouterModule],
  templateUrl: './direct-checkin.component.html',
  styleUrl: './direct-checkin.component.scss'
})
export class DirectCheckinComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('cameraStream', { static: true }) videoElement!: ElementRef<HTMLVideoElement>;
  @ViewChild('cameraCanvas', { static: true }) canvasElement!: ElementRef<HTMLCanvasElement>;
  @ViewChild('timeInput') timeInput: any = ElementRef;

  @Output()
  public pictureTaken = new EventEmitter<WebcamImage>();

  @Input()
  public videoWidth: number = 100;  // Default value

  @Input()
  public videoHeight: number = 150; // Default value
  public webcamImage: WebcamImage | null = null;
  public showCamera: boolean = false;
  public imageHeight: number = 50; // Adjust as needed
  public imageWidth: number = 50;  // Adjust as needed

  constructor(private readonly fb: FormBuilder, private readonly hrmsService: HrmsServiceService, private route: Router,
    private readonly cdr: ChangeDetectorRef, private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    if (storedEmployeeDetails) {
      this.employeeDetails = JSON.parse(storedEmployeeDetails);
    }
    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Direct Checkin'
    // );
    // this.controlAccessPage=viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Direct Checkin'
      );
    });
  }
  isSpinner: boolean = false;
  accessPolicy: any;
  controlAccessPage: any;
  directCheckinForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails;
  getFacalities: any[] = [];
  employees: any[] = [];
  errorMessageFacility: any;
  errorMessage: any;
  searchTerm: string = '';
  filteredSuggestions: any[] = [];
  selectedEmployee: any = null;
  today: any;
  hours: string[] = [];
  minutes: string[] = [];
  ampm: string[] = ['AM', 'PM'];
  imageSrc: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;
  cameraPicture?: any;
  ImagePath: any;
  isEdited: boolean = false;
  isUploadSuccess: boolean = false;
  isPhotoChosen: boolean = false; // Track if a photo has been chosen
  enableDocFile: boolean = false;
  isSuggestionSelected: boolean = false;
  showDropdown: boolean = false;
  approverEmployeeNames: any[] = [];
  filteredEmployeeNames: any[] = [];
  errorMessageEmpName: any;
  previousTimeValue: string = '';
  isFileUploaded = false;

  ngOnInit(): void {
    this.directCheckinForm = this.fb.group({
      contact_name: ['', [Validators.required, Validators.pattern(/^[A-Za-z -]*$/), Validators.minLength(2), Validators.maxLength(30)]],
      designation: ['',],
      company: ['', []],
      purpose: [''],
      // per_email_id: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9+_.-.]+@[a-zA-Z0-9-]+.[a-z]{2,7}')]],
      // official_email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9+_.-.]+@[a-zA-Z0-9-]+.[a-z]{2,7}')]],
      official_email: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      mobile_number: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      alter_mobile_number: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      meet_to: ['', [Validators.required]],
      facilities: ['', [Validators.required]],
      IdCard: ['', [Validators.required]],
      description: [],
      uploadImage: [''],
      date: ['', [Validators.required]],
      // hour: ['', Validators.required],
      // minute: ['', Validators.required],
      // ampm: ['', Validators.required],
      Time: ['', [Validators.required]]

    });

    setTimeout(() => {
      this.getVisitorAccessDDCompany();
      setTimeout(() => {
        this.visitorAccessDDEmployee();
      }, 100);
    }, 1000);
    this.populateHours();
    this.populateMinutes();
    // this.today = this.getCurrentDate();
    const now = new Date();
    this.today = now.toISOString().split('T')[0]; // Format as YYYY-MM-DD;

    this.directCheckinForm?.get('contact_name').valueChanges.subscribe((res: any) => {
      if (this.directCheckinForm?.get('contact_name').valid) {
        this.enableDocFile = true;
      } else {
        this.enableDocFile = false;
      }
    })
  }
  convertToUppercase(event: any, controlName: string) {
    const value = event.target.value.toUpperCase();
    this.directCheckinForm.get(controlName)?.setValue(value, { emitEvent: false });
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
    for (let i = 1; i <= 12; i++) {
      this.hours.push(i < 10 ? '0' + i : i.toString());
    }
  }

  populateMinutes(): void {
    for (let i = 0; i < 60; i++) {
      this.minutes.push(i < 10 ? '0' + i : i.toString());
    }
  }

  getFormattedTime(): string {
    const hour = this.directCheckinForm.get('hour')?.value;
    const minute = this.directCheckinForm.get('minute')?.value;
    const ampm = this.directCheckinForm.get('ampm')?.value;
    return `${hour}:${minute} ${ampm}`;
  }

  getVisitorAccessDDCompany() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;

    this.hrmsService.visitorAccessDDCompany(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getFacalities = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
          this.isSpinner = false;
        }
      },
      error: (error: any) => {
        this.errorMessageFacility = 'Error loading data. Please try again.';
        this.triggerToast('Internal Server Error', 'Error Loading In Facility Data, Please Refresh Once', "danger");
        this.isSpinner = false;
      },
      complete: () => {
        // Optional: Handle any cleanup or additional actions after completion
      }
    });
  }


  //this is second code for contact person
  searchText: string = '';
  isDropdownOpen = false;
  filteredEmployees: any[] = [];
  isValidEmployee: boolean = true;
  visitorAccessDDEmployee() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsService.visitorAccessDDEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
          this.isSpinner = false;
        }
      },
      error: (error: any) => {
        this.errorMessageEmpName = 'Error loading data. Please try again.';
        this.triggerToast('Internal Server Error', 'Error Loading Contact Person Please Refresh Once', "danger");
        this.isSpinner = false;
      },
      complete: () => {
        // Optional: Handle any cleanup or additional actions here if necessary
      }
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

  selectEmployeee(employee: any) {
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
      this.directCheckinForm.get('meet_to')?.setErrors({ invalidEmployee: true });
    } else {
      this.directCheckinForm.get('meet_to')?.setErrors(null);
    }
  }
  //this is second code for contact person

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (this.directCheckinForm?.get('contact_name').valid) {
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
        this.isFileUploaded = true;
        this.isPhotoChosen = false; // Disable camera if file is uploaded
        this.uploadImage();
      }
    } else {
      this.directCheckinForm?.get('contact_name').reset();
      this.triggerToast('Invalid', 'Please Fill The Name', 'warning');
      input.value = '';
    }
  }

  handleImage(webcamImage: any) {
    this.webcamImage = webcamImage;
    this.isPhotoChosen = true;
    this.isFileUploaded = false; // Disable file upload if camera photo is chosen
    this.showCamera = false;
    this.uploadImage();
  }

  toggleCamera(): void {
    if (!this.isFileUploaded) {
      this.showCamera = !this.showCamera;
    }
  }
  // Convert WebcamImage to Blob (you may need to adjust based on your webcamImage format)
  convertWebcamImageToBlob(webcamImage: WebcamImage): Blob {
    // Assuming webcamImage has imageAsDataUrl property
    const dataUrl = webcamImage.imageAsDataUrl;
    const byteString = atob(dataUrl.split(',')[1]);
    const mimeString = dataUrl.split(',')[0].split(':')[1].split(';')[0];
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
  }

  uploadImage(): void {
    if (!this.selectedFile && !this.webcamImage) {
      alert('No file or image selected.');
      return;
    }
    if (this.directCheckinForm?.get('contact_name').valid) {
      let fileToUpload: File | null = null;

      if (this.selectedFile) {
        fileToUpload = this.selectedFile;
      } else if (this.webcamImage) {
        const blob = this.convertWebcamImageToBlob(this.webcamImage);
        fileToUpload = new File([blob], 'webcam_image.jpg', { type: blob.type });
      }
      if (fileToUpload) {
        this.isSpinner = true;
        this.hrmsService.visitorFileUploadImage(
          this.directCheckinForm?.get('contact_name').value,
          fileToUpload
        ).subscribe(
          (res: any) => {
            if (res) {
              this.ImagePath = res.path;
              this.isSpinner = false;
              this.triggerToast(res['msg'], 'Profile Picture Uploaded', "success");
              this.isUploadSuccess = true;
            }
          },
          error => {
            this.triggerToast(error['Message'], 'Internal Server Error', "danger");
            this.isSpinner = false;
            this.isUploadSuccess = false;
          }
        );
      }
    } else {
      this.triggerToast('', 'Please Enter The Name', "warning");
      this.resetImageSelection();
    }

  }
  resetImageSelection(): void {
    this.imageSrc = '';
    this.selectedFile = null;
    this.ImagePath = '';
    this.showCamera = false;
    this.webcamImage = null;
    this.isFileUploaded = false;
    this.isPhotoChosen = false;
  }

  submitForm() {
    this.isFormSubmitted = true;
    if (this.directCheckinForm.valid) {
      const isPhotoRequired = this.directCheckinForm?.get('uploadImage')?.value;
      if (isPhotoRequired && !this.ImagePath) {
        this.triggerToast("Photo Required", "Please click send photo before submitting.", "danger");
        return; // Exit the function if a required photo is missing
      }

      const formattedTime = this.getFormattedTime();
      const dateObject = new Date(this.directCheckinForm?.get('date').value);
      const year = dateObject.getFullYear();
      const month = String(dateObject.getMonth() + 1).padStart(2, '0'); // getMonth() is zero-based
      const day = String(dateObject.getDate()).padStart(2, '0');
      const dateOnly = `${day}-${month}-${year}`;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        Name: this.directCheckinForm?.get('contact_name').value,
        Designation: this.directCheckinForm?.get('designation').value,
        Company: this.directCheckinForm?.get('company').value,
        Purpose: this.directCheckinForm?.get('purpose').value,
        // PMail: this.directCheckinForm?.get('per_email_id').value,
        OMail: this.directCheckinForm?.get('official_email').value,
        Mobile: this.directCheckinForm?.get('mobile_number').value,
        AMobile: this.directCheckinForm?.get('alter_mobile_number').value,
        Photo: this.ImagePath ? this.ImagePath : '',
        CompId: this.directCheckinForm?.get('facilities').value,
        // WhomtoMeet: this.selectedEmployee ? this.selectedEmployee.EmpId : '',
        WhomtoMeet: this.selectedEmployee,
        IdCard: this.directCheckinForm?.get('IdCard').value,
        Accessories: this.directCheckinForm?.get('description').value ? this.directCheckinForm?.get('description').value : '',
        Date: dateOnly,
        // Time: formattedTime,
        Time: this.directCheckinForm?.get('Time').value
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.visitorDirectCheckIn(reqBody).subscribe((res: any) => {
        if (res["msg"]) {
          this.triggerToast(res['msg'], 'Mail Sent Successfully', 'success');
          this.isSpinner = false;
          this.resetFormData();
          this.isFormSubmitted = false;
          this.visitorAccessDDEmployee();
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
    this.directCheckinForm.reset();
    this.isFormSubmitted = false;
    this.isUploadSuccess = false;
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.imageSrc = '';
    this.selectedFile = null;
    this.ImagePath = '';
    this.directCheckinForm.value = '';
    this.isEdited = false;
    this.showCamera = false; // Hide camera
    this.webcamImage = null; // Clear webcam image
    this.isPhotoChosen = false;
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
      this.showDropdown = false; // Hide the dropdown on blur
    }, 200);
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

}
