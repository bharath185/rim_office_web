import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DepartmentComponent } from './department/department.component';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { RoleComponent } from './role/role.component';
import { ModuleComponent } from './module/module.component';
import { SubModuleComponent } from './sub-module/sub-module.component';
import { PageModuleComponent } from './page-module/page-module.component';
import { canLoadModule } from 'src/app/shared/canload.guard';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { AccessGuard } from 'src/app/shared/access.guard';

const routes: Routes = [{
  path: '',
  component: AdminComponent,
  canActivate: [AuthGuard],
  children: [
    { path: 'department', component: DepartmentComponent },
    { path: 'role', component: RoleComponent },
    {path:'module',component:ModuleComponent},
    {path:'subModule',component:SubModuleComponent},
    {path:'pageModule',component:PageModuleComponent}

  ]
}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccessPolicyRoutingModule { }
