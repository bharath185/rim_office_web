import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeProbationReportComponent } from './employee-probation-report.component';

describe('EmployeeProbationReportComponent', () => {
  let component: EmployeeProbationReportComponent;
  let fixture: ComponentFixture<EmployeeProbationReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmployeeProbationReportComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmployeeProbationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
