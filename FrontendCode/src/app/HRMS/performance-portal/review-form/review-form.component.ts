import { Component, Input, NgModule, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { error } from 'console';
import { PerformancePortalService } from '../../service/performancePortal/performance-portal.service';

@Component({
  selector: 'app-review-form',
  standalone: true,
  imports: [SharedModule, ToastMessageComponent, FormsModule],
  templateUrl: './review-form.component.html',
  styleUrl: './review-form.component.scss',
  providers: [NgbModal],

})
export class ReviewFormComponent {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @Input() employeeData: any[] = [];
  cusomerfocusFlag: boolean = false;
  employeeId: any;
  empId: any;
  tableData: any[] = [];
  tasks: any[] = [];
  rows: any;
  loading: boolean = false;
  employeeDetails: any;
  isSubmitted = false; 
 getFinancialYear: any;
  constructor(private performanceService: PerformancePortalService) { }
  ngOnInit(): void {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    const fYStoredData = JSON.parse(sessionStorage.getItem('financialYearDetails') || '[]');
    this.getFinancialYear = fYStoredData
    this.getALLwithStatus();
    this.getALLBehaviors();
    // this.getAllBehavior(sessionData.EmpId);
    this.getEmpDetails()
    this.getAllReviewList();
  }

  selectedGoal: any;
  tab1: boolean = true;
  tab2: boolean = false;
  tab3: boolean = false;
  tab4: boolean = false;
  showform: boolean = false;
  showInfo1: boolean = false;
  showInfo2: boolean = false;
  showInfo3: boolean = false;
  showInfo4: boolean = false;
  showInfo5: boolean = false;
  showInfo7: boolean = false;
  goals: any[] = [];
  behavior: any[] = [];
  showcustomerFocus: boolean = false;


  getALLwithStatus() {
    const empData = {
      EmpId: this.employeeDetails[0].EmpId,
    };
    // this.isSpinner = true;
    this.performanceService.getALLGoal(empData).subscribe({
      next: (data: any) => {
        this.goals = data.filter((row: any) => row.FinalSubmit == true);
      }, error: () => {
        // this.triggerToast('Internal Server Error', 'Failed To Add Records', 'danger');
        // this.isSpinner = false;
      }
    })
  }

  getALLBehaviors() {
    const empData = {
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.performanceService.getALLBehaviors(empData).subscribe({
      next: (data: any) => {
        console.log(data);
        this.behavior = data;
      }, error: (err: any) => {

      }
    })
  }
 
  ReviewDetails: any;
  getAllReviewList() {
    const empData = {
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.performanceService.getAllReviewList(empData).subscribe({
      next:(data:any)=>{
        this.ReviewDetails = data;// this.behavior = data;
      },error:(err:any)=>{
        console.log(err);
        
      }
    })
  }

  employeeData1:any;
  getEmpDetails(){
    const empData = {
      EmpId: this.employeeDetails[0].EmpId,
      username: this.employeeDetails[0].UserName
    };
    this.performanceService.getEmployeeDetails(empData).subscribe({
      next:(data:any)=>{
        this.employeeData = data;
        // console.log("before converting date ", JSON.parse(data));
        this.employeeData.forEach((rowToUpdate: any) => {
          const item = data.find((item: any) => item.EmpId === rowToUpdate.EmpId);
          if (item) {
            // Parse and format the StartDate
            const parsedStartDate = this.parseDate(item.JoiningDate);
            // console.log(JSON.parse(item.JoiningDate));
            // Update the StartDate of  console.log(data); the current row
            rowToUpdate.JoiningDate = parsedStartDate;


          }
        });
      },error:(err:any)=>{

      }
    })
  }

  submitForm() {
    // Submit form logic goes here
    // console.log("Form submitted!");
    // console.log("Goals:", this.goals);
    // You can send the data to backend or perform any other operation here
  }
  // Define a variable to keep track of the selected goal
  listofBehavior: any[] = [];
  listofGoal: any[] = [];
  finalReviewSubmit() {
    const empMap = new Map();
    this.goals.forEach(item => {
      if ('GoalId' in item) {
        this.listofGoal.push({ GoalId: item.GoalId, EmpReview: item.EmpReview, EDescription: item.EDescription });
      }
    });
    this.behavior.forEach(item => {
      if ('Id' in item) {
        if (!empMap.has(item.EmpId)) {
          empMap.set(item.EmpId, {});
        }
        this.listofBehavior.push({ Id: item.Id, EmpReview: item.EmpReview, EDescription: item.EDescription });
      }
    });
    const result = Array.from(empMap.values()).map(() => ({
      EmpId: this.empId,
      listofGoal: this.listofGoal,
      listofBehavior: this.listofBehavior
    }));
    console.log(result);
    console.log("result-------", result);

    this.performanceService.submitEmpReview(result).subscribe({
      next:(data:any)=>{
        console.log("finalsubmit response", data);
        this.showform = false;
        this.isSubmitted = true;
        this.toastMessageComponent.showToast("success", "Review Submitted Successfully", "success");
        this.listofBehavior = [];
        this.listofGoal = [];
      },error:(err:any)=>{
        this.listofBehavior = [];
        this.listofGoal = [];
        this.toastMessageComponent.showToast("error", "Something Went Wrong", "danger");
      }
    })

  }
  validateInput(event: any) {
    const value = event.target.value;
    if (value < 1 || value > 5) {
      event.target.value = ''; // Clear input if out of range
    }
  }
  validateWeightage(row: any) {
    if (row.Weightage < 0) {
      row.Weightage = 0; // Reset to minimum
    } else if (row.Weightage >= 5) {
      row.Weightage = 5; // Reset to maximum
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
    if (tab == 'tab1') {
      this.tab2 = true;
      this.tab1 = false;
    } else if (tab == 'tab2prev') {
      this.tab2 = false;
      this.tab1 = true;
    } else if (tab == 'tab2next') {
      this.tab3 = true;
      this.tab1 = false;
      this.tab2 = false;

    } else if (tab == 'tab3prev') {
      this.tab2 = true;
      this.tab1 = false;
      this.tab3 = false;
    } else if (tab == 'tab3next') {
      this.tab2 = false;
      this.tab1 = false;
      this.tab3 = false;
      this.tab4 = true;
    } else if (tab == 'tab4prev') {
      this.tab2 = false;
      this.tab1 = false;
      this.tab3 = true;
      this.tab4 = false;
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
  reviewformbutton() {
    this.showform = !this.showform;
  }
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
  triggerToast() {
    if (sessionStorage.getItem('loginstatus') === 'true') {

      this.toastMessageComponent.showToast("success", "Logged In Successfully", "success");
      sessionStorage.setItem('loginstatus', 'false');
    }
  }
  parseDate(dateString: string): string {
    if (!dateString) {
      return ''; // Handle case where dateString is empty or null
    }

    const timestamp = parseInt(dateString.replace(/[^0-9]/g, ''));
    const date = new Date(timestamp);
    return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(parseInt(dateString.substr(6)));
    return date.toLocaleDateString(); // Adjust format as needed
  }

}
