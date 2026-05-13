import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Config } from 'src/app/configuration/env.config';
import { environment } from 'src/assets/environment';

@Injectable({
  providedIn: 'root'
})
export class ConfigrationService {

  BaseEndpoint: any;
    reqHeader: HttpHeaders;
    userDetails: any;
    authorization: any;
    authKey: any;
    isLoggedIn = false;
    tokenkey: any;

    constructor(private http: HttpClient) {
      this.userDetails = sessionStorage.getItem('userdata');
      const sessionData = this.userDetails ? JSON.parse(this.userDetails) : null;

        // Retrieve employeeId from session data
        //  this.authorization= sessionData.EmpId;
        this.authKey = sessionData.UserAuth;
        this.tokenkey = sessionData.TokenId;
        console.log(this.authKey);
        this.BaseEndpoint = Config.BaseEndpoint
        this.reqHeader = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': this.tokenkey,
            'AuthKey': this.authKey
        });
    }

    getALLDDFyear(data: any) {
        return this.http.post(environment.baseUrl + '/Performance/DDFYear', JSON.stringify(data), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    GetQuaterDetails(data: any) {
        return this.http.post(environment.baseUrl + '/Performance/GetQuaterDetails', JSON.stringify(data), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }

    submitQuaterDetails(data: any) {
        return this.http.post(environment.baseUrl + '/Performance/SubmitConfigSetup', JSON.stringify(data), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }

    
    getMainTableFinancialYear(data: any) {
        return this.http.post(environment.baseUrl + '/Performance/GetAllConfigSetup', JSON.stringify(data), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    updateConfigsetupReviewExtDate(data: any) {
        return this.http.post(environment.baseUrl + '/Performance/UpdateConfigSetup', JSON.stringify(data), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    updateConfigsetupGoalExtDate(data: any) {
        return this.http.post(environment.baseUrl + '/Performance/UpdateConfigSetup', JSON.stringify(data), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }

}
