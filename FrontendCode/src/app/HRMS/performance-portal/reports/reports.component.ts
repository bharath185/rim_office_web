import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { FormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PerformancePortalService } from '../../service/performancePortal/performance-portal.service';
import { NgxPaginationModule } from 'ngx-pagination';


@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, FormsModule, NgxPaginationModule],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent {
  @ViewChild('inputValue') inputValue!: ElementRef;
  loading: boolean = false;
  getFinancialYear: any;
  employeeDetails: any;
  empId: any;
  reportForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  yearDropdown: any = []
  DDQuaterValue: any = []
  DDReviewStatusValue: any = []
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  errorMessage: any;
  isTableData: boolean = false;
  rows: any = [];
  viewModalData: any;
  isSpinner: boolean = false;
  originalRows: any;

  constructor(private fb: FormBuilder, private perPortalMain: PerformancePortalService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
  }
  ngOnInit(): void {
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData;
    this.reportForm = this.fb.group({
      DDYear: ['', [Validators.required]],
      quaters: ['', [Validators.required]],
      status: ['', []],
    })
    this.DDYearPerMainService();
    setTimeout(() => {
      this.DDQuaterPerMainService();
      setTimeout(() => {
        this.DDReviewStatusMainService();
        this.getPerformaceReport();
      }, 1000);
    }, 100);
  }

  DDYearPerMainService() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.perPortalMain.DDYearPer(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        this.yearDropdown = res;
      }, error: (err: any) => {
        console.log(err);
      }
    })
  }

  DDQuaterPerMainService() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.perPortalMain.DDQuater(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        this.DDQuaterValue = res;
      }, error: (err: any) => {
        console.log(err);
      }
    })
  }

  DDReviewStatusMainService() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.perPortalMain.DDReviewStatus(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        this.DDReviewStatusValue = res;
      }, error: (err: any) => {
        console.log(err);
      }
    })
  }

  getPerformaceReport() {
    this.isSpinner = true;
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      FYearId: 0,
      QId: 0,
      OverAllStatus: "ALL",
    }
    this.perPortalMain.PerformanceReport(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        setTimeout(() => {
          this.rows = res.map((item: any) => ({
            ...item,
            CreatedDate: this.parseJsonDate(item.CreatedDate),
            LastUpdatedDate: this.parseJsonDate(item.LastUpdatedDate),
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

  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    return match ? new Date(parseInt(match[1], 10)) : null;
  }

  submitForm() {
    if (this.reportForm.valid) {
      this.isFormSubmitted = false;
      const reqBody = {
        EmpId: this.employeeDetails[0].EmpId,
        FYearId: this.reportForm?.get('DDYear').value,
        QId: this.reportForm?.get('quaters').value,
        OverAllStatus: this.reportForm?.get('status').value,
      }
      this.perPortalMain.PerformanceReport(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          this.isSpinner = true
          this.rows = res;
          if (res.length >= 1) {
            setTimeout(() => {
              this.rows = res.map((item: any) => ({
                ...item,
                CreatedDate: this.parseJsonDate(item.CreatedDate),
                LastUpdatedDate: this.parseJsonDate(item.LastUpdatedDate),
              }));
              // this.originalRows = [...this.rows];
              this.page=1
              this.isSpinner = false;
            }, 10);
            this.isTableData = false;
            this.isSpinner = false
            // this.page = 1;
          } else {
            this.errorMessage = res.Message;
            this.isTableData = true;
            this.isSpinner = false
          }

        }, error: (err: any) => {
          console.log(err);
          this.isTableData = true;
          this.errorMessage = "Internal Server Error"
          this.isSpinner = false
        }
      })
    } else {
      this.isFormSubmitted = true;
    }
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
    this.viewModalData = data
  }

  resetForm() {
    this.reportForm.reset();
    this.page=1
    setTimeout(() => {
      if (this.inputValue?.nativeElement) {
        this.inputValue.nativeElement.value = null;
        const event = new KeyboardEvent('keyup', { bubbles: true });
        this.inputValue.nativeElement.dispatchEvent(event);
        this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));  // Ensure this method handles its own logic
      }
    }, 100);
  }

}
