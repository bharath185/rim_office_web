import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { AddWorkTypeComponent } from './add-work-type/add-work-type.component';
import { ViewWorktypeComponent } from './view-worktype/view-worktype.component';
import { EmployeeAttendanceComponent } from './employee-attendance/employee-attendance.component';
import { WfhModeComponent } from './wfh-mode/wfh-mode.component';
import { SelfAttendanceComponent } from './self-attendance/self-attendance.component';
import { ReportingEmpAttendanceComponent } from './reporting-emp-attendance/reporting-emp-attendance.component';
import { OnSiteWorkComponent } from './on-site-work/on-site-work.component';
import { AttendanceDashboardComponent } from './attendance-dashboard/attendance-dashboard.component';
import { UploadFileComponent } from './upload-file/upload-file.component';
import { AttendanceContractComponent } from './attendance-contract/attendance-contract.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'add_worktype', component: AddWorkTypeComponent },
      { path: 'view_worktype', component: ViewWorktypeComponent },
      { path: 'wfh_mode', component: WfhModeComponent },
      //  {path:'add_shifts',component:AddShiftsComponent},
      { path: 'employee_attendance', component: EmployeeAttendanceComponent },
      { path: 'employee_self_attendance', component: SelfAttendanceComponent },
      { path: 'reporting_employee', component: ReportingEmpAttendanceComponent },
      { path: 'on_site', component: OnSiteWorkComponent },
      { path: 'upload_attendance_file', component: UploadFileComponent },
      { path: 'attendance_dashboard', component: AttendanceDashboardComponent },
      { path: 'attendance_contract', component: AttendanceContractComponent },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttendenceManagementRoutingModule { }
