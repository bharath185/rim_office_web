import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from 'src/assets/environment';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class DashboardService {
    constructor(private http: HttpClient) { }

    employeeSelectEmployee(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/SelectEmployee`, reqbody)
    }

    employeeGetLocation(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/DDGetLocation`, reqbody)
    }

    DDselectEmployee(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/DDselectEmployee`, reqbody)
    }

    GetConsolidatedAttendanceData(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetConsolidatedAttendanceData`, reqbody)
    }

    attendanceDashboardLogin(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/OnSiteLogin`, reqbody)
    }

    attendanceDashboardLogout(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/OnSiteLogout`, reqbody)
    }

    dashboardLoginDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Login/LoginDetails`, reqbody)
    }

    GetAllLoginLogs(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetAllLoginLogs`, reqbody)
    }

    GetLoginLogs(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetLoginLogs`, reqbody)
    }

    DashboardDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/DashboardDetails`, reqbody)
    }
    GetEmployeeEvents(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Dashboard/GetEmployeeEvents`, reqbody)
    }
    GetAllHRCount(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Dashboard/GetAllHRCount`, reqbody)
    }
}
