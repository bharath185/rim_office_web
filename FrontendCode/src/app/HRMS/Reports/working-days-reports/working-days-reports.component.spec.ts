import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkingDaysReportsComponent } from './working-days-reports.component';

describe('WorkingDaysReportsComponent', () => {
  let component: WorkingDaysReportsComponent;
  let fixture: ComponentFixture<WorkingDaysReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkingDaysReportsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WorkingDaysReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
