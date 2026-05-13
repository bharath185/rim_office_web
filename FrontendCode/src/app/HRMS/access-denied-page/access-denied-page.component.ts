import { Component, OnInit } from '@angular/core';
import { HrmsServiceService } from '../hrms-service.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-access-denied-page',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './access-denied-page.component.html',
  styleUrl: './access-denied-page.component.scss'
})
export class AccessDeniedPageComponent implements OnInit {
  employeeDetails: any;
  userDetails: any;
  constructor(private readonly hrmsService: HrmsServiceService,
    private readonly route: Router,
  ) { }
  ngOnInit(): void {
    const storedEmployeeData = sessionStorage.getItem('userdata');
    this.userDetails = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    const employeeDetails = sessionStorage.getItem('employeeDetails');
    if (employeeDetails) {
      try {
        this.employeeDetails = JSON.parse(employeeDetails);
      } catch (error) {
        console.error('Error parsing JSON:', error);
      }
    } else {
      console.warn('No employee details provided');
    }
    this.logout();
  }


  logout() {
    const reqbody = {
      UserName: this.userDetails.UserName,
      TokenId: this.userDetails.TokenId,
      AuthKey: this.userDetails.UserAuth,
      RoleId: this.employeeDetails[0].DesignationId
    }
    console.log(reqbody);
    this.hrmsService.logoutApi(reqbody).subscribe((res: any) => {
      if (res['TokenId'] === 'Expired') {
        sessionStorage.removeItem('accessPolicy');
        sessionStorage.removeItem('employeeDetails');
        sessionStorage.removeItem('token');
        sessionStorage.removeItem('userAuth');
        sessionStorage.removeItem('userdata');
      } else if (res['Message']) {
        window.alert('Sorry Something went wrong');
      }
    }, error => {
      window.alert('Internal Server Error')
    })
  }
}
