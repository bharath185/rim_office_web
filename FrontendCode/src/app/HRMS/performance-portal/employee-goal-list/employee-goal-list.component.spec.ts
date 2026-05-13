import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeGoalListComponent } from './employee-goal-list.component';

describe('EmployeeGoalListComponent', () => {
  let component: EmployeeGoalListComponent;
  let fixture: ComponentFixture<EmployeeGoalListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmployeeGoalListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmployeeGoalListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
