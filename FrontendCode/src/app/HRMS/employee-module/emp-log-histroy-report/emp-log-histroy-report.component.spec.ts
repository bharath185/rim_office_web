import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpLogHistroyReportComponent } from './emp-log-histroy-report.component';

describe('EmpLogHistroyReportComponent', () => {
  let component: EmpLogHistroyReportComponent;
  let fixture: ComponentFixture<EmpLogHistroyReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmpLogHistroyReportComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmpLogHistroyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
