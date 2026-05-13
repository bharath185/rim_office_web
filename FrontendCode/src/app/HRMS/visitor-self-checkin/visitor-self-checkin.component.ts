import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { WebcamImage, WebcamModule } from 'ngx-webcam';
import { HrmsServiceService } from '../hrms-service.service';
import { Subscription } from 'rxjs';
import { CameraComponent } from '../camera/camera.component';
import { Router } from '@angular/router';
import { environment } from 'src/assets/environment';
import { NgOtpInputModule } from 'ng-otp-input';
import { formatDate } from '@angular/common';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-visitor-self-checkin',
  standalone: true,
  imports: [ToastMessageComponent, SharedModule, CameraComponent, WebcamModule, NgOtpInputModule, FormsModule,
    ReactiveFormsModule,],
  templateUrl: './visitor-self-checkin.component.html',
  styleUrl: './visitor-self-checkin.component.scss'
})
export class VisitorSelfCheckinComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('cameraStream', { static: true }) videoElement!: ElementRef<HTMLVideoElement>;
  @ViewChild('cameraCanvas', { static: true }) canvasElement!: ElementRef<HTMLCanvasElement>;
  @ViewChild('timeInput') timeInput: any = ElementRef;
  baseUrl: string = environment.baseUrl;

  @Output()
  public pictureTaken = new EventEmitter<WebcamImage>();
  @Input()
  public videoWidth: number = 100;

  @Input()
  public videoHeight: number = 150;
  public webcamImage: WebcamImage | null = null;
  public showCamera: boolean = false;
  public imageHeight: number = 50;
  public imageWidth: number = 50;

  config: any = {
    length: 6,
    allowNumbersOnly: true,
    isPasswordInput: false,
    inputStyles: {
      width: '25px',
      height: '30px',
      'border-radius': '3px',
      'font-size': '15px',
      'cursor': 'pointer',
      'margin-right': '2px',
      'margin-top': '10px',
    },
  };

  otpForm: any = FormGroup;
  otp: FormControl = new FormControl('', [Validators.required]);
  otpSubscription: Subscription;

  constructor(private readonly fb: FormBuilder, private readonly hrmsService: HrmsServiceService, private readonly route: Router,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.otpForm = new FormGroup({
      otp: this.otp,
    });

    this.otpSubscription = this.otpForm.get('otp').valueChanges.pipe(
      debounceTime(500)
    ).subscribe((value: string) => {
      if (value.length === this.config.length) { // Check if OTP length is valid
        this.checkOtpValidity(value);
      }
    });
  }

  isSpinner: boolean = false;
  visitorSelfCheckInForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  getFacalities: any[] = [];
  employees: any[] = [];
  errorMessageFacility: any;
  errorMessage: any;
  searchTerm: string = '';
  filteredSuggestions: any[] = [];
  selectedEmployee: any = null;
  today: any;
  // hours: string[] = [];
  // minutes: string[] = [];
  ampm: string[] = ['AM', 'PM'];
  imageSrc: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;
  ImagePath: any;
  isUploadSuccess: boolean = false;
  isPhotoChosen: boolean = false;
  isSuggestionSelected: boolean = false;
  showDropdown: boolean = false;
  approverEmployeeNames: any[] = [];
  filteredEmployeeNames: any[] = [];
  errorMessageEmpName: any;
  previousTimeValue: string = '';
  isValidPhoto: any;
  patchPhotoUrl: any;
  patchValue: any;
  isInvitedCode: boolean = true;
  showForm: string | null = null;
  isValidEmployee: boolean = true;
  isRestButtonShow: boolean = true;
  searchText: string = '';
  isDropdownOpen = false;
  filteredEmployees: any[] = [];

  ngOnInit(): void {

    this.visitorSelfCheckInForm = this.fb.group({
      contact_name: ['', [Validators.required, Validators.pattern(/^[A-Za-z -]*$/), Validators.minLength(2), Validators.maxLength(30)]],
      designation: ['', [Validators.required]],
      company: ['', [Validators.required]],
      purpose: ['', [Validators.required]],
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
    this.getVisitorAccessDDCompany();
    this.visitorAccessDDEmployee();
    // this.populateHours();
    // this.populateMinutes();
    // this.today = this.getCurrentDate();
    const now = new Date();
    this.today = now.toISOString().split('T')[0]; // Format as YYYY-MM-DD;
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

  // getCurrentDate(): string {
  //   const today = new Date();
  //   const dd = String(today.getDate()).padStart(2, '0');
  //   const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  //   const yyyy = today.getFullYear();
  //   return `${dd}-${mm}-${yyyy}`;
  // }

  // populateHours(): void {
  //   for (let i = 1; i <= 12; i++) {
  //     this.hours.push(i < 10 ? '0' + i : i.toString());
  //   }
  // }

  // populateMinutes(): void {
  //   for (let i = 0; i < 60; i++) {
  //     this.minutes.push(i < 10 ? '0' + i : i.toString());
  //   }
  // }

  // getFormattedTime(): string {
  //   const hour = this.visitorSelfCheckInForm.get('hour')?.value;
  //   const minute = this.visitorSelfCheckInForm.get('minute')?.value;
  //   const ampm = this.visitorSelfCheckInForm.get('ampm')?.value;
  //   return `${hour}:${minute} ${ampm}`;
  // }

  getVisitorAccessDDCompany() {
    this.isSpinner = true;
    this.hrmsService.visitorDDCompany().subscribe((res: any) => {
      if (res.length >= 1) {
        this.getFacalities = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.errorMessageFacility = 'Error loading data. Please try again.';
      this.triggerToast('Internal Server Error', 'Error Loading In Facility Data, Please Refresh Once', "danger");
      this.isSpinner = false;
    })
  }
  visitorAccessDDEmployee() {
    this.isSpinner = true;
    this.hrmsService.visitorDDEmployee().subscribe((res: any) => {
      if (res && res.length >= 1) {
        this.employees = res;
        this.isSpinner = false;
      } else {
        this.isSpinner = false;
        this.triggerToast('No data Found ', 'To Load The Employee Name', "warning");
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Error Loading Contact Person Please Refresh Once', "danger");
      this.isSpinner = false;
      // this.errorMessageEmpName = 'Error loading data. Please try again later'
    });
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  openDropdown() {
    this.isDropdownOpen = true;
    this.filteredEmployees = [...this.employees]; // Show all employees when opening
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
      // If no search text, show the full list
      this.filteredEmployees = [...this.employees];
    }
  }

  selectEmployee(employee: any) {
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
      this.visitorSelfCheckInForm.get('meet_to')?.setErrors({ invalidEmployee: true });
    } else {
      this.visitorSelfCheckInForm.get('meet_to')?.setErrors(null);
    }
  }
  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (this.visitorSelfCheckInForm?.get('contact_name').valid) {
      if (input.files && input.files.length > 0) {
        const file = input.files[0];

        // Validate file type
        if (!file.type.match(/image\/(jpg|jpeg)/)) {
          alert('Only JPG and JPEG files are allowed.');
          input.value = ''; // Clear the input
          return;
        }

        // Check file size and compress if needed
        if (file.size > 5 * 1024 * 1024) { // 5 MB
          const reader = new FileReader();
          reader.onload = (e: any) => {
            const img = new Image();
            img.src = e.target.result;

            img.onload = () => {
              const canvas = document.createElement('canvas');
              const ctx = canvas.getContext('2d')!;
              const maxWidth = 800; // Adjust max width if needed
              const maxHeight = 800; // Adjust max height if needed

              let width = img.width;
              let height = img.height;

              // Maintain aspect ratio
              if (width > height) {
                if (width > maxWidth) {
                  height = (height * maxWidth) / width;
                  width = maxWidth;
                }
              } else {
                if (height > maxHeight) {
                  width = (width * maxHeight) / height;
                  height = maxHeight;
                }
              }

              canvas.width = width;
              canvas.height = height;
              ctx.drawImage(img, 0, 0, width, height);

              // Convert canvas to Blob and handle the reduced file
              canvas.toBlob(
                (blob) => {
                  if (blob) {
                    const compressedFile = new File([blob], file.name, { type: file.type });
                    this.processFile(compressedFile);
                  }
                },
                file.type,
                0.7 // Adjust quality if needed (0.7 = 70% quality)
              );
            };
          };
          reader.readAsDataURL(file);
        } else {
          // File size is under 5 MB, process directly
          this.processFile(file);
        }
      }
    } else {
      this.visitorSelfCheckInForm?.get('contact_name').reset();
      this.triggerToast('Invalid', 'Please Fill The Name', 'warning');
      input.value = '';
    }
  }

  private processFile(file: File): void {
    const reader = new FileReader();
    reader.onload = () => {
      this.imageSrc = reader.result;
    };
    reader.readAsDataURL(file);

    this.selectedFile = file;
    this.uploadImage();
  }


  handleImage(webcamImage: any) {
    this.webcamImage = webcamImage;
    this.isPhotoChosen = false;
    this.showCamera = false;
    this.uploadImage();
  }

  public toggleCamera(): void {
    if (!this.isPhotoChosen) {
      this.showCamera = !this.showCamera;
    }
  }
  convertWebcamImageToBlob(webcamImage: WebcamImage): Blob {
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
    if (this.visitorSelfCheckInForm?.get('contact_name').valid) {
      let fileToUpload: File | null = null;

      if (this.selectedFile) {
        fileToUpload = this.selectedFile;
      }
      // else if (this.webcamImage) {
      //   const blob = this.convertWebcamImageToBlob(this.webcamImage);
      //   fileToUpload = new File([blob], 'webcam_image.jpg', { type: blob.type });
      // }
      if (fileToUpload) {
        this.isSpinner = true;
        this.hrmsService.visitorFileUploadImage(
          this.visitorSelfCheckInForm?.get('contact_name').value,
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
            this.imageSrc = '';
            this.imageSrc = null;
            this.selectedFile = null;
            this.ImagePath = '';
            this.showCamera = false;
            this.webcamImage = null;
          }
        );
      }
    } else {
      this.triggerToast('', 'Please Enter The Name', "warning");
      this.imageSrc = '';
      this.imageSrc = null;
      this.selectedFile = null;
      this.ImagePath = '';
      this.showCamera = false;
      this.webcamImage = null;
      return;
    }
  }
  isPhotoPreset() {
    if (!this.isValidPhoto) {
      console.log(this.ImagePath);

      if (this.ImagePath) {
        return this.ImagePath;
      } else {
        return '';
      }
    } else {
      const getPhotoUrl = this.patchValue?.Photo.replace(/\\/g, "\\\\");
      this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;
      return this.patchPhotoUrl;
    }
  }
  checkOtpValidity(value: string) {
    if (value && value.length === 6) {
      this.otpForm.get('otp').setErrors(null);
      this.verifyOtpAutomatically();
    } else {
      this.otpForm.get('otp').setErrors({ 'invalid': true });
    }
  }
  verifyOtpAutomatically() {
    if (this.otpForm.valid) {
      const reqBody = {
        otp: this.otpForm.get('otp').value
      };
      this.isSpinner = true;
      this.hrmsService.visitorVerifyOTPCheckIn(reqBody).subscribe(
        (res: any) => {
          const photo = res.Photo;
          this.patchValue = res;
          this.isValidPhoto = photo !== null && photo !== undefined && photo !== '';
          this.patchPhotoUrl = `${this.baseUrl}/${photo}`;
          let formattedDate: string | undefined;
          if (res.Date) {
            const dateMatch = res.Date.match(/\d+/);
            if (dateMatch) {
              const visitDate = new Date(parseInt(dateMatch[0], 10));
              formattedDate = formatDate(visitDate, 'yyyy-MM-dd', 'en-US');
            } else {
              console.error("Invalid date format received:", res.Date);
            }
          } else {
            console.error("Date is missing in the response.");
          }
          if (res['msg'] === 'OTP Verified Successfully') {
            this.showForm = 'yes';
            this.visitorSelfCheckInForm.patchValue({
              contact_name: res.Name,
              designation: res.Designation,
              company: res.Company,
              purpose: res.Purpose,
              official_email: res.OMail,
              mobile_number: res.Mobile,
              alter_mobile_number: res.AMobile,
              facilities: res.CompId,
              meet_to: res.WName,
              IdCard: res.IdCard,
              description: res.Accessories,
              date: formattedDate,
              Time: res.Time,
            });
            this.visitorSelfCheckInForm.disable();
            this.visitorSelfCheckInForm?.get('IdCard').enable();
            this.visitorSelfCheckInForm?.get('description').enable();
            this.isRestButtonShow = false;
          } else if (res['Message'] === "OTP Invalid") {
            this.triggerToast(res['Message'], res['Message'], "warning");
          } else if (res['Message']) {
            this.triggerToast(res['Message'], res['Message'], "warning");
          }
        },
        (error) => {
          console.error("Error during OTP verification:", error);
          this.triggerToast("An error occurred while verifying OTP", "Please try again.", "danger");
        }
      ).add(() => {
        this.isSpinner = false;
      });
    }
  }

  yesValue() {
    this.isInvitedCode = false;
  }

  noValue() {
    this.showForm = 'yes';
    this.resetFormData();
    this.isValidPhoto = false;
    this.isInvitedCode = false;
  }

  //this is geoloaction
  locationError: string | null = null;
  officeLat: number = 12.976128
  officeLng: number = 77.5258112

  // Calculate the distance between two coordinates using the Haversine formula
  getDistanceFromLatLon(lat1: number, lon1: number, lat2: number, lon2: number): number {
    const R = 6371e3; // Radius of the Earth in meters
    const dLat = this.deg2rad(lat2 - lat1);
    const dLon = this.deg2rad(lon2 - lon1);
    const a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(this.deg2rad(lat1)) * Math.cos(this.deg2rad(lat2)) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return R * c; // Distance in meters
  }

  // Convert degrees to radians
  deg2rad(deg: number): number {
    return deg * (Math.PI / 180);
  }

  // Check if the user is within a 20-meter radius
  isWithinOfficeRadius(userLat: number, userLng: number): boolean {
    const distance = this.getDistanceFromLatLon(userLat, userLng, this.officeLat, this.officeLng);
    return distance <= 20;  // 20 meters radius
  }

  // checkInSubmitForm(): void {
  //   if (navigator.geolocation) {
  //     navigator.geolocation.getCurrentPosition(
  //       (position) => {
  //         const userLat = position.coords.latitude;
  //         const userLng = position.coords.longitude;
  //         if (this.isWithinOfficeRadius(userLat, userLng)) {
  //           this.locationError = null;
  //           if (this.visitorSelfCheckInForm?.valid) {
  //             this.isFormSubmitted = false;
  //             const dateObject = new Date(this.visitorSelfCheckInForm?.get('date').value);
  //             const year = dateObject.getFullYear();
  //             const month = String(dateObject.getMonth() + 1).padStart(2, '0');
  //             const day = String(dateObject.getDate()).padStart(2, '0');
  //             const dateOnly = `${day}-${month}-${year}`;
  //             const reqBody = {
  //               Name: this.visitorSelfCheckInForm?.get('contact_name').value,
  //               Designation: this.visitorSelfCheckInForm?.get('designation').value,
  //               Company: this.visitorSelfCheckInForm?.get('company').value,
  //               Purpose: this.visitorSelfCheckInForm?.get('purpose').value,
  //               OMail: this.visitorSelfCheckInForm?.get('official_email').value,
  //               Mobile: this.visitorSelfCheckInForm?.get('mobile_number').value,
  //               AMobile: this.visitorSelfCheckInForm?.get('alter_mobile_number').value,
  //               CompId: this.visitorSelfCheckInForm?.get('facilities').value,
  //               WhomtoMeet: this.selectedEmployee ? this.selectedEmployee : this.patchValue.WhomtoMeet,
  //               IdCard: this.visitorSelfCheckInForm?.get('IdCard').value,
  //               Accessories: this.visitorSelfCheckInForm?.get('description').value ? this.visitorSelfCheckInForm?.get('description').value : '',
  //               Date: dateOnly,
  //               Time: this.visitorSelfCheckInForm?.get('Time').value,
  //               Photo: this.isPhotoPreset()
  //             }
  //             console.log(reqBody);
  //             this.isSpinner = true;
  //             this.hrmsService.visitorVisitorSelftCheckIn(reqBody).subscribe((res: any) => {
  //               if (res['msg']) {
  //                 this.triggerToast(res['msg'], 'Updated Successfully', 'success');
  //                 this.route.navigate(['invite_success']);
  //                 this.isSpinner = false;
  //               } else {
  //                 this.triggerToast(res['Message'], "", "warning");
  //                 this.isSpinner = false;
  //               }
  //             }, error => {
  //               this.triggerToast(error['Message'], 'Internal Server Error', "danger");
  //               this.isSpinner = false;
  //             })
  //           } else {
  //             this.isSpinner = false;
  //           }
  //           this.triggerToast('', 'you are within the office premises', "success");
  //           this.isFormSubmitted = true;
  //         } else {
  //           this.locationError = "You are not on office premises!";
  //           this.triggerToast('', 'You are not on office premises!', "warning");
  //         }
  //       },
  //       (error) => { /* Error Handler */ },
  //       {
  //         enableHighAccuracy: true,
  //         timeout: 10000,
  //         maximumAge: 0
  //       }
  //     );
  //   } else {
  //     this.locationError = "Geolocation is not supported by this browser!";
  //     this.triggerToast('', 'Geolocation is not supported by this browser!', "warning");
  //   }
  // }

  //this is geoloaction

  checkInSubmitForm() {
    this.isFormSubmitted = true;
    console.log(this.visitorSelfCheckInForm?.valid);

    if (this.visitorSelfCheckInForm?.valid) {
      this.isFormSubmitted = false;
      const dateObject = new Date(this.visitorSelfCheckInForm?.get('date').value);
      const year = dateObject.getFullYear();
      const month = String(dateObject.getMonth() + 1).padStart(2, '0');
      const day = String(dateObject.getDate()).padStart(2, '0');
      const dateOnly = `${day}-${month}-${year}`;
      const reqBody = {
        Name: this.visitorSelfCheckInForm?.get('contact_name').value,
        Designation: this.visitorSelfCheckInForm?.get('designation').value,
        Company: this.visitorSelfCheckInForm?.get('company').value,
        Purpose: this.visitorSelfCheckInForm?.get('purpose').value,
        OMail: this.visitorSelfCheckInForm?.get('official_email').value,
        Mobile: this.visitorSelfCheckInForm?.get('mobile_number').value,
        AMobile: this.visitorSelfCheckInForm?.get('alter_mobile_number').value,
        CompId: this.visitorSelfCheckInForm?.get('facilities').value,
        WhomtoMeet: this.selectedEmployee ? this.selectedEmployee : this.patchValue.WhomtoMeet,
        IdCard: this.visitorSelfCheckInForm?.get('IdCard').value,
        Accessories: this.visitorSelfCheckInForm?.get('description').value ? this.visitorSelfCheckInForm?.get('description').value : '',
        Date: dateOnly,
        Time: this.visitorSelfCheckInForm?.get('Time').value,
        Photo: this.isPhotoPreset()
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.visitorVisitorSelftCheckIn(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Updated Successfully', 'success');
          this.route.navigate(['success']);
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
    } else {
      this.isSpinner = false;
    }
  }

  resetFormData() {
    this.visitorSelfCheckInForm.reset();
    this.isFormSubmitted = false;
    this.isUploadSuccess = false;
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.imageSrc = '';
    this.selectedFile = null;
    this.ImagePath = '';
    this.visitorSelfCheckInForm.value = '';
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
      this.showDropdown = false;
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

  onCapture(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    const fileToUpload = fileInput.files?.[0];

    if (this.visitorSelfCheckInForm?.get('contact_name').valid) {
      if (fileToUpload) {
        if (fileToUpload.size > 5 * 1024 * 1024) {
          // If file size exceeds 5MB, compress it
          this.compressImage(fileToUpload, 5 * 1024 * 1024).then((compressedFile) => {
            this.uploadFile(compressedFile);
          });
        } else {
          // If file size is within limit, upload directly
          this.uploadFile(fileToUpload);
        }
      }
    } else {
      this.triggerToast('', 'Please Enter The Name', 'warning');
      this.resetUploadState();
    }
  }

  uploadFile(file: File): void {
    // Display a preview of the image
    const reader = new FileReader();
    reader.onload = () => (this.imageSrc = reader.result);
    reader.readAsDataURL(file);
    const contactName = this.visitorSelfCheckInForm?.get('contact_name').value;
    this.isSpinner = true;
    this.hrmsService.visitorFileUploadImage(contactName, file).subscribe(
      (res: any) => {
        if (res) {
          this.ImagePath = res.path;
          this.isSpinner = false;
          this.triggerToast(res['msg'], 'Profile Picture Uploaded', 'success');
          this.isUploadSuccess = true;
        }
      },
      (error) => {
        this.triggerToast(error['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
        this.isUploadSuccess = false;
      }
    );
  }

  compressImage(file: File, maxSizeInBytes: number): Promise<File> {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.onload = (event: any) => {
        const img = new Image();
        img.src = event.target.result;
        img.onload = () => {
          const canvas = document.createElement('canvas');
          const ctx = canvas.getContext('2d')!;
          const maxWidth = 800;
          const maxHeight = 800;
          let width = img.width;
          let height = img.height;
          // Maintain aspect ratio
          if (width > height) {
            if (width > maxWidth) {
              height = (height * maxWidth) / width;
              width = maxWidth;
            }
          } else {
            if (height > maxHeight) {
              width = (width * maxHeight) / height;
              height = maxHeight;
            }
          }

          canvas.width = width;
          canvas.height = height;
          ctx.drawImage(img, 0, 0, width, height);

          // Convert the canvas to a Blob
          canvas.toBlob(
            (blob) => {
              if (blob) {
                const compressedFile = new File([blob], file.name, { type: file.type });
                resolve(compressedFile);
              }
            },
            file.type,
            0.7 // Compression quality (0.7 = 70%)
          );
        };
      };

      reader.readAsDataURL(file);
    });
  }

  resetUploadState(): void {
    this.imageSrc = null;
    this.selectedFile = null;
    this.ImagePath = '';
    this.showCamera = false;
    this.webcamImage = null;
  }




  // Example 2
  hours: string[] = Array.from({ length: 24 }, (_, i) => ('0' + i).slice(-2));
  minutes: string[] = Array.from({ length: 60 }, (_, i) => ('0' + i).slice(-2));

  selectedHour: string = '';
  selectedMinute: string = '';

  isHourDropdownOpen = false;
  isMinuteDropdownOpen = false;

  openHourDropdown() {
    this.isHourDropdownOpen = true;
  }

  openMinuteDropdown() {
    this.isMinuteDropdownOpen = true;
  }

  closeDropdowns() {
    this.isHourDropdownOpen = false;
    this.isMinuteDropdownOpen = false;
  }

  selectHour(hour: string) {
    this.selectedHour = hour;
    this.closeDropdowns();
  }

  selectMinute(minute: string) {
    this.selectedMinute = minute;
    this.closeDropdowns();
  }

  // Optional: Validation check when form is submitted
  checkValidTime() {
    if (!this.selectedHour || !this.selectedMinute) {
      this.isFormSubmitted = true;
    }
  }

}
