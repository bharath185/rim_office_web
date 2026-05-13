import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpFinancialDetailsComponent } from './emp-financial-details.component';

describe('EmpFinancialDetailsComponent', () => {
  let component: EmpFinancialDetailsComponent;
  let fixture: ComponentFixture<EmpFinancialDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmpFinancialDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmpFinancialDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
