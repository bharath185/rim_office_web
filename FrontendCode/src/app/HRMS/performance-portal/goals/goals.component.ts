import { Component, Input, NgModule, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GoalsApiService } from './goals-apiservice';

@Component({
  selector: 'app-goals',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, FormsModule],
  templateUrl: './goals.component.html',
  styleUrl: './goals.component.scss'
})
export class GoalsComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  rows: any[] = [];
  rowstemp: any;
  isEditing: boolean = false;
  editingRowIndex: number = -1;
  errormessage: any;
  maxWeightage: any = 80;
  Weightage: any = 0;
  showWarning: boolean = false;
  showCard: boolean = false;
  showCard1: boolean = false;
  employeeId: any;
  localStorageData: any | undefined;
  tableData: any;
  names: any;
  empId: any;
  GoalButton: boolean = true;
  ReturnButton: boolean = false;
  loading: boolean = false;
  public isButtonDisabled: boolean = false;
  public isButtonDisabledNew: boolean = true;
  getAllRecordLength: any;
  getFinancialYear: any;
  constructor(private apiService: GoalsApiService) { }
  ngOnInit(): void {
    this.employeeId = sessionStorage.getItem('userdata');
    const sessionData = JSON.parse(this.employeeId);
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData
    this.getALLwithStatus(sessionData.EmpId);
    this.getALLTask(sessionData.EmpId, 0);
    const employeeId = sessionData.EmpId;
    this.empId = employeeId;
  }

  addRow() {
    const sessionData = JSON.parse(this.employeeId);

    const employeeId = sessionData.EmpId;
    //  const employeeName = "sunder";
    this.empId = employeeId;
    console.log();
    if (!this.isEditing) {
      this.rows.push({ GoalId: '', QId: 0, PeriodId: 0, Goal: '', Weightage: '', Discription: ' ', status: 'pending', EmpId: employeeId, isEditing: true });
      this.isEditing = true;
      this.isButtonDisabled = true;
      this.editingRowIndex = this.rows.length - 1;
    } else {

    }
  }
  cancelEdit(row: any) {
    if (row.GoalId != 0 || row.GoalId != '') {
      row.isEditing = false;
      this.isEditing = false;
      this.isButtonDisabled = false;
      this.editingRowIndex = -1;
      this.getALL(this.empId);
    } else {

      const index = this.rows.indexOf(row);
      if (index !== -1) {
        this.rows.splice(index, 1);
        this.isButtonDisabled = false;
        this.isEditing = false;
      }
    }
  }
  async removeRow(index: number, row: any) {
    if (!this.isEditing) {
      if (row.GoalId != 0 || row.GoalId != '') {

        const empData = {
          GoalId: row.GoalId,
          EmpId: this.empId

        };
        const data = await this.apiService.deleteGoal(empData);
        console.log(`Row ${index} updated:`, data);
        if (data.msg == 'Deleted') {
          this.rows.splice(index, 1);
          this.triggerToast(data.msg, "deleted Successfully", "danger");
          const numbers = this.rows.map(row => parseInt(row.Weightage));
          const sum = numbers.reduce((acc, curr) => acc + curr, 0);
          this.Weightage = sum;
        }
        // this.triggerToast("data.Message", data.Message);
      } else {
        // Create API call for other rows
        //  const data = await this.behaviorService.addBehavior([row]);
        //  console.log(`Row ${index} created:`, data);
        this.rows.splice(index, 1);
      }
    }

  }
  toggleCard() {
    this.ReturnButton = true;
    this.GoalButton = false;
    this.showCard = !this.showCard;
    this.showCard1 = false;
  }
  toggleCard1() {
    this.ReturnButton = true;
    this.GoalButton = false;
    this.showCard1 = true;
    this.showCard = false;
    this.getALL(this.empId);

  }
  onPaste(event: ClipboardEvent): void {
    event.preventDefault();
  }

  validateWeightage(row: any) {
    if (row.Weightage < 0) {
      row.Weightage = 0; // Reset to minimum
    } else if (row.Weightage >= 80) {
      row.Weightage = 80; // Reset to maximum
    }
  }

  //#########################################

  async toggleEdit(row: any, index: number) {
    if (this.editingRowIndex === index) {
      if (row.Goal.trim() === '' || row.Weightage.trim() === '') {
        this.triggerToast('Warning', 'Cannot save an empty row.', "danger");
        return;
      } else {
        row.isEditing = !row.isEditing;
        this.errormessage = '';
        this.isEditing = this.rows.some(row => row.isEditing);
        if (!row.isEditing) {
          this.editingRowIndex = -1;
        }
      }
      const numbers = this.rows.map(row => parseInt(row.Weightage));
      const sum = numbers.reduce((acc, curr) => acc + curr, 0);
      this.Weightage = sum;
      console.log("sum", sum, this.maxWeightage, this.Weightage);
      if (this.Weightage > this.maxWeightage) {
        this.triggerToast('Warning', 'Weightage exeeded it cant be more than 80 %', "danger");
        this.Weightage = 0;
        console.log("sum", sum, this.maxWeightage, this.Weightage);
        row.isEditing = true;
        this.isButtonDisabled = true;
      } else {
        if (row.GoalId != 0 || row.GoalId != '') {
          this.isButtonDisabled = true;
          if (row.Weightage && row.Weightage % 1 !== 0) {
            this.triggerToast('Warning', 'Decimal or Text values wont be Allowed Please check', "danger");
            row.isEditing = true;
            this.isButtonDisabled = true;
          } else {
            const data = await this.apiService.updateGoal([row]);
            console.log(`Row ${index} updated:`, data);
            if (data.msg == 'Updated') {
              this.triggerToast(data.msg, "Updated Successfully", "success");
              this.getALL(this.empId);
              const numbers = this.rows.map(row => parseInt(row.Weightage));
              const sum = numbers.reduce((acc, curr) => acc + curr, 0);
              this.isButtonDisabled = false;
              this.Weightage = sum;
            } else {
              this.triggerToast(data.StatusCode, data.Message, "danger");
              row.isEditing = true;
              this.isButtonDisabled = true;
              const numbers = this.rows.map(row => parseInt(row.Weightage));
              const sum = numbers.reduce((acc, curr) => acc + curr, 0);
              this.Weightage = sum;
            }
          }

        } else {
          // Create API call for other rows
          if (row.Weightage && row.Weightage % 1 !== 0) {
            this.triggerToast('Warning', 'Decimal or Text values wont be Allowed Please check', "danger");
            row.isEditing = true;
            this.isButtonDisabled = true;
          } else {
            const data = await this.apiService.addGoal([row]);
            console.log(`Row ${index} created:`, data);

            if (data.msg == 'Added') {
              const numbers = this.rows.map(row => parseInt(row.Weightage));
              const sum = numbers.reduce((acc, curr) => acc + curr, 0);
              this.Weightage = sum;
              this.triggerToast(data.msg, "Added Successfully", "success");
              this.isButtonDisabled = false;
              this.getALL(this.empId);
            } else {
              this.triggerToast(data.StatusCode, data.Message, "danger");
              row.isEditing = true;
              const numbers = this.rows.map(row => parseInt(row.Weightage));
              const sum = numbers.reduce((acc, curr) => acc + curr, 0);
              this.Weightage = sum;
              this.isButtonDisabled = true;
            }
          }
          // this.errormessage = "Weightage exeeded it won't be more than 20 %";
        }
      }

    } else {
      if (!this.isEditing) {
        row.isEditing = true;
        this.isEditing = true;
        this.editingRowIndex = index;
        this.isButtonDisabled = true;
      }
    }
  }

  async finalSubmitGoal(): Promise<void> {
    this.loading = true;
    console.log('Initial Form Data:', JSON.stringify(this.rows));
    try {
      const empMap = new Map();
      this.rows.forEach(data => {
        if (!empMap.has(data.EmpId)) {
          empMap.set(data.EmpId, []);
        }
        empMap.get(data.EmpId).push({ EmpId: data.EmpId, GoalId: data.GoalId, Goal: data.Goal, Weightage: data.Weightage });
      });
      const result = Array.from(empMap, ([EmpId, listofGoal]) => ({ EmpId, listofGoal }));
      const jsonResult = result[0];
      console.log(jsonResult);
      if (!this.isButtonDisabled && jsonResult) {
        const data = await this.apiService.AddAllGoals(jsonResult);
        if (data != null) {
          this.triggerToast(data.msg, "Final Submitted  Successfully", "success");
          this.loading = false;
          this.showCard = false;
          this.showCard1 = false;
          this.ReturnButton = false;
          this.getALLwithStatus(this.empId);
        }
      } else {
        this.triggerToast("warning", "Goals Should not be Empty", "success");
        this.showCard = false;
        this.showCard1 = true;
        this.loading = false;
        this.isEditing = false;
      }
      console.log('Updated Form Data:', JSON.stringify(this.rows));
    } catch (error) {
      console.error('An error occurred:', error);
      this.loading = false;
      this.triggerToast("Error is Occur", "Something Went Wrong", "danger");
    }
  }




  //#########################################

  selectedGoal: string = '';
  selectedQuarter: string = '';
  taskName: string = '';
  selectedStatus: string = '';
  tasks: any[] = [];

  async addTask() {
    if (this.selectedGoal && this.selectedQuarter && this.taskName && this.selectedStatus) {
      this.tasks.push({
        TaskId: '',
        GoalId: this.selectedGoal,
        EmpId: this.empId,
        Task: this.taskName,
        QId: 0,
        PeriodId: 0,
        Description: ''
      });

      const data = await this.apiService.addTask(this.tasks);
      if (data.msg == 'Added') {
        this.getALLTask(this.empId, this.selectedGoal);
      }
      // Clear form fields after submission
      this.selectedGoal = '';
      this.selectedQuarter = '';
      this.taskName = '';
      this.selectedStatus = '';
    }
  }



  getALL(empDetails: any) {
    this.loading = true;
    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };

    this.apiService.getALLGoal(empData).then(
      (data) => {
        console.log(data);
        this.getAllRecordLength = data.length;
        console.log(this.getAllRecordLength);

        this.rows = data.filter((row: any) => row.FinalSubmit == false);
        const numbers = this.rows.map(row => parseInt(row.Weightage));
        const sum = numbers.reduce((acc, curr) => acc + curr, 0);
        this.Weightage = sum;
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.triggerToast("Error is Occur", "Something Went Wrong", "danger");
      }
    );
  }

  getALLTask(empDetails: any, goalId: any) {

    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails,
      GoalId: goalId
    };

    this.apiService.getAllTask(empData).then(
      (data) => {
        console.log(data);
        //  this.tableData=data;
        this.tasks = data;
        const groupedByGoalId = data.reduce((acc: any, item: any) => {
          if (!acc[item.GoalId]) {
            acc[item.GoalId] = [];
          }
          acc[item.GoalId].push(item);
          return acc;
        }, {});

        console.log(groupedByGoalId);

      },
      (error) => {
        console.error('An error occurred:', error.msg);
        this.loading = false;
        this.triggerToast("Error is Occur", "Something Went Wrong", "danger");
      }
    );
  }
  weightageFlag: boolean = false;
  getALLwithStatus(empDetails: any) {
    this.loading = true;
    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };

    this.apiService.getALLGoal(empData).then(
      (data) => {
        console.log(data);
        //  this.tableData=data;
        this.tableData = data.filter((row: any) => row.FinalSubmit == true);
        this.names = this.tableData.map((item: any) => ({ Goal: item.Goal, GoalId: item.GoalId }));
        const sumOfWeightage = this.tableData.reduce((total: number, row: { Weightage: string }) => {
          // Convert Weightage from string to number
          const weightage = parseInt(row.Weightage, 10);
          // Add to the total
          return total + weightage;
        }, 0);
        this.GoalButton = sumOfWeightage < 80;
        console.log("sum of weights", sumOfWeightage < 80, sumOfWeightage);
        this.loading = false;
      },
      (error) => {
        console.error('An error occurred:', error.msg);
        this.loading = false;
        this.triggerToast("Error is Occur", "Something Went Wrong", "danger");
      }
    );
  }

  // getFinancialYear:any;
  // financialyear(empDetails: any) {
  //   this.loading = true;
  //   console.log('Form Data:', JSON.stringify(empDetails));
  //   const empData = {
  //     EmpId: empDetails
  //   };
  //   this.apiService.getFYearDetails(empData).then(
  //     (data) => {
  //       console.log(data);
  //       this.getFinancialYear = data
  //       this.loading = false;
  //     },
  //     (error) => {
  //       console.error('An error occurred:', error.msg);
  //       this.loading = false;
  //       this.triggerToast("Error is Occur", "Something Went Wrong", "danger");
  //     }
  //   );
  // }

  triggerToast(header: any, body: any, mess: any) {
    // const header = 'Toast Header';
    // const body = 'This is a toast message.';
    this.toastMessageComponent.showToast(header, body, mess);
  }


  returnPage() {
    this.GoalButton = true;
    this.ReturnButton = false;
    this.showCard = false;
    this.showCard1 = false;
    this.isEditing = false;
    this.getALLwithStatus(this.empId);

  }

}
