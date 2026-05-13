import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from 'src/assets/environment';

@Injectable({
    providedIn: 'root'
})

export class leavesService {
    constructor(private http: HttpClient) { }

    GetAllLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllLeaveType`, reqbody)
    }

    GetLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetLeaveType`, reqbody)
    }

    DeleteLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DeleteLeaveType`, reqbody)
    } 

    AddLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/AddLeaveType`, reqbody)
    }
    UpdateLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/UpdateLeaveType`, reqbody)
    }
    DDLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DDLeaveType`, reqbody)
    }
    GetAllLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllLeave`, reqbody)
    }
    ApplyLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/ApplyLeave`, reqbody)
    }
    IndividualLeaveCount(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/IndividualLeaveCount`, reqbody)
    }
    CancelLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/CancelLeave`, reqbody)
    }
    DeleteDraftLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DeleteDraftLeave`, reqbody)
    }
    WithDrawLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/WithDrawLeave`, reqbody)
    }
    GetAllHRLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllHRLeave`, reqbody)
    }
    GetAllApplyHRLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllApplyHRLeave`, reqbody)
    }
    ApproveLeaveByHR(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/ApproveLeaveByHR`, reqbody)
    }
    RejectLeaveByHR(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/RejectLeaveByHR`, reqbody)
    }
    GetAllManagerLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllManagerLeave`, reqbody)
    }

    GetAllApplyManagerLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllApplyManagerLeave`, reqbody)
    }
    ApproveLeaveByManager(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/ApproveLeaveByManager`, reqbody)
    }
    RejectLeaveByManager(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/RejectLeaveByManager`, reqbody)
    }
    DraftApplyLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DraftApplyLeave`, reqbody)
    }
    DraftLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DraftLeave`, reqbody)
    }
    ActivateLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/ActivateLeaveType`, reqbody)
    }
    DeactivateLeaveType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DeactivateLeaveType`, reqbody)
    }
    DDApproveManager(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/DDApproveManager`, reqbody)
    }
    CompOffLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/CompOffLeave`, reqbody)
    }
    CompOffHours(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/CompOffHours`, reqbody)
    }
    GetAllCompOffLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllCompOffLeave`, reqbody)
    }
    GetAllEmpCompOffLeave(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/GetAllEmpCompOffLeave`, reqbody)
    }
    ApproveCompOff(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/ApproveCompOff`, reqbody)
    }
    RejectCompOff(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/RejectCompOff`, reqbody)
    }
    LeaveBalReport(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Leave/LeaveBalReport`, reqbody)
    }

    UploadFileLeave(empId: number, DocName: string, files: File[]): Observable<any> {
        const formData = new FormData();
        formData.append('EmpId', empId.toString());
        formData.append('DocName', DocName);

        files.forEach((file, index) => {
            formData.append('Files', file); // Or use `Files[]` if backend expects array
        });

        return this.http.post(`${environment.baseUrl}/Leave/UploadFileLeave`, formData);
    }

}