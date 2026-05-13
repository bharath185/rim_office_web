import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsleaveHrComponent } from './teamsleave-hr.component';

describe('TeamsleaveHrComponent', () => {
  let component: TeamsleaveHrComponent;
  let fixture: ComponentFixture<TeamsleaveHrComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamsleaveHrComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamsleaveHrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
