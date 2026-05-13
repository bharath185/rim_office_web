import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { leavesService } from '../../service/leaves.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { EmployeeModuleService } from '../../service/employee.service';
import { HrmsServiceService } from '../../hrms-service.service';
import { trigger, style, animate, transition } from '@angular/animations';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { RouterModule } from '@angular/router';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-create-leave',
  standalone: true,
 imports: [ToastMessageComponent, SharedModule, CommonModule, ReactiveFormsModule, 
  NgxPaginationModule,RouterModule],
  animations: [
    trigger('slideToggle', [
      transition(':enter', [
        style({ height: '0', opacity: 0 }),
        animate('0.5s ease', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('0.5s ease', style({ height: '0', opacity: 0 }))
      ])
    ])
  ],
  templateUrl: './create-leave.component.html',
  styleUrl: './create-leave.component.scss'
})
export class CreateLeaveComponent implements OnInit{
@ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild('dropdownWrapper') dropdownWrapper!: ElementRef;
  @ViewChild('locationDropdownWrapper') locationDropdownWrapper!: ElementRef;
  @ViewChild('gradeDropdownWrapper') gradeDropdownWrapper!: ElementRef;

  leaveSettingsForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  employeeDetails;
  accessPolicy:any;
  controlAccessPage:any
  isSpinner: boolean = false;
  errorMessage: any;
  rows: any;
  originalRows: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  viewdata: any;
  isRecordDeleted: boolean = false;
  patchValue: any;
  isEdited: boolean = false;
  getEmployeeTypeList: any[] = [];
  showDropdown = false;
  getLocations: any;
  showLocationDropdown = false;
  getDD_grade: any;
  showGradeDropdown = false;
  isCardOpen = false;

