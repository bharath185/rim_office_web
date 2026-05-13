// import { Injectable } from '@angular/core';
// import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
// import { Observable, catchError, throwError } from 'rxjs';
// import { ApiService } from '../demo/authentication/sign-in/api.service';
// import { Router } from '@angular/router';


// @Injectable()
// export class TokenInterceptor implements HttpInterceptor {

//   constructor(private router: Router,private apiService: ApiService ) {}
//   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//       var token: any;
//       var user: any|null;
//           user=localStorage.getItem('token');
//       if(user!=null){
//           request = request.clone({
//               setHeaders: {
//                 Authorization: `OfficeConnect`,
//                 AuthKey: '${user}'
                  
//               }
//           });

//       }else{
//           // const data=JSON.parse(sessionStorage.getItem('userDetail'));
//           // user =data.token;
//           // request = request.clone({
//           //     setHeaders: {
//           //         Authorization: `Bearer ${user}`
//           //     }
//           // });
//       }
//       // user = JSON.parse(sessionStorage.getItem('token'))

//   // token = JSON.parse(JSON.stringify('' + user));
//      // console.log(user)
//       // if (user != null)
//       // {

//       // }else{
//       // //         // if(JSON.parse(sessionStorage.getItem('token'))===null){
//       // //     const data=JSON.parse(sessionStorage.getItem('userDetail'));
//       // //     user = data.token;
//       // // // }
//       // // request = request.clone({
//       // //     setHeaders: {
//       // //         Authorization: `Bearer ${user}`
//       // //     }
//       // // });
//       // }
//       return next.handle(request).pipe(catchError(err => {
//           console.log(err)
//           //const error = err.error || err.error.error;
//           if (err.status === 401) {
//               // sessionStorage.removeItem('token');
//               //.error('Session has expired and must log in again','Error')
          
//               // this.router.navigate(['/']);
//           }
//           else if(err.status === 400){
//               console.log(err)
//               //.error(err.error['error msg'] || err.error['error_msg'] || err.error['errorMessage'] || err.error['msg'] || err.error ,'Error' )
//           }
//           else if(err.status === 500){
//               //console.log(err.error.error)
//               //.error( err.error.error ||'Something went Wrong....Please try again after sometime','Error')
//               //  this.router.navigate(['/']);
//           }
//           else if(err.status == 404){
//               if(err.error !=null){
//                //.error('Requested Url Not Found','Error')
//               } 
//            }
//            else if(err.status == 403){
//               //.error("You are not eligible for this Feature.You are unauthorized User. Please contact the administrator to assign permission! ",'Error')
              
//           }
//            else if(err.status == 204){
//            //.error('No Data Found','Error')  
//            }else{
//               // sessionStorage.removeItem('token');
//               // this.router.navigate(['/']);

//            }

//           const error = err.error.message || err.statusText;
//           return throwError(error);
//       }));
      
//   }
// }

