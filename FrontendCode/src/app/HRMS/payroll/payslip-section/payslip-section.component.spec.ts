import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayslipSectionComponent } from './payslip-section.component';

describe('PayslipSectionComponent', () => {
  let component: PayslipSectionComponent;
  let fixture: ComponentFixture<PayslipSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PayslipSectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PayslipSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
