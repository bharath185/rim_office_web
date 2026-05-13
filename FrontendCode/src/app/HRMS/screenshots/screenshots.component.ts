import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../hrms-service.service';
import { AccessPolicyStoreService } from '../service/accessPolicayApi.service';

@Component({
  selector: 'app-screenshots',
  standalone: true,
  imports: [FormsModule, CommonModule, SharedModule, ToastMessageComponent, NgxPaginationModule],
  templateUrl: './screenshots.component.html',
  styleUrl: './screenshots.component.scss'
})
export class ScreenshotsComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  screenshotForm: any = FormGroup;
  workAnalysisForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  today: any;
  accessPolicy: any;
  controlAccessPage: any;
  employeeDetails;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  availableDates: any[] = [];
  rows: any;
  originalRows: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 20;
  pageSizes = [20, 50, 100, 500];
  errorMessage: any;

  constructor(private readonly fb: FormBuilder,
    private readonly hrmsMainServer: HrmsServiceService,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Screenshots Analysis'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Screenshots Analysis'
      );
    });
  }
  ngOnInit(): void {
    const timeRegex = /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)\.(\d{3})$/;
    this.workAnalysisForm = this.fb.group({
      emp_code_WAF: ['', [Validators.required]],
      date_WAF: ['', [Validators.required]],
      analysis_hour: ['', [Validators.required, Validators.pattern(timeRegex)
      ]]
    });

    this.screenshotForm = this.fb.group({
      employee_code: ['', [Validators.required]],
      date: ['', []]
    });
    const now = new Date();
    this.today = now.toISOString().split('T')[0];

  }
  callWorkAnaHr() {
    console.log('fghjk');

    this.getAllWFHAnalysis();
  }

  getAllWFHAnalysis() {
    const reqBody = {
      loginId: this.employeeDetails[0].LoginId,
    };

    this.isSpinner = true;

    this.hrmsMainServer.GetAllWFHAnalysis(reqBody).subscribe({
      next: (res: any) => {

        const formattedData = res
          .map((row: any) => {
            const dateObj = this.parseJsonDate(row.Date);

            return {
              ...row,
              DateObj: dateObj,
              Date: this.formatDate(dateObj),

              ActiveHoursStr: this.formatHourMinute(row.Activehrs),
              AnalysisHoursStr: this.formatHourMinute(row.AnalysisHr),

              LoginTimeStr: this.formatHourMinute(row.LoginTime),
              LogoutTimeStr: this.formatHourMinute(row.LogOutTime),
            };
          })
          .sort((a: any, b: any) => b.DateObj.getTime() - a.DateObj.getTime());

        this.rows = formattedData;
        this.originalRows = [...formattedData];   // ✅ VERY IMPORTANT

        this.isSpinner = false;
      },
      error: (err: any) => {
        console.error(err);
        this.isSpinner = false;
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value
      .trim()
      .toUpperCase();

    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) => {
        const EmpName = row.EmpName?.toUpperCase() || '';
        const EmpCode = row.EmpCode?.toUpperCase() || '';

        return (
          EmpName.includes(filterValue) ||
          EmpCode.includes(filterValue)
        );
      });

      if (this.rows.length === 0) {
        this.isTableData = true;
        this.errorMessage = 'No Records Found for Searched Data';
      } else {
        this.isTableData = false;
        this.errorMessage = null;
      }

    } else {
      this.rows = [...this.originalRows];
      this.isTableData = false;
      this.errorMessage = null;
    }
  }

  formatHourMinute(timeObj: any): string {
    if (!timeObj) return '-';
    const hours = timeObj?.Hours || 0;
    const minutes = timeObj?.Minutes || 0;
    return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  }

  submitFormdata() {
    if (this.screenshotForm.invalid) {
      this.isFormSubmitted = true;
    } else {
      const rawDate = this.screenshotForm?.get('date')?.value;
      let formattedDate = '';
      if (rawDate) {
        const dateObj = new Date(rawDate);
        const day = String(dateObj.getDate()).padStart(2, '0');
        const month = String(dateObj.getMonth() + 1).padStart(2, '0');
        const year = dateObj.getFullYear();
        formattedDate = `${day}-${month}-${year}`;
      }
      const reqBody = {
        EmpCode: this.screenshotForm?.get('employee_code')?.value,
        Date: formattedDate
      };
      this.isSpinner = true;
      console.log(reqBody);
      this.hrmsMainServer.ViewScreenShots(reqBody).subscribe({
        next: (res: Blob) => {
          const reader = new FileReader();
          reader.onload = () => {
            const text = reader.result as string;
            try {
              const json = JSON.parse(text); // Try parsing as JSON
              // If successful, show list instead of download
              console.log("Available screenshots:", json);
              this.availableDates = json;
              this.triggerToast('Info', "Available Screenshot Dates Listed", "info");
            } catch (e) {
              // Not JSON — treat as zip blob
              const blob = new Blob([res], { type: 'application/zip' });
              const url = window.URL.createObjectURL(blob);
              const a = document.createElement('a');
              a.href = url;
              a.download = `screenshot_${reqBody.EmpCode}_${reqBody.Date || 'multiple'}.zip`;
              document.body.appendChild(a);
              a.click();
              document.body.removeChild(a);
              window.URL.revokeObjectURL(url);
              this.triggerToast('Success', "ZIP File Downloaded Successfully", "success");
            }
            this.isSpinner = false;
          };
          reader.readAsText(res); // To check if JSON or binary
        },
        error: (err: any) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', "Failed to download the file", "danger");
          console.error('Error:', err);
        }
      });
    }
  }

  downloadByDate(date: string) {
    const reqBody = {
      EmpCode: this.screenshotForm?.get('employee_code')?.value,
      Date: date
    };
    this.isSpinner = true;
    this.hrmsMainServer.ViewScreenShots(reqBody).subscribe({
      next: (res: Blob) => {
        const blob = new Blob([res], { type: 'application/zip' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `screenshot_${this.screenshotForm?.get('employee_code')?.value}_${date}.zip`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
        this.triggerToast('Success', `ZIP for ${date} Downloaded`, "success");
        this.isSpinner = false;
      },
      error: (err: any) => {
        this.isSpinner = false;
        this.triggerToast('Failed', "Download Failed", "danger");
        console.error('Download error:', err);
      }
    });
  }

  submitWorkAnalysisHour() {
    if (this.workAnalysisForm.valid) {
      const reqBody = {
        EmpCode: this.workAnalysisForm?.get('emp_code_WAF').value,
        Date: this.workAnalysisForm?.get('date_WAF').value,
        AnalysisHr: this.workAnalysisForm?.get('analysis_hour').value,
      }
      this.isSpinner1 = true;
      console.log(reqBody);
      this.hrmsMainServer.SaveWFHAnalysis(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === "Analysis Hr Added") {
            this.getAllWFHAnalysis();
            this.triggerToast(res['msg'], 'Data Added Successfully', 'success');
          } else if (res['Message']) {
            this.triggerToast(res['Message'], 'Failed To Add Record', 'warning');
          }
          this.isSpinner1 = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'Failed to submit the data', 'danger');
          this.isSpinner1 = false;
        }
      })
    } else {
      this.isFormSubmitted = true
    }
  }

  resetData() {
    this.screenshotForm.reset();
    this.availableDates = [];
    this.isFormSubmitted = false;
  }

  resetWAF() {
    this.workAnalysisForm.reset();
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
    return `${day}-${month}-${year}`;
    // return `${year}-${month}-${day}`;
  }

  preventKeyboardInput(event: KeyboardEvent) {
    event.preventDefault(); // Prevents any keyboard input
  }
  preventPaste(event: ClipboardEvent) {
    event.preventDefault(); // Prevents paste input
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

}
