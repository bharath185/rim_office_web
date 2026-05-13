import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BehaviorcomponentComponent } from './behaviorcomponent.component';

describe('BehaviorcomponentComponent', () => {
  let component: BehaviorcomponentComponent;
  let fixture: ComponentFixture<BehaviorcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BehaviorcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BehaviorcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
