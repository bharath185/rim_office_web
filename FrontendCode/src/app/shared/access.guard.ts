import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HrmsServiceService } from 'src/app/HRMS/hrms-service.service';

@Injectable({
  providedIn: 'root'
})
export class AccessGuard implements CanActivate {
  constructor(
    private router: Router,
    private hrmsService: HrmsServiceService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const currentUser = JSON.parse(sessionStorage.getItem('userdata') || '{}');
    const currentUrl = state.url;
    return this.hrmsService.accessGetAccessPolicy({ EmpId: currentUser.EmpId }).pipe(
      map((accessPolicy: any) => {
        const allowedPageNames = accessPolicy.map((access: any) => access.PageName);
        const hasAccess = allowedPageNames.includes(this.getPageNameFromUrl(currentUrl));

        if (!hasAccess) {
          this.router.navigate(['auth/signin']);  
          return false;
        }
        this.router.navigate(['das']);
        return true;
      }),
      catchError(() => {
        this.router.navigate(['auth/signin']);  
        return of(false); 
      })
    );
  }

  private getPageNameFromUrl(url: string): string {
    return url.split('/').pop() || '';
  }
}
