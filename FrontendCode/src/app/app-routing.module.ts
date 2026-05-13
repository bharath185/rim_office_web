// Angular Import
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// project import
import { AdminComponent } from './theme/layout/admin/admin.component';
import { GuestComponent } from './theme/layout/guest/guest.component';
import { DashboardComponent } from './demo/dashboard/dashboard.component';
import { SignInComponent } from './demo/authentication/sign-in/sign-in.component';
import { AuthGuard } from './shared/auth.guard';
import { EmployeeComponent } from './HRMS/employee/employee.component';
import { AddAccessComponent } from './HRMS/add-access/add-access.component';
import { VerifyOtpComponent } from './HRMS/verify-otp/verify-otp.component';
import { VisitorPageComponent } from './HRMS/visitor-page/visitor-page.component';
import { VisitorCheckinComponent } from './HRMS/visitor-checkin/visitor-checkin.component';
import { VisitorSuccessModalComponent } from './HRMS/visitor-success-modal/visitor-success-modal.component';
import { VisitorSelfCheckinComponent } from './HRMS/visitor-self-checkin/visitor-self-checkin.component';
import { ChangePasswordComponent } from './HRMS/change-password/change-password.component';
import { AccessDeniedPageComponent } from './HRMS/access-denied-page/access-denied-page.component';
import { OnsiteComponent } from './HRMS/onsite/onsite.component';
import { ScreenshotsComponent } from './HRMS/screenshots/screenshots.component';
import { LeavesComponent } from './HRMS/leavess/leaves/leaves.component';
import { CreateLeaveComponent } from './HRMS/leavess/create-leave/create-leave.component';
import { TeamsleaveHrComponent } from './HRMS/leavess/teamsleave-hr/teamsleave-hr.component';
import { TeamsleaveMgrComponent } from './HRMS/leavess/teamsleave-mgr/teamsleave-mgr.component';
import { ViewAllEmployeeComponent } from './HRMS/employee-module/view-all-employee/view-all-employee.component';
import { CreateEmployeeComponent } from './HRMS/employee-module/create-employee/create-employee.component';
import { UpdateAllEmployeeComponent } from './HRMS/employee-module/update-all-employee/update-all-employee.component';
import { InvitePageComponent } from './HRMS/visitor/invite-page/invite-page.component';
import { ViewVisitorComponent } from './HRMS/visitor/view-visitor/view-visitor.component';
import { DirectCheckinComponent } from './HRMS/visitor/direct-checkin/direct-checkin.component';
import { ViewCompoffReqComponent } from './HRMS/leavess/view-compoff-req/view-compoff-req.component';
import { EmpMasReportComponent } from './HRMS/Reports/emp-mas-report/emp-mas-report.component';
import { AppraisalReportComponent } from './HRMS/Reports/appraisal-report/appraisal-report.component';
import { LeaveReportComponent } from './HRMS/Reports/leave-report/leave-report.component';
import { OrgChartComponent } from './HRMS/org-chart/org-chart.component';
import { LeaveBalanceComponent } from './HRMS/Reports/leave-balance/leave-balance.component';
import { WorkingDaysReportsComponent } from './HRMS/Reports/working-days-reports/working-days-reports.component';
import { ContractAttendanceComponent } from './HRMS/contract-attendance/contract-attendance.component';
import { EmployeeProbationReportComponent } from './HRMS/employee-module/employee-probation-report/employee-probation-report.component';
import { EmpLogHistroyReportComponent } from './HRMS/employee-module/emp-log-histroy-report/emp-log-histroy-report.component';

