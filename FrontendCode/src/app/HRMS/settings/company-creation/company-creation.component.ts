import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { SettingsService } from '../../service/settings.service';
import { RouterModule } from '@angular/router';
import { Modal } from 'bootstrap';
import { Dropdown } from 'bootstrap';
import { EmployeeModuleService } from '../../service/employee.service';
import commonData from 'src/assets/common.json';
import { environment } from 'src/assets/environment';
import { HolidaysComponent } from '../holidays/holidays.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs';
import { EmpFinancialDetailsComponent } from '../../payroll/emp-financial-details/emp-financial-details.component';

const deleteKeyMap: { [key: string]: string } = {
  company: 'CompId',
  entity: 'LEId',
  businessUnit: 'BUId',
  location: 'LocId'
};

@Component({
  selector: 'app-company-creation',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent,
    NgxPaginationModule, RouterModule, HolidaysComponent, EmpFinancialDetailsComponent],
  templateUrl: './company-creation.component.html',
  styleUrl: './company-creation.component.scss'
})
export class CompanyCreationComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDeleteLocation') closeModalDeleteLocation!: ElementRef;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;
  @ViewChild('fileInput') fileInput!: ElementRef;

  companyCreationForm: any = FormGroup;
  addForm: any = FormGroup;
  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  isFormSubmitted: boolean = false;
  isFormSubmittedAddForm: boolean = false;
  isEdited: boolean = false;
  rows: any;
  originalRows: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  errorMessage: any;
  isSpinner: boolean = false;
  getDDCompanyMain: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;

  currentAddType: 'company' | 'entity' | 'businessUnit' | 'location' | null = null;
  isRecordDeleted: boolean = false;
  isRecordDeletedCommon: boolean = false;

  countries: string[] = [];
  timezone: string[] = [];
  filteredCountries: string[] = [];
  countrySearch: string = '';
  showDropdown: boolean = false;
  isCardOpen: boolean = false;
  selectedCountry: string = '';

  timezones: string[] = [
    "UTC−12:00 — International Date Line West (IDLW)",
    "UTC−11:00 — Samoa Standard Time (SST)",
    "UTC−10:00 — Hawaii-Aleutian Standard Time (HAST)",
    "UTC−09:30 — Marquesas Time (MART)",
    "UTC−09:00 — Alaska Standard Time (AKST)",
    "UTC−08:00 — Pacific Standard Time (PST)",
    "UTC−07:00 — Mountain Standard Time (MST)",
    "UTC−06:00 — Central Standard Time (CST)",
    "UTC−05:00 — Eastern Standard Time (EST)",
    "UTC−04:00 — Atlantic Standard Time (AST)",
    "UTC−03:30 — Newfoundland Standard Time (NST)",
    "UTC−03:00 — Argentina Time (ART)",
    "UTC−02:00 — South Georgia Time (GST)",
    "UTC−01:00 — Azores Standard Time (AZOT)",
    "UTC±00:00 — Greenwich Mean Time (GMT)",
    "UTC+01:00 — Central European Time (CET)",
    "UTC+02:00 — Eastern European Time (EET)",
    "UTC+03:00 — Moscow Standard Time (MSK)",
    "UTC+03:30 — Iran Standard Time (IRST)",
    "UTC+04:00 — Gulf Standard Time (GST)",
    "UTC+04:30 — Afghanistan Time (AFT)",
    "UTC+05:00 — Pakistan Standard Time (PKT)",
    "UTC+05:30 — India Standard Time (IST)",
    "UTC+05:45 — Nepal Time (NPT)",
    "UTC+06:00 — Bangladesh Standard Time (BST)",
    "UTC+06:30 — Myanmar Time (MMT)",
    "UTC+07:00 — Indochina Time (ICT)",
    "UTC+08:00 — China Standard Time (CST)",
    "UTC+08:45 — Australian Central Western Time (ACWST)",
    "UTC+09:00 — Japan Standard Time (JST)",
    "UTC+09:30 — Australian Central Time (ACST)",
    "UTC+10:00 — Australian Eastern Time (AEST)",
    "UTC+10:30 — Lord Howe Standard Time (LHST)",
    "UTC+11:00 — Solomon Islands Time (SBT)",
    "UTC+12:00 — New Zealand Standard Time (NZST)",
    "UTC+12:45 — Chatham Islands Time (CHAST)",
    "UTC+13:00 — Tonga Time (TOT)",
    "UTC+14:00 — Line Islands Time (LINT)"
  ];

  weekDays = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wednesday',
    'Thursday',
    'Friday',
    'Saturday'
  ];

  selectedWeeklyDays: string[] = [];
  weeklyDropdownOpen = false;

  filteredTimezones: string[] = [];
  timezoneSearch: string = '';
  showTimezoneDropdown: boolean = false;
  selectedTimezone: string = '';


  constructor(private readonly fb: FormBuilder,
    private readonly settingService: SettingsService,
    private readonly hrmsService: EmployeeModuleService,
    private cdr: ChangeDetectorRef,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private entityStateService: EntityStateService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Master Creation'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Master Creation'
      );
    });

    this.countries = commonData.country;
    this.filteredCountries = [...this.countries];
  }

  loginId: any;

  calendarYears: any[] = [];
  financialYears: any[] = [];

  selectedCalendarId: number | null = null;
  selectedFinancialId: number | null = null;

  newCalendarYear: number | null = null;
  newFinancialYear: string = '';

  ngOnInit(): void {
    this.companyCreationForm = this.fb.group({
      companyName: ['', [Validators.required]],
      LegalEntity: ['', [Validators.required]],
      businessUnit: [0],
      location: ['', [Validators.required]],
      ProbationPeriod: ['',],
      WeeklyHoliday: ['',],
      CompanyRegNo: ['',],
      DateofReg: ['',],
      PFNo: ['',],
      ESINo: ['',],
      TANNo: ['',],
      VATNo: ['',],
      PANNo: ['',],
      ServiceTaxNo: ['',],
      GSTNo: ['',],

      locationDes: [''],
      locationMap: [''],
      locationAddress: [''],
      country: [''],
      state: [''],
      city: [''],
      postalCode: [''],
      timezone: [''],
    });

    this.addForm = this.fb.group({
      Company: ['', [Validators.required]],
      CompanyCode: [''],
      LocationMap: [''],
      Address: [''],

      CompId: ['', [Validators.required]],
      LegalEntity: ['', [Validators.required]],
      CompanyType: [''],
      Website: [''],
      Description: [''],

      LOGO: [''],
      LOGOWITHADDRESS: [''],
      WEBAPPLOGO: [''],

      CompNameBusiness: ['', [Validators.required]],
      legalEntityBusiness: ['', [Validators.required]],
      businessUnitName: ['', [Validators.required]],
      businessUnitDesc: ['']
    });
    this.getAllLocation();
    this.DD_Company_Main();
    this.getLocation();
    this.loadCalendarYears();
    this.loadFinancialYears();

    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;
        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.resetData();
        }
        this.currentEntityId = newEntityId;
      });
  }

  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }

  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }

  filterCountries(): void {
    const search = this.countrySearch.toLowerCase();
    this.filteredCountries = this.countries.filter(c =>
      c.toLowerCase().includes(search)
    );
    this.showDropdown = true;
  }

  selectCountry(country: string): void {
    this.countrySearch = country;
    this.showDropdown = false;
  }

  filterTimezones(): void {
    const search = this.timezoneSearch.toLowerCase();
    this.filteredTimezones = this.timezones.filter(tz =>
      tz.toLowerCase().includes(search)
    );
    this.showTimezoneDropdown = true;
  }

  selectTimezone(tz: string): void {
    this.timezoneSearch = tz;
    this.showTimezoneDropdown = false;
  }

  // When you open the timezone dropdown (e.g. on focus)
  onTimezoneFocus() {
    this.filteredTimezones = [...this.timezones]; // reset filter to all timezones
    this.showTimezoneDropdown = true;
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;

    const countryInput = document.getElementById('countryId');
    const timezoneInput = document.getElementById('timezoneId');

    const countryDropdown = document.querySelector('.country-dropdown-menu');
    const timezoneDropdown = document.querySelector('.timezone-dropdown-menu');

    // ✅ NEW (Weekly dropdown)
    const weeklyDropdown = document.querySelector('.weekly-dropdown-menu');
    const weeklyButton = document.querySelector('#weeklyDropdownBtn'); // optional (see below)

    // Existing checks
    const clickedInsideCountry =
      countryInput?.contains(target) || countryDropdown?.contains(target);

    const clickedInsideTimezone =
      timezoneInput?.contains(target) || timezoneDropdown?.contains(target);

    // ✅ NEW check
    const clickedInsideWeekly =
      weeklyDropdown?.contains(target) || weeklyButton?.contains(target);

    if (!clickedInsideCountry) {
      this.showDropdown = false;
    }

    if (!clickedInsideTimezone) {
      this.showTimezoneDropdown = false;
    }

    // ✅ CLOSE WEEKLY DROPDOWN
    if (!clickedInsideWeekly) {
      this.weeklyDropdownOpen = false;
    }
  }

  // Toggle dropdown
  toggleWeeklyDropdown(event: Event) {
    event.stopPropagation();
    this.weeklyDropdownOpen = !this.weeklyDropdownOpen;
  }

  // Check selected
  isDaySelected(day: string): boolean {
    return this.selectedWeeklyDays.includes(day);
  }

  // Handle selection (MAX 3)
  onDayChange(day: string) {
    if (this.isDaySelected(day)) {
      this.selectedWeeklyDays = this.selectedWeeklyDays.filter(d => d !== day);
    } else {
      this.selectedWeeklyDays.push(day);
    }

    this.companyCreationForm.patchValue({
      WeeklyHoliday: this.selectedWeeklyDays
    });
  }
  getLocation(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const latitude = position.coords.latitude;
          const longitude = position.coords.longitude;
          this.getPlaceDetails(latitude, longitude);
        },
        (error) => {
          const latitude = null;
          const longitude = null;
        }
      );
    } else {
    }
  }

  getLocationData: any;

  getPlaceDetails(latitude: number, longitude: number) {
    const geocodeUrl = `https://nominatim.openstreetmap.org/reverse?lat=${latitude}&lon=${longitude}&format=json`;
    fetch(geocodeUrl)
      .then(response => response.json())
      .then(data => {
        if (data && data.address) {
          this.getLocationData = data;
        } else {
          console.error('Unable to retrieve location data');
        }
      })
      .catch(error => {
        console.error('Error with reverse geocoding:', error);
      });
  }

  DD_Company_Main() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDCompany(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDCompanyMain = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
        this.isSpinner = false;
      }
    },
      error => {
        this.triggerToast('Internal Server Error', 'Error loading Company Name', "danger");
        this.isSpinner = false;
      })
  }
  onCompanyChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    if (selectedValue === 'createNew') {
      this.openAddModal('company');
      // Reset selection if needed
      this.companyCreationForm.get('companyName')?.setValue(null);
    } else {
      this.calllegalEntity();
    }
  }
  calllegalEntity() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: this.entityStateService.getEntityId(),
      CompId: Number(this.companyCreationForm?.get('companyName').value)
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
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
        this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
        this.isSpinner = false;
      })
  }
  onLegalEntityChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    if (selectedValue === 'createNew') {
      // Clear selection if needed
      this.companyCreationForm.get('LegalEntity')?.setValue(null);

      // Open the modal for entity creation
      this.openAddModal('entity');
    } else {
      // Normal processing
      this.getBusinessUnit();
    }
  }

  onBusinessUnitChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    if (selectedValue === 'createNew') {
      this.companyCreationForm.get('businessUnit')?.setValue(null);  // reset selection if needed
      this.openAddModal('businessUnit');
    } else {
      // You can add other logic here if needed when selecting normal options
    }
  }


  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: this.entityStateService.getEntityId(),
      CompId: Number(this.companyCreationForm?.get('companyName').value),
      LEId: Number(this.companyCreationForm?.get('LegalEntity').value),
    }
    this.isSpinner = true;
    this.hrmsService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getBusinessUnitlist = res;
        this.isSpinner = false;
      } else {
        this.isSpinner = false;
        this.getBusinessUnitlist = [];
      }
    },
      error => {
        this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
        this.isSpinner = false;
      })
  }

  tabs = [
    { label: 'Locations', icon: 'feather icon-map-pin' },
    { label: 'Holiday', icon: 'feather icon-sun' },
    { label: 'Account Details', icon: 'feather icon-credit-card' },
    { label: 'Year Setup', icon: 'feather icon-calendar' }
    // { label: 'Employee Types', icon: 'feather icon-users' },
    // { label: 'Work Week', icon: 'feather icon-calendar' }
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
  }

  // ****************this is for open modal type******************
  openAddModal(type: 'company' | 'entity' | 'businessUnit' | 'location') {
    this.currentAddType = type;
    console.log(this.currentAddType);
    switch (this.currentAddType) {
      case 'company':
        this.getAllCompanyDD();
        break;

      case 'entity':
        this.getAllCompanyDD();
        this.getAllLegalEntityDD();
        break;

      case 'businessUnit':
        this.getAllBusinessUnit();
        this.getAllCompanyDD();
        this.getAllLegalEntityDD();
        break;

      case 'location':
        // Logic here
        break;
    }
    const modalElement = document.getElementById('exampleModal');
    if (modalElement) {
      const modal = new Modal(modalElement);
      modal.show();
    }
  }

  closeModalReset() {
    this.addForm?.reset();
    this.isEdited = false;
    this.isFormSubmittedAddForm = false;
  }


  // ****************this is for open modal type******************


  ///////////// This is for Add Company Data //////////////////////
  modalgetDDCompany: any = [];
  isTableDataAddCompany: boolean = false;
  patchComapnyData: any;
  errorMessageCompany: string = '';

  getAllCompanyDD() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.settingService.GetAllCompany(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length >= 1) {
          this.modalgetDDCompany = res;
          this.errorMessageCompany = '';
          this.isTableDataAddCompany = false;
        } else {
          this.errorMessageCompany = 'No Data Found';
          this.modalgetDDCompany = [];
          this.isTableDataAddCompany = true;
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.isSpinner = false;
        this.errorMessageCompany = 'Internal Server Error';
        this.isTableDataAddCompany = true;
      }
    })
  }

  patchVlaues(data: any, edited: boolean) {
    console.log(data);
    this.isEdited = edited;
    this.patchComapnyData = data;
    this.addForm.patchValue({
      Company: data.Company,
      CompanyCode: data.CompanyCode,
      LocationMap: data.LocationMap,
      Address: data.Address,
    });
  }

  toggleIsActive(row: any): void {
    row.IsActive = !row.IsActive;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: row.CompId
    }
    this.isSpinner = true;
    const apiCall = row.IsActive
      ? this.settingService.ActivateCompany(reqBody)
      : this.settingService.DeActivateCompany(reqBody);
    apiCall.subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.getAllCompanyDD();
        } else if (res['msg']) {
          this.getAllCompanyDD();
          this.triggerToast(`${row.IsActive ? 'Activated' : 'Deactivated'} successfully`, `${row.IsActive ? 'Activated' : 'Deactivated'}`, 'success');
        }
        this.isSpinner = false;
      },
      error: (err) => {
        this.isSpinner = false;
        console.error('API error:', err);
        row.IsActive = !row.IsActive;
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      }
    });
  }
  ///////////// This is for Add Company Data //////////////////////


  ///////////// This is for Add LegalEntity Data //////////////////////
  modalLegalEntities: any[] = [];
  isTableDataAddLegalEntity: boolean = false;
  patchLegalEntityData: any;
  errorMessageLegalEntity: string = '';
  baseUrl: string = environment.baseUrl;

  selectedFiles: any = {
    LOGO: null,
    LOGOWITHADDRESS: null,
    WEBAPPLOGO: null
  };

  filePreviewUrls: any = {
    LOGO: null,
    LOGOWITHADDRESS: null,
    WEBAPPLOGO: null
  };

  getLeaveDocPath: any = {
    LOGO: '',
    LOGOWITHADDRESS: '',
    WEBAPPLOGO: ''
  };

  onFileSelected(event: any, type: string) {

    const file: File = event.target.files[0];
    if (!file) return;

    // JPG validation
    const isJpg =
      file.type === 'image/jpeg' ||
      file.name.toLowerCase().endsWith('.jpg') ||
      file.name.toLowerCase().endsWith('.jpeg');

    if (!isJpg) {
      this.triggerToast('Invalid file', 'Only JPG files allowed', 'warning');
      event.target.value = '';
      return;
    }

    // store file + preview
    this.selectedFiles[type] = file;
    this.filePreviewUrls[type] = URL.createObjectURL(file);

    // upload immediately
    this.uploadFile(type);
  }

  uploadFile(type: string) {

    const file = this.selectedFiles[type];
    if (!file) return;

    this.isSpinner = true;

    this.hrmsService.employeeUploadImage(
      this.patchLegalEntityData?.LEId,
      file,
      type
    ).subscribe({
      next: (res: any) => {

        if (res.msg) {

          this.triggerToast('Success', 'File uploaded successfully', 'success');

          this.getLeaveDocPath[type] = res.path;

        } else {
          this.triggerToast('Warning', res.Message, 'warning');
        }

        this.isSpinner = false;
      },

      error: () => {
        this.isSpinner = false;
        this.triggerToast('Error', 'Upload failed', 'danger');
      }
    });
  }
  openFile(type: string) {

    const file = this.selectedFiles[type];

    if (!file) return;

    const url = this.filePreviewUrls[type];

    if (this.isImage(file) || this.isPdf(file)) {
      window.open(url, '_blank');
    } else {
      this.triggerToast('Warning', 'Preview not supported', 'warning');
    }
  }

  getFullUrl(path: string): string {

    if (!path) return '';

    return this.baseUrl + '/' +
      path
        .replace(/^~\//, '')   // remove "~/"
        .replace(/\\/g, '/');  // fix slashes
  }
  removeFile(type: string) {

    if (this.filePreviewUrls[type]) {
      URL.revokeObjectURL(this.filePreviewUrls[type]);
    }

    this.selectedFiles[type] = null;
    this.filePreviewUrls[type] = null;

    if (this.getLeaveDocPath?.[type]) {
      delete this.getLeaveDocPath[type];
    }
  }

  getAllLegalEntityDD() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.settingService.GetAllLegalEntity(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length >= 1) {
          this.modalLegalEntities = res;
          this.isTableDataAddLegalEntity = false;
          this.errorMessageLegalEntity = '';
        } else {
          this.errorMessageLegalEntity = 'No Data Found';
          this.modalLegalEntities = [];
          this.isTableDataAddLegalEntity = true;
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.isSpinner = false;
        this.errorMessageLegalEntity = 'Internal Server Error';
        this.isTableDataAddLegalEntity = true;
      }
    })
  }

  isImage(file: File): boolean {
    return file.type.startsWith('image/');
  }
  isPdf(file: File): boolean {
    return file.type === 'application/pdf';
  }

  patchVlauesEntity(data: any, edited: boolean) {
    this.isEdited = edited;
    this.patchLegalEntityData = data;

    this.addForm.patchValue({
      CompId: data.CompId,
      LegalEntity: data.LegalEntity,
      Description: data.Description,
      CompanyType: data.CompanyType,
      Website: data.Website
    });
    // 🔥 IMPORTANT: store image paths separately
    this.getLeaveDocPath = {
      LOGO: data.Logo || '',
      LOGOWITHADDRESS: data.LogoWithAddress || '',
      WEBAPPLOGO: data.WebAppLogo || ''
    };

  }

  toggleIsActiveEntity(row: any) {
    row.IsActive = !row.IsActive;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: row.LEId
    }
    this.isSpinner = true;
    const apiCall = row.IsActive
      ? this.settingService.ActivateLegalEntity(reqBody)
      : this.settingService.DeActivateLegalEntity(reqBody);
    apiCall.subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.getAllLegalEntityDD();
        } else if (res['msg']) {
          this.getAllLegalEntityDD();
          this.triggerToast(`${row.IsActive ? 'Activated' : 'Deactivated'} successfully`, `${row.IsActive ? 'Activated' : 'Deactivated'}`, 'success');
        }
        this.isSpinner = false;
      },
      error: (err) => {
        this.isSpinner = false;
        console.error('API error:', err);
        row.IsActive = !row.IsActive;
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      }
    });
  }

  ///////////// This is for Add LegalEntity Data //////////////////////


  ///////////// This is for Add BusinessUnit Data //////////////////////
  modalBusinessUnitData: any[] = [];
  isTableDataAddBusinessUnit: boolean = false;
  patchBusinessUnitData: any;
  errorMessageBusinessUnit: string = '';

  getAllBusinessUnit() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.settingService.GetAllBusinessUnit(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length >= 1) {
          this.modalBusinessUnitData = res;
          this.isTableDataAddBusinessUnit = false;
          this.errorMessageBusinessUnit = '';
        } else {
          this.errorMessageBusinessUnit = 'No Data Found';
          this.modalLegalEntities = [];
          this.isTableDataAddBusinessUnit = true;
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.isSpinner = false;
        this.errorMessageBusinessUnit = 'Internal Server Error';
        this.isTableDataAddBusinessUnit = true;
        this.cdr.detectChanges();
        console.log('isTableDataAddBusinessUnit:', this.isTableDataAddBusinessUnit);
      }
    })
  }

  patchVlauesBusiness(data: any, edited: boolean) {
    this.isEdited = edited;
    this.patchBusinessUnitData = data;
    console.log(data)
    this.addForm.patchValue({
      CompNameBusiness: data.CompId,
      legalEntityBusiness: data.LEId,
      businessUnitName: data.BusinessUnit,
      businessUnitDesc: data.Description,
    });
  }

  toggleIsActiveBusiness(row: any) {
    row.IsActive = !row.IsActive;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      BUId: row.BUId
    }
    this.isSpinner = true;
    const apiCall = row.IsActive
      ? this.settingService.ActivateBusinessUnit(reqBody)
      : this.settingService.DeActivateBusinessUnit(reqBody);
    apiCall.subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.getAllBusinessUnit();
        } else if (res['msg']) {
          this.getAllBusinessUnit();
          this.triggerToast(`${row.IsActive ? 'Activated' : 'Deactivated'} successfully`, `${row.IsActive ? 'Activated' : 'Deactivated'}`, 'success');
        }
        this.isSpinner = false;
      },
      error: (err) => {
        this.isSpinner = false;
        console.error('API error:', err);
        row.IsActive = !row.IsActive;
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      }
    });
  }

  ///////////// This is for Add BusinessUnit Data //////////////////////


  ///////////// This is for Add Location Data //////////////////////
  patchLocationData: any;
  getAllLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.settingService.GetAllLocation(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res?.length > 0) {
          this.rows = res;
          this.isTableData = false;
          this.errorMessage = '';
        } else {
          this.errorMessage = 'No Data Found';
          this.modalLegalEntities = [];
          this.isTableData = true;
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.isSpinner = false;
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
      }
    })
  }

  patchVlauesLocation(data: any, edited: boolean) {
    this.isEdited = edited;
    this.isCardOpen = true;
    console.log(data)
    this.patchLocationData = data;
    setTimeout(() => {
      this.calllegalEntity();
      this.getBusinessUnit();
    }, 100);

    setTimeout(() => {
      this.companyCreationForm.patchValue({
        companyName: data.CompId,
        LegalEntity: data.LEId,
        businessUnit: data.BUId,
        location: data.Location,

        locationDes: data.Description, // ⚠️ FIX (you used wrong key before)
        locationMap: data.LocationMap,
        locationAddress: data.Address,

        country: data.Country,
        state: data.State,
        city: data.City,
        postalCode: data.PostalCode,
        timezone: data.TimeZone,

        // ✅ NEW FIELDS
        ProbationPeriod: data.ProbationPeriod,

        // ✅ IMPORTANT (string → array)
        WeeklyHoliday: data.WeeklyHoliday
          ? data.WeeklyHoliday.split(',')
          : [],

        CompanyRegNo: data.CompanyRegNo,
        DateofReg: data.DateofReg,

        PFNo: data.PFNo,
        ESINo: data.ESINo,
        TANNo: data.TANNo,
        VATNo: data.VATNo,
        PANNo: data.PANNo,
        ServiceTaxNo: data.ServiceTaxNo,
        GSTNo: data.GSTNo
      });

      // ✅ ALSO sync your UI array (important for checkbox selection)
      this.selectedWeeklyDays = data.WeeklyHoliday
        ? data.WeeklyHoliday.split(',')
        : [];

    }, 1000);
  }

  submitFormdataMain() {
    if (this.companyCreationForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        CompId: Number(this.companyCreationForm?.get('companyName').value),
        LEId: Number(this.companyCreationForm?.get('LegalEntity').value),
        BUId: Number(this.companyCreationForm?.get('businessUnit').value),

        // Location Details
        Location: this.companyCreationForm?.get('location').value,
        Description: this.companyCreationForm?.get('locationDes').value,
        LoginLatitude: this.getLocationData?.lat,
        LoginLongitude: this.getLocationData?.lon,
        // LocationMap: this.getLocationData?.display_name,
        LocationMap: this.companyCreationForm?.get('locationMap').value,
        Address: this.companyCreationForm?.get('locationAddress').value,
        Country: this.companyCreationForm?.get('country').value,
        State: this.companyCreationForm?.get('state').value,
        City: this.companyCreationForm?.get('city').value,
        PostalCode: this.companyCreationForm?.get('postalCode').value,
        TimeZone: this.companyCreationForm?.get('timezone').value,

        // ✅ Newly Added Fields
        ProbationPeriod: this.companyCreationForm?.get('ProbationPeriod').value,

        WeeklyHoliday: this.companyCreationForm?.get('WeeklyHoliday').value?.join(','),
        // 👉 convert array to string if API expects "Sunday,Monday"

        CompanyRegNo: this.companyCreationForm?.get('CompanyRegNo').value,
        DateofReg: this.companyCreationForm?.get('DateofReg').value,

        PFNo: this.companyCreationForm?.get('PFNo').value,
        ESINo: this.companyCreationForm?.get('ESINo').value,
        TANNo: this.companyCreationForm?.get('TANNo').value,
        VATNo: this.companyCreationForm?.get('VATNo').value,
        PANNo: this.companyCreationForm?.get('PANNo').value,
        ServiceTaxNo: this.companyCreationForm?.get('ServiceTaxNo').value,
        GSTNo: this.companyCreationForm?.get('GSTNo').value
      };
      this.isSpinner = true;
      this.settingService.AddLocation(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast(res['Message'], res['Message'], "info");
          } else if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], "");
            this.getAllLocation();
            this.companyCreationForm.reset();
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', "Failed to submit data", "danger");
          this.isSpinner = false;
        }

      })
    } else {
      this.isFormSubmitted = true;
    }
  }

  updateCompanyCreationForm() {
    if (this.companyCreationForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        CompId: Number(this.companyCreationForm?.get('companyName').value),
        LEId: Number(this.companyCreationForm?.get('LegalEntity').value),
        BUId: Number(this.companyCreationForm?.get('businessUnit').value),

        // Location Details
        Location: this.companyCreationForm?.get('location').value,
        Description: this.companyCreationForm?.get('locationDes').value,
        LocationMap: this.companyCreationForm?.get('locationMap').value,
        Address: this.companyCreationForm?.get('locationAddress').value,
        Country: this.companyCreationForm?.get('country').value,
        State: this.companyCreationForm?.get('state').value,
        City: this.companyCreationForm?.get('city').value,
        PostalCode: this.companyCreationForm?.get('postalCode').value,
        TimeZone: this.companyCreationForm?.get('timezone').value,

        // ✅ Existing update identifier
        LocationId: this.patchLocationData.LocationId,

        // ✅ Newly Added Fields
        ProbationPeriod: this.companyCreationForm?.get('ProbationPeriod').value,

        WeeklyHoliday: this.companyCreationForm?.get('WeeklyHoliday').value?.join(','),

        CompanyRegNo: this.companyCreationForm?.get('CompanyRegNo').value,
        DateofReg: this.companyCreationForm?.get('DateofReg').value,

        PFNo: this.companyCreationForm?.get('PFNo').value,
        ESINo: this.companyCreationForm?.get('ESINo').value,
        TANNo: this.companyCreationForm?.get('TANNo').value,
        VATNo: this.companyCreationForm?.get('VATNo').value,
        PANNo: this.companyCreationForm?.get('PANNo').value,
        ServiceTaxNo: this.companyCreationForm?.get('ServiceTaxNo').value,
        GSTNo: this.companyCreationForm?.get('GSTNo').value
      };
      this.isSpinner = true;
      this.settingService.UpdateLocation(reqBody).subscribe({
        next: (res: any) => {
          if (res['Message']) {
            this.triggerToast(res['Message'], res['Message'], "info");
          } else if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], "");
            this.getAllLocation();
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', "Failed to submit data", "danger");
          this.isSpinner = false;
        }

      })
    } else {
      this.isFormSubmitted = true;
      this.triggerToast('Invalid', "Please Fill All Required Data", "info");
    }
  }
  getDeleteLocationId: any;
  getDeleteRow(row: any) {
    this.getDeleteLocationId = row;
  }

  deleteLocationRecord() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LocationId: this.getDeleteLocationId.LocationId,
    }
    this.isSpinner = true;
    this.settingService.DeleteLocation(reqBody).subscribe({
      next: (res: any) => {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.isRecordDeleted = true;
        setTimeout(() => {
          this.closeModalDeleteLocation.nativeElement?.click();
          this.getAllLocation()
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
        this.isSpinner = false;
        this.errorMessage = "Internal Server Error"
      }
    })
  }

  toggleIsActiveLocation(row: any) {
    row.IsActive = !row.IsActive;
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      LocationId: row.LocationId,
    };
    this.isSpinner = true;
    // Choose correct API based on new status
    const apiCall = row.IsActive
      ? this.settingService.ActivateLocation(payload)
      : this.settingService.DeActivateLocation(payload);
    apiCall.subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.getAllLocation();
        } else if (res['msg']) {
          this.getAllLocation();
          this.triggerToast(`${row.IsActive ? 'Activated' : 'Deactivated'} successfully`, `${row.IsActive ? 'Activated' : 'Deactivated'}`, 'success');
        }
        this.isSpinner = false;

      },
      error: (err) => {
        this.isSpinner = false;
        console.error('API error:', err);
        row.IsActive = !row.IsActive;
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      }
    });
  }
  ///////////// This is for Add Location Data //////////////////////

  submitAddForm() {
    switch (this.currentAddType) {
      case 'company':
        this.isFormSubmittedAddForm = true;
        if (this.addForm?.get('Company').valid) {
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            Company: this.addForm?.get('Company').value,
            CompanyCode: this.addForm?.get('CompanyCode').value,
            // LocationMap: "",
            Address: this.addForm?.get('Address').value,
          }
          this.isSpinner = true;
          this.settingService.AddCompany(reqBody).subscribe({
            next: (res: any) => {
              if (res['msg']) {
                this.triggerToast(res['msg'], res['msg'], '');
                this.getAllCompanyDD();
                window.location.reload();
              } else if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], '');
                this.getAllCompanyDD();
              }
              this.isSpinner = false;
            }, error: (err: any) => {
              this.isSpinner = false;
              this.triggerToast('Internal Server Error', 'Failed To Add The Records', 'danger')
            }
          })
          this.addForm.reset();
          this.isFormSubmittedAddForm = false;
          // this.currentAddType = null;
        }
        break;

      case 'entity':
        this.isFormSubmittedAddForm = true;
        if (this.addForm?.get('CompId').valid && this.addForm?.get('LegalEntity').valid) {
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            CompId: Number(this.addForm?.get('CompId').value),
            LegalEntity: this.addForm?.get('LegalEntity').value,
            Description: this.addForm?.get('Description').value,
            CompanyType: this.addForm?.get('CompanyType').value,
            Website: this.addForm?.get('Website').value,

            LOGO: this.getLeaveDocPath?.LOGO || '',
            LOGOWITHADDRESS: this.getLeaveDocPath?.LOGOWITHADDRESS || '',
            WEBAPPLOGO: this.getLeaveDocPath?.WEBAPPLOGO || ''
          };
          console.log(reqBody)
          this.isSpinner = true;
          this.settingService.AddLegalEntity(reqBody).subscribe({
            next: (res: any) => {
              if (res['msg']) {
                this.triggerToast(res['msg'], res['msg'], '');
                this.getAllLegalEntityDD();
                window.location.reload();
              } else if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], '');
                this.getAllLegalEntityDD();
              }
              this.isSpinner = false;
            }, error: (err: any) => {
              this.isSpinner = false;
              this.triggerToast('Internal Server Error', 'Failed To Add The Records', 'danger')
            }
          })
          this.addForm.reset();
          this.isFormSubmittedAddForm = false;
          // this.currentAddType = null;
        }
        break;

      case 'businessUnit':
        this.isFormSubmittedAddForm = true;
        if (this.addForm?.get('CompNameBusiness').valid && this.addForm?.get('legalEntityBusiness').valid && this.addForm?.get('businessUnitName').valid) {
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            CompId: Number(this.addForm?.get('CompNameBusiness').value),
            LEId: this.addForm?.get('legalEntityBusiness').value,
            Businessunit: this.addForm?.get('businessUnitName').value,
            Description: this.addForm?.get('businessUnitDesc').value,
          }
          this.isSpinner = true;
          this.settingService.AddBusinessUnit(reqBody).subscribe({
            next: (res: any) => {
              if (res['msg']) {
                this.triggerToast(res['msg'], res['msg'], '');
                this.getAllBusinessUnit();
                window.location.reload();
              } else if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], '');
                this.getAllBusinessUnit();
              }
              this.isSpinner = false;
            }, error: (err: any) => {
              this.isSpinner = false;
              this.triggerToast('Internal Server Error', 'Failed To Add The Records', 'danger')
            }
          })
          this.addForm.reset();
          this.isFormSubmittedAddForm = false;
          // this.currentAddType = null;
        }
        break;

      case 'location':
        // Logic here
        break;
    }
  }

  updateAddForm() {
    switch (this.currentAddType) {
      case 'company':
        this.isFormSubmittedAddForm = true;
        if (this.addForm?.get('Company').valid) {
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            CompId: Number(this.patchComapnyData.CompId),
            Company: this.addForm?.get('Company').value,
            CompanyCode: this.addForm?.get('CompanyCode').value,
            LocationMap: this.addForm?.get('LocationMap').value,
            Address: this.addForm?.get('Address').value,
          }
          this.isSpinner = true;
          this.settingService.UpdateCompany(reqBody).subscribe({
            next: (res: any) => {
              if (res['msg']) {
                this.triggerToast(res['msg'], res['msg'], '');
                this.getAllCompanyDD();
              } else if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], '');
                this.getAllCompanyDD();
              }
              this.isSpinner = false;
            }, error: (err: any) => {
              this.isSpinner = false;
              this.triggerToast('Internal Server Error', 'Failed To Add The Records', 'danger')
            }
          })
          this.addForm.reset();
          this.isFormSubmittedAddForm = false;
          // this.currentAddType = null;
        }
        break;

      case 'entity':
        this.isFormSubmittedAddForm = true;
        if (this.addForm?.get('LegalEntity').valid) {
          const reqBody = {
           LoginId: this.employeeDetails[0].LoginId,
            CompId: Number(this.addForm?.get('CompId').value),
            LegalEntity: this.addForm?.get('LegalEntity').value,
            Description: this.addForm?.get('Description').value,
            CompanyType: this.addForm?.get('CompanyType').value,
            Website: this.addForm?.get('Website').value,

            LOGO: this.getLeaveDocPath?.LOGO || '',
            LOGOWITHADDRESS: this.getLeaveDocPath?.LOGOWITHADDRESS || '',
            WEBAPPLOGO: this.getLeaveDocPath?.WEBAPPLOGO || ''
          };
          this.isSpinner = true;
          this.settingService.UpdateLegalEntity(reqBody).subscribe({
            next: (res: any) => {
              if (res['msg']) {
                this.triggerToast(res['msg'], res['msg'], '');
                this.getAllLegalEntityDD();
              } else if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], '');
                this.getAllLegalEntityDD();
              }
              this.isSpinner = false;
            }, error: (err: any) => {
              this.isSpinner = false;
              this.triggerToast('Internal Server Error', 'Failed To Add The Records', 'danger')
            }
          })
          this.addForm.reset();
          this.isFormSubmittedAddForm = false;
          // this.currentAddType = null;
        }
        break;

      case 'businessUnit':
        this.isFormSubmittedAddForm = true;
        if (this.addForm?.get('CompNameBusiness').valid && this.addForm?.get('legalEntityBusiness').valid && this.addForm?.get('businessUnitName').valid) {
          const reqBody = {
            LoginId: this.employeeDetails[0].LoginId,
            CompId: Number(this.patchBusinessUnitData.CompId),
            LEId: Number(this.patchBusinessUnitData.LEId),
            BUId: Number(this.patchBusinessUnitData.BUId),
            BusinessUnit: this.addForm?.get('businessUnitName').value,
            Description: this.addForm?.get('businessUnitDesc').value,
          }
          this.isSpinner = true;
          this.settingService.UpdateBusinessUnit(reqBody).subscribe({
            next: (res: any) => {
              if (res['msg']) {
                this.triggerToast(res['msg'], res['msg'], '');
                this.getAllBusinessUnit();
              } else if (res['Message']) {
                this.triggerToast(res['Message'], res['Message'], '');
                this.getAllBusinessUnit();
              }
              this.isSpinner = false;
            }, error: (err: any) => {
              this.isSpinner = false;
              this.triggerToast('Internal Server Error', 'Failed To Add The Records', 'danger')
            }
          })
          this.addForm.reset();
          this.isFormSubmittedAddForm = false;
          // this.currentAddType = null;
        }
        break;

      case 'location':
        // Logic here
        break;
    }
  }

  recordToDelete: any = null;
  deleteType: 'company' | 'entity' | 'businessUnit' | 'location' | null = null;

  confirmDelete(row: any) {
    console.log(row)
    this.recordToDelete = row;
    this.deleteType = this.currentAddType;
    // this.isRecordDeleted = false;
  }

  deleteRecord() {
    if (!this.recordToDelete || !this.deleteType) return;
    const key = deleteKeyMap[this.deleteType];
    if (!key) {
      this.triggerToast('Delete Failed', 'Unknown delete type', 'danger');
      return;
    }
    const reqBody: any = {
      LoginId: this.employeeDetails[0].LoginId,
      [key]: this.recordToDelete[key]
    };
    this.isSpinner = true;
    switch (this.deleteType) {
      case 'company':
        this.settingService.DeleteCompany(reqBody).subscribe({
          next: (res: any) => {
            this.isSpinner = true;
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], '');
              this.isRecordDeletedCommon = true;
              setTimeout(() => {
                this.closeModalDelete.nativeElement?.click();
                this.getAllCompanyDD();
                window.location.reload();
                setTimeout(() => {
                  this.isRecordDeletedCommon = false;
                }, 1100);
              }, 1000);
            } else if (res['Message']) {
              this.triggerToast(res['Message'], res['Message'], '');
              this.getAllCompanyDD();
            }
            this.isSpinner = false;
          }, error: (err: any) => {
            this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
            this.isSpinner = false;
          }
        });
        break;

      case 'entity':
        this.settingService.DeleteLegalEntity(reqBody).subscribe({
          next: (res: any) => {
            this.isSpinner = true;
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], '');
              this.isRecordDeletedCommon = true;
              setTimeout(() => {
                this.closeModalDelete.nativeElement?.click();
                this.getAllLegalEntityDD();
                window.location.reload();
                setTimeout(() => {
                  this.isRecordDeletedCommon = false;
                }, 1100);
              }, 1000);

            } else if (res['Message']) {
              this.triggerToast(res['Message'], res['Message'], '');
              this.getAllLegalEntityDD();
            }
            this.isSpinner = false;
          }, error: (err: any) => {
            this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
            this.isSpinner = false;
          }
        });
        break;

      case 'businessUnit':
        this.settingService.DeleteBusinessUnit(reqBody).subscribe({
          next: (res: any) => {
            this.isSpinner = true;
            if (res['msg']) {
              this.triggerToast(res['msg'], res['msg'], '');
              this.isRecordDeletedCommon = true;
              setTimeout(() => {
                this.closeModalDelete.nativeElement?.click();
                this.getAllBusinessUnit();
                window.location.reload();
                setTimeout(() => {
                  this.isRecordDeletedCommon = false;
                }, 1100);
              }, 1000);

            } else if (res['Message']) {
              this.triggerToast(res['Message'], res['Message'], '');
              this.getAllBusinessUnit();
            }
            this.isSpinner = false;
          }, error: (err: any) => {
            this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
            this.isSpinner = false;
          }
        });
        break;

      case 'location':
        break;

      default:
        this.triggerToast('Error', 'Invalid delete type', 'danger');
        this.isSpinner = false;
        return;
    }
  }


  resetData() {
    this.companyCreationForm.reset();
    this.isFormSubmitted = false;
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }



  loadCalendarYears() {
    this.settingService.getAllCalendarYears(this.employeeDetails[0].EmpId).subscribe({
      next: (res) => {
        this.calendarYears = res;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }


  // ================= FINANCIAL ================= //

  loadFinancialYears() {
    this.settingService.getAllFinancialYears(this.employeeDetails[0].EmpId).subscribe({
      next: (res) => {
        this.financialYears = res;
      },
      error: (err) => console.error(err)
    });
  }

  showCalendarModal = false;
  showFinancialModal = false;
  isCalendarEdit = false
  calendarYearForm: any
  // ===================== CALENDAR =====================

  openCalendarModal() {
    this.isCalendarEdit = false;
    this.calendarYearForm = { Id: 0, Year: null };
    this.showCalendarModal = true;
  }

  editCalendarYear(row: any) {
    this.isCalendarEdit = true;
    this.calendarYearForm = {
      Id: row.Id,
      Year: row.Year
    };
    this.showCalendarModal = true;
  }

  saveCalendarYear() {
    if (!this.calendarYearForm.Year) return;

    const apiCall = this.isCalendarEdit
      ? this.settingService.updateCalendarYear(
        this.employeeDetails[0].EmpId,
        this.calendarYearForm.Id,
        this.calendarYearForm.Year
      )
      : this.settingService.addCalendarYear(
        this.employeeDetails[0].EmpId,
        this.calendarYearForm.Year
      );

    apiCall.subscribe(() => {
      this.loadCalendarYears();
      this.showCalendarModal = false;
    });
  }

  closeCalendarModal() {
    this.showCalendarModal = false;
  }
  isFinancialEdit = false;
  financialYearForm: any;
  openFinancialModal() {
    this.isFinancialEdit = false;
    this.financialYearForm = { YearId: 0, FinancialYear: '' };
    this.showFinancialModal = true;
  }

  editFinancialYear(row: any) {
    this.isFinancialEdit = true;
    this.financialYearForm = {
      YearId: row.YearId,
      FinancialYear: row.FinancialYear
    };
    this.showFinancialModal = true;
  }

  saveFinancialYear() {
    if (!this.financialYearForm.FinancialYear) return;

    const apiCall = this.isFinancialEdit
      ? this.settingService.updateFinancialYear(
        this.employeeDetails[0].EmpId,
        this.financialYearForm.YearId,
        this.financialYearForm.FinancialYear
      )
      : this.settingService.addFinancialYear(
        this.employeeDetails[0].EmpId,
        this.financialYearForm.FinancialYear
      );

    apiCall.subscribe(() => {
      this.loadFinancialYears();
      this.showFinancialModal = false;
    });
  }

  closeFinancialModal() {
    this.showFinancialModal = false;
  }
  deleteCalendarYear(id: any) {
    if (!id) return;
    console.log(id)
    this.isSpinner = true;

    this.settingService.deleteCalendarYear(this.employeeDetails[0].EmpId, id.Id).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast('Deleted', 'Financial year deleted successfully', 'success');
          this.loadCalendarYears();
        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning')
        }
        this.isSpinner = false;
      },
      error: (err) => {
        console.error(err);
        this.triggerToast('Error', 'Failed to delete calendar year', 'danger');
        this.isSpinner = false;
      }
    });
  }

  deleteFinancialYear(id: any) {
    if (!id) return;
    this.isSpinner = true;
    this.settingService.deleteFinancialYear(this.employeeDetails[0].EmpId, id.Id).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast('Deleted', 'Financial year deleted successfully', 'success');
          this.loadFinancialYears();
        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning')
        }
        this.isSpinner = false;
      },
      error: (err) => {
        console.error(err);
        this.triggerToast('Error', 'Failed to delete financial year', 'danger');
        this.isSpinner = false;
      }
    });
  }

}
