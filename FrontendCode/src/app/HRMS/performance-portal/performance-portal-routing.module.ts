import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { ReviewFormComponent } from './review-form/review-form.component';
import { GoalsComponent } from './goals/goals.component';
import { BehaviorComponent } from './behavior/behavior.component';
import { ReviewListComponent } from './review-list/review-list.component';
import { SelfDevelopmentGoalComponent } from './self-development-goal/self-development-goal.component';
import { EmployeeGoalListComponent } from './employee-goal-list/employee-goal-list.component';
import { ConfigrationComponent } from './configration/configration.component';
import { ReportsComponent } from './reports/reports.component';

const routes: Routes = [
  {
      path: '',
      component: AdminComponent,
      canActivate: [AuthGuard],
      children:[
        {path:'reviewform',component:ReviewFormComponent},
        {path:'goals',component:GoalsComponent},
        {path:'behavior',component:BehaviorComponent},
        {path:'reviewList',component:ReviewListComponent},
        {path:'self-development',component:SelfDevelopmentGoalComponent},
        {path:'EmployeeGoalList',component:EmployeeGoalListComponent},
        {path:'configuration',component:ConfigrationComponent},
        {path:'reports',component:ReportsComponent},

      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PerformancePortalRoutingModule { }
