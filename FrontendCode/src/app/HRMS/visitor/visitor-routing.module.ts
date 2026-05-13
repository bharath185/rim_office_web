import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvitePageComponent } from './invite-page/invite-page.component';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { ViewVisitorComponent } from './view-visitor/view-visitor.component';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { DirectCheckinComponent } from './direct-checkin/direct-checkin.component';
import { AccessGuard } from 'src/app/shared/access.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    // canActivate: [AuthGuard, AccessGuard],
    children:[
      {path:'invite',component:InvitePageComponent},
      {path:'view_visitor',component:ViewVisitorComponent},
      {path:'direct_checkin',component:DirectCheckinComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VisitorRoutingModule { }
