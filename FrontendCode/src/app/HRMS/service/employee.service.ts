import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from 'src/assets/environment';

@Injectable({
  providedIn: 'root'
})

export class EmployeeModuleService {
  constructor(private http: HttpClient) { }

  employeeGetAllEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllEmployee`, reqbody)
  }

  employeeFetchEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/FetchEmployee`, reqbody)
  }

  employeeDDCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDCompany`, reqbody)
  }

  employeeDDLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDLegalEntity`, reqbody)
  }

  employeeDDBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDBusinessUnit`, reqbody)
  }

  employeeDDLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDLocation`, reqbody)
  }

  employeeDDSalutation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDSalutation`, reqbody)
  }

  employeeDDGender(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDGender`, reqbody)
  }
  employeeDDApprover(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDApprover`, reqbody)
  }
  employeeDDEmpType(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDEmpType`, reqbody)
  }
  employeeGetEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetEmployee`, reqbody)
  }

  employeeAddEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddEmployee`, reqbody)
  }

  employeeUpdateEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateEmployee`, reqbody)
  }

  employeeDeleteEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteEmployee`, reqbody)
  }

  employeeActiveEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/ActiveEmployee`, reqbody)
  }
  employeeDeActiveEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeActiveEmployee`, reqbody)
  }

  employeeGetContactDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetEmployeeContactInformation`, reqbody)
  }

  employeeGetDDEducationDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDEducationDoc`, reqbody)
  }

  employeeGetDDGovtDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDGovtDoc`, reqbody)
  }

  employeeAddContactDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddEmployeeContactInformation`, reqbody)
  }

  employeeUpdateContactDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateEmployeeContactInformation`, reqbody)
  }

  EmployeeUploadFileCareer(empId: number, DocName: string, file: File,): Observable<any> {
    const formData = new FormData();
    formData.append('EmpId', empId.toString());
    formData.append('DocName', DocName);
    formData.append('File', file);
    return this.http.post(`${environment.baseUrl}/Employee/UploadFileCareer`, formData)
  }
  employeeGetEmpCareerDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetEmpCareerDetails`, reqbody)
  }

  employeeAddEmpCareerDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddEmpCareerDetails`, reqbody)
  }

  employeeUpdateEmpCareerDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateEmpCareerDetails`, reqbody)
  }

  employeeDeleteEmpCareerDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteEmpCareerDetails`, reqbody)
  }

  employeeUploadImage(empId: any, file: File, imageType: string = 'PROFILEPIC'): Observable<any> {

    const formData = new FormData();
    formData.append('EmpId', empId);
    formData.append('File', file);

    // ✅ dynamic now
    formData.append('ImageType', imageType);

    return this.http.post(`${environment.baseUrl}/Employee/UploadImage`, formData);
  }

  employeeUploadEducFileDoc(empId: number, DocName: string, file: File,): Observable<any> {
    const formData = new FormData();
    formData.append('EmpId', empId.toString());
    formData.append('DocName', DocName);
    formData.append('File', file);
    return this.http.post(`${environment.baseUrl}/Employee/UploadFileEducation`, formData)
  }

  employeeAddEducationDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddEducationDoc`, reqbody)
  }

  employeeUploadGovtFileDoc(empId: number, DocName: string, file: File,): Observable<any> {
    const formData = new FormData();
    formData.append('EmpId', empId.toString());
    formData.append('DocName', DocName);
    formData.append('File', file);
    return this.http.post(`${environment.baseUrl}/Employee/UploadFileGovt`, formData)
  }

  employeeGetEducationDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetEducationDoc`, reqbody)
  }

  employeeUpdateEducationDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateEducationDoc`, reqbody)
  }

  employeeDeleteEducationDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteEducationDoc`, reqbody)
  }

  GetAllEmpAccDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllEmpAccDetails`, reqbody)
  }
  GetEmpAccDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetEmpAccDetails`, reqbody)
  }

  UpdateEmpAccDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateEmpAccDetails`, reqbody)
  }
  DeleteEmpAccDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteEmpAccDetails`, reqbody)
  }
  employeeAddEmpAccDetails(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddEmpAccDetails`, reqbody)
  }

  employeeGetGovtDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetGovtDoc`, reqbody)
  }

  employeeAddGovtDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddGovtDoc`, reqbody)
  }

  employeeUpdateGovtDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateGovtDoc`, reqbody)
  }

  employeeUpdateDeleteDoc(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteGovtDoc`, reqbody)
  }

  employeeRelievedEmployee(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/RelievedEmployee`, reqbody)
  }
  employeeSPAttendance(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/SPAttendance`, reqbody)
  }
  employeeDDAuthorisedEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DDAuthorisedEntity`, reqbody)
  }

  NewDDCompany(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/NewDDCompany`, reqbody)
  }
  NewDDLegalEntity(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/NewDDLegalEntity`, reqbody)
  }
  NewDDBusinessUnit(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/NewDDBusinessUnit`, reqbody)
  }
  NewDDLocation(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/NewDDLocation`, reqbody)
  }

  employeeAttendanceDeptReport(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AttendanceDeptReport`, reqbody)
  }

  ApprovedbyManager(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/ApprovedbyManager`, reqbody)
  }
  LogoutbyManager(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/LogoutbyManager`, reqbody)
  }
  ApprovedHrbyManager(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/ApprovedHrbyManager`, reqbody)
  }
  GetAllEmpProbationTrackingHistory(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllEmpProbationTrackingHistory`, reqbody)
  }
  DDReporterList(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DDReporterList`, reqbody)
  }
  DDEmployeeList(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DDEmployeeList`, reqbody)
  }
  ConfirmProbation(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/ConfirmProbation`, reqbody)
  }
  GetAllEmployeeLogHistory(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllEmployeeLogHistory`, reqbody)
  }


}