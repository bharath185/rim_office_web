import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EntityStateService {

  private selectedEntityIdSource = new BehaviorSubject<any>(null);
  selectedEntityId$ = this.selectedEntityIdSource.asObservable();

  setEntityId(leId: number) {
    this.selectedEntityIdSource.next(leId);

    // optional: persist in sessionStorage
    sessionStorage.setItem('SelectedLEId', leId.toString());
  }

  getEntityId(): number {
    return this.selectedEntityIdSource.value;
  }
}
