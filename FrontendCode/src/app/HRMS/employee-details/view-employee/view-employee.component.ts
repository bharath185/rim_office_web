import { Component, OnInit, ViewChild } from '@angular/core';
import { HrmsServiceService } from '../../hrms-service.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { EmployeeModuleService } from '../../service/employee.service';
import { Router } from '@angular/router';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [SharedModule, CommonModule, NgxPaginationModule, ToastMessageComponent],
  templateUrl: './view-employee.component.html',
  styleUrl: './view-employee.component.scss'
})
export class ViewEmployeeComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  constructor(private hrmsService: EmployeeModuleService, private router: Router,
    private fb: FormBuilder
  ) {
    const storedEmployeeData = sessionStorage.getItem('userdata');
    this.userData = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    console.log(this.userData);

    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    console.log('Employee Details', this.employeeDetails);
  }
  userData;
  rows: any[] = [];
  originalRows: any;
  errorMessage: any;
  isSpinner: boolean = false;
  viewdata: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20, 25];
  viewData: any;
  deleteForm: any = FormGroup;
  employeeDetails;
  isActive = false;

  ngOnInit(): void {
    this.getAllEmployeeList();
    this.deleteForm = this.fb.group({
      reason: ['', []]
    })
  }

  getAllEmployeeList() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.hrmsService.employeeGetAllEmployee(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        setTimeout(() => {
          this.rows = res.map((employee: any) => ({
            ...employee,
            // isActive: employee.IsActive
          }));
          this.originalRows = this.rows;
          this.isSpinner = false;
        }, 1000);
      } else {
        this.errorMessage = "No records found";
        this.isSpinner = false;
      }
    }, error => {
      this.errorMessage = "Internal Server Error";
      this.isSpinner = false;
    })
  }
  // getAllEmployeeList(): void {
  //   const reqBody = {
  //     LoginId: this.employeeDetails[0].LoginId
  //   };
  //   this.isSpinner = true;
  //   this.hrmsService.employeeGetAllEmployee(reqBody).subscribe((res: any) => {
  // if (res.length >= 1) {
  //   setTimeout(() => {
  //     this.rows = res.map((employee: any) => ({
  //       ...employee,
  //       isActive: employee.IsActive
  //     }));
  //     this.originalRows = this.rows;
  //     this.isSpinner = false;
  //   }, 1000);
  // } else {
  //   this.errorMessage = "No records found";
  //   this.isSpinner = false;
  // }
  //   }, error => {
  //     this.errorMessage = "Internal Server Error";
  //     this.triggerToast('Internal Server Error','To load the All Employee Details','danger')
  //     this.isSpinner = false;
  //   });
  // }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value.trim().toUpperCase();
    if (filterValue) {
      this.rows = this.rows.filter((row: any) =>
        Object.values(row).some(val =>
          String(val).toUpperCase().includes(filterValue)
        )
      );
    } else {
      this.rows = [...this.originalRows];
      this.rows = this.rows
    }
    if (this.rows.length === 0) {
      this.errorMessage = 'No Records Found for Searched Data';
    } else {
      this.errorMessage = null;
    }
  }

  onFocus(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  onBlur(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  setFloatingLabel(selectElement: HTMLSelectElement) {
    const label = selectElement.nextElementSibling as HTMLElement;
    if (selectElement.value) {
      label.classList.add('floating');
    } else {
      label.classList.remove('floating');
    }
  }

  onViewEdit(data: any) {
    console.log(data);
    this.router.navigate(['update_employee'], {
      queryParams: { EmpId: data.EmpId }
    });
  }

  onViewDelete(data: any) {
    console.log(data);
    this.viewData = data
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
  deleteEmployee() {
    console.log(this.viewdata);
    this.isSpinner = true;
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.viewData.EmpId,
      Reason: this.deleteForm?.get('reason').value,
    }
    this.hrmsService.employeeDeleteEmployee(reqBody).subscribe((res: any) => {
      console.log(res);
      if (res['msg'] === "Deleted") {
        this.isSpinner = false;
        this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
        this.getAllEmployeeList();
      } else if (res['Message']) {
        this.triggerToast(res['Message'], '', 'warning');
        this.isSpinner = false;
      }
      else {
        this.triggerToast(res['msg'], 'Something went wrong', 'warning');
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast(error, 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }

  // toggleState(row: any): void {
  //   console.log(row);
  //   row.isActive = !row.isActive;
  //   console.log(row.isActive);

    // this.isSpinner = true;
    // const reqBody = {
    //   LoginId: this.employeeDetails[0].LoginId,
    //   EmpId: row.EmpId,
    //   Reason: "Performenec Issue",
    // };
    // if (row.isActive) {
    //   this.hrmsService.employeeActiveEmployee(reqBody).subscribe((res: any) => {
    //     console.log(res);
    //     if (res['msg'] === 'Actived') {
    //       this.isSpinner = false;
    //       this.triggerToast('Activated', 'Employee Activated Successfully', 'success');
    //     } else {
    //       this.triggerToast(res['msg'], res['msg'], 'warning');
    //     }
    //   }, error => {
    //     this.isSpinner = false;
    //     this.triggerToast('Error', 'Activation Failed', 'error');
    //   });
    // } else {
    //   this.hrmsService.employeeDeActiveEmployee(reqBody).subscribe((res: any) => {
    //     console.log(res);
    //     if (res['msg'] === 'Deactived') {
    //       this.isSpinner = false;
    //       this.triggerToast('Deactivated', 'Employee Deactivated Successfully', 'success');
    //     } else {
    //       this.triggerToast(res['msg'], res['msg'], 'warning');
    //     }
    //   }, error => {
    //     this.isSpinner = false;
    //     this.triggerToast('Error', 'Deactivation Failed', 'error');
    //   });
    // }
  // }

  toggleState(row: any): void {
    console.log(row);
    // Toggle the EmpStatus between 'Active' and 'Inactive'
    row.EmpStatus = row.EmpStatus === 'Active' ? 'Inactive' : 'Active';
    console.log(row.EmpStatus);
    
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: row.EmpId,
      Reason: "Performenec Issue",
    };
    this.isSpinner = true;
    if (row.EmpStatus === 'Active') {
      this.hrmsService.employeeActiveEmployee(reqBody).subscribe((res: any) => {
        console.log(res);
        if (res['msg'] === 'Actived') {
          this.isSpinner = false;
          this.triggerToast('Activated', 'Employee Activated Successfully', 'success');
        } else {
          this.triggerToast(res['msg'], res['msg'], 'warning');
        }
      }, error => {
        this.isSpinner = false;
        this.triggerToast('Error', 'Activation Failed', 'error');
      });
    } else if(row.EmpStatus === 'Inactive'){
      this.hrmsService.employeeDeActiveEmployee(reqBody).subscribe((res: any) => {
        console.log(res);
        if (res['msg'] === 'Deactived') {
          this.isSpinner = false;
          this.triggerToast('Deactivated', 'Employee Deactivated Successfully', 'success');
        } else {
          this.triggerToast(res['msg'], res['msg'], 'warning');
        }
      }, error => {
        this.isSpinner = false;
        this.triggerToast('Error', 'Deactivation Failed', 'error');
      });
    }
  }

}
