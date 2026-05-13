import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractAttendanceComponent } from './contract-attendance.component';

describe('ContractAttendanceComponent', () => {
  let component: ContractAttendanceComponent;
  let fixture: ComponentFixture<ContractAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractAttendanceComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ContractAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