const routes: Routes = [
  {
    path: '',
    component: GuestComponent,
    children: [
      {
        path: '',
        redirectTo: '/auth/signin',
        pathMatch: 'full'
      },
      {
        path: 'auth/signin',
        component: SignInComponent,
      },

      {
        path: 'verify_otp',
        component: VerifyOtpComponent
      },
      {
        path: 'visitor_page',
        component: VisitorPageComponent
      },
      {
        path: 'visitor_checkin',
        component: VisitorCheckinComponent
      },
      {
        path: 'success',
        component: VisitorSuccessModalComponent
      },
      {
        path: 'visitor_self_checkin',
        component: VisitorSelfCheckinComponent
      },
      {
        path: 'onsite',
        component: OnsiteComponent
      },
      {
        path: 'contract_attendance',
        component: ContractAttendanceComponent
      },

    ]
  },

  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: '/auth/signin',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: DashboardComponent,

      },
      {
        path: 'change_password',
        component: ChangePasswordComponent
      },
      {
        path: 'profile',
        component: EmployeeComponent,

      },
      {
        path: 'add_access',
        component: AddAccessComponent
      },
      {
        path: 'screenshots_analysis',
        component: ScreenshotsComponent
      },
      {
        path: 'access_policy',
        loadChildren: () => import('./HRMS/access-policy/access-policy.module').then((m => m.AccessPolicyModule)),
      },
      {
        path: 'performance_portal',
        loadChildren: () => import('./HRMS/performance-portal/performance-portal.module').then((m => m.PerformancePortalModule)),
      },
      //this is for Visitor module
      {
        path: 'visitor',
        component: InvitePageComponent
      },
      {
        path: 'view_visitor',
        component: ViewVisitorComponent
      },
      {
        path: 'direct_checkin',
        component: DirectCheckinComponent
      },
      //this is for Visitor module

      // this is for Employee module
      {
        path: 'view_all_employee',
        component: ViewAllEmployeeComponent
      },
      {
        path: 'create_employee',
        component: CreateEmployeeComponent
      },
      {
        path: 'employee_probation_report',
        component: EmployeeProbationReportComponent
      },
      {
        path: 'employee_loghistroy_report',
        component: EmpLogHistroyReportComponent
      },
      {
        path: 'update_all_employee',
        component: UpdateAllEmployeeComponent
      },
      // this is for Employee module

      {
        path: 'attendence_management',
        loadChildren: () => import('./HRMS/attendence-management/attendence-management.module').then((m => m.AttendenceManagementModule))
      },

      // this is for leave module
      {
        path: 'leave',
        component: LeavesComponent
      },
      {
        path: 'create_leave_type',
        component: CreateLeaveComponent
      },
      {
        path: 'leave_balance_report',
        component: LeaveBalanceComponent
      },
      {
        path: 'option_report',
        component: WorkingDaysReportsComponent
      },
      {
        path: 'teams_leaves',
        component: TeamsleaveHrComponent
      },
      {
        path: 'teams_leave',
        component: TeamsleaveMgrComponent
      },
      {
        path: 'compoff_request',
        component: ViewCompoffReqComponent
      },
      // this is for leave module


      // this is for Reports module
      {
        path: 'emp_master_report',
        component: EmpMasReportComponent
      },
      {
        path: 'appraisal_report',
        component: AppraisalReportComponent
      },
      {
        path: 'leave_report',
        component: LeaveReportComponent
      },
      // this is for Reports module


      {
        path: 'settings',
        loadChildren: () => import('./HRMS/settings/settings.module').then((m => m.SettingsModule))
      },
      {
        path: 'payroll',
        loadChildren: () => import('./HRMS/payroll/payroll.module').then((m => m.PayrollModule))
      },
      {
        path: 'org_chart',
        component: OrgChartComponent
      },
    ]
  },
  {
    path: 'verify_otp',
    component: VerifyOtpComponent
  },
  {
    path: 'visitor_page',
    component: VisitorPageComponent
  },
  {
    path: 'visitor_checkin',
    component: VisitorCheckinComponent
  },
  {
    path: 'success',
    component: VisitorSuccessModalComponent
  },
  {
    path: 'visitor_self_checkin',
    component: VisitorSelfCheckinComponent
  },
  {
    path: 'contract_attendance',
    component: ContractAttendanceComponent
  },
  {
    path: 'access_denied',
    component: AccessDeniedPageComponent
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
