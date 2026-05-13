import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Config } from 'src/app/configuration/env.config';
import { environment } from 'src/assets/environment';


@Injectable({
    providedIn: 'root'
})
export class GoalsApiService {
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
        // console.log(this.authKey);
        this.BaseEndpoint = Config.BaseEndpoint
        this.reqHeader = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': this.tokenkey,
            'AuthKey': this.authKey
        });
    }

    addGoal(GoalDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/AddGoal', JSON.stringify(GoalDetails[0]), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    getALLGoal(empIdDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/GetAllGoal', JSON.stringify(empIdDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    getALLGoalEmployee(empIdDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/GetAllGoalEmployee', JSON.stringify(empIdDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    updateGoal(empIdDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/UpdateGoal', JSON.stringify(empIdDetails[0]), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    AddAllGoals(empIdDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/AddAllGoal', JSON.stringify(empIdDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    ApproveAllGoal(empIdDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/ApproveAllGoal', JSON.stringify(empIdDetails[0]), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    deleteGoal(GoalDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/DeleteGoal', JSON.stringify(GoalDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    addTask(GoalDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/AddTask', JSON.stringify(GoalDetails[0]), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    updateTask(GoalDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/UpdateTask', JSON.stringify(GoalDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    deleteTask(GoalDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/DeleteTask', JSON.stringify(GoalDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }
    getAllTask(empDetails: any) {
        return this.http.post(environment.baseUrl + '/Performance/GetAllTask', JSON.stringify(empDetails), { headers: this.reqHeader })
            .toPromise()
            .then((response: any) => {
                return response;
            })
            .catch((error) => {
                return Promise.reject(error);
            });
    }


}