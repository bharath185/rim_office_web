import { Component, ViewChild } from '@angular/core';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { DatePipe } from '@angular/common';
import { SelfDevApiService } from './self-developmentapi';

@Component({
  selector: 'app-self-development-goal',
  standalone: true,
  providers: [DatePipe],
  imports: [SharedModule, FormsModule, ToastMessageComponent],
  templateUrl: './self-development-goal.component.html',
  styleUrl: './self-development-goal.component.scss'
})
export class SelfDevelopmentGoalComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  employeeId: any;
  empId: any;
  getFinancialYear: any;
  constructor(private datePipe: DatePipe, private selfapiService: SelfDevApiService) { }

  rows: any[] = [];
  isEditing: boolean = false;
  editingRowIndex: number = -1;
  errormessage: any;
  maxWeightage: any = 80;
  Weightage: any = 0;
  showWarning: boolean = false;
  loading: boolean = false;
  ngOnInit(): void {
    this.employeeId = sessionStorage.getItem('userdata');
    const sessionData = JSON.parse(this.employeeId);
    console.log(this.employeeId);
    this.getALLSelfDev(sessionData.EmpId);
    const employeeId = sessionData.EmpId;
    //  const employeeName = "sunder";
    this.empId = employeeId;
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData
  }

  addRow() {
    if (!this.isEditing) {
      // Get current date and format it
      // const currentDate = new Date();
      // const formattedDate = this.datePipe.transform(currentDate, 'yyyy-MM-dd');

      // Push new row with current date
      this.rows.push({
        Id: '',
        Activity: '',
        ActionDescription: '',
        ActionType: '',
        Status: '',
        StartDate: '', // Set Startdate to current date
        DueDate: '',   // Set Duedate to current date
        CompletedDate: '',
        EmpId: this.empId,
        QId: 0,
        PeriodId: 0,
        isEditing: true
      });

      this.isEditing = true;
      this.editingRowIndex = this.rows.length - 1;
    }
  }

  // removeRow(index: number ,row: any) {
  //   if (!this.isEditing) {
  //     this.rows.splice(index, 1);
  //   }

  // }
  cancelEdit(row: any) {
    if (row.GoalId != 0 || row.GoalId != '') {
      row.isEditing = false;
      this.isEditing = false;
     
      this.editingRowIndex = -1;
      this.getALLSelfDev(this.empId);
    }else{

      const index = this.rows.indexOf(row);
      if (index !== -1) {
          this.rows.splice(index, 1);
          this.isEditing = false;
      }
    }
}
  async removeRow(index: number, row: any) {

    if (!this.isEditing) {
      this.loading=true;
      if (row.Id != 0 || row.Id != '') {

        const empData = {
          Id: row.Id,
          EmpId: this.empId

        };
        const data = await this.selfapiService.deleteSelfDev(empData);
        console.log(`Row ${index} updated:`, data);
        if (data.msg == 'Deleted') {
          this.rows.splice(index, 1);
          this.loading=false;
          this.triggerToast(data.msg, "deleted Successfully", "danger");
         // this.triggerToast(data.msg, "deleted Successfully", "danger");
        //  const numbers = this.rows.map(row => parseInt(row.Weightage));
         // const sum = numbers.reduce((acc, curr) => acc + curr, 0);
         // this.Weightage = sum;
        }

        // this.triggerToast("data.Message", data.Message);
      } else {
        this.loading=false;
        // Create API call for other rows
        //  const data = await this.behaviorService.addBehavior([row]);
        //  console.log(`Row ${index} created:`, data);
        this.rows.splice(index, 1);
      }
    }

  }

  async toggleEdit(row: any, index: number) {
    if (this.editingRowIndex === index) {
      if (row.Activity.trim() === '' || row.ActionDescription.trim() === '' || row.StartDate.trim() === '' ) {

        // this.showWarning = true;
        // setTimeout(() => {
        //   this.showWarning = false;
        // }, 2000);
        // this.errormessage = 'Cannot save an empty row.';
        this.triggerToast("warning", "Cannot save an empty row.", "danger");
        return;

      } else {
        row.isEditing = !row.isEditing;
        this.errormessage = '';
      }


      this.isEditing = this.rows.some(row => row.isEditing);
      if (!row.isEditing) {
        this.editingRowIndex = -1;
      }
      // const numbers = this.rows.map(row => parseInt(row.email, 10));
      // const sum = numbers.reduce((acc, curr) => acc + curr, 0);
      // this.Weightage=sum;
      // if(this.Weightage > this.maxWeightage){

      //   this.showWarning = true;
      //   setTimeout(() => {
      //     this.showWarning = false;
      //   }, 2000);
      //   this.errormessage = "Weightage exeeded it won't be more than 80 %";
      //   console.log("sum",sum,this.maxWeightage,this.Weightage);
      //     row.isEditing = true;
      // }
      if (row.Id != 0 || row.Id != '') {
        // Update API call for row with Id: 5
this.loading=true;
        const data = await this.selfapiService.updateSelfDev([row]);
        console.log(`Row ${index} updated:`, data);
        if (data.msg == 'Updated') {
          this.loading=false;
          this.triggerToast("success", "Updated Successfully.", "success");
          //  this.getALL(this.empId);
          this.getALLSelfDev(this.empId);
        }


      } else {
        // Create API call for other rows
        this.loading=true;
        // this.errormessage = "Weightage exeeded it won't be more than 20 %";
        const data = await this.selfapiService.addSelfDev([row]);
        console.log(`Row ${index} created:`, data);

        if (data.msg == 'Added') {
          this.loading=false;
          this.triggerToast("success", "Added Successfully.", "success");
          // this.getALL(this.empId);
          this.getALLSelfDev(this.empId);
        }



      }

    } else {
      if (!this.isEditing) {
        row.isEditing = true;
        this.isEditing = true;
        this.editingRowIndex = index;
      }
    }
  }

  getALLSelfDev(empDetails: any) {
 
    this.rows=[];
    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };

    this.selfapiService.getALLSelfDev(empData).then(
      (data:any) => {
        console.log(data);
        //  this.tableData=data;
        // this.rows=data.filter((row: any) => !row.Description);
        this.rows = data
        this.rows.forEach((rowToUpdate: any) => {
          const item = data.find((item: any) => item.Id === rowToUpdate.Id);
          if (item) {
              // Parse and format the StartDate
              const parsedStartDate = this.parseDate(item.StartDate);
              const parsedStartDate1 = this.parseDate(item.DueDate);
              const parsedStartDate2 = this.parseDate(item.CompletedDate);
              // Update the StartDate of the current row
              rowToUpdate.StartDate = parsedStartDate;
              rowToUpdate.DueDate = parsedStartDate1;
              rowToUpdate.CompletedDate = parsedStartDate2;
             
          }
      });
      
        //   this.names = this.tableData.map((item: any) => item.Goal);
        // if (this.employeeId && this.tableData) {
        //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
        //   this.names = this.tableData.map((item: any) => item.name);
        // }

      },
      (error:any) => {
        console.error('An error occurred:', error.msg);
        this.triggerToast("danger", "Something Went Wrong Contact Admin", "danger");
        
      }
    );
  }
  parseDate(dateString: string): string {
    if (!dateString) {
      return ''; // Handle case where dateString is empty or null
    }
  
    const timestamp = parseInt(dateString.replace(/[^0-9]/g, ''));
    const date = new Date(timestamp);
    return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
  }
  triggerToast(header: any, body: any, mess: any) {
    // const header = 'Toast Header';
    // const body = 'This is a toast message.';
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
