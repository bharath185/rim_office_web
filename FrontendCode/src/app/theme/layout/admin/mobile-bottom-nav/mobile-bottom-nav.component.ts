import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

interface MobileNavItem {
  id: string;
  title: string;
  icon: string;
  url: string;
  badge?: number;
}

@Component({
  selector: 'app-mobile-bottom-nav',
  templateUrl: './mobile-bottom-nav.component.html',
  styleUrls: ['./mobile-bottom-nav.component.scss']
})
export class MobileBottomNavComponent implements OnInit {
  navItems: MobileNavItem[] = [];
  currentUrl: string = '';

  constructor(private router: Router) {
    this.router.events.subscribe(() => {
      this.currentUrl = this.router.url;
    });
  }

  ngOnInit(): void {
    this.navItems = [
      { id: 'dashboard', title: 'Home', icon: 'feather icon-grid', url: '/dashboard' },
      { id: 'attendance', title: 'Attendance', icon: 'feather icon-calendar', url: '/employee_self_attendance' },
      { id: 'leave', title: 'Leave', icon: 'feather icon-check-square', url: '/leave' },
      { id: 'employee', title: 'Employees', icon: 'feather icon-users', url: '/view_all_employee' },
      { id: 'more', title: 'More', icon: 'feather icon-more-horizontal', url: '/dashboard' }
    ];
    this.currentUrl = this.router.url;
  }

  isActive(url: string): boolean {
    return this.currentUrl === url || this.currentUrl.startsWith(url + '/');
  }

  navigate(url: string): void {
    this.router.navigate([url]);
  }
}
