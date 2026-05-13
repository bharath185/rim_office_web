import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OnSiteWorkComponent } from './on-site-work.component';

describe('OnSiteWorkComponent', () => {
  let component: OnSiteWorkComponent;
  let fixture: ComponentFixture<OnSiteWorkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OnSiteWorkComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OnSiteWorkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
