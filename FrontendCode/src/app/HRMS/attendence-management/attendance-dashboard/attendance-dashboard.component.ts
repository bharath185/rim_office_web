import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';

@Component({
  selector: 'app-attendance-dashboard',
  standalone: true,
  imports: [CommonModule, SharedModule],
  templateUrl: './attendance-dashboard.component.html',
  styleUrl: './attendance-dashboard.component.scss'
})
export class AttendanceDashboardComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

 annualCTC: number = 0;

  rows: any[] = [];
  benefitsRows: any[] = [];
  deduction: any[] = [];
  isEditing = false;
  editingRowIndex = -1;

  ngOnInit(): void {
    this.rows = [
      { Component: 'Monthly CTC', Formula: 'CTC / 12', Amount: 0, isEditing: false },
      { Component: 'Basic Salary', Formula: '25% of MCTC', Amount: 0, isEditing: false },
      { Component: 'HRA', Formula: '15% of MCTC', Amount: 0, isEditing: false },
      { Component: 'Conveyance', Formula: '10% of MCTC', Amount: 0, isEditing: false },
      // { Component: 'Indian Allowance', Formula: '10% of MCTC', Amount: 0, isEditing: false },
    ];
    this.benefitsRows = [
      { Component: 'Group Insurance', Formula: 'Fixed', Amount: 617, isEditing: false },
      { Component: 'Employer PF', Formula: '12% of Basic', Amount: 0, isEditing: false },
      { Component: 'Employer ESI', Formula: '3.25% of GS', Amount: 0, isEditing: false },
      { Component: 'Gratuity', Formula: '4.81% of BS', Amount: 0, isEditing: false },
      { Component: 'Bonus', Formula: '4.81% of BS', Amount: 0, isEditing: false },
    ];
    this.deduction = [
      { Component: 'PF', Formula: '12% of Basic', Amount: 0, isEditing: false },
      { Component: 'ESI', Formula: '3.25% of GS', Amount: 0, isEditing: false },
      { Component: 'Professional Tax', Formula: '4.81% of BS', Amount: 0, isEditing: false },
      { Component: 'Income Tax', Formula: '4.81% of BS', Amount: 0, isEditing: false },
    ];
  }

  addRow() {
    if (!this.isEditing) {
      this.rows.push({
        Component: '',
        Formula: '',
        Amount: 0,
        isEditing: true
      });
      this.isEditing = true;
      this.editingRowIndex = this.rows.length - 1;
    }
  }

  addBenefitRow() {

  }

  editRow(row: any, index: number) {
    if (!this.isEditing) {
      row.isEditing = true;
      this.isEditing = true;
      this.editingRowIndex = index;
    }
  }

  saveRow(row: any, index: number) {
    row.isEditing = false;
    this.isEditing = false;
    this.editingRowIndex = -1;
    if (this.annualCTC) {
      // Recalculate all rows in order
      this.rows.forEach(r => this.calculateAmount(r));
    }
  }


  cancelEdit(row: any) {
    if (!row.Component.trim()) {
      const idx = this.rows.indexOf(row);
      this.rows.splice(idx, 1);
    } else {
      row.isEditing = false;
    }
    this.isEditing = false;
    this.editingRowIndex = -1;
  }

  removeRow(index: number) {
    this.rows.splice(index, 1);
  }

  onCTCChange(event: any) {
    this.annualCTC = Number(event.target.value) || 0;
    this.rows.forEach(row => this.calculateAmount(row));
  }

  calculateAmount(row: any) {
    if (!this.annualCTC || !row.Formula?.trim()) {
      row.Amount = 0;
      return;
    }

    const formula = row.Formula.trim().toLowerCase();

    try {
      if (formula.includes('% of mctc')) {
        const percent = parseFloat(formula.split('%')[0].trim()) || 0;
        row.Amount = (percent / 100) * (this.annualCTC / 12);

      } else if (/^ctc\s*\/\s*12$/i.test(row.Formula.trim())) {
        row.Amount = this.annualCTC / 12;

      } else if (formula.includes('fixed')) {
        row.Amount = 617;
      } else if (formula.includes('% of bs') || formula.includes('% of basic')) {
        const basicRow = this.rows.find(r => r.Component.toLowerCase() === 'basic salary');
        const basic = basicRow?.Amount || 0;
        const percent = parseFloat(formula.split('%')[0].trim()) || 0;
        row.Amount = (percent / 100) * basic;
      } else if (formula.includes('% of gs')) {
        const gs = this.rows
          .slice(0, this.rows.indexOf(row))
          .reduce((sum, r) => sum + (r.Amount || 0), 0);
        const percent = parseFloat(formula.split('%')[0].trim()) || 0;
        row.Amount = (percent / 100) * gs;

      } else {
        row.Amount = 0;
      }
    } catch (error) {
      console.error('Error calculating formula:', formula, error);
      row.Amount = 0;
    }
  }


  downloadPDF() {
    const doc = new jsPDF('p', 'pt', 'a4');
    const pageWidth = doc.internal.pageSize.getWidth();

    // load the logo image
    const logo = new Image();
    logo.src = './assets/Logo3DCAD.png';

    logo.onload = () => {
      // ===== HEADER =====
      const margin = 6;
      const headerHeight = 110;
      doc.setFillColor(245, 245, 245);
      doc.rect(margin, 0, pageWidth - 2 * margin, headerHeight, 'F'
      );

      try {
        doc.addImage(logo, 'PNG', margin + 30, 36, 110, 45);
      } catch (e) { }

      // right-side company address block
      doc.setFontSize(9);
      doc.setTextColor(1);
      const addressX = pageWidth - 30;
      const addressLines = [
        '3D Concept Analysis & Development India Pvt Ltd',
        'Sapthagiri Towers, #12,60Feet Road,',
        'NHBC Layout, Prashanth Nagar,',
        'Bangalore-560079',
        'Phone : +91-80-46504500',
        'Fax : +91 80 42459595',
        'E-mail : india@3dcad-global.com'
      ];
      let ay = 30;
      addressLines.forEach((line) => {
        doc.text(line, addressX, ay, { align: 'right' });

        // If this is the email line, draw the line to its left
        if (line.startsWith('E-mail')) {
          const emailText = line;
          const emailY = ay; // same Y as text
          const emailX = pageWidth - 30; // right-aligned X
          const textWidth = doc.getTextWidth(emailText);

          doc.setDrawColor(0, 112, 192); // blue
          doc.setLineWidth(1);

          // Draw line from left margin (e.g., 40) to start of email text
          const startX = 40;
          const endX = emailX - textWidth; // stop at start of email text
          doc.line(startX, emailY + 1, endX, emailY + 1);
        }

        ay += 12.5; // move to next line
      });

      // Title line (positioned similar to provided image)
      doc.setFontSize(11);
      doc.setFont('helvetica', 'bold');
      doc.text('Salary Slip for the month :', 40, 135);
      doc.setFont('helvetica', 'normal');
      doc.text('January - 2025', 300, 135);

      // ===== EMPLOYEE DETAILS (two-column style up to ESI) =====
      const empHead = [['Name', 'GIRISH J', 'Designation', 'Junior Design Engineer']];
      const empBody = [
        ['EmpNo', '3DCAD-971', 'Location', 'Bangalore'],
        ['PAN No', 'DJMPG1331N', 'Bank A/c No', '736301501298'],
        ['PFNo', '/PNY/30785PYPNY00307850000011420', 'Days Paid', '31'],
        ['UAN No', '101702697980', 'LOP', '0'],
        ['ESI No', '5347593254', '', '']
      ];

      autoTable(doc, {
        startY: 150,
        head: empHead,
        body: empBody,
        theme: 'plain',
        styles: {
          fontSize: 9,
          cellPadding: 6,
          textColor: 20,
          overflow: 'linebreak'
        },
        headStyles: {
          fillColor: [255, 255, 255],
          textColor: 0,
          fontStyle: 'bold',
          halign: 'right',
        },
        bodyStyles: {
          fillColor: [230, 230, 230],
          textColor: 20
        },
        columnStyles: {
          0: { halign: 'left' },
          1: { halign: 'right' },
          2: { halign: 'left' },
          3: { halign: 'right' }
        },
        margin: { left: 5, right: 5 },
        tableLineColor: [0, 0, 0],
        tableLineWidth: 1,
        didDrawCell: (data) => {
          const { cell, column, row } = data;
          if (!cell) return;
          const table: any = (data as any).table;
          doc.setDrawColor(1);
          doc.setLineWidth(0.5);

          if (row.index === 0 && column.index === 0) {
            try {
              doc.rect(table.startX, table.headRow.y, table.width, table.height, 'S');
            } catch (e) {
            }
          }
          // doc.line(cell.x + cell.width, cell.y, cell.x + cell.width, cell.y + cell.height);
          if (row.index === 0) {
            doc.line(cell.x, cell.y, cell.x + cell.width, cell.y);
          }
          if (column.index === 2) {
            doc.line(cell.x, cell.y, cell.x, cell.y + cell.height);
          }
        }
      });

      // ===== EARNINGS & DEDUCTIONS: start exactly at previous finalY so they join =====
      const startY = (doc as any).lastAutoTable.finalY;

      const earnings: any[] = [
        ['Basic Salary', '8,750.00', '0.00', 'Employee PF Contribution', '1,050.00', '0.00'],
        ['Indian Allowance', '15,412.00', '0.00', 'Professional Tax', '200.00', '0.00'],
        ['HRA', '5,250.00', '0.00', 'Income Tax', '0.00', '0.00'],
        ['Conveyance Allowance', '3,500.00', '0.00', 'Employee ESI Contribution', '0.00', '0.00'],
        ['Bonus', '0.00', '0.00', 'VPF Contribution', '0.00', '0.00'],
        ['Special Project Allowance', '0.00', '0.00', 'Transport Deduction', '0.00', '0.00'],
        ['Medical Reimbursement', '0.00', '0.00', 'Employee Welfare Fund', '0.00', '0.00'],
        ['Shift Allowance', '0.00', '0.00', 'Salary Advance', '0.00', '0.00'],
        ['Leave Travel Allowance', '0.00', '0.00', 'Other Deduction', '0.00', '0.00'],
        ['Gross Salary', '32,912.00', '0.00', 'Total Deduction', '1,250.00', '0.00'],
        [
          {
            content: 'Net Salary Rs. 31,662.00',
            colSpan: 3,
            styles: { fontStyle: 'bold' as const, halign: 'left' as const }
          },
          {
            content: '(Thirty-One Thousand Six Hundred Sixty-Two Only)',
            colSpan: 3,
            styles: { fontStyle: 'italic' as const, halign: 'left' as const }
          }
        ]
      ];

      autoTable(doc, {
        startY: startY,
        head: [['Earnings', 'Current', 'Arrear', 'Deductions', 'Current', 'Arrear']],
        body: earnings,
        theme: 'plain',
        styles: {
          fontSize: 9,
          cellPadding: 6,
          valign: 'middle',
          textColor: 20,

        },
        headStyles: {
          fillColor: [255, 255, 255],
          textColor: 0,
          fontStyle: 'bold',
          // halign: 'center' as const
        },
        bodyStyles: {
          fillColor: [255, 255, 255], // white rows by default
          textColor: 20
        },
        columnStyles: {
        },
        margin: { left: 5, right: 5 },
        tableLineColor: [0, 0, 0],
        tableLineWidth: 0.5,
        didParseCell: (data) => {
          const { row, column, cell } = data;
          if (row.section === 'body') {
            if (Array.isArray(row.raw) && typeof row.raw[0] === 'object') return;
            if ([1, 2, 4, 5].includes(column.index)) {
              cell.styles.fillColor = [230, 230, 230];
            }
          }
        },

        didDrawCell: (data) => {
          const { cell, row, column, table } = data;
          if (!cell) return;

          const raw: any = row.raw;

          const x1 = cell.x;
          const y1 = cell.y;
          const x2 = cell.x + cell.width;
          const y2 = cell.y + cell.height;

          // 1) Vertical borders for all cells
          doc.setDrawColor(0);
          doc.setLineWidth(0.5);
          // Left border (first column)
          if (column.index === 0) doc.line(x1, y1, x1, y2);
          if (column.index === 1) doc.line(x1, y1, x1, y2);
          if (column.index === 2) doc.line(x1, y1, x1, y2);
          if (column.index === 3) doc.line(x1, y1, x1, y2);
          if (column.index === 4) doc.line(x1, y1, x1, y2);
          if (column.index === 5) doc.line(x1, y1, x1, y2);

          // 2) Horizontal border ONLY for special rows
          const t: any = table; // bypass TS typing
          const fullX1 = t.startX ?? 5;
          const fullX2 = t.startX != null && t.width != null ? t.startX + t.width : x2;
          doc.setLineWidth(1); 
          if (Array.isArray(raw) && raw[0] === "Earnings") {
            doc.line(fullX1, y2, fullX2, y2); // bottom
          }

           if (Array.isArray(raw) && raw[0] === "Deductions") {
            doc.line(fullX1, y2, fullX2, y2); // bottom
          }

          if (Array.isArray(raw) && raw[0] === "Gross Salary") {
            doc.line(fullX1, y1, fullX2, y1); // top
            doc.line(fullX1, y2, fullX2, y2); // bottom
          }

          if (Array.isArray(raw) && raw[3] === "Total Deduction") {
            doc.line(fullX1, y1, fullX2, y1); // top
            doc.line(fullX1, y2, fullX2, y2); // bottom
          }

          if (
            Array.isArray(raw) &&
            raw[0] &&
            typeof raw[0] === "object" &&
            typeof raw[0].content === "string" &&
            raw[0].content.startsWith("Net Salary")
          ) {
            doc.line(fullX1, y1, fullX2, y1); // top
            doc.line(fullX1, y2, fullX2, y2); // bottom
          }
        }
      });

      // ===== FOOTER/NOTES =====
      const footerY = (doc as any).lastAutoTable.finalY + 20;
      doc.setFontSize(9);
      doc.text('** Note:', 40, footerY);
      doc.text('(Figures in INR)', 115, footerY);
      doc.text('Generated On: 29/10/2025', 40, footerY + 22);

      // Save
      doc.save('Salary_Slip_January_2025.pdf');
    };

    logo.onerror = () => {
      console.warn('Logo load failed; continuing without logo.');
      // you could call a fallback path to build same pdf without waiting for onload
    };
  }

  
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }

}
