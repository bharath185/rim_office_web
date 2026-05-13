import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { RouterModule } from '@angular/router';
import { EmployeeModuleService } from '../../service/employee.service';
import { NomineeDetailsComponent } from '../nominee-details/nominee-details.component';
import { InsuranceDetailsComponent } from '../insurance-details/insurance-details.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { payRollService } from '../../service/payroll.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { Modal } from 'bootstrap';


@Component({
  selector: 'app-emp-financial-details',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent,
    NgxPaginationModule, RouterModule],
  templateUrl: './emp-financial-details.component.html',
  styleUrl: './emp-financial-details.component.scss'
})
export class EmpFinancialDetailsComponent implements OnInit {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;

  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;
  isSpinner: boolean = false;
  isSpinner1: boolean = false;
  accountForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isEdited: boolean = false;
  errorMessage: any;
  isTableData: boolean = false;
  getAllEmpAccList: any = [];
  originalAllEmpAccList: any = [];
  searchValue: string = '';
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  isRecordDeletedCommon: boolean = false;
  selectedRowForDelete: any;
  patchEmpAccDetails: any;
  isCardOpen = false;
  constructor(private readonly fb: FormBuilder,
    private payrollService: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly hrmsService: HrmsServiceService,) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Account Details'
      );
    });
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.getEmployeeSelectEmployee()
      setTimeout(() => {
        this.getAllEmpAccDetails();
      }, 200);
    }, 200);
    this.accountForm = this.fb.group({
      emloyee: ['', [Validators.required]],
      BankName: ['', [Validators.required]],
      IFSCCode: ['', [Validators.required, Validators.pattern('^[A-Za-z0-9]{10,16}$')]],
      BranchName: ['', [Validators.required]],
      AccHolderName: ['', [Validators.required, Validators.pattern("^[a-zA-Z' ]*$"), Validators.minLength(2), Validators.maxLength(50)]],
      AccNo: ['', [Validators.required]],
      Acc_MobileNo: ['', [Validators.required, Validators.pattern('^[6-9][0-9]{9}$')]],
      ESIInsuranceNo: [''],
      HealthInsuranceNo: [''],
      PANNo: [''],
      UANNo: [''],
      AadharNo: [''],
    });
  }
  selectedTab = 0;
  selectTab(index: number) {
    this.selectedTab = index;
  }
  tabs = [
    { label: 'Account Details', icon: 'feather icon-credit-card' },
    { label: 'Nominee Details', icon: 'feather icon-users' },
    { label: 'Insurance Details', icon: 'feather icon-shield' },
  ];
  toggleButton() {
    this.isCardOpen = !this.isCardOpen
  }
  convertToUppercase(event: any, controlName: string) {
    const value = event.target.value.toUpperCase();
    this.accountForm.get(controlName)?.setValue(value, { emitEvent: false });
  }

  //this is Employee list
  employees: any[] = [];
  errorMessageEmpName: any;
  searchText: string = '';
  filteredEmployees: any[] = [];
  selectedEmployee: any = null;
  isDropdownOpen = false;
  isValidEmployee: boolean = true;

  getEmployeeSelectEmployee() {
    const reqBody = { EmpId: this.employeeDetails[0].EmpId };
    this.isSpinner = true;
    this.hrmsService.visitorAccessDDEmployee(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.employees = res;
          this.errorMessageEmpName = ''
        } else {
          this.triggerToast(res['Message'], "Sorry No Data Found", "warning");
          this.errorMessageEmpName = 'No Data Found.'
        }
        this.isSpinner = false;
      },
      error: (error: any) => {
        this.errorMessageEmpName = 'Error loading data. Please try again.';
        this.isSpinner = false;
      }
    });
  }
  filterEmployees() {
    if (this.searchText) {
      this.filteredEmployees = this.employees.filter((employee: any) =>
        employee.EmpName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        employee.EmpCode.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredEmployees = [...this.employees];
    }
  }
  selectEmployeee(employee: any) {
    this.searchText = employee.EmpName;
    this.selectedEmployee = employee.EmpId;
    this.isDropdownOpen = false;
    this.isValidEmployee = true;
  }
 
  checkValidEmployee() {
    const isMatch = this.employees.some(employee =>
      employee.EmpName.toLowerCase() === this.searchText?.toLowerCase()
    );
    this.isValidEmployee = isMatch;
    if (!isMatch) {
      this.accountForm.get('emloyee')?.setErrors({ invalidEmployee: true });
    } else {
      this.accountForm.get('emloyee')?.setErrors(null);
    }
  }
  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  openDropdown() {
    this.isDropdownOpen = true;
    this.filteredEmployees = [...this.employees];
  }
  closeDropdown() {
    setTimeout(() => {
      this.isDropdownOpen = false;
    }, 200);
  }
  //this is Employee list
  getAllEmpAccDetails() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.payrollService.GetAllEmpAccDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getAllEmpAccList = res;
          this.originalAllEmpAccList = res;
          this.isSpinner = false;
          this.errorMessage = '';
          this.isTableData = false;
        } else {
          this.isSpinner = false;
          this.errorMessage = 'No Data Found';
          this.isTableData = true;
        }
      }, error: (err: any) => {
        this.isSpinner = false;
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
      }
    })
  }
  applyFilter() {
    const val = this.searchValue.toLowerCase().trim();
    this.getAllEmpAccList = this.originalAllEmpAccList.filter((row: any) => {
      return (
        row.AccHolderName?.toLowerCase().includes(val) ||
        row.BankName?.toLowerCase().includes(val) ||
        row.BranchName?.toString().includes(val) ||
        row.AccNo?.toLowerCase().includes(val) ||
        row.IFSCCode?.toLowerCase().includes(val) ||
        row.MobileNo?.toLowerCase().includes(val) ||
        row.PANNo?.toLowerCase().includes(val) ||
        row.UANNo?.toLowerCase().includes(val) ||
        row.PFNo?.toLowerCase().includes(val)
      );
    });
    // If no records found
    if (this.getAllEmpAccList.length === 0) {
      this.isTableData = true;
      this.errorMessage = `No record found for "${this.searchValue}"`;
    } else {
      this.isTableData = false;
      this.errorMessage = '';
    }
    this.page = 1;
  }

  patchVlaues(data: any, edited: boolean) {
    this.isEdited = edited;
    this.patchEmpAccDetails = data;

    this.getEmployeeSelectEmployee();

    this.accountForm.patchValue({
      BankName: data.BankName,
      IFSCCode: data.IFSCCode,
      BranchName: data.BranchName,
      AccHolderName: data.AccHolderName,
      AccNo: data.AccNo, // ✅ FIXED (was wrong earlier)
      Acc_MobileNo: data.MobileNo,
      ESIInsuranceNo: data.ESIInsuranceNo,
      HealthInsuranceNo: data.HealthInsuranceNo,
      PANNo: data.PANNo,
      UANNo: data.UANNo,
      AadharNo: data.AadharNo,
      emloyee: data.EmpId // store ID
    });

    // ✅ THIS shows name in UI
    this.searchText = data.EmpName?.trim();

    // ✅ THIS keeps ID separately
    this.selectedEmployee = data.EmpId;

    this.isCardOpen = true;
  }
  updateAddForm() {
    const reqBody = {
      AccId: this.patchEmpAccDetails.AccId,
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.selectedEmployee,
      BankName: this.accountForm.get('BankName')?.value,
      IFSCCode: this.accountForm.get('IFSCCode')?.value,
      BranchName: this.accountForm.get('BranchName')?.value,
      AccHolderName: this.accountForm.get('AccHolderName')?.value,
      AccNo: this.accountForm.get('AccNo')?.value,
      MobileNo: this.accountForm.get('Acc_MobileNo')?.value,
      ESIInsuranceNo: this.accountForm.get('ESIInsuranceNo')?.value,
      HealthInsuranceNo: this.accountForm.get('HealthInsuranceNo')?.value,
      PANNo: this.accountForm.get('PANNo')?.value,
      UANNo: this.accountForm.get('UANNo')?.value,
      AadharNo: this.accountForm.get('AadharNo')?.value,
    };

    this.isSpinner = true;

    this.payrollService.UpdateEmpAccDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast('', res['msg'], '');
          this.getAllEmpAccDetails();
          this.resetData();
        } else if (res['Message']) {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
        }
        this.isSpinner = false;
      },
      error: () => {
        this.triggerToast('', 'Internal Server Error', 'danger');
        this.isSpinner = false;
      }
    });
  }

  confirmDelete(data: any) {
    console.log(data);

    const isConfirmed = window.confirm('Are you sure you want to delete this record?');

    if (isConfirmed) {
      this.selectedRowForDelete = data;
      this.deleteRecord();
    } else {
      console.log('Delete cancelled');
    }
  }

  deleteRecord() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      AccId: this.selectedRowForDelete.AccId,
      EmpId: this.selectedRowForDelete.EmpId,
    }
    this.isSpinner1 = true;
    this.payrollService.DeleteEmpAccDetails(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], 'success');
          this.isRecordDeletedCommon = true;
          setTimeout(() => {
            this.closeModalDelete.nativeElement?.click();
            this.getAllEmpAccDetails();
            setTimeout(() => {
              this.isRecordDeletedCommon = false;
            }, 1100);
          }, 1000);
        } else if (res['Message']) {
          this.triggerToast(res['Message'], res['Message'], 'warning');
        }
        this.isSpinner1 = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'Delete Failed', 'danger');
        this.isSpinner1 = false;
      }
    })
  }

  submitAccountDetails() {
    if (this.accountForm.valid) {
      const reqBody = {
        // LoginId: this.employeeDetails[0].LoginId,
       EmpId: this.selectedEmployee,
        LoginId: this.employeeDetails[0].LoginId ? this.employeeDetails[0].LoginId : '',
        BankName: this.accountForm?.get('BankName').value,
        IFSCCode: this.accountForm?.get('IFSCCode').value,
        BranchName: this.accountForm?.get('BranchName').value,
        AccHolderName: this.accountForm?.get('AccHolderName').value,
        AccNo: this.accountForm?.get('AccNo').value,
        MobileNo: this.accountForm?.get('Acc_MobileNo').value,
        ESIInsuranceNo: this.accountForm?.get('ESIInsuranceNo').value,
        HealthInsuranceNo: this.accountForm?.get('HealthInsuranceNo').value,
        PANNo: this.accountForm?.get('PANNo').value,
        UANNo: this.accountForm?.get('UANNo').value,
        AadharNo: this.accountForm?.get('AadharNo').value
      }
      this.isSpinner = true;
      this.payrollService.employeeAddEmpAccDetails(reqBody).subscribe((res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], "Data Added Successfully", "success");
          this.getAllEmpAccDetails();
          this.resetData();
          this.isSpinner = false;
          this.isFormSubmitted = false;
        } else {
          this.triggerToast(res['Message'], "Something went wrong", "warning");
          this.isSpinner = false;
        }
      }, error => {
        this.triggerToast('', 'Internal Server Error', 'danger');
        this.isSpinner = false;
      })
    } else {
      this.isFormSubmitted = true;
    }
  }

  resetData() {
    this.isFormSubmitted = false;
    this.accountForm.reset();
    this.isEdited = false
  }

  handleAlphaChar(event: any) {
    if (
      (event.charCode > 32 && event.charCode < 48) ||
      (event.charCode > 57 && event.charCode < 127)
    ) {
      event.preventDefault();
    }
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


}
