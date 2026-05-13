import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccessPolicyRoutingModule } from "../../access-policy/access-policy-routing.module";
import { leavesService } from '../../service/leaves.service';


@Component({
  selector: 'app-view-compoff-req',
  standalone: true,
  imports: [ToastMessageComponent, SharedModule, CommonModule, ReactiveFormsModule, NgxPaginationModule, AccessPolicyRoutingModule],
  templateUrl: './view-compoff-req.component.html',
  styleUrl: './view-compoff-req.component.scss'
})
export class ViewCompoffReqComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalApprove') closeModalApprove!: ElementRef;
  @ViewChild('closeModalReject') closeModalReject!: ElementRef;

  isSpinner: boolean = false;
  employeeDetails;
  isTableDataCompOff: boolean = false;
  rows: any = [];
  errorMessageCompOff: any;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 50, 100, 500];
  selectAllHR: boolean = false;
  selectedRowsHR: any[] = [];
  isRecordDeleted: boolean = false;
  approvalRemarks: string = '';
  rejectRemarks: string = '';
  showStatusDropdown = false;
  filterStatus: string = '';
  originalRows: any;
  isTableDataEmpty: any;
  filterApprovedBy: string = '';
  filterDateRange: string = '';
  statusOptions: string[] = [
    'APPROVED',
    'REJECTED',
    'APPLIED',
  ];

  constructor(private leaveSerive: leavesService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
  }
  ngOnInit(): void {
    this.getAllCompOffRequest();
  }

  formatJsonDate(jsonDate: string | null | undefined): string {
    if (!jsonDate || typeof jsonDate !== 'string') {
      return '';
    }
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (!match) {
      return '';
    }
    const timestamp = +match[1];
    const date = new Date(timestamp);
    return `${date.getDate().toString().padStart(2, '0')}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getFullYear()}`;
  }

  toggleAllSelectionHR() {
    this.rows.forEach((row: any) => row.selected = this.selectAllHR);
    this.updateSelectedRows();
  }
  updateSelection() {
    this.selectAllHR = this.rows.every((row: any) => row.selected);
    this.updateSelectedRows();
  }
  updateSelectedRows() {
    this.selectedRowsHR = this.rows.filter((row: any) => row.selected);
  }
  getAllCompOffRequest() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
    }
    this.isSpinner = true;
    this.leaveSerive.GetAllCompOffLeave(reqBody).subscribe({
      next: (res: any) => {
        if (res.length > 0) {
          this.rows = res;
          this.originalRows = res;
          this.isTableDataCompOff = false;
          this.isSpinner = false;
        } else {
          this.errorMessageCompOff = 'No records found';
          this.isSpinner = false;
          this.isTableDataCompOff = true;
        }
      }, error: (err: any) => {
        this.errorMessageCompOff = 'Internal Server Error';
        this.isSpinner = false;
        this.isTableDataCompOff = true;
      }
    })
  }

  toggleStatusDropdown() {
    this.showStatusDropdown = !this.showStatusDropdown;
  }

  setFilterStatus(value: string) {
    this.filterStatus = value;
    this.showStatusDropdown = false;
    this.onFilterChange();
  }

  onFilterChange() {
    const approvedBy = this.filterApprovedBy;
    const status = this.filterStatus;
    const daysRange = this.filterDateRange;
    const now = new Date();
    this.rows = this.originalRows.filter((row: any) => {
      let isMatch = true;
      // Dynamically determine status
      const derivedStatus = row.IsApproved
        ? 'APPROVED'
        : row.IsRejected
          ? 'REJECTED'
          : row.IsRequested
            ? 'APPLIED'
            : 'UNKNOWN';

      if (approvedBy && row.Approver !== approvedBy) {
        isMatch = false;
      }
      if (status && derivedStatus !== status) {
        isMatch = false;
      }
      if (daysRange) {
        const days = parseInt(daysRange, 10);
        const cutoffDate = new Date(now);
        cutoffDate.setDate(now.getDate() - days);
        // Assuming you're using `CreatedDate` or `Date` to filter by date range
        const dateString = this.formatJsonDate(row.Date); // Format to DD-MM-YYYY
        const [day, month, year] = dateString.split('-').map(Number);
        const leaveDate = new Date(year, month - 1, day);
        if (leaveDate < cutoffDate) {
          isMatch = false;
        }
      }
      return isMatch;
    });
    this.isTableDataEmpty = this.rows.length === 0;
    this.errorMessageCompOff = this.isTableDataEmpty ? 'No Data Found' : '';
  }

  approveSelected() {
    if (this.selectedRowsHR.length === 0) {
      this.triggerToast('Please select at least one record.', 'No Selection', 'warning');
      return;
    }
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      lstofCompOffReqId: this.selectedRowsHR.map(row => ({
        CompOffReqId: row.CompOffReqId,
        Remarks: this.approvalRemarks || '',
      }))
    };
    console.log(reqBody);
    this.isSpinner = true;
    this.leaveSerive.ApproveCompOff(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Approved Successfully', 'success');
          this.isRecordDeleted = true;
          setTimeout(() => {
            this.closeModalApprove.nativeElement?.click();
            this.selectAllHR = false;
            this.getAllCompOffRequest();
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
          this.isSpinner = false;
        } else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.isSpinner = false;
        }

      },
      error: (err: any) => {
        this.triggerToast(err['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
      },
      complete: () => {
        this.isSpinner = false;
      }
    });
  }

  rejectSelected() {
    if (this.selectedRowsHR.length === 0) {
      this.triggerToast('Please select at least one record.', 'No Selection', 'warning');
      return;
    }
    if (!this.rejectRemarks || this.rejectRemarks.trim() === '') {
      this.triggerToast('Remarks are required to reject the records.', 'Missing Remarks', 'warning');
      return;
    }
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      lstofCompOffReqId: this.selectedRowsHR.map(row => ({
        CompOffReqId: row.CompOffReqId,
        Remarks: this.rejectRemarks || '',
      }))
    };
    this.isSpinner = true;
    this.leaveSerive.RejectCompOff(reqBody).subscribe({
      next: (res: any) => {
        if (res['msg']) {
          this.triggerToast(res['msg'], 'Rejected Successfully', 'success');
          this.isRecordDeleted = true;
          setTimeout(() => {
            this.closeModalReject.nativeElement?.click();
            this.getAllCompOffRequest();
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
          this.isSpinner = false;
        }
        else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.isSpinner = false;
        }
      },
      error: (err: any) => {
        this.triggerToast(err['Message'], 'Internal Server Error', 'danger');
        this.isSpinner = false;
      },
      complete: () => {
        this.isSpinner = false;
      }
    });
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
