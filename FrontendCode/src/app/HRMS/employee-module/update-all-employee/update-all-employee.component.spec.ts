import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateAllEmployeeComponent } from './update-all-employee.component';

describe('UpdateAllEmployeeComponent', () => {
  let component: UpdateAllEmployeeComponent;
  let fixture: ComponentFixture<UpdateAllEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateAllEmployeeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdateAllEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
