// import { Injectable } from '@angular/core';
// import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
// import { Observable } from 'rxjs';

// @Injectable()
// export class AuthInterceptor implements HttpInterceptor {
//   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     const token = sessionStorage.getItem('token') || '';
//     const userAuth = sessionStorage.getItem('userAuth') || '';

//     const clonedReq = req.clone({
//       setHeaders: {
//         Authorization: token,
//         AuthKey: userAuth,
//         'Strict-Transport-Security': 'max-age=31536000',
//         'Content-Security-Policy': "default-src 'self'",
//         'X-Content-Type-Options': 'nosniff',
//         'X-Frame-Options': 'DENY',
//         'X-Xss-Protection': '1; mode=block',
//         'Referrer-Policy': 'strict-origin-when-cross-origin',
//       }
//     });

//     return next.handle(clonedReq);
//   }
// }


import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // const token = sessionStorage.getItem('token') || '';
    // const userAuth = sessionStorage.getItem('userAuth') || '';
    const userData = JSON.parse(sessionStorage.getItem('userdata') || '{}');
    const token = userData?.TokenId || '';
    const userAuth = userData?.UserAuth || '';

    // Predefined URLs for different purposes
    const loginUrls = [
      '/Login/Login',
      '/Login/ForgetPassword',
      '/Login/FPwdVerify',
      'Login/ChangePassword',
      '/Performance/GetEmployeeDetails',
      '/Employee/DDVendorList',
      '/Employee/DDSiteList',
      '/Employee/DDProjectList',
      '/Employee/ContractAttendanceChecking',
      '/Employee/AddContractAttendance',
      
    ];

    const visitorUrls = [
      '/Visitor/VerifyOTP',
      '/Visitor/VerifyOTPCheckIn',
      '/Visitor/AcceptInvite',
      '/Visitor/VisitorCheckIn',
      '/Visitor/VisitorCheckOut',
      '/Visitor/UploadFileVisitor',
      '/Visitor/DDCompany',
      '/Visitor/DDEmployee',
      '/Visitor/VisitorDirectCheckIn',
      '/Employee/AddOnSiteData',
      
    ];

    // Check if the request is for a login-related API
    if (loginUrls.some(url => req.url.includes(url))) {
      const loginReq = req.clone({
        setHeaders: {
          Authorization: 'OfficeConnect',
          // 'Accept': 'application/json',
        }
      });
      return next.handle(loginReq);
    }

    // Check if the request is for a visitor-related API
    if (visitorUrls.some(url => req.url.includes(url))) {
      const visitorReq = req.clone({
        setHeaders: {
          Authorization: 'Visitors',
          // 'Accept': 'application/json',
        }
      });
      return next.handle(visitorReq);
    }

    // For all other requests, add comprehensive headers
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: token,
        AuthKey: userAuth,
        // 'Accept': 'application/json',
        'Strict-Transport-Security': 'max-age=31536000',
        'Content-Security-Policy': "default-src 'self'",
        'X-Content-Type-Options': 'nosniff',
        'X-Frame-Options': 'DENY',
        'X-Xss-Protection': '1; mode=block',
        'Referrer-Policy': 'strict-origin-when-cross-origin',
      }
    });

    return next.handle(clonedReq);
  }
}




