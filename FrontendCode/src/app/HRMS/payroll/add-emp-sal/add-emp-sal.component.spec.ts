import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEmpSalComponent } from './add-emp-sal.component';

describe('AddEmpSalComponent', () => {
  let component: AddEmpSalComponent;
  let fixture: ComponentFixture<AddEmpSalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddEmpSalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddEmpSalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
