import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Config } from 'src/app/configuration/env.config';
import { environment } from 'src/assets/environment';

@Injectable({
    providedIn: 'root'
})
export class SelfDevApiService {
    BaseEndpoint: any;
    reqHeader: HttpHeaders;
    userDetails: any;
    authorization: any;
    authKey: any;
    isLoggedIn = false;
    tokenkey: any;

    constructor(private http: HttpClient) {
        this.userDetails = sessionStorage.getItem('userdata');
        const sessionData = this.userDetails ? JSON.parse(this.userDetails)  : null;

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

    addSelfDev(selfDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/AddSelfDevelopment', JSON.stringify(selfDetails[0]), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    getALLSelfDev(empIdDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/GetAllSelfDevelopment', JSON.stringify(empIdDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    updateSelfDev(selfDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/UpdateSelfDevelopment', JSON.stringify(selfDetails[0]), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    deleteSelfDev(selfDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/DeleteSelfDevelopment', JSON.stringify(selfDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }



}