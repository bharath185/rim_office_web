import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitorSuccessModalComponent } from './visitor-success-modal.component';

describe('VisitorSuccessModalComponent', () => {
  let component: VisitorSuccessModalComponent;
  let fixture: ComponentFixture<VisitorSuccessModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisitorSuccessModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VisitorSuccessModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
