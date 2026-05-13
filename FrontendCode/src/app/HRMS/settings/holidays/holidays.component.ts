import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { SettingsService } from '../../service/settings.service';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Modal } from 'bootstrap';
import { Dropdown } from 'bootstrap';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-holidays',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent, NgxPaginationModule, RouterModule],
  templateUrl: './holidays.component.html',
  styleUrl: './holidays.component.scss'
})
export class HolidaysComponent {

  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild('closeModal') closeModal!: ElementRef;
  @ViewChild('closeModalDelete') closeModalDelete!: ElementRef;

  @ViewChild('dropdownBtn') dropdownBtn: any = ElementRef;
  @ViewChild('dropdownMenu') dropdownMenu: any = ElementRef;

  @ViewChild('dropdownLocationBtn') dropdownLocationBtn: any = ElementRef;
  @ViewChild('dropdownLocationMenu') dropdownLocationMenu: any = ElementRef;

  accessPolicy:any;
  controlAccessPage:any;
  isSpinner: boolean = false;
  holidaysForm: any = FormGroup;
  isFormSubmitted: boolean = false;
  isEdited: boolean = false;
  getEditdata: any;
  employeeDetails;
  getLocations: any;
  rows: any;
  originalRows: any;
  isTableData: boolean = false;
  page = 1;
  pageSize = 10;
  pageSizes = [10, 15, 20];
  isRecordDeleted: boolean = false;
  errorMessage: any;
  viewdata: any;
  dropdownInstance: any = Dropdown;
  selectedDays: string[] = [];
  selectedLocation: string[] = [];
  patchSelectedDays: string[] = [];
  selectedLocationIds: number[] = [];
  dropdownLocationInstance: any = Dropdown;
  getYears: any[] = []
  currentYear: number = new Date().getFullYear();
  currentMonth: number = new Date().getMonth();
  weekDays: string[] = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  monthNames: string[] = [
    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
  ];
  weekDaysDDForm: string[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  years: number[] = [];
  calendar: number[][] = [];
  viewMode: 'list' | 'calendar' = 'list';
  showDateField = true;
  showWeeksField = true;
  showTableDropdowm = true;
  // this is AllGeneral Holiday List start
  holidaysMap: Map<string, { title: string; type: string }[]> = new Map();
  isDatePresent: boolean = false;
  isDayPresent: boolean = false;
  constructor(private readonly fb: FormBuilder, private readonly settingService: SettingsService,
    private readonly hrmsService: HrmsServiceService, private cdr: ChangeDetectorRef,
    private accessPolicyStoreService: AccessPolicyStoreService,private route: ActivatedRoute,
  ) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    // const accessPolicy = sessionStorage.getItem('accessPolicy');
    // this.accessPolicy = accessPolicy ? JSON.parse(accessPolicy) : null;
    // const viewEmployeeAccess = this.accessPolicy.find(
    //   (item: any) => item.PageName === 'Holidays'
    // );
    // this.controlAccessPage = viewEmployeeAccess;
    // console.log(this.controlAccessPage);
    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Holidays'
      );
    });
  }

  ngOnInit(): void {
    // this.employeeGetAllHolidays('General Holidays');
    // this.callLocation();
    // this.GetAllFinanceMaster();
    this.route.queryParams.subscribe(params => {
      if (params['openModal'] === 'true') {
        setTimeout(() => {
          this.openModal();
        }, 0);
      }
    });
    setTimeout(() => {
      this.employeeGetAllHolidays('General Holidays');
      setTimeout(() => {
        this.callLocation();
        setTimeout(() => {
          this.GetAllFinanceMaster();
        }, 100);
      }, 100);
    }, 100);
    this.generateYears();
    this.generateCalendar();
    this.holidaysForm = this.fb.group({
      date: ['', [Validators.required]],
      titleName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      Description: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      Location: ['', [Validators.required]],
      year: ['', [Validators.required]],
      holidayTypes: ['', [Validators.required]],
      selectDays: ['', []],
    });

  }
  callLocation() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.hrmsService.employeeGetLocation(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.holidaysForm?.get('Location').reset();
        this.getLocations = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Location", "warning");
        this.isSpinner = false;
        this.getLocations = []
      }
    },
      error => {
        // this.errorMessage = 'Error loading data. Please try again later.';
        this.triggerToast('Internal Server Error', 'Error loading data. Location', "danger");
        this.isSpinner = false;
      })
  }

  GetAllFinanceMaster() {
    const reqBody = {
      Id: 1,
    }
    this.isSpinner = true;
    this.settingService.employeeGetAllFinanceMaster(reqBody).subscribe((res: any) => {
      if (res.length >= 1) {
        this.getYears = res;
        this.isSpinner = false;
      } else {
        this.triggerToast(res['Message'], "No Data Found For Years", "warning");
        this.isSpinner = false;
        this.getYears = []
      }
    },
      error => {
        this.triggerToast('Internal Server Error', 'Error loading data. Years', "danger");
        this.isSpinner = false;
      })
  }

  getHolidayTitle(day: number): { title: string; type: string }[] | null {
    const date = new Date(this.currentYear, this.currentMonth, day);
    const dateStr = date.toISOString().split('T')[0];
    return this.holidaysMap.get(dateStr) || null;
  }


  employeeGetAllHolidays(holidayType: string) {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.settingService.employeeGetAllHolidays(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          setTimeout(() => {
            const formatted = res.map((row: any) => {
              row.Date = this.formatDate(this.parseJsonDate(row.Date));
              return row;
            });
            // Filter here based on the passed holidayType
            this.isDayPresent = false;
            this.rows = formatted.filter((row: any) => row.HolidayType === holidayType);
            this.isDatePresent = this.rows.some((row: any) => row.Date != null);
            this.originalRows = this.rows; // Save if needed for further logic
            this.isSpinner = false;
            this.isTableData = false;
          }, 1000);

          this.holidaysMap = new Map<string, { title: string, type: string }[]>();
          res.forEach((row: any) => {
            const dateObj = new Date(+row.Date.match(/\d+/)[0]);
            const dateStr = dateObj.toISOString().split('T')[0];
            const holidayEntry = { title: row.Title, type: row.HolidayType };

            if (!this.holidaysMap.has(dateStr)) {
              this.holidaysMap.set(dateStr, [holidayEntry]);
            } else {
              this.holidaysMap.get(dateStr)?.push(holidayEntry);
            }
          });


        } else {
          this.errorMessage = "No records found";
          this.isSpinner = false;
          this.isTableData = true;
        }
      },
      error: (error: any) => {
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
      }
    });
  }
  // this is AllGeneral Holiday List End

  //this is AllWeekly Holiday Start
  employeeGetAllWeekHolidays() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
    }
    this.isSpinner = true;
    this.settingService.employeeGetAllWeekHolidays(reqBody).subscribe({
      next: (res: any) => {
        if (res.length >= 1) {
          setTimeout(() => {
            this.rows = res;
            this.originalRows = res;
            this.isDatePresent = false;
            this.isDayPresent = this.rows.some((row: any) => row.Date == null);
            this.isSpinner = false;
            this.isTableData = false;
          }, 1000);
        } else {
          this.errorMessage = "No records found";
          this.isSpinner = false;
          this.isTableData = true;
        }
      },
      error: (error: any) => {
        this.errorMessage = "Internal Server Error";
        this.isSpinner = false;
        this.isTableData = true;
      },
      complete: () => {
      }
    })
  }
  //this is AllWeekly Holiday

  // *****************this is for Calender Purpose*****************
  generateYears(): void {
    const start = 2024;
    const end = 2050;
    for (let y = start; y <= end; y++) {
      this.years.push(y);
    }
  }
  generateCalendar(): void {
    const firstDay = new Date(this.currentYear, this.currentMonth, 1).getDay();
    const totalDays = new Date(this.currentYear, this.currentMonth + 1, 0).getDate();
    const calendar: number[][] = [];
    let week: number[] = Array(firstDay).fill(0);
    for (let i = 1; i <= totalDays; i++) {
      week.push(i);
      if (week.length === 7) {
        calendar.push(week);
        week = [];
      }
    }
    if (week.length) {
      while (week.length < 7) {
        week.push(0);
      }
      calendar.push(week);
    }
    this.calendar = calendar;
  }
  get flatCalendar(): number[] {
    return this.calendar.flat();
  }
  prevMonth(): void {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }
    this.generateCalendar();
  }
  nextMonth(): void {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.generateCalendar();
  }
  getDayClass(day: number): string {
    if (!day) return '';

    const date = new Date(this.currentYear, this.currentMonth, day);
    const today = new Date();
    const isToday =
      date.getFullYear() === today.getFullYear() &&
      date.getMonth() === today.getMonth() &&
      date.getDate() === today.getDate();
    const isWeekend = date.getDay() === 0 || date.getDay() === 6;

    const dateStr = date.toISOString().split('T')[0];
    const holidayEntries = this.holidaysMap.get(dateStr);

    let holidayClass = '';
    if (holidayEntries && holidayEntries.length) {
      const isGeneral = holidayEntries.some((e: any) => e.type === 'General Holidays');
      const isRH = holidayEntries.some((e: any) => e.type === 'RH Holidays');

      if (isGeneral) {
        holidayClass = 'general-holiday';
      } else if (isRH) {
        holidayClass = 'rh-holiday';
      }
    }

    return `${isWeekend ? 'bg-light-warning' : ''} ${isToday ? 'today-badge' : ''} ${holidayClass}`;
  }

  isToday(day: number): boolean {
    if (!day) return false;
    const date = new Date(this.currentYear, this.currentMonth, day);
    const today = new Date();
    return (
      date.getFullYear() === today.getFullYear() &&
      date.getMonth() === today.getMonth() &&
      date.getDate() === today.getDate()
    );
  }
  onMonthInput(event: any) {
    const [year, month] = event.target.value.split('-');
    this.currentYear = +year;
    this.currentMonth = +month - 1;
    this.generateCalendar();
  }
  setView(mode: 'list' | 'calendar') {
    this.viewMode = mode;
    if (mode === 'calendar') {
      this.showTableDropdowm = false;
    } else {
      this.showTableDropdowm = true;
    }
  }
  getEventsForDay(day: number): { title: string, type: string }[] {
    if (!day) return [];
    const dateStr = new Date(this.currentYear, this.currentMonth, day)
      .toISOString()
      .split("T")[0];
    return this.holidaysMap.get(dateStr) || [];
  }

  // this for top Calender and list purpose

  //this is for weelky days list
  toggleDropdown() {
    const dropdownElement = this.dropdownBtn.nativeElement;
    this.dropdownInstance = Dropdown.getOrCreateInstance(dropdownElement);
    this.dropdownInstance.toggle();
  }
  onCheckboxChange(event: any) {
    const value = event.target.value;
    if (event.target.checked) {
      this.selectedDays.push(value);
    } else {
      this.selectedDays = this.selectedDays.filter(day => day !== value);
    }
    this.holidaysForm?.get('selectDays').setValue(this.selectedDays);
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent): void {
    const clickedInsideDaysDropdown =
      this.dropdownBtn?.nativeElement.contains(event.target) ||
      this.dropdownMenu?.nativeElement.contains(event.target);

    const clickedInsideLocationDropdown =
      this.dropdownLocationBtn?.nativeElement.contains(event.target) ||
      this.dropdownLocationMenu?.nativeElement.contains(event.target);
    if (!clickedInsideDaysDropdown && this.dropdownInstance && typeof this.dropdownInstance.hide === 'function') {
      this.dropdownInstance.hide();  // Close the Days dropdown
    }
    if (!clickedInsideLocationDropdown && this.dropdownLocationInstance && typeof this.dropdownLocationInstance.hide === 'function') {
      this.dropdownLocationInstance.hide();  // Close the Location dropdown
    }
  }
  //this is for weelky days list

  //this is for Location list
  toggleLocationDropdown(): void {
    const dropdownElement = this.dropdownLocationBtn.nativeElement;
    this.dropdownLocationInstance = Dropdown.getOrCreateInstance(dropdownElement);
    this.dropdownLocationInstance.toggle();
  }
  onLocationCheckboxChange(event: any): void {
    const value = event.target.value;
    const id = parseInt(event.target.id);
    if (event.target.checked) {
      this.selectedLocation.push(value);
      this.selectedLocationIds.push(id);
    } else {
      this.selectedLocation = this.selectedLocation.filter(location => location !== value);
      this.selectedLocationIds = this.selectedLocationIds.filter(locationId => locationId !== id);
    }
    this.holidaysForm?.get('Location')?.setValue(this.selectedLocation);
  }
  //this is for Location list

  getSelectedHoliday(event: any) {
    const selectedType = event.target.value;
    if (selectedType === 'Weekly Holidays') {
      this.employeeGetAllWeekHolidays(); // Call separate API for weekly holidays
    } else {
      this.employeeGetAllHolidays(selectedType); // Call API and filter
    }
  }


  selectedHolidayForm(event: any) {
    const selectedValue = event.target.value;
    const dateControl = this.holidaysForm.get('date');
    const selectDaysControl = this.holidaysForm.get('selectDays');
    if (selectedValue === 'Weekly Holidays') {
      this.showDateField = false;
      this.showWeeksField = true;
      dateControl?.clearValidators();
      dateControl?.setValue('');
      selectDaysControl?.setValidators([Validators.required]);
    } else {
      this.showDateField = true;
      this.showWeeksField = false;
      dateControl?.setValidators([Validators.required]);
      // selectDaysControl?.setValidators([Validators.required]);
      selectDaysControl?.clearValidators();
      selectDaysControl?.setValue('');
    }
    dateControl?.updateValueAndValidity();
    selectDaysControl?.updateValueAndValidity();
  }

  submitFormdata() {
    if (this.holidaysForm.valid) {
      if (this.holidaysForm?.get('holidayTypes').value != 'Weekly Holidays') {
        const reqBody = {
          Created_By: this.employeeDetails[0].LoginId,
          Status: "Active",
          HolidayLocationId: this.selectedLocationIds,
          HolidayLocation: this.selectedLocation,
          Title: this.holidaysForm?.get('titleName').value,
          Description: this.holidaysForm?.get('Description').value,
          Year: this.holidaysForm?.get('year').value,
          HolidayType: this.holidaysForm?.get('holidayTypes').value,
          Date: this.holidaysForm?.get('date').value,
        };
        console.log(reqBody)
        this.isSpinner = true;
        this.settingService.EmployeeAddHoliday(reqBody).subscribe({
          next: (res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
              this.resetData()
              setTimeout(() => {
                this.closeModal.nativeElement?.click();
                this.employeeGetAllHolidays('General Holidays');
              }, 100);
              this.isFormSubmitted = false;
            } else if (res['Message']) {
              this.triggerToast(res['Message'], 'Failed To Add The Holidays', 'warning');
            }
            this.isSpinner = false;
          },
          error: () => {
            this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
            this.isSpinner = false;
          }
        });
      } else {
        const reqBody = {
          Day: this.selectedDays,
          Created_By: this.employeeDetails[0].LoginId,
          Status: "Active",
          LocationId: this.selectedLocationIds,
          Location: this.selectedLocation,
          Title: this.holidaysForm?.get('titleName').value,
          Description: this.holidaysForm?.get('Description').value,
          Year: this.holidaysForm?.get('year').value,
          HolidayType: this.holidaysForm?.get('holidayTypes').value
        };
        console.log(reqBody)
        this.isSpinner = true;
        this.settingService.EmployeeAddHoliday(reqBody).subscribe({
          next: (res: any) => {
            if (res['msg']) {
              this.triggerToast(res['msg'], 'Record Added Successfully', 'success');
              this.holidaysForm?.reset();
              setTimeout(() => {
                this.closeModal.nativeElement?.click();
                this.employeeGetAllWeekHolidays();
              }, 100);
              this.isFormSubmitted = false;
            } else if (res['Message']) {
              this.triggerToast(res['Message'], 'Failed To Add The Holidays', 'warning');
            }
            this.isSpinner = false;
          },
          error: () => {
            this.triggerToast('Internal Server Error', 'Failed To Add Record', 'danger');
            this.isSpinner = false;
          }
        });
      }

    } else {
      this.isFormSubmitted = true;
      this.triggerToast('Invalid', 'Please Fill The * Marked Filled', 'warning');
    }
  }

  editData(data: any, edited: boolean) {
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
    this.getEditdata = data;
    console.log(data);
    this.isEdited = edited;
    this.selectedLocation = [];
    this.selectedLocationIds = [];
    if (this.getEditdata.HolidayType === 'RH Holidays' || this.getEditdata.HolidayType === 'General Holidays') {
      this.holidaysForm?.get('holidayTypes').patchValue(this.getEditdata?.HolidayType);
      this.holidaysForm?.get('date').patchValue(this.getEditdata?.Date);
      this.holidaysForm?.get('Location').patchValue(this.getEditdata?.HolidayLocation);
      console.log('gen & rh holiday')
      this.selectedLocation = [...this.getEditdata?.HolidayLocation];
      this.selectedLocationIds = [...this.getEditdata?.HolidayLocationId];
      this.showWeeksField = false;
    } else {
      this.holidaysForm?.get('holidayTypes').patchValue('Weekly Holidays');
      let rawDays = this.getEditdata?.Day;
      if (rawDays.length === 1 && typeof rawDays[0] === 'string' && rawDays[0].includes(',')) {
        this.selectedDays = rawDays[0].split(',').map(d => d.trim());
      } else {
        this.selectedDays = [...rawDays];
      }
      this.cdr.detectChanges();
      this.holidaysForm?.get('Location').patchValue(this.getEditdata?.Location);
      console.log('weekly holiday')
      this.selectedLocation = [...this.getEditdata?.Location];
      this.selectedLocationIds = [...this.getEditdata?.LocationId];
      this.showDateField = false;
      this.showWeeksField = true;
    }

    this.holidaysForm?.get('titleName').patchValue(this.getEditdata?.Title);
    this.holidaysForm?.get('Description').patchValue(this.getEditdata?.Description);
    this.holidaysForm?.get('year').patchValue(this.getEditdata?.Year);
  }


  updateholidaysForm() {
    console.log(this.holidaysForm.value)
    if (this.holidaysForm?.get('holidayTypes').value != 'Weekly Holidays') {
      console.log('not weekly')
      if (this.holidaysForm.valid) {
        const reqBody = {
          Created_By: this.employeeDetails[0].LoginId,
          Modify_By: this.employeeDetails[0].LoginId,
          Holiday_Id: this.getEditdata.Holiday_Id,
          Status: "Active",
          HolidayLocationId: this.selectedLocationIds,
          HolidayLocation: this.selectedLocation,
          Title: this.holidaysForm?.get('titleName').value,
          Description: this.holidaysForm?.get('Description').value,
          Year: this.holidaysForm?.get('year').value,
          HolidayType: this.holidaysForm?.get('holidayTypes').value,
          Date: this.holidaysForm?.get('date').value,
        };
        console.log(reqBody)
        this.isSpinner = true;
        this.settingService.employeeUpdateHoliday(reqBody).subscribe({
          next: (res: any) => {
            if (res['msg'] === null || res['msg']) {
              this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
              this.holidaysForm.reset();
              this.isEdited = false;
              setTimeout(() => {
                this.closeModal.nativeElement?.click();
                this.employeeGetAllHolidays('General Holidays');
              }, 100);
              this.isFormSubmitted = false;
            } else if (res['Message']) {
              this.triggerToast('Failed To Update', res['Message'], 'warning');
            }
            this.isSpinner = false;
          },
          error: (err) => {
            this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
            this.isSpinner = false;
          }
        });
      }
      else {
        this.isFormSubmitted = true;
        this.triggerToast('Invalid', 'Please Fill The * Marked Filled', 'warning');
      }
    }
    else if (this.holidaysForm?.get('holidayTypes').value === 'Weekly Holidays') {
      console.log(' weekly')
      const dateControl = this.holidaysForm.get('date');
      dateControl?.clearValidators();
      dateControl?.setValue('');
      dateControl?.updateValueAndValidity();
      const reqBody = {
        Day: this.selectedDays,
        WeekDay_ID: this.getEditdata.WeekDay_ID,
        Created_By: this.employeeDetails[0].LoginId,
        Modified_By: this.employeeDetails[0].LoginId,
        Status: "Active",
        LocationId: this.selectedLocationIds,
        Location: this.selectedLocation,
        Title: this.holidaysForm?.get('titleName').value,
        Description: this.holidaysForm?.get('Description').value,
        Year: this.holidaysForm?.get('year').value,
        HolidayType: this.holidaysForm?.get('holidayTypes').value
      };
      console.log(reqBody);
      this.isSpinner = true;
      this.settingService.employeeUpdateWeekHoliday(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === null || res['msg']) {
            this.triggerToast(res['msg'], 'Records Updated Successfully', 'success');
            this.holidaysForm.reset();
            setTimeout(() => {
              this.closeModal.nativeElement?.click();
              this.employeeGetAllWeekHolidays();
            }, 100);
            this.isEdited = false;
            this.isFormSubmitted = false;
          } else if (res['Message']) {
            this.triggerToast('Failed To Update', res['Message'], 'warning');
          }
          this.isSpinner = false;
        },
        error: (err) => {
          this.triggerToast('Internal Server Error', 'Failed To Update Records', 'danger');
          this.isSpinner = false;
        }
      });
    }
  }

  onView(data: any) {
    console.log(data);
    this.viewdata = data
  }

  deleteHolidaysData() {
    if (this.viewdata.HolidayType === 'RH Holidays' || this.viewdata.HolidayType === 'General Holidays') {
      const reqBody = {
        Holiday_Id: this.viewdata.Holiday_Id,
        LoginId: this.viewdata.Created_By,
        LocationId: this.viewdata.LocationId,
        Status: "InActive"
      };
      this.isSpinner = true;
      this.settingService.employeeDeleteHoliday(reqBody).subscribe({
        next: (res: any) => {
          if (res['msg'] === null || res['msg']) {
            this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
            this.isRecordDeleted = true;
            console.log('1')
            setTimeout(() => {
              this.closeModalDelete.nativeElement?.click();
              this.employeeGetAllHolidays(this.viewdata.HolidayType)
              setTimeout(() => {
                this.isRecordDeleted = false;
              }, 1100);
            }, 1000);
          } else if (res['Message']) {
            this.triggerToast('Failed to delete', res['Message'], 'warning');
            console.log('2')
          } else {
            this.triggerToast(res['msg'], 'Something went wrong', 'warning');
            console.log('1')
          }
          this.isSpinner = false;
        },
        error: () => {
          this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
          this.isSpinner = false;
          console.log('4')
        }
      });
    }
    else {
      const reqBody = {
        WeekDay_ID: this.viewdata.WeekDay_ID,
        LoginId: this.viewdata.Created_By,
        LocationId: this.viewdata.LocationId,
        Status: "InActive"
      };
      this.isSpinner = true;
      this.settingService.employeeDeleteWeekHoliday(reqBody).subscribe({
        next: (res: any) => {
          if (res) {
            this.triggerToast(res['msg'], 'Record Deleted Successfully', 'success');
            this.isRecordDeleted = true;
            setTimeout(() => {
              this.closeModalDelete.nativeElement?.click();
              this.employeeGetAllWeekHolidays();
              setTimeout(() => {
                this.isRecordDeleted = false;
              }, 1100);
            }, 1000);
          } else if (res['Message']) {
            this.triggerToast('Failed to Delete', res['Message'], 'warning');
          } else {
            this.triggerToast(res['msg'], 'Something went wrong', 'warning');
          }
          this.isSpinner = false;
        },
        error: () => {
          this.triggerToast('Internal Server Error', 'something went wrong', 'danger');
          this.isSpinner = false;
        }
      });
    }

  }

  resetData() {
    this.holidaysForm.reset();
    this.isEdited = false;
    this.isFormSubmitted = false;
    this.showDateField = true;
    this.selectedDays = [];
    this.selectedLocation = [];
    this.selectedLocationIds = [];
    // this.getLocations = [];
    // this.getYears = [];
    setTimeout(() => {
      this.inputValue.nativeElement.value = null;
      let event = new KeyboardEvent('keyup', { 'bubbles': true });
      this.applyFilter(this.inputValue.nativeElement.dispatchEvent(event));
    }, 100);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toUpperCase();

    if (filterValue) {
      this.rows = this.originalRows.filter((row: any) => {
        const locationStr = Array.isArray(row.Location) ? row.Location.join(', ').toUpperCase() : '';
        const titleStr = row.Title?.toUpperCase() || '';
        const dateStr = row.Date?.toUpperCase?.() || '';  // use optional chaining in case it's null
        const dayStr = row.Day?.toUpperCase?.() || '';
        const descStr = row.Description?.toUpperCase() || '';

        return (
          locationStr.includes(filterValue) ||
          titleStr.includes(filterValue) ||
          dateStr.includes(filterValue) ||
          dayStr.includes(filterValue) ||
          descStr.includes(filterValue)
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


  openModal(): void {
    // this.callLocation();
    // this.GetAllFinanceMaster();
    const modalElement = document.getElementById('modal-right');
    const modal = new Modal(modalElement);
    modal.show();
  }

  parseJsonDate(jsonDate: string): Date | null {
    const match = /\/Date\((\d+)\)\//.exec(jsonDate);
    if (match) {
      return new Date(parseInt(match[1], 10));
    }
    return null;
  }
  formatDate(date: Date | null): string {
    if (!date) return '';
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
    const year = date.getFullYear();
    // return `${day}-${month}-${year}`;
    return `${year}-${month}-${day}`;
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
  setFloatingLabel(selectElement: HTMLSelectElement) {
    const label = selectElement.nextElementSibling as HTMLElement;
    if (selectElement.value) {
      label.classList.add('floating');
    } else {
      label.classList.remove('floating');
    }
  }
  onFocus(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }

  onBlur(event: FocusEvent) {
    this.setFloatingLabel(event.target as HTMLSelectElement);
  }
}
