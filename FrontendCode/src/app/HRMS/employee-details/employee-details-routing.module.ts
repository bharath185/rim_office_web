import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { ViewEmployeeComponent } from './view-employee/view-employee.component';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { UpdateEmployeeComponent } from './update-employee/update-employee.component';
import { AccessGuard } from 'src/app/shared/access.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    // canActivate: [AuthGuard, AccessGuard],
    children:[
      {path:'add_employee',component:AddEmployeeComponent},
      {path:'view_employee',component:ViewEmployeeComponent},
      {path:'update_employee',component:UpdateEmployeeComponent},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeDetailsRoutingModule { }
