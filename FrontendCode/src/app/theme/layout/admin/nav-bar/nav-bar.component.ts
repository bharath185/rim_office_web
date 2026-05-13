// Angular Import
import { Component, ElementRef, EventEmitter, HostListener, OnInit, Output, ViewChild } from '@angular/core';
import { EmployeeModuleService } from 'src/app/HRMS/service/employee.service';
import { EntityStateService } from 'src/app/HRMS/service/entity-state.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  @ViewChild('searchInput') searchInput!: ElementRef;
  employeeDetails;

  constructor(private entityStateService: EntityStateService,
    private readonly hrmsEmployeeService: EmployeeModuleService,) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;
    setTimeout(() => {
      if (this.employeeDetails[0].Authorised && this.employeeDetails[0].DeptName?.toLowerCase() === 'human resource') {
        // this.getAllEmployeeList();
      }
    }, 100);
  }
  // public props
  menuClass = false;
  // collapseStyle = 'none';
  collapseStyle = 'block';


  windowWidth = window.innerWidth;
  @Output() NavCollapse = new EventEmitter();
  @Output() NavCollapsedMob = new EventEmitter();

  // public method old code
  toggleMobOption() {
    this.menuClass = !this.menuClass;
    this.collapseStyle = this.menuClass ? 'block' : 'none';
    //  this.NavCollapsedMob.emit();
  }

  // public method newCode code
  // toggleMobOption() {
  //   this.menuClass = !this.menuClass;
  //   this.collapseStyle = this.menuClass ? 'block' : 'block';
  //  this.NavCollapsedMob.emit();
  // }

  navCollapse() {
    if (this.windowWidth >= 992) {
      this.NavCollapse.emit();
    }
  }
  ngOnInit(): void {

  }

  // Add these properties
  listOfEmp: any[] = [];
  searchResults: any[] = [];
  searchTerm: string = '';
  isSearching: boolean = false;
  showDropdown: boolean = false; // ✅ Track dropdown visibility
  selectedEmployee: any = null;
  showModal: boolean = false;
  isEmployeeLoading = false;

  getAllEmployeeList() {
    const reqBody = {
      LoginId: this.employeeDetails[0].LoginId,
      EmpId: 0,
      AuthorisedEntity: Number(this.entityStateService.getEntityId()),
      CompId: 1,
      LEId: Number(this.entityStateService.getEntityId()),
      BUId: 0,
      LocationId: 0,
      DeptId: 0,
      DesignationId: 0,
      FromDate: null,
      ToDate: null,
      Status: ""
    };
    this.isEmployeeLoading = true;
    this.hrmsEmployeeService.employeeGetAllEmployee(reqBody).subscribe(
      (res: any[]) => {
        this.listOfEmp = res;
        this.isEmployeeLoading = false;
      },
      () => {
        this.isEmployeeLoading = false;
      }
    );
  }

  formatDotNetDate(dateString: string): string {
    if (!dateString) return '';
    const timestamp = Number(dateString.replace(/[^0-9]/g, ''));
    const date = new Date(timestamp);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  }

  openEmployee(emp: any) {
    this.selectedEmployee = emp;
    this.showModal = true;

    this.showDropdown = false;
    this.searchResults = [];
    this.searchTerm = '';

    if (this.searchInput) {
      this.searchInput.nativeElement.value = '';
    }
  }

  closeModal() {
    this.showModal = false;
  }

  onSearch(event: any) {
    this.searchTerm = (event.target as HTMLInputElement).value.toLowerCase().trim();

    if (this.searchTerm.length < 2) {
      this.searchResults = [];
      this.showDropdown = false;
      return;
    }

    this.isSearching = true;

    setTimeout(() => {
      this.searchResults = this.searchThroughData(this.searchTerm);
      this.isSearching = false;
      this.showDropdown = true; // show dropdown after search
    }, 300);
  }

  // When input is clicked, show dropdown if there are results
  onInputClick() {
    this.showDropdown = true;
    if (this.searchResults.length > 0) {
      this.showDropdown = true;
    }
  }

  clearSearch(input: HTMLInputElement) {
    input.value = '';
    input.focus();
    this.searchResults = [];
  }

  searchThroughData(term: string): any[] {
    console.log('Searching for:', term);
    const results: any[] = [];

    if (this.listOfEmp) {
      console.log('Employee list:', this.listOfEmp);
      const employeeResults = this.listOfEmp.filter(emp =>
        emp.FirstName?.toLowerCase().includes(term) ||
        emp.LastName?.toLowerCase().includes(term) ||
        emp.EmpCode?.toLowerCase().includes(term) ||
        emp.Designation?.toLowerCase().includes(term)
      );
      results.push(...employeeResults);
    }
    return results;
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: Event) {
    const target = event.target as HTMLElement;
    const searchContainer = document.querySelector('.header-search');

    if (searchContainer && !searchContainer.contains(target)) {
      this.showDropdown = false;
    }
  }

}
