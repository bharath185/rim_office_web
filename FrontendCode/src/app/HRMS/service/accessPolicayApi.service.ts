import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccessPolicyStoreService {
  private accessPolicySubject = new BehaviorSubject<any[]>([]);

  setAccessPolicy(policy: any[]) {
    this.accessPolicySubject.next(policy);
  }

  getAccessPolicy(): Observable<any[]> {
    return this.accessPolicySubject.asObservable();
  }

  getCurrentAccessPolicy(): any[] {
    return this.accessPolicySubject.getValue();
  }
}
