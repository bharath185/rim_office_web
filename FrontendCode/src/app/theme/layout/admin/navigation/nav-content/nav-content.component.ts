import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Location, LocationStrategy } from '@angular/common';
import { NavigationItem, NavigationItems } from '../navigation';
import { HrmsServiceService } from 'src/app/HRMS/hrms-service.service';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { NavigationEnd, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/assets/environment';
import { AccessPolicyStoreService } from 'src/app/HRMS/service/accessPolicayApi.service';

const INTERNAL_ALLOWED_ROUTES = [
  '/view_emp_leaves',
  '/teams_leaves',
  '/teams_leave',
  '/compoff_request',
  '/holidays',
  '/update_all_employee'
];


@Component({
  selector: 'app-nav-content',
  templateUrl: './nav-content.component.html',
  styleUrls: ['./nav-content.component.scss']
})
export class NavContentComponent implements OnInit {
  @Input() isNavCollapsed: boolean = false;
  title = 'Demo application for version numbering';
  userRole: any;
  DeptName: any;
  navigations: NavigationItem[] = [];
  wrapperWidth!: number;
  windowWidth: number;
  @Output() NavMobCollapse = new EventEmitter();
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  userData
  constructor(
    private readonly location: Location,
    private readonly locationStrategy: LocationStrategy,
    private readonly router: Router,
    private readonly hrmsService: HrmsServiceService,
    private readonly http: HttpClient,
    private accessPolicyStoreService: AccessPolicyStoreService
  ) {
    this.windowWidth = window.innerWidth;
    const storedEmployeeData = sessionStorage.getItem('userdata');
    this.userData = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
  }

  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }
  ngOnInit() {
    this.loadNavigationItems();
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.checkUrl(event.urlAfterRedirects);
      }
    });

    if (this.windowWidth < 992) {
      document.querySelector('.pcoded-navbar')?.classList.add('menupos-static');
    }
  }
  checkUrl(url: string): boolean {
    const allowedUrls = this.extractUrls(this.navigations);
    const checkedUrl = this.cleanUrl(url);

    // const accessPolicy = JSON.parse(sessionStorage.getItem('accessPolicy') || '[]');
    const accessPolicy = this.accessPolicyStoreService.getCurrentAccessPolicy() || [];
    const hasAccess = accessPolicy.some((item: any) => {
      const pageUrl = this.convertPageNameToUrl(item.PageName);
      return checkedUrl === pageUrl && item.ViewAccess;
    });

    if (
      INTERNAL_ALLOWED_ROUTES.includes(checkedUrl) ||
      allowedUrls.includes(checkedUrl) ||
      hasAccess ||
      url.includes('/dashboard') ||
      url.includes('/profile') ||
      url.includes('/change_password') ||
      url.includes('/reporting_employee') ||
      url.includes('/upload_attendance_file') ||
      url.includes('/access_denied') ||
      url.includes('/view_emp_leaves')

    ) {
      return true;
    }

    if (url.includes('/auth/signin')) {
      this.onLogout();
      return true;
    }

    this.router.navigate(['access_denied']);
    return false;
  }
  convertPageNameToUrl(pageName: string): string {
    const map: Record<string, string> = {
      'SubModule List': '/subModule',
      'Page Module List': '/pageModule',
      'View Visitor': '/view_visitor',
      'Direct Checkin': '/direct_checkin',
      'Add Employee': '/add_employee',
      'View Work Type': '/view_worktype',
      'Employee Attendance': '/employee_attendance',
      'Create Leave Types': '/create_leave_type',
      'Leave Balance Report': '/leave_balance_report',
      'Present-Absent Report': '/option_report',
      'Create Employee': '/create_employee',
      'Employee Probation Report': '/employee_probation_report',
      'Employee Log Report': '/employee_loghistroy_report',
    };
    return map[pageName] || '';
  }

  onLogout() {
    const reqbody = {
      UserName: this.userData.UserName,
      TokenId: this.userData.TokenId,
      AuthKey: this.userData.UserAuth,
      RoleId: this.userData.DesignationId
    }
    const headers = new HttpHeaders({
      Authorization: this.userData.TokenId,
      AuthKey: this.userData.UserAuth,
    });
    this.http.post(`${environment.baseUrl}/Login/LogOut`, reqbody, { headers })
      .subscribe(
        (response) => {
          console.log('Logout successful', response);
        },
        (error) => {
          console.error('Logout failed', error);
        }
      );
  }
  cleanUrl(url: string): string {
    return url.split('?')[0].split('#')[0];
  }
  extractUrls(items: NavigationItem[]): string[] {
    const urls: string[] = [];
    items.forEach(item => {
      if (item.url) {
        urls.push(item.url);
      }
      if (item.children) {
        urls.push(...this.extractUrls(item.children));
      }
    });
    // console.log(urls);
    return urls;
  }

  private loadNavigationItems() {
    this.navigations = [];
    const reqBody = { EmpId: this.userData.EmpId };
    // this.accessPolicyService.accessGetAccessPolicy(reqBody).subscribe((accessPolicy: any) => {
    //   sessionStorage.setItem('accessPolicy', JSON.stringify(accessPolicy));
    //   if (accessPolicy) {
    //     const allowedPageNames = accessPolicy.map((access: any) => access.PageName);
    //     const alwaysIncludeIds = ["Dashboard"];

    //     this.navigations = NavigationItems.map(group =>
    //       this.filterNavigation(group, allowedPageNames, alwaysIncludeIds)
    //     ).filter(group => group !== null) as NavigationItem[];

    //     this.router.events.subscribe(event => {
    //       if (event instanceof NavigationEnd) {
    //         this.checkUrl(event.urlAfterRedirects);
    //       }
    //     });
    //   }
    // }, error => {
    //   window.alert('Internal Server Error For AccessPolicy Refresh Page/Logout And Again Login');
    //   this.triggerToast('Internal Server Error', 'To Load AccessPolicy Refresh Page Please', 'danger');
    // });
    this.hrmsService.accessGetAccessPolicy(reqBody).subscribe((accessPolicy: any) => {
      this.accessPolicyStoreService.setAccessPolicy(accessPolicy);

      if (accessPolicy) {
        const allowedPageNames = accessPolicy.map((access: any) => access.PageName);
        const alwaysIncludeIds = ["Dashboard"];

        this.navigations = NavigationItems.map(group =>
          this.filterNavigation(group, allowedPageNames, alwaysIncludeIds)
        ).filter(group => group !== null) as NavigationItem[];
      }
    }, error => {
      window.alert('Internal Server Error For AccessPolicy Refresh Page/Logout And Again Login');
      this.triggerToast('Internal Server Error', 'To Load AccessPolicy Refresh Page Please', 'danger');
    });

  }


  private filterNavigation(item: NavigationItem, allowedPageNames: string[], alwaysIncludeIds: string[]): NavigationItem | null {
    const filterChildren = (children: NavigationItem[]): NavigationItem[] => {
      return children
        .map(child => {
          const filteredChildren = child.children ? filterChildren(child.children) : [];
          const isAllowed = allowedPageNames.includes(child.title) || alwaysIncludeIds.includes(child.title);

          return (isAllowed || filteredChildren.length > 0) ? {
            ...child,
            children: filteredChildren.length > 0 ? filteredChildren : undefined
          } : null;
        })
        .filter(child => child !== null) as NavigationItem[];
    };
    const filteredChildren = item.children ? filterChildren(item.children) : [];
    const isAllowedGroup = allowedPageNames.includes(item.title) || alwaysIncludeIds.includes(item.title) || filteredChildren.length > 0;
    return isAllowedGroup ? {
      ...item,
      children: filteredChildren.length > 0 ? filteredChildren : undefined
    } : null;
  }

  logNavigationItemIds(): void {
    const allIds = this.collectIds(this.navigations);
    console.log('All menu IDs:', allIds);
  }
  collectIds(items: NavigationItem[], collectedIds: string[] = []): string[] {
    for (const item of items) {
      collectedIds.push(item.title);
      if (item.children) {
        this.collectIds(item.children, collectedIds);
      }
    }
    return collectedIds;
  }
  navMob() {
    if (this.windowWidth < 992 && document.querySelector('app-navigation.pcoded-navbar')?.classList.contains('mob-open')) {
      this.NavMobCollapse.emit();
    }
  }


  fireOutClick() {
    let current_url = this.location.path();
    console.log(current_url);

    const baseHref = this.locationStrategy.getBaseHref();
    if (baseHref) {
      current_url = baseHref + this.location.path();
    }
    const link = "a.nav-link[ href='" + current_url + "' ]";
    const ele = document.querySelector(link);
    if (ele !== null) {
      const parent = ele.parentElement;
      const up_parent = parent?.parentElement?.parentElement;
      const last_parent = up_parent?.parentElement;
      if (parent?.classList.contains('pcoded-hasmenu')) {
        parent.classList.add('pcoded-trigger');
        parent.classList.add('active');
      } else if (up_parent?.classList.contains('pcoded-hasmenu')) {
        up_parent.classList.add('pcoded-trigger');
        up_parent.classList.add('active');
      } else if (last_parent?.classList.contains('pcoded-hasmenu')) {
        last_parent.classList.add('pcoded-trigger');
        last_parent.classList.add('active');
      }
    }
  }

  closeAllMenusOnMouseOut(): void {
    const allMenus = document.querySelectorAll('.pcoded-hasmenu.active, .pcoded-hasmenu.pcoded-trigger');
    allMenus.forEach(menu => {
      menu.classList.remove('active');
      menu.classList.remove('pcoded-trigger');
    });
  }

}








