import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryStatutoryComponent } from './salary-statutory.component';

describe('SalaryStatutoryComponent', () => {
  let component: SalaryStatutoryComponent;
  let fixture: ComponentFixture<SalaryStatutoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SalaryStatutoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SalaryStatutoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
