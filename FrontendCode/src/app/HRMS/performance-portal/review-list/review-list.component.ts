import { Component, Input, NgModule, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorService } from '../behavior/behavior.service';
import { GoalsApiService } from '../goals/goals-apiservice';
import { PerformancePortalService } from '../../service/performancePortal/performance-portal.service';

@Component({
  selector: 'app-review-list',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, FormsModule],
  templateUrl: './review-list.component.html',
  styleUrl: './review-list.component.scss'
})
export class ReviewListComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  cusomerfocusFlag: boolean = false;
  employeeId: any;
  empId: any;
  tableData: any[] = [];
  tasks: any[] = [];
  rows: any;
  getFinancialYear: any;
  // names: any;

  constructor(private apiService: GoalsApiService, private empService:PerformancePortalService,private behaviorService: BehaviorService) { }
  ngOnInit(): void {
    this.employeeId = sessionStorage.getItem('userdata');
    const sessionData = JSON.parse(this.employeeId);
    console.log(this.employeeId);
    this.getAllEmployeeReviewList(sessionData.EmpId);
    const employeeId = sessionData.EmpId;
    //  const employeeName = "sunder";
    this.empId = employeeId;
    // console.log(this.names);
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData
  }
  selectedGoal: any;
  tab1: boolean = true;
  tab2: boolean = false;
  tab3: boolean = false;
  tab4: boolean = false;
  showreviewform:boolean =false;
  showform: boolean = true;
  showInfo1: boolean = false;
  showInfo2: boolean = false;
  showInfo3: boolean = false;
  showInfo4: boolean = false;
  showInfo5: boolean = false;
  showInfo7: boolean = false;
  goals: any[] = [];
  behavior: any[] = [];
  showcustomerFocus: boolean = false;

  // activeTab: string = 'tab1'; // Initially set to tab1

  // switchTab(tab: string) {
  //   this.activeTab = tab;
  // }

  dataRows: any[] = [];

  getALLwithStatus(empDetails: any) {

    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };

    this.apiService.getALLGoal(empData).then(
      (data) => {
        console.log(data);
        //  this.tableData=data;
        this.goals = data.filter((row: any) => row.FinalSubmit == true);
        console.log(data);
      //  this.goals = data.filter((row: any) => row.Description == 'Approved');
        //   this.names = this.tableData.map((item: any) => item.Goal);
        // if (this.employeeId && this.tableData) {
        //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
        //   this.names = this.tableData.map((item: any) => item.name);
        // }

      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );
  }
  getAllBehavior(empDetails: any) {
    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };

    this.behaviorService.getALLBehaviors(empData).then(
      (data) => {
        console.log(data);
        this.behavior = data;
        console.log(this.behavior,"behavior");
        //this.names = this.tableData.map((item: any) => item.Goal);
        // if (this.employeeId && this.tableData) {
        //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
        //   this.names = this.tableData.map((item: any) => item.name);
        // }

      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );

  }
  rowemployee:any;
  viewReviewForm(row:any){
 console.log("rpw",row);
 this.showform = !this.showform;
 this.showreviewform=true;
 this.getALLwithStatus(row.EmpId);
 this.rowemployee=row.EmpId;
 this.getAllBehavior(row.EmpId);
 this.getEmpDetails(row.EmpId,row.employeeCode);
  }

  
// getEmployeeDetails(empId:any){
//   this.empService.getUserDetais(empId).then(
//     (data) => {
//       console.log(data);
//       sessionStorage.setItem('userDetail1', JSON.stringify(data));
//     }
//   );
// }

