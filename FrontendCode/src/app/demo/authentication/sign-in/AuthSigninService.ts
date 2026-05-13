// import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { Injectable } from '@angular/core';

// import { Config } from 'src/app/shared/env.config';

// @Injectable({
//   providedIn: 'root'
// })
// export class AuthSigninService {
//   BaseEndpoint: any;
//   reqHeader:HttpHeaders;
//     constructor(private http: HttpClient) {
//       this.BaseEndpoint = Config.BaseEndpoint
//       this.reqHeader = new HttpHeaders({ 
//         'Content-Type': 'application/json',
//         'Accept': 'application/json',
//      });
//     }
    
//     login(loginDetails: any){
//       return this.http.post(this.BaseEndpoint + 'user/signin',JSON.stringify(loginDetails), { headers:this.reqHeader })
//       .toPromise()
//       .then((response: any) => {
//         return response;
//       })
//       .catch((error) => {
//         return Promise.reject(error);
//       });
//     }

// }
