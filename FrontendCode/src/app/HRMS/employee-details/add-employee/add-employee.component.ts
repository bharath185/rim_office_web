import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { EmployeeModuleService } from '../../service/employee.service';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/assets/environment';
import { trigger, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [CommonModule, SharedModule, ReactiveFormsModule, ToastMessageComponent],
  animations: [
    trigger('slideToggle', [
      transition(':enter', [
        style({ height: '0', opacity: 0 }),
        animate('0.5s ease', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('0.5s ease', style({ height: '0', opacity: 0 }))
      ])
    ])
  ],
  templateUrl: './add-employee.component.html',
  styleUrl: './add-employee.component.scss'
})
export class AddEmployeeComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  baseUrl: string = environment.baseUrl;
  isSpinner: boolean = false;
  userData;
  employeeDetails;
  isFormSubmitted: boolean = false;
  errorMessage: any;
  today = new Date().toISOString().split('T')[0]; // Format the date as YYYY-MM-DD
  minDate: string | undefined;
  maxDate: string | undefined;
  toDateValue: string | undefined;
  isUploadSuccess: boolean = false;
  storeQueryParamsData: any
  constructor(private fb: FormBuilder, private hrmsService: EmployeeModuleService,
    private fromQueryParams: ActivatedRoute, private hrmsServiceMain: HrmsServiceService,
    private cdr: ChangeDetectorRef
  ) {
    const storedEmployeeData = sessionStorage.getItem('userdata');
    this.userData = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    console.log('login User', this.userData);

    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    console.log('Employee Details', this.employeeDetails);

    const todayDate = new Date();
    this.today = todayDate.toISOString().split('T')[0];
    this.minDate = new Date(todayDate.getFullYear() - 100, 0, 1).toISOString().split('T')[0];
  }
  isOpen = [true, false, false, false, false, false];
  togglePanel(index: number): void {
    this.isOpen[index] = !this.isOpen[index];
  }
  onEditClick(event: Event): void {
    event.stopPropagation();
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
  // ***************Basic details Top variable Valus starts************************
  basicDetailsForm: any = FormGroup;
  basicDetailsUploadedImg: string | ArrayBuffer | null = null;
  basicDetailsSelectedFile: File | null = null;
  getDDCompany: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getSalutationList: any[] = [];
  getGenderList: any[] = [];
  getDepartementName = [];
  getDepartementRole: any[] = [];
  getApproverList: any[] = [];
  getEmployeeTypeList: any[] = [];
  getDeptDataID: any;
  getDeptDataName: any;
  getDesignationID: any;
  getDesignationName: any;
  isContractEnd: boolean = false;
  storeAddedEmployeeDetails: any;
  isEmpIdCreated: boolean = false;
  getProfilePhoto: any;
  isProfilePhotoUploded: boolean = false;
  getEntityName: any;
  getEmployeeType: any;
  // ***************Basic details Top variable Valus Ends************************

  // ***************Contact Information Top variable Valus starts************************
  contactInfoForm: any = FormGroup;
  // ***************Contact Information Top variable Valus starts************************

  // ***************Career Details Top variable Valus starts************************
  careerForm: any = FormGroup;
  // Variables for Offer Letter
  offerLetterSrc: string | ArrayBuffer | null = null;
  offerLetterPath: string | null = null;
  offerLetterName: string | null = null;
  isOfferLetterImage = false;
  offerLetterSelectedFile: File | null = null;
  getOfferLetter: any
  patchOfferLatter: any

  // Variables for Salary Letter
  salaryLetterSrc: string | ArrayBuffer | null = null;
  salaryLetterPath: string | null = null;
  salaryLetterName: string | null = null;
  isSalaryLetterImage = false;
  salaryLetterSelectedFile: File | null = null;
  getSalaryLetter: any
  patchExperienceLetter: any;

  // Variables for Experience/Relieving Letter
  experienceLetterSrc: string | ArrayBuffer | null = null;
  experienceLetterPath: string | null = null;
  experienceLetterName: string | null = null;
  isExperienceLetterImage = true;
  experienceLetterSelectedFile: File | null = null;
  getExperienceLetter: any
  patchSalaryLetter: any

  getEmpCareerDetailsRows: any[] = [];
  isCareerUpdateButton: boolean = false;
  patchCareerData: any;
  getCareerTableDeleteId: any
  showErrorCareer = false;
  errorMessageCareer: string = '';

  monthHeaders: string[] = [];
  dynamicHeaders: any = {};
  minDateCareer: string | undefined;
  maxDateCareer: string | undefined;
  isOfferLatterUploaded = false;
  isSalaryLatterUploaded: boolean = false;
  isExperienceLatterUploaded: boolean = false;

  previewUrls: string[] = []; // To hold preview URLs for images
  fileNames: string[] = []; // To store file names
  uploadStatus: string[] = []; // To hold status messages for each upload
  fileUploads: { [key: string]: string } = {}; // To store file paths/identifiers
  maxSets = 3; // Maximum number of form groups allowed
  isValidPhotoSalary: any;
  isValidPhotoOffer: any;
  isValidPhotoExperience: any;
  // ***************Career Details Top variable Valus Ends************************

  // ***************Education Details Top variable Valus Starts************************
  isShowEducOthers: boolean = false
  EducationForm: any = FormGroup;
  educSelectedFile: File | null = null;
  isEducUpdateButton: boolean = false;
  educationDocName: any;
  imageSrcEducion: string | ArrayBuffer | null = null;
  educImagePath: any;
  educationDetailsRows: any[] = [];
  imageSrcEducDoc: string | ArrayBuffer | null = null;
  filePathEducDoc: string | null = null;
  fileNameEducDoc: string | null = null;
  isImageEducDoc = false;
  SelectedFileEducDoc: File | null = null;
  getEducDoc: any;
  patchEducDoc: any
  educselectedID: any;
  educselectedName: any;
  pathchEducationData: any;
  patchEducPath: any;
  getEducTableDeleteId: any;
  minDateEducation: string | undefined;
  maxDateEducation: string | undefined;
  isEducationUploaded: boolean = false
  // ***************Education Details Top variable Valus Ends************************

  // ***************Account Details Top variable Valus Starts************************
  accountForm: any = FormGroup;
  // ***************Account Details Top variable Valus Ends************************

  // ***************Government Details Top variable Valus Starts************************
  governmentForm: any = FormGroup;
  // imageSrcGovt: string | ArrayBuffer | null = null;
  // govtSelectedFile: File | null = null;
  // govImagePath: any;
  govtDocName: any;
  govtDetailsRows: any[] = [];
  imageSrcGovtDoc: string | ArrayBuffer | null = null;
  filePathGovtDoc: string | null = null;
  fileNameGovtDoc: string | null = null;
  isImageGovtDoc = false;
  SelectedFileGovtDoc: File | null = null;
  getGovtDoc: any;
  govtselectedID: any;
  govtselectedName: any;
  patchGovtData: any;
  patchGovtPath: any;
  showErrorGovt = false; // To control the display of error message
  errorMessageGovt: string = ''; // To store error or no data message
  isGovtUpdateButton: boolean = false;
  isViewGovtOthers: boolean = false;
  getGovtTableDeleteId: any;
  minDateGovt: string | undefined;
  maxDateGovt: string | undefined;
  isGovtUploaded: boolean = false;
  isGovtVisibleDates: boolean = false;
  // ***************Government Details Top variable Valus Ends************************
  uploadDocumentinputFile: any;
  ngOnInit(): void {
    this.basicdetailsFormval();
    this.contactInformationFormVal();
    this.careerFormVal();
    this.educationFormval();
    this.accountFormVal();
    this.governmentFormval();
    // this.retryQueryParams();
  }
  // ***************Basic details .ts Starts************************
  basicdetailsFormval() {
    this.basicDetailsForm = this.fb.group({
      Company: ['', [Validators.required]],
      LegalEntity: ['', [Validators.required]],
      BusinessUnit: [''],
      Location: [''],
      DeptName: ['', [Validators.required]],
      Designation: ['', [Validators.required]],
      EmpCode: ['', [Validators.required]],
      basic_salutation: [],
      FirstName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      MiddleName: ['', [Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      LastName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(1), Validators.maxLength(50)]],
      DOB: ['', [Validators.required]],
      MobileNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      EmailId: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      // EmailId: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')]],
      BloodGroup: ['', [Validators.required]],
      MaritalStatus: ['', [Validators.required]],
      Gender: ['', [Validators.required]],
      JoiningDate: ['', [Validators.required]],
      basic_photo: [],
      employeeType: [''],
      approverPerson: [''],
      contractEndDate: [''],
    }, { validator: this.dateComparisonValidator() });
    this.employee_DD_Employee();
    this.access_DD_department();
    this.getDDSalutationList();
    this.getDDGenderList();
    this.getDDEmpTypeList();

    this.basicDetailsForm?.get('Company').valueChanges.subscribe((val: any) => {
      this.getBusinessUnitlist = [];
      this.getLocations = [];
    });
  }
  isEnableBusiness(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    // this.getDeptDataID = selectElement.value;
    this.getEntityName = selectElement.options[selectElement.selectedIndex].text;
    console.log(this.getEntityName)
    if (this.getEntityName === 'RIM India Pvt Ltd') {
      this.basicDetailsForm?.get('BusinessUnit').disable();
      this.basicDetailsForm?.get('Location').disable();
    } else {
      this.basicDetailsForm?.get('BusinessUnit').enable();
      this.basicDetailsForm?.get('Location').enable();
      this.basicDetailsForm.get('BusinessUnit')?.updateValueAndValidity();
      this.basicDetailsForm.get('Location')?.updateValueAndValidity();
    }
  }
  getEmployeeTypeValue(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    this.getEmployeeType = selectElement.options[selectElement.selectedIndex].text;
    if (this.getEmployeeType === 'Contract') {
      this.isContractEnd = true;
      this.basicDetailsForm?.get('contractEndDate').setValidators([Validators.required]);
    } else {
      this.isContractEnd = false;
      this.basicDetailsForm.get('contractEndDate').clearValidators();
      this.basicDetailsForm.get('contractEndDate')?.updateValueAndValidity();
    }
  }
  // Custom validator function // Custom validator function
  dateComparisonValidator(): ValidatorFn {
    return (formGroup: AbstractControl): { [key: string]: any } | null => {
      const form = formGroup as FormGroup;
      const dobControl = form.get('DOB');
      const joiningDateControl = form.get('JoiningDate');

      if (dobControl && joiningDateControl) {
        const dob = new Date(dobControl.value);
        const joiningDate = new Date(joiningDateControl.value);

        if (joiningDate < dob) {
          return { 'dateComparison': true };
        }
      }
      return null;
    };
  }
  calllegalEntity(event: any) {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: Number(this.basicDetailsForm?.get('Company').value)
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
      console.log(res);
      setTimeout(() => {
        this.basicDetailsForm?.get('LegalEntity').reset();
        this.basicDetailsForm?.get('BusinessUnit').reset();
        this.basicDetailsForm?.get('Location').reset();
      }, 100);
      if (res) {
        this.getLegalEntity = res;
        this.isSpinner = false;

      } else {
        this.triggerToast(res['Message'], "No Data Found For Legal Entity", "warning");
        this.isSpinner = false;
        this.getLegalEntity = []
      }
    },
      error => {
        this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
        this.isSpinner = false;
      })
  }
  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: Number(this.basicDetailsForm?.get('Company').value),
      LEId: Number(this.basicDetailsForm?.get('LegalEntity').value),
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
      console.log(res);
      if (res.length >= 1) {
        this.basicDetailsForm?.get('BusinessUnit').reset();
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
        this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
        this.isSpinner = false;
      })
  }
  callApproverDataEntity(event: any) {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: this.basicDetailsForm?.get('Company').value,
      LEId: this.basicDetailsForm?.get('LegalEntity').value,
      BUId: 0,
      LocationId: 0,
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDApprover(reqBody).subscribe((res: any) => {
      console.log(res);
      if (res.length >= 1) {
        this.getApproverList = res;
        this.isSpinner = false;
      } else {
        // this.triggerToast(res['Message'], "No Data Found For Approver Person", "warning");
        this.isSpinner = false;
        this.getApproverList = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Approver', "danger");
        this.isSpinner = false;
      })
  }
  callLocation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: Number(this.basicDetailsForm?.get('Company').value),
      LEId: Number(this.basicDetailsForm?.get('LegalEntity').value),
      BUId: Number(this.basicDetailsForm?.get('BusinessUnit').value),
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDLocation(reqBody).subscribe((res: any) => {
      console.log(res);
      if (res.length >= 1) {
        this.basicDetailsForm?.get('Location').reset();
        this.getLocations = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Location", "warning");
        this.isSpinner = false;
        this.getLocations = []
      }
    },
      error => {
        this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
        this.isSpinner = false;
      })
  }
  callApproverDataLocation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: this.basicDetailsForm?.get('Company').value,
      LEId: this.basicDetailsForm?.get('LegalEntity').value,
      BUId: this.basicDetailsForm?.get('BusinessUnit').value,
      LocationId: this.basicDetailsForm?.get('Location').value,
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDApprover(reqBody).subscribe((res: any) => {
      console.log(res);
      if (res.length >= 1) {
        this.getApproverList = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Approver ", "warning");
        this.isSpinner = false;
        this.getApproverList = []
      }
    },
      error => {
        this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. Approvers', "danger");
        this.isSpinner = false;
      })
  }
  callDDDesignation(event: any) {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.basicDetailsForm?.get('DeptName')?.value,
    }
    console.log(reqBody);
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe((res: any) => {

      this.getDepartementRole = res;
    }, error => {
      this.triggerToast('Internal Server Error', 'Error loading Designation', "danger");
      this.isSpinner = false;
    })
  }
  getDesignationData(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    this.getDesignationID = selectElement.value;
    this.getDesignationName = selectElement.options[selectElement.selectedIndex].text;
  }
  getDeptData(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    this.getDeptDataID = selectElement.value;
    this.getDeptDataName = selectElement.options[selectElement.selectedIndex].text;
  }
  employee_DD_Employee() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDCompany(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDCompany = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        this.isSpinner = false;
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Company Name', "danger");
        this.isSpinner = false;
      })
  }
  getDDSalutationList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDSalutation(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getSalutationList = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Select List", "warning");
        this.isSpinner = false;
        this.getSalutationList = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Select List', "danger");
        this.isSpinner = false;
      })
  }
  getDDGenderList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDGender(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getGenderList = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Gender List", "warning");
        this.isSpinner = false;
        this.getGenderList = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Gender List', "danger");
        this.isSpinner = false;
      })
  }
  getDDEmpTypeList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDEmpType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getEmployeeTypeList = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Employee Type List", "warning");
        this.isSpinner = false;
        this.getEmployeeTypeList = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'To Load Employee Type List', "danger");
        this.isSpinner = false;
      })
  }
  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner = true;
    this.hrmsServiceMain.access_DD_department(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDepartementName = res;
        this.isSpinner = false;
      } else {
        this.triggerToast('', 'Record Not Found', 'Warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Department List', 'danger');
      this.isSpinner = false;
    })
  }
  triggerFileUpload() {
    const fileInput = document.getElementById('basicDetailsImage') as HTMLInputElement;
    fileInput.click();
  }
  basicDetailsImage(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (!file) {
        alert('No file selected.');
        return;
      }
      console.log(file.type);
      // Check for MIME type (allow both jpg and jpeg)
      if (!file.type.match(/image\/(jpg|jpeg)/)) {
        alert('Only JPG files are allowed.'); // Alert for only JPG
        input.value = '';
        return;
      }
      // Check file size
      if (file.size > 5 * 1024 * 1024) { // 5 MB
        alert('File size should not exceed 5 MB.');
        input.value = '';
        return;
      }
      const reader = new FileReader();
      reader.onload = () => {
        this.basicDetailsUploadedImg = reader.result;
        this.cdr.detectChanges(); // Ensure the changes are detected
      };
      reader.readAsDataURL(file);
      this.basicDetailsSelectedFile = file;
      this.uploadBasicDetailPhoto();
    }
  }
  uploadBasicDetailPhoto() {
    if (!this.basicDetailsSelectedFile) {
      alert('No file selected.');
      return;
    }
    this.isSpinner = true
    this.hrmsService.employeeUploadImage(this.employeeDetails[0].EmpId, this.basicDetailsSelectedFile).subscribe((res: any) => {
      if (res['msg']) {
        console.log(res);
        this.getProfilePhoto = res.path;
        this.isProfilePhotoUploded = true
        this.isSpinner = false;
        this.triggerToast(res['msg'], 'Profile Picture Uploaded', "success");
        // this.isUploadSuccess = true;
      } else if (res['Message']) {
        this.isSpinner = false;
        this.triggerToast(res['Message'], res['Message'], "warning");
        this.basicDetailsUploadedImg = '';
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Upload Profile Picture', "danger");
      this.isSpinner = false;
    })
  }
  addBasicDetails() {
    this.isFormSubmitted = true;
    if (this.basicDetailsForm.valid) {
      const reqBody = {
        // EmpId: this.employeeDetails[0].EmpId,  approverPerson
        LoginId: this.employeeDetails[0].LoginId,
        Password: this.employeeDetails[0].Password ? this.employeeDetails[0].Password : '',
        ReportId: Number(this.basicDetailsForm?.get('approverPerson').value ? this.basicDetailsForm?.get('approverPerson').value : ''), // this is for Approver Person
        CompId: Number(this.basicDetailsForm?.get('Company').value),
        LEId: Number(this.basicDetailsForm?.get('LegalEntity').value),
        BUId: Number(this.basicDetailsForm?.get('BusinessUnit').value),
        LocationId: Number(this.basicDetailsForm?.get('Location').value),
        DeptName: this.getDeptDataName,
        DeptId: Number(this.getDeptDataID ? this.getDeptDataID : 0),
        Designation: this.getDesignationName,
        DesignationId: Number(this.getDesignationID ? this.getDesignationID : 0),
        EmpCode: this.basicDetailsForm?.get('EmpCode').value ? this.basicDetailsForm?.get('EmpCode').value : '',
        SalutationId: Number(this.basicDetailsForm?.get('basic_salutation').value ? this.basicDetailsForm?.get('basic_salutation').value : ''),
        FirstName: this.basicDetailsForm?.get('FirstName').value ? this.basicDetailsForm?.get('FirstName').value : '',
        MiddleName: this.basicDetailsForm?.get('MiddleName').value ? this.basicDetailsForm?.get('MiddleName').value : '',
        LastName: this.basicDetailsForm?.get('LastName').value ? this.basicDetailsForm?.get('LastName').value : '',
        DOB: this.basicDetailsForm?.get('DOB').value ? this.basicDetailsForm?.get('DOB').value : '',
        MobileNo: this.basicDetailsForm?.get('MobileNo').value ? this.basicDetailsForm?.get('MobileNo').value : '',
        EmailId: this.basicDetailsForm?.get('EmailId').value ? this.basicDetailsForm?.get('EmailId').value : '',
        BloodGroup: this.basicDetailsForm?.get('BloodGroup').value ? this.basicDetailsForm?.get('BloodGroup').value : '',
        MaritalStatus: this.basicDetailsForm?.get('MaritalStatus').value ? this.basicDetailsForm?.get('MaritalStatus').value : '',
        Gender: this.basicDetailsForm?.get('Gender').value ? this.basicDetailsForm?.get('Gender').value : '',
        JoiningDate: this.basicDetailsForm?.get('JoiningDate').value ? this.basicDetailsForm?.get('JoiningDate').value : '',
        EmpTypeId: Number(this.basicDetailsForm?.get('employeeType').value ? this.basicDetailsForm?.get('employeeType').value : ''),
        CEndDate: this.basicDetailsForm?.get('contractEndDate').value ? this.basicDetailsForm?.get('contractEndDate').value : '',
        Photo: this.getProfilePhoto ? this.getProfilePhoto : ''
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeAddEmployee(reqBody).subscribe((res: any) => {
        if (res['msg'] === "Added") {
          this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
          this.isSpinner = false;
          this.storeAddedEmployeeDetails = res;
          this.isEmpIdCreated = true;
          this.isFormSubmitted = false;
        } else if ((res["Message"])) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
          this.isSpinner = false;
          // this.isEmpIdCreated = false;
        }
      }, error => {
        this.triggerToast(error['msg'], 'Internal Server Error', "danger");
        this.isSpinner = false;
        // this.isEmpIdCreated = false;
      })
    } else {
      this.triggerToast("Invalid", "Please Fill All Datas", "danger");
      this.isSpinner = false;
      // this.isEmpIdCreated = false;
    }
  }
  resetBasicDetials() {
    this.basicDetailsForm?.reset();
  }
  // ***************Basic details .ts Finished************************

  // ***************Contact Information .ts Starts************************
  contactInformationFormVal() {
    this.contactInfoForm = this.fb.group({
      AMobileNo: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      PMailId: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      // PMailId: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')]],
      FatherName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      MotherName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      HusbandName: ['', [Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      FContactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      MContactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      HContactNo: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      date_of_anniversary: [''],
      EContactName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      EContactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      EContactRelationship: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      Cast: [''],
      Religion: [''],
      Country: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Nationality: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Height: ['', [Validators.pattern('^[0-9]{2,3}$')]],
      Weight: ['', [Validators.pattern('^[0-9]{2,3}$')]],
      Disability: ['', [Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      TotalExperience: [''],
      RelevantExperience: [''],
      ECActivities: [''],
      Sports: [''],
      Per_Door_Number: [''],
      Per_Building: [''],
      Per_Street: [''],
      Per_Location: [''],
      Per_City: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Per_State: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Per_Country: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Per_PinCode: ['', [Validators.pattern('^[0-9]{5,6}$')]],
      Curr_Door_Number: [''],
      Curr_Building: [''],
      Curr_Street: [''],
      Curr_Location: [''],
      Curr_City: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Curr_State: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Curr_Country: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      Curr_PinCode: ['', [Validators.pattern('^[0-9]{5,6}$')]],
    });
  }
  addContactDetails() {
    this.isFormSubmitted = true;
    console.log(this.contactInfoForm.valid);
    console.log(this.contactInfoForm.value);
    if (this.contactInfoForm.valid) {
      const reqBody = {
        EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
        LoginId: this.employeeDetails[0].LoginId,
        AMobileNo: this.contactInfoForm?.get('AMobileNo').value ? this.contactInfoForm?.get('AMobileNo').value : '',
        PMailId: this.contactInfoForm?.get('PMailId').value ? this.contactInfoForm?.get('PMailId').value : '',
        FatherName: this.contactInfoForm?.get('FatherName').value ? this.contactInfoForm?.get('FatherName').value : '',
        MotherName: this.contactInfoForm?.get('MotherName').value ? this.contactInfoForm?.get('MotherName').value : '',
        HusbandName: this.contactInfoForm?.get('HusbandName').value ? this.contactInfoForm?.get('HusbandName').value : '',
        FContactNo: this.contactInfoForm?.get('FContactNo').value ? this.contactInfoForm?.get('FContactNo').value : '',
        DateOfAnniversary: this.contactInfoForm?.get('date_of_anniversary').value ? this.contactInfoForm?.get('date_of_anniversary').value : '',
        MContactNo: this.contactInfoForm?.get('MContactNo').value ? this.contactInfoForm?.get('MContactNo').value : '',
        HContactNo: this.contactInfoForm?.get('HContactNo').value ? this.contactInfoForm?.get('HContactNo').value : '',
        EContactName: this.contactInfoForm?.get('EContactName').value ? this.contactInfoForm?.get('EContactName').value : '',
        EContactNo: this.contactInfoForm?.get('EContactNo').value ? this.contactInfoForm?.get('EContactNo').value : '',
        EContactRelationship: this.contactInfoForm?.get('EContactRelationship').value ? this.contactInfoForm?.get('EContactRelationship').value : '',
        Caste: this.contactInfoForm?.get('Cast').value ? this.contactInfoForm?.get('Cast').value : '',
        Region: this.contactInfoForm?.get('Religion').value ? this.contactInfoForm?.get('Religion').value : '',
        Country: this.contactInfoForm?.get('Country').value ? this.contactInfoForm?.get('Country').value : '',
        Nationality: this.contactInfoForm?.get('Nationality').value ? this.contactInfoForm?.get('Nationality').value : '',
        Height: Number(this.contactInfoForm?.get('Height').value) ? Number(this.contactInfoForm?.get('Height').value) : '',
        Weight: Number(this.contactInfoForm?.get('Weight').value) ? Number(this.contactInfoForm?.get('Weight').value) : '',
        Disability: this.contactInfoForm?.get('Disability').value ? this.contactInfoForm?.get('Disability').value : '',
        TotalExperience: this.contactInfoForm?.get('TotalExperience').value ? this.contactInfoForm?.get('TotalExperience').value : '',
        RelevantExperience: this.contactInfoForm?.get('RelevantExperience').value ? this.contactInfoForm?.get('RelevantExperience').value : '',
        ECActivities: this.contactInfoForm?.get('ECActivities').value ? this.contactInfoForm?.get('ECActivities').value : '',
        Sports: this.contactInfoForm?.get('Sports').value ? this.contactInfoForm?.get('Sports').value : '',
        PermanentBuildingName: this.contactInfoForm?.get('Per_Building').value ? this.contactInfoForm?.get('Per_Building').value : '',
        PermanentDoorNumber: this.contactInfoForm?.get('Per_Door_Number').value ? this.contactInfoForm?.get('Per_Door_Number').value : '',
        PermanentStreet: this.contactInfoForm?.get('Per_Street').value ? this.contactInfoForm?.get('Per_Street').value : '',
        PermanentCity: this.contactInfoForm?.get('Per_City').value ? this.contactInfoForm?.get('Per_City').value : '',
        PermanentLocation: this.contactInfoForm?.get('Per_Location').value ? this.contactInfoForm?.get('Per_Location').value : '',
        PermanentState: this.contactInfoForm?.get('Per_State').value ? this.contactInfoForm?.get('Per_State').value : '',
        PermanentCountry: this.contactInfoForm?.get('Per_Country').value ? this.contactInfoForm?.get('Per_Country').value : '',
        PermanentPinCode: this.contactInfoForm?.get('Per_PinCode').value ? this.contactInfoForm?.get('Per_PinCode').value : '',
        CurrentBuildingName: this.contactInfoForm?.get('Curr_Building').value ? this.contactInfoForm?.get('Curr_Building').value : '',
        CurrentDoorNumber: this.contactInfoForm?.get('Curr_Door_Number').value ? this.contactInfoForm?.get('Curr_Door_Number').value : '',
        CurrentStreet: this.contactInfoForm?.get('Curr_Street').value ? this.contactInfoForm?.get('Curr_Street').value : '',
        CurrentCity: this.contactInfoForm?.get('Curr_City').value ? this.contactInfoForm?.get('Curr_City').value : '',
        CurrentLocation: this.contactInfoForm?.get('Curr_Location').value ? this.contactInfoForm?.get('Curr_Location').value : '',
        CurrentState: this.contactInfoForm?.get('Curr_State').value ? this.contactInfoForm?.get('Curr_State').value : '',
        CurrentCountry: this.contactInfoForm?.get('Curr_Country').value ? this.contactInfoForm?.get('Curr_Country').value : '',
        CurrentPinCode: this.contactInfoForm?.get('Curr_PinCode').value ? this.contactInfoForm?.get('Curr_PinCode').value : '',
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeAddContactDetails(reqBody).subscribe((res: any) => {
        console.log(res);
        if (res['msg'] === "Added") {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.isFormSubmitted = false;
        } else {
          this.triggerToast(res['Message'], "", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error['Message'], 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
    }
  }
  resetContactDetails() {
    this.contactInfoForm?.reset();
  }
  // ***************Contact Information .ts Finished************************

  // ***************Career Details .ts Starts************************
  onEditCareerDetails(event: any) {
    event.stopPropagation();
  }
  monthsCareer: string[] = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];
  careerFormVal() {
    this.careerForm = this.fb.group({
      careerCompanyName: ['', [Validators.required]],
      career_Designation: ['', [Validators.required, Validators.pattern("^[a-zA-Z ]*$")]],
      date_from: ['', [Validators.required]],
      date_to: ['', [Validators.required]],
      career_Experience: [''],
      career_HR_ManagerName: ['', [Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      career_HR_ManagerDesignation: ['', [Validators.pattern("^[a-zA-Z ]*$")]],
      career_HR_ManagerEmail: ['', [Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      career_HR_ManagerMob: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      career_CTC: [''],
      career_Reason: [''],
      OfferLetterMonth: [],
      OfferLetter: [''],
      SalaryLetterMonth: [],
      SalaryLetter: [],
      ExperienceLetterMonth: [],
      ExperienceLetter: [''],
      sets: this.fb.array([this.createSet()]) // Initialize with one set
    }, { validators: this.dateRangeValidator });
  }
  // Custom validator to ensure date_to is after date_from
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }
  onFromDateCareer(): void {
    if (this.careerForm.get('date_from')?.value) {
      this.minDateCareer = this.careerForm.get('date_from')?.value;
    }
  }
  onToDateCareer(): void {
    if (this.careerForm.get('date_to')?.value) {
      this.maxDateCareer = this.careerForm.get('date_to')?.value;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.careerForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.careerForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.careerForm.hasError('dateRange');
  }

  offerLetterUpload(event: any) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0] ?? null;
    if (file) {
      const maxSize = 5 * 1024 * 1024;
      if (file.size > maxSize) {
        this.triggerToast('', 'File Size Must Be Less Than 5MB', 'warning');
        return;
      }
      if (file.type.startsWith('image/')) {
        this.isOfferLetterImage = true;
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.offerLetterSrc = e.target.result;
        };
        reader.readAsDataURL(file);
        this.offerLetterSelectedFile = file;
        this.offerLetterPath = null;
        this.offerLetterName = null;
      } else if (file.type === 'application/pdf') {
        this.offerLetterName = file.name;
        this.offerLetterPath = URL.createObjectURL(file);
        this.offerLetterSrc = null;
        this.offerLetterSelectedFile = file;
      }
    }
    this.isSpinner = true;
    if (this.offerLetterSelectedFile && this.careerForm?.get('OfferLetter')?.valid) {
      this.hrmsService.EmployeeUploadFileCareer(
        // this.employeeDetails[0].EmpId,
        this.storeAddedEmployeeDetails.EmpId,
        this.careerForm?.get('OfferLetterMonth')?.value ? this.careerForm?.get('OfferLetterMonth')?.value : 'offerLatter',
        this.offerLetterSelectedFile
      ).subscribe(
        (res: any) => {
          console.log(res);
          this.getOfferLetter = res.path,
            this.isOfferLatterUploaded = true;
          this.triggerToast('', 'Offer Letter Uploaded Successfully', 'success');
          this.isSpinner = false;
        },
        (error) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
          this.careerForm?.get('OfferLetter').reset();
          this.offerLetterPath = '';
          this.offerLetterSrc = null
        }
      );
    }
  }
  salaryLetterUpload(event: any) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0] ?? null;
    if (file) {
      const maxSize = 5 * 1024 * 1024;
      if (file.size > maxSize) {
        this.triggerToast('', 'File Size Must Be Less Than 5MB', 'warning');
        return;
      }
      if (file.type.startsWith('image/')) {
        this.isSalaryLetterImage = true;
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.salaryLetterSrc = e.target.result;
        };
        reader.readAsDataURL(file);
        this.salaryLetterSelectedFile = file;
        this.salaryLetterPath = null;
        this.salaryLetterName = null;
      } else if (file.type === 'application/pdf') {
        this.salaryLetterName = file.name;
        this.salaryLetterPath = URL.createObjectURL(file);
        this.salaryLetterSrc = null;
        this.salaryLetterSelectedFile = file;
      }
    }
    this.isSpinner = true;
    if (this.salaryLetterSelectedFile && this.careerForm?.get('SalaryLetter')?.valid) {
      this.hrmsService.EmployeeUploadFileCareer(
        // this.employeeDetails[0].EmpId,
        this.storeAddedEmployeeDetails.EmpId,
        this.careerForm?.get('SalaryLetterMonth')?.value ? this.careerForm?.get('SalaryLetterMonth')?.value : 'salaryLatter',
        this.salaryLetterSelectedFile
      ).subscribe(
        (res: any) => {
          console.log(res);
          this.getSalaryLetter = res.path,
            this.isSalaryLatterUploaded = true;
          this.triggerToast('', 'Salary Letter Uploaded Successfully', 'success');
          this.isSpinner = false;
        },
        (error) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
          this.careerForm?.get('SalaryLetter').reset();
          this.salaryLetterPath = '';
          this.salaryLetterSrc = null
        }
      );
    }
  }
  experienceLetterUpload(event: any) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0] ?? null;
    if (file) {
      const maxSize = 5 * 1024 * 1024;
      if (file.size > maxSize) {
        this.triggerToast('', 'File Size Must Be Less Than 5MB', 'warning');
        return;
      }
      if (file.type.startsWith('image/')) {
        this.isExperienceLetterImage = true;
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.experienceLetterSrc = e.target.result;
        };
        reader.readAsDataURL(file);
        this.experienceLetterSelectedFile = file;
        this.experienceLetterPath = null;
        this.experienceLetterName = null;
      } else if (file.type === 'application/pdf') {
        this.isExperienceLetterImage = false;
        this.experienceLetterName = file.name;
        this.experienceLetterPath = URL.createObjectURL(file);
        this.experienceLetterSrc = null;
        this.experienceLetterSelectedFile = file;
      }
    }
    this.isSpinner = true;
    if (this.experienceLetterSelectedFile && this.careerForm?.get('ExperienceLetter')?.valid) {
      this.hrmsService.EmployeeUploadFileCareer(
        // this.employeeDetails[0].EmpId,
        this.storeAddedEmployeeDetails.EmpId,
        this.careerForm?.get('ExperienceLetterMonth')?.value ? this.careerForm?.get('ExperienceLetterMonth')?.value : 'salaryLatter',
        this.experienceLetterSelectedFile
      ).subscribe(
        (res: any) => {
          console.log(res);
          this.getExperienceLetter = res.path
          this.isExperienceLatterUploaded = true,
            this.triggerToast('', 'Experience Letter Uploaded Successfully', 'success');
          this.isSpinner = false;
        },
        (error) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
          this.careerForm?.get('SalaryLetter').reset();
          this.experienceLetterPath = '';
          this.experienceLetterSrc = null
        }
      );
    }
  }
  // GetEmpCareerDetails() {
  //   console.log(this.employeeDetails[0]);
  //   const reqBody = {
  // EmpId: this.storeAddedEmployeeDetails.EmpId ? this.storeAddedEmployeeDetails.EmpId : '', //this is storeAddedEmployeeDetails API  
  // LoginId: this.employeeDetails[0].LoginId,
  //   };
  //   this.isSpinner = true;
  //   this.hrmsService.employeeGetEmpCareerDetails(reqBody).subscribe({
  //     next: (res: any) => {
  //       this.isSpinner = false;
  //       if (res.length > 0) {
  //         this.getEmpCareerDetailsRows = res.map((item: any) => {
  //           const fullPath = this.formatPath(item.Path);
  //           return {
  //             ...item,
  //             FromDate: this.formatDate(this.parseJsonDate(item.FromDate)),
  //             ToDate: this.formatDate(this.parseJsonDate(item.ToDate)),
  //             FullPath: fullPath, 
  //             DisplayPath: this.getDisplayPath(fullPath) 
  //           };
  //         });
  //         this.showErrorCareer = false;
  //       } else {
  //         this.showErrorCareer = true;
  //         this.getEmpCareerDetailsRows = [];
  //       }
  //     },
  //     error: (error) => {
  //       this.isSpinner = false;
  //       this.showErrorCareer = true;
  //       if (error.status === 500) {
  //         this.errorMessageCareer = 'Internal Server Error';
  //       } else {
  //         this.errorMessageCareer = 'An unexpected error occurred. Please try again later.';
  //       }
  //       this.getEmpCareerDetailsRows = [];
  //     }
  //   });
  // }
  GetEmpCareerDetails() {
    const reqBody = {
      EmpId: this.storeAddedEmployeeDetails.EmpId ? this.storeAddedEmployeeDetails.EmpId : '', //this is storeAddedEmployeeDetails API  
      LoginId: this.employeeDetails[0].LoginId,
    };
    this.isSpinner = true;
    this.hrmsService.employeeGetEmpCareerDetails(reqBody).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (res.length > 0) {
          this.getEmpCareerDetailsRows = res.map((item: any) => {
            return {
              ...item,
              FromDate: this.formatDate(this.parseJsonDate(item.FromDate)),
              ToDate: this.formatDate(this.parseJsonDate(item.ToDate)),
              // Create the full path for each file
              PaySlip1: this.baseUrl + '/' + item.PaySlip1.replace('\\', '/'),
              PaySlip2: this.baseUrl + '/' + item.PaySlip2.replace('\\', '/'),
              PaySlip3: this.baseUrl + '/' + item.PaySlip3.replace('\\', '/'),
            };
          });
          this.monthHeaders = [
            res[0].PMonth1,
            res[0].PMonth2,
            res[0].PMonth3,
          ];
          // Set dynamic headers based on the first item
          if (res.length > 0) {
            this.dynamicHeaders = {
              offerLetter: res[0].OfferLetter,
              salaryLetter: res[0].SalaryLetter,
              experienceLetter: res[0].ExperienceLetter,
            };
          }
          this.showErrorCareer = false;
        } else {
          this.showErrorCareer = true;
          this.getEmpCareerDetailsRows = [];
          this.monthHeaders = [];
          this.dynamicHeaders = {};
        }
      },
      error: (error) => {
        this.isSpinner = false;
        if (error.status === 500) {
          this.errorMessageCareer = 'Internal Server Error';
        } else {
          this.errorMessageCareer = 'An unexpected error occurred. Please try again later.';
        }
        this.showErrorCareer = true;
        this.getEmpCareerDetailsRows = [];
        this.monthHeaders = [];
        this.dynamicHeaders = {};
      }
    });
  }

  createSet(): FormGroup {
    return this.fb.group({
      PMonth: ['', Validators.required],
      PaySlip: ['']
    });
  }

  get sets(): FormArray {
    return this.careerForm.get('sets') as FormArray;
  }
  addSet(): void {
    if (this.sets.length < this.maxSets) {
      this.sets.push(this.createSet());
      this.previewUrls.push('');
      this.fileNames.push('');
      this.uploadStatus.push('');
    }
  }
  removeSet(index: number): void {
    if (this.sets.length > 1) {
      this.sets.removeAt(index);
      this.previewUrls.splice(index, 1);
      this.fileNames.splice(index, 1);
      this.uploadStatus.splice(index, 1);
      delete this.fileUploads[`PaySlip${index + 1}`];
    }
  }
  truncateFileName(fileName: string, maxLength: number = 15): string {
    if (fileName.length <= maxLength) {
      return fileName; // Return the full name if it's already short enough
    }
    const parts = fileName.split('.');
    const extension = parts.length > 1 ? parts.pop() : ''; // Get the file extension safely
    const nameWithoutExtension = parts.join('.'); // Join the remaining parts to get the name without extension
    // Calculate the maximum length for the name part
    const maxNameLength = maxLength - (extension ? extension.length + 1 : 0); // +1 for the dot
    const truncatedName = nameWithoutExtension.slice(0, maxNameLength);
    return `${truncatedName}${truncatedName.length < nameWithoutExtension.length ? '...' : ''}${extension ? '.' + extension : ''}`;
  }
  payslipMonth(index: number, event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files ? input.files[0] : null;

    if (file) {
      if (file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.previewUrls[index] = e.target.result; // Set image preview
        };
        reader.readAsDataURL(file);
      } else {
        // Clear previous image preview for non-image files
        this.previewUrls[index] = ''; // Set to empty string
      }

      // Set the truncated file name
      this.fileNames[index] = this.truncateFileName(file.name);
      this.uploadFile(index, file);
    }
  }

  uploadFile(index: number, file: File): void {
    const empId = this.storeAddedEmployeeDetails.EmpId; // Replace with actual employee ID
    const docName = `PaySlip${index + 1}`; // Use dynamic document name based on index
    this.isSpinner = true;
    this.hrmsService.EmployeeUploadFileCareer(empId, docName, file).subscribe({
      next: (response) => {
        this.fileUploads[docName] = response.path;
        this.triggerToast('', 'Payslip Uploaded Successfully', 'success');
        this.isSpinner = false;
        // Update form field with file path
        const paySlipControl = this.sets.at(index).get('PaySlip');
        if (paySlipControl) {
          paySlipControl.setValue(response.path); // Set file path
        }
        // Reset file input element
        const fileInput = document.getElementById(`PMonthOne${index}`) as HTMLInputElement;
        if (fileInput) {
          fileInput.value = ''; // This will clear the file input, but won't set it to a specific file
        }
      },
      error: (error) => {
        this.uploadStatus[index] = 'Upload failed!';
        this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
        this.previewUrls = [];
        this.fileNames = [];
        this.uploadStatus = [];
        this.fileUploads = {};
        this.isSpinner = false;
      }
    });
  }
  validateFileUploads(): boolean {
    // Check if at least 3 files have been uploaded
    let uploadedFiles = 0;
    this.sets.controls.forEach((set, index) => {
      if (this.fileUploads[`PaySlip${index + 1}`]) {
        uploadedFiles++;
      }
    });
    return uploadedFiles >= 3; // Ensure at least 3 files are uploaded
  }
  submitCareerDetails() {
    console.log(this.careerForm.valid);
    console.log(this.careerForm);
    this.isFormSubmitted = true;
    const fromDateValue = this.careerForm?.get('date_from')?.value;
    const toDateValue = this.careerForm?.get('date_to')?.value;
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
    if (this.careerForm?.valid && this.validateFileUploads()) {
      const reqBody = {
        EmpId: this.storeAddedEmployeeDetails.EmpId,
        LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        Company: this.careerForm?.get('careerCompanyName').value ? this.careerForm?.get('careerCompanyName').value : '',
        Designation: this.careerForm?.get('career_Designation').value ? this.careerForm?.get('career_Designation').value : '',
        FromDate: fromOnly ? fromOnly : '',
        ToDate: toOnly ? toOnly : '',
        Experience: this.careerForm?.get('career_Experience').value ? this.careerForm?.get('career_Experience').value : '',
        ContactName: this.careerForm?.get('career_HR_ManagerName').value ? this.careerForm?.get('career_HR_ManagerName').value : '',
        ContactDesignation: this.careerForm?.get('career_HR_ManagerDesignation').value ? this.careerForm?.get('career_HR_ManagerDesignation').value : '',
        ContactEmail: this.careerForm?.get('career_HR_ManagerEmail').value ? this.careerForm?.get('career_HR_ManagerEmail').value : '',
        ContactMobile: this.careerForm?.get('career_HR_ManagerMob').value ? this.careerForm?.get('career_HR_ManagerMob').value : '',
        CTC: this.careerForm?.get('career_CTC').value ? this.careerForm?.get('career_CTC').value : '',
        Reason: this.careerForm?.get('career_Reason').value ? this.careerForm?.get('career_Reason').value : '',
        PMonth1: this.careerForm?.get('sets').at(0)?.get('PMonth')?.value || '',
        PaySlip1: this.fileUploads['PaySlip1'] || '',
        PMonth2: this.careerForm?.get('sets').at(1)?.get('PMonth')?.value || '',
        PaySlip2: this.fileUploads['PaySlip2'] || '',
        PMonth3: this.careerForm?.get('sets').at(2)?.get('PMonth')?.value || '',
        PaySlip3: this.fileUploads['PaySlip3'] || '',
        OfferLetter: this.getOfferLetter ? this.getOfferLetter : '',
        SalaryLetter: this.getSalaryLetter ? this.getSalaryLetter : '',
        ExperienceLetter: this.getExperienceLetter ? this.getExperienceLetter : '',
        RelievingLetter: '',
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeAddEmpCareerDetails(reqBody).subscribe((res: any) => {
        console.log(res);
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
          this.isSpinner = false;
          this.GetEmpCareerDetails();
          this.resetCareerDetails();
        } else if (res['Message']) {
          this.triggerToast('Something Went Wrong', res['Message'], 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
        this.isSpinner = false;
      })
    } else {
      this.triggerToast('Form is invalid', 'Upload Last 3 Months Pay Slips', 'danger');
      console.log('Form is invalid or not enough files uploaded');
    }
  }
  updateCareerDetails(data: any) {
    console.log(data);
    this.isCareerUpdateButton = true;
    this.careerForm?.patchValue({
      careerCompanyName: data.Company,
      career_Designation: data.Designation,
      date_from: data.FromDate,
      date_to: data.ToDate,
      career_HR_ManagerName: data.ContactName,
      career_HR_ManagerDesignation: data.ContactDesignation,
      career_HR_ManagerEmail: data.ContactEmail,
      career_HR_ManagerMob: data.ContactMobile,
      career_CTC: data.CTC,
      career_Reason: data.Reason,
    });
    this.patchCareerData = data;
    // Ensure that the FormArray has the correct number of items
    while (this.sets.length < 3) { // Adjust this number if you have a different maximum
      this.addSet();
    }
    this.sets.controls.forEach((set, index) => {
      const pMonthKey = `PMonth${index + 1}`;
      const paySlipKey = `PaySlip${index + 1}`;
      if (data[pMonthKey]) {
        set.get('PMonth')?.setValue(data[pMonthKey]);
      } else {
        set.get('PMonth')?.setValue(''); // Set to empty if not found
      }
      if (data[paySlipKey]) {
        this.fileUploads[paySlipKey] = data[paySlipKey];
        this.fileNames[index] = data[paySlipKey].split('/').pop() || '';
        this.previewUrls[index] = `${this.baseUrl}/${data[paySlipKey].replace(/\\/g, '/')}`;
      } else {
        this.fileUploads[paySlipKey] = '';
        this.fileNames[index] = '';
        this.previewUrls[index] = '';
      }
    });
    const photoSalary = this.patchCareerData.SalaryLetter;
    this.isValidPhotoSalary = photoSalary !== null && photoSalary !== undefined && photoSalary !== '';
    const photoOffer = this.patchCareerData.OfferLetter;
    this.isValidPhotoOffer = photoOffer !== null && photoOffer !== undefined && photoOffer !== '';
    const photoExperience = this.patchCareerData.ExperienceLetter;
    this.isValidPhotoExperience = photoExperience !== null && photoExperience !== undefined && photoExperience !== '';
  }
  isOfferLetterPreset() {
    if (this.isValidPhotoOffer) {
      if (this.isOfferLatterUploaded) {
        return this.getOfferLetter;
      } else {
        const getPhotoUrl = this.patchCareerData?.OfferLetter.replace(/\\/g, "\\\\");
        this.patchOfferLatter = `${this.baseUrl}/${getPhotoUrl}`;
        return this.patchOfferLatter;
      }
    } else if (!this.isValidPhotoOffer) {
      if (this.isOfferLatterUploaded) {
        return this.getOfferLetter;
      } else {
        return '';
      }
    }
  }
  isSalaryLetterPreset() {
    if (this.isValidPhotoSalary) {
      if (this.isSalaryLatterUploaded) {
        return this.getSalaryLetter;
      } else {
        const getPhotoUrl = this.patchCareerData?.SalaryLetter.replace(/\\/g, "\\\\");
        this.patchSalaryLetter = `${this.baseUrl}/${getPhotoUrl}`;
        return this.patchSalaryLetter;
      }
    } else if (!this.isValidPhotoSalary) {
      if (this.isSalaryLatterUploaded) {
        return this.getSalaryLetter;
      } else {
        return '';
      }
    }
  }
  isExperienceLetterPreset() {
    if (this.isValidPhotoExperience) {
      if (this.isExperienceLatterUploaded) {
        return this.getExperienceLetter;
      } else {
        const getPhotoUrl = this.patchCareerData?.ExperienceLetter.replace(/\\/g, "\\\\");
        this.patchExperienceLetter = `${this.baseUrl}/${getPhotoUrl}`;
        return this.patchExperienceLetter;
      }
    } else if (!this.isValidPhotoExperience) {
      if (this.isExperienceLatterUploaded) {
        return this.getExperienceLetter;
      } else {
        return '';
      }
    }
  }
  updatecareerForm() {
    this.isFormSubmitted = true;
    if (this.careerForm?.valid) {
      const reqBody = {
        // EmpId: this.employeeDetails[0].EmpId ? this.employeeDetails[0].EmpId : '',
        // LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
        LoginId: this.employeeDetails[0].LoginId,
        CareerId: this.patchCareerData.CareerId ? this.patchCareerData.CareerId : '',
        Company: this.careerForm?.get('careerCompanyName').value ? this.careerForm?.get('careerCompanyName').value : '',
        Designation: this.careerForm?.get('career_Designation').value ? this.careerForm?.get('career_Designation').value : '',
        FromDate: this.careerForm?.get('date_from').value,
        ToDate: this.careerForm?.get('date_to').value,
        Experience: this.careerForm?.get('career_Experience').value ? this.careerForm?.get('career_Experience').value : '',
        ContactName: this.careerForm?.get('career_HR_ManagerName').value ? this.careerForm?.get('career_HR_ManagerName').value : '',
        ContactDesignation: this.careerForm?.get('career_HR_ManagerDesignation').value ? this.careerForm?.get('career_HR_ManagerDesignation').value : '',
        ContactEmail: this.careerForm?.get('career_HR_ManagerEmail').value ? this.careerForm?.get('career_HR_ManagerEmail').value : '',
        ContactMobile: this.careerForm?.get('career_HR_ManagerMob').value ? this.careerForm?.get('career_HR_ManagerMob').value : '',
        CTC: this.careerForm?.get('career_CTC').value ? this.careerForm?.get('career_CTC').value : '',
        Reason: this.careerForm?.get('career_Reason').value ? this.careerForm?.get('career_Reason').value : '',
        PMonth1: this.careerForm?.get('sets').at(0)?.get('PMonth')?.value || '',
        PaySlip1: this.fileUploads['PaySlip1'] || '',
        PMonth2: this.careerForm?.get('sets').at(1)?.get('PMonth')?.value || '',
        PaySlip2: this.fileUploads['PaySlip2'] || '',
        PMonth3: this.careerForm?.get('sets').at(2)?.get('PMonth')?.value || '',
        PaySlip3: this.fileUploads['PaySlip3'] || '',
        SalaryLetter: this.isSalaryLetterPreset(),
        ExperienceLetter: this.isExperienceLetterPreset(),
        RelievingLetter: '',
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeUpdateEmpCareerDetails(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast('', 'Records Updated Successfully', 'success');
          this.isSpinner = false;
          this.GetEmpCareerDetails();
          this.resetCareerDetails()
        } else {
          this.triggerToast('Something Went Wrong', 'Try Again', 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Update Records', "danger");
        this.isSpinner = false;
      })
    } else {

    }
  }
  deleteCareerDetails(data: any) {
    console.log(data);
    this.getCareerTableDeleteId = data.CareerId
  }
  deleteCareerTableList() {
    const reqBody = {
      // LoginId: this.employeeDetails[0].LoginId,
      // EmpId: this.employeeDetails[0].EmpId,
      EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
      LoginId: this.employeeDetails[0].LoginId,
      CareerId: this.getCareerTableDeleteId
    }
    console.log(reqBody);
    this.isSpinner = true;
    this.hrmsService.employeeDeleteEmpCareerDetails(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.GetEmpCareerDetails();
        this.isSpinner = false;
      } else if (res['Message']) {
        this.triggerToast('Please Try Agian', res['Message'], 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To delete The Record', 'danger');
      this.isSpinner = false;
    })
  }
  resetCareerDetails() {
    this.careerForm?.reset();
    this.offerLetterSrc = null;
    this.offerLetterName = null;
    this.salaryLetterSrc = null;
    this.salaryLetterName = null;
    this.experienceLetterSrc = null;
    this.experienceLetterName = null;
    this.minDateCareer = undefined;
    this.maxDateCareer = undefined;
    this.isCareerUpdateButton = false;
    this.isFormSubmitted = false;

    // Reset the FormArray to contain only one FormGroup
    const formArray = this.sets;
    while (formArray.length > 1) {
      formArray.removeAt(1);
    }
    // Reset values and clear other arrays
    formArray.at(0).reset();
    this.previewUrls = [''];
    this.fileNames = [''];
    this.uploadStatus = [''];
    this.fileUploads = {};
    this.isSalaryLatterUploaded = false;
    this.getSalaryLetter = '';
    this.patchSalaryLetter = '';

    this.isOfferLatterUploaded = false;
    this.getOfferLetter = '';
    this.patchOfferLatter = '';

    this.isExperienceLatterUploaded = false;
    this.getExperienceLetter = '';
    this.patchExperienceLetter = '';
  }
  // ***************Career Details .ts Finished************************

  // ***************Education .ts Starts************************
  educationFormval() {
    this.EducationForm = this.fb.group({
      school: ['', [Validators.required]],
      field_Of_Study: ['', [Validators.required]],
      date_from: ['', [Validators.required]],
      date_to: ['', [Validators.required]],
      grade: ['', Validators.pattern('^[0-9. %]*$')],
      edu_description: [''],
      edu_doc_name: ['', [Validators.required]],
      Others: [''],
      edu_photo: ['']
    }, { validators: this.dateRangeValidatorEducation });
    this.getEducationDropDown();
    // this.getGetEducationDoc();
    this.EducationForm.get('edu_doc_name').valueChanges.subscribe((selectedValue: any) => {
      const othresControl = this.EducationForm?.get('Others');
      if (selectedValue === '5') {
        othresControl.enable();
        othresControl.setValidators([Validators.required]);
        this.isShowEducOthers = true
      } else {
        othresControl.disable();
        othresControl.clearValidators();
        this.isShowEducOthers = false;
      }
      othresControl.updateValueAndValidity();
    });
  }
  // Custom validator to ensure date_to is after date_from
  dateRangeValidatorEducation(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRangeEducation: true };
    }
    return null;
  }
  onFromDateEducation(): void {
    if (this.EducationForm.get('date_from')?.value) {
      this.minDateEducation = this.EducationForm.get('date_from')?.value;
    }
  }
  onToDateEducation(): void {
    if (this.EducationForm.get('date_to')?.value) {
      this.maxDateEducation = this.EducationForm.get('date_to')?.value;
    }
  }
  isFromDateInvalidEducation(): boolean {
    const fromDate = this.EducationForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalidEducation(): boolean {
    const toDate = this.EducationForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeErrorEducation(): boolean {
    return this.EducationForm.hasError('dateRangeEducation');
  }
  getEducationDropDown() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner = true;
    this.hrmsService.employeeGetDDEducationDoc(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.educationDocName = res;
        this.isSpinner = false;
      } else {
        // this.triggerToast('No Data Found!,Document Name',' Please Refersh Page at Once','Warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Education Qualification', 'danger');
      this.isSpinner = false;
    })
  }
  chooseEducDoc(event: Event) {
    // const input = event.target as HTMLInputElement;
    // if (input.files && input.files.length > 0) {
    //   const file = input.files[0];
    //   if (!file.type.match(/image\/(jpg|jpeg)/)) {
    //     alert('Only JPG and JPEG files are allowed.');
    //     input.value = ''; // Clear the input
    //     return;
    //   }
    //   if (file.size > 5 * 1024 * 1024) { // 5 MB
    //     alert('File size should not exceed 5 MB.');
    //     input.value = ''; // Clear the input
    //     return;
    //   }
    //   const reader = new FileReader();
    //   reader.onload = () => {
    //     this.imageSrcEducion = reader.result;
    //   };
    //   reader.readAsDataURL(file);
    //   this.educSelectedFile = file;
    // }
    if (this.EducationForm?.get('edu_doc_name').valid) {
      const input = event.target as HTMLInputElement;
      const file = input?.files?.[0] ?? null;
      if (file) {
        const maxSize = 5 * 1024 * 1024;
        if (file.size > maxSize) {
          this.triggerToast('', 'File Size Must Be Less Than 5MB', 'warning');
          return;
        }
        if (file.type.startsWith('image/')) {
          this.isImageEducDoc = true;
          const reader = new FileReader();
          reader.onload = (e: any) => {
            this.imageSrcEducDoc = e.target.result;
          };
          reader.readAsDataURL(file);
          this.SelectedFileEducDoc = file;
          this.filePathEducDoc = null;
          this.fileNameEducDoc = null;
        } else if (file.type === 'application/pdf') {
          this.isImageEducDoc = false;
          // this.fileNameEducDoc = file.name;
          this.fileNameEducDoc = this.truncateFileName(file.name);
          this.filePathEducDoc = URL.createObjectURL(file);
          this.imageSrcEducDoc = null;
          this.SelectedFileEducDoc = file;
        }
      }
    } else {
      this.triggerToast('', 'Please Fill The Qualification', 'warning');
      this.EducationForm?.get('edu_doc_name').reset();
      this.EducationForm?.get('edu_photo').reset();
      this.filePathEducDoc = '';
      this.imageSrcEducDoc = null
    }
    if (this.SelectedFileEducDoc && this.EducationForm?.get('edu_doc_name')?.valid) {
      this.isSpinner = true;
      this.hrmsService.employeeUploadEducFileDoc(
        // this.employeeDetails[0].EmpId,
        this.storeAddedEmployeeDetails.EmpId,
        this.educselectedName,
        this.SelectedFileEducDoc
      ).subscribe(
        (res: any) => {
          console.log(res);
          this.getEducDoc = res.path;
          this.isEducationUploaded = true;
          this.triggerToast('', 'Education Document Uploaded Successfully', 'success');
          this.isSpinner = false;
        },
        (error) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
          this.EducationForm?.get('edu_doc_name').reset();
          this.EducationForm?.get('edu_photo').reset();
        }
      );
    } else {
      this.isSpinner = false;
    }
  }
  // uploadEducImage() {
  //   if (!this.educSelectedFile) {
  //     alert('No file selected.');
  //     return;
  //   }
  //   this.isSpinner = true
  //   if (this.EducationForm?.get('edu_doc_name').valid) {
  //     this.hrmsService.employeeUploadEducFileDoc(this.employeeDetails[0].EmpId, this.EducationForm?.get('edu_doc_name').value, this.educSelectedFile).subscribe((res: any) => {
  //       if (res) {
  //         this.educImagePath = res.path;
  //         console.log(this.educImagePath);

  //         this.isSpinner = false;
  //         this.triggerToast(res['msg'], 'Education Document Uploaded', "success");
  //         this.isUploadSuccess = true;
  //       }
  //     }, error => {
  //       this.triggerToast(error['Message'], 'Internal Server Error', "danger");
  //       this.isSpinner = false;
  //       this.isUploadSuccess = false;
  //     })
  //   } else {
  //     this.triggerToast('', 'Please Select Document Name', "warning");
  //     this.isSpinner = false;
  //     this.isUploadSuccess = false;
  //   }
  // }
  getEducDocName(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.educselectedID = selectElement.value;
    this.educselectedName = selectElement.options[selectElement.selectedIndex].text;
  }
  addEductionDetails() {
    this.isFormSubmitted = true;
    const fromDateValue = this.EducationForm?.get('date_from')?.value;
    const toDateValue = this.EducationForm?.get('date_to')?.value;
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
    if ((this.EducationForm.valid)) {
      // const fromDateValue = this.EducationForm?.get('date_from')?.value;
      // const toDateValue = this.EducationForm?.get('date_to')?.value;
      // const formatDate = (dateValue: string): string => {
      //   const date = new Date(dateValue);
      //   if (isNaN(date.getTime())) {
      //     return "";
      //   }
      //   const year = date.getFullYear();
      //   const month = String(date.getMonth() + 1).padStart(2, '0');
      //   const day = String(date.getDate()).padStart(2, '0');
      //   return `${day}-${month}-${year}`;
      // };
      // const fromOnly = formatDate(fromDateValue);
      // const toOnly = formatDate(toDateValue);

      // const isPhotoRequired = this.EducationForm?.get('edu_photo')?.value;
      // if (isPhotoRequired && !this.educImagePath) {
      //   this.triggerToast("Photo Required", "Please click send photo before submitting.", "danger");
      //   return; 
      // }
      const reqbody = {
        EmpId: this.storeAddedEmployeeDetails.EmpId,
        LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        DocId: this.educselectedID ? this.educselectedID : '',
        Others: this.EducationForm?.get('Others').value ? this.EducationForm?.get('Others').value : '',
        School: this.EducationForm?.get('school').value ? this.EducationForm?.get('school').value : '',
        DocName: this.educselectedName ? this.educselectedName : '',
        Filed: this.EducationForm?.get('field_Of_Study').value ? this.EducationForm?.get('field_Of_Study').value : '',
        StartDate: fromOnly ? fromOnly : '',
        EndDate: toOnly ? toOnly : '',
        Grade: this.EducationForm?.get('grade').value ? this.EducationForm?.get('grade').value : '',
        Description: this.EducationForm?.get('edu_description').value ? this.EducationForm?.get('edu_description').value : '',
        Path: this.getEducDoc ? this.getEducDoc : ''
      }
      console.log(reqbody);
      this.isSpinner = true;
      this.hrmsService.employeeAddEducationDetails(reqbody).subscribe((res: any) => {
        if (res['msg'] == 'Added') {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.getGetEducationDoc();
          this.resetEducationDoc()
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error, 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
    }
  }
  getGetEducationDoc() {
    console.log(this.employeeDetails);
    const reqBody = {
      // LoginId: this.employeeDetails[0].LoginId,
      // EmpId: this.employeeDetails[0].EmpId,
      EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
      LoginId: this.employeeDetails[0].LoginId,
    }
    console.log(reqBody);
    this.isSpinner = true;
    this.hrmsService.employeeGetEducationDoc(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        // this.educationDetailsRows = res.map((item: any) => {
        //   const fullPath = this.formatPath(item.Path);
        //   return {
        //     ...item,
        //     StartDate: this.formatDate(this.parseJsonDate(item.StartDate)),
        //     EndDate: this.formatDate(this.parseJsonDate(item.EndDate)),
        //     FullPath: fullPath, // Full URL
        //     DisplayPath: this.getDisplayPath(fullPath) // Shortened display text
        //   };
        // });
        this.educationDetailsRows = res.map((item: any) => ({
          ...item,
          StartDate: this.formatDate(this.parseJsonDate(item.StartDate)),
          EndDate: this.formatDate(this.parseJsonDate(item.EndDate)),
          // Path: this.baseUrl + '/' + item.Path.replace('\\', '/'),
        }));
        this.isSpinner = false;
      } else {
        this.isSpinner = false;
        this.educationDetailsRows = []
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Get Education Details', "danger");
      this.isSpinner = false;
      this.educationDetailsRows = []
    })
  }
  onEditEducationDoc(event: any) {
    event.stopPropagation();
  }
  updateEducationDoc(data: any) {
    console.log(data);
    this.isEducUpdateButton = true;
    this.EducationForm.patchValue({
      school: data?.School,
      degree: data?.DegreeId,
      field_Of_Study: data?.Filed,
      date_from: data?.StartDate,
      date_to: data?.EndDate,
      grade: data?.Grade,
      edu_doc_name: data?.DocId,
      Others: data?.Others,
      edu_description: data?.Description
    });
    this.pathchEducationData = data
    const educPath = this.pathchEducationData?.Path.replace(/\\/g, "\\\\");
    this.patchEducPath = `${this.baseUrl}/${educPath}`;
  }
  isEducationPreset() {
    if (this.pathchEducationData.Path === '') {
      if (this.isEducationUploaded) {
        return this.getEducDoc
      } else {
        return ''
      }
    } else {
      return this.patchEducPath;
    }
  }
  updateEducationForm() {
    this.isFormSubmitted = true;
    if (this.EducationForm?.valid) {
      const reqBody = {
        // LoginId: this.employeeDetails[0].LoginId,
        // EmpId: this.employeeDetails[0].EmpId,
        EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
        LoginId: this.employeeDetails[0].LoginId,
        Id: this.pathchEducationData.Id,
        DocId: this.pathchEducationData.DocId,
        Others: this.EducationForm?.get('Others').value ? this.EducationForm?.get('Others').value : '',
        School: this.EducationForm?.get('school').value ? this.EducationForm?.get('school').value : '',
        DocName: this.educselectedName ? this.educselectedName : this.pathchEducationData.DocName,
        Filed: this.EducationForm?.get('field_Of_Study').value ? this.EducationForm?.get('field_Of_Study').value : '',
        StartDate: this.EducationForm?.get('date_from').value ? this.EducationForm?.get('date_from').value : '',
        EndDate: this.EducationForm?.get('date_to').value ? this.EducationForm?.get('date_to').value : '',
        Grade: this.EducationForm?.get('grade').value ? this.EducationForm?.get('grade').value : '',
        Description: this.EducationForm?.get('edu_description').value ? this.EducationForm?.get('edu_description').value : '',
        Path: this.isEducationPreset(),
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeUpdateEducationDoc(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast('', 'Records Updated Successfully', 'success');
          this.isSpinner = false;
          this.getGetEducationDoc();
          this.resetEducationDoc()
        } else {
          this.triggerToast('Something Went Wrong', 'Try Again', 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Update Records', "danger");
        this.isSpinner = false;
      })
    } else {
      this.isFormSubmitted = true;
      this.triggerToast('', 'Please Fill Required Data', "danger");
      this.isSpinner = false;
    }
  }
  deleteEducation(data: any) {
    this.getEducTableDeleteId = data.Id;
  }
  deleteEducationTableList() {
    const reqBody = {
      // LoginId: this.employeeDetails[0].LoginId,
      // EmpId: this.employeeDetails[0].EmpId,
      EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
      LoginId: this.employeeDetails[0].LoginId,
      Id: this.getEducTableDeleteId
    }
    console.log(reqBody);
    this.isSpinner = true;
    this.hrmsService.employeeDeleteEducationDoc(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getGetEducationDoc();
        this.isSpinner = false;
      } else if (res['Message']) {
        this.triggerToast('Something Went Wrong', 'Please Try Again', 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To delete The Record', 'danger');
      this.isSpinner = false;
    })
  }
  resetEducationDoc() {
    this.EducationForm?.reset();
    this.isEducUpdateButton = false;
    this.imageSrcEducDoc = '';
    this.fileNameEducDoc = '';
    this.isImageEducDoc = false;
    this.filePathEducDoc = '';
    this.minDateEducation = undefined;
    this.maxDateEducation = undefined;
    this.getEducDoc = null;
    this.isFormSubmitted = false;
    this.EducationForm?.get('edu_photo').reset();
  }
  // ***************Education .ts Finished************************

  // ***************Account .ts Starts************************
  accountFormVal() {
    this.accountForm = this.fb.group({
      BankName: ['', [Validators.required]],
      IFSCCode: ['', [Validators.required, Validators.pattern('^[A-Za-z0-9]{10,16}$')]],
      BranchName: ['', [Validators.required]],
      AccHolderName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      AccNo: ['', [Validators.required]],
      // PFNo: ['',],
      Acc_MobileNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
    })
  }
  addAccountDetails() {
    this.isFormSubmitted = true;
    if (this.accountForm?.valid) {
      const reqBody = {
        EmpId: this.storeAddedEmployeeDetails.EmpId,
        LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        BankName: this.accountForm?.get('BankName').value,
        IFSCCode: this.accountForm?.get('IFSCCode').value,
        BranchName: this.accountForm?.get('BranchName').value,
        AccHolderName: this.accountForm?.get('AccHolderName').value,
        AccNo: this.accountForm?.get('AccNo').value,
        // PFNo: this.accountForm?.get('PFNo').value,
        MobileNo: this.accountForm?.get('Acc_MobileNo').value,
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeAddEmpAccDetails(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.isFormSubmitted = false;
        } else {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('', 'Internal Server Error', 'danger');
        this.isSpinner = false;
      })
    } else {
      this.triggerToast('', 'Please Fill All details', 'warning');
    }
  }
  // ***************Account .ts Finished************************

  // ***************Government .ts Starts************************
  onEditGovtDetails(event: any) {
    event.stopPropagation();
  }
  governmentFormval() {
    this.governmentForm = this.fb.group({
      // govt_name: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      gov_doc_name: ['', [Validators.required]],
      govt_doc_num: ['', [Validators.required]],
      date_from: [''],
      date_to: [''],
      Others: [''],
      govt_description: [''],
      gov_photo: ['']
    }, { validators: this.dateRangeValidatorGovt });
    this.getGovDropDownDoc();
    this.governmentForm.get('gov_doc_name').valueChanges.subscribe((val: any) => {
      this.governmentForm.get('date_from').clearValidators();
      this.governmentForm.get('date_to').clearValidators();
      this.governmentForm.get('Others').clearValidators();
      switch (val) {
        case '9':
          this.governmentForm.get('date_from').setValidators([Validators.required]);
          this.governmentForm.get('date_to').setValidators([Validators.required]);
          this.isGovtVisibleDates = true;
          this.isViewGovtOthers = false;
          break;

        case '10':
          this.governmentForm.get('Others').setValidators([Validators.required]);
          this.isGovtVisibleDates = true;
          this.isViewGovtOthers = true;
          break;

        default:
          this.isGovtVisibleDates = false;
          this.isViewGovtOthers = false;
          break;
      }
      this.governmentForm.get('date_from').updateValueAndValidity();
      this.governmentForm.get('date_to').updateValueAndValidity();
      this.governmentForm.get('Others').updateValueAndValidity();
      this.cdr.detectChanges();
    });
  }
  // Custom validator to ensure date_to is after date_from
  dateRangeValidatorGovt(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRangeGovt: true };
    }
    return null;
  }
  onFromDateGovt(): void {
    if (this.governmentForm.get('date_from')?.value) {
      this.minDateGovt = this.governmentForm.get('date_from')?.value;
    }
  }
  onToDateGovt(): void {
    if (this.governmentForm.get('date_to')?.value) {
      this.maxDateGovt = this.governmentForm.get('date_to')?.value;
    }
  }
  isFromDateInvalidGovt(): boolean {
    const fromDate = this.governmentForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalidGovt(): boolean {
    const toDate = this.governmentForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeErrorGovt(): boolean {
    return this.governmentForm.hasError('dateRangeGovt');
  }
  getGovDropDownDoc() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner = true;
    this.hrmsService.employeeGetDDGovtDoc(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.govtDocName = res;
        this.isSpinner = false;
      } else {
        // this.triggerToast('No Data Found!,Document Name',' Please Refersh Page at Once','Warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To load Government Document Name', 'danger');
      this.isSpinner = false;
    })
  }
  // Helper function to format the path
  formatPath(path: string): string {
    return `${this.baseUrl}/${path.replace(/\\/g, "/")}`;
  }
  // Helper function to generate a shortened display text
  getDisplayPath(fullPath: string): string {
    // Adjust the length or format according to your needs
    const maxLength = 20; // Maximum number of characters for display
    if (fullPath.length > maxLength) {
      return `...${fullPath.slice(-maxLength)}`;
    }
    return fullPath;
  }
  getGovtDocName(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.govtselectedID = selectElement.value;
    this.govtselectedName = selectElement.options[selectElement.selectedIndex].text;
  }

  chooseGovDoc(event: Event) {
    // const input = event.target as HTMLInputElement;
    // if (input.files && input.files.length > 0) {
    //   const file = input.files[0];

    //   // Check file type and size
    //   if (!file.type.match(/image\/(jpg|jpeg)/)) {
    //     alert('Only JPG and JPEG files are allowed.');
    //     input.value = ''; // Clear the input
    //     return;
    //   }

    //   if (file.size > 5 * 1024 * 1024) { // 5 MB
    //     alert('File size should not exceed 5 MB.');
    //     input.value = ''; // Clear the input
    //     return;
    //   }

    //   // File is valid; proceed to preview
    //   const reader = new FileReader();
    //   reader.onload = () => {
    //     this.imageSrcGovt = reader.result;
    //   };
    //   reader.readAsDataURL(file);
    //   // Save the file for uploading
    //   this.govtSelectedFile = file;
    // }
    if (this.governmentForm?.get('gov_doc_name').valid) {
      const input = event.target as HTMLInputElement;
      const file = input?.files?.[0] ?? null;
      if (file) {
        const maxSize = 5 * 1024 * 1024;
        if (file.size > maxSize) {
          this.triggerToast('', 'File Size Must Be Less Than 5MB', 'warning');
          return;
        }
        if (file.type.startsWith('image/')) {
          this.isImageGovtDoc = true;
          const reader = new FileReader();
          reader.onload = (e: any) => {
            this.imageSrcGovtDoc = e.target.result;
          };
          reader.readAsDataURL(file);
          this.SelectedFileGovtDoc = file;
          this.filePathGovtDoc = null;
          this.fileNameGovtDoc = null;
        } else if (file.type === 'application/pdf') {
          this.isImageGovtDoc = false;
          // this.fileNameGovtDoc = file.name;
          this.fileNameGovtDoc = this.truncateFileName(file.name);
          this.filePathGovtDoc = URL.createObjectURL(file);
          this.imageSrcGovtDoc = null;
          this.SelectedFileGovtDoc = file;
        }
      }
    } else {
      this.triggerToast('', 'Please Fill The Document Name', 'warning');
      this.governmentForm?.get('gov_doc_name').reset();
      this.governmentForm?.get('gov_photo').reset();
      this.filePathGovtDoc = '';
      this.imageSrcGovtDoc = null
    }
    if (this.SelectedFileGovtDoc && this.governmentForm?.get('gov_doc_name')?.valid) {
      this.isSpinner = true;
      this.hrmsService.employeeUploadGovtFileDoc(
        // this.employeeDetails[0].EmpId,
        this.storeAddedEmployeeDetails.EmpId,
        this.govtselectedName,
        this.SelectedFileGovtDoc
      ).subscribe(
        (res: any) => {
          console.log(res);
          this.getGovtDoc = res.path;
          this.isGovtUploaded = true;
          this.triggerToast('', 'Government Document Uploaded Successfully', 'success');
          this.isSpinner = false;
        },
        (error) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'Something Went Wrong', 'danger');
          this.governmentForm?.get('gov_doc_name').reset();
          this.governmentForm?.get('gov_photo').reset();
          this.filePathGovtDoc = '';
          this.imageSrcGovtDoc = null
        }
      );
    }
    else {
      this.isSpinner = false;
    }
  }
  // uploadGovImage() {
  //   if (!this.govtSelectedFile) {
  //     alert('No file selected.');
  //     return;
  //   }
  //   this.isSpinner = true
  //   this.hrmsService.employeeUploadGovtFileDoc(this.employeeDetails[0].EmpId, this.governmentForm?.get('gov_doc_name').value, this.govtSelectedFile).subscribe((res: any) => {
  //     if (res) {
  //       this.govImagePath = res.path;
  //       console.log(this.govImagePath);

  //       this.isSpinner = false;
  //       this.triggerToast(res['msg'], 'Government Document Uploaded', "success");
  //       this.isUploadSuccess = true;
  //     }
  //   }, error => {
  //     this.triggerToast(error['Message'], 'Internal Server Error', "danger");
  //     this.isSpinner = false;
  //     this.isUploadSuccess = false;
  //   })
  // }

  getGetGovtDoc() {
    console.log(this.employeeDetails[0]);
    const reqBody = {
      // LoginId: this.employeeDetails[0].LoginId,
      // EmpId: this.employeeDetails[0].EmpId,
      EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
      LoginId: this.employeeDetails[0].LoginId,
    }
    console.log(reqBody);

    this.isSpinner = true;
    this.hrmsService.employeeGetGovtDoc(reqBody).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (res.length >= 1) {
          this.govtDetailsRows = res;
          // this.govtDetailsRows = res.map((item: any) => {
          //   const fullPath = this.formatPath(item.Path);
          //   return {
          //     ...item,
          //     FullPath: fullPath, // Full URL
          //     DisplayPath: this.getDisplayPath(fullPath) // Shortened display text
          //   };
          // });
          this.isSpinner = false;
          this.showErrorGovt = false;
        } else {
          this.showErrorGovt = true;
          this.govtDetailsRows = [];
          this.isSpinner = false;
        }
      },
      error: (error) => {
        this.isSpinner = false;
        if (error.status === 500) {
          this.errorMessageCareer = 'Internal Server Error';
        } else {
          this.errorMessageCareer = 'An unexpected error occurred. Please try again later.';
        }
        this.showErrorCareer = true;
      }
    });
  }
  addGovermentDetails() {
    this.isFormSubmitted = true;
    if ((this.governmentForm.valid)) {
      // const isPhotoRequired = this.governmentForm?.get('gov_photo')?.value;
      // if (isPhotoRequired && !this.govImagePath) {
      //   this.triggerToast("Photo Required", "Please click send photo before submitting.", "danger");
      //   return; 
      // }
      const fromDateValue = this.governmentForm?.get('date_from')?.value;
      const toDateValue = this.governmentForm?.get('date_to')?.value;
      const parseDate = (date: any): Date | null => {
        if (date === null || date === undefined) return null;
        if (typeof date === 'string') {
          const parsedDate = new Date(date);
          return isNaN(parsedDate.getTime()) ? null : parsedDate;
        }
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
      const reqbody = {
        EmpId: this.storeAddedEmployeeDetails.EmpId,
        LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        DocId: this.govtselectedID,
        // Name: this.governmentForm?.get('govt_name').value ? this.governmentForm?.get('govt_name').value : '',
        DocName: this.govtselectedName,
        DocNo: this.governmentForm?.get('govt_doc_num').value,
        IssuedDate: fromOnly ? fromOnly : '',
        ExpiredDate: toOnly ? toOnly : '',
        Description: this.governmentForm?.get('govt_description').value,
        Others: this.governmentForm?.get('Others').value ? this.governmentForm?.get('Others').value : '',
        Path: this.getGovtDoc ? this.getGovtDoc : ''
      }
      console.log(reqbody);
      this.isSpinner = true;
      this.hrmsService.employeeAddGovtDoc(reqbody).subscribe((res: any) => {
        if (res['msg'] == 'Added') {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.getGetGovtDoc();
          this.resetGovtDoc();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error, 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
    } else {
      this.triggerToast('', 'Please Fill All Details', "warning");
    }
  }
  editgovtData(data: any) {
    console.log(data);
    this.isGovtUpdateButton = true;
    // Function to convert date from 'dd-MM-yyyy' to 'yyyy-MM-dd'
    const convertToDateInputFormat = (dateStr: string): string => {
      if (!dateStr) return '';
      const [day, month, year] = dateStr.split('-').map(Number);
      return `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
    };
    this.governmentForm?.patchValue({
      // govt_name: data?.Name,
      gov_doc_name: data?.DocId,
      govt_doc_num: data?.DocNo,
      govt_description: data?.Description,
    });
    this.patchGovtData = data;
    const govtPath = this.patchGovtData?.Path.replace(/\\/g, "\\\\");
    this.patchGovtPath = `${this.baseUrl}/${govtPath}`;
    // Set date values correctly
    if (data['DocName'] === 'Driving License') {
      this.isGovtVisibleDates = true;
      this.governmentForm?.get('date_from').patchValue(data?.IssuedDate ? convertToDateInputFormat(data.IssuedDate) : null);
      this.governmentForm?.get('date_to').patchValue(data?.ExpiredDate ? convertToDateInputFormat(data.ExpiredDate) : null);
    } else if (data['DocName'] === 'Others') {
      this.isGovtVisibleDates = true;
      this.isViewGovtOthers = true;
      this.governmentForm?.get('date_from').patchValue(data?.IssuedDate ? convertToDateInputFormat(data.IssuedDate) : null);
      this.governmentForm?.get('date_to').patchValue(data?.ExpiredDate ? convertToDateInputFormat(data.ExpiredDate) : null);
      this.governmentForm?.get('Others').patchValue(data?.Others);
    } else {
      this.isGovtVisibleDates = false;
      this.isViewGovtOthers = false;
    }
  }
  isGovtPreset() {
    if (this.patchGovtData.Path === '') {
      if (this.isGovtUploaded) {
        return this.getGovtDoc
      } else {
        return ''
      }
    } else {
      return this.patchGovtPath;
    }
  }
  updateGovtForm() {
    this.isFormSubmitted = true;
    if (this.governmentForm?.valid) {
      const reqBody = {
        // EmpId: this.employeeDetails[0].EmpId ? this.employeeDetails[0].EmpId : '',
        // LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
        LoginId: this.employeeDetails[0].LoginId,
        GovId: this.patchGovtData?.GovId,
        DocId: this.patchGovtData?.DocId,
        // Others: this.patchGovtData.Others ? this.patchGovtData.Others : this.governmentForm?.get('Others').value,
        // Name: this.patchGovtData.Name ? this.patchGovtData.Name : this.governmentForm?.get('govt_name').value,
        // DocName: this.patchGovtData.DocName ? this.patchGovtData.DocName : this.governmentForm?.get('gov_doc_name').value,
        // DocNo: this.patchGovtData.DocNo ? this.patchGovtData.DocNo : this.governmentForm?.get('govt_doc_num').value,
        // IssuedDate: this.patchGovtData.IssuedDate ? this.patchGovtData.IssuedDate : this.governmentForm?.get('date_from').value,
        // ExpiredDate: this.patchGovtData.ExpiredDate ? this.patchGovtData.ExpiredDate : this.governmentForm?.get('date_to').value,
        // Description: this.patchGovtData.Description ? this.patchGovtData.Description : this.governmentForm?.get('govt_description').value,
        // Path: this.patchGovtPath ? this.patchGovtPath : this.getGovtDoc,
        Others: this.governmentForm?.get('Others').value ? this.governmentForm?.get('Others').value : this.patchGovtData.Others,
        // Name: this.governmentForm?.get('govt_name').value ? this.governmentForm?.get('govt_name').value : this.patchGovtData.Name,
        DocName: this.govtselectedName ? this.govtselectedName : this.patchGovtData.DocName,
        DocNo: this.governmentForm?.get('govt_doc_num').value ? this.governmentForm?.get('govt_doc_num').value : this.patchGovtData.DocNo,
        IssuedDate: this.governmentForm?.get('date_from').value ? this.governmentForm?.get('date_from').value : this.patchGovtData.IssuedDate,
        ExpiredDate: this.governmentForm?.get('date_to').value ? this.governmentForm?.get('date_to').value : this.patchGovtData.ExpiredDate,
        Description: this.governmentForm?.get('govt_description').value ? this.governmentForm?.get('govt_description').value : this.patchGovtData.Description,
        Path: this.isGovtPreset(),
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsService.employeeUpdateGovtDoc(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast('', 'Records Updated Successfully', 'success');
          this.isSpinner = false;
          this.getGetGovtDoc();
          this.resetGovtDoc();
        } else if (res['Message']) {
          this.triggerToast('Something Went Wrong', res['Message'], 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Failed To Update Records', "danger");
        this.isSpinner = false;
      })
    }
  }
  deleteGovtDoc(data: any) {
    console.log(data);
    this.getGovtTableDeleteId = data.GovId;
  }
  deleteGovtDocument() {
    const reqBody = {
      // LoginId: this.employeeDetails[0].LoginId,
      // EmpId: this.employeeDetails[0].EmpId,
      EmpId: this.storeAddedEmployeeDetails.EmpId, //this is storeAddedEmployeeDetails API  
      LoginId: this.employeeDetails[0].LoginId,
      GovId: this.getGovtTableDeleteId
    }
    console.log(reqBody);
    this.isSpinner = true;
    this.hrmsService.employeeUpdateDeleteDoc(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getGetGovtDoc();
        this.isSpinner = false;
      } else if (res['Message']) {
        this.triggerToast('Something Went Wrong', 'Please Try Again', 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To delete The Record', 'danger');
      this.isSpinner = false;
    })
  }
  resetGovtDoc() {
    this.governmentForm.reset();
    this.isGovtUpdateButton = false;
    this.imageSrcGovtDoc = null;
    this.fileNameGovtDoc = null;
    this.minDateGovt = undefined;
    this.maxDateGovt = undefined;
    this.getGovtDoc = null;
    this.governmentForm?.get('gov_photo').reset();
    this.isFormSubmitted = false;
  }
  // ***************Government .ts Ends***********************

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
  preventKeyboardInput(event: KeyboardEvent) {
    event.preventDefault(); // Prevents any keyboard input
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault(); // Prevents paste input
  }
  // private truncateFileName(name: string, maxLength: number): string {
  //   if (name.length > maxLength) {
  //     return name.substring(0, maxLength) + '...';
  //   }
  //   return name;
  // }

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

}
