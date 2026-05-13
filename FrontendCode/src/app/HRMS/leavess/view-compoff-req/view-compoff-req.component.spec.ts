import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCompoffReqComponent } from './view-compoff-req.component';

describe('ViewCompoffReqComponent', () => {
  let component: ViewCompoffReqComponent;
  let fixture: ComponentFixture<ViewCompoffReqComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewCompoffReqComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewCompoffReqComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
