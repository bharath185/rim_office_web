import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfDevelopmentGoalComponent } from './self-development-goal.component';

describe('SelfDevelopmentGoalComponent', () => {
  let component: SelfDevelopmentGoalComponent;
  let fixture: ComponentFixture<SelfDevelopmentGoalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelfDevelopmentGoalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SelfDevelopmentGoalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
