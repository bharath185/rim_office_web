import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { ShiftsComponent } from './shifts/shifts.component';
import { HolidaysComponent } from './holidays/holidays.component';
import { CompanyCreationComponent } from './company-creation/company-creation.component';

const routes: Routes = [
  {
      path: '',
      component: AdminComponent,
      canActivate: [AuthGuard],
      // canActivate: [AuthGuard, AccessGuard],
      children:[
        {path:'shifts',component:ShiftsComponent},
        {path:'settings/master_creation',component:CompanyCreationComponent},
        {path:'holidays',component:HolidaysComponent},
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
