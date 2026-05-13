import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendanceContractComponent } from './attendance-contract.component';

describe('AttendanceContractComponent', () => {
  let component: AttendanceContractComponent;
  let fixture: ComponentFixture<AttendanceContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AttendanceContractComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AttendanceContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
