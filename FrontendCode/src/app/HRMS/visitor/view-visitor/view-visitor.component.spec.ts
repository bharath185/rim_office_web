import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewVisitorComponent } from './view-visitor.component';

describe('ViewVisitorComponent', () => {
  let component: ViewVisitorComponent;
  let fixture: ComponentFixture<ViewVisitorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewVisitorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewVisitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
