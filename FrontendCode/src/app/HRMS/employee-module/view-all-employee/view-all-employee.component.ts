import { Component, OnInit, ViewChild, ElementRef, OnDestroy, HostListener } from '@angular/core';
import { HrmsServiceService } from '../../hrms-service.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { EmployeeModuleService } from '../../service/employee.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs'
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { payRollService } from '../../service/payroll.service';

@Component({
  selector: 'app-view-all-employee',
  standalone: true,
  imports: [SharedModule, CommonModule, NgxPaginationModule, ToastMessageComponent,
    RouterModule, ReactiveFormsModule
  ],
  templateUrl: './view-all-employee.component.html',
  styleUrl: './view-all-employee.component.scss'
})
export class ViewAllEmployeeComponent implements OnInit, OnDestroy {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModal') closeModal!: ElementRef;
  @ViewChild('closeModalRelieve') closeModalRelieve!: ElementRef;
  @ViewChild('inputValue') inputValue!: ElementRef;


  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  public isComingBack: boolean = false;

  employeeFilterForm: any = FormGroup;
  getDDCompany: any;
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];

  accessPolicy: any;
  controlAccessPage: any;
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  viewdata: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 500];
  viewData: any;
  deleteForm: any = FormGroup;
  RelieveForm: any = FormGroup;
  employeeDetails: any;
  isActive = false;
  totalEmployees: any
  activeEmployeeDetails: any
  inactiveEmployeeDetails: any
  relievedEmployeeDetails: any
  maleEmployeeDetails: any
  femaleEmployeeDetails: any
  isTableData: boolean = false;
  isRecordDeleted: boolean = false;
  statusOptions = ['ALL', 'Active', 'Deactive', 'Relieved'];
  selectedStatus = 'ALL';

  isFormSubmitted: boolean = false;
  minDate: string | undefined;
  maxDate: string | undefined;
  today = new Date().toISOString().split('T')[0];
  minDateCareer: string | undefined;
  maxDateCareer: string | undefined;
  yesterday: string | undefined;
  dropdownVisible = false;


  tabs: any[] = [];

  allTabs = [
    {
      id: 'create_employee',
      title: 'Create Employee',
      type: 'item',
      url: '/create_employee',
      icon: 'feather icon-user-plus'
    },
    {
      id: 'employee_probation_report',
      title: 'Employee Probation Report',
      type: 'item',
      url: '/employee_probation_report',
      icon: 'feather icon-clipboard'
    },
    {
      id: 'employee_loghistroy_report',
      title: 'Employee Log Report',
      type: 'item',
      url: '/employee_loghistroy_report',
      icon: 'feather icon-file-text'
    }
  ];

  selectedTab = 0;

  selectTab(index: number) {
    this.selectedTab = index;
    const selected = this.tabs[index];
    if (selected?.url) {
      this.router.navigate([selected.url]);
    }
  }
  constructor(private readonly hrmsEmployeeService: EmployeeModuleService,
    private readonly router: Router,
    private readonly fb: FormBuilder,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly hrmsServiceMain: HrmsServiceService,
    private entityStateService: EntityStateService,
    private route: ActivatedRoute,
    private readonly payrollLocationDD: payRollService,
  ) {
    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // console.log(this.accessPolicy);
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Employee'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    const yesterdayDate = new Date();
    yesterdayDate.setDate(yesterdayDate.getDate() - 1);
    this.yesterday = yesterdayDate.toISOString().split('T')[0];
  }

  // getEntityId(): number {
  //   const storedEntityId = sessionStorage.getItem('SelectedLEId');
  //   return storedEntityId ? Number(storedEntityId) : 0;  // If no value, return 0
  // }

  ngOnInit(): void {
    // ✅ FIRST create the form
    this.employeeFilterForm = this.fb.group({
      date_from: [''],
      date_to: [''],
      BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      statusEmpForm: [''],
      employeeType: [''],
    }, { validators: this.careerDateValidator.bind(this) });
    this.deleteForm = this.fb.group({
      reason: ['']
    });
    this.RelieveForm = this.fb.group({
      isRelieved: [true],
      date_from: ['', [Validators.required]],
      date_to: ['', [Validators.required]],
      relievingReason: [],
    }, { validators: this.dateRangeValidator });
    // ✅ THEN subscribe to query params
    this.route.queryParams.subscribe(params => {
      if (Object.keys(params).length > 0) {
        // coming back from edit page
        this.isComingBack = true;

        // patch form after dropdowns loaded
        this.employeeFilterForm.patchValue(params);
        this.employeeFilterForm.patchValue({
          BusinessUnit: params['BusinessUnit'],
          DeptName: params['DeptName']
        });
        if (params['DeptName']) {
          this.callDDDesignation();
        }
        if (params['BusinessUnit']) {

        }
        this.submitFilterData();
      } else {
        this.getAllEmployeeList();
      }
    });
    // Rest of your code
    this.getBusinessUnit();
    setTimeout(() => {
      this.access_DD_department();
      setTimeout(() => {
        this.callLocation();
      }, 200);
      setTimeout(() => {
        this.getDDEmpTypeList();
      }, 200);
    }, 200);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Employee'
      );
      this.tabs = this.allTabs.filter(tab =>
        this.accessPolicy.some((p: any) => p.PageName === tab.title && p.ViewAccess)
      );
    });
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;

        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.getBusinessUnit();
          this.resetData();
        }
        this.currentEntityId = newEntityId;
      });
  }


  ngAfterViewInit() {
    setTimeout(() => {
      this.isComingBack = false;
    }, 500); // small delay after first patch
  }


  ngOnDestroy(): void {
    this.entitySubscription?.unsubscribe();
  }

  //this is EmpForm

  careerDateValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    const errors: any = {};
    // Check: date_from <= date_to
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      errors.dateRange = true;
    }
    return Object.keys(errors).length > 0 ? errors : null;
  }

  onFromDateCareer(): void {
    if (this.employeeFilterForm.get('date_from')?.value) {
      this.minDateCareer = this.employeeFilterForm.get('date_from')?.value;
    }
  }
  onToDateCareer(): void {
    if (this.employeeFilterForm.get('date_to')?.value) {
      this.maxDateCareer = this.employeeFilterForm.get('date_to')?.value;
    }
  }
  isFromDateInvalidEmpForm(): boolean {
    const control = this.employeeFilterForm.get('date_from');
    if (!control) return false;

    return (
      (control.touched || this.isFormSubmitted) &&
      (control.invalid || this.employeeFilterForm.hasError('careerDateComparison'))
    );
  }

  isToDateInvalidEmpForm(): boolean {
    const toDate = this.employeeFilterForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeErrorEmpForm(): boolean {
    return this.employeeFilterForm.hasError('dateRange');
  }
  //this is EmpForm

  //this is for  RelieveForm
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }

  onFromDate(): void {
    if (this.RelieveForm.get('date_from')?.value) {
      this.minDate = this.RelieveForm.get('date_from')?.value;
    }
  }
  onToDate(): void {
    if (this.RelieveForm.get('date_to')?.value) {
      this.maxDate = this.RelieveForm.get('date_to')?.value;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.RelieveForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.RelieveForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.RelieveForm.hasError('dateRange');
  }
  //this is for  RelieveForm

  // employee_DD_Comapny() {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId
  //   };
  //   this.isSpinner1 = true;
  //   this.hrmsEmployeeService.employeeDDCompany(reqBody).subscribe({
  //     next: (res: any) => {
  //       if (res.length >= 1) {
  //         this.getDDCompany = res;
  //       } else {
  //         this.triggerToast(res['Message'], 'Sorry No Data Found', 'warning');
  //       }
  //       this.isSpinner1 = false;
  //     },
  //     error: (error: any) => {
  //       this.triggerToast('Internal Server Error', 'Error loading Company Name', 'danger');
  //       this.isSpinner1 = false;
  //     }
  //   });
  // }

  // calllegalEntity(event: any) {
  //   const reqBody = {
  //     EmpId: this.employeeDetails[0].EmpId,
  //     AuthorisedEntity: this.entityStateService.getEntityId(),
  //     CompId: Number(this.employeeFilterForm?.get('company').value)
  //   }
  //   this.isSpinner1 = true;
  //   this.getLegalEntity = []
  //   this.hrmsEmployeeService.employeeDDLegalEntity(reqBody).subscribe((res: any) => {
  //     setTimeout(() => {
  //       this.employeeFilterForm?.get('LegalEntity').reset();
  //       this.employeeFilterForm?.get('BusinessUnit').reset();
  //       this.employeeFilterForm?.get('Location').reset();
  //     }, 100);
  //     if (res.length >= 1) {
  //       this.getLegalEntity = res;
  //       this.isSpinner1 = false;
  //     } else {
  //       this.triggerToast(res['Message'], "No Data Found For Legal Entity", "warning");
  //       this.isSpinner1 = false;
  //       this.getLegalEntity = []
  //     }
  //   },
  //     error => {
  //       this.errorMessage = 'Error loading data. Please try again.';
  //       this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
  //       this.isSpinner1 = false;
  //     })
  // }
  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner1 = true;
    this.getBusinessUnitlist = [];
    // this.getLocations = []
    setTimeout(() => {
      this.hrmsEmployeeService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          // this.employeeFilterForm?.get('BusinessUnit').reset();
          if (!this.isComingBack) {
            this.employeeFilterForm?.get('BusinessUnit').reset();
          }
          this.getBusinessUnitlist = res;
          this.isSpinner1 = false;
        } else {
          this.isSpinner1 = false;
          this.getBusinessUnitlist = [];
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
          this.isSpinner1 = false;
        })
    }, 100);
  }

  callLocation() {
    // const reqBody = {
    //   EmpId: this.employeeDetails[0].EmpId,
    //   AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    //   CompId: 1,
    //   LEId: Number(this.entityStateService.getEntityId()),
    //   BUId: Number(this.employeeFilterForm?.get('BusinessUnit').value) ? Number(this.employeeFilterForm?.get('BusinessUnit').value) : 0,
    // }
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner1 = true;
    this.getLocations = []
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          // this.employeeFilterForm?.get('Location').reset();
          if (!this.isComingBack) {
            this.employeeFilterForm?.get('Location').reset();
          }

          this.getLocations = res;
          this.isSpinner1 = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner1 = false;
          this.getLocations = []
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner1 = false;
          this.getLocations = []
        })
    }, 100);
  }
  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner1 = true;
    this.hrmsServiceMain.access_DD_department(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDepartementName = res;
        } else {
          this.triggerToast('', 'Record Not Found', 'Warning');
        }
        this.isSpinner1 = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Department List', 'danger');
        this.isSpinner1 = false;
      }
    });
  }

  callDDDesignation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.employeeFilterForm?.get('DeptName')?.value,
    };
    this.isSpinner1 = true;
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe({
      next: (res: any) => {
        this.getDepartementRole = res;
        this.isSpinner1 = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error loading Designation', 'danger');
        this.isSpinner1 = false;
      }
    });
  }

  // getAllEmployeeList() {
  //   const reqBody = { LoginId: this.employeeDetails[0].LoginId };
  //   this.isSpinner = true;

  //   this.hrmsEmployeeService.employeeGetAllEmployee(reqBody).subscribe(
  //     (res: any) => {
  //       if (res.length >= 1) {
  //         this.isTableData = false;

  //         setTimeout(() => {
  //           this.totalEmployees = res.length;

  //           // Separate lists if needed
  //           this.activeEmployeeDetails = res.filter((e: any) => e.EmpStatus === "Active");
  //           this.relievedEmployeeDetails = res.filter((e: any) => e.EmpStatus === "RELIVED");
  //           this.inactiveEmployeeDetails = res.filter(
  //             (e: any) =>
  //               e.EmpStatus === "Inactive" ||
  //               e.EmpStatus === "Deactive" ||
  //               e.EmpStatus === "0"
  //           );

  //           this.maleEmployeeDetails = res.filter((e: any) => e.Gender === "Male");
  //           this.femaleEmployeeDetails = res.filter((e: any) => e.Gender === "Female");

  //           // Sort rows: Active first, then others
  //           this.rows = res.sort((a: any, b: any) => {
  //             if (a.EmpStatus === "Active" && b.EmpStatus !== "Active") return -1;
  //             if (a.EmpStatus !== "Active" && b.EmpStatus === "Active") return 1;
  //             return 0; // keep relative order for non-active
  //           });

  //           this.originalRows = this.rows;
  //           this.isSpinner = false;
  //         }, 1000);
  //       } else {
  //         this.errorMessage = "No records found";
  //         this.isSpinner = false;
  //         this.isTableData = true;
  //       }
  //     },
  //     (error) => {
  //       this.errorMessage = "Internal Server Error";
  //       this.isSpinner = false;
  //       this.isTableData = true;
  //     }
  //   );
  // }

  submitFilterData() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: 0,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: this.employeeFilterForm?.get('BusinessUnit')?.value || 0,
      LocationId: this.employeeFilterForm?.get('Location')?.value || 0,
      DeptId: this.employeeFilterForm?.get('DeptName')?.value || 0,
      DesignationId: this.employeeFilterForm?.get('Designation')?.value || 0,
      FromDate: this.employeeFilterForm?.get('date_from')?.value || null,
      ToDate: this.employeeFilterForm?.get('date_to')?.value || null,
      Status: this.employeeFilterForm?.get('statusEmpForm')?.value || ""
    };

    this.isSpinner = true;
    this.hrmsEmployeeService.employeeGetAllEmployee(reqBody).subscribe(
      (res: any[]) => {
        this.processEmployeeResponse(res);
      },
      () => {
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
      }
    );
  }

  getAllEmployeeList() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.hrmsEmployeeService.employeeGetAllEmployee(reqBody).subscribe(
      (res: any[]) => {
        this.processEmployeeResponse(res);
      },
      () => {
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
      }
    );
  }

  public showJoiningDate: boolean = false;
  public showRelievedDate: boolean = false;

  private processEmployeeResponse(res: any[]) {

    const parseDotNetDate = (dateString: string | null): Date | null => {
      if (!dateString) return null;
      const match = /\/Date\((\d+)\)\//.exec(dateString);
      return match ? new Date(+match[1]) : null;
    };

    if (res && res.length >= 1) {
      this.isTableData = false;

      const formattedData = res.map((e: any) => ({
        ...e,
        DOB: parseDotNetDate(e.DOB),
        JoiningDate: parseDotNetDate(e.JoiningDate),
        RelievedDate: parseDotNetDate(e.RelievedDate),
        EndDate: parseDotNetDate(e.EndDate)
      }));

      this.totalEmployees = formattedData.length;

      const normalizeStatus = (status: string) =>
        status ? status.toLowerCase() : '';

      this.activeEmployeeDetails = formattedData.filter(
        (e: any) => normalizeStatus(e.EmpStatus) === "active"
      );

      this.relievedEmployeeDetails = formattedData.filter(
        (e: any) => normalizeStatus(e.EmpStatus) === "relieved"
      );

      this.inactiveEmployeeDetails = formattedData.filter(
        (e: any) => {
          const status = normalizeStatus(e.EmpStatus);
          return status === "inactive" || status === "deactive" || status === "0";
        }
      );
      this.maleEmployeeDetails = formattedData.filter(
        (e: any) => e.Gender === "Male"
      );
      this.femaleEmployeeDetails = formattedData.filter(
        (e: any) => e.Gender === "Female"
      );

      this.showJoiningDate = this.activeEmployeeDetails.length > 0;
      this.showRelievedDate = this.relievedEmployeeDetails.length > 0;
      
      this.rows = [...formattedData].sort((a: any, b: any) => {
        const statusA = normalizeStatus(a.EmpStatus);
        const statusB = normalizeStatus(b.EmpStatus);
        if (statusA === "active" && statusB !== "active") return -1;
        if (statusA !== "active" && statusB === "active") return 1;
        return 0;
      });

      this.originalRows = [...this.rows];

    } else {
      this.errorMessage = "No records found";
      this.isTableData = true;
      this.page = 1;
    }

    this.isSpinner = false;
  }

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const isDropdown = target.closest('.dropdown-content') !== null;
    const isButton = target.matches('.export-button');
    if (!isDropdown && !isButton) {
      this.dropdownVisible = false;
    }
  }
  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
  }

  exportFile(format: string) {
    if (format === 'excel') {
      this.exportToExcel();
    }

  }

  exportToExcel(): void {
    if (!this.rows || this.rows.length === 0) return;

    // 1️⃣ Map only required fields
    const exportData = this.rows.map(row => ({
      'Employee Code': row.EmpCode,
      'Name': `${row.FirstName} ${row.MiddleName} ${row.LastName}`,
      'Email': row.EmailId,
      'Department': row.DeptName,
      'Designation': row.Designation,
      'Company': row.Company,
      'Gender': row.Gender,
      'Employment Status': row.EmpStatus,
      'Rept.Manager': row.ReportEmpCode,
      'Joining Date': row.JoiningDate ? this.formatDate(row.JoiningDate) : '',
      'Relieved Date': row.RelievedDate ? this.formatDate(row.RelievedDate) : ''
    }));

    // 2️⃣ Create worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);

    // 3️⃣ Create workbook
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Employees': worksheet },
      SheetNames: ['Employees']
    };

    // 4️⃣ Write workbook
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

    // 5️⃣ Save file
    const blob: Blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
    saveAs(blob, 'EmployeeData.xlsx');
  }

  // Helper function to format date as dd-MM-yyyy
  private formatDate(date: Date): string {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    return `${day}-${month}-${year}`;
  }


  viewAll(category: string) {
    this.rows = [...this.originalRows];

    const normalize = (status: string) => (status ? status.toLowerCase() : '');

    switch (category.toLowerCase()) {
      case 'total':
        this.rows = [...this.originalRows];
        this.page = 1;
        break;

      case 'active':
        this.rows = this.originalRows.filter(
          (employee: any) => normalize(employee.EmpStatus) === 'active'
        );
        this.page = 1;
        break;

      case 'inactive':
        this.rows = this.originalRows.filter(
          (employee: any) => {
            const status = normalize(employee.EmpStatus);
            return status === 'inactive' || status === 'deactive' || status === '0';
          }
        );
        this.page = 1;
        break;

      case 'relieved':
        this.rows = this.originalRows.filter(
          (employee: any) => normalize(employee.EmpStatus) === 'relieved'
        );
        this.page = 1;
        break;

      case 'male':
        this.rows = this.originalRows.filter(
          (employee: any) => normalize(employee.Gender) === 'male'
        );
        this.page = 1;
        break;

      case 'female':
        this.rows = this.originalRows.filter(
          (employee: any) => normalize(employee.Gender) === 'female'
        );
        this.page = 1;
        break;

      default:
        this.rows = [...this.originalRows];
        this.page = 1;
        break;
    }

    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = "No records found for this category";
    } else {
      this.isTableData = false;
      this.errorMessage = null;
    }
  }

  resetData() {
    this.employeeFilterForm.reset();
    this.rows = [];
    this.originalRows = [];
    this.totalEmployees = 0;
    this.activeEmployeeDetails = [];
    this.inactiveEmployeeDetails = [];
    this.relievedEmployeeDetails = [];
    this.isTableData = false;
    this.errorMessage = null;
    this.page = 1;
    if (this.inputValue?.nativeElement) {
      this.inputValue.nativeElement.value = '';
    }
    this.getAllEmployeeList();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();

    // Only filter rows when the filter value exists
    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) =>
      // Check if any part of the name or code contains the filter value
      (row.FirstName?.toUpperCase().includes(filterValue) ||
        row.LastName?.toUpperCase().includes(filterValue) ||
        row.EmpCode?.toUpperCase().includes(filterValue))
      );
    } else {
      // If no filter value, reset the rows to the original list
      this.isTableData = false;
      this.rows = [...this.originalRows];
    }

    // If no rows are found, show error message
    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = 'No Records Found for Searched Data';
    } else {
      this.isTableData = false;
      this.errorMessage = null;
    }
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

  onViewEdit(data: any) {
    this.isComingBack = true;
    this.router.navigate(['update_all_employee'], {
      queryParams: {
        EmpId: data.EmpId,
        ...this.employeeFilterForm.value
      }
    });
  }

  onViewDelete(data: any) {
    this.viewData = data
  }


  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
  deleteEmployee() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.viewData.EmpId,
      Reason: this.deleteForm?.get('reason').value,
    }
    this.hrmsEmployeeService.employeeDeleteEmployee(reqBody).subscribe((res: any) => {
      if (res['msg'] === "Deleted") {
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.deleteForm.reset()
        this.isRecordDeleted = true;
        this.getAllEmployeeList();
        setTimeout(() => {
          this.closeModal.nativeElement?.click();
          setTimeout(() => {
            this.isRecordDeleted = false;
          }, 1100);
        }, 1000);
      } else if (res['Message']) {
        this.triggerToast(res['Message'], '', 'warning');
      }
      else {
        this.triggerToast(res['msg'], 'Something went wrong', 'warning');
      }
    }, error => {
      this.triggerToast(error, 'Internal Server Error', 'danger');
    })
  }

  resetDeleteForm() {
    this.deleteForm.reset()
  }
  relievedSeletedRow: any;
  onViewRelieved(row: any) {
    this.relievedSeletedRow = row;
    console.log(row);
  }

  relieveSubmit() {
    if (this.RelieveForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.relievedSeletedRow.EmpId,
        RelievedReason: this.RelieveForm?.get('relievingReason').value,
        RelievedEffectiveDate: this.RelieveForm?.get('date_from').value,
        RelievedDate: this.RelieveForm?.get('date_to').value,
        IsRelieved: this.RelieveForm?.get('isRelieved').value
      }
      this.isSpinner1 = true;
      this.hrmsEmployeeService.employeeRelievedEmployee(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === 'Relieved') {
            this.isSpinner1 = false;
            this.closeModalRelieve.nativeElement?.click();
            this.getAllEmployeeList();
            this.RelieveForm?.get('date_to').reset();
            this.RelieveForm?.get('relievingReason').reset();
            this.triggerToast('', 'Relieved Successfully', 'success')
          } else if (res['Message']) {
            this.triggerToast(res['Message'], 'Relieved Failed', 'warning')
          }
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'Failed To Relieved', 'danger');
          this.isSpinner1 = false;
        }
      })
    } else {
      this.isFormSubmitted = true
    }
  }
  resetRelieve() {
    this.isFormSubmitted = false;
    this.RelieveForm.reset();
    this.RelieveForm?.get('isRelieved').setValue(true)
  }

  toggleState(row: any): void {
    row.EmpStatus = row.EmpStatus === 'Active' ? 'Inactive' : 'Active';
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: row.EmpId,
      Reason: "Performenec Issue",
    };
    if (row.EmpStatus === 'Active') {
      this.hrmsEmployeeService.employeeActiveEmployee(reqBody).subscribe((res: any) => {
        if (res['msg'] === 'Actived') {
          this.triggerToast('Activated', 'Employee Activated Successfully', 'success');
          this.getAllEmployeeList();
        } else {
          this.triggerToast(res['msg'], res['msg'], 'warning');
        }
      }, error => {
        this.triggerToast('Error', 'Activation Failed', 'error');
      });
    } else if (row.EmpStatus === 'Inactive') {
      this.hrmsEmployeeService.employeeDeActiveEmployee(reqBody).subscribe((res: any) => {
        if (res['msg'] === 'Deactived') {
          this.triggerToast('Deactivated', 'Employee Deactivated Successfully', 'success');
          this.getAllEmployeeList();
        } else {
          this.triggerToast(res['msg'], res['msg'], 'warning');
        }
      }, error => {
        this.triggerToast('Error', 'Deactivation Failed', 'error');
      });
    }
  }
  getEmployeeTypeList: any[] = [];

  getDDEmpTypeList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.hrmsEmployeeService.employeeDDEmpType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getEmployeeTypeList = res;
      } else {
        this.getEmployeeTypeList = []
      }
    },
      error => {
        this.getEmployeeTypeList = []
      })
  }
}
