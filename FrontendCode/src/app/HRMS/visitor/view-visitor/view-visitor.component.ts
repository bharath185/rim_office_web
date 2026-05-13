import { Component, ElementRef, EventEmitter, HostListener, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { NgbDropdownConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { HrmsServiceService } from '../../hrms-service.service';
import { environment } from 'src/assets/environment';
import { WebcamImage, WebcamModule } from 'ngx-webcam';
import { CameraComponent } from '../../camera/camera.component';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
@Component({
  selector: 'app-view-visitor',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule, CommonModule, ToastMessageComponent, NgxPaginationModule,
    WebcamModule, CameraComponent, RouterModule
  ],
  templateUrl: './view-visitor.component.html',
  styleUrl: './view-visitor.component.scss'
})
export class ViewVisitorComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;
  @ViewChild('inputValue') inputValue!: ElementRef;
  // @ViewChild('pagination') pagination!: ElementRef;
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

  viewDetailsForm: any = FormGroup;
  viewSearchForm: any = FormGroup;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  isTableData: boolean = false;
  errorMessage: any;
  rows: any[] = [];
  viewdata: any;
  isValidPhoto: any;
  page = 1;
  pageSize = 10;
  pageSizes =[10, 50, 100,500];
  formatedDate: any;
  accessPolicy: any;
  getViewAccess: any;
  originalRows: any;
  checkInDate: any;
  checkOutDate: any;
  today = new Date().toISOString().split('T')[0]; // Format the date as YYYY-MM-DD
  minDate: string | undefined;
  maxDate: string | undefined;
  toDateValue: string | undefined;
  isFormSubmitted: boolean = false;
  patchPhotoUrl: any;
  employeeDetails;
  controlAccessPage:any;
  imageSrc: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;
  cameraPicture?: any;
  ImagePath: any;
  isEdited: boolean = false;
  isUploadSuccess: boolean = false;
  isPhotoChosen: boolean = false; // Track if a photo has been chosen
  enableDocFile: boolean = false;
  isSuggestionSelected: boolean = false;
  showUploadCameraCheckOut: boolean = false;
  dropdownVisible = false;
  todayDate: Date = new Date();
  checkOutTime: any
  checkInTime: any

  constructor(private readonly hrmsService: HrmsServiceService,
    private readonly fb: FormBuilder,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // console.log('this.accessPolicy=>', this.accessPolicy);

    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'View Visitor'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log('this.controlAccessPage=>', this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'View Visitor'
      );
    });
  }


  ngOnInit(): void {
    this.viewDetailsForm = this.fb.group({
      description: [],
      IdCard: ['', [Validators.required]],
      uploadImage: ['']
    });
    this.viewSearchForm = this.fb.group({
      date_from: ['',],
      date_to: [''],
      status: ['']
    });
    this.getAllInviteList();
    this.viewSearchForm?.get('date_from')?.valueChanges.subscribe((value: any) => {
      console.log(value);

      if (value) {
        this.viewSearchForm?.get('date_to')?.setValidators([Validators.required]);
      } else {
        this.viewSearchForm?.get('date_to')?.clearValidators();
      }
      this.viewSearchForm?.get('date_to')?.updateValueAndValidity();
    })
  }

  onFromDateChange(event: Event) {
    const fromDate = (event.target as HTMLInputElement).value;
    if (fromDate) {
      this.minDate = fromDate;
    } else {
      this.minDate = undefined;
    }
    this.updateToDateMax();
  }

  onToDateChange(event: Event) {
    const toDate = (event.target as HTMLInputElement).value;
    if (toDate) {
      this.maxDate = toDate;
    } else {
      this.maxDate = undefined;
    }
    this.updateFromDateMin();
  }

  updateFromDateMin() {
    const toDate = this.viewSearchForm.get('date_to')?.value;
    if (toDate) {
      this.minDate = toDate;
    }
  }

  updateToDateMax() {
    const fromDate = this.viewSearchForm.get('date_from')?.value;
    if (fromDate) {
      this.maxDate = fromDate;
    }
  }
  isFromDateInvalid() {
    const control = this.viewSearchForm.get('date_from');
    return (this.isFormSubmitted || control?.touched) && control?.invalid;
  }

  isToDateInvalid() {
    const control = this.viewSearchForm.get('date_to');
    return (this.isFormSubmitted || control?.touched) && control?.invalid;
  }

  preventKeyboardInput(event: KeyboardEvent) {
    event.preventDefault();
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault();
  }

  getAllInviteList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner = true;
    this.hrmsService.visitorGetAllInvite(reqBody).subscribe((res: any) => {
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
          this.isSpinner = false;
        }, 1000);
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

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (!file.type.match(/image\/(jpg|jpeg)/)) {
        alert('Only JPG and JPEG files are allowed.');
        input.value = '';
        return;
      }
      if (file.size > 5 * 1024 * 1024) {
        alert('File size should not exceed 5 MB.');
        input.value = '';
        return;
      }
      const reader = new FileReader();
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        this.isPhotoChosen = true;
        this.showCamera = false;
      };
      reader.readAsDataURL(file);
      this.selectedFile = file;
    }
    this.uploadImage();
  }

  handleImage(webcamImage: any) {
    this.webcamImage = webcamImage;
    console.log(this.webcamImage);
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
        this.viewdata?.Name,
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

  submitForm() {
    this.isFormSubmitted = true;
    if (this.viewSearchForm?.valid) {
      const statusValue = this.viewSearchForm?.get('status')?.value;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        FromDate: this.viewSearchForm?.get('date_from')?.value ? this.viewSearchForm?.get('date_from')?.value : '',
        ToDate: this.viewSearchForm?.get('date_to')?.value ? this.viewSearchForm?.get('date_to')?.value : '',
        Status: this.viewSearchForm?.get('status')?.value || "",
      };
      this.isSpinner = true;
      this.hrmsService.VisitorFilter(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.rows = res;
          this.isTableData = false;
          this.rows = res.map((date: any) => ({
            ...date,
            Date: this.formatDate(this.parseJsonDate(date.Date)),
            CheckIn: this.formatDate(this.parseJsonDate(date.CheckIn)),
            CheckOut: this.formatDate(this.parseJsonDate(date.CheckOut))
          }));
          this.isSpinner = false;
          this.page = 1;
        } else {
          this.errorMessage = "No records found";
          this.rows = [];
          this.isTableData = true;
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Please Try Again', "danger");
        this.isSpinner = false;
      })
    }
  }
  resetForm() {
    this.viewSearchForm?.get('date_from')?.setValue(null);
    this.viewSearchForm?.get('date_to')?.setValue(null);
    this.minDate = undefined;
    this.maxDate = undefined;
    this.viewSearchForm?.reset();
    this.viewSearchForm?.updateValueAndValidity();
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
  onView(data: any) {
    console.log(data);
    this.viewdata = data;
    const photo = this.viewdata.Photo;
    this.isValidPhoto = photo !== null && photo !== undefined && photo !== '';
    const hasValidCheckIn = this.viewdata?.CheckIn !== '' && this.viewdata?.CheckIn !== null;
    const hasValidVisitorCheckIn = this.viewdata?.VisitorCheckIn !== '' && this.viewdata?.VisitorCheckIn !== null;
    // Combine the conditions
    if ((hasValidCheckIn || hasValidVisitorCheckIn)) {
      this.viewDetailsForm?.get('description')?.patchValue(this.viewdata?.Accessories);
      this.viewDetailsForm?.get('description')?.disable();
    }
    else {
      this.viewDetailsForm?.get('description')?.enable();
    }
    if ((this.viewdata?.IdCard !== '' && this.viewdata?.IdCard !== null)) {
      this.viewDetailsForm?.get('IdCard')?.patchValue(this.viewdata?.IdCard);
      this.viewDetailsForm?.updateValueAndValidity();
    } else {
      this.viewDetailsForm?.get('IdCard')?.enable();
      this.viewDetailsForm?.get('IdCard').reset();
      this.viewDetailsForm.updateValueAndValidity();
    }
    const getPhotoUrl = this.viewdata?.Photo.replace(/\\/g, "\\\\");
    this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;

  }

  closeModalResetData() {
    this.viewDetailsForm.reset();
    this.isFormSubmitted = false;
    this.isUploadSuccess = false;
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.imageSrc = '';
    this.selectedFile = null;
    this.ImagePath = '';
    this.isEdited = false;
    this.showCamera = false;
    this.webcamImage = null;
    this.isPhotoChosen = false;
  }

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
      return ` ${hours}:${minutes}`;
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

  isCheckInCompare(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    const hours = date.getHours();
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const ampm = hours >= 12 ? 'PM' : 'AM';
    const formattedHours = ((hours % 12) || 12).toString().padStart(2, '0');
    // return `${day}-${month}-${year} ${formattedHours}:${minutes} ${ampm}`;
    return `${year}-${month}-${day}`
  }
  shouldShowCheckIn() {
    const formattedDate = this.isCheckInCompare(this.todayDate);
    return this.viewdata?.Accept === true &&
      this.viewdata?.Approved === false &&
      this.viewdata?.CheckIn === '' &&
      this.viewdata?.CheckOut === '' &&
      formattedDate === this.viewdata?.Date;
  }

  isPhotoPreset() {
    if (!this.isValidPhoto) {
      if (this.ImagePath) {
        return this.ImagePath;
      } else {
        return '';
      }
    } else {
      const getPhotoUrl = this.viewdata?.Photo.replace(/\\/g, "\\\\");
      this.patchPhotoUrl = `${this.baseUrl}/${getPhotoUrl}`;
      return this.patchPhotoUrl;
    }
  }
  checkIn() {
    this.isFormSubmitted = true;
    if (this.viewDetailsForm.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        VisitId: this.viewdata.VisitId,
        Accessories: this.viewDetailsForm?.get('description')?.value,
        IdCard: this.viewDetailsForm?.get('IdCard')?.value,
        Photo: this.isPhotoPreset()
      }
      console.log(reqBody);
      this.isSpinner1 = true;
      this.hrmsService.visitorCheckIn(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
          this.isSpinner1 = false;
          this.closeModal.nativeElement.click();
          this.getAllInviteList();
          this.showUploadCameraCheckOut = true;
        } else if (res['Message']) {
          this.triggerToast(res['Message'], "", "warning");
          this.isSpinner1 = false;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner1 = false;
      })
    } else {
      this.triggerToast('', 'Please Fill The ID Card Number', 'warning');
      this.isSpinner1 = false;
    }
  }

  checkOut() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      VisitId: this.viewdata.VisitId,
      Accessories: this.viewDetailsForm?.get('description')?.value,
      IdCard: this.viewDetailsForm?.get('IdCard')?.value,
    }
    this.isSpinner1 = true;
    this.hrmsService.visitorCheckOut(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], 'Updated Successfully', 'success');
        this.isSpinner1 = false;
        this.closeModal.nativeElement.click();
        this.getAllInviteList();
      } else if (res['Message']) {
        this.triggerToast(res['Message'], "", "warning");
        this.isSpinner1 = false;
      }
    }, error => {
      this.triggerToast(error['Message'], 'Internal Server Error', "danger");
      this.isSpinner1 = false;
    })
  }

  pageChange(event: any) {
    this.page = event
  }

  toggleDropdown() {
    this.dropdownVisible = !this.dropdownVisible;
  }
  // Listen for clicks anywhere in the document
  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const isDropdown = target.closest('.dropdown-content') !== null;
    const isButton = target.matches('.export-button');
    if (!isDropdown && !isButton) {
      this.dropdownVisible = false;
    }
  }
  exportFile(format: string) {
    if (this.viewSearchForm?.valid) {
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        FromDate: this.viewSearchForm?.get('date_from')?.value || '',
        ToDate: this.viewSearchForm?.get('date_to')?.value || '',
        Status: this.viewSearchForm?.get('status')?.value || '',
      };
      this.isSpinner = true; // Start spinner
      if (format === 'csv') {
        this.hrmsService.VisitorVisitExportCSV(reqBody).subscribe((res: Blob) => {
          this.isSpinner = false;
          const url = window.URL.createObjectURL(res);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'export.csv';
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
        }, error => {
          this.isSpinner = false;
          console.error('Download error', error);
        });
      } else if (format === 'excel') {
        this.hrmsService.VisitorVisitExportExcel(reqBody).subscribe((res: Blob) => {
          this.isSpinner = false;
          const url = window.URL.createObjectURL(res);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'export.xlsx';
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
        }, error => {
          this.isSpinner = false;
          console.error('Download error', error);
        });
      }
    }
    else {
      this.isSpinner = false;
      this.triggerToast('', 'Please Choose The Required Date For Export data', "danger");
    }
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


}
