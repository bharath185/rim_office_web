import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportingEmpAttendanceComponent } from './reporting-emp-attendance.component';

describe('ReportingEmpAttendanceComponent', () => {
  let component: ReportingEmpAttendanceComponent;
  let fixture: ComponentFixture<ReportingEmpAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportingEmpAttendanceComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReportingEmpAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
