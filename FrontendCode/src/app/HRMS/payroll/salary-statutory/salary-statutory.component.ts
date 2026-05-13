import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { DashboardService } from '../../service/dashboard.service';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { payRollService } from '../../service/payroll.service';
import { SalaryStructureComponent } from '../salary-structure/salary-structure.component';
import { PaySlipSectionComponent } from '../pay-slip-section/pay-slip-section.component';
import { AddEmpSalComponent } from '../add-emp-sal/add-emp-sal.component';
import { MappingComponent } from '../mapping/mapping.component';
import { EmpSalSummaryComponent } from '../emp-sal-summary/emp-sal-summary.component';
import { VariableHistroyComponent } from '../variable-histroy/variable-histroy.component';

@Component({
  selector: 'app-salary-statutory',
  standalone: true,
  imports: [CommonModule, SharedModule, ToastMessageComponent, ReactiveFormsModule,
    SalaryStructureComponent, PaySlipSectionComponent, AddEmpSalComponent,
    MappingComponent, EmpSalSummaryComponent, VariableHistroyComponent
  ],
  templateUrl: './salary-statutory.component.html',
  styleUrl: './salary-statutory.component.scss'
})
export class SalaryStatutoryComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  employees: any[] = [];
  errorMessageEmpName: any;
  employeeDetails;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;
  salaryForm: any = FormGroup;
  accessPolicy: any;
  controlAccessPage: any;
  employeeCTC: any[] = [];
  groupedData: any[] = [];
  isTableData = false;
  errorMessage = "";
  isShowTable: boolean = false;
  years: number[] = [];
  months: { id: number, name: string }[] = [];
  selectedYear!: number;
  selectedMonth: any;

  selectedTab = 0;
  selectTab(index: number) {
    this.selectedTab = index;
    if (index === 0) {
      this.getEmployeeSelectEmployee();
    }
  }
  tabs = [
    { label: 'Salary Statutory', icon: 'feather icon-briefcase' },
    { label: 'Salary Structure', icon: 'feather icon-layers' },
    { label: 'Payslip Section', icon: 'feather icon-layers' },
    { label: 'Employee Salary', icon: 'feather icon-plus' },
    { label: 'Employee Mapping', icon: 'feather icon-share-2' },
    { label: 'Employee Salary Summary', icon: 'feather icon-credit-card' },
    { label: 'Payroll Variable History', icon: 'feather icon-clock' }
  ];

  constructor(private fb: FormBuilder,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private payrollService: payRollService,
  ) {
    const storedEmployeeData = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Salary Management'
      );
    });
  }

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();
    for (let yr = 2020; yr <= currentYear; yr++) {
      this.years.push(yr);
    }
    this.getEmployeeSelectEmployee();
    this.salaryForm = this.fb.group({
      emloyee: ['', [Validators.required]],
      emloyeeCode: ['', [Validators.required]],
      year: ['', [Validators.required]],
      month: ['', [Validators.required]],
    });
  }
  getAllMonths() {
    return [
      { id: 1, name: 'January' }, { id: 2, name: 'February' },
      { id: 3, name: 'March' }, { id: 4, name: 'April' },
      { id: 5, name: 'May' }, { id: 6, name: 'June' },
      { id: 7, name: 'July' }, { id: 8, name: 'August' },
      { id: 9, name: 'September' }, { id: 10, name: 'October' },
      { id: 11, name: 'November' }, { id: 12, name: 'December' }
    ];
  }

  onYearChange() {
    this.selectedYear = Number(this.selectedYear);   // <--- FIX

    this.selectedMonth = '';

    const currentYear = new Date().getFullYear();
    const currentMonth = new Date().getMonth() + 1;

    const allMonths = this.getAllMonths();

    if (this.selectedYear === currentYear) {
      this.months = allMonths.filter(m => m.id < currentMonth);
    } else {
      this.months = allMonths;
    }
  }

  //this is second code for contact person
  getEmployeeSelectEmployee() {
    const reqBody = { LoginId: this.employeeDetails[0].LoginId };
    this.isSpinner = true;
    this.payrollService.DDPayrollEmpList(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.errorMessageEmpName = ''
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
          this.errorMessageEmpName = 'No Data Found.'
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessageEmpName = 'Error loading data. Please try again.';
        this.isSpinner = false;
      }
    });
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
    this.salaryForm.get('emloyeeCode')?.patchValue(employee.EmpCode);
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.salaryForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.salaryForm.get('emloyee')?.setErrors(null);
    }
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
  //this is second code for contact person
  arrearMap: any = {};
  submit() {
    if (this.salaryForm.valid) {
      const month = this.salaryForm?.get('month').value;
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpId: this.selectedEmployee,
        Year: this.salaryForm?.get('year').value ? this.salaryForm?.get('year').value : 0,
        MonthNo: month ? month.id : 0,
        Month: month ? month.name : '',
      };
      this.isSpinner = true;
      this.payrollService.EmpCTCCalculation(reqBody).subscribe({
        next: (res: any) => {
          console.log("CTC API:", res);
          this.isSpinner = false;
          if (res['Message']) {
            this.errorMessage = res['Message']
            this.triggerToast('', res['Message'], "");
            this.isShowTable = false;
            this.isTableData = true;
          }
          else if (!res || res.length === 0) {
            this.isTableData = true;
            this.errorMessage = "No records found";
            this.isShowTable = true;
            return;
          } else if (res) {

            // normal data (existing logic depends on this)
            const componentList = res.lstofComponentDetails || [];

            // 🔴 arrear mapping logic (ADD THIS)
            const arrearList = res.lstofArrearComponentDetails || [];

            this.arrearMap = arrearList.reduce((acc: any, item: any) => {
              acc[item.ComponentId] = item.ComponentValue;
              return acc;
            }, {});

            // existing assignment
            this.employeeCTC = componentList;

            this.groupResponseByPayoutAndSegment();

            this.isShowTable = true;
            this.isTableData = false;
            this.errorMessage = '';
          }


        },
        error: () => {
          this.isSpinner = false;
          this.isTableData = true;
          this.errorMessage = "Internal Server Error";
          this.isShowTable = false
          this.triggerToast('Sorry', 'Internal Server Error', "danger");
        }
      });
    } else {
      this.isFormSubmitted = true
    }
  }

  // GROUP BY PAYOUT → SEGMENT → COMPONENT
  groupResponseByPayoutAndSegment() {
    const grouped: any = {};
    this.employeeCTC.forEach((item: any) => {
      const payout = item.PayoutTypeName;
      const segment = item.SegmentName;
      if (!grouped[payout]) {
        grouped[payout] = {};
      }
      if (!grouped[payout][segment]) {
        grouped[payout][segment] = [];
      }

      grouped[payout][segment].push(item);
    });
    // Convert object → array for HTML looping
    this.groupedData = Object.entries(grouped).map(([payoutName, segments]: any) => ({
      payoutName,
      segments: Object.entries(segments).map(([segmentName, comps]: any) => ({
        segmentName,
        components: comps
      }))
    }));
  }

  resetData() {
    this.salaryForm.reset();
    this.isFormSubmitted = false;
    this.isShowTable = false;
  }



  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
