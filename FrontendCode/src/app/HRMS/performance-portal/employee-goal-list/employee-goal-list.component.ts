import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { Router } from '@angular/router';
import { GoalsApiService } from '../goals/goals-apiservice';

@Component({
  selector: 'app-employee-goal-list',
  standalone: true,
  imports: [SharedModule, FormsModule, ToastMessageComponent],
  templateUrl: './employee-goal-list.component.html',
  styleUrl: './employee-goal-list.component.scss'
})
export class EmployeeGoalListComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChildren('weightageInput') weightageInputs!: QueryList<ElementRef<HTMLInputElement>>;
  @ViewChildren('goalInput') goalInputs!: QueryList<ElementRef<HTMLInputElement>>;

  ngAfterViewInit() {
    // Ensure all input elements are available before accessing them
    this.weightageInputs.changes.subscribe(() => {
      console.log('Input elements:', this.weightageInputs);
    });
    // Ensure all input elements are available before accessing them
    this.goalInputs.changes.subscribe(() => {
      console.log('Input elements:', this.goalInputs);
    });
  }

  empId: any;
  employeeId: any;
  isDetailView: boolean = false;
  goalDetails: any[] = [];
  rowsGoalforApprove: any[] = [];
  buttonView: boolean = false;
  loading: boolean = false;
  isTableData: boolean = false;

  constructor(private apiService: GoalsApiService, private router: Router) { }

  employeeGoals: any[] = [];
  employeeGoalstemp: any[] = [];
  localStorageData: any | undefined;
  rows: any;
  getFinancialYear: any;

  ngOnInit(): void {

    // this.localStorageData=localStorage.getItem('GoalData');
    // this.employeeGoalstemp = JSON.parse(this.localStorageData);

    //     this.employeeId = sessionStorage.getItem('userDetail');
    // const sessionData = JSON.parse(this.employeeId);
    this.employeeId = sessionStorage.getItem('userdata');
    const sessionData = this.employeeId ? JSON.parse(this.employeeId) : null;
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData
    const employeeId = sessionData.EmpId;
    //  const employeeName = "sunder";
    this.empId = employeeId;
    this.getAllGoalList(this.empId);
    // Loop through your data


  }
  getAllGoalEmpError: any;
  getAllGoalList(empDetails: any) {
    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };
    this.employeeGoals = [];
    this.apiService.getALLGoalEmployee(empData).then(
      (data: any) => {
        console.log(data);
        //  this.tableData=data;
        this.employeeGoalstemp = data;
        const addedEmployeeIds = new Set<number>();
        this.employeeGoalstemp.forEach(item => {
          // Check if the employeeid has not been added yet
          if (!addedEmployeeIds.has(item.EmpId)) {
            // Add the employeeid to the Set
            addedEmployeeIds.add(item.EmpId);

            // Push the item to the rows array
            this.employeeGoals.push(item);
            this.getAllGoalEmpError = '';
            console.log("TEST-------", this.employeeGoals, addedEmployeeIds);
          }
        });
        console.log("dispaly all goadDAta here", this.employeeGoals);
        //   this.names = this.tableData.map((item: any) => item.Goal);
        // if (this.employeeId && this.tableData) {
        //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
        //   this.names = this.tableData.map((item: any) => item.name);
        // }

      },
      (error: any) => {
        console.error('An error occurred:', error.msg);
        this.employeeGoals = [];
        this.getAllGoalEmpError = "Internal Server Error";
        this.isTableData = true;
      }
    );
  }
  viewGoal(empId: any) {
    this.goalDetails = this.employeeGoalstemp.filter((row: any) => row.EmpId == empId);
    console.log(this.goalDetails, "goal data in view goal");
    this.isDetailView = true;
    this.buttonView = true;
  }
  goal = {
    Weightage: ''
  };

  getTotalWeightageApproved: any;
  logGoalWeightages() {
    const weightages = this.goalDetails.map(goal => Number(goal.Weightage));
    const total = weightages.reduce((sum, w) => sum + w, 0);
    console.log("Goal Weightages:", weightages);
    console.log("Total Weightage:", total);
    this.getTotalWeightageApproved = total
  }

  async ApprpveGoal() {
    if (this.getTotalWeightageApproved == 80) {
      const empMap = new Map();

      this.goalDetails.forEach((data, index) => {
        const inputElement = this.weightageInputs.toArray()[index];
        const weightage = inputElement.nativeElement.value;

        const inputElement1 = this.goalInputs.toArray()[index];
        const goal = inputElement1.nativeElement.value;

        if (!empMap.has(data.EmpId)) {
          empMap.set(data.EmpId, []);
        }
        empMap.get(data.EmpId).push({
          EmpId: data.EmpId,
          GoalId: data.GoalId,
          Goal: goal,
          Weightage: weightage
        });
        console.log(empMap);

      });

      // Convert the map to the desired output format
      const result = Array.from(empMap, ([EmpId, listofGoal]) => ({
        EmpId: this.empId, // Assuming this.empId is the same for all goals
        listofGoal
      }));

      console.log("approved Goals", result);
      console.log("approved Goals Map", empMap); // Logging the map for debugging

      if (result.length > 0) { // Check if there are goals to approve
        try {
          const data = await this.apiService.ApproveAllGoal(result);
          if (data != null) {
            this.triggerToast(data.msg, "Goal Approved Successfully", "success");
            this.loading = false;
            this.isDetailView = false;
            this.buttonView = false;
            this.getAllGoalList(this.empId); // Refresh goal list after approval
          }
        } catch (error) {
          console.error("Error approving goals:", error);
          this.triggerToast("Error occurred while approving goals", "Error", "danger");
        }
      }
      else {
        this.triggerToast("Goals Should not be Empty", "Warning", "danger");
        this.loading = false;
      }
    } else {
      this.triggerToast("Weight cannot be greater than or less than 80%.", "Warning", "warning");
    }

  }



  returnPage() {
    // Navigate to the desired route
    this.isDetailView = false;
    this.buttonView = false;
    this.getAllGoalList(this.empId);
    //this.router.navigate(['EmployeeGoalList']);
    // this.router.navigate(['/auth/signin']);
  }

  triggerToast(header: any, body: any, mess: any) {
    // const header = 'Toast Header';
    // const body = 'This is a toast message.';
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