getAllEmployeeReviewList(oempDetails: any) {
  console.log('Form Data:', JSON.stringify(oempDetails));
  const empData = {
    EmpId: oempDetails
  };

  this.behaviorService.GetAllEmployeeReviewList(empData)
    .then((data) => {
      console.log(data);
      this.dataRows = data;

      // Process each row to fetch employee details
      this.dataRows.forEach(row => {
        const empData = {
          EmpId: row.EmpId
        };      
        this.empService.getUserDetais(empData)
          .then((response:any) => {
           // row.EmpId = response[0].EmpCode; // Assuming response structure has employeeCode
            row.employeeCode = response[0].EmpCode;
            console.log(row.EmpId);
          })
          .catch((error:any) => {
            console.error(`Error fetching employee details for EmpId ${row.EmpId}:`, error);
          });
      });
    })
    .catch((error) => {
      console.error('An error occurred:', error.msg);
    });
}
parseDate(dateString: string): string {
  if (!dateString) {
    return ''; // Handle case where dateString is empty or null
  }

  const timestamp = parseInt(dateString.replace(/[^0-9]/g, ''));
  const date = new Date(timestamp);
  return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
}
  employeeData:any;
  getEmpDetails(empId: any,username:any) {
    // this.employeeData1=[];
    console.log('Form Data:', JSON.stringify(username));
    const empData = {
      EmpId: empId,
      username: username
    };

    this.behaviorService.getEmployeeDetails(empData).then(
      (data) => {
        console.log(data);
        this.employeeData = data;
// console.log("before converting date ", JSON.parse(data));
        this.employeeData.forEach((rowToUpdate: any) => {
          const item =  data.find((item: any) => item.EmpId === rowToUpdate.EmpId);
          if (item) {
              // Parse and format the StartDate
              const parsedStartDate = this.parseDate(item.JoiningDate);
             // console.log(JSON.parse(item.JoiningDate));
              // Update the StartDate of  console.log(data); the current row
              rowToUpdate.JoiningDate = parsedStartDate;

             
          }
      });
        //this.names = this.tableData.map((item: any) => item.Goal);
        // if (this.employeeId && this.tableData) {
        //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
        //   this.names = this.tableData.map((item: any) => item.name);
        // }

      },
      (error) => {
        console.error('An error occurred:', error.msg);
      }
    );

  }

  triggerToast(header: any, body: any,mess:any) {
    // const header = 'Toast Header';
    // const body = 'This is a toast message.';
    this.toastMessageComponent.showToast(header, body,mess);
  }

  submitForm() {
    // Submit form logic goes here
    console.log("Form submitted!");
    console.log("Goals:", this.goals);
    // You can send the data to backend or perform any other operation here
  }
  // Define a variable to keep track of the selected goal

  listofBehavior:any[]=[];
  listofGoal:any[]=[];
    finalReviewSubmit() {
      // const employeeRatings = this.behavior.map(goal => goal.employeeRating);
      // const id = this.behavior.map(goal => goal.Id);
      // const acheivements = this.behavior.map(goal => goal.Achievements);
      // const goalemployeeRatings = this.goals.map(goal => goal.employeeRating);
      // const goalid = this.goals.map(goal => goal.GoalId);
      // const goalacheivements = this.goals.map(goal => goal.Achievements);
  
      const data = [...this.goals, ...this.behavior];
  
      const empMap = new Map();
      
      // Iterate over the data array
      this.goals.forEach(item => {
          // Check if the item contains 'Id', then push it to listofBehavior
          if ('GoalId' in item) {
             this.listofGoal.push({ GoalId: item.GoalId, ManagerReview: item.ManagerReview, MDescription: item.MDescription });
          } 
          // Otherwise, if the item contains 'GoalId', push it to listofGoal
  
      });
      this.behavior.forEach(item => {
        // Check if the item contains 'Id', then push it to listofBehavior
        if ('Id' in item) {
            if (!empMap.has(item.EmpId)) {
                empMap.set(item.EmpId, { });
            }
           this.listofBehavior.push({ Id: item.Id, ManagerReview: item.ManagerReview, MDescription: item.MDescription });
        } 
        // Otherwise, if the item contains 'GoalId', push it to listofGoal
  
    });
      // Convert the map values to an array
      const result = Array.from(empMap.values()).map(({ EmpId, listofGoal, listofBehavior }) => ({
          ManagerId: this.empId,
          EmpId:this.rowemployee,
          listofGoal:this.listofGoal,
          listofBehavior:this.listofBehavior
      }));
      
      console.log(result,data);
      
      
      console.log("result-------",result);
      this.behaviorService.submitManagerReview(result).then(
        (data) => {
          console.log("finalsubmit response",data);
          // this.employeeData = data;
          this.triggerToast("success", "Manager Review Submitted.. ","success");
          //this.names = this.tableData.map((item: any) => item.Goal);
          // if (this.employeeId && this.tableData) {
          //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
          //   this.names = this.tableData.map((item: any) => item.name);
          // }
  this.showform=true;
  this.showreviewform=false;
        },
        (error) => {
          console.error('An error occurred:', error.msg);
        }
      );
    }
    validateInput(event: any) {
      const value = event.target.value;
      if (value < 1 || value > 5) {
        event.target.value = ''; // Clear input if out of range
      }
    }
    handleAlphaChar(event: any) {
      if (
        (event.charCode > 32 && event.charCode < 48) ||
        (event.charCode > 57 && event.charCode < 127)
      ) {
        event.preventDefault();
      }
    }
    onPaste(event: ClipboardEvent): void {
    event.preventDefault();
  }

  toggleDetails(goal: any) {
    console.log("slected Goals:", goal);
    if (this.selectedGoal === goal.GoalId) {
      this.selectedGoal = null;
    } else {
      this.selectedGoal = goal.GoalId;
    }
  }

  tabVisibility(tab: any) {
 if (tab == 'tab2') {
      this.tab2 = true;
      this.tab1 = false;
    }  else if (tab == 'tab2prev') {

      this.tab1 = true;
      this.tab2 = false;
    }

  }

  getBackgroundColor(rating: string): string {
    switch (rating) {
      case '1':
        return '#e74c3c'; // Unsatisfactory
      case '2':
        return '#f39c12'; // Needs improvement
      case '3':
        return '#3498db'; // Meets expectation
      case '4':
        return '#2ecc71'; // Exceeds expectation
      case '5':
        return '#9b59b6'; // Outstanding
      default:
        return '#fff'; // Default color
    }
  }
  // reviewformbutton() {
  //   this.showform = !this.showform;
  //   this.showreviewform=true;
  // }
  employeeGoals: any[] = [];

  customerfocus() {
    this.showcustomerFocus = !this.showcustomerFocus;
  }

  // showPopup: boolean = false;

  // closePopup() {
  //   this.showPopup = false;
  // }



  showPopup: boolean = false;
  showPopup2: boolean = false;
  isDragging: boolean = false;
  initialMouseX: number = 0;
  initialMouseY: number = 0;
  initialPopupX: number = 0;
  initialPopupY: number = 0;

  startDrag(event: MouseEvent) {
    this.isDragging = true;
    this.initialMouseX = event.clientX;
    this.initialMouseY = event.clientY;
    const popup = document.querySelector('.sticky-popup') as HTMLElement;
    const rect = popup.getBoundingClientRect();
    this.initialPopupX = rect.left;
    this.initialPopupY = rect.top;
    popup.classList.add('dragging');
  }

  movePopup(event: MouseEvent) {
    if (this.isDragging) {
      const popup = document.querySelector('.sticky-popup') as HTMLElement;
      const newX = this.initialPopupX + (event.clientX - this.initialMouseX);
      const newY = this.initialPopupY + (event.clientY - this.initialMouseY);
      popup.style.left = newX + 'px';
      popup.style.top = newY + 'px';
    }
  }

  endDrag() {
    this.isDragging = false;
    const popup = document.querySelector('.sticky-popup') as HTMLElement;
    popup.classList.remove('dragging');
  }

  closePopup() {
    this.showPopup = false;
  }
  closePopup2() {
    this.showPopup2 = false;
  }
  stickypop() {
    this.cusomerfocusFlag = !this.cusomerfocusFlag;
  }

  returnPage(){

    this.showform = true;
    this.showreviewform=false;
  }
  isHovered = false;
  fullMessage = 'Tasks completed by short time, given project deliveries on time.';

}
