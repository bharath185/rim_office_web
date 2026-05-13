import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Config } from 'src/app/configuration/env.config';



@Injectable({
  providedIn: 'root'
})
export class BehaviorService  {
  BaseEndpoint: any;
  reqHeader:HttpHeaders;
    userDetails:any ;
    authorization: any;
    authKey: any;
    isLoggedIn = false;
    tokenkey: any;

      constructor(private http: HttpClient) {
        this.userDetails=sessionStorage.getItem('userDetail');
        const sessionData = JSON.parse(this.userDetails);
  
        // Retrieve employeeId from session data
      //  this.authorization= sessionData.EmpId;
        // this.authKey= sessionData.UserAuth;
        // this.tokenkey= sessionData.TokenId;
console.log(this.authKey);
        this.BaseEndpoint = Config.BaseEndpoint
        this.reqHeader = new HttpHeaders({ 
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': this.tokenkey,
          'AuthKey':this.authKey
       });
      }
    
    addBehavior(BehaviorDetails: any){
      return this.http.post(this.BaseEndpoint + '/Performance/AddBehaviour',JSON.stringify(BehaviorDetails[0]), { headers:this.reqHeader })
      .toPromise()
      .then((response: any) => {
        return response;
      })
      .catch((error) => {
        return Promise.reject(error);
      });
    }
    getALLBehaviors(empIdDetails: any){
        return this.http.post(this.BaseEndpoint + '/Performance/GetAllBehaviour',JSON.stringify(empIdDetails), { headers:this.reqHeader })
        .toPromise()
        .then((response: any) => {
          return response;
        })
        .catch((error) => {
          return Promise.reject(error);
        });
 
      }
      getALLReviewList(empIdDetails: any){
        return this.http.post(this.BaseEndpoint + '/Performance/GetEmployeeReviewList',JSON.stringify(empIdDetails), { headers:this.reqHeader })
        .toPromise()
        .then((response: any) => {
          return response;
        })
        .catch((error) => {
          return Promise.reject(error);
        });
      }

      GetAllEmployeeReviewList(empIdDetails: any){
        return this.http.post(this.BaseEndpoint + '/Performance/GetAllEmployeeReviewList',JSON.stringify(empIdDetails), { headers:this.reqHeader })
        .toPromise()
        .then((response: any) => {
          return response;
        })
        .catch((error) => {
          return Promise.reject(error);
        });
      }
      submitEmpReview(empIdDetails: any){
        return this.http.post(this.BaseEndpoint + '/Performance/SaveEmployeeReview',JSON.stringify(empIdDetails[0]), { headers:this.reqHeader })
        .toPromise()
        .then((response: any) => {
          return response;
        })
        .catch((error) => {
          return Promise.reject(error);
        });
      }
      submitManagerReview(empIdDetails: any){
        return this.http.post(this.BaseEndpoint + '/Performance/SaveManagerReview',JSON.stringify(empIdDetails[0]), { headers:this.reqHeader })
        .toPromise()
        .then((response: any) => {
          return response;
        })
        .catch((error) => {
          return Promise.reject(error);
        });
      }
      updateBehavior(BehaviorDetails: any){
      return this.http.post(this.BaseEndpoint + '/Performance/UpdateBehaviour',JSON.stringify(BehaviorDetails), { headers:this.reqHeader })
      .toPromise()
      .then((response: any) => {
        return response;
      })
      .catch((error) => {
        return Promise.reject(error);
      });
      }
      deleteBehavior(BehaviorDetails: any){
        return this.http.post(this.BaseEndpoint + '/Performance/DeleteBehaviour',JSON.stringify(BehaviorDetails), { headers:this.reqHeader })
        .toPromise()
        .then((response: any) => {
          return response;
        })
        .catch((error) => {
          return Promise.reject(error);
        });
        }
        getEmployeeDetails(empIdDetails: any){
          return this.http.post(this.BaseEndpoint + '/Performance/GetEmployeeDetails',JSON.stringify(empIdDetails), { headers:this.reqHeader })
          .toPromise()
          .then((response: any) => {
            return response;
          })
          .catch((error) => {
            return Promise.reject(error);
          });
        }
}