import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { environment } from 'src/assets/environment';


@Injectable({
  providedIn: 'root'
})
export class HrmsServiceService {

  constructor(private readonly http: HttpClient) { }

  logoutApi(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Login/LogOut`, reqbody)
  }

  access_DD_department(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDDept`, reqbody)
  }

  access_DD_Role(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDRole`, reqbody)
  }

  access_DD_Grade(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDGrade`, reqbody)
  }

  GetAllPages(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllPages`, reqbody)
  }
  SubmitAccessControls(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/SubmitAccessControls`, reqbody,)
  }
  DeletePageModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DeletePageModules`, reqbody,)
  }
  UpdatePageModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/UpdatePageModules`, reqbody,)
  }
  employeeGetLocation(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DDGetLocation`, reqbody)
  }

  access_DDDesignation(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDDesignation`, reqbody)
  }

  access_Module(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDModule`, reqbody)
  }

  access_Sub_Module(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDSubModule`, reqbody)
  }

  access_Page_Module(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDPageModule`, reqbody)
  }

  AccessDDDeptEmployee(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDDeptEmployee`, reqbody)
  }

  LoginCheckAuth(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Login/CheckAuth`, reqbody)
  }

  LoginChangePassword(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Login/ChangePassword`, reqbody)
  }

  GetEmployeeDetails(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/GetEmployeeDetails`, reqbody)
  }

  accessGetAccessPolicy(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAccessPolicy`, reqbody)
  }

  getAllAccess(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAllAccess`, reqbody,)
  }

  addAccess(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/AddAccess`, reqbody)
  }

  EditAccess(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/UpdateAccess`, reqbody)
  }

  DeleteAccess(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DeleteAccess`, reqbody)
  }

  addDepartmentData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/AddDept`, reqbody)
  }
  getAllDepartmentData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAllDept`, reqbody)
  }

  updateDepartmentData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/UpdateDept`, reqbody)
  }

  deleteDepartmentData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DeleteDept`, reqbody)
  }

  getAllRoleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAllRole`, reqbody)
  }

  addRoleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/AddRole`, reqbody)
  }

  updateRoleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/UpdateRole`, reqbody)
  }

  deleteRoleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DeleteRole`, reqbody)
  }

  getAllModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAllModule`, reqbody)
  }

  addModuleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/AddModule`, reqbody)
  }

  updateModuleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/UpdateModule`, reqbody)
  }

  deleteModuleData(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DeleteModule`, reqbody)
  }

  getAllSubModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAllSubModule`, reqbody)
  }

  addSubModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/AddSubModule`, reqbody)
  }

  updateSubModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/UpdateSubModule`, reqbody)
  }

  deleteSubModule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DeleteSubModule`, reqbody)
  }

  getAllPagemodule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/GetAllPageModule`, reqbody)
  }

  addPagemodule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/AddPageModule`, reqbody)
  }

  updatePagemodule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/UpdatePageModule`, reqbody)
  }

  deletePagemodule(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DeletePageModule`, reqbody)
  }

  visitorAccessDDEmployee(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDEmployee`, reqbody)
  }

  visitorAccessDDCompany(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Access/DDCompany`, reqbody)
  }

  visitorDDEmployee() {
    return this.http.post(`${environment.baseUrl}/Visitor/DDEmployee`, '')
  }

  visitorDDCompany() {
    return this.http.post(`${environment.baseUrl}/Visitor/DDCompany`, '')
  }

  visitorInvitevisit(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/InviteVisit`, reqbody)
  }

  visitorDirectCheckIn(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/DirectCheckIn`, reqbody)
  }
  visitorVisitorSelftCheckIn(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/VisitorDirectCheckIn`, reqbody)
  }

  // visitorVerifyOTP(reqbody: any) {
  //   const headers = new HttpHeaders({
  //     Authorization: "Visitors",
  //   })
  //   return this.http.post(`${environment.baseUrl}/Visitor/VerifyOTP`, reqbody,
  //     { headers }
  //   )
  // }

  visitorVerifyOTP(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/VerifyOTP`, reqbody)
  }

  visitorVerifyOTPCheckIn(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/VerifyOTPCheckIn`, reqbody)
  }

  visitorAcceptInvite(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/AcceptInvite`, reqbody)
  }

  visitorSelfCheckIn(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/VisitorCheckIn`, reqbody)
  }

  visitorSelfCheckOut(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/VisitorCheckOut`, reqbody)
  }

  visitorGetAllInvite(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/GetAllInvite`, reqbody)
  }

  VisitorFilter(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/VisitFilter`, reqbody)
  }

  // VisitorVisitExportCSV(reqBody: any): Observable<Blob> {
  // const headers = new HttpHeaders({
  //   Authorization: sessionStorage.getItem('token') || '',
  //   AuthKey: sessionStorage.getItem('userAuth') || '',
  // });
  //   return this.http.post(`${environment.baseUrl}/Visitor/VisitExportCSV`, reqBody, {
  //     headers: headers,
  //     responseType: 'blob' // Specify that you expect a Blob
  //   });
  // }

  VisitorVisitExportCSV(reqBody: any): Observable<Blob> {
    return this.http.post(`${environment.baseUrl}/Visitor/VisitExportCSV`, reqBody, {
      responseType: 'blob' // Specify that you expect a Blob
    });
  }

  VisitorVisitExportExcel(reqBody: any): Observable<Blob> {
    return this.http.post(`${environment.baseUrl}/Visitor/VisitExportExcel`, reqBody, {
      responseType: 'blob' // Specify that you expect a Blob
    });
  }

  visitorCheckIn(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/CheckIn`, reqbody)
  }

  visitorCheckOut(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/CheckOut`, reqbody)
  }

  visitorFileUploadImage(empName: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('Visitor', empName);
    formData.append('File', file);
    return this.http.post(`${environment.baseUrl}/Visitor/UploadFileVisitor`, formData)
  }

  VisitorGetAllEmployeeInvite(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/GetAllEmployeeInvite`, reqbody)
  }

  VisitorCancelInvite(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Visitor/CancelInvite`, reqbody)
  }

  ViewScreenShots(reqbody: any): Observable<Blob> {
    return this.http.post(`${environment.baseUrl}/Performance/ViewScreenShots`, reqbody, {
      responseType: 'blob'
    })
  }

  GetAllWFHAnalysis(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/WFHLogin/GetAllWFHAnalysis`, reqbody)
  }

  SaveWFHAnalysis(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/WFHLogin/SaveWFHAnalysis`, reqbody)
  }
  DDVendorList(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DDVendorList`, reqbody)
  }
  DDSiteList(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DDSiteList`, reqbody)
  }
  DDProjectList(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/DDProjectList`, reqbody)
  }
  ContractAttendanceChecking(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/ContractAttendanceChecking`, reqbody)
  }
  AddContractAttendance(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/AddContractAttendance`, reqbody)
  }

  ContractAttendanceManager(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Employee/ContractAttendanceManager`, reqbody)
  }

  erpContractAttendanceVendor() {
    return this.http.get('http://erp.3dcad-global.com:8071/api/manpower_vendors')
    // local http://192.168.100.86:8014/api/manpower_vendors
  }
  erpContractAttendanceProject() {
    return this.http.get('http://erp.3dcad-global.com:8071/api/project_codes')
  }

}
