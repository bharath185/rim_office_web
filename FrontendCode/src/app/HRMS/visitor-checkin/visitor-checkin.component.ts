import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { ActivatedRoute, Router } from '@angular/router';
import { HrmsServiceService } from '../hrms-service.service';
import { environment } from 'src/assets/environment';
import { WebcamImage, WebcamModule } from 'ngx-webcam';
import { CameraComponent } from '../camera/camera.component';

@Component({
  selector: 'app-visitor-checkin',
  standalone: true,
  imports: [ToastMessageComponent, CommonModule, SharedModule, FormsModule, ReactiveFormsModule,
    WebcamModule,CameraComponent
  ],
  templateUrl: './visitor-checkin.component.html',
  styleUrl: './visitor-checkin.component.scss'
})
export class VisitorCheckinComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  baseUrl: string = environment.baseUrl;

  @Output()
  public pictureTaken = new EventEmitter<WebcamImage>();

  @Input()
  public videoWidth: number = 100;  // Default value

  @Input()
  public videoHeight: number = 150; // Default value
  public webcamImage: WebcamImage | null = null;
  public showCamera: boolean = false;
  public imageHeight: number = 100; // Adjust as needed
  public imageWidth: number = 100;  // Adjust as needed

  constructor(private fb: FormBuilder, private hrmsService: HrmsServiceService,
    private route: Router, private fromQueryParams: ActivatedRoute) {
    const storedEmployeeData = sessionStorage.getItem('userdata');
    this.userData = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
  }

  invitePageForm: any = FormGroup;
  viewDetailsForm: any = FormGroup;

  isFormSubmitted: boolean = false;
  userData: any;
  isSpinner: boolean = false;
  selectedImage: any | string | ArrayBuffer | null = null;
  getQueryParamsdata: any;
  today!: string;
  hours: string[] = [];
  minutes: string[] = [];
  ampm: string[] = ['AM', 'PM'];
  imageSrc: string | ArrayBuffer | null = null;
  fileSizeError: string | null = null;
  selectedFile: File | null = null;
  ImagePath: any;
  patchPhotoUrl: any;
  isPhotoChosen: boolean = false; // Track if a photo has been chosen
  isUploadSuccess: boolean = false;
  isValidPhoto: any;

  ngOnInit(): void {
    this.invitePageForm = this.fb.group({
      contact_name: ['', [Validators.required, Validators.pattern(/^[A-Za-z -]*$/), Validators.minLength(2), Validators.maxLength(30)]],
      designation: ['', [Validators.required]],
      company: ['', [Validators.required]],
      purpose: ['', [Validators.required]],
      // per_email_id: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9+_.-.]+@[a-zA-Z0-9-]+.[a-z]{2,7}')]],
      official_email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9+_.-.]+@[a-zA-Z0-9-]+.[a-z]{2,7}')]],
      mobile_number: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      alter_mobile_number: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      meet_to: ['', [Validators.required]],
      facilities: ['', [Validators.required]],
      date: ['', [Validators.required]],
      // hour: [''],
      // minute: [],
      // ampm: [],
      Time: ['', [Validators.required]]
      
    });
    this.viewDetailsForm = this.fb.group({
      description: [''],
      IdCard: ['', [Validators.required]],
      uploadImage: ['']
    });
    this.retrieveQueryParams();
    this.populateHours();
    this.populateMinutes();
    this.today = this.getCurrentDate();
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
    const hour = this.invitePageForm.get('hour')?.value;
    const minute = this.invitePageForm.get('minute')?.value;
    const ampm = this.invitePageForm.get('ampm')?.value;
    return `${hour}:${minute} ${ampm}`;
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
  retrieveQueryParams() {
    this.fromQueryParams.queryParams.subscribe(res => {
      console.log(res);
      this.getQueryParamsdata = res;
      const photo = this.getQueryParamsdata.Photo;
      this.isValidPhoto = photo !== null && photo !== undefined && photo !== '';
      console.log(this.isValidPhoto);
      const date = this.getQueryParamsdata.Date ? this.parseJsonDate(this.getQueryParamsdata.Date) : null;
      if (this.getQueryParamsdata.VisitorCheckIn === 'true' && this.getQueryParamsdata.VisitorCheckOut === 'false') {
        const formatedDate = this.formatDate(date);
        const getPhotoUrl = this.getQueryParamsdata?.Photo.replace(/\\/g, "\\\\");
        this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;
        this.invitePageForm?.get('contact_name')?.patchValue(res['Name']);
        this.invitePageForm?.get('contact_name').disable();
        this.invitePageForm?.get('designation')?.patchValue(res['Designation']);
        this.invitePageForm?.get('designation').disable();
        this.invitePageForm?.get('company')?.patchValue(res['Company']);
        this.invitePageForm?.get('company').disable();
        this.invitePageForm?.get('purpose')?.patchValue(res['Purpose']);
        this.invitePageForm?.get('purpose').disable();
        // this.invitePageForm?.get('per_email_id')?.patchValue(res['PMail']);
        this.invitePageForm?.get('official_email')?.patchValue(res['OMail']);
        this.invitePageForm?.get('official_email').disable();
        this.invitePageForm?.get('mobile_number')?.patchValue(res['Mobile']);
        this.invitePageForm?.get('mobile_number').disable();
        this.invitePageForm?.get('alter_mobile_number')?.patchValue(res['AMobile']);
        this.invitePageForm?.get('alter_mobile_number').disable();
        this.invitePageForm?.get('facilities')?.patchValue(this.getQueryParamsdata['CompName']);
        this.invitePageForm?.get('facilities').disable();
        this.invitePageForm?.get('meet_to')?.patchValue(this.getQueryParamsdata['WName']);
        this.invitePageForm?.get('meet_to').disable();
        this.invitePageForm?.get('date')?.patchValue(formatedDate);
        this.invitePageForm?.get('date').disable();
        this.invitePageForm?.get('Time').patchValue(res['Time']);
        this.invitePageForm?.get('Time').disable();
        // Handling the time value
        // if (res['Time']) {
        //   const [time, ampm] = res['Time'].split(' ');
        //   const [hours, minutes] = time.split(':');
        //   // Assuming you have form controls named 'hours', 'minutes', and 'ampm'
        //   this.invitePageForm?.get('hour')?.patchValue(hours);
        //   this.invitePageForm?.get('hour').disable();
        //   this.invitePageForm?.get('minute')?.patchValue(minutes);
        //   this.invitePageForm?.get('minute').disable();
        //   this.invitePageForm?.get('ampm')?.patchValue(ampm);
        //   this.invitePageForm?.get('ampm').disable();
        // }
        this.viewDetailsForm?.get('description')?.patchValue(res['Accessories']);
        this.viewDetailsForm?.get('description').disable();
        this.viewDetailsForm?.get('IdCard')?.patchValue(res['IdCard']);
        this.viewDetailsForm?.get('IdCard').disable();
        this.viewDetailsForm.updateValueAndValidity();
        console.log(res['IdCard']);
        
      } else {
        const formatedDate = this.formatDate(date);
        const getPhotoUrl = this.getQueryParamsdata?.Photo.replace(/\\/g, "\\\\");
        this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;

        this.invitePageForm?.get('contact_name')?.patchValue(res['Name']);
        this.invitePageForm?.get('contact_name').disable();
        this.invitePageForm?.get('designation')?.patchValue(res['Designation']);
        this.invitePageForm?.get('company')?.patchValue(res['Company']);
        this.invitePageForm?.get('purpose')?.patchValue(res['Purpose']);
        // this.invitePageForm?.get('per_email_id')?.patchValue(res['PMail']);
        this.invitePageForm?.get('official_email')?.patchValue(res['OMail']);
        this.invitePageForm?.get('mobile_number')?.patchValue(res['Mobile']);
        this.invitePageForm?.get('alter_mobile_number')?.patchValue(res['AMobile']);
        this.invitePageForm?.get('alter_mobile_number')?.patchValue(res['AMobile']);
        this.invitePageForm?.get('facilities')?.patchValue(this.getQueryParamsdata['CompName']);
        this.invitePageForm?.get('facilities').disable();
        this.invitePageForm?.get('meet_to')?.patchValue(this.getQueryParamsdata['WName']);
        this.invitePageForm?.get('meet_to').disable();
        this.invitePageForm?.get('date')?.patchValue(formatedDate);
        this.invitePageForm?.get('date').disable();
        this.invitePageForm?.get('Time').patchValue(res['Time']);
        this.invitePageForm?.get('Time').disable();
        // Handling the time value
        // if (res['Time']) {
        //   const [time, ampm] = res['Time'].split(' ');
        //   const [hours, minutes] = time.split(':');
        //   console.log([hours, minutes]);


        //   // Assuming you have form controls named 'hours', 'minutes', and 'ampm'
        //   this.invitePageForm?.get('hour')?.patchValue(hours);
        //   this.invitePageForm?.get('hour').disable();
        //   this.invitePageForm?.get('minute')?.patchValue(minutes);
        //   this.invitePageForm?.get('minute').disable();
        //   this.invitePageForm?.get('ampm')?.patchValue(ampm);
        //   this.invitePageForm?.get('ampm').disable();
        // }
        this.viewDetailsForm?.get('description')?.patchValue(res['Accessories']);
        this.viewDetailsForm.updateValueAndValidity();
      }

    })
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
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
        this.imageSrc = reader.result as string;
        this.isPhotoChosen = true; // Set flag to true
        this.showCamera = false;   // Disable camera when photo is chosen
      };
      reader.readAsDataURL(file);
      this.selectedFile = file;
    }
    this.uploadImage();
  }

  // Handle image from camera
  handleImage(webcamImage: any) {
    this.webcamImage = webcamImage;
    console.log(this.webcamImage);
    this.isPhotoChosen = false; // Reset flag for photo chosen
    this.showCamera = false;    // Hide camera after image is taken
    this.uploadImage();

  }

  // Toggle camera visibility
  public toggleCamera(): void {
    if (!this.isPhotoChosen) { // Only toggle if no photo has been chosen
      this.showCamera = !this.showCamera;
    }
  }

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
          this.invitePageForm?.get('contact_name').value,
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
  }

  isPhotoPreset() {
    if (!this.isValidPhoto) {
      if (this.ImagePath) {
        return this.ImagePath; 
      } else {
        return ''; 
      }
    } else {
      const getPhotoUrl = this.getQueryParamsdata?.Photo.replace(/\\/g, "\\\\");
      this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;
      return this.patchPhotoUrl; 
    }
  }
  visitorCkeckIn() {
    this.isFormSubmitted = true;
    if (this.viewDetailsForm?.valid) {
      this.isFormSubmitted = false;
      const reqBody = {
        VisitId: this.getQueryParamsdata.VisitId,
        Accessories: this.viewDetailsForm?.get('description')?.value,
        IdCard:  this.viewDetailsForm?.get('IdCard')?.value,
        Photo : this.isPhotoPreset()
      }
      console.log(reqBody);
      
      this.isSpinner = true;
      this.hrmsService.visitorSelfCheckIn(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Updated Successfully', 'success');
          this.route.navigate(['success'],{
            state: { message: 'Visitor Checked-In Successfully!' }
          });
          this.isSpinner = false;
        } else if(res['Message']){
          this.triggerToast(res['Message'], "", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
    } else{
      this.triggerToast('', 'Please Fill The Visitor ID', "warning");
      this.isSpinner = false;
    }
  }
  visitorCkeckOut() {
    const reqBody = {
      VisitId: this.getQueryParamsdata.VisitId,
      Accessories: this.viewDetailsForm?.get('description')?.value,
      IdCard:  this.viewDetailsForm?.get('IdCard')?.value,
    }
    this.isSpinner = true;
    this.hrmsService.visitorSelfCheckOut(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], 'Updated Successfully', 'success');
        this.route.navigate(['success'],{
          state: { message: 'Visitor Checked-Out Successfully!' }
        });
        this.isSpinner = false;
      } else if(res['Message']){
        this.triggerToast(res['Message'], "", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast(error['Message'], 'Internal Server Error', "danger");
      this.isSpinner = false;
    })
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
