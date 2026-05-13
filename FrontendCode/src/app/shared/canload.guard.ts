import { Injectable } from '@angular/core';
import { CanLoad, Route, Router, UrlSegment } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class canLoadModule implements CanLoad {

    userData
    constructor(private router: Router) {
        const storedEmployeeData = sessionStorage.getItem('userdata');
        this.userData = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    }

    canLoad(route: Route, segments: UrlSegment[]): boolean {
        if( (this.userData.DeptName === "IT & Admin") || (this.userData.DeptName === "Digital Manufacturing") || (this.userData.DeptName === "Sales & Marketing")){
            console.log('1');
            return true
          }
          else if ((this.userData.DeptName === 'Management') || (this.userData.DeptName === 'Accounts')) {
            console.log('2');
            return true; 
          }  
           else {
            this.router.navigate(['auth/signin']); 
            return false;
          }
    }
}
