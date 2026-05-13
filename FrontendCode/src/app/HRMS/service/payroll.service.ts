import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from 'src/assets/environment';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class payRollService {

    constructor(private http: HttpClient) { }

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

    DDPayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollPayoutType`, reqbody)
    }
    AddPayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayrollPayoutType`, reqbody)
    }
    GetAllPayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayrollPayoutType`, reqbody)
    }
    GetAllComponentDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllComponentDetails`, reqbody)
    }
    DDPayrollFrequency(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollFrequency`, reqbody)
    }
    GetPayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetPayrollPayoutType`, reqbody)
    }
    UpdatePayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayrollPayoutType`, reqbody)
    }
    DeletePayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayrollPayoutType`, reqbody)
    }
    ActivatePayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/ActivatePayrollPayoutType`, reqbody)
    }
    DeactivatePayrollPayoutType(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeactivatePayrollPayoutType`, reqbody)
    }
    DDPayrollSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollSegment`, reqbody)
    }
    GetAllPayrollSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayrollSegment`, reqbody)
    }
    GetAllPayrollPayoutTypeSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayrollPayoutTypeSegment`, reqbody)
    }
    GetPayrollSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetPayrollSegment`, reqbody)
    }
    AddPayrollSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayrollSegment`, reqbody)
    }
    UpdatePayrollSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayrollSegment`, reqbody)
    }
    DeletePayrollSegment(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayrollSegment`, reqbody)
    }
    DDPayrollSymbols(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollSymbols`, reqbody)
    }
    DDPayrollComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollComponent`, reqbody)
    }
    AddComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddComponent`, reqbody)
    }
    EmpCTCCalculation(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/EmpCTCCalculation`, reqbody)
    }
    DDPayrollEmpList(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollEmpList`, reqbody)
    }
    DDPayslipSection(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayslipSection`, reqbody)
    }
    GetAllPayslipSection(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayslipSection`, reqbody)
    }
    GetPayslipSection(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetPayslipSection`, reqbody)
    }
    AddPayslipSection(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayslipSection`, reqbody)
    }
    UpdatePayslipSection(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayslipSection`, reqbody)
    }
    DeletePayslipSection(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayslipSection`, reqbody)
    }
    GetAllEmployeeSalaryDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllEmployeeSalaryDetails`, reqbody)
    }
    GetEmployeeSalaryDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetEmployeeSalaryDetails`, reqbody)
    }
    AddEmployeeSalaryDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddEmployeeSalaryDetails`, reqbody)
    }
    UpdateEmployeeSalaryDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdateEmployeeSalaryDetails`, reqbody)
    }
    DeleteEmployeeSalaryDetails(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeleteEmployeeSalaryDetails`, reqbody)
    }
    GetAllPayslipSectionComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayslipSectionComponent`, reqbody)
    }
    GetPayslipSectionComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetPayslipSectionComponent`, reqbody)
    }
    AddPayslipSectionComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayslipSectionComponent`, reqbody)
    }
    UpdatePayslipSectionComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayslipSectionComponent`, reqbody)
    }
    DeletePayslipSectionComponent(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayslipSectionComponent`, reqbody)
    }
    EmpPayslipGeneration(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/EmpPayslipGeneration`, reqbody)
    }
    AddPayoutMappingMaster(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayoutMappingMaster`, reqbody)
    }
    GetAllPayoutMappingMaster(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayoutMappingMaster`, reqbody)
    }
    GetPayoutMappingMaster(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetPayoutMappingMaster`, reqbody)
    }
    UpdatePayoutMappingMaster(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayoutMappingMaster`, reqbody)
    }
    DeletePayoutMappingMaster(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayoutMappingMaster`, reqbody)
    }
    PayrollReportforALL(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/PayrollReportforALL`, reqbody)
    }
    DDLegalEntity(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDLegalEntity`, reqbody)
    }
    payrollDDLocation(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDLocation`, reqbody)
    }
    DDPayrollVariable(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DDPayrollVariable`, reqbody)
    }
    GetAllPayrollVariable(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetAllPayrollVariable`, reqbody)
    }
    GetPayrollVariable(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/GetPayrollVariable`, reqbody)
    }
    AddPayrollVariable(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayrollVariable`, reqbody)
    }
    UpdatePayrollVariable(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayrollVariable`, reqbody)
    }
    DeletePayrollVariable(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayrollVariable`, reqbody)
    }
    PayrollVariableHistory(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/PayrollVariableHistory`, reqbody)
    }
    AddPayrollVariableHistory(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/AddPayrollVariableHistory`, reqbody)
    }
    UpdatePayrollVariableHistory(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/UpdatePayrollVariableHistory`, reqbody)
    }
    DeletePayrollVariableHistory(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Payroll/DeletePayrollVariableHistory`, reqbody)
    }
   
}