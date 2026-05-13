import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpMasReportComponent } from './emp-mas-report.component';

describe('EmpMasReportComponent', () => {
  let component: EmpMasReportComponent;
  let fixture: ComponentFixture<EmpMasReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmpMasReportComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmpMasReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
