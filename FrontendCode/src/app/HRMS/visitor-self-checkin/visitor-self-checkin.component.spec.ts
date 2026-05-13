import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitorSelfCheckinComponent } from './visitor-self-checkin.component';

describe('VisitorSelfCheckinComponent', () => {
  let component: VisitorSelfCheckinComponent;
  let fixture: ComponentFixture<VisitorSelfCheckinComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisitorSelfCheckinComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VisitorSelfCheckinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
