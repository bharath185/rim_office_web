import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { environment } from 'src/assets/environment';

@Injectable({
    providedIn: 'root'
})


export class AttendenceModuleService {

    constructor(private http: HttpClient) { }

    employeeAddWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/AddWorkType`, reqbody)
    }

    EmployeeGetAllWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetAllWorkType`, reqbody)
    }

    EmployeeGetWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetWorkType`, reqbody)
    }

    EmployeeUpdateWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/UpdateWorkType`, reqbody)
    }

    EmployeeDeleteWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/DeleteWorkType`, reqbody)
    }
 
    EmployeeApproveWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/ApproveWorkType`, reqbody)
    }

    EmployeeRejectWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/RejectWorkType`, reqbody)
    }

    EmployeeDDEmployeeApprover(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/DDEmployeeApprover`, reqbody)
    }

    EmployeeGetAllApproverWorkType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetAllApproverWorkType`, reqbody)
    }

    EmployeeGetAllWorkTypeFilter(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetAllWorkTypeFilter`, reqbody)
    }

    employeeGetAllWFHDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/WFHLogin/GetAllWFHDetails`, reqbody)
    }

    employeeGetAllWFHFilterDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/WFHLogin/GetAllWFHFilterDetails`, reqbody)
    }

    ShiftAddShift(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/AddShift`, reqbody)
    }
    ShiftGetAllShift(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/GetAllShift`, reqbody)
    }

    ShiftGetShift(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/GetShift`, reqbody)
    }

    ShiftUpdateShift(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/UpdateShift`, reqbody)
    }
    ShiftDeleteShift(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/DeleteShift`, reqbody)
    }

    ShiftGetAllShiftGrouping(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/GetAllShiftGrouping`, reqbody)
    }
    ShiftAddShiftGrouping(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/AddShiftGrouping`, reqbody)
    }

    ShiftDDShift(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/DDShift`, reqbody)
    }

    ShiftGetAllShiftEmployee(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/GetAllShiftEmployee`, reqbody)
    }

    ShiftAddShiftEmployee(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/AddShiftEmployee`, reqbody)
    }

    ShiftRemoveShiftEmployee(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Shift/RemoveShiftEmployee`, reqbody)
    }

    EmployeeEmployeeAttendance(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/EmployeeAttendance`, reqbody)
    }

    EmployeeAttendanceFilter(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/AttendanceFilter`, reqbody)
    }

    EmployeeEachEmployeeAttendance(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/EachEmployeeAttendance`, reqbody)
    }

    EmployeeReportingEmployeeAttendance(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/ReportingEmployeeAttendance`, reqbody)
    }

//     API: Employee/ReportingEmployeeAttendance
// Payload:
// {
//     "LoginId": 149,
//     "CompId": 1,
//     "LEId": "1",
//     "BUId": 0,
//     "LocId": 1,
//     "StartDate": null,
//     "EndDate": null,
//     "DeptId": 0,
//     "DesignationId": 0,
//     "EmpTypeId": 0,
//     "EmpId": 0
// }

    EmployeeGetOnSiteData(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetOnSiteData`, reqbody)
    }

    EmployeeAddOnSiteData(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/AddOnSiteData`, reqbody)
    }

    employeeUploadAttendance(LoginId: number, EmpId: number, FileName: string, file: File,): Observable<any> {
        const formData = new FormData();
        formData.append('LoginId', LoginId.toString());
        formData.append('EmpId', EmpId.toString());
        formData.append('FileName', FileName);
        formData.append('File', file);
        return this.http.post(`${environment.baseUrl}/Employee/UploadAttendance`, formData)
    }

    employeeGetAllManualAttendance(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/GetAllManualAttendance`, reqbody)
    }
    employeeUploadSingleAttendance(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/UploadSingleAttendance`, reqbody)
    }
    employeeDDEmpList(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/DDEmpList`, reqbody)
    }
    employeeUploadMultiAttendance(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Employee/UploadMultiAttendance`, reqbody)
    }
}