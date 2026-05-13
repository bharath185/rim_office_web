import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectCheckinComponent } from './direct-checkin.component';

describe('DirectCheckinComponent', () => {
  let component: DirectCheckinComponent;
  let fixture: ComponentFixture<DirectCheckinComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DirectCheckinComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DirectCheckinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
