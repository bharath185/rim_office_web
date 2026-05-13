
// this is new one above one is old one
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ActivatedRouteSnapshot, CanActivate, NavigationEnd, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, OnInit {
  constructor(private readonly router: Router) { }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        console.log('NavigationEnd:', event.urlAfterRedirects);
      }
    });
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const url: string = state.url;
    return this.checkLogin(url);
  }

  checkLogin(url: string): boolean | UrlTree {
    // Directly handle the URL if it's for login or verification
    if (url.includes('/auth/signin') || url.includes('/verify_otp')) {
      return true;
    }
    const storedEmployeeData = sessionStorage.getItem('userdata');
    const loginStatus = storedEmployeeData ? JSON.parse(storedEmployeeData) : null;
    if (loginStatus && loginStatus.TokenId && loginStatus.UserAuth ) {
      return true;
    }
    else {
      this.router.navigate(['auth/signin']);
      sessionStorage.clear();
      return false;
    }
  }
}