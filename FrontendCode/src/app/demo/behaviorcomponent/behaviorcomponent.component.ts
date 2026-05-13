import { Component, OnInit,ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { BehaviorService } from './behavior.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-behaviorcomponent',
  standalone: true,
  imports: [SharedModule,ToastMessageComponent,FormsModule],
  templateUrl: './behaviorcomponent.component.html',
  styleUrl: './behaviorcomponent.component.scss'
})
export class BehaviorcomponentComponent implements OnInit{
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  rows: any[] = [];
 // weightagetotal: any[]=[];
  isEditing: boolean = false;
  editingRowIndex: number = -1;
  errormessage: any;
  showWarning: boolean = false;
  maxWeightage:any=20;
Weightage:any=0;
employeeId: any;
  empId: any;
  loading: boolean = false;
  public isButtonDisabled: boolean = false;
  constructor(private behaviorService: BehaviorService) {}
  ngOnInit(): void {
    this.employeeId=sessionStorage.getItem('userDetail');
    const sessionData = JSON.parse(this.employeeId);
    this.empId=sessionData.EmpId;
    this.getAllBehavior(sessionData.EmpId);
  }


 dismiss() {
    this.showWarning = false;
  }
  addRow() {
    const sessionData = JSON.parse(this.employeeId);

    const employeeId = sessionData.EmpId;

    if (!this.isEditing) {
      this.rows.push({ Id:'',QId:0,PeriodId: 0,Behaviour: '', Weightage: '', Discription:'', status: 'pending', EmpId: employeeId, isEditing: true });

      this.isEditing = true;
      
      this.editingRowIndex = this.rows.length - 1;
    }

  }

  async removeRow(index: number,row: any) {
    this.loading = true;
    if(!this.isEditing){
      if (row.Id !=0 || row.Id !='') {
        
        const empData = {
          // EmpId:  this.empId,
          Id:row.Id

        };
        const data = await this.behaviorService.deleteBehavior(empData);
        
        if(data.msg == 'Deleted'){
          console.log(`Row ${index} updated:`, data);
          this.rows.splice(index, 1);
          this.triggerToast(data.msg, "Deleted  Successfully","danger");
          this.loading = false;
          this.getAllBehavior(this.empId);
        }
       
      } else {
        // Create API call for other rows
      //  const data = await this.behaviorService.addBehavior([row]);
      //  console.log(`Row ${index} created:`, data);
        this.rows.splice(index, 1);
      }
     
    }
   
  }
  cancelEdit(row: any) {
    if (row.Id != 0 || row.Id != '') {
      row.isEditing = false;
      this.isEditing = false;
      // this.isButtonDisabled = false;
      this.editingRowIndex = -1;
      this.getAllBehavior(this.empId);
    }else{

      const index = this.rows.indexOf(row);
      if (index !== -1) {
          this.rows.splice(index, 1);
          // this.isButtonDisabled = false;
          this.isEditing = false;
      }
    }
}
  async toggleEdit(row: any, index: number) {
  
    if (this.editingRowIndex === index) {
      if(row.Behaviour.trim() === '' && row.Weightage.trim() === ''){
       

          this.triggerToast("warning", "Cannot save an empty row.","danger");
          return;
        
      }else{
        row.isEditing = !row.isEditing;
        this.errormessage = '';

      this.isEditing = this.rows.some(row => row.isEditing);

      if (!row.isEditing) {
        this.editingRowIndex = -1;
      }
      const numbers = this.rows.map(row => parseFloat(row.Weightage));
      const sum = numbers.reduce((acc, curr) => acc + curr, 0);
      this.Weightage=sum;
      if(this.Weightage > this.maxWeightage){
     //   this.getAllBehavior(this.empId);
        // this.showWarning = true;
        // setTimeout(() => {
        //   this.showWarning = false;
        // }, 2000);
        this.triggerToast("warning", "Weightage exeeded it won't be more than 20 %","danger");
        // this.errormessage = "";
        console.log("sum",sum,this.maxWeightage,this.Weightage);
        row.isEditing = true;
    }else{
      if (row.Id !=0 || row.Id !='') {
        // Update API call for row with Id: 5
        if (row.Weightage && row.Weightage % 1 !== 0) {
          this.triggerToast('Warning', 'Decimal or Text values wont be Allowed Please check', "danger");
          row.isEditing = true;
         
        } else {
          this.loading = true;
        const data = await this.behaviorService.updateBehavior(row);
        
        if(data.msg =='Updated'){
          console.log(`Row ${index} updated:`, data);
          this.triggerToast(data.msg, "Updated  Successfully","success");
          this.loading = false;
          this.getAllBehavior(this.empId);
        }
      }
     
      } else {
        // Create API call for other rows
        if (row.Weightage && row.Weightage % 1 !== 0) {
          this.triggerToast('Warning', 'Decimal or Text values wont be Allowed Please check', "danger");
          row.isEditing = true;
         
        } else {
          this.loading = true;
         // this.errormessage = "Weightage exeeded it won't be more than 20 %";
          const data = await this.behaviorService.addBehavior([row]);
          if(data.msg =='Added'){
            console.log(`Row ${index} created:`, data);
            this.triggerToast(data.msg, "Added  Successfully","success");
            this.loading = false;
            this.getAllBehavior(this.empId);
          }
        }
       
        
  
      }
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

  saveRow(row: any) {
    row.isEditing = false;
    this.isEditing = this.rows.some(row => row.isEditing);
    this.editingRowIndex = -1;

  }
 

  getAllBehavior(empDetails:any){
    this.loading = true;
    console.log('Form Data:', JSON.stringify(empDetails));
    const empData = {
      EmpId: empDetails
    };
  
    this.behaviorService.getALLBehaviors(empData).then(
      (data) => {
        console.log(data);
          this.rows=data;
          this.loading = false;
          this.Weightage=data.reduce((sum:any, item:any) => {
            return sum + parseFloat(item.Weightage);
        }, 0);
          //this.names = this.tableData.map((item: any) => item.Goal);
          // if (this.employeeId && this.tableData) {
          //   this.tableData = this.tableData.filter((item :any)=> item.employeeid === empDetails.EmpId);
          //   this.names = this.tableData.map((item: any) => item.name);
          // }
          
      },
      (error) => {
        console.error('An error occurred:', error.msg);
        this.loading = false;
      }
    );

  }
  triggerToast(header: any, body: any,mess:any) {
    this.toastMessageComponent.showToast(header, body,mess);
  }
}
