
export interface NavigationItem {
  id: string;
  title: string;
  type: 'item' | 'collapse' | 'group';
  translate?: string;
  icon?: string;
  hidden?: boolean;
  url?: string;
  classes?: string;
  exactMatch?: boolean;
  external?: boolean;
  target?: boolean;
  breadcrumbs?: boolean;
  color?: any;

  badge?: {
    title?: string;
    type?: string;
  };

  children?: NavigationItem[];

}

export const NavigationItems: NavigationItem[] = [
  {
    id: 'navigation',
    title: 'Navigation',
    type: 'group',
    icon: 'icon-group',
    children: [
      {
        id: 'Dashboard',
        title: 'Dashboard',
        type: 'item',
        url: '/dashboard',
        icon: 'feather icon-grid'
      },
      {
        id: 'add_access',
        title: 'Add Access',
        type: 'item',
        url: '/add_access',
        icon: 'feather icon-lock'
      },
      {
        id: 'screenshots_analysis',
        title: 'Screenshots Analysis',
        type: 'item',
        url: '/screenshots_analysis',
        icon: 'feather icon-camera'
      },
      {
        id: 'access_policy',
        title: 'Access Policy',
        type: 'collapse',
        icon: 'feather icon-shield',
        color: 'c1',
        children: [
          { id: 'department', title: 'Department List', type: 'item', url: '/department', icon: 'feather icon-briefcase' },
          { id: 'role_list', title: 'Role List', type: 'item', url: '/role', icon: 'feather icon-user-check' },
          { id: 'module', title: 'Module List', type: 'item', url: '/module', icon: 'feather icon-layers' },
        ]
      },
      {
        id: 'performance_portal',
        title: 'Performance Portal',
        type: 'collapse',
        icon: 'feather icon-trending-up',
        color: 'c2',
        children: [
          { id: 'reviewform', title: 'Review Form', type: 'item', url: '/reviewform', icon: 'feather icon-edit' },
          { id: 'goals', title: 'Goals', type: 'item', url: '/goals', icon: 'feather icon-target' },
          { id: 'behavior', title: 'Behavior', type: 'item', url: '/behavior', icon: 'feather icon-activity' },
          { id: 'review_list', title: 'Employee Review List', type: 'item', url: '/reviewList', icon: 'feather icon-list' },
          { id: 'self_development', title: 'Self Development', type: 'item', url: '/self-development', icon: 'feather icon-award' },
          { id: 'employee_goal_list', title: 'Employee Goal List', type: 'item', url: '/EmployeeGoalList', icon: 'feather icon-clipboard' },
          { id: 'configuration', title: 'Configuration', type: 'item', url: '/configuration', icon: 'feather icon-sliders' },
          { id: 'performance_reports', title: 'Report', type: 'item', url: '/reports', icon: 'feather icon-bar-chart-2' }
        ]
      },
      // {
      //   id: 'visitor',
      //   title: 'Visitor Module',
      //   type: 'collapse',
      //   icon: 'feather icon-users',
      //   color: 'c3',

      //   children: [
      //     { id: 'invite', title: 'Invite Page', type: 'item', url: '/invite', icon: 'feather icon-mail' },
      //   ]
      // },
      {
        id: 'visitor',
        title: 'Visitor',
        type: 'item',
        url: '/visitor',
        color: 'c3',
        icon: 'feather icon-users'
      },

      {
        id: 'view_all_employee',
        title: 'Employee',
        type: 'item',
        url: '/view_all_employee',
        color: 'c4',
        icon: 'feather icon-users'
      },
      // {
      //   id: 'employee_details',
      //   title: 'Employee',
      //   type: 'collapse',
      //   icon: 'feather icon-user',
      //   color: 'c4',
      //   children: [
      //     { id: 'view_employee', title: 'View Employee', type: 'item', url: '/view_employee', icon: 'feather icon-users' }
      //   ]
      // },
      {
        id: 'attendance',
        title: 'Attendance',
        type: 'collapse',
        icon: 'feather icon-calendar',
        color: 'c5',
        children: [
          { id: 'add_worktype', title: 'Add Work Type', type: 'item', url: '/add_worktype', icon: 'feather icon-clipboard' },
          { id: 'wfh_mode', title: 'WFH Mode', type: 'item', url: '/wfh_mode', icon: 'feather icon-home' },
          { id: 'employee_self_attendance', title: 'Self Attendance', type: 'item', url: '/employee_self_attendance', icon: 'feather icon-user' },
          { id: 'on_site', title: 'On Site', type: 'item', url: '/on_site', icon: 'feather icon-map-pin' },
          { id: 'attendance_contract', title: 'Contract Attendance', type: 'item', url: '/attendance_contract', icon: 'feather icon-calendar' },
        ]
      },
      {
        id: 'leave',
        title: 'Leave',
        type: 'item',
        url: '/leave',
        color: 'c6',
        icon: 'feather icon-check-square'
      },
      {
        id: 'settings',
        title: 'Settings',
        type: 'collapse',
        color: 'c7',
        icon: 'feather icon-settings',
        children: [
          { id: 'settings/master_creation', title: 'Master Creation', type: 'item', url: '/settings/master_creation', icon: 'feather icon-plus-circle' },
          { id: 'shifts', title: 'Shifts', type: 'item', url: '/shifts', icon: 'feather icon-clock' },
          // { id: 'holidays', title: 'Holidays', type: 'item', url: '/holidays', icon: 'feather icon-sun' }
        ]
      },
      {
        id: 'payroll',
        title: 'Payroll',
        type: 'collapse',
        color: 'c8',
        icon: 'fas fa-rupee-sign',
        children: [
          // {
          //   id: 'financial_details',
          //   title: 'Financial Details',
          //   type: 'item',
          //   url: '/payroll/financial_details',
          //   icon: 'feather icon-bar-chart'
          // },
          {
            id: 'payslip',
            title: 'Pay Slip',
            type: 'item',
            url: '/payroll/payslip',
            icon: 'feather icon-file-text'
          },
          {
            id: 'salary_management',
            title: 'Salary Management',
            type: 'item',
            url: '/payroll/salary_management',
            icon: 'feather icon-briefcase'
          },
        ]
      },
      {
        id: 'emp_master_report',
        title: 'Reports',
        type: 'item',
        url: '/emp_master_report',
        color: 'c9',
        icon: 'feather icon-bar-chart-2'
      },
      {
        id: 'org_chart',
        title: 'Org Chart',
        type: 'item',
        url: '/org_chart',
        color: 'c9',
        icon: 'feather icon-layers'
      },

    ]
  }
];
