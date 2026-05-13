import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { catchError, finalize, of, Subscription } from 'rxjs';
import { SettingsService } from '../service/settings.service';
import { ActivatedRoute } from '@angular/router';
import { HrmsServiceService } from '../hrms-service.service';
import { AccessPolicyStoreService } from '../service/accessPolicayApi.service';
import { EmployeeModuleService } from '../service/employee.service';
import { EntityStateService } from '../service/entity-state.service';
import { payRollService } from '../service/payroll.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';

interface Employee {
  EmpId: number;
  EmpCode: string;
  EmployeeName: string;
  DesignationName: string;
  GradeId: number;
  GradeName: string;
  DeptName: string;
  DeptShortName: string;
  LocationId?: number;
  ReporteesCount: number;
  Reportees: Employee[];
  IsLoggedInUser?: boolean;
  isExpanded?: boolean; // For tree expansion
  level?: number; // Tree level for styling
}

@Component({
  selector: 'app-org-chart',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, ToastMessageComponent],
  templateUrl: './org-chart.component.html',
  styleUrls: ['./org-chart.component.scss']
})
export class OrgChartComponent implements OnInit {
  private readonly LOGGED_IN_EMP_ID = 149;

  treeData: Employee[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';
  searchTerm: string = '';
  selectedGrade: string = '';
  availableGrades: string[] = [];
  accessPolicy: any;
  controlAccessPage: any;
  employeeDetails;
  entitySubscription!: Subscription;

  isFormSubmitted: boolean = false;
  hierarchyForm: any = FormGroup;
  isTableData: boolean = false;
  currentEntityId: number | null = null;
  rows: any[] = [];
  getLegalEntity: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  getDropdownReporter: any[] = [];
  getDropdownEmployee: any[] = [];

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  private http = inject(HttpClient);


  constructor(private readonly fb: FormBuilder,
    private readonly hrmsEmployeeModuleService: EmployeeModuleService,
    private readonly fromQueryParams: ActivatedRoute,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly cdr: ChangeDetectorRef,
    private eRef: ElementRef,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private entityStateService: EntityStateService,
    private readonly payrollLocationDD: payRollService,
  ) {
    const accessPolicy = sessionStorage.getItem('accessPolicy');
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Employee Probation Report'
      );
    });

    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

  }
  ngOnInit(): void {
    this.hierarchyForm = this.fb.group({
      CompId: [1],
      LEId: [Number(this.entityStateService.getEntityId())],
      BUId: [0],
      LocationId: [0],
      DeptId: [0],
      DesignationId: [0],
      ReporterId: [0],
      GradeId: [0],
      EmpId: [0],
      Login: [this.employeeDetails[0].LoginId]
    });

    this.getBusinessUnit();
    this.callLocation();
    this.access_DD_department();
    this.getAllEmployeeLogHistory();
    // setTimeout(() => {

    //   setTimeout(() => {

    //     setTimeout(() => {

    //       setTimeout(() => {

    //       }, 200);
    //     }, 200);
    //   }, 200);
    // }, 200);

    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        if (!newEntityId) return;

        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          this.getBusinessUnit();
          this.callLocation();
          setTimeout(() => {
            this.resetData();


          }, 200);
        }
        this.currentEntityId = newEntityId;
      });
    const payload = {
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocationId: 0,
      DeptId: 0,
      DesignationId: 0,
      ReporterId: 0,
      GradeId: 0,
      EmpId: 0,
      Login: this.employeeDetails[0].LoginId
    };
    this.loadHierarchy(payload);

  }



  resetData() {
    this.hierarchyForm.reset();
    const payload = {
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocationId: 0,
      DeptId: 0,
      DesignationId: 0,
      ReporterId: 0,
      GradeId: 0,
      EmpId: 0,
      LoginId: this.employeeDetails[0].LoginId
    }
    this.loadHierarchy(payload)

  }
  isSpinner: boolean = false;
  getAllEmployeeLogHistory() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocId: 0,
      DeptId: 0,
      DesignationId: 0,
      ReporterId: 0
    };

    this.isSpinner = true;

    this.hrmsEmployeeModuleService
      .GetAllEmployeeLogHistory(reqBody)
      .subscribe({
        next: (res: any) => {
          if (res && res.length > 0) {
            this.rows = res;              // ✅ bind data to table
            this.isTableData = false;     // ✅ show table
          } else {
            this.rows = [];
            this.isTableData = true;      // ✅ show "no data"
            this.errorMessage = 'No records found';
          }

          this.isSpinner = false;
        },
        error: () => {
          this.isSpinner = false;
        }
      });
  }
  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getBusinessUnitlist = [];
    setTimeout(() => {
      this.hrmsEmployeeModuleService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
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
    }, 100);
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
    }
    this.isSpinner = true;
    this.getLocations = []
    setTimeout(() => {
      this.payrollLocationDD.payrollDDLocation(reqBody).subscribe((res: any) => {
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
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner = false;
          this.getLocations = []
        })
    }, 100);
  }

  access_DD_department() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;

    this.hrmsServiceMain.access_DD_department(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDepartementName = res;
        } else {
          this.triggerToast('', 'Record Not Found', 'Warning');
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Department List', 'danger');
        this.isSpinner = false;
      }
    });
  }

  callDDDesignation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.hierarchyForm?.get('DeptId')?.value,
    };
    this.isSpinner = true;
    this.hierarchyForm.patchValue({
      reporter_name: ''
    });
    this.hrmsServiceMain.access_DDDesignation(reqBody).subscribe({
      next: (res: any) => {
        this.getDepartementRole = res;
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error loading Designation', 'danger');
        this.isSpinner = false;
      }
    });
  }
  settingService = inject(SettingsService)
  loadHierarchy(hierarchy: any): void {
    this.isLoading = true;
    this.errorMessage = '';


    this.settingService.getAllOrgDetails(hierarchy).pipe(
      catchError((error) => {
        this.errorMessage = error.error?.Message || 'Failed to load hierarchy data';
        return of(null);
      }),
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe({
      next: (response: any) => {
        if (response && response.Success) {
          this.treeData = response.Data?.Hierarchy || [];
          this.markLoggedInUser(this.treeData);
          this.sortByGradeAndLoggedIn(this.treeData);
          this.assignLevels(this.treeData, 0);
          this.extractAvailableGrades();

          // Auto-expand path to logged in user
          this.expandPathToUser();
        } else {
          this.errorMessage = response?.Message || 'No data found';
        }
      }
    });
  }

  assignLevels(nodes: Employee[], level: number): void {
    for (const node of nodes) {
      node.level = level;
      if (node.Reportees?.length) {
        this.assignLevels(node.Reportees, level + 1);
      }
    }
  }

  markLoggedInUser(nodes: Employee[]): boolean {
    for (const node of nodes) {
      if (node.EmpId === this.LOGGED_IN_EMP_ID) {
        node.IsLoggedInUser = true;
        return true;
      }
      if (node.Reportees?.length && this.markLoggedInUser(node.Reportees)) {
        return true;
      }
    }
    return false;
  }

  sortByGradeAndLoggedIn(nodes: Employee[]): void {
    nodes.sort((a, b) => {
      if (a.IsLoggedInUser && !b.IsLoggedInUser) return -1;
      if (!a.IsLoggedInUser && b.IsLoggedInUser) return 1;
      const gradeA = this.getGradeNumber(a.GradeName);
      const gradeB = this.getGradeNumber(b.GradeName);
      return gradeA - gradeB;
    });
    for (const node of nodes) {
      if (node.Reportees?.length) {
        this.sortByGradeAndLoggedIn(node.Reportees);
      }
    }
  }

  expandPathToUser(): void {
    const expandPath = (nodes: Employee[]): boolean => {
      for (const node of nodes) {
        if (node.EmpId === this.LOGGED_IN_EMP_ID) {
          return true;
        }
        if (node.Reportees?.length && expandPath(node.Reportees)) {
          node.isExpanded = true;
          return true;
        }
      }
      return false;
    };
    expandPath(this.treeData);

    // Also expand Grade 1 nodes by default
    this.expandGrade1Nodes(this.treeData);
  }

  expandGrade1Nodes(nodes: Employee[]): void {
    for (const node of nodes) {
      if (this.getGradeNumber(node.GradeName) === 1 && node.Reportees?.length) {
        node.isExpanded = true;
      }
      if (node.Reportees?.length) {
        this.expandGrade1Nodes(node.Reportees);
      }
    }
  }

  getGradeNumber(gradeName: string): number {
    if (!gradeName) return 999;
    const match = gradeName.match(/\d+/);
    return match ? parseInt(match[0], 10) : 999;
  }

  extractAvailableGrades(): void {
    const grades = new Set<string>();
    const extract = (nodes: Employee[]) => {
      for (const node of nodes) {
        if (node.GradeName) grades.add(node.GradeName);
        if (node.Reportees?.length) extract(node.Reportees);
      }
    };
    extract(this.treeData);
    this.availableGrades = Array.from(grades).sort((a, b) =>
      this.getGradeNumber(a) - this.getGradeNumber(b)
    );
  }

  // Toggle node expansion - this is the key tree function
  toggleNode(node: Employee, event: Event): void {
    event.stopPropagation();
    node.isExpanded = !node.isExpanded;
  }

  filterByGrade(): void {
    if (!this.selectedGrade) {
      this.resetAndShowAll();
      return;
    }

    const filterTree = (nodes: Employee[]): Employee[] => {
      const result: Employee[] = [];
      for (const node of nodes) {
        const matchesGrade = node.GradeName === this.selectedGrade;
        const filteredChildren = node.Reportees?.length ? filterTree(node.Reportees) : [];

        if (matchesGrade || filteredChildren.length > 0) {
          const newNode = { ...node, Reportees: filteredChildren };
          if (matchesGrade) {
            newNode.isExpanded = true;
          }
          result.push(newNode);
        }
      }
      return result;
    };

    this.treeData = filterTree([...this.treeData]);
  }

  resetAndShowAll(): void {
    this.loadHierarchy(this.hierarchyForm.value);
  }

  clearAllFilters(): void {
    this.searchTerm = '';
    this.selectedGrade = '';
    this.loadHierarchy(this.hierarchyForm.value);
  }

  isLoggedInUser(node: Employee): boolean {
    return node.IsLoggedInUser === true;
  }

  getInitials(name: string): string {
    if (!name) return '?';
    const parts = name.trim().split(' ');
    if (parts.length === 1) return parts[0].charAt(0).toUpperCase();
    return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
  }

  getTreeLevelClass(level: number = 0): string {
    if (level === 0) return 'level-0';
    if (level === 1) return 'level-1';
    if (level === 2) return 'level-2';
    return 'level-3';
  }

  getGradeClass(gradeName: string): string {
    const grade = this.getGradeNumber(gradeName);
    if (grade === 1) return 'grade-executive';
    if (grade === 2) return 'grade-senior';
    if (grade === 3) return 'grade-mid';
    return 'grade-junior';
  }

  getTotalCount(): number {
    const count = (nodes: Employee[]): number => {
      let total = nodes.length;
      for (const node of nodes) {
        if (node.Reportees?.length) {
          total += count(node.Reportees);
        }
      }
      return total;
    };
    return count(this.treeData);
  }

  submitFilterData() {
    const payload  = {
      LoginId: this.employeeDetails[0].LoginId,
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()) || 0,
      BUId: Number(this.hierarchyForm?.get('BUId').value) || 0,
      LocationId: Number(this.hierarchyForm?.get('LocationId').value) || 0,
      DeptId: Number(this.hierarchyForm?.get('DeptId').value) || 0,
      DesignationId: Number(this.hierarchyForm?.get('DesignationId').value) || 0,
      ReporterId: Number(this.hierarchyForm?.get('ReporterId').value || 0),
      "GradeId": 0,
      EmpId:Number(this.hierarchyForm?.get('EmpId').value || 0)
    }
    this.loadHierarchy(payload);
  }

  callDDEmployee() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()) || 0,
      BUId: Number(this.hierarchyForm?.get('BUId').value) || 0,
      LocationId: Number(this.hierarchyForm?.get('LocationId').value) || 0,
      DeptId: Number(this.hierarchyForm?.get('DeptId').value) || 0,
      DesignationId: Number(this.hierarchyForm?.get('DesignationId').value) || 0,
      ReporterId: Number(this.hierarchyForm?.get('EmpId').value || 0),
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.DDEmployeeList(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length > 0) {
          this.getDropdownEmployee = res;
        } else {
          this.getDropdownEmployee = [];
          this.triggerToast('', 'No Data Found For Employee Name', 'warning')
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'No Data Found For Employee Name', 'danger');
        this.getDropdownEmployee = [];
        this.isSpinner = false;
      }
    })
  }

  callReporter() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LEId: Number(this.entityStateService.getEntityId()) || 0,
      BUId: Number(this.hierarchyForm?.get('BUId').value) || 0,
      LocationId: Number(this.hierarchyForm?.get('LocationId').value) || 0,
      DeptId: Number(this.hierarchyForm?.get('DeptId').value) || 0,
      DesignationId: Number(this.hierarchyForm?.get('DesignationId').value) || 0,
    }
    this.isSpinner = true;
    this.hrmsEmployeeModuleService.DDReporterList(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res.length > 0) {
          this.getDropdownReporter = res;
        } else {
          this.getDropdownReporter = [];
          this.triggerToast('', 'No Data Found For Reporter Name', 'warning')
        }
        this.isSpinner = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'No Data Found For Reporter Name', 'danger');
        this.getDropdownReporter = [];
        this.isSpinner = false;
      }
    })
  }
}
