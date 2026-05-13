import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from 'src/assets/environment';

@Injectable({
    providedIn: 'root'
})

export class NotificationService {

    constructor(private http: HttpClient) { }

    GetUnreadCount(reqbody: any): Observable<any> {
        return this.http.post(`${environment.baseUrl}/Notification/GetUnreadCount`, reqbody)
    }
}