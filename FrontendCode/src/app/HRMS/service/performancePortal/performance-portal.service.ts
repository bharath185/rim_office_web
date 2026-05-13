import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from 'src/assets/environment';

@Injectable({
  providedIn: 'root'
})
export class PerformancePortalService {
  reqHeader: HttpHeaders;

  constructor(private http: HttpClient) { 
    this.reqHeader = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: 'OfficeConnect'
    });
  }

  getALLGoal(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/GetAllGoal`, reqbody)
  }

  getALLBehaviors(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/GetAllBehaviour`, reqbody)
  }

  getAllReviewList(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/GetEmployeeReviewList`, reqbody)
  }
  
  getEmployeeDetails(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/GetEmployeeDetails`, reqbody)
  }

  submitEmpReview(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/SaveEmployeeReview`, reqbody)
  }

  addTask(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/AddTask`, reqbody)
  }

  AddAllGoals(reqBody:any){
    return this.http.post(`${environment.baseUrl}/Performance/AddAllGoal`, reqBody)
  }

  deleteGoal(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/DeleteGoal`, reqbody)
  }

  updateGoal(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/UpdateGoal`, reqbody)
  }

  DDYearPer(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/DDFYear`, reqbody)
  }

  DDQuater(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/DDQuater`, reqbody)
  }

  DDReviewStatus(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/DDReviewStatus`, reqbody)
  }

  PerformanceReport(reqbody: any) {
    return this.http.post(`${environment.baseUrl}/Performance/PerformanceReport`, reqbody)
  }

  getUserDetais(empdetails: any) {
    return this.http
      .post(environment.baseUrl + '/Performance/GetEmployeeDetails', JSON.stringify(empdetails), { headers: this.reqHeader })
      .toPromise()
      .then((response: any) => {
        return response;
      })
      .catch((error) => {
        return Promise.reject(error);
      });
  }
}
