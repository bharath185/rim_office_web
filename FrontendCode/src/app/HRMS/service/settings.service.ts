import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from 'src/assets/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(private http: HttpClient) { }

  GetAllCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetAllCompany`, reqbody)
  }
  AddCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/AddCompany`, reqbody)
  }
  UpdateCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/UpdateCompany`, reqbody)
  }
  DeleteCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeleteCompany`, reqbody)
  }
  ActivateCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/ActivateCompany`, reqbody)
  }
  DeActivateCompany(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeActivateCompany`, reqbody)
  }


  GetAllLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetAllLegalEntity`, reqbody)
  }
  AddLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/AddLegalEntity`, reqbody)
  }
  UpdateLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/UpdateLegalEntity`, reqbody)
  }
  DeleteLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeleteLegalEntity`, reqbody)
  }
  ActivateLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/ActivateLegalEntity`, reqbody)
  }
  DeActivateLegalEntity(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeActivateLegalEntity`, reqbody)
  }

  GetAllBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetAllBusinessUnit`, reqbody)
  }
  AddBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/AddBusinessUnit`, reqbody)
  }
  UpdateBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/UpdateBusinessUnit`, reqbody)
  }
  DeleteBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeleteBusinessUnit`, reqbody)
  }
  ActivateBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/ActivateBusinessUnit`, reqbody)
  }
  DeActivateBusinessUnit(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeActivateBusinessUnit`, reqbody)
  }

  GetAllLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetAllLocation`, reqbody)
  }
  AddLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/AddLocation`, reqbody)
  }
  UpdateLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/UpdateLocation`, reqbody)
  }
  ActivateLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/ActivateLocation`, reqbody)
  }
  DeActivateLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeActivateLocation`, reqbody)
  }
  DeleteLocation(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeleteLocation`, reqbody)
  }


  CreateCompanySetting(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/CreateCompanySetting`, reqbody)
  }
  employeeGetAllFinanceMaster(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllFinanceMaster`, reqbody)
  }

  EmployeeAddHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/AddHoliday`, reqbody)
  }

  employeeGetAllHolidays(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllHolidays`, reqbody)
  }

  employeeCreateHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/CreateHoliday`, reqbody)
  }

  employeeUpdateHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateHoliday`, reqbody)
  }

  employeeDeleteHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteHoliday`, reqbody)
  }

  employeeGetHolidayById(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetHolidayById`, reqbody)
  }

  employeeCreateWeekHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/CreateWeekHoliday`, reqbody)
  }

  employeeUpdateWeekHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/UpdateWeekHoliday`, reqbody)
  }

  employeeDeleteWeekHoliday(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/DeleteWeekHoliday`, reqbody)
  }

  employeeGetAllWeekHolidays(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetAllWeekHolidays`, reqbody)
  }

  employeeGetWeekHolidayById(reqbody: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetWeekHolidayById`, reqbody)
  }

  getAllCalendarYears(loginId: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetAllCalendarYear`, {
      LoginId: loginId
    });
  }

  getCalendarYear(loginId: number, id: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetCalendarYear`, {
      LoginId: loginId,
      Id: id
    });
  }

  addCalendarYear(loginId: number, year: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/AddCalendarYear`, {
      LoginId: loginId,
      Year: year
    });
  }

  updateCalendarYear(loginId: number, id: number, year: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/UpdateCalendarYear`, {
      LoginId: loginId,
      Id: id,
      Year: year
    });
  }

  deleteCalendarYear(loginId: number, id: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeleteCalendarYear`, {
      LoginId: loginId,
      Id: id
    });
  }

  // ===================== FINANCIAL YEAR ===================== //

  getAllFinancialYears(loginId: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetAllFinancialYear`, {
      LoginId: loginId
    });
  }

  getFinancialYear(loginId: number, yearId: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/GetFinancialYear`, {
      LoginId: loginId,
      YearId: yearId
    });
  }

  addFinancialYear(loginId: number, financialYear: string): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/AddFinancialYear`, {
      LoginId: loginId,
      FinancialYear: financialYear
    });
  }

  updateFinancialYear(loginId: number, yearId: number, financialYear: string): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/UpdateFinancialYear`, {
      LoginId: loginId,
      YearId: yearId,
      FinancialYear: financialYear
    });
  }

  deleteFinancialYear(loginId: number, yearId: number): Observable<any> {
    return this.http.post(`${environment.baseUrl}/BusinessEntity/DeleteFinancialYear`, {
      LoginId: loginId,
      YearId: yearId
    });
  }

  getAllOrgDetails(payload: any): Observable<any> {
    return this.http.post(`${environment.baseUrl}/Employee/GetDesignationHierarchy`, payload);
  }
  
}
