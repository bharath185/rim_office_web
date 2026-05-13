import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { payRollService } from '../../service/payroll.service';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { EntityStateService } from '../../service/entity-state.service';

@Component({
  selector: 'app-emp-sal-summary',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent,
    NgxPaginationModule],
  templateUrl: './emp-sal-summary.component.html',
  styleUrl: './emp-sal-summary.component.scss'
})
export class EmpSalSummaryComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  empSalSummaryForm: any = FormGroup;
  employeeDetails;
  controlAccessPage: any;
  accessPolicy: any;
  isSpinner: boolean = false;
  getDDCompany: any;
  isFormSubmitted: boolean = false;
  getLegalEntity: any;
  errorMessage: any;
  getBusinessUnitlist: any;
  getLocations: any;
  getDepartementName = [];
  getDepartementRole: any[] = [];
  isTableData: boolean = false
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  rows: any = [];
  originalrows: any = [];
  searchValue: string = '';
  years: number[] = [];
  months: { id: number, name: string }[] = [];
  selectedYear!: number;
  selectedMonth: any;
  dropdownVisible = false;


  constructor(private payrollService: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly fb: FormBuilder,
    private readonly hrmsServiceMain: HrmsServiceService,
    private readonly employeeService: EmployeeModuleService,
    private entityStateService: EntityStateService) 
    {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
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

    // this.dropdown_Comapny();
    setTimeout(() => {
      this.pagerollDropdownlegalEntity();
      this.payrollDropdwonLocation();
    }, 100);
    setTimeout(() => {
      this.dropdwon_department();
      this.getPayrollReportforALL()
    }, 1000);
    this.empSalSummaryForm = this.fb.group({
      // Company: [''],
      LegalEntity: [''],
      // BusinessUnit: [''],
      Location: [''],
      DeptName: [''],
      Designation: [''],
      year: [0, ''],
      month: [0, ''],
    })
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
    this.selectedYear = Number(this.selectedYear);
    this.selectedMonth = '';
    const currentYear = new Date().getFullYear();
    const currentMonth = new Date().getMonth() + 1;
    const allMonths = this.getAllMonths();
    if (this.selectedYear === currentYear) {
      this.months = allMonths.filter(m => m.id <= currentMonth);
    } else {
      this.months = allMonths;
    }
  }


  dropdown_Comapny() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId
    };
    this.isSpinner = true;
    this.employeeService.employeeDDCompany(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getDDCompany = res;
        } else {
          this.triggerToast(res['Message'], 'Sorry No Data Found', 'warning');
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.triggerToast('Internal Server Error', 'Error loading Company Name', 'danger');
        this.isSpinner = false;
      }
    });
  }
  pagerollDropdownlegalEntity() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      // CompId: Number(this.empSalSummaryForm?.get('Company').value)
    }
    this.isSpinner = true;
    this.getLegalEntity = []
    this.payrollService.DDLegalEntity(reqBody).subscribe((res: any) => {
      setTimeout(() => {
        this.empSalSummaryForm?.get('LegalEntity').reset();
        // this.empSalSummaryForm?.get('BusinessUnit').reset();
        this.empSalSummaryForm?.get('Location').reset();
      }, 100);
      if (res.length >= 1) {
        this.getLegalEntity = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Legal Entity", "warning");
        this.isSpinner = false;
        this.getLegalEntity = []
      }
    },
      error => {
        this.errorMessage = 'Error loading data. Please try again.';
        this.triggerToast('Internal Server Error', 'Error loading data. For Legal Entity', "danger");
        this.isSpinner = false;
      })
  }

  getBusinessUnit() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      AuthorisedEntity: this.entityStateService.getEntityId(),
      // CompId: Number(this.empSalSummaryForm?.get('Company').value),
      // LEId: Number(this.empSalSummaryForm?.get('LegalEntity').value),
    }
    this.isSpinner = true;
    this.getBusinessUnitlist = []
    setTimeout(() => {
      this.employeeService.employeeDDBusinessUnit(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.empSalSummaryForm?.get('BusinessUnit').reset();
          this.getBusinessUnitlist = res;
          this.isSpinner = false;
        } else {
          this.isSpinner = false;
          this.getBusinessUnitlist = [];
          this.getLocations = []
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. For Business Unit', "danger");
          this.isSpinner = false;
        })
    }, 100);

  }

  payrollDropdwonLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AuthorisedEntity:this.entityStateService.getEntityId(),
    }
    this.isSpinner = true;
    this.getLocations = [];
    setTimeout(() => {
      this.payrollService.payrollDDLocation(reqBody).subscribe((res: any) => {
        if (res.length >= 1) {
          this.empSalSummaryForm?.get('Location').reset();
          this.getLocations = res;
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = []
        }
      },
        error => {
          this.errorMessage = 'Error loading data. Please try again.';
          this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
          this.isSpinner = false;
        })
    }, 100);
  }

  dropdwon_department() {
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

  callDDDesignation(event: any) {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      DeptId: this.empSalSummaryForm?.get('DeptName')?.value,
    };
    this.isSpinner = true;
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

  callPayrollReport(reqBody: any) {
    this.isSpinner = true;

    this.payrollService.PayrollReportforALL(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.rows = res;
          this.originalrows = res;
          this.errorMessage = '';
          this.isTableData = false;
        }
        else if (res['Message']) {
          this.errorMessage = res['Message'];
          this.isTableData = true;
          this.triggerToast('', res['Message'], 'warning');
        }
        else {
          this.errorMessage = 'No Data Found';
          this.isTableData = true;
        }
        this.isSpinner = false;
      },
      error: () => {
        this.errorMessage = 'Internal Server Error';
        this.isSpinner = false;
        this.isTableData = true;
      }
    });
  }

  getPayrollReportforALL() {
    this.isSpinner = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: 0,
      LEId: 0,
      LocationId: 0,
      DeptId: 0,
      DesignationId: 0,
      Year: 0,
      MonthNo: 0,
      Month: ""
    }
    this.callPayrollReport(reqBody);
  }

  filterData() {
    const month = this.empSalSummaryForm?.get('month').value;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: 0,
      LEId: Number(this.empSalSummaryForm?.get('LegalEntity').value ? this.empSalSummaryForm?.get('LegalEntity').value : 0),
      LocationId: Number(this.empSalSummaryForm?.get('Location').value ? this.empSalSummaryForm?.get('Location').value : 0),
      DeptId: Number(this.empSalSummaryForm?.get('DeptName').value ? this.empSalSummaryForm?.get('DeptName').value : 0),
      DesignationId: Number(this.empSalSummaryForm?.get('Designation').value ? this.empSalSummaryForm?.get('Designation').value : 0),
      Year: this.empSalSummaryForm?.get('year').value ? this.empSalSummaryForm?.get('year').value : 0,
      MonthNo: month ? month.id : 0,
      Month: month ? month.name : '',
    };
    this.callPayrollReport(reqBody);
  }

  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
  }
  // Listen for clicks anywhere in the document

  exportFile(format: string) {
    if (format === 'excel') {
      this.exportToExcel();
    }
    if (format === 'pdf') {
      this.exportToPDF()
    }
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

  exportToExcel(): void {
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
    const filteredData = this.rows.map((item: any) => ({
      "Name": item.EmpName,
      'Code': item.EmpCode,
      'Year': item.Year,
      'Month': item.Month,
      "Company": item.Company,
      'Location': item.Location,
      "Department": item.Department,
      "Designation": item.Designation,
      "Total Days": item.TotalDays,
      "Working Days": item.WorkingDays,
      'Paid Leave Days(CL)': item.PaidLeaveDaysCL,
      'Paid Leave Days(EL)': item.PaidLeaveDaysEL,
      'LOP Days': item.LOPDays,
      "LOP Amt": item.LOPAmt,
      "Arrears": item.Arrears,
    }));
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(filteredData);
    const workbook: XLSX.WorkBook = { Sheets: { 'Employee Salary Summary': worksheet }, SheetNames: ['Employee Salary Summary'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'EmployeeSalarySummary.xlsx');
    this.dropdownVisible = false;
  }

  exportToPDF(): void {
    if (this.isTableData === true) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
      return;
    }
    const filteredData = this.rows.map((item: any) => [
      item.EmpName,
      item.EmpCode,
      item.Year,
      item.Month,
      item.Company,
      item.Location,
      item.Department,
      item.Designation,
      item.TotalDays,
      item.WorkingDays,
      item.PaidLeaveDaysCL,
      item.PaidLeaveDaysEL,
      item.LOPDays,
      item.LOPAmt,
      item.Arrears,
    ]);

    const headers = [
      'Name',
      'Code',
      'Year',
      'Month',
      'Company',
      'Employee Level',
      'Location',
      'Department',
      'Designation',
      'TotalDays',
      'WorkingDays',
      'PaidLeaveDaysCL',
      'PaidLeaveDaysEL',
      'LOP Days',
      'LOP Amt',
      'Arrears',
    ];

    // Use A3 format for wider page
    const doc = new jsPDF({
      orientation: 'landscape',
      unit: 'pt',
      format: 'a3',
    });

    autoTable(doc, {
      head: [headers],
      body: filteredData,
      styles: {
        fontSize: 7,
        cellPadding: 3,
        overflow: 'linebreak',
        halign: 'left',
      },
      headStyles: {
        fillColor: [7, 47, 95],  // RGB for #072F5F
        textColor: 255,          // white
        fontStyle: 'bold',
        fontSize: 7,
      },
      alternateRowStyles: { fillColor: [245, 245, 245] },

      didDrawPage: (data) => {
        doc.setFontSize(12);
        doc.text('Employee Salary Summary Report', data.settings.margin.left, 20);
      },
    });

    doc.save('Employee Salary Summary.pdf');
    this.dropdownVisible = false;
  }

  applyFilter() {
    const val = this.searchValue.toLowerCase().trim();

    this.rows = this.originalrows.filter((row: any) => {
      return (
        row.BusinessUnit?.toLowerCase().includes(val) ||
        row.Company?.toLowerCase().includes(val) ||
        row.EmpCode?.toString().toLowerCase().includes(val) ||
        row.EmpName?.toLowerCase().includes(val) ||
        row.LegalEntity?.toLowerCase().includes(val) ||
        row.Location?.toLowerCase().includes(val) ||
        row.Month?.toLowerCase().includes(val) ||
        row.Year?.toString().toLowerCase().includes(val) ||
        `${row.Month} ${row.Year}`.toLowerCase().includes(val) || // Search Month + Year

        // 🔥 Amount fields (number → string)
        row.LOPAmt?.toString().toLowerCase().includes(val) ||
        row.Arrears?.toString().toLowerCase().includes(val)
      );
    });

    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = `No record found for "${this.searchValue}"`;
    } else {
      this.isTableData = false;
      this.errorMessage = '';
    }

    this.page = 1;
  }

  resetData() {
    this.empSalSummaryForm.reset();
    this.isFormSubmitted = false;
    this.getPayrollReportforALL();
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
