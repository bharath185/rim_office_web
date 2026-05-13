// api.service.ts
import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Config } from 'src/app/configuration/env.config';
import { environment } from 'src/assets/environment';
import { Subscription } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class ApiService implements OnDestroy {
  private routerEventsSubscription: Subscription;
  constructor(private readonly http: HttpClient, private readonly router: Router) {
    this.routerEventsSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        // console.log('NavigationEnd:', event.urlAfterRedirects);
        if (event.urlAfterRedirects.includes('/auth/signin')) {
          // sessionStorage.clear();
          sessionStorage.removeItem('accessPolicy');
          sessionStorage.removeItem('employeeDetails');
          setTimeout(() => {
            sessionStorage.removeItem('token');
            sessionStorage.removeItem('userAuth');
            sessionStorage.removeItem('userdata');
          }, 100);
        }
      }
    });
  }
  ngOnDestroy(): void {
    // Clean up subscription when service is destroyed
    if (this.routerEventsSubscription) {
      this.routerEventsSubscription.unsubscribe();
    }
  }
  login(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Login/Login`, reqbody,
      // {headers: { Authorization: 'OfficeConnect' },}
    )
  }

  LoginForgetPassword(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Login/ForgetPassword`, reqbody)
  }

  LoginFPwdVerify(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Login/FPwdVerify`, reqbody)
  }

  getFYearDetails(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/GetFYearDetails`, reqbody)
  }

}