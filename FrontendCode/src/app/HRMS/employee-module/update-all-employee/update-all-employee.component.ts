import { ChangeDetectorRef, Component, HostListener, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { EmployeeModuleService } from '../../service/employee.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HrmsServiceService } from '../../hrms-service.service';
import { environment } from 'src/assets/environment';
import { trigger, style, animate, transition } from '@angular/animations';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { EntityStateService } from '../../service/entity-state.service';

@Component({
  selector: 'app-update-all-employee',
  standalone: true,
  imports: [ToastMessageComponent, CommonModule, SharedModule, ReactiveFormsModule, RouterModule],
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
  templateUrl: './update-all-employee.component.html',
  styleUrl: './update-all-employee.component.scss'
})
export class UpdateAllEmployeeComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;
  @ViewChild('closeModalEduc') closeModalEduc!: ElementRef;
  @ViewChild('closeModalGovt') closeModalGovt!: ElementRef;

  baseUrl: string = environment.baseUrl;
  isSpinner: boolean = false;
  employeeDetails;
  isFormSubmitted: boolean = false;
  isRecordDeleted: boolean = false;
  updateEmployeeAccess: any;
  accessPolicy: any;
  today = new Date().toISOString().split('T')[0]; // Format the date as YYYY-MM-DD
  constructor(private readonly fb: FormBuilder,
    private readonly hrmsEmployeeModuleService: EmployeeModuleService,
    public readonly fromQueryParams: ActivatedRoute,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly cdr: ChangeDetectorRef, private readonly route: Router,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private entityStateService: EntityStateService
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.updateEmployeeAccess = this.accessPolicy.find(
        (item: any) => item.PageName === 'Department List'
      );
    });
  }

  ngOnInit(): void {
    this.retryQueryParams();
    this.basicdetailsFormval();

    this.getEmployeeDeatils();

    // this.accountFormVal();
  }

  isOpen = [true, false, false, false, false, false];
  // togglePanel(index: number): void {
  //   this.isOpen[index] = !this.isOpen[index];
  // }

  togglePanel(index: number): void {
    const wasOpen = this.isOpen[index];
    this.isOpen[index] = !wasOpen;

    if (!wasOpen) {
      this.loadPanelData(index);
    }
  }

  loadPanelData(index: number): void {
    switch (index) {
      case 0:
        break;
      case 1:
        this.contactInformationFormVal();
        break;
      case 2:
        this.careerFormVal();
        break;
      case 3:
        this.educationFormval();
        break;
      case 5:
        this.governmentFormval();
        break;
    }
  }
  // onEditClick(event: Event): void {
  //   event.stopPropagation();
  // }
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
  preventKeyboardInput(event: KeyboardEvent) {
    event.preventDefault(); // Prevents any keyboard input
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault(); // Prevents paste input
  }
  handleAlphaChar(event: any) {
    if (
      (event.charCode > 32 && event.charCode < 48) ||
      (event.charCode > 57 && event.charCode < 127)
    ) {
      event.preventDefault();
    }
  }

  // ***************Basic details Top variable Valus starts************************
  basicDetailsForm: any = FormGroup;
  basicDetailsUploadedImg: string | ArrayBuffer | null = null;
  basicDetailsSelectedFile: File | null = null;
  getDDCompany: any;
  getLegalEntity: any;
  getLegalEntityAuthourized: any;
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
  isPermanent: boolean = false;
  storeAddedEmployeeDetails: any;
  isEmpIdCreated: boolean = false;
  errorMessageBasic: any;
  currentEmployeeDetails: any;
  storeQueryParamsData: any;
  isBasicDetailsUpdateButton: boolean = false;
  getProfilePhoto: any;
  isProfilePhotoUploded: boolean = false;
  isValidPhoto: any;
  patchPhotoUrl: any;
  getEntityName: any;
  getEmployeeType: any;
  // ***************Basic details Top variable Valus Ends************************

  // ***************Contact Information Top variable Valus starts************************
  contactInfoForm: any = FormGroup;
  getemployeeGetContactDetails: any;
  isContactDetailsUpdateButton: boolean = false;
  // ***************Contact Information Top variable Valus starts************************

  // ***************Career Details Top variable Valus starts************************
  careerForm: any = FormGroup;
  getEmpCareerDetailsRows: any[] = [];
  isCareerUpdateButton: boolean = false;
  patchCareerData: any;
  getCareerTableDeleteId: any
  showErrorCareer = false; // To control the display of error message
  errorMessageCareer: string = ''; // To store error or no data message
  minDateCareer: string | undefined;
  maxDateCareer: string | undefined;
  monthHeaders: string[] = [];
  dynamicHeaders: any = {};

  // Variables for Offer Letter
  offerLetterSrc: string | ArrayBuffer | null = null;
  offerLetterPath: string | null = null;
  offerLetterName: string | null = null;
  isOfferLetterImage = false;
  offerLetterSelectedFile: File | null = null;
  getOfferLetter: any
  patchOfferLatter: any
  isOfferLatterUploaded = false;

  // Variables for Salary Letter
  salaryLetterSrc: string | ArrayBuffer | null = null;
  salaryLetterPath: string | null = null;
  salaryLetterName: string | null = null;
  isSalaryLetterImage = false;
  salaryLetterSelectedFile: File | null = null;
  getSalaryLetter: any
  patchExperienceLetter: any;
  isSalaryLatterUploaded: boolean = false;

  // Variables for Experience/Relieving Letter
  experienceLetterSrc: string | ArrayBuffer | null = null;
  experienceLetterPath: string | null = null;
  experienceLetterName: string | null = null;
  isExperienceLetterImage = true;
  experienceLetterSelectedFile: File | null = null;
  getExperienceLetter: any
  patchSalaryLetter: any
  isExperienceLatterUploaded: boolean = false;

  previewUrls: string[] = []; // To hold preview URLs for images
  fileNames: string[] = []; // To store file names
  uploadStatus: string[] = []; // To hold status messages for each upload
  fileUploads: { [key: string]: string } = {}; // To store file paths/identifiers
  maxSets = 3; // Maximum number of form groups allowed
  isValidPhotoSalary: any;
  isValidPhotoOffer: any;
  isValidPhotoExperience: any;
  // ***************Career Details Top variable Valus Finished************************

  // ***************Education details Top variable Valus Starts************************
  EducationForm: any = FormGroup;
  imageSrcEducDoc: string | ArrayBuffer | null = null;
  filePathEducDoc: string | null = null;
  fileNameEducDoc: string | null = null;
  isImageEducDoc = false;
  SelectedFileEducDoc: File | null = null;
  getEducDoc: any;
  patchEducDoc: any
  educselectedID: any;
  educselectedName: any;
  patchEducPath: any;
  getEducTableDeleteId: any;
  isShowEducOthers: boolean = false
  educationDetailsRows: any[] = [];
  isEducUpdateButton: boolean = false;
  educationDocName: any;
  pathchEducationData: any;
  minDateEducation: string | undefined;
  maxDateEducation: string | undefined;
  isEducationUploaded: boolean = false
  // ***************Education details Top variable Valus Finished************************

  // ***************Account Details Top variable Valus Starts************************
  accountForm: any = FormGroup;
  pathchAccountDetails: any;
  isUpdateAccountDetails: boolean = false;
  // ***************Account Details Top variable Valus Ends************************

  // ***************Government Details Top variable Valus Starts************************
  governmentForm: any = FormGroup;
  imageSrcGovtDoc: string | ArrayBuffer | null = null;
  filePathGovtDoc: string | null = null;
  fileNameGovtDoc: string | null = null;
  isImageGovtDoc = false;
  SelectedFileGovtDoc: File | null = null;
  getGovtDoc: any;
  patchGovtDoc: any
  govtselectedID: any;
  govtselectedName: any;
  patchGovtData: any;
  patchGovtPath: any;
  showErrorGovt = false; // To control the display of error message
  errorMessageGovt: string = ''; // To store error or no data message
  isGovtUpdateButton: boolean = false;
  isViewGovtOthers: boolean = false;
  getGovtTableDeleteId: any;
  govtDocName: any;
  govtDetailsRows: any[] = [];
  minDateGovt: string | undefined;
  maxDateGovt: string | undefined;
  isGovtUploaded: boolean = false;
  isGovtVisibleDates: boolean = false;
  // ***************Government Details Top variable Valus Ends************************

  // ***************Basic details Starts************************
  basicdetailsFormval() {
    this.basicDetailsForm = this.fb.group({
      Company: ['', [Validators.required]],
      LegalEntity: ['', [Validators.required]],
      BusinessUnit: ['', [Validators.required]],
      Location: ['', [Validators.required]],
      DeptName: ['', [Validators.required]],
      Designation: ['', [Validators.required]],
      EmpCode: ['', [Validators.required]],
      basic_salutation: [],
      FirstName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      MiddleName: [''],
      LastName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(1), Validators.maxLength(50)]],
      DOB: ['', [Validators.required]],
      MobileNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      EmailId: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
      BloodGroup: ['', [Validators.required]],
      MaritalStatus: ['', [Validators.required]],
      Gender: ['', [Validators.required]],
      JoiningDate: ['', [Validators.required]],
      basic_photo: [],
      employeeType: [''],
      authorisedEntity: [[]],
      approverPerson: [''],
      contractEndDate: [''],
      InterviewDate: [''],
      Probation: [],
      ProbationConfirmationStatus: [],
      ProbationConfirmationEffectiveDate: [],
      ProbationConfirmationDate: [],
      ProbationRemark: [],
    }, { validator: this.dateComparisonValidator() });
    this.basicDetailsForm.disable();
    this.basicDetailsForm.get('authorisedEntity').disable();
    this.basicDetailsForm?.get('employeeType').valueChanges.subscribe((res: any) => {
      if (res === 3 || res === 'Contract') {
        this.isContractEnd = true;
        this.basicDetailsForm?.get('contractEndDate').setValidators([Validators.required]);
      } else {
        this.isContractEnd = false;
        this.basicDetailsForm.get('contractEndDate').clearValidators();
        this.basicDetailsForm.get('contractEndDate')?.updateValueAndValidity();
      }
    });
  }

  convertToUppercase(event: any, controlName: string) {
    const value = event.target.value.toUpperCase();
    this.basicDetailsForm?.get(controlName)?.setValue(value, { emitEvent: false });
    this.contactInfoForm?.get(controlName)?.setValue(value, { emitEvent: false });
  }

  isEnableBusiness(event: any) {
    const selectElement = event.target as HTMLSelectElement;
    this.getEntityName = selectElement.options[selectElement.selectedIndex].text;
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
      this.isPermanent = false;
      this.basicDetailsForm?.get('contractEndDate').setValidators([Validators.required]);
    } else if (this.getEmployeeType === 'Permanent') {
      this.isPermanent = true;
      this.isContractEnd = false;
      this.basicDetailsForm.get('Probation')?.setValue(this.currentEmployeeDetails?.IsProbation);
    } else {
      this.isContractEnd = false;
      this.isPermanent = false;
      this.basicDetailsForm.get('contractEndDate').clearValidators();
      this.basicDetailsForm.get('contractEndDate')?.updateValueAndValidity();
    }
  }
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
  onEditClickBasicDetails(event: Event): void {
    event.stopPropagation();
    this.isBasicDetailsUpdateButton = true;
    this.basicDetailsForm.enable();
    // console.log(this.updateEmployeeAccess.PageName === 'Edit Employee HR' && this.updateEmployeeAccess.UpdateAccess === true );
    if (this.updateEmployeeAccess != undefined) {
      if (this.updateEmployeeAccess?.PageName === 'Edit Employee HR') {
        this.basicDetailsForm?.get('Company').enable();
        this.basicDetailsForm?.get('EmpCode').enable();
        this.basicDetailsForm?.get('DOB').enable();
        this.basicDetailsForm?.get('EmailId').enable();
        this.basicDetailsForm?.get('JoiningDate').enable();
        this.basicDetailsForm?.get('LegalEntity').enable();
        this.basicDetailsForm?.get('BusinessUnit').enable();
        this.basicDetailsForm?.get('Location').enable();
      }
    } else if (this.updateEmployeeAccess === undefined || this.updateEmployeeAccess === null) {
      this.basicDetailsForm?.get('EmpCode').disable();
      this.basicDetailsForm?.get('EmailId').disable();
      this.basicDetailsForm?.get('JoiningDate').disable();
      this.basicDetailsForm?.get('DOB').disable();
      this.basicDetailsForm?.get('Company').disable();
      this.basicDetailsForm?.get('LegalEntity').disable();
      this.basicDetailsForm?.get('BusinessUnit').disable();
      this.basicDetailsForm?.get('Location').disable();
    }

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
    this.hrmsEmployeeModuleService.employeeUploadImage(this.employeeDetails[0].EmpId, this.basicDetailsSelectedFile).subscribe((res: any) => {
      if (res['msg']) {
        this.getProfilePhoto = res.path;
        this.isProfilePhotoUploded = true
        this.isSpinner = false;
        this.triggerToast(res['msg'], 'Profile Picture Uploaded', "success");
      } else if (res['Message']) {
        this.isSpinner = false;
        this.triggerToast(res['Message'], res['Message'], "warning");
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Error to upload Basic detials Photo', "danger");
      this.isSpinner = false;
    })
  }


  // ***************RetryQueryParams .ts Starts***********************
  // parseJsonDate(jsonDate: string): Date | null {
  //   const match = /\/Date\((\d+)\)\//.exec(jsonDate);
  //   if (match) {
  //     return new Date(parseInt(match[1], 10));
  //   }
  //   return null;
  // }
  parseJsonDate(jsonDate: string): Date | null {
    if (!jsonDate) return null;

    const match = /\/Date\((-?\d+)\)\//.exec(jsonDate); // ✅ allow negative

    if (match) {
      return new Date(Number(match[1]));
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
  retryQueryParams() {
    // this.fromQueryParams.queryParams.subscribe(res => {
    //   this.storeQueryParamsData = res
    // });
    this.fromQueryParams.queryParams.subscribe(params => {
      const empId = params['EmpId'];
      if (!empId) {
        this.route.navigate(['access_denied'])
      } else {
        this.storeQueryParamsData = params
      }
    });
  };
  getPhotoPath(): string {
    if (this.currentEmployeeDetails.Photo) {
      // Current photo exists
      return this.isProfilePhotoUploded ? this.getProfilePhoto : this.patchPhotoUrl;
    } else {
      // No current photo
      return this.isProfilePhotoUploded ? this.getProfilePhoto : '';
    }
  }



  // This is the Second code of Employee Name
  isDropdownOpen = false;
  filteredEmployees: any[] = [];
  employees: any[] = [];
  searchText: string = '';
  selectedEmpId: any;
  isValidEmployee: boolean = true;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
    if (this.isDropdownOpen) {
      this.filteredEmployees = [...this.employees];
    }
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
        employee.Approver.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employees];
    }
  }

  selectEmployee(employee: any) {
    this.searchText = employee.Approver;
    this.selectedEmpId = employee.ApproverId;
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }

  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.Approver.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.basicDetailsForm.get('approverPerson')?.setErrors({ invalidEmployee: true });
    } else {
      this.basicDetailsForm.get('approverPerson')?.setErrors(null);
    }

  }
  // This is the Second code of Employee Name

  //this is for entity multi select option
  callAuthourizedEntity() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: 0
    }
    this.hrmsEmployeeModuleService.employeeDDAuthorisedEntity(reqBody).subscribe({
      next: (res: any) => {
        this.getLegalEntityAuthourized = res;
      }, error: (err: any) => {

      }
    })
  }

  bcaDropdownOpen = false;

  @ViewChild('dropdownContainer') dropdownContainer!: ElementRef;

  // Toggle dropdown
  bcaToggleDropdown(event: MouseEvent) {
    event.stopPropagation(); // Prevent document click from closing immediately
    this.bcaDropdownOpen = !this.bcaDropdownOpen;
  }

  // Close dropdown when clicking outside
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (this.bcaDropdownOpen && !this.dropdownContainer.nativeElement.contains(event.target)) {
      this.bcaDropdownOpen = false;
    }
  }

  // Handle checkbox changes
  bcaOnEntityChange(entity: any) {
    const control = this.basicDetailsForm.get('authorisedEntity');
    let value = control?.value;

    if (!Array.isArray(value)) {
      value = [];
    }

    if (value.includes(entity.LEId)) {
      control?.setValue(value.filter((id: any) => id !== entity.LEId));
    } else {
      control?.setValue([...value, entity.LEId]);
    }
  }



  // Check if an entity is selected
  bcaIsSelected(id: any): boolean {
    return this.basicDetailsForm.get('authorisedEntity')?.value?.includes(id);
  }

  // Display selected entity names
  get bcaSelectedEntityNames(): string {
    const selectedIds = this.basicDetailsForm.get('authorisedEntity')?.value || [];
    return (this.getLegalEntityAuthourized || [])
      .filter((e: any) => selectedIds.includes(e.LEId))
      .map((e: any) => e.LegalEntity)
      .join(', ');
  }


  //this is for entity multi select option

  // getEmployeeDeatils() {
  //   const reqBody = {
  //     LoginId: this.employeeDetails[0].EmpId,
  //     EmpId: Number(this.storeQueryParamsData.EmpId),
  //   }
  //   this.isSpinner = true
  //   this.hrmsEmployeeModuleService.employeeGetEmployee(reqBody).subscribe((res: any) => {
  //     this.currentEmployeeDetails = res;
  //     this.employee_DD_Company();
  //     this.calllegalEntity();
  //     this.getBusinessUnit();
  //     this.callLocation();
  //     this.access_DD_department();
  //     this.callApproverData();
  //     this.callDDDesignation();
  //     this.callAuthourizedEntity()
  //     this.getDDSalutationList();
  //     this.getDDGenderList();
  //     this.getDDEmpTypeList();

  //     if (res['msg'] == null || undefined) {
  //       const dob = this.currentEmployeeDetails.DOB ? this.parseJsonDate(this.currentEmployeeDetails.DOB) : null;
  //       const joiningDate = this.currentEmployeeDetails.JoiningDate ? this.parseJsonDate(this.currentEmployeeDetails.JoiningDate) : null;
  //       const endofdate = this.currentEmployeeDetails.CEndDate ? this.parseJsonDate(this.currentEmployeeDetails.CEndDate) : null;
  //       const formattedDOB = this.formatDate(dob);
  //       const formattedJoiningDate = this.formatDate(joiningDate);
  //       const formatedEndOfDate = this.formatDate(endofdate);

  //       const photo = this.currentEmployeeDetails.Photo;
  //       this.isValidPhoto = photo !== null && photo !== undefined && photo !== '';
  //       const getPhotoUrl = this.currentEmployeeDetails?.Photo || '';
  //       const normalizedPath = getPhotoUrl.replace(/\\/g, '/');
  //       this.patchPhotoUrl = `${this.baseUrl}/${normalizedPath}`;

  //       this.basicDetailsForm?.patchValue({
  //         FirstName: this.currentEmployeeDetails?.FirstName ? this.currentEmployeeDetails?.FirstName : '',
  //         MiddleName: this.currentEmployeeDetails?.MiddleName ? this.currentEmployeeDetails?.MiddleName : '',
  //         LastName: this.currentEmployeeDetails?.LastName ? this.currentEmployeeDetails?.LastName : '',
  //         EmpCode: this.currentEmployeeDetails?.EmpCode ? this.currentEmployeeDetails?.EmpCode : '',
  //         DOB: formattedDOB ? formattedDOB : '',
  //          MobileNo: this.currentEmployeeDetails?.MobileNo ? this.currentEmployeeDetails?.MobileNo : '',
  //         EmailId: this.currentEmployeeDetails?.EmailId ? this.currentEmployeeDetails?.EmailId : '',
  //         BloodGroup: this.currentEmployeeDetails?.BloodGroup ? this.currentEmployeeDetails?.BloodGroup : '',
  //         JoiningDate: formattedJoiningDate ? formattedJoiningDate : '',
  //         contractEndDate: formatedEndOfDate ? formatedEndOfDate : '',
  //         MaritalStatus: this.currentEmployeeDetails?.MaritalStatus ? this.currentEmployeeDetails?.MaritalStatus : '',


  //         Company: this.currentEmployeeDetails?.CompId,
  //         LegalEntity: this.currentEmployeeDetails?.LEId,
  //         BusinessUnit: this.currentEmployeeDetails?.BUId,
  //         Location: this.currentEmployeeDetails?.LocationId,
  //         basic_salutation: this.currentEmployeeDetails?.SalutationId,
  //         DeptName: this.currentEmployeeDetails?.DeptId ? this.currentEmployeeDetails?.DeptId : '',
  //         Designation: this.currentEmployeeDetails?.DesignationId,
  //         Gender: this.currentEmployeeDetails?.Gender ? this.currentEmployeeDetails?.Gender : '',
  //         approverPerson: this.currentEmployeeDetails?.Approver,
  //         employeeType: this.currentEmployeeDetails?.EmpTypeId,
  //         authorisedEntity: this.currentEmployeeDetails?.AuthorisedEntity
  //           ? this.currentEmployeeDetails.AuthorisedEntity
  //             .split(',')
  //             .map((id: string) => Number(id))
  //           : [],
  //       });
  //       this.cdr.detectChanges();
  //       setTimeout(() => {
  //         this.basicDetailsForm?.get('LegalEntity').patchValue(this.currentEmployeeDetails?.LEId)
  //         this.basicDetailsForm?.get('BusinessUnit').patchValue(this.currentEmployeeDetails?.BUId)
  //         setTimeout(() => {
  //           this.basicDetailsForm?.get('Location').patchValue(this.currentEmployeeDetails?.LocationId)
  //         }, 900)
  //       }, 800);

  //       this.isSpinner = false
  //     } else if (res['Message']) {
  //       this.triggerToast('', res['Message'], "warning");
  //       this.isSpinner = false;
  //     }
  //     else {
  //       this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
  //       this.isSpinner = false;
  //     }
  //   }, error => {
  //     this.errorMessageBasic = 'Error loading data. Please try again.';
  //     this.triggerToast('Internal Server Error', 'To Load The Basic Details', "danger");
  //     this.isSpinner = false;
  //   })
  // }
  getEmployeeDeatils() {
    const reqBody = {
      LoginId: this.employeeDetails[0].EmpId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
    };
    this.isSpinner = true;

    this.hrmsEmployeeModuleService.employeeGetEmployee(reqBody).subscribe((res: any) => {
      this.currentEmployeeDetails = res;
      this.isPermanent = res.EmpType === 'Permanent';
      // Patch basic input fields immediately
      const dob = res.DOB ? this.formatDate(this.parseJsonDate(res.DOB)) : '';
      const joiningDate = res.JoiningDate ? this.formatDate(this.parseJsonDate(res.JoiningDate)) : '';
      const endOfDate = res.CEndDate ? this.formatDate(this.parseJsonDate(res.CEndDate)) : '';
      const InterviewDate = res.InterviewDate ? this.formatDate(this.parseJsonDate(res.InterviewDate)) : '';

      this.basicDetailsForm.patchValue({
        FirstName: res.FirstName || '',
        MiddleName: res.MiddleName || '',
        LastName: res.LastName || '',
        EmpCode: res.EmpCode || '',
        DOB: dob,
        JoiningDate: joiningDate,
        InterviewDate: InterviewDate,
        contractEndDate: endOfDate,
        // Probation:res.IsProbation,
        MobileNo: res.MobileNo || '',
        EmailId: res.EmailId || '',
        BloodGroup: res.BloodGroup || '',
        MaritalStatus: res.MaritalStatus || '',
        Gender: res.Gender || '',
        approverPerson: res.Approver || '',
        authorisedEntity: res.AuthorisedEntity ? res.AuthorisedEntity.split(',').map((id: string) => Number(id)) : [],
      });
      // Patch the photo immediately
      const photo = res.Photo || '';
      this.isValidPhoto = !!photo;
      this.patchPhotoUrl = photo ? `${this.baseUrl}/${photo.replace(/\\/g, '/')}` : '';
      this.employee_DD_Company(); // company dropdown

      setTimeout(() => {
        this.calllegalEntity(); // legal entity depends on company
        setTimeout(() => {
          this.getBusinessUnit(); // business unit depends on legal entity
          setTimeout(() => {
            this.callLocation(); // location depends on business unit
            setTimeout(() => {
              this.access_DD_department(); // department
              setTimeout(() => {
                this.callApproverData(); // approver depends on company/legal/bu/location
                setTimeout(() => {
                  this.callDDDesignation(); // designation depends on department
                  setTimeout(() => {
                    this.callAuthourizedEntity(); // authorised entity
                    setTimeout(() => {
                      this.getDDSalutationList();
                      setTimeout(() => {
                        this.getDDGenderList();
                        setTimeout(() => {
                          this.getDDEmpTypeList();
                          // finally patch dropdown values after all APIs finish
                          this.basicDetailsForm.patchValue({
                            Company: this.currentEmployeeDetails.CompId,
                            LegalEntity: this.currentEmployeeDetails.LEId,
                            BusinessUnit: this.currentEmployeeDetails.BUId,
                            Location: this.currentEmployeeDetails.LocationId,
                            DeptName: this.currentEmployeeDetails.DeptId,
                            Designation: this.currentEmployeeDetails.DesignationId,
                            basic_salutation: this.currentEmployeeDetails.SalutationId,
                            employeeType: this.currentEmployeeDetails.EmpTypeId,
                            Probation: this.currentEmployeeDetails.IsProbation,
                            ProbationConfirmationStatus: this.currentEmployeeDetails.ProbationConfirmationStatus,
                            ProbationConfirmationEffectiveDate: this.currentEmployeeDetails.ProbationConfirmationEffectiveDate
                              ? this.formatDate(
                                new Date(
                                  this.currentEmployeeDetails.ProbationConfirmationEffectiveDate
                                    .split('-')
                                    .reverse()
                                    .join('-')
                                )
                              )
                              : ''
                          });
                          this.cdr.detectChanges();
                          this.isSpinner = false;
                        }, 200);
                      }, 200);
                    }, 200);
                  }, 200);
                }, 200);
              }, 200);
            }, 200);
          }, 200);
        }, 200);
      }, 200);

    }, error => {
      this.errorMessageBasic = 'Error loading data. Please try again.';
      this.triggerToast('Internal Server Error', 'To Load The Basic Details', "danger");
      this.isSpinner = false;
    });
  }
  // starts api all
  employee_DD_Company() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.NewDDCompany(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDCompany = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        this.isSpinner = false;
      }
    },
      error => {
        this.errorMessageBasic = 'Error loading data. Please try again.';
        this.triggerToast('Internal Server Error', 'Error loading Company Name', "danger");
        this.isSpinner = false;
      });
  }
  calllegalEntity() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: this.currentEmployeeDetails?.CompId ? this.currentEmployeeDetails?.CompId : Number(this.basicDetailsForm?.get('Company').value)
    }
    this.isSpinner = true;
    setTimeout(() => {
      this.hrmsEmployeeModuleService.NewDDLegalEntity(reqBody).subscribe((res: any) => {
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
          this.triggerToast('Internal Server Error', 'Error loading Legal Entity', "danger");
          this.isSpinner = false;
        })
    }, 100)
  }
  getBusinessUnit() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: this.currentEmployeeDetails?.CompId ? this.currentEmployeeDetails?.CompId : Number(this.basicDetailsForm?.get('Company').value),
      LEId: this.currentEmployeeDetails?.LEId ? this.currentEmployeeDetails?.LEId : Number(this.basicDetailsForm?.get('LegalEntity').value),
    }
    this.isSpinner = true;
    this.getBusinessUnitlist = []
    setTimeout(() => {
      this.hrmsEmployeeModuleService.NewDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.getBusinessUnitlist = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Business Unit", "warning");
          this.isSpinner = false;
          this.getBusinessUnitlist = [];
          this.getLocations = []
        }
      },
        error => {
          this.errorMessageBasic = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading Business Unit', "danger");
          this.isSpinner = false;
        })
    }, 100)
  }
  callApproverDataEntity() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: this.basicDetailsForm?.get('Company').value,
      LEId: this.basicDetailsForm?.get('LegalEntity').value,
      BUId: 0,
      LocationId: 0,
    }
    this.isSpinner = true;
    setTimeout(() => {
      this.hrmsEmployeeModuleService.employeeDDApprover(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.isSpinner = false;
        } else {
          this.isSpinner = false;
          this.employees = []
        }
      },
        error => {
          this.triggerToast('Internal Server Error', 'Error loading Approver', "danger");
          this.isSpinner = false;
        })
    }, 100)
  }
  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: this.currentEmployeeDetails?.CompId ? this.currentEmployeeDetails?.CompId : Number(this.basicDetailsForm?.get('Company').value),
      LEId: this.currentEmployeeDetails?.LEId ? this.currentEmployeeDetails?.LEId : Number(this.basicDetailsForm?.get('LegalEntity').value),
      BUId: this.currentEmployeeDetails?.BUId ? this.currentEmployeeDetails?.BUId : Number(this.basicDetailsForm?.get('BusinessUnit').value),
    }
    this.isSpinner = true;
    this.getLocations = [];
    setTimeout(() => {
      this.hrmsEmployeeModuleService.NewDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.getLocations = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = []
        }
      },
        error => {
          this.errorMessageBasic = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading Location', "danger");
          this.isSpinner = false;
        })
    }, 100)
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
    setTimeout(() => {
      this.hrmsEmployeeModuleService.employeeDDApprover(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Approver", "warning");
          this.isSpinner = false;
          this.employees = []
        }
      },
        error => {
          this.errorMessageBasic = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading Approver', "danger");
          this.isSpinner = false;
        })
    }, 100)
  }
  callDDDesignation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.basicDetailsForm?.get('DeptName')?.value || this.currentEmployeeDetails?.DeptId
    };
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe((res: any) => {
      this.getDepartementRole = res;
      setTimeout(() => {
        this.basicDetailsForm.get('Designation')?.setValue(this.currentEmployeeDetails?.DesignationId);
      }, 100);
      // ✅ patch Designation after dropdown options are loaded
      const designationId = this.currentEmployeeDetails?.DesignationId;
      if (designationId) {
        const exists = this.getDepartementRole.some(d => Number(d.DesignationId) === Number(designationId));
        if (exists) {
          this.basicDetailsForm.get('Designation')?.setValue(Number(designationId));
        }
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'Error loading Designation', "danger");
      this.isSpinner = false;
    });
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

  getDDSalutationList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeDDSalutation(reqBody).subscribe((res: any) => {
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
        // this.errorMessageBasic = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Select List', "danger");
        this.isSpinner = false;
      })
  }
  getDDGenderList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeDDGender(reqBody).subscribe((res: any) => {
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
        // this.errorMessageBasic = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Gender List', "danger");
        this.isSpinner = false;
      })
  }
  getDDEmpTypeList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeDDEmpType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getEmployeeTypeList = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Employee TypeList", "warning");
        this.isSpinner = false;
        this.getEmployeeTypeList = []
      }
    },
      error => {
        this.errorMessageBasic = 'Error loading data. Please try again.';
        this.triggerToast('Internal Server Error', 'Error loading Employee TypeList', "danger");
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
      this.triggerToast('Internal Server Error', 'Error loding Department List', 'danger');
      this.isSpinner = false;
    })
  }
  callApproverData() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      CompId: this.currentEmployeeDetails.CompId ? this.currentEmployeeDetails?.CompId : this.basicDetailsForm?.get('Company').value,
      LEId: this.currentEmployeeDetails?.LEId ? this.currentEmployeeDetails?.LEId : this.basicDetailsForm?.get('LegalEntity').value,
      BUId: this.currentEmployeeDetails?.BUId ? this.currentEmployeeDetails?.BUId : this.basicDetailsForm?.get('BusinessUnit').value,
      LocationId: this.currentEmployeeDetails?.LocationId ? this.currentEmployeeDetails?.LocationId : this.basicDetailsForm?.get('Location').value,
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeDDApprover(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.employees = res;
        this.isSpinner = false;
      } else {
        // this.triggerToast(res['Message'], "No Data Found For Approver Person", "warning");
        this.isSpinner = false;
        this.employees = []
      }
    },
      error => {
        // this.  errorMessageBasic = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading Approver', "danger");
        this.isSpinner = false;
      })
  }

  updateBasicDetials() {
    this.isFormSubmitted = true;
    const authorisedEntityValue = this.basicDetailsForm.get('authorisedEntity')?.value;
    if (this.basicDetailsForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].EmpId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
        Password: this.currentEmployeeDetails.Password ? this.currentEmployeeDetails.Password : '',
        // CompId: this.currentEmployeeDetails.CompId ? this.currentEmployeeDetails.CompId : this.basicDetailsForm?.get('Company').value,
        CompId: Number(this.basicDetailsForm?.get('Company').value),
        // LEId: Number(this.currentEmployeeDetails.LEId ? this.currentEmployeeDetails.LEId : this.basicDetailsForm?.get('LegalEntity').value),
        LEId: Number(this.basicDetailsForm?.get('LegalEntity').value),
        // BUId: Number(this.currentEmployeeDetails.BUId ? this.currentEmployeeDetails.BUId : this.basicDetailsForm?.get('BusinessUnit').value),
        BUId: Number(this.basicDetailsForm?.get('BusinessUnit').value),
        // LocationId: Number(this.currentEmployeeDetails.LocationId ? this.currentEmployeeDetails.LocationId : this.basicDetailsForm?.get('Location').value),
        LocationId: Number(this.basicDetailsForm?.get('Location').value),
        // ReportId: this.currentEmployeeDetails.ReportId ? this.currentEmployeeDetails.ReportId : this.basicDetailsForm?.get('approverPerson').value,
        EmpTypeId: Number(this.basicDetailsForm?.get('employeeType').value) ? Number(this.basicDetailsForm?.get('employeeType').value) : this.currentEmployeeDetails.EmpTypeId,
        DeptId: Number(this.getDeptDataID ? this.getDeptDataID : this.currentEmployeeDetails.DeptId),
        DeptName: this.getDeptDataName ? this.getDeptDataName : this.currentEmployeeDetails.DeptName,
        DesignationId: Number(this.basicDetailsForm?.get('Designation').value ? this.basicDetailsForm?.get('Designation').value : this.currentEmployeeDetails.DesignationId),
        Designation: this.getDesignationName ? this.getDesignationName : this.currentEmployeeDetails.Designation,
        SalutationId: Number(this.basicDetailsForm?.get('basic_salutation').value ? this.basicDetailsForm?.get('basic_salutation').value : this.currentEmployeeDetails.SalutationId),
        EmpCode: this.basicDetailsForm?.get('EmpCode').value ? this.basicDetailsForm?.get('EmpCode').value : '',
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
        InterviewDate: this.basicDetailsForm?.get('InterviewDate').value ? this.basicDetailsForm?.get('InterviewDate').value : '',
        CEndDate: this.basicDetailsForm?.get('contractEndDate').value ? this.basicDetailsForm?.get('contractEndDate').value : '',
        ReportId: this.selectedEmpId ? this.selectedEmpId : this.currentEmployeeDetails?.ApproverId,
        Photo: this.getPhotoPath(),
        AuthorisedEntity: Array.isArray(authorisedEntityValue) ? authorisedEntityValue.join(',') : '',
        
        IsProbation: this.basicDetailsForm?.get('Probation').value,
        IsProbationConfirm:this.currentEmployeeDetails?.IsProbationConfirm,
        ProbationConfirmationDate:this.basicDetailsForm?.get('ProbationConfirmationDate').value,
        ProbationRemarks:this.basicDetailsForm?.get('ProbationRemark').value,
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeUpdateEmployee(reqBody).subscribe((res: any) => {
        if (res['msg'] === "Updated") {
          this.triggerToast(res['msg'], 'Data Updated Successfully', 'success');
          this.isSpinner = false;
          this.getEmployeeDeatils();
          this.isFormSubmitted = false;
          this.isBasicDetailsUpdateButton = false;
        } else if ((res["Message"])) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Error to update Bas Details', "danger");
        this.isSpinner = false;
      })
    } else {
      this.triggerToast("Invalid", "Please Fill All Datas", "danger");
      this.isSpinner = false;
    }
  }
  resetBasicDetials() {
    this.basicDetailsForm?.reset();
  }
  // ***************Contact Information .ts Starts************************

  contactInformationFormVal() {
    this.contactInfoForm = this.fb.group({
      // AMobileNo: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      // MotherName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      // FatherName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      // FContactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      // MContactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      // HusbandName: ['', [Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      // HContactNo: ['', [Validators.pattern('^[6-9][0-9]{9}$')]],
      // date_of_anniversary: [''],
      // Sports: [''],

      PMailId: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$/)]],
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

      EContactName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      EContactNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      EContactRelationship: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],

      EContactName1: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      EContactNo1: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      EContactRelationship1: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],

      EContactName2: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      EContactNo2: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      EContactRelationship2: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],

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
    this.getContactInformation();
    this.contactInfoForm.disable();
  }
  onEditClickContactDetails(event: Event): void {
    event.stopPropagation();
    this.isContactDetailsUpdateButton = true;
    this.contactInfoForm.enable();
  }
  sameAsPermanent(event: any) {
    const perAddress = {
      Curr_Building: this.contactInfoForm.value.Per_Building,
      Curr_Door_Number: this.contactInfoForm.value.Per_Door_Number,
      Curr_Street: this.contactInfoForm.value.Per_Street,
      Curr_City: this.contactInfoForm.value.Per_City,
      Curr_State: this.contactInfoForm.value.Per_State,
      Curr_Country: this.contactInfoForm.value.Per_Country,
      Curr_Location: this.contactInfoForm.value.Per_Location,
      Curr_PinCode: this.contactInfoForm.value.Per_PinCode
    };

    if (event.target.checked) {
      this.contactInfoForm.patchValue(perAddress);
    } else {
      this.contactInfoForm.patchValue({
        Curr_Building: '',
        Curr_Door_Number: '',
        Curr_Street: '',
        Curr_City: '',
        Curr_State: '',
        Curr_Country: '',
        Curr_Location: '',
        Curr_PinCode: ''
      });
    }
  }
  getContactInformation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].EmpId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
    }
    this.hrmsEmployeeModuleService.employeeGetContactDetails(reqBody).subscribe((res: any) => {
      this.getemployeeGetContactDetails = res;
      const DateOfAnniversary = this.getemployeeGetContactDetails.DateOfAnniversary ? this.parseJsonDate(this.getemployeeGetContactDetails.DateOfAnniversary) : null;
      const formattedDateOfAnniversary = this.formatDate(DateOfAnniversary);
      if (res) {
        this.contactInfoForm.patchValue({
          // AMobileNo: res.AMobileNo ? res.AMobileNo : '',
          // FatherName: res.FatherName ? res.FatherName : '',
          // MotherName: res.MotherName ? res.MotherName : '',
          // HusbandName: res.HusbandName ? res.HusbandName : '',
          // FContactNo: res.FContactNo ? res.FContactNo : '',
          // MContactNo: res.MContactNo ? res.MContactNo : '',
          // HContactNo: res.HContactNo ? res.HContactNo : '',
          // Sports: res.Sports ? res.Sports : '',
          // date_of_anniversary: formattedDateOfAnniversary ? formattedDateOfAnniversary : '',

          PMailId: res.PMailId ? res.PMailId : '',
          EContactName: res.EContactName ? res.EContactName : '',
          EContactNo: res.EContactNo ? res.EContactNo : '',
          EContactRelationship: res.EContactRelationship ? res.EContactRelationship : '',
          EContactName1: res.EContactName1 ? res.EContactName1 : '',
          EContactNo1: res.EContactNo ? res.EContactNo1 : '',
          EContactRelationship1: res.EContactRelationship1 ? res.EContactRelationship1 : '',

          EContactName2: res.EContactName2 ? res.EContactName2 : '',
          EContactNo2: res.EContactNo2 ? res.EContactNo2 : '',
          EContactRelationship2: res.EContactRelationship2 ? res.EContactRelationship2 : '',

          Height: res.Height ? res.Height : '',
          Weight: res.Weight ? res.Weight : '',
          Disability: res.Disability ? res.Disability : '',
          TotalExperience: res.TotalExperience ? res.TotalExperience : '',
          RelevantExperience: res.RelevantExperience ? res.RelevantExperience : '',
          ECActivities: res.ECActivities ? res.ECActivities : '',
          Cast: res.Caste ? res.Caste : '',
          Religion: res.Region ? res.Region : '',
          Country: res.Country ? res.Country : '',
          Nationality: res.Nationality ? res.Nationality : '',
          Per_Door_Number: res.PermanentDoorNumber ? res.PermanentDoorNumber : '',
          Per_Building: res.PermanentBuildingName ? res.PermanentBuildingName : '',
          Per_Street: res.PermanentStreet ? res.PermanentStreet : '',
          Per_Location: res.PermanentLocation ? res.PermanentLocation : '',
          Per_City: res.PermanentCity ? res.PermanentCity : '',
          Per_State: res.PermanentState ? res.PermanentState : '',
          Per_Country: res.PermanentCountry ? res.PermanentCountry : '',
          Per_PinCode: res.PermanentPinCode ? res.PermanentPinCode : '',
          Curr_Door_Number: res.CurrentDoorNumber ? res.CurrentDoorNumber : '',
          Curr_Building: res.CurrentBuildingName ? res.CurrentBuildingName : '',
          Curr_Street: res.CurrentStreet ? res.CurrentStreet : '',
          Curr_Location: res.CurrentLocation ? res.CurrentLocation : '',
          Curr_City: res.CurrentCity ? res.CurrentCity : '',
          Curr_State: res.CurrentState ? res.CurrentState : '',
          Curr_Country: res.CurrentCountry ? res.CurrentCountry : '',
          Curr_PinCode: res.CurrentPinCode ? res.CurrentPinCode : ''
        })
      }
    })
  }
  updateContactInformation() {
    this.isFormSubmitted = true;

    if (this.contactInfoForm.valid) {
      const reqBody = {
        // AMobileNo: this.contactInfoForm?.get('AMobileNo').value ? this.contactInfoForm?.get('AMobileNo').value : '',
        // FatherName: this.contactInfoForm?.get('FatherName').value ? this.contactInfoForm?.get('FatherName').value : '',
        // MotherName: this.contactInfoForm?.get('MotherName').value ? this.contactInfoForm?.get('MotherName').value : '',
        // HusbandName: this.contactInfoForm?.get('HusbandName').value ? this.contactInfoForm?.get('HusbandName').value : '',
        // FContactNo: this.contactInfoForm?.get('FContactNo').value ? this.contactInfoForm?.get('FContactNo').value : '',
        // MContactNo: this.contactInfoForm?.get('MContactNo').value ? this.contactInfoForm?.get('MContactNo').value : '',
        // HContactNo: this.contactInfoForm?.get('HContactNo').value ? this.contactInfoForm?.get('HContactNo').value : '',
        // DateOfAnniversary: this.contactInfoForm?.get('date_of_anniversary').value ? this.contactInfoForm?.get('date_of_anniversary').value : '',
        // Sports: this.contactInfoForm?.get('Sports').value ? this.contactInfoForm?.get('Sports').value : '',

        LoginId: this.employeeDetails[0].EmpId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
        Id: this.getemployeeGetContactDetails.Id ? this.getemployeeGetContactDetails.Id : 0,
        PMailId: this.contactInfoForm?.get('PMailId').value ? this.contactInfoForm?.get('PMailId').value : '',

        EContactName: this.contactInfoForm?.get('EContactName').value ? this.contactInfoForm?.get('EContactName').value : '',
        EContactRelationship: this.contactInfoForm?.get('EContactRelationship').value ? this.contactInfoForm?.get('EContactRelationship').value : '',
        EContactNo: this.contactInfoForm?.get('EContactNo').value ? this.contactInfoForm?.get('EContactNo').value : '',

        EContactName1: this.contactInfoForm?.get('EContactName1').value ? this.contactInfoForm?.get('EContactName1').value : '',
        EContactNo1: this.contactInfoForm?.get('EContactNo1').value ? this.contactInfoForm?.get('EContactNo1').value : '',
        EContactRelationship1: this.contactInfoForm?.get('EContactRelationship1').value ? this.contactInfoForm?.get('EContactRelationship1').value : '',

        EContactName2: this.contactInfoForm?.get('EContactName2').value ? this.contactInfoForm?.get('EContactName2').value : '',
        EContactNo2: this.contactInfoForm?.get('EContactNo2').value ? this.contactInfoForm?.get('EContactNo2').value : '',
        EContactRelationship2: this.contactInfoForm?.get('EContactRelationship2').value ? this.contactInfoForm?.get('EContactRelationship2').value : '',

        Height: this.contactInfoForm?.get('Height').value ? this.contactInfoForm?.get('Height').value : '',
        Weight: this.contactInfoForm?.get('Weight').value ? this.contactInfoForm?.get('Weight').value : '',
        Disability: this.contactInfoForm?.get('Disability').value ? this.contactInfoForm?.get('Disability').value : '',
        TotalExperience: this.contactInfoForm?.get('TotalExperience').value ? this.contactInfoForm?.get('TotalExperience').value : '',
        RelevantExperience: this.contactInfoForm?.get('RelevantExperience').value ? this.contactInfoForm?.get('RelevantExperience').value : '',
        ECActivities: this.contactInfoForm?.get('ECActivities').value ? this.contactInfoForm?.get('ECActivities').value : '',

        Caste: this.contactInfoForm?.get('Cast').value ? this.contactInfoForm?.get('Cast').value : '',
        Region: this.contactInfoForm?.get('Religion').value ? this.contactInfoForm?.get('Religion').value : '',
        Country: this.contactInfoForm?.get('Country').value ? this.contactInfoForm?.get('Country').value : '',
        Nationality: this.contactInfoForm?.get('Nationality').value ? this.contactInfoForm?.get('Nationality').value : '',
        PermanentBuildingName: this.contactInfoForm?.get('Per_Building').value ? this.contactInfoForm?.get('Per_Building').value : '',
        PermanentCity: this.contactInfoForm?.get('Per_City').value ? this.contactInfoForm?.get('Per_City').value : '',
        PermanentCountry: this.contactInfoForm?.get('Per_Country').value ? this.contactInfoForm?.get('Per_Country').value : '',
        PermanentDoorNumber: this.contactInfoForm?.get('Per_Door_Number').value ? this.contactInfoForm?.get('Per_Door_Number').value : '',
        PermanentLocation: this.contactInfoForm?.get('Per_Location').value ? this.contactInfoForm?.get('Per_Location').value : '',
        PermanentPinCode: this.contactInfoForm?.get('Per_PinCode').value ? this.contactInfoForm?.get('Per_PinCode').value : '',
        PermanentState: this.contactInfoForm?.get('Per_State').value ? this.contactInfoForm?.get('Per_State').value : '',
        PermanentStreet: this.contactInfoForm?.get('Per_Street').value ? this.contactInfoForm?.get('Per_Street').value : '',
        CurrentBuildingName: this.contactInfoForm?.get('Curr_Building').value ? this.contactInfoForm?.get('Curr_Building').value : '',
        CurrentCity: this.contactInfoForm?.get('Curr_City').value ? this.contactInfoForm?.get('Curr_City').value : '',
        CurrentCountry: this.contactInfoForm?.get('Curr_Country').value ? this.contactInfoForm?.get('Curr_Country').value : '',
        CurrentDoorNumber: this.contactInfoForm?.get('Curr_Door_Number').value ? this.contactInfoForm?.get('Curr_Door_Number').value : '',
        CurrentLocation: this.contactInfoForm?.get('Curr_Location').value ? this.contactInfoForm?.get('Curr_Location').value : '',
        CurrentPinCode: this.contactInfoForm?.get('Curr_PinCode').value ? this.contactInfoForm?.get('Curr_PinCode').value : '',
        CurrentState: this.contactInfoForm?.get('Curr_State').value ? this.contactInfoForm?.get('Curr_State').value : '',
        CurrentStreet: this.contactInfoForm?.get('Curr_Street').value ? this.contactInfoForm?.get('Curr_Street').value : '',
      }
      console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeUpdateContactDetails(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Data Updated Successfully', 'success');
          this.isSpinner = false;
          this.getContactInformation();
          this.isFormSubmitted = false;
          this.isContactDetailsUpdateButton = false;
        } else if ((res["Message"])) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('Internal Server Error', 'Error to update Contact Information', "danger");
        this.isSpinner = false;
      })
    } else {
      this.triggerToast("Invalid", "Please Fill All Datas", "danger");
      this.isSpinner = false;
    }
  }
  resetContactDetails() {
    this.contactInfoForm?.reset();
    this.getContactInformation();
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
      sets: this.fb.array([this.createSet()])
    }, { validators: this.dateRangeValidator });
    this.GetEmpCareerDetails();
  }
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
      this.hrmsEmployeeModuleService.EmployeeUploadFileCareer(
        this.employeeDetails[0].EmpId,
        this.careerForm?.get('OfferLetterMonth')?.value ? this.careerForm?.get('OfferLetterMonth')?.value : 'offerLatter',
        this.offerLetterSelectedFile
      ).subscribe(
        (res: any) => {
          this.getOfferLetter = res.path;
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
      this.hrmsEmployeeModuleService.EmployeeUploadFileCareer(
        this.employeeDetails[0].EmpId,
        this.careerForm?.get('SalaryLetterMonth')?.value ? this.careerForm?.get('SalaryLetterMonth')?.value : 'salaryLatter',
        this.salaryLetterSelectedFile
      ).subscribe(
        (res: any) => {
          this.getSalaryLetter = res.path;
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
      this.hrmsEmployeeModuleService.EmployeeUploadFileCareer(
        this.employeeDetails[0].EmpId,
        this.careerForm?.get('ExperienceLetterMonth')?.value ? this.careerForm?.get('ExperienceLetterMonth')?.value : 'salaryLatter',
        this.experienceLetterSelectedFile
      ).subscribe(
        (res: any) => {
          this.getExperienceLetter = res.path;
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
  GetEmpCareerDetails() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: Number(this.storeQueryParamsData.EmpId)
    };
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeGetEmpCareerDetails(reqBody).subscribe({
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
          this.errorMessageCareer = 'An unexpected error occurred. Please try again.';
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
    const empId = this.employeeDetails[0].EmpId; // Replace with actual employee ID
    const docName = `PaySlip${index + 1}`; // Use dynamic document name based on index
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.EmployeeUploadFileCareer(empId, docName, file).subscribe({
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
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
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
      // console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeAddEmpCareerDetails(reqBody).subscribe((res: any) => {
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
    }
  }
  updateCareerDetails(data: any) {
    // console.log(data);
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
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
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
        OfferLetter: this.isOfferLetterPreset(),
        SalaryLetter: this.isSalaryLetterPreset(),
        ExperienceLetter: this.isExperienceLetterPreset(),
        RelievingLetter: '',
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeUpdateEmpCareerDetails(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast('', 'Records Updated Successfully', 'success');
          this.isSpinner = false;
          this.GetEmpCareerDetails();
          this.resetCareerDetails();
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
  deleteCareerDetails(data: any) {
    // console.log(data);
    this.getCareerTableDeleteId = data.CareerId
  }
  deleteCareerTableList() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
      CareerId: this.getCareerTableDeleteId
    }
    // console.log(reqBody);
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeDeleteEmpCareerDetails(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.isRecordDeleted = true;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.GetEmpCareerDetails();
        this.isSpinner = false;
        this.isFormSubmitted = false;
        setTimeout(() => {
          this.closeModal.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
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
  // ***************Career Details .ts Ends************************

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
      Others: [{ value: '', disabled: true }],
      edu_photo: ['']
    }, { validators: this.dateRangeValidatorEducation });
    this.getEducationDropDown();
    setTimeout(() => {
      this.getGetEducationDoc();
    }, 100)
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
    this.hrmsEmployeeModuleService.employeeGetDDEducationDoc(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.educationDocName = res; this.isSpinner = false;
      } else {
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Education Qualification', 'danger');
      this.isSpinner = false;
    })
  }
  chooseEducDoc(event: Event) {
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
      this.hrmsEmployeeModuleService.employeeUploadEducFileDoc(
        this.employeeDetails[0].EmpId,
        this.educselectedName,
        this.SelectedFileEducDoc
      ).subscribe(
        (res: any) => {
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
  getEducDocName(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.educselectedID = selectElement.value;
    this.educselectedName = selectElement.options[selectElement.selectedIndex].text;
  }
  addEductionDetails() {
    this.isFormSubmitted = true;
    if ((this.EducationForm.valid)) {
      const fromDateValue = this.EducationForm?.get('date_from')?.value;
      const toDateValue = this.EducationForm?.get('date_to')?.value;
      const formatDate = (dateValue: string): string => {
        const date = new Date(dateValue);
        if (isNaN(date.getTime())) {
          return "";
        }
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${day}-${month}-${year}`;
      };
      const fromOnly = formatDate(fromDateValue);
      const toOnly = formatDate(toDateValue);
      const reqbody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
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
      // console.log(reqbody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeAddEducationDetails(reqbody).subscribe((res: any) => {
        if (res['msg'] == 'Added') {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
          this.getGetEducationDoc();
          this.resetEducationDoc();
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
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeGetEducationDoc(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
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
    // console.log(data);
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
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
        Id: this.pathchEducationData.Id ? this.pathchEducationData.Id : '',
        DocId: this.pathchEducationData.DocId ? this.pathchEducationData.DocId : '',
        Others: this.EducationForm?.get('Others').value ? this.EducationForm?.get('Others').value : '',
        School: this.EducationForm?.get('school').value ? this.EducationForm?.get('school').value : '',
        DocName: this.educselectedName ? this.educselectedName : this.pathchEducationData.DegreeId,
        Filed: this.EducationForm?.get('field_Of_Study').value ? this.EducationForm?.get('field_Of_Study').value : '',
        StartDate: this.EducationForm?.get('date_from').value ? this.EducationForm?.get('date_from').value : '',
        EndDate: this.EducationForm?.get('date_to').value ? this.EducationForm?.get('date_to').value : '',
        Grade: this.EducationForm?.get('grade').value ? this.EducationForm?.get('grade').value : '',
        Description: this.EducationForm?.get('edu_description').value ? this.EducationForm?.get('edu_description').value : '',
        Path: this.isEducationPreset(),
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeUpdateEducationDoc(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast('', 'Records Updated Successfully', 'success');
          this.getGetEducationDoc();
          this.isSpinner = false;
          this.resetEducationDoc();
        } else if (res['Message']) {
          this.triggerToast('Something Went Wrong', res['Message'], 'warning');
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
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
      Id: this.getEducTableDeleteId
    }
    // console.log(reqBody);
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeDeleteEducationDoc(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.isRecordDeleted = true;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getGetEducationDoc();
        this.isSpinner = false;
        this.isFormSubmitted = false;
        setTimeout(() => {
          this.closeModalEduc.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
      } else if (res['Message']) {
        this.triggerToast('Something Went Wrong', res['Message'], 'warning');
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
    this.imageSrcEducDoc = null;
    this.fileNameEducDoc = null;
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
    });
    // this.getEmployeeAccDetails();
    this.accountForm.disable();
  }
  onEditAccountClick(event: any) {
    event.stopPropagation();
    this.isUpdateAccountDetails = true;
    this.accountForm.enable();
  }
  getEmployeeAccDetails() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.GetEmpAccDetails(reqBody).subscribe((res: any) => {
      if (res) {
        this.pathchAccountDetails = res;
        this.accountForm.patchValue({
          BankName: this.pathchAccountDetails.BankName,
          IFSCCode: this.pathchAccountDetails.IFSCCode,
          BranchName: this.pathchAccountDetails.BranchName,
          AccHolderName: this.pathchAccountDetails.AccHolderName,
          AccNo: this.pathchAccountDetails.AccNo,
          // PFNo: this.pathchAccountDetails.PFNo,
          Acc_MobileNo: this.pathchAccountDetails.MobileNo,
        });
      }
      else {
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Account Details', "danger");
      this.isSpinner = false;
    })
  }
  updateAccountDetails() {
    this.isFormSubmitted = true;
    if (this.accountForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
        AccId: this.pathchAccountDetails.AccId ? this.pathchAccountDetails.AccId : 0,
        BankName: this.accountForm?.get('BankName').value ? this.accountForm?.get('BankName').value : '',
        IFSCCode: this.accountForm?.get('IFSCCode').value ? this.accountForm?.get('IFSCCode').value : '',
        BranchName: this.accountForm?.get('BranchName').value ? this.accountForm?.get('BranchName').value : '',
        AccHolderName: this.accountForm?.get('AccHolderName').value ? this.accountForm?.get('AccHolderName').value : '',
        AccNo: this.accountForm?.get('AccNo').value ? this.accountForm?.get('AccNo').value : '',
        // PFNo: this.accountForm?.get('PFNo').value ? this.accountForm?.get('PFNo').value : '',
        MobileNo: this.accountForm?.get('Acc_MobileNo').value ? this.accountForm?.get('Acc_MobileNo').value : '',
      }
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.UpdateEmpAccDetails(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Account Details Updated Successfully', "success");
          this.getEmployeeAccDetails();
          this.isSpinner = false;
          this.isFormSubmitted = false;
          this.isUpdateAccountDetails = false;
        } else if (res['Message']) {
          this.triggerToast(res['Message'], 'Something Went Wrong', "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast(error, 'Internal Server Error', "danger");
        this.isSpinner = false;
      })
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
    setTimeout(() => {
      this.getGetGovtDoc();
    }, 100)
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
    this.hrmsEmployeeModuleService.employeeGetDDGovtDoc(reqBody).subscribe((res: any) => {
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
  getDisplayPath(fullPath: string): string {
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
      this.hrmsEmployeeModuleService.employeeUploadGovtFileDoc(
        this.employeeDetails[0].EmpId,
        this.govtselectedName,
        this.SelectedFileGovtDoc
      ).subscribe(
        (res: any) => {
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
    } else {
      this.isSpinner = false;
    }
  }
  getGetGovtDoc() {
    const reqBody = {
      LoginId: this.employeeDetails[0].EmpId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
    }
    // console.log(reqBody);
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeGetGovtDoc(reqBody).subscribe({
      next: (res: any) => {
        this.isSpinner = false;
        if (res.length >= 1) {
          // this.govtDetailsRows = res.map((item: any) => {
          //   const fullPath = this.formatPath(item.Path);
          //   return {
          //     ...item,
          //     FullPath: fullPath, 
          //     DisplayPath: this.getDisplayPath(fullPath) 
          //   };
          // });
          this.govtDetailsRows = res;
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
          this.errorMessageCareer = 'An unexpected error occurred. Please try again.';
        }
        this.showErrorCareer = true;
      }
    });
  }
  addGovermentDetails() {
    this.isFormSubmitted = true;
    if ((this.governmentForm.valid)) {
      this.isFormSubmitted = true;
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
        LoginId: this.employeeDetails[0].EmpId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
        DocId: this.govtselectedID ? this.govtselectedID : '',
        // Name: this.governmentForm?.get('govt_name').value ? this.governmentForm?.get('govt_name').value : '',
        DocName: this.govtselectedName ? this.govtselectedName : '',
        DocNo: this.governmentForm?.get('govt_doc_num').value ? this.governmentForm?.get('govt_doc_num').value : '',
        IssuedDate: fromOnly ? fromOnly : '',
        ExpiredDate: toOnly ? toOnly : '',
        Description: this.governmentForm?.get('govt_description').value ? this.governmentForm?.get('govt_description').value : '',
        Others: this.governmentForm?.get('Others').value ? this.governmentForm?.get('Others').value : '',
        Path: this.getGovtDoc ? this.getGovtDoc : ''
      }
      // console.log(reqbody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeAddGovtDoc(reqbody).subscribe((res: any) => {
        if (res['msg'] == 'Added') {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.isSpinner = false;
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
    // console.log(data);
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
        LoginId: this.employeeDetails[0].EmpId,
        EmpId: Number(this.storeQueryParamsData.EmpId),
        GovId: this.patchGovtData?.GovId,
        DocId: this.patchGovtData?.DocId,
        Others: this.governmentForm?.get('Others').value ? this.governmentForm?.get('Others').value : this.patchGovtData.Others,
        // Name: this.governmentForm?.get('govt_name').value ? this.governmentForm?.get('govt_name').value : this.patchGovtData.Name,
        DocName: this.govtselectedName ? this.govtselectedName : this.patchGovtData.DocName,
        DocNo: this.governmentForm?.get('govt_doc_num').value ? this.governmentForm?.get('govt_doc_num').value : this.patchGovtData.DocNo,
        IssuedDate: this.governmentForm?.get('date_from').value ? this.governmentForm?.get('date_from').value : this.patchGovtData.IssuedDate,
        ExpiredDate: this.governmentForm?.get('date_to').value ? this.governmentForm?.get('date_to').value : this.patchGovtData.ExpiredDate,
        Description: this.governmentForm?.get('govt_description').value ? this.governmentForm?.get('govt_description').value : this.patchGovtData.Description,
        Path: this.isGovtPreset(),
      }
      // console.log(reqBody);
      this.isSpinner = true;
      this.hrmsEmployeeModuleService.employeeUpdateGovtDoc(reqBody).subscribe((res: any) => {
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
    // console.log(data);
    this.getGovtTableDeleteId = data.GovId;
  }
  deleteGovtDocument() {
    const reqBody = {
      LoginId: this.employeeDetails[0].EmpId,
      EmpId: Number(this.storeQueryParamsData.EmpId),
      GovId: this.getGovtTableDeleteId
    }
    // console.log(reqBody);
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.employeeUpdateDeleteDoc(reqBody).subscribe((res: any) => {
      if (res['msg']) {
        this.isRecordDeleted = true;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getGetGovtDoc();
        this.isSpinner = false;
        this.isFormSubmitted = false;
        setTimeout(() => {
          this.closeModalGovt.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
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
}
