import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsleaveMgrComponent } from './teamsleave-mgr.component';

describe('TeamsleaveMgrComponent', () => {
  let component: TeamsleaveMgrComponent;
  let fixture: ComponentFixture<TeamsleaveMgrComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamsleaveMgrComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamsleaveMgrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
