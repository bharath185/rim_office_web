import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { AdminComponent } from 'src/app/theme/layout/admin/admin.component';
import { EmpFinancialDetailsComponent } from './emp-financial-details/emp-financial-details.component';
import { PayslipComponent } from './payslip/payslip.component';
import { SalaryStatutoryComponent } from './salary-statutory/salary-statutory.component';

const routes: Routes = [
  {
        path: '',
        component: AdminComponent,
        canActivate: [AuthGuard],
        children:[
          // {path:'payroll/financial_details',component:EmpFinancialDetailsComponent},
          {path:'payroll/payslip',component:PayslipComponent},
          {path:'payroll/salary_management',component:SalaryStatutoryComponent},
        ]
      }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PayrollRoutingModule { }
