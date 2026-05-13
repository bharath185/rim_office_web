import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaySlipSectionComponent } from './pay-slip-section.component';

describe('PaySlipSectionComponent', () => {
  let component: PaySlipSectionComponent;
  let fixture: ComponentFixture<PaySlipSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaySlipSectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PaySlipSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