  constructor(private fb: FormBuilder, private leaveSerive: leavesService,
    private readonly hrmsEmpModuleService: EmployeeModuleService, 
    private eRef: ElementRef, private hrmsService: HrmsServiceService,
   private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Create Leave Types'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Create Leave Types'
      );
    });
  }
  isOpen = [true];

  togglePanel(index: number): void {
    this.isOpen[index] = !this.isOpen[index];
  }

  leaves = [
    {
      image: 'https://randomuser.me/api/portraits/men/32.jpg',
      name: 'Anthony Lewis',
      role: 'Finance',
      type: 'Medical Leave',
      from: '14 Jan 2024',
      to: '15 Jan 2024',
      days: '2 Days'
    },
    {
      image: 'https://randomuser.me/api/portraits/men/45.jpg',
      name: 'Brian Villalobos',
      role: 'Developer',
      type: 'Casual Leave',
      from: '21 Jan 2024',
      to: '25 Jan 2024',
      days: '5 Days'
    }

  ];

  ngOnInit(): void {
    this.leaveSettingsForm = this.fb.group({
      levaeType: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      shortName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      durationType: ['', [Validators.required]],
      yearType: ['', [Validators.required]],
      applicableTo: ['', [Validators.required]],
      carryForward: [false],
      encashable: [false],
      maxPerMonth: [null, [Validators.required, Validators.minLength(1), Validators.maxLength(3)]],
      maxPerYear: [null, [Validators.required, Validators.minLength(1), Validators.maxLength(3)]],
      isPaid: [true],
      Location: [[], Validators.required],
      grade: [[]],
      employeeType: [[], Validators.required],
      maxApply: [''],
      applicableDuration: [0],
      maxAllowedEvent: [''],
      singleApplication: [false],
      credit: [],
      month: [false],
      year: [false],
      weekendInc: [false],
      resetYear: [false],
      maxCarryForward: [],
    });
    this.callLocation();
    setTimeout(() => {
      this.getDDEmpTypeList();
      this.DDGrade();
      setTimeout(() => {
        this.getAllLeaveType()
      }, 1000);
    }, 100);

    // Reset related fields when carryForward is unchecked
    this.leaveSettingsForm.get('carryForward')?.valueChanges.subscribe((val: any) => {
      if (!val) {
        this.leaveSettingsForm.patchValue({
          credit: null,
          month: false,
          year: false,
          maxCarryForward: null
        });
      }
    });
    this.leaveSettingsForm.get('month')?.valueChanges.subscribe(() => {
      this.onMonthChange();
    });
    this.leaveSettingsForm.get('year')?.valueChanges.subscribe(() => {
      this.onYearChange();
    });

    // Subscribe to changes in applicableTo
    this.leaveSettingsForm.get('applicableTo')?.valueChanges.subscribe((value: any) => {
      if (value === 'All') {
        this.leaveSettingsForm.get('applicableDuration')?.setValue(0);
      }
      this.toggleApplicableFields(value);
    });
  }

  onMonthChange(): void {
    const monthCtrl = this.leaveSettingsForm.get('month');
    const yearCtrl = this.leaveSettingsForm.get('year');

    if (monthCtrl?.value) {
      yearCtrl?.setValue(false, { emitEvent: false }); // Uncheck "year" if "month" is checked
    }
  }

  onYearChange(): void {
    const monthCtrl = this.leaveSettingsForm.get('month');
    const yearCtrl = this.leaveSettingsForm.get('year');

    if (yearCtrl?.value) {
      monthCtrl?.setValue(false, { emitEvent: false }); // Uncheck "month" if "year" is checked
    }
  }

  toggleApplicableFields(value: string): void {
    const maxAllowedEventCtrl = this.leaveSettingsForm.get('maxAllowedEvent');
    const applicableDurationCtrl = this.leaveSettingsForm.get('applicableDuration');

    if (value === 'Male' || value === 'Female') {
      maxAllowedEventCtrl?.setValidators([Validators.required]);
      applicableDurationCtrl?.clearValidators();
      applicableDurationCtrl?.setValue(null); // Optional: clear field
    } else if (value === 'All') {
      applicableDurationCtrl?.setValidators([Validators.required]);
      maxAllowedEventCtrl?.clearValidators();
      maxAllowedEventCtrl?.setValue(null); // Optional: clear field
    } else {
      maxAllowedEventCtrl?.clearValidators();
      applicableDurationCtrl?.clearValidators();
    }
    maxAllowedEventCtrl?.updateValueAndValidity();
    applicableDurationCtrl?.updateValueAndValidity();
  }

  // this is for location dropdown
  toggleLocationDropdown() {
    this.showLocationDropdown = !this.showLocationDropdown;
  }

  onLocationCheckboxChange(event: any) {
    const selected = this.leaveSettingsForm.get('Location')?.value || [];
    const value = +event.target.value;

    if (value === 0) {
      // "ALL" checkbox clicked
      if (event.target.checked) {
        const allIds = this.getLocations
          .filter((loc: any) => loc.LocationId !== 0)
          .map((loc: any) => loc.LocationId);
        this.leaveSettingsForm.get('Location')?.setValue(allIds);
      } else {
        this.leaveSettingsForm.get('Location')?.setValue([]);
      }
    } else {
      // Individual checkbox clicked
      if (event.target.checked) {
        if (!selected.includes(value)) {
          selected.push(value);
        }
      } else {
        const index = selected.indexOf(value);
        if (index !== -1) {
          selected.splice(index, 1);
        }
      }
      // If all individual items are selected, auto-check "ALL"
      const allIds = this.getLocations
        .filter((loc: any) => loc.LocationId !== 0)
        .map((loc: any) => loc.LocationId);

      const isAllSelected = allIds.every((id: any) => selected.includes(id));

      if (isAllSelected) {
        this.leaveSettingsForm.get('Location')?.setValue(allIds);
      } else {
        this.leaveSettingsForm.get('Location')?.setValue([...selected]);
      }
    }

    this.leaveSettingsForm.get('Location')?.markAsTouched();
  }

  isLocationSelected(id: number): boolean {
    return this.leaveSettingsForm.get('Location')?.value?.includes(id);
  }
  getSelectedLocationsLabel(): string {
    const selectedIds: number[] = this.leaveSettingsForm.get('Location')?.value || [];
    if (!selectedIds.length) {
      return '';
    }
    return this.getLocations
      .filter((loc: any) => selectedIds.includes(loc.LocationId))
      .map((loc: any) => loc.Location)
      .join(', ');
  }
  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const clickedEmployeeDropdown = this.dropdownWrapper?.nativeElement?.contains(event.target);
    const clickedLocationDropdown = this.locationDropdownWrapper?.nativeElement?.contains(event.target);
    const clickedgradeDropdown = this.gradeDropdownWrapper?.nativeElement?.contains(event.target);
    if (!clickedEmployeeDropdown) {
      this.showDropdown = false;
    }
    if (!clickedgradeDropdown) {
      this.showGradeDropdown = false;
    }
    if (!clickedLocationDropdown) {
      this.showLocationDropdown = false;
    }
    const target = event.target as HTMLElement;
    const isDropdown = target.closest('.dropdown-content') !== null;
    const isButton = target.matches('.export-button');
    if (!isDropdown && !isButton) {
      this.dropdownVisible = false;
    }
  }

  //  @HostListener('document:click', ['$event'])
  // onClick(event: MouseEvent) {
  //   const target = event.target as HTMLElement;
  //   const isDropdown = target.closest('.dropdown-content') !== null;
  //   const isButton = target.matches('.export-button');
  //   if (!isDropdown && !isButton) {
  //     this.dropdownVisible = false;
  //   }
  // }
  callLocation() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.isSpinner = true;
    this.hrmsService.employeeGetLocation(reqBody).subscribe(
      (res: any) => {
        if (res.length >= 1) {
          this.getLocations = [
            { LocationId: 0, Location: 'ALL', EmpId: 0 }, // Manually added
            ...res
          ];
          this.isSpinner = false;
        } else {
          this.triggerToast(res['Message'], "No Data Found For Location", "warning");
          this.isSpinner = false;
          this.getLocations = [];
        }
      },
      error => {
        this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
        this.isSpinner = false;
      }
    );
  }
  // this is for location dropdown

  // this is for Empl Type dropdown
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
    console.log('Dropdown toggled:', this.showDropdown);
  }
  onCheckboxChange(event: any) {
    const selected = this.leaveSettingsForm.get('employeeType')?.value || [];
    const value = +event.target.value;
    if (value === 0) {
      // "ALL" clicked
      if (event.target.checked) {
        const allIds = this.getEmployeeTypeList
          .filter(emp => emp.EmpTypeId !== 0)
          .map(emp => emp.EmpTypeId);
        this.leaveSettingsForm.get('employeeType')?.setValue(allIds);
      } else {
        this.leaveSettingsForm.get('employeeType')?.setValue([]);
      }
    } else {
      // Individual item clicked
      if (event.target.checked) {
        if (!selected.includes(value)) {
          selected.push(value);
        }
      } else {
        const index = selected.indexOf(value);
        if (index !== -1) {
          selected.splice(index, 1);
        }
      }
      const allIds = this.getEmployeeTypeList
        .filter(emp => emp.EmpTypeId !== 0)
        .map(emp => emp.EmpTypeId);

      const isAllSelected = allIds.every(id => selected.includes(id));
      if (isAllSelected) {
        this.leaveSettingsForm.get('employeeType')?.setValue(allIds);
      } else {
        this.leaveSettingsForm.get('employeeType')?.setValue([...selected]);
      }
    }
    this.leaveSettingsForm.get('employeeType')?.markAsTouched();
  }

  isSelected(id: number): boolean {
    return this.leaveSettingsForm.get('employeeType')?.value?.includes(id);
  }
  getSelectedEmployeeTypesLabel(): string {
    const selectedIds = this.leaveSettingsForm.get('employeeType')?.value || [];
    return this.getEmployeeTypeList
      .filter(type => selectedIds.includes(type.EmpTypeId))
      .map(type => type.EmpType)
      .join(', ');
  }

  getDDEmpTypeList() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
    };
    this.isSpinner = true;
    this.hrmsEmpModuleService.employeeDDEmpType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getEmployeeTypeList = [
          { EmpTypeId: 0, EmpType: 'ALL' }, // Add manually
          ...res
        ];
      } else {
        this.triggerToast(res['Message'], "No Data Found For Employee Type List", "warning");
        this.getEmployeeTypeList = [];
      }
      this.isSpinner = false;
    }, error => {
      this.triggerToast('Internal Server Error', 'To Load Employee Type List', "danger");
      this.isSpinner = false;
    });
  }
  // this is for Empl Type dropdown

  //this is for DD grade
  toggleGradeDropdown() {
    this.showGradeDropdown = !this.showGradeDropdown;
  }

  onGradeCheckboxChange(event: any) {
    const selected = this.leaveSettingsForm.get('grade')?.value || [];
    const value = +event.target.value;

    if (value === 0) {
      // "ALL" clicked
      if (event.target.checked) {
        const allIds = this.getDD_grade
          .filter((grade: any) => grade.GradeId !== 0)
          .map((grade: any) => grade.GradeId);
        this.leaveSettingsForm.get('grade')?.setValue(allIds);
      } else {
        this.leaveSettingsForm.get('grade')?.setValue([]);
      }
    } else {
      if (event.target.checked) {
        if (!selected.includes(value)) {
          selected.push(value);
        }
      } else {
        const index = selected.indexOf(value);
        if (index !== -1) {
          selected.splice(index, 1);
        }
      }

      const allIds = this.getDD_grade
        .filter((grade: any) => grade.GradeId !== 0)
        .map((grade: any) => grade.GradeId);
      const isAllSelected = allIds.every((id: any) => selected.includes(id));

      if (isAllSelected) {
        this.leaveSettingsForm.get('grade')?.setValue(allIds);
      } else {
        this.leaveSettingsForm.get('grade')?.setValue([...selected]);
      }
    }

    this.leaveSettingsForm.get('grade')?.markAsTouched();
  }

  isGradeSelected(id: number): boolean {
    const selected = this.leaveSettingsForm.get('grade')?.value || [];

    if (id === 0) {
      const allIds = this.getDD_grade
        .filter((grade: any) => grade.GradeId !== 0)
        .map((grade: any) => grade.GradeId);
      return allIds.every((id: any) => selected.includes(id));
    }

    return selected.includes(id);
  }

  getSelectedGradesLabel(): string {
    const selectedIds = this.leaveSettingsForm.get('grade')?.value || [];
    if (!this.getDD_grade || !Array.isArray(this.getDD_grade)) {
      return '';
    }
    const allIds = this.getDD_grade
      .filter(grade => grade.GradeId !== 0)
      .map(grade => grade.GradeId);

    const isAllSelected = allIds.every(id => selectedIds.includes(id));

    if (isAllSelected) {
      return 'ALL';
    }
    return this.getDD_grade
      .filter(grade => selectedIds.includes(grade.GradeId) && grade.GradeId !== 0)
      .map(grade => grade.Grade)
      .join(', ');
  }


  DDGrade() {
    const reqBody = {
      EmpId: this.employeeDetails[0].EmpId,
      GradeId: 0
    }
    this.hrmsService.access_DD_Grade(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getDD_grade = [
          { GradeId: 0, Grade: 'ALL' },
          ...res
        ];
      } else {
        this.getDD_grade = []
      }
    }, error => {
      this.getDD_grade = [];
      this.triggerToast('Internal Server Error', 'To Grade List', 'danger')
    })
  }
  //this is for DD grade

  // this is for GetAllEMployee
  getAllLeaveType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.leaveSerive.GetAllLeaveType(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.isSpinner = false;
        console.log(res);
        this.rows = res;
        this.originalRows = this.rows;
        this.isTableData = false;
      } else {
        this.errorMessage = "No records found";
        this.isSpinner = false;
        this.isTableData = true;
      }
    }, error => {
      this.errorMessage = "Internal Server Error";
      this.isSpinner = false;
      this.isTableData = true;
    })
  }

  dropdownVisible = false;

  toggleDropdownExport() {
    this.dropdownVisible = !this.dropdownVisible;
  }
  // Listen for clicks anywhere in the document

  exportFile(format: string) {
    if (format === 'excel') {
      this.exportToExcel();
    }
    else if (format === 'csv') {
      this.exportToCSV();
    }
    if (format === 'pdf') {
      this.exportToPDF()
    }
  }

  exportToExcel(): void {
    if (!this.rows || this.rows.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
    }
    const filteredData = this.rows.map((item: any) => ({
      "Location": item.Location,
      'Year Type': item.YearType,
      'Leave Type': item.LeaveName,
      'Short Name': item.ShortName,
      "Employee Type": item.EmpType,
      "Employee Level": item.EmpLevel,
      "Period Type": item.DurationType,
      "Applicable To": item.ApplicableTo,
      'Max Per Month': item.MaxPerMonth,
      'Max Per Year': item.MaxPerYear,
      'Max Apply': item.MaxApply,
      "Applicable Period": item.ApplicableDuration,
      "MaxAllowedEvent": item.MaxAllowedEvents,
      // 'Carry Forward': item.CarryForward ? 'Yes' : 'No',
      'Carry Forward': item.CarryForward ? '✔️' : '❌',
      'Encashable': item.Encashable ? 'Yes' : 'No',
      'Paid': item.IsPaid ? 'Yes' : 'No',
      'Single Application': item.IsSingleApplication ? 'Yes' : 'No',
      'Weekend Inclusive': item.WeekEndInclusive ? 'Yes' : 'No',
      'Credit': item.Credit,
      'Year': item.IsYear ? 'Yes' : 'No',
      'Month': item.IsMonth ? 'Yes' : 'No',
      'MaxCarryForward': item.MaxCarryForward,
      'Description': item.Description,
    }));
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(filteredData);
    const workbook: XLSX.WorkBook = { Sheets: { 'Leave Types': worksheet }, SheetNames: ['Leave Types'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blobData = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blobData, 'LeaveTypes.xlsx');
    this.dropdownVisible = false;
  }

  exportToCSV(): void {
    if (!this.rows || this.rows.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
    }
    const filteredData = this.rows.map((item: any) => ({
      "Location": item.Location,
      'Year Type': item.YearType,
      'Leave Type': item.LeaveName,
      'Short Name': item.ShortName,
      "Employee Type": item.EmpType,
      "Employee Level": item.EmpLevel,
      "Period Type": item.DurationType,
      "Applicable To": item.ApplicableTo,
      'Max Per Month': item.MaxPerMonth,
      'Max Per Year': item.MaxPerYear,
      'Max Apply': item.MaxApply,
      "Applicable Period": item.ApplicableDuration,
      "MaxAllowedEvent": item.MaxAllowedEvents,
      'Carry Forward': item.CarryForward ? 'Yes' : 'No',
      'Encashable': item.Encashable ? 'Yes' : 'No',
      'Paid': item.IsPaid ? 'Yes' : 'No',
      'Single Application': item.IsSingleApplication ? 'Yes' : 'No',
      'Weekend Inclusive': item.WeekEndInclusive ? 'Yes' : 'No',
      'Credit': item.Credit,
      'Year': item.IsYear ? 'Yes' : 'No',
      'Month': item.IsMonth ? 'Yes' : 'No',
      'MaxCarryForward': item.MaxCarryForward,
      'Description': item.Description,
    }));

    const replacer = (key: string, value: any) => value ?? ''; // Handle null/undefined
    const header = Object.keys(filteredData[0]);
    const csv = [
      header.join(','),
      ...filteredData.map((row: any) => header.map(fieldName =>
        JSON.stringify(row[fieldName], replacer)).join(','))
    ].join('\r\n');

    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    FileSaver.saveAs(blob, 'LeaveTypes.csv');
    this.dropdownVisible = false;
  }


  exportToPDF(): void {
    if (!this.rows || this.rows.length === 0) {
      this.triggerToast('Sorry', 'No data to export!', 'info');
    }
    const filteredData = this.rows.map((item: any) => [
      item.Location,
      item.YearType,
      item.LeaveName,
      item.ShortName,
      item.EmpType,
      item.EmpLevel,
      item.DurationType,
      item.ApplicableTo,
      item.MaxPerMonth,
      item.MaxPerYear,
      item.MaxApply,
      item.ApplicableDuration,
      item.MaxAllowedEvents,
      item.CarryForward ? 'Yes' : 'No',
      item.Encashable ? 'Yes' : 'No',
      item.IsPaid ? 'Yes' : 'No',
      item.IsSingleApplication ? 'Yes' : 'No',
      item.WeekEndInclusive ? 'Yes' : 'No',
      item.Credit,
      item.IsYear ? 'Yes' : 'No',
      item.IsMonth ? 'Yes' : 'No',
      item.MaxCarryForward,
      item.Description,
    ]);

    const headers = [
      'Location',
      'Year Type',
      'Leave Type',
      'Short Name',
      'Employee Type',
      'Employee Level',
      'Period Type',
      'Applicable To',
      'Max Per Month',
      'Max Per Year',
      'Max Apply',
      'Applicable Period',
      'MaxAllowedEvent',
      'Carry Forward',
      'Encashable',
      'Paid',
      'Single Application',
      'Weekend Inclusive',
      'Credit',
      'Year',
      'Month',
      'MaxCarryForward',
      'Description'
    ];

    // Use A3 format for wider page
    const doc = new jsPDF({
      orientation: 'landscape',
      unit: 'pt',
      format: 'a3',
    });

    autoTable(doc, {
      head: [headers],
      body: filteredData,
      styles: {
        fontSize: 7,
        cellPadding: 3,
        overflow: 'linebreak',
        halign: 'left',
      },
      headStyles: {
        fillColor: [7, 47, 95],  // RGB for #072F5F
        textColor: 255,          // white
        fontStyle: 'bold',
        fontSize: 7,
      },
      alternateRowStyles: { fillColor: [245, 245, 245] },

      didDrawPage: (data) => {
        doc.setFontSize(12);
        doc.text('Leave Type Report', data.settings.margin.left, 20);
      },
    });

    doc.save('LeaveTypes.pdf');
    this.dropdownVisible = false;
  }

  addLeaveType() {
    if (this.leaveSettingsForm.valid) {
      const selectedIds: number[] = this.leaveSettingsForm?.get('grade')?.value || [];

      const selectedGrades: string[] = this.getDD_grade
        .filter((grade: any) => selectedIds.includes(grade.GradeId) && grade.GradeId !== 0)
        .map((grade: any) => grade.Grade);

      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        LeaveName: this.leaveSettingsForm?.get('levaeType').value,
        ShortName: this.leaveSettingsForm?.get('shortName').value,
        Description: this.leaveSettingsForm?.get('description').value,
        DurationType: this.leaveSettingsForm?.get('durationType').value,
        ApplicableTo: this.leaveSettingsForm?.get('applicableTo').value,
        CarryForward: this.leaveSettingsForm?.get('carryForward').value,
        Encashable: this.leaveSettingsForm?.get('encashable').value,
        IsPaid: this.leaveSettingsForm?.get('isPaid').value,
        MaxPerMonth: this.leaveSettingsForm?.get('maxPerMonth').value,
        MaxPerYear: this.leaveSettingsForm?.get('maxPerYear').value,
        EmpTypeId: (this.leaveSettingsForm?.get('employeeType').value || []).join(','),
        LocationId: (this.leaveSettingsForm?.get('Location').value || []).join(','),
        // EmpLevel: (this.leaveSettingsForm?.get('grade').value || []).join(','),
        EmpLevel: selectedGrades.join(','),
        YearType: this.leaveSettingsForm?.get('yearType').value,
        MaxApply: Number(this.leaveSettingsForm?.get('maxApply').value),
        MaxAllowedEvents: Number(this.leaveSettingsForm?.get('maxAllowedEvent').value),
        ApplicableDuration: Number(this.leaveSettingsForm?.get('applicableDuration').value),
        Credit: Number(this.leaveSettingsForm?.get('credit').value),
        WeekEndInclusive: this.leaveSettingsForm?.get('weekendInc').value,
        MaxCarryForward: Number(this.leaveSettingsForm?.get('maxCarryForward').value),
        ResetYear: this.leaveSettingsForm?.get('resetYear').value,
        IsMonth: this.leaveSettingsForm?.get('month').value,
        IsYear: this.leaveSettingsForm?.get('year').value,
        IsSingleApplication: this.leaveSettingsForm?.get('singleApplication').value,
      };
      console.log(reqBody);
      this.isSpinner = true;
      this.leaveSerive.AddLeaveType(reqBody).subscribe({
        next: (res: any) => {
          console.log(res);
          if (res['msg']) {
            this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
            this.getAllLeaveType();
            this.resetData();
            this.isFormSubmitted = false;
          }
          else if (res['Message']) {
            this.triggerToast(res['Message'], 'Failed To Add The Leaves', 'warning');
          }
          this.isSpinner = false;
        }, error: (err: any) => {
          console.log(err);

        }
      })
    } else {
      this.isFormSubmitted = true;
    }

  }

  selectedGrades: number[] = [];
  patchFormWithLeaveData(leave: any): void {
    const empLevelsFromApi = leave.EmpLevel?.split(',') || [];
    this.selectedGrades = empLevelsFromApi.map((gradeName: any) => {
      const matched = this.getDD_grade.find((g: any) => g.Grade === gradeName);
      return matched ? matched.GradeId : null;
    }).filter((id: any) => id !== null); // Remove unmatched ones
    this.leaveSettingsForm.patchValue({
      grade: this.selectedGrades
    });
  }
  getGradeNamesFromIds(): string {
    return this.selectedGrades
      .map(id => {
        const grade = this.getDD_grade.find((g: any) => g.GradeId === id);
        return grade ? grade.Grade : null;
      })
      .filter(name => name !== null)
      .join(',');
  }
  
  toggleButton(){
    this.isCardOpen = !this.isCardOpen
  }
  editPatchData(data: any, edited: boolean) {
    this.patchValue = data;
    this.isEdited = edited;
    this.isCardOpen = true;
    console.log(data);
    this.leaveSettingsForm.patchValue({
      levaeType: data.LeaveName,
      shortName: data.ShortName,
      description: data.Description,
      durationType: data.DurationType,
      applicableTo: data.ApplicableTo,
      carryForward: data.CarryForward,
      encashable: data.Encashable,
      isPaid: data.IsPaid,
      maxPerMonth: data.MaxPerMonth,
      maxPerYear: data.MaxPerYear,
      grade: typeof data.EmpLevel === 'string'
        ? data.EmpLevel.split(',').map((gradeName: any) => {
          const gradeObj = this.getDD_grade.find((g: any) => g.Grade === gradeName.trim());
          return gradeObj ? gradeObj.GradeId : null;
        }).filter((id: any) => id !== null)
        : [],
      employeeType: typeof data.EmpTypeId === 'string'
        ? data.EmpTypeId.split(',').map(Number)
        : Array.isArray(data.EmpTypeId)
          ? data.EmpTypeId.map(Number)
          : [],
      Location: typeof data.LocationId === 'string'
        ? data.LocationId.split(',').map(Number)
        : data.LocationId || [],
      year: data.IsYear,
      month: data.IsMonth,
      resetYear: data.ResetYear,
      maxCarryForward: data.MaxCarryForward,
      weekendInc: data.WeekEndInclusive,
      credit: data.Credit,
      applicableDuration: data.ApplicableDuration,
      maxAllowedEvent: data.MaxAllowedEvents,
      maxApply: data.MaxApply,
      yearType: data.YearType,
      singleApplication: data.IsSingleApplication
    });
  }
  // EmpLevel: (this.leaveSettingsForm?.get('grade').value || []).join(','),
  updateLeaveType() {
    if (this.leaveSettingsForm.valid) {
      this.isSpinner = true;
      // 🛠 Sync selectedGrades with form value
      this.selectedGrades = this.leaveSettingsForm.get('grade')?.value || [];
      const reqBody = {
        LoginId: this.employeeDetails[0].LoginId,
        LeaveTypeId: this.patchValue.LeaveTypeId,
        LeaveName: this.leaveSettingsForm?.get('levaeType').value,
        ShortName: this.leaveSettingsForm?.get('shortName').value,
        Description: this.leaveSettingsForm?.get('description').value,
        DurationType: this.leaveSettingsForm?.get('durationType').value,
        ApplicableTo: this.leaveSettingsForm?.get('applicableTo').value,
        CarryForward: this.leaveSettingsForm?.get('carryForward').value,
        Encashable: this.leaveSettingsForm?.get('encashable').value,
        IsPaid: this.leaveSettingsForm?.get('isPaid').value,
        MaxPerMonth: this.leaveSettingsForm?.get('maxPerMonth').value,
        MaxPerYear: this.leaveSettingsForm?.get('maxPerYear').value,
        EmpTypeId: (this.leaveSettingsForm?.get('employeeType').value || []).join(','),
        LocationId: (this.leaveSettingsForm?.get('Location').value || []).join(','),
        // ✅ Convert GradeIds to Grade names
        EmpLevel: this.getGradeNamesFromIds(),
        YearType: this.leaveSettingsForm?.get('yearType').value,
        MaxApply: Number(this.leaveSettingsForm?.get('maxApply').value),
        MaxAllowedEvents: Number(this.leaveSettingsForm?.get('maxAllowedEvent').value),
        ApplicableDuration: Number(this.leaveSettingsForm?.get('applicableDuration').value),
        Credit: Number(this.leaveSettingsForm?.get('credit').value),
        WeekEndInclusive: this.leaveSettingsForm?.get('weekendInc').value,
        MaxCarryForward: Number(this.leaveSettingsForm?.get('maxCarryForward').value),
        ResetYear: this.leaveSettingsForm?.get('resetYear').value,
        IsMonth: this.leaveSettingsForm?.get('month').value,
        IsYear: this.leaveSettingsForm?.get('year').value,
        IsSingleApplication: this.leaveSettingsForm?.get('singleApplication').value,
      };
      this.leaveSerive.UpdateLeaveType(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === "Updated") {
            this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
            this.isEdited = false;
            this.leaveSettingsForm.reset();
            this.isFormSubmitted = false;
            this.getAllLeaveType();
            this.isSpinner = false;
          } else if (res['Message']) {
            this.triggerToast(res['Message'], res['Message'], 'warning');
            this.isSpinner = false;
          }
        },
        error: (err: any) => {
          console.log(err);
          this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
          this.isSpinner = false;
        }
      });
    } else {
      this.isFormSubmitted = true;
    }
  }

  toggleIsActive(row: any): void {
    row.IsActive = !row.IsActive;
    const payload = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: this.employeeDetails[0].EmpId,
      LeaveTypeId: row.LeaveTypeId,
    };
    this.isSpinner = true;
    // Choose correct API based on new status
    const apiCall = row.IsActive
      ? this.leaveSerive.ActivateLeaveType(payload)
      : this.leaveSerive.DeactivateLeaveType(payload);
    apiCall.subscribe({
      next: (res: any) => {
        if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.getAllLeaveType();
        } else if (res['msg']) {
          this.getAllLeaveType();
          this.triggerToast(`${row.IsActive ? 'Activated' : 'Deactivated'} successfully`, `${row.IsActive ? 'Activated' : 'Deactivated'}`, 'success');
        }
        this.isSpinner = false;

      },
      error: (err) => {
        this.isSpinner = false;
        console.error('API error:', err);
        // Revert toggle on failure
        row.IsActive = !row.IsActive;
        // Optional: show error toast/snackbar
        this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
      }
    });

    console.log(`Toggled IsActive to ${row.IsActive} for`, row);
  }



  resetData() {
    this.leaveSettingsForm.reset();
    this.isEdited = false;
    this.isFormSubmitted = false;
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
  }

  onView(data: any) {
    console.log(data);
    this.viewdata = data;

  }

  deleteLeaveType() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      LeaveTypeId: this.viewdata.LeaveTypeId,
    }
    this.isSpinner = true;
    this.leaveSerive.DeleteLeaveType(reqBody).subscribe({
      next: (res: any) => {
        if(res['msg']){
          this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
          this.isRecordDeleted = true;
          console.log('1')
          setTimeout(() => {
            this.closeModalDelete.nativeElement?.click();
            this.getAllLeaveType()
            setTimeout(() => {
              this.isRecordDeleted = false;
            }, 1100);
          }, 1000);
        }else if (res['Message']) {
          this.triggerToast('', res['Message'], 'warning');
          this.isSpinner = false;
        }
      },
      error: () => {
        this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
        this.isSpinner = false;
        this.errorMessage = "Internal Server Error"
      }
    })
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();
    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) => {
        const leaveName = row.LeaveName?.toUpperCase() || '';
        const durationType = row.DurationType?.toUpperCase() || '';
        const applicableTo = row.ApplicableTo?.toUpperCase() || '';
        const carryForward = row.CarryForward?.toString().toUpperCase() || '';
        const encashable = row.Encashable?.toString().toUpperCase() || '';
        const isPaid = row.IsPaid?.toString().toUpperCase() || '';
        const maxPerMonth = row.MaxPerMonth?.toString().toUpperCase() || '';
        const maxPerYear = row.MaxPerYear?.toString().toUpperCase() || '';
        return (
          leaveName.includes(filterValue) ||
          durationType.includes(filterValue) ||
          applicableTo.includes(filterValue) ||
          carryForward.includes(filterValue) ||
          encashable.includes(filterValue) ||
          isPaid.includes(filterValue) ||
          maxPerMonth.includes(filterValue) ||
          maxPerYear.includes(filterValue)
        );
      });
    } else {
      this.rows = [...this.originalRows];
      this.isTableData = false;
    }
    if (this.rows.length === 0) {
      this.isTableData = true;
      this.errorMessage = 'No Records Found for Searched Data';
      this.rows = [...this.originalRows];
    } else {
      this.isTableData = false;
      this.errorMessage = null;
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
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
}
