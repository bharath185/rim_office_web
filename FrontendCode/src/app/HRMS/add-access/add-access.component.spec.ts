import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAccessComponent } from './add-access.component';

describe('AddAccessComponent', () => {
  let component: AddAccessComponent;
  let fixture: ComponentFixture<AddAccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAccessComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
