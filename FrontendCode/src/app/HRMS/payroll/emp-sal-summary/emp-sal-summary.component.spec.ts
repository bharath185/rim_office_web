import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpSalSummaryComponent } from './emp-sal-summary.component';

describe('EmpSalSummaryComponent', () => {
  let component: EmpSalSummaryComponent;
  let fixture: ComponentFixture<EmpSalSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmpSalSummaryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmpSalSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
