import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { payRollService } from '../../service/payroll.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-payslip-section',
  standalone: true,
  imports: [SharedModule, CommonModule, ToastMessageComponent,
    ReactiveFormsModule, NgxPaginationModule],
  templateUrl: './payslip-section.component.html',
  styleUrl: './payslip-section.component.scss'
})
export class PayslipSectionComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;

  employeeDetails;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  isFormSubmittedAddForm: boolean = false;
  accessPolicy: any;
  controlAccessPage: any
  payslipSectionForm: any = FormGroup;
  addForm: any = FormGroup;
  getDDOfPayoutType: any = [];
  getDDOfPayslipSection: any = [];
  isSpinner1: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  isTableData: boolean = false;
  errorMessage: any;
  getListOfPayslipSection: any[] = [];
  isEdited: any;
  payslipSectionPatchData: any;
  recordToDelete: any;
  isRecordDeletedCommon: boolean = false;
  getDDOfComponent: any = [];

  constructor(private payrollService: payRollService,
    private accessPolicyStoreService: AccessPolicyStoreService,
    private readonly fb: FormBuilder) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Financial Details'
      );
    });
  }
  ngOnInit(): void {
    this.payslipSectionForm = this.fb.group({
      PayoutType: ['', [Validators.required]],
      payslipSection: ['', [Validators.required]],
      components: this.fb.array([])
    });
    this.addForm = this.fb.group({
      SectionName: ['', [Validators.required]],
    })
    this.dropdownPayoutType();
    setTimeout(() => {
      this.gerDDPayslipSection();
      this.getDDPayrollComponent();
    }, 100);
  }
  get componentsFormArray(): FormArray {
    return this.payslipSectionForm.get('components') as FormArray;
  }
  createComponentRow(seq: number): FormGroup {
    return this.fb.group({
      componentDD: ['', Validators.required],
      SequenceNo: [{ value: seq, disabled: true }, Validators.required]  // readonly
    });
  }
  addComponentRow() {
    if (this.componentsFormArray.length >= 10) {
      alert("Maximum 10 components allowed.");
      return;
    }
    const seq = this.componentsFormArray.length + 1;  // auto-increment
    this.componentsFormArray.push(this.createComponentRow(seq));
  }
  removeRow(i: number) {
    this.componentsFormArray.removeAt(i);
    this.componentsFormArray.controls.forEach((ctrl, index) => {
      ctrl.get('SequenceNo')?.setValue(index + 1);
    });
  }
  dropdownPayoutType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.DDPayrollPayoutType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfPayoutType = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Payout Type", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('PayoutType', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }
  gerDDPayslipSection() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.DDPayslipSection(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfPayslipSection = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Payout Type", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('PayslipSection', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }
  getDDPayrollComponent() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner = true;
    this.payrollService.DDPayrollComponent(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDDOfComponent = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Payout Type", "warning");
        this.isSpinner = false;
      }
    }, error => {
      this.triggerToast('Component Value', 'Internal Server Error', 'danger');
      this.isSpinner = false;
    })
  }

  isComponentDisabled(component: any, rowIndex: number): boolean {
    const selectedValues = this.componentsFormArray.controls
      .map((ctrl, idx) =>
        idx !== rowIndex ? ctrl.get('componentDD')?.value?.ComponentId : null
      );
    return selectedValues.includes(component.ComponentId);
  }

  resetData() {
    this.payslipSectionForm.reset();
    this.isFormSubmitted = false;
    this.componentsFormArray.clear();
  }

  submit() {
    this.isFormSubmitted = true;
    this.payslipSectionForm.markAllAsTouched();
    if (this.componentsFormArray.length === 0) {
      alert("Please add at least one component.");
      return;
    }
    if (this.componentsFormArray.invalid || this.payslipSectionForm.invalid) {
      return;
    }
    console.log(this.payslipSectionForm.value);
  }

  //*********** This is for open Modal Payslip Section************* */
  onPayslipSectionChange(event: any) {
    const value = event.target.value;
    if (value === "createNew") {
      this.getAllPayslipSection()
      event.target.value = "";
      const modalElement = document.getElementById('payslipSectionModal');
      if (modalElement) {
        const modal = new Modal(modalElement);
        modal.show();
      }
    }
  }
  closeModal() {
    const modalElement = document.getElementById('payslipSectionModal');
    if (modalElement) {
      const modal = new Modal(modalElement);
      modal.hide();
    }
  }
  submitAddForm() {
    if (this.addForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        SectionName: this.addForm?.get('SectionName').value,
        SequenceNo: 0,
      }
      this.isSpinner1 = true;
      this.payrollService.AddPayslipSection(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], "");
            this.getAllPayslipSection();
            this.closeModalReset();
          } else if (res['Message']) {
            this.triggerToast(res['Message'], "Failed To Add", "warning");
          }
          this.isSpinner1 = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'To Add Record', 'danger');
          this.isSpinner1 = false;
        }
      })
    } else {
      this.isFormSubmittedAddForm = true
    }
  }
  getAllPayslipSection() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId
    }
    this.isSpinner1 = true;
    this.payrollService.GetAllPayslipSection(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          this.getListOfPayslipSection = res;
          this.errorMessage = '';
          this.isTableData = false;
        } else {
          this.errorMessage = 'No Data Found';
          this.getListOfPayslipSection = [];
          this.isTableData = true;
        }
        this.isSpinner1 = false;
      }, error: (err: any) => {
        this.isSpinner1 = false;
        this.errorMessage = 'Internal Server Error';
        this.isTableData = true;
      }
    })
  }
  patchVlaues(data: any, edited: boolean) {
    console.log(data);
    this.isEdited = edited;
    this.payslipSectionPatchData = data;
    this.addForm.patchValue({
      SectionName: data.SectionName,
    });
  }
  updateAddForm() {
    if (this.addForm.valid) {
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        SectionId: this.payslipSectionPatchData.SectionId,
        SectionName: this.addForm?.get('SectionName').value,
        SequenceNo: this.payslipSectionPatchData.SequenceNo,
      }
      this.isSpinner1 = true;
      this.payrollService.UpdatePayslipSection(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg']) {
            this.triggerToast(res['msg'], res['msg'], "");
            this.getAllPayslipSection();
            this.closeModalReset();
          } else if (res['Message']) {
            this.triggerToast(res['Message'], "Failed To Update", "warning");
          }
          this.isSpinner1 = false;
        }, error: (err: any) => {
          this.triggerToast('Internal Server Error', 'To Update Record', 'danger');
          this.isSpinner1 = false;
        }
      })
    } else {
      this.isFormSubmittedAddForm = true
    }
  }
  confirmDelete(row: any) {
    console.log(row)
    this.recordToDelete = row;
    // this.isRecordDeleted = false;
  }
  deleteRecord() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      SectionId: this.recordToDelete.SectionId
    }
    this.isSpinner1 = true;
    this.payrollService.DeletePayslipSection(reqBody).subscribe({
      next: (res: any) => {
        console.log(res);
        if (res['msg']) {
          this.triggerToast(res['msg'], res['msg'], "");
          this.isRecordDeletedCommon = true;
          setTimeout(() => {
            this.closeModalDelete.nativeElement?.click();
            this.getAllPayslipSection();
            this.closeModalReset();
            setTimeout(() => {
              this.isRecordDeletedCommon = false;
            }, 1100);
          }, 1000);
        } else if (res['Message']) {
          this.triggerToast(res['Message'], "Failed To Update", "warning");
        }
        this.isSpinner1 = false;
      }, error: (err: any) => {
        this.triggerToast('Internal Server Error', 'To Update Record', 'danger');
        this.isSpinner1 = false;
      }
    })
  }

  closeModalReset() {
    this.addForm?.reset();
    this.isEdited = false;
    this.isFormSubmittedAddForm = false;
  }
  //*********** This is for open Modal Payslip Section************* */

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

  //  generatePayslip(event?: Event) {
  //     if (event) event.preventDefault();

  //     const reqBody = {
  //       LoginId: 1719,
  //       EmpCode: "RIM-906",
  //       Year: 2025,
  //       MonthNo: 11,
  //       Month: 'Nov',
  //     };

  //     this.isSpinner = true;

  //     this.payrollService.EmpPayslipGeneration(reqBody).subscribe({
  //       next: (res: any) => {
  //         this.isSpinner = false;

  //         if (!res || !res.EmployeeDetails) {
  //           this.triggerToast('', res['Message'] || 'No Data Found', 'warning');
  //           return;
  //         }

  //         const doc = new jsPDF('p', 'pt', 'a4');
  //         const pageWidth = doc.internal.pageSize.getWidth();
  //         const margin = 6;

  //         const logo = new Image();
  //         logo.src = './assets/Logo3DCAD.png';

  //         const generatePDF = () => {
  //           // ===== HEADER =====
  //           doc.setFillColor(245, 245, 245);
  //           doc.rect(margin, 0, pageWidth - 2 * margin, 120, 'F');
  //           try { doc.addImage(logo, 'PNG', margin + 30, 36, 120, 45); } catch { }

  //           // ===== Company Address =====
  //           const addrX = pageWidth - 30;
  //           const addrText = [
  //             res.Company.CompanyName,
  //             res.Company.CompanyAddress,
  //             `Phone : ${res.Company.CompanyPhoneNo}`,
  //             `Fax : ${res.Company.CompanyFax || '-'}`,
  //             `E-mail : ${res.Company.CompanyEmail}`
  //           ];

  //           doc.setFontSize(9);
  //           let ay = 30;

  //           addrText.forEach(line => {
  //             let wrapWidth = pageWidth - 250; // adjust width to control wrapping
  //             let wrapped = doc.splitTextToSize(line, wrapWidth);

  //             // If this is the company address and we want exactly 3 lines
  //             if (line === res.Company.CompanyAddress && wrapped.length !== 3) {
  //               const words = line.split(' ');
  //               wrapped = [];
  //               const approxWordsPerLine = Math.ceil(words.length / 3);
  //               let tempLine = '';
  //               for (let i = 0; i < words.length; i++) {
  //                 tempLine += words[i] + ' ';
  //                 if ((i + 1) % approxWordsPerLine === 0) {
  //                   wrapped.push(tempLine.trim());
  //                   tempLine = '';
  //                 }
  //               }
  //               if (tempLine) wrapped.push(tempLine.trim());

  //               // If we somehow get more than 3 lines, merge the extra words into the last line
  //               while (wrapped.length > 3) {
  //                 wrapped[2] += ' ' + wrapped.splice(3).join(' ');
  //               }
  //               // If less than 3 lines, pad with empty strings
  //               while (wrapped.length < 3) wrapped.push('');
  //             }

  //             wrapped.forEach((textLine: any) => {
  //               doc.text(textLine, addrX, ay, { align: 'right' });
  //               ay += 12.5;
  //             });

  //             if (line.startsWith('E-mail')) {
  //               const textWidth = doc.getTextWidth(line);
  //               doc.setDrawColor(0, 112, 192);
  //               doc.setLineWidth(1);
  //               doc.line(40, ay - 6, addrX - textWidth, ay - 6);
  //             }
  //           });


  //           // TITLE
  //           doc.setFont('helvetica', 'bold');
  //           doc.setFontSize(11);
  //           doc.text('Salary Slip for the month :', 40, 135);
  //           doc.setFont('helvetica', 'normal');
  //           doc.text(res.SalaryMonth, 300, 135);

  //           // ===== EMPLOYEE DETAILS =====
  //           const e = res.EmployeeDetails;
  //           const empHead = [['Name', e.Name, 'Designation', e.Designation]];
  //           const empBody = [
  //             ['EmpNo', e.EmpCode, 'Location', e.Location],
  //             ['PAN No', e.PanNo || '-', 'Bank A/c No', e.BankAccNo || '-'],
  //             ['PFNo', e.PFNo || '-', 'Days Paid', e.DaysPaid],
  //             ['UAN No', e.UANNo || '-', 'LOP', e.LOP],
  //             ['ESI No', e.ESINo || '-', '', '']
  //           ];

  //           const tableMargin = 6;
  //           const tableWidth = pageWidth - 2 * tableMargin;
  //           let empTableMinX = Infinity, empTableMaxX = -Infinity, empTableMinY = Infinity, empTableMaxY = -Infinity;

  //           autoTable(doc, {
  //             startY: 150,
  //             head: empHead,
  //             body: empBody,
  //             theme: 'plain',
  //             styles: { fontSize: 9, cellPadding: 6 },
  //             headStyles: { fillColor: [255, 255, 255], textColor: 0, fontStyle: 'bold', halign: 'right' },
  //             bodyStyles: { fillColor: [230, 230, 230], textColor: 20 },
  //             columnStyles: { 0: { halign: 'left' }, 1: { halign: 'right' }, 2: { halign: 'left' }, 3: { halign: 'right' } },
  //             margin: { left: tableMargin, right: tableMargin },
  //             didDrawCell: (data) => {
  //               const { cell, row, column } = data;
  //               if (!cell) return;

  //               // --- Track bounds ---
  //               empTableMinX = Math.min(empTableMinX, cell.x);
  //               empTableMinY = Math.min(empTableMinY, cell.y);
  //               empTableMaxX = Math.max(empTableMaxX, cell.x + cell.width);
  //               empTableMaxY = Math.max(empTableMaxY, cell.y + cell.height);

  //               // --- Draw vertical lines (existing) ---
  //               if (column.index < 3) {
  //                 doc.setDrawColor(0);
  //                 doc.setLineWidth(0.5);
  //                 doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
  //               }

  //               // ============================
  //               // ADD ONLY BOTTOM BORDER FOR HEADER ROW
  //               // ============================
  //               if (row.section === 'head') {
  //                 doc.setDrawColor(0);
  //                 doc.setLineWidth(1);
  //                 doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
  //               }
  //             },

  //             didDrawPage: () => {
  //               // Draw outer border
  //               doc.setDrawColor(0);
  //               doc.setLineWidth(1);
  //               doc.rect(empTableMinX, empTableMinY, empTableMaxX - empTableMinX, empTableMaxY - empTableMinY);
  //             }
  //           });

  //           // ===== EARNINGS & DEDUCTIONS =====
  //           const startY = empTableMaxY; // Merge first table bottom with second table top
  //           const earnings = res.PayslipSections.find((s: any) => s.SectionName === 'EARNINGS')?.Components || [];
  //           const deductions = res.PayslipSections.find((s: any) => s.SectionName === 'DEDUCTIONS')?.Components || [];
  //           const summary = res.PayslipSections.find((s: any) => s.SectionName === 'SUMMARY')?.Components || [];

  //           const maxRows = Math.max(earnings.length, deductions.length);
  //           const rows: any[] = [];
  //           for (let i = 0; i < maxRows; i++) {
  //             rows.push([
  //               earnings[i]?.ComponentName || '',
  //               this.f(earnings[i]?.ComponentValue),
  //               '0.00',
  //               deductions[i]?.ComponentName || '',
  //               this.f(deductions[i]?.ComponentValue),
  //               '0.00'
  //             ]);
  //           }

  //           // Add summary
  //           const gross = summary.find((s: any) => s.ComponentCode === 'GS');
  //           const totalDed = summary.find((s: any) => s.ComponentCode === 'TD');
  //           const net = summary.find((s: any) => s.ComponentCode === 'NS');

  //           rows.push([
  //             'Gross Salary', this.f(gross?.ComponentValue), '0.00',
  //             'Total Deduction', this.f(totalDed?.ComponentValue), '0.00'
  //           ]);

  //           rows.push([
  //             { content: `Net Salary Rs. ${this.f(net?.ComponentValue)}`, colSpan: 3, styles: { fontStyle: 'bold', halign: 'left', fillColor: [230, 230, 230] } },
  //             { content: `(${this.amountInWords(net?.ComponentValue)} Only)`, colSpan: 3, styles: { fontStyle: 'italic', halign: 'left', fillColor: [230, 230, 230] } }
  //           ]);

  //           let payTableMinX = Infinity, payTableMaxX = -Infinity, payTableMinY = Infinity, payTableMaxY = -Infinity;
  //           const numberOfCols = 6;

  //           autoTable(doc, {
  //             startY,
  //             head: [['Earnings', 'Current', 'Arrear', 'Deductions', 'Current', 'Arrear']],
  //             body: rows,
  //             theme: 'plain',
  //             styles: { fontSize: 9, cellPadding: 6, valign: 'middle', textColor: 20 },
  //             margin: { left: tableMargin, right: tableMargin },
  //             didParseCell: (data) => {
  //               const col = data.column.index;
  //               if ([1, 2, 4, 5].includes(col)) {
  //                 data.cell.styles.fillColor = [220, 220, 220];  // background color
  //               }
  //               data.cell.styles.lineWidth = 0;
  //             },

  //             didDrawCell: (data) => {
  //               const { cell, row, column } = data;
  //               if (!cell) return;

  //               // Track table box bounds
  //               payTableMinX = Math.min(payTableMinX, cell.x);
  //               payTableMinY = Math.min(payTableMinY, cell.y);
  //               payTableMaxX = Math.max(payTableMaxX, cell.x + cell.width);
  //               payTableMaxY = Math.max(payTableMaxY, cell.y + cell.height);

  //               // ==== HEADER ROW: Only Bottom Border ====
  //               if (row.section === 'head') {
  //                 doc.setDrawColor(0);
  //                 doc.setLineWidth(1);
  //                 doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
  //                 return; // do not draw other borders
  //               }

  //               // ==== BODY ROW NORMAL VERTICAL LINES ====
  //               if (column.index < numberOfCols - 1) {
  //                 doc.setDrawColor(0);
  //                 doc.setLineWidth(0.5);
  //                 doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
  //               }

  //               // ==== SUMMARY FULL-BORDER ROWS ====
  //               const raw = row.raw;
  //               let isSummary = false;

  //               if (Array.isArray(raw)) {
  //                 const first = raw[0];

  //                 // Check if this row is the "Net Salary" row (CellDef)
  //                 if (
  //                   first &&
  //                   typeof first === 'object' &&
  //                   'content' in first &&
  //                   typeof first.content === 'string' &&
  //                   first.content.startsWith('Net Salary')
  //                 ) {
  //                   isSummary = true;
  //                 }

  //                 // Check Gross Salary row (plain string)
  //                 if (first === 'Gross Salary') {
  //                   isSummary = true;
  //                 }
  //               }

  //               if (isSummary) {
  //                 doc.setDrawColor(0);
  //                 doc.setLineWidth(1);

  //                 // FULL BORDER (top, bottom, left, right)
  //                 doc.line(payTableMinX, cell.y, payTableMaxX, cell.y);                      // TOP
  //                 doc.line(payTableMinX, cell.y + cell.height, payTableMaxX, cell.y + cell.height); // BOTTOM
  //                 doc.line(payTableMinX, cell.y, payTableMinX, cell.y + cell.height);        // LEFT
  //                 doc.line(payTableMaxX, cell.y, payTableMaxX, cell.y + cell.height);        // RIGHT
  //               }
  //             },
  //             didDrawPage: () => {
  //               // Outer border
  //               doc.setDrawColor(0);
  //               doc.setLineWidth(1);
  //               doc.rect(payTableMinX, payTableMinY, payTableMaxX - payTableMinX, payTableMaxY - payTableMinY);
  //             }
  //           });

  //           // ===== FOOTER =====
  //           const footerY = (doc as any).lastAutoTable.finalY + 20;
  //           doc.setFontSize(9);
  //           doc.text('** Note:', 40, footerY);
  //           doc.text('(Figures in INR)', 115, footerY);
  //           doc.text(`Generated On: ${new Date().toLocaleDateString()}`, 40, footerY + 22);

  //           // OPEN PDF in new tab
  //           const pdfBlob = doc.output('blob');
  //           const pdfUrl = URL.createObjectURL(pdfBlob);
  //           window.open(pdfUrl, '_blank');
  //         };

  //         logo.onload = generatePDF;
  //         logo.onerror = generatePDF;

  //       },
  //       error: (err: any) => {
  //         this.isSpinner = false;
  //         this.triggerToast('Internal Server Error', 'To Generate Payslip', 'danger');
  //       }
  //     });
  //   }
  //   f(v: any) { return v ? Number(v).toLocaleString('en-IN', { minimumFractionDigits: 2 }) : '0.00'; }

  //   // Convert salary to words
  //   amountInWords(num: number | string): string {
  //     const n = Number(num);
  //     if (!n) return 'Zero';
  //     const a = ["", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"];
  //     const b = ["", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"];
  //     const inWords = (num: number): string => {
  //       if (num < 20) return a[num];
  //       if (num < 100) return b[Math.floor(num / 10)] + " " + a[num % 10];
  //       if (num < 1000) return a[Math.floor(num / 100)] + " Hundred " + inWords(num % 100);
  //       if (num < 100000) return inWords(Math.floor(num / 1000)) + " Thousand " + inWords(num % 1000);
  //       if (num < 10000000) return inWords(Math.floor(num / 100000)) + " Lakh " + inWords(num % 100000);
  //       return inWords(Math.floor(num / 10000000)) + " Crore " + inWords(num % 10000000);
  //     };
  //     return inWords(n).trim();
  //   }

  // // Generate rows dynamically (two columns per row)
  // const empBody: any[] = [];
  // for (let i = 0; i < keys.length; i += 2) {
  //   const key1 = keys[i];
  //   const key2 = keys[i + 1];
  //   empBody.push([
  //     formatLabel(key1),
  //     e[key1] ?? '-',
  //     key2 ? formatLabel(key2) : '',
  //     key2 ? e[key2] ?? '-' : ''
  //   ]);
  // }
  // Header row (Name & Designation only, as before)

  // downloadPDF() {
  //   const doc = new jsPDF('p', 'pt', 'a4');
  //   const pageWidth = doc.internal.pageSize.getWidth();
  //   // load the logo image
  //   const logo = new Image();
  //   logo.src = './assets/Logo3DCAD.png';

  //   logo.onload = () => {
  //     // ===== HEADER =====
  //     const margin = 6;
  //     const headerHeight = 110;
  //     doc.setFillColor(245, 245, 245);
  //     doc.rect(margin, 0, pageWidth - 2 * margin, headerHeight, 'F'
  //     );

  //     try {
  //       doc.addImage(logo, 'PNG', margin + 30, 36, 110, 45);
  //     } catch (e) { }

  //     // right-side company address block
  //     doc.setFontSize(9);
  //     doc.setTextColor(1);
  //     const addressX = pageWidth - 30;
  //     const addressLines = [
  //       '3D Concept Analysis & Development India Pvt Ltd',
  //       'Sapthagiri Towers, #12,60Feet Road,',
  //       'NHBC Layout, Prashanth Nagar,',
  //       'Bangalore-560079',
  //       'Phone : +91-80-46504500',
  //       'Fax : +91 80 42459595',
  //       'E-mail : india@3dcad-global.com'
  //     ];
  //     let ay = 30;
  //     addressLines.forEach((line) => {
  //       doc.text(line, addressX, ay, { align: 'right' });

  //       // If this is the email line, draw the line to its left
  // if (line.startsWith('E-mail')) {
  //   const emailText = line;
  //   const emailY = ay; // same Y as text
  //   const emailX = pageWidth - 30; // right-aligned X
  //   const textWidth = doc.getTextWidth(emailText);

  //   doc.setDrawColor(0, 112, 192); // blue
  //   doc.setLineWidth(1);

  //   // Draw line from left margin (e.g., 40) to start of email text
  //   const startX = 40;
  //   const endX = emailX - textWidth; // stop at start of email text
  //   doc.line(startX, emailY + 1, endX, emailY + 1);
  // }

  //       ay += 12.5; // move to next line
  //     });

  //     // Title line (positioned similar to provided image)
  //     doc.setFontSize(11);
  //     doc.setFont('helvetica', 'bold');
  //     doc.text('Salary Slip for the month :', 40, 135);
  //     doc.setFont('helvetica', 'normal');
  //     doc.text('January - 2025', 300, 135);

  //     // ===== EMPLOYEE DETAILS (two-column style up to ESI) =====
  //     const empHead = [['Name', 'GIRISH J', 'Designation', 'Junior Design Engineer']];
  //     const empBody = [
  //       ['EmpNo', '3DCAD-971', 'Location', 'Bangalore'],
  //       ['PAN No', 'DJMPG1331N', 'Bank A/c No', '736301501298'],
  //       ['PFNo', '/PNY/30785PYPNY00307850000011420', 'Days Paid', '31'],
  //       ['UAN No', '101702697980', 'LOP', '0'],
  //       ['ESI No', '5347593254', '', '']
  //     ];

  //     autoTable(doc, {
  //       startY: 150,
  //       head: empHead,
  //       body: empBody,
  //       theme: 'plain',
  //       styles: {
  //         fontSize: 9,
  //         cellPadding: 6,
  //         textColor: 20,
  //         overflow: 'linebreak'
  //       },
  //       headStyles: {
  //         fillColor: [255, 255, 255],
  //         textColor: 0,
  //         fontStyle: 'bold',
  //         halign: 'right',
  //       },
  //       bodyStyles: {
  //         fillColor: [230, 230, 230],
  //         textColor: 20
  //       },
  //       columnStyles: {
  //         0: { halign: 'left' },
  //         1: { halign: 'right' },
  //         2: { halign: 'left' },
  //         3: { halign: 'right' }
  //       },
  //       margin: { left: 5, right: 5 },
  //       tableLineColor: [0, 0, 0],
  //       tableLineWidth: 1,
  //       didDrawCell: (data) => {
  //         const { cell, column, row } = data;
  //         if (!cell) return;
  //         const table: any = (data as any).table;
  //         doc.setDrawColor(1);
  //         doc.setLineWidth(0.5);

  //         if (row.index === 0 && column.index === 0) {
  //           try {
  //             doc.rect(table.startX, table.headRow.y, table.width, table.height, 'S');
  //           } catch (e) {
  //           }
  //         }
  //         // doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
  //         if (row.index === 0) {
  //           doc.line(cell.x, cell.y, cell.x + cell.width, cell.y);
  //         }
  //         if (column.index === 2) {
  //           doc.line(cell.x, cell.y, cell.x, cell.y + cell.height);
  //         }
  //       }
  //     });

  //     // ===== EARNINGS & DEDUCTIONS: start exactly at previous finalY so they join =====
  //     const startY = (doc as any).lastAutoTable.finalY;

  //     const earnings: any[] = [
  //       ['Basic Salary', '8,750.00', '0.00', 'Employee PF Contribution', '1,050.00', '0.00'],
  //       ['Indian Allowance', '15,412.00', '0.00', 'Professional Tax', '200.00', '0.00'],
  //       ['HRA', '5,250.00', '0.00', 'Income Tax', '0.00', '0.00'],
  //       ['Conveyance Allowance', '3,500.00', '0.00', 'Employee ESI Contribution', '0.00', '0.00'],
  //       ['Bonus', '0.00', '0.00', 'VPF Contribution', '0.00', '0.00'],
  //       ['Special Project Allowance', '0.00', '0.00', 'Transport Deduction', '0.00', '0.00'],
  //       ['Medical Reimbursement', '0.00', '0.00', 'Employee Welfare Fund', '0.00', '0.00'],
  //       ['Shift Allowance', '0.00', '0.00', 'Salary Advance', '0.00', '0.00'],
  //       ['Leave Travel Allowance', '0.00', '0.00', 'Other Deduction', '0.00', '0.00'],
  //       ['Gross Salary', '32,912.00', '0.00', 'Total Deduction', '1,250.00', '0.00'],
  //       [
  //         {
  //           content: 'Net Salary Rs. 31,662.00',
  //           colSpan: 3,
  //           styles: { fontStyle: 'bold' as const, halign: 'left' as const, fillColor: [230, 230, 230] }
  //         },
  //         {
  //           content: '(Thirty-One Thousand Six Hundred Sixty-Two Only)',
  //           colSpan: 3,
  //           styles: { fontStyle: 'italic' as const, halign: 'left' as const, fillColor: [230, 230, 230], }
  //         }
  //       ]
  //     ];

  //     autoTable(doc, {
  //       startY: startY,
  //       head: [['Earnings', 'Current', 'Arrear', 'Deductions', 'Current', 'Arrear']],
  //       body: earnings,
  //       theme: 'plain',
  //       styles: {
  //         fontSize: 9,
  //         cellPadding: 6,
  //         valign: 'middle',
  //         textColor: 20,

  //       },
  //       headStyles: {
  //         fillColor: [255, 255, 255],
  //         textColor: 0,
  //         fontStyle: 'bold',
  //         // halign: 'center' as const
  //       },
  //       bodyStyles: {
  //         fillColor: [255, 255, 255], // white rows by default
  //         textColor: 20
  //       },
  //       columnStyles: {
  //       },
  //       margin: { left: 5, right: 5 },
  //       tableLineColor: [0, 0, 0],
  //       tableLineWidth: 0.5,
  //       didParseCell: (data) => {
  //         const { row, column, cell } = data;
  //         if (row.section === 'body') {
  //           if (Array.isArray(row.raw) && typeof row.raw[0] === 'object') return;
  //           if ([1, 2, 4, 5].includes(column.index)) {
  //             cell.styles.fillColor = [230, 230, 230];
  //           }
  //         }
  //       },

  //       didDrawCell: (data) => {
  //         const { cell, row, column, table } = data;
  //         if (!cell) return;

  //         const raw: any = row.raw;

  //         const x1 = cell.x;
  //         const y1 = cell.y;
  //         const x2 = cell.x + cell.width;
  //         const y2 = cell.y + cell.height;

  //         // 1) Vertical borders for all cells
  //         doc.setDrawColor(0);
  //         doc.setLineWidth(0.5);
  //         // Left border (first column)
  //         if (column.index === 0) doc.line(x1, y1, x1, y2);
  //         if (column.index === 1) doc.line(x1, y1, x1, y2);
  //         if (column.index === 2) doc.line(x1, y1, x1, y2);
  //         if (column.index === 3) doc.line(x1, y1, x1, y2);
  //         if (column.index === 4) doc.line(x1, y1, x1, y2);
  //         if (column.index === 5) doc.line(x1, y1, x1, y2);

  //         // 2) Horizontal border ONLY for special rows
  //         const t: any = table; // bypass TS typing
  //         const fullX1 = t.startX ?? 5;
  //         const fullX2 = t.startX != null && t.width != null ? t.startX + t.width : x2;
  //         doc.setLineWidth(1);
  //         if (Array.isArray(raw) && raw[0] === "Earnings") {
  //           doc.line(fullX1, y2, fullX2, y2); // bottom
  //         }

  //         if (Array.isArray(raw) && raw[0] === "Deductions") {
  //           doc.line(fullX1, y2, fullX2, y2); // bottom
  //         }

  //         if (Array.isArray(raw) && raw[0] === "Gross Salary") {
  //           doc.line(fullX1, y1, fullX2, y1); // top
  //           doc.line(fullX1, y2, fullX2, y2); // bottom
  //         }

  //         if (Array.isArray(raw) && raw[3] === "Total Deduction") {
  //           doc.line(fullX1, y1, fullX2, y1); // top
  //           doc.line(fullX1, y2, fullX2, y2); // bottom
  //         }

  //         if (
  //           Array.isArray(raw) &&
  //           raw[0] &&
  //           typeof raw[0] === "object" &&
  //           typeof raw[0].content === "string" &&
  //           raw[0].content.startsWith("Net Salary")
  //         ) {
  //           doc.line(fullX1, y1, fullX2, y1); // top
  //           doc.line(fullX1, y2, fullX2, y2); // bottom
  //         }
  //       }
  //     });

  //     // ===== FOOTER/NOTES =====
  //     const footerY = (doc as any).lastAutoTable.finalY + 20;
  //     doc.setFontSize(9);
  //     doc.text('** Note:', 40, footerY);
  //     doc.text('(Figures in INR)', 115, footerY);
  //     doc.text('Generated On: 29/10/2025', 40, footerY + 22);

  //     // Save
  //     // doc.save('Salary_Slip_January_2025.pdf');

  //     // Open in new tab
  //     const pdfBlob = doc.output('blob');
  //     const url = URL.createObjectURL(pdfBlob);
  //     window.open(url, '_blank');

  //   };

  //   logo.onerror = () => {
  //     console.warn('Logo load failed; continuing without logo.');
  //     // you could call a fallback path to build same pdf without waiting for onload
  //   };
  // }



  // Format numbers
}
