import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { ConfigrationService } from './configration.service';

interface Quarter {
  name: string;
  startDate: Date;
  endDate: Date;
  days: number;
  selected: boolean;
}

@Component({
  selector: 'app-configration',
  standalone: true,
  imports: [SharedModule, FormsModule, ToastMessageComponent],
  templateUrl: './configration.component.html',
  styleUrl: './configration.component.scss'
})
export class ConfigrationComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  employeeId: any;
  empId: any;
  financialYears: any;
  YearId: any;
  Qtype: any;
  getFinancialYear: any;
  isSpinner: boolean = false;

  constructor(private hrtimeFrameService: ConfigrationService) { }

  ngOnInit(): void {
    this.employeeId = sessionStorage.getItem('userdata');
    const sessionData = JSON.parse(this.employeeId);
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData
    console.log(this.employeeId);
    const employeeId = sessionData.EmpId;
    //  const employeeName = "sunder";
    this.empId = employeeId;
    this.getALLDDFyear(this.empId);
    this.getMainTableFinancialYear();
  }

  showCard: boolean = false;
  showCard1: boolean = false;

  toggleCard() {
    this.showCard = !this.showCard;
    this.showCard1 = false;
  }
  toggleCard1() {
    this.showCard1 = !this.showCard1;
    this.showCard = false;

  }

  selectedYear: any = [];
  isQuarterly: boolean = false;
  isHalfYearly: boolean = false;
  isYearly: boolean = false;
  dates: any[] = [];
  mainTableData: any[] = [];


  onCheckboxChange(type: string) {
    if (type === 'quarterly') {
      this.GetQuaterDetails('Quater', this.selectedYear);
      this.Type = 'Quater';
      this.isHalfYearly = false;
      this.isYearly = false;
    } else if (type === 'halfYearly') {
      this.GetQuaterDetails('Half', this.selectedYear);
      this.Type = 'Half';
      this.isQuarterly = false;
      this.isYearly = false;
    } else if (type === 'Yearly') {
      this.GetQuaterDetails('Annual', this.selectedYear);
      this.Type = 'Annual';
      this.isQuarterly = false;
      this.isHalfYearly = false;
    }

  }


  triggerToast(header: any, body: any, mess: any) {
    // const header = 'Toast Header';
    // const body = 'This is a toast message.';
    this.toastMessageComponent.showToast(header, body, mess);
  }


  Fyear: any;
  Type: any;
  onSubmit() {
    this.showCard = false;
    const empData = {
      EmpId: this.empId,
      FYearId: this.selectedYear,
      FYear: this.Fyear,
      Type: this.Type
    };
    this.isSpinner = true;
    this.hrtimeFrameService.submitQuaterDetails(empData).then(
      (data) => {
        //  this.selectedYear = this.financialYears.length > 0 ? this.financialYears[0].YearId : null;
        console.log(this.financialYears, "years");
        this.getMainTableFinancialYear();
        if (data.StatusCode === 404) {
          this.triggerToast("warning", data.Message, "danger");
        } else {
          this.triggerToast("Success", "Details Saved Successfully..", "success");
        }
        this.isSpinner = false;

      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
    this.dates = [];  // Clear the temporary table
  }



  getALLDDFyear(empId: any) {
    const empData = {
      EmpId: empId
    };
    this.isSpinner = true;

    console.log(this.selectedYear);
    this.hrtimeFrameService.getALLDDFyear(empData).then(
      (data) => {

        this.financialYears = data;
        this.selectedYear = this.financialYears.length > 0 ? this.financialYears[0].YearId : null;
        const selectedYear = this.financialYears.find((year: any) => year.YearId === this.selectedYear); // Specify the type of year
        console.log("Selected year:", selectedYear);
        this.Fyear = selectedYear.FinancialYear;
        console.log(this.financialYears, "years");
        this.isSpinner = false;

      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
  }

  GetQuaterDetails(qtype: any, yearId: any) {
    const empData = {
      EmpId: this.empId,
      YearId: yearId,
      Type: qtype
    };
    this.isSpinner = true;


    this.hrtimeFrameService.GetQuaterDetails(empData).then(
      (data) => {

        // this.financialYears = data;
        console.log(this.financialYears, "years");
        //this.getALLDDFyear(this.empId);
        this.dates = data;
        console.log(this.dates, "dates");
      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
    this.isSpinner = false;

  }
  OnYearchange(event: any) {
    const selectedValue = parseInt(event.target.value, 10); // Parse the value as a number
    const selectedYear = this.financialYears.find((year: any) => year.YearId === selectedValue); // Specify the type of year
    console.log("Selected year:", selectedYear);
    this.Fyear = selectedYear.FinancialYear;
    this.selectedYear = selectedYear.YearId;
  }
  financialYear: any;
  getMainTableFinancialYear() {
    const empData = {
      EmpId: this.empId

    };

    this.isSpinner = true;

    this.hrtimeFrameService.getMainTableFinancialYear(empData).then(
      (data) => {

        // this.financialYears = data;
        console.log(this.financialYears, "years");
        //this.getALLDDFyear(this.empId);
        this.mainTableData = data;
        this.financialYear = this.mainTableData[0].FYear;
        console.log(this.dates, "dates");
      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
    this.isSpinner = false;

  }
  // endDAte:any;

  // endDate:boolean=true;
  // extDate:boolean=false;

  // goalEndDate(data: any) {
  //   console.log("End Date for", ":", this.endDAte);
  //   // Implement your logic for handling end date update
  //   this.endDate=false;
  //   this.extDate=true;
  //   this.endDAte='';
  // }

  goalExtension(data: any) {
    const empData = {
      ConfigSetupId: data.ConfigSetupId,
      EmpId: this.empId,
      FYearId: data.FYearId,
      FYear: data.FYear,
      QId: data.QId,
      Type: data.Type,
      ExtendCreationDate: data.ExtendCreationDate,
      ExtendSubmitDate: data.ExtendSubmitDate

    };
    this.isSpinner = true;

    console.log("Start Date for", ":", data);
    this.hrtimeFrameService.updateConfigsetupGoalExtDate(empData).then(
      (data) => {

        console.log(data);
        this.getMainTableFinancialYear();
        data.flag2 = false;
      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
    this.isSpinner = false;

  }
  reviewExtension(data: any) {

    const empData = {
      ConfigSetupId: data.ConfigSetupId,
      EmpId: this.empId,
      FYearId: data.FYearId,
      FYear: data.FYear,
      QId: data.QId,
      Type: data.Type,
      ExtendCreationDate: data.ExtendCreationDate,
      ExtendSubmitDate: data.ExtendSubmitDate

    };
    this.isSpinner = true;

    console.log("Start Date for", ":", data);
    this.hrtimeFrameService.updateConfigsetupReviewExtDate(empData).then(
      (data) => {

        console.log(data);
        this.getMainTableFinancialYear();
        data.flag2 = false;
      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
    this.isSpinner = false;

  }

  // reviewExtension(data: any) {
  //   console.log("Start Date for review ", data.QName, ":", this.endDAte);
  //   this.endDAte='';
  //   // Implement your logic for handling start date update
  // }
  // flag:boolean=false;
  goalExtDatechange(data: any, newStartDate: string) {
    console.log("New Start Date for", data.QName, ":", newStartDate);
    data.flag1 = true;
    if (new Date(data.ExtendSubmitDate) < new Date(data.ExtendCreationDate)) {
      // Optionally reset ExtendSubmitDate to the new ExtendCreationDate or a valid date
      data.ExtendSubmitDate = data.ExtendCreationDate;
    }

  }

  reviewExtDatechange(data: any, newEndDate: string) {
    console.log("New End Date for", data.QName, ":", newEndDate);
    data.flag2 = true;
  }
}
