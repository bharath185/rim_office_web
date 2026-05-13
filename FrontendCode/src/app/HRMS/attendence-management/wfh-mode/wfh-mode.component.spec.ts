import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WfhModeComponent } from './wfh-mode.component';

describe('WfhModeComponent', () => {
  let component: WfhModeComponent;
  let fixture: ComponentFixture<WfhModeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WfhModeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WfhModeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
