import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { DashboardService } from '../../service/dashboard.service';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';
import { payRollService } from '../../service/payroll.service';

@Component({
  selector: 'app-payslip',
  standalone: true,
  imports: [CommonModule, SharedModule, ToastMessageComponent, ReactiveFormsModule],
  templateUrl: './payslip.component.html',
  styleUrl: './payslip.component.scss'
})
export class PayslipComponent implements OnInit {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  isSpinner: boolean = false;
  years: number[] = [];
  months: { id: number, name: string }[] = [];
  selectedYear!: number;
  selectedMonth: any;
  payslipForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails;
  accessPolicy: any;
  controlAccessPage: any;

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
    const currentYear = new Date().getFullYear();
    for (let yr = 2020; yr <= currentYear; yr++) {
      this.years.push(yr);
    }

    this.payslipForm = this.fb.group({
      year: ['', [Validators.required]],
      month: ['', [Validators.required]],
    })

  }

  getAllMonths() {
    return [
      { id: 1, name: 'January' }, { id: 2, name: 'February' },
      { id: 3, name: 'March' }, { id: 4, name: 'April' },
      { id: 5, name: 'May' }, { id: 6, name: 'June' },
      { id: 7, name: 'July' }, { id: 8, name: 'August' },
      { id: 9, name: 'September' }, { id: 10, name: 'October' },
      { id: 11, name: 'November' }, { id: 12, name: 'December' }
    ];
  }

  onYearChange() {
    this.selectedYear = Number(this.selectedYear);
    this.selectedMonth = '';
    const currentYear = new Date().getFullYear();
    const currentMonth = new Date().getMonth() + 1;
    const allMonths = this.getAllMonths();
    if (this.selectedYear === currentYear) {
      this.months = allMonths.filter(m => m.id < currentMonth);
    } else {
      this.months = allMonths;
    }
  }


  resetData() {
    this.payslipForm.reset();
    this.isFormSubmitted = false;
  }


  // generatePayslip(event?: Event) {
  //   if (this.payslipForm.invalid) {
  //     this.isFormSubmitted = true;
  //   } else {
  //     if (event) event.preventDefault();
  //     const month = this.payslipForm?.get('month').value;
  //     console.log(month)
  //     const reqBody = {
  //       LoginId: this.employeeDetails[0].LoginId,
  //       EmpCode: this.employeeDetails[0].EmpCode,
  //       Year: this.payslipForm?.get('year').value,
  //       MonthNo: month.id,
  //       Month: month.name,
  //     };
  //     this.isSpinner = true;
  //     this.payrollService.EmpPayslipGeneration(reqBody).subscribe({
  //       next: (res: any) => {
  //         this.isSpinner = false;
  //         if (!res || !res.EmployeeDetails) {
  //           this.triggerToast('', res['Message'], 'warning');
  //           return;
  //         } if (res.EmployeeDetails === null) {
  //           this.triggerToast(res['Message'], 'No Employee Details Found', 'warning');
  //           return;
  //         }
  //         const doc = new jsPDF('p', 'pt', 'a4');
  //         const pageWidth = doc.internal.pageSize.getWidth();
  //         const margin = 11;
  //         const totalWidth = pageWidth - margin * 2;
  //         const leftHalf = totalWidth / 2;
  //         const rightHalf = totalWidth - leftHalf;

  //         const empCol0 = 90;
  //         const empCol1 = leftHalf - empCol0;
  //         const empCol2 = 110;
  //         const empCol3 = rightHalf - empCol2;

  //         const payCols = [
  //           Math.round(leftHalf * 0.5),
  //           Math.round(leftHalf * 0.25),
  //           Math.round(leftHalf * 0.25),
  //           Math.round(rightHalf * 0.5),
  //           Math.round(rightHalf * 0.25),
  //           Math.round(rightHalf * 0.25)
  //         ];

  //         const logo = new Image();
  //         logo.src = './assets/Logo3DCAD.png';
  //         const generatePDF = () => {
  //           const headerHeight = 120;
  //           doc.setFillColor(245, 245, 245);
  //           doc.rect(margin, 8, pageWidth - margin * 2, headerHeight, 'F');
  //           try { doc.addImage(logo, 'PNG', margin + 18, 40, 140, 52); } catch { }

  //           const companyLines = [
  //             res.Company.CompanyName || '',
  //             res.Company.CompanyAddress || '',
  //             `Phone : ${res.Company.CompanyPhoneNo || '-'}`,
  //             `Fax : ${res.Company.CompanyFax || '-'}`,
  //             `E-mail : ${res.Company.CompanyEmail || '-'}`,
  //           ];
  //           doc.setFontSize(9);
  //           let companyY = 26;
  //           const rightX = pageWidth - margin - 18;
  //           const leftX = margin + 18;

  //           companyLines.forEach((ln, index) => {
  //             const wrapped = doc.splitTextToSize(ln, 180);
  //             wrapped.forEach((wl: any) => {
  //               doc.text(wl, rightX, companyY, { align: 'right' });
  //               if (index === companyLines.length - 1) {
  //                 const textWidth = doc.getTextWidth(wl);
  //                 const lineEndX = rightX - textWidth - 4;
  //                 doc.setDrawColor(0, 112, 192);
  //                 doc.setLineWidth(1);
  //                 doc.line(leftX, companyY + 2, lineEndX, companyY + 2);
  //               }
  //               companyY += 12;
  //             });
  //           });
  //           const salaryText =
  //             res.SalaryMonth && res.Year
  //               ? `${res.SalaryMonth} - ${res.Year}`
  //               : res.SalaryMonth || res.Year || '';

  //           const label = "Salary Slip for the month :";
  //           const value = salaryText;

  //           doc.setFont('helvetica', 'bold');
  //           doc.setFontSize(9);
  //           const titleY = 145;
  //           doc.text(label, pageWidth / 2 - 140, titleY); // lable
  //           doc.text(value, pageWidth / 2 + 20, titleY);  //month

  //           /// This is EmployeeDetails //////////////
  //           const e = res.EmployeeDetails;
  //           const empHead = [['Name', e.Name ?? '-', 'Designation', e.Designation ?? '-']];
  //           const excludeKeys = ["Name", "Designation"];
  //           const empEntries = Object.entries(e).filter(([key]) => !excludeKeys.includes(key));
  //           const empBody = [];
  //           for (let i = 0; i < empEntries.length; i += 2) {
  //             const [key1, val1] = empEntries[i];
  //             const [key2, val2] = empEntries[i + 1] || ['', ''];
  //             empBody.push([
  //               this.formatLabel(key1),
  //               val1 || '-',
  //               this.formatLabel(key2),
  //               val2 || '-'
  //             ]);
  //           }
  //           const leftHalf = totalWidth / 2;
  //           const rightHalf = totalWidth - leftHalf;
  //           const empCol0 = Math.round(leftHalf * 0.3);
  //           const empCol1 = Math.round(leftHalf * 0.7);
  //           const empCol2 = Math.round(rightHalf * 0.3);
  //           const empCol3 = Math.round(rightHalf * 0.7);

  //           let empMinX = Infinity, empMinY = Infinity, empMaxX = -Infinity, empMaxY = -Infinity;
  //           autoTable(doc, {
  //             startY: 155,
  //             head: empHead,
  //             body: empBody,
  //             theme: 'plain',
  //             styles: { fontSize: 9, cellPadding: 6, textColor: 20 },
  //             headStyles: { fillColor: [255, 255, 255], textColor: 0, fontStyle: 'bold' },
  //             bodyStyles: { fillColor: [245, 245, 245], textColor: 20 },
  //             margin: { left: margin, right: margin },
  //             columnStyles: {
  //               0: { cellWidth: empCol0, halign: 'left' },
  //               1: { cellWidth: empCol1, halign: 'right' },
  //               2: { cellWidth: empCol2, halign: 'left' },
  //               3: { cellWidth: empCol3, halign: 'right' }
  //             },
  //             didParseCell: (data) => {
  //               if (data.section === 'head') {
  //                 if (data.column.index === 1 || data.column.index === 3) {
  //                   data.cell.styles.halign = 'right';
  //                 }
  //               }
  //             },
  //             didDrawCell: (data) => {
  //               const { cell, row, column } = data;
  //               if (!cell) return;
  //               empMinX = Math.min(empMinX, cell.x);
  //               empMinY = Math.min(empMinY, cell.y);
  //               empMaxX = Math.max(empMaxX, cell.x + cell.width);
  //               empMaxY = Math.max(empMaxY, cell.y + cell.height);
  //               if (row.section === 'head') {
  //                 doc.setDrawColor(180);
  //                 doc.setLineWidth(1.5);
  //                 doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
  //                 if (column.index === 2) doc.line(cell.x, cell.y, cell.x, cell.y + cell.height);
  //               } else {
  //                 if (column.index === 2) {
  //                   doc.setDrawColor(180);
  //                   doc.setLineWidth(1.5);
  //                   doc.line(cell.x, cell.y, cell.x, cell.y + cell.height);
  //                 }
  //               }
  //             },
  //             didDrawPage: () => {
  //               doc.setDrawColor(180);
  //               doc.setLineWidth(1.5);
  //               if (empMinX < Infinity) {
  //                 doc.rect(empMinX - 0.5, empMinY - 0.5, empMaxX - empMinX + 1, empMaxY - empMinY - 1);
  //               }
  //             },
  //           });
  //           const startY = empMaxY - 1;

  //           /// This is EARNINGS DEDUCTIONS  SUMMARY//////////////
  //           const earnings = res.PayslipSections?.find((s: any) => s.SectionName === 'EARNINGS')?.Components || [];
  //           const deductions = res.PayslipSections?.find((s: any) => s.SectionName === 'DEDUCTIONS')?.Components || [];
  //           const summary = res.PayslipSections?.find((s: any) => s.SectionName === 'SUMMARY')?.Components || [];

  //           // ✅ NEW: Handle VariableSections (DO NOT REMOVE EXISTING CODE)
  //           const variableSections = res.VariableSections || [];

  //           // Push variable components into earnings ONLY if exists
  //           if (variableSections.length > 0) {
  //             variableSections.forEach((v: any) => {
  //               earnings.push({
  //                 ComponentName: v.ComponentName,
  //                 ComponentValue: v.ComponentValue
  //               });
  //             });
  //           }

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
  //           const gross = summary.find((s: any) => s.ComponentCode === 'GS');
  //           const totalDed = summary.find((s: any) => s.ComponentCode === 'TD');
  //           const net = summary.find((s: any) => s.ComponentCode === 'NS');
  //           rows.push([
  //             'Gross Salary', this.f(gross?.ComponentValue), '0.00',
  //             'Total Deduction', this.f(totalDed?.ComponentValue), '0.00'
  //           ]);
  //           rows.push([
  //             {
  //               content: `Net Salary Rs. ${this.f(net?.ComponentValue)}`,
  //               colSpan: 3,
  //               styles: { fontStyle: 'bold', halign: 'left', fillColor: [245, 245, 245] }
  //             },
  //             {
  //               content: `(${this.amountInWords(Math.floor(Number(net?.ComponentValue)))} Only)`,
  //               colSpan: 3,
  //               styles: { fontStyle: 'italic', halign: 'left', fillColor: [245, 245, 245] }
  //             }
  //           ]);
  //           rows.push([
  //             {
  //               content: '** Note:',
  //               colSpan: 1,
  //               styles: {
  //                 fontStyle: 'italic',
  //                 halign: 'left',
  //                 fillColor: [255, 255, 255],
  //                 cellPadding: 15,
  //                 minCellHeight: 20,
  //                 lineWidth: 0.4,
  //                 lineColor: [180, 180, 180]
  //               }
  //             },
  //             {
  //               content: '(Figures in INR)',
  //               colSpan: 5,
  //               styles: {
  //                 fontStyle: 'italic',
  //                 halign: 'left',
  //                 fillColor: [0, 0, 0],
  //                 cellPadding: 15,
  //                 minCellHeight: 20,
  //                 lineWidth: 0.4,
  //                 lineColor: [180, 180, 180],
  //               }
  //             }
  //           ]);
  //           let payMinX = Infinity, payMinY = Infinity, payMaxX = -Infinity, payMaxY = -Infinity;
  //           const numCols = 6;

  //           autoTable(doc, {
  //             startY,
  //             head: [['Earnings', 'Current', 'Arrear', 'Deductions', 'Current', 'Arrear']],
  //             body: rows,
  //             theme: 'plain',
  //             styles: { fontSize: 9, cellPadding: 6, valign: 'middle', textColor: 20 },
  //             headStyles: { fillColor: [255, 255, 255], textColor: 0, fontStyle: 'bold' },
  //             margin: { left: margin, right: margin },
  //             columnStyles: {
  //               0: { cellWidth: payCols[0], halign: 'left' },
  //               1: { cellWidth: payCols[1], halign: 'right' },
  //               2: { cellWidth: payCols[2], halign: 'right' },
  //               3: { cellWidth: payCols[3], halign: 'left' },
  //               4: { cellWidth: payCols[4], halign: 'right' },
  //               5: { cellWidth: payCols[5], halign: 'right' }
  //             },
  //             didParseCell: (data) => {
  //               const c = data.column.index;
  //               if ([1, 2, 4, 5].includes(c)) {
  //                 data.cell.styles.fillColor = [245, 245, 245];
  //               }
  //               if (data.section === 'head') {
  //                 data.cell.styles.lineWidth = 1;
  //               } else {
  //                 data.cell.styles.lineWidth = 0;
  //               }
  //             },

  //             didDrawCell: (data) => {
  //               const { cell, row, column } = data;
  //               if (!cell) return;

  //               payMinX = Math.min(payMinX, cell.x);
  //               payMinY = Math.min(payMinY, cell.y);
  //               payMaxX = Math.max(payMaxX, cell.x + cell.width);
  //               payMaxY = Math.max(payMaxY, cell.y + cell.height);

  //               const grossRowIndex = maxRows;
  //               const netRowIndex = maxRows + 1;

  //               if (row.section === 'head') {
  //                 // doc.setDrawColor(0);
  //                 doc.setLineWidth(0);
  //                 doc.rect(cell.x, cell.y, cell.width, cell.height);
  //                 doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
  //               }

  //               if (row.section === 'body') {
  //                 if (row.index === grossRowIndex || row.index === netRowIndex) {
  //                   doc.setDrawColor(180);
  //                   doc.setLineWidth(1);
  //                   doc.rect(cell.x, cell.y, cell.width, cell.height);
  //                 } else if (column.index < numCols - 1) {
  //                   doc.setDrawColor(180);
  //                   doc.setLineWidth(1.5);
  //                   doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
  //                 }
  //               }

  //               // ⭐ FIX: Add right-side border for NET SALARY colSpan row (CTC RELATED)
  //               if (row.index === netRowIndex && column.index === numCols - 1) {
  //                 doc.setDrawColor(180);
  //                 doc.setLineWidth(1.5);
  //                 doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
  //               }
  //             },

  //             didDrawPage: () => {
  //               doc.setDrawColor(180);
  //               doc.setLineWidth(1.5);
  //               if (payMinX < Infinity) doc.rect(payMinX - 0.5, payMinY - 0.5, payMaxX - payMinX + 1, payMaxY - payMinY + 1);
  //             }
  //           });

  //           const lastY = (doc as any).lastAutoTable && (doc as any).lastAutoTable.finalY
  //             ? (doc as any).lastAutoTable.finalY
  //             : payMaxY;
  //           const footerY = lastY + 18;
  //           // doc.setFontSize(9);
  //           // doc.text('** Note:', margin + 10, footerY);
  //           // doc.text('(Figures in INR)', margin + 95, footerY);
  //           doc.text(`Generated On: ${new Date().toLocaleDateString()}`, margin + 10, footerY + 20);

  //           const pageHeight = doc.internal.pageSize.getHeight();
  //           const bottomY = pageHeight - 20;
  //           doc.setFontSize(9);
  //           doc.text('System Generated Payslip Signature Not Required', margin + 10, bottomY);

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
  // }


  generatePayslip(event?: Event) {
    if (this.payslipForm.invalid) {
      this.isFormSubmitted = true;
    } else {
      if (event) event.preventDefault();
      const month = this.payslipForm?.get('month').value;
      console.log(month)
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        EmpCode: this.employeeDetails[0].EmpCode,
        Year: this.payslipForm?.get('year').value,
        MonthNo: month.id,
        Month: month.name,
      };
      this.isSpinner = true;
      this.payrollService.EmpPayslipGeneration(reqBody).subscribe({
        next: (res: any) => {
          this.isSpinner = false;
          if (!res || !res.EmployeeDetails) {
            this.triggerToast('', res['Message'], 'warning');
            return;
          } if (res.EmployeeDetails === null) {
            this.triggerToast(res['Message'], 'No Employee Details Found', 'warning');
            return;
          }
          const doc = new jsPDF('p', 'pt', 'a4');
          const pageWidth = doc.internal.pageSize.getWidth();
          const margin = 11;
          const totalWidth = pageWidth - margin * 2;
          const leftHalf = totalWidth / 2;
          const rightHalf = totalWidth - leftHalf;

          const empCol0 = 90;
          const empCol1 = leftHalf - empCol0;
          const empCol2 = 110;
          const empCol3 = rightHalf - empCol2;

          const payCols = [
            Math.round(leftHalf * 0.5),
            Math.round(leftHalf * 0.25),
            Math.round(leftHalf * 0.25),
            Math.round(rightHalf * 0.5),
            Math.round(rightHalf * 0.25),
            Math.round(rightHalf * 0.25)
          ];

          const logo = new Image();
          logo.src = './assets/Logo3DCAD.png';
          const generatePDF = () => {
            const headerHeight = 120;
            doc.setFillColor(245, 245, 245);
            doc.rect(margin, 8, pageWidth - margin * 2, headerHeight, 'F');
            try { doc.addImage(logo, 'PNG', margin + 18, 40, 140, 52); } catch { }

            const companyLines = [
              res.Company.CompanyName || '',
              res.Company.CompanyAddress || '',
              `Phone : ${res.Company.CompanyPhoneNo || '-'}`,
              `Fax : ${res.Company.CompanyFax || '-'}`,
              `E-mail : ${res.Company.CompanyEmail || '-'}`,
            ];
            doc.setFontSize(9);
            let companyY = 26;
            const rightX = pageWidth - margin - 18;
            const leftX = margin + 18;

            companyLines.forEach((ln, index) => {
              const wrapped = doc.splitTextToSize(ln, 180);
              wrapped.forEach((wl: any) => {
                doc.text(wl, rightX, companyY, { align: 'right' });
                if (index === companyLines.length - 1) {
                  const textWidth = doc.getTextWidth(wl);
                  const lineEndX = rightX - textWidth - 4;
                  doc.setDrawColor(0, 112, 192);
                  doc.setLineWidth(1);
                  doc.line(leftX, companyY + 2, lineEndX, companyY + 2);
                }
                companyY += 12;
              });
            });
            const salaryText =
              res.SalaryMonth && res.Year
                ? `${res.SalaryMonth} - ${res.Year}`
                : res.SalaryMonth || res.Year || '';

            const label = "Salary Slip for the month :";
            const value = salaryText;

            doc.setFont('helvetica', 'bold');
            doc.setFontSize(9);
            const titleY = 145;
            doc.text(label, pageWidth / 2 - 140, titleY); // lable
            doc.text(value, pageWidth / 2 + 20, titleY);  //month

            /// This is EmployeeDetails //////////////
            const e = res.EmployeeDetails;
            const empHead = [['Name', e.Name ?? '-', 'Designation', e.Designation ?? '-']];
            const excludeKeys = ["Name", "Designation"];
            const empEntries = Object.entries(e).filter(([key]) => !excludeKeys.includes(key));
            const empBody = [];
            for (let i = 0; i < empEntries.length; i += 2) {
              const [key1, val1] = empEntries[i];
              const [key2, val2] = empEntries[i + 1] || ['', ''];
              empBody.push([
                this.formatLabel(key1),
                val1 || '-',
                this.formatLabel(key2),
                val2 || '-'
              ]);
            }
            const leftHalf = totalWidth / 2;
            const rightHalf = totalWidth - leftHalf;
            const empCol0 = Math.round(leftHalf * 0.3);
            const empCol1 = Math.round(leftHalf * 0.7);
            const empCol2 = Math.round(rightHalf * 0.3);
            const empCol3 = Math.round(rightHalf * 0.7);

            let empMinX = Infinity, empMinY = Infinity, empMaxX = -Infinity, empMaxY = -Infinity;
            autoTable(doc, {
              startY: 155,
              head: empHead,
              body: empBody,
              theme: 'plain',
              styles: { fontSize: 9, cellPadding: 6, textColor: 20 },
              headStyles: { fillColor: [255, 255, 255], textColor: 0, fontStyle: 'bold' },
              bodyStyles: { fillColor: [245, 245, 245], textColor: 20 },
              margin: { left: margin, right: margin },
              columnStyles: {
                0: { cellWidth: empCol0, halign: 'left' },
                1: { cellWidth: empCol1, halign: 'right' },
                2: { cellWidth: empCol2, halign: 'left' },
                3: { cellWidth: empCol3, halign: 'right' }
              },
              didParseCell: (data) => {
                if (data.section === 'head') {
                  if (data.column.index === 1 || data.column.index === 3) {
                    data.cell.styles.halign = 'right';
                  }
                }
              },
              didDrawCell: (data) => {
                const { cell, row, column } = data;
                if (!cell) return;
                empMinX = Math.min(empMinX, cell.x);
                empMinY = Math.min(empMinY, cell.y);
                empMaxX = Math.max(empMaxX, cell.x + cell.width);
                empMaxY = Math.max(empMaxY, cell.y + cell.height);
                if (row.section === 'head') {
                  doc.setDrawColor(180);
                  doc.setLineWidth(1.5);
                  doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
                  if (column.index === 2) doc.line(cell.x, cell.y, cell.x, cell.y + cell.height);
                } else {
                  if (column.index === 2) {
                    doc.setDrawColor(180);
                    doc.setLineWidth(1.5);
                    doc.line(cell.x, cell.y, cell.x, cell.y + cell.height);
                  }
                }
              },
              didDrawPage: () => {
                doc.setDrawColor(180);
                doc.setLineWidth(1.5);
                if (empMinX < Infinity) {
                  doc.rect(empMinX - 0.5, empMinY - 0.5, empMaxX - empMinX + 1, empMaxY - empMinY - 1);
                }
              },
            });
            const startY = empMaxY - 1;

            /// This is EARNINGS DEDUCTIONS  SUMMARY//////////////
            const earnings = res.PayslipSections?.find((s: any) => s.SectionName === 'EARNINGS')?.Components || [];
            const deductions = res.PayslipSections?.find((s: any) => s.SectionName === 'DEDUCTIONS')?.Components || [];
            const summary = res.PayslipSections?.find((s: any) => s.SectionName === 'SUMMARY')?.Components || [];
            const arrearComponents = res.ArrearSections?.[0]?.Components || [];

            // ✅ NEW: Handle VariableSections (DO NOT REMOVE EXISTING CODE)
            const variableSections = res.VariableSections || [];

            // Push variable components into earnings ONLY if exists
            if (variableSections.length > 0) {
              variableSections.forEach((v: any) => {
                earnings.push({
                  ComponentName: v.ComponentName,
                  ComponentValue: v.ComponentValue
                });
              });
            }

            const maxRows = Math.max(earnings.length, deductions.length);
            const rows: any[] = [];
            for (let i = 0; i < maxRows; i++) {
              const earn = earnings[i];
              const ded = deductions[i];

              // match arrear by ComponentCode (IMPORTANT)
              const earnArrear = arrearComponents.find(
                (a: any) => a.ComponentCode === earn?.ComponentCode
              );

              const dedArrear = arrearComponents.find(
                (a: any) => a.ComponentCode === ded?.ComponentCode
              );

              rows.push([
                earn?.ComponentName || '',
                this.f(earn?.ComponentValue),
                this.f(earnArrear?.ComponentValue),   // ✅ arrear earnings
                ded?.ComponentName || '',
                this.f(ded?.ComponentValue),
                this.f(dedArrear?.ComponentValue)    // ✅ arrear deductions
              ]);
            }
            const gross = summary.find((s: any) => s.ComponentCode === 'GS');
            const grossArrear = arrearComponents.find(
              (a: any) => a.ComponentCode === 'GS'
            );
            const totalDed = summary.find((s: any) => s.ComponentCode === 'TD');
            const net = summary.find((s: any) => s.ComponentCode === 'NS');
            rows.push([
              'Gross Salary',
              this.f(gross?.ComponentValue),
              this.f(grossArrear?.ComponentValue),   // ✅ NOW SHOWS 40000
              'Total Deduction',
              this.f(totalDed?.ComponentValue),
              '0.00'
            ]);
            rows.push([
              {
                content: `Net Salary Rs. ${this.f(net?.ComponentValue)}`,
                colSpan: 3,
                styles: { fontStyle: 'bold', halign: 'left', fillColor: [245, 245, 245] }
              },
              {
                content: `(${this.amountInWords(Math.floor(Number(net?.ComponentValue)))} Only)`,
                colSpan: 3,
                styles: { fontStyle: 'italic', halign: 'left', fillColor: [245, 245, 245] }
              }
            ]);
            rows.push([
              {
                content: '** Note:',
                colSpan: 1,
                styles: {
                  fontStyle: 'italic',
                  halign: 'left',
                  fillColor: [255, 255, 255],
                  cellPadding: 15,
                  minCellHeight: 20,
                  lineWidth: 0.4,
                  lineColor: [180, 180, 180]
                }
              },
              {
                content: '(Figures in INR)',
                colSpan: 5,
                styles: {
                  fontStyle: 'italic',
                  halign: 'left',
                  fillColor: [0, 0, 0],
                  cellPadding: 15,
                  minCellHeight: 20,
                  lineWidth: 0.4,
                  lineColor: [180, 180, 180],
                }
              },
            ]);
            if (res.DescriptionforArrear) {
              rows.push([
                {
                  content: '** Arrear Note:',
                  colSpan: 1,
                  styles: {
                    fontStyle: 'italic',
                    halign: 'left',
                    fillColor: [255, 255, 255],
                    cellPadding: 15,
                    minCellHeight: 20,
                    lineWidth: 0.4,
                    lineColor: [180, 180, 180]
                  }
                },
                {
                  content: res.DescriptionforArrear,
                  colSpan: 5,
                  styles: {
                    fontStyle: 'italic',
                    halign: 'left',
                    fillColor: [245, 245, 245],
                    cellPadding: 15,
                    minCellHeight: 20,
                    lineWidth: 0.4,
                    lineColor: [180, 180, 180],
                  }
                }
              ]);
            }
            let payMinX = Infinity, payMinY = Infinity, payMaxX = -Infinity, payMaxY = -Infinity;
            const numCols = 6;

            autoTable(doc, {
              startY,
              head: [['Earnings', 'Current', 'Arrear', 'Deductions', 'Current', 'Arrear']],
              body: rows,
              theme: 'plain',
              styles: { fontSize: 9, cellPadding: 6, valign: 'middle', textColor: 20 },
              headStyles: { fillColor: [255, 255, 255], textColor: 0, fontStyle: 'bold' },
              margin: { left: margin, right: margin },
              columnStyles: {
                0: { cellWidth: payCols[0], halign: 'left' },
                1: { cellWidth: payCols[1], halign: 'right' },
                2: { cellWidth: payCols[2], halign: 'right' },
                3: { cellWidth: payCols[3], halign: 'left' },
                4: { cellWidth: payCols[4], halign: 'right' },
                5: { cellWidth: payCols[5], halign: 'right' }
              },
              didParseCell: (data) => {
                const c = data.column.index;
                if ([1, 2, 4, 5].includes(c)) {
                  data.cell.styles.fillColor = [245, 245, 245];
                }
                if (data.section === 'head') {
                  data.cell.styles.lineWidth = 1;
                } else {
                  data.cell.styles.lineWidth = 0;
                }
              },

              didDrawCell: (data) => {
                const { cell, row, column } = data;
                if (!cell) return;

                payMinX = Math.min(payMinX, cell.x);
                payMinY = Math.min(payMinY, cell.y);
                payMaxX = Math.max(payMaxX, cell.x + cell.width);
                payMaxY = Math.max(payMaxY, cell.y + cell.height);

                const grossRowIndex = maxRows;
                const netRowIndex = maxRows + 1;

                if (row.section === 'head') {
                  // doc.setDrawColor(0);
                  doc.setLineWidth(0);
                  doc.rect(cell.x, cell.y, cell.width, cell.height);
                  doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
                }

                if (row.section === 'body') {
                  if (row.index === grossRowIndex || row.index === netRowIndex) {
                    doc.setDrawColor(180);
                    doc.setLineWidth(1);
                    doc.rect(cell.x, cell.y, cell.width, cell.height);
                  } else if (column.index < numCols - 1) {
                    doc.setDrawColor(180);
                    doc.setLineWidth(1.5);
                    doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
                  }
                }

                // ⭐ FIX: Add right-side border for NET SALARY colSpan row (CTC RELATED)
                if (row.index === netRowIndex && column.index === numCols - 1) {
                  doc.setDrawColor(180);
                  doc.setLineWidth(1.5);
                  doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
                }
              },

              didDrawPage: () => {
                doc.setDrawColor(180);
                doc.setLineWidth(1.5);
                if (payMinX < Infinity) doc.rect(payMinX - 0.5, payMinY - 0.5, payMaxX - payMinX + 1, payMaxY - payMinY + 1);
              }
            });

            const lastY = (doc as any).lastAutoTable && (doc as any).lastAutoTable.finalY
              ? (doc as any).lastAutoTable.finalY
              : payMaxY;
            const footerY = lastY + 18;
            // doc.setFontSize(9);
            // doc.text('** Note:', margin + 10, footerY);
            // doc.text('(Figures in INR)', margin + 95, footerY);
            doc.text(`Generated On: ${new Date().toLocaleDateString()}`, margin + 10, footerY + 20);

            const pageHeight = doc.internal.pageSize.getHeight();
            const bottomY = pageHeight - 20;
            doc.setFontSize(9);
            doc.text('System Generated Payslip Signature Not Required', margin + 10, bottomY);

            const pdfBlob = doc.output('blob');
            const pdfUrl = URL.createObjectURL(pdfBlob);
            window.open(pdfUrl, '_blank');
          };
          logo.onload = generatePDF;
          logo.onerror = generatePDF;
        },
        error: (err: any) => {
          this.isSpinner = false;
          this.triggerToast('Internal Server Error', 'To Generate Payslip', 'danger');
        }
      });
    }
  }


  formatLabel(key: string) {
    return key
      .replace(/([A-Z])/g, ' $1')
      .replace(/^./, c => c.toUpperCase())
      .trim();
  }

  f(v: any) { return v ? Number(v).toLocaleString('en-IN', { minimumFractionDigits: 2 }) : '0.00'; }

  // Convert salary to words
  amountInWords(num: number | string): string {
    let n = Number(num);
    if (isNaN(n) || n === 0) return 'Zero';
    // Handle decimals by rounding down (e.g., 30315.50 → 30315)
    n = Math.floor(n);
    const a = [
      "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
      "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
      "Seventeen", "Eighteen", "Nineteen"
    ];
    const b = [
      "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    ];
    const inWords = (num: number): string => {
      if (num < 20) return a[num];
      if (num < 100) return b[Math.floor(num / 10)] + (num % 10 ? " " + a[num % 10] : "");
      if (num < 1000) return inWords(Math.floor(num / 100)) + " Hundred" +
        (num % 100 ? " " + inWords(num % 100) : "");
      if (num < 100000) return inWords(Math.floor(num / 1000)) + " Thousand" +
        (num % 1000 ? " " + inWords(num % 1000) : "");
      if (num < 10000000) return inWords(Math.floor(num / 100000)) + " Lakh" +
        (num % 100000 ? " " + inWords(num % 100000) : "");
      return inWords(Math.floor(num / 10000000)) + " Crore" +
        (num % 10000000 ? " " + inWords(num % 10000000) : "");
    };

    return inWords(n).trim();
  }



  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


}
