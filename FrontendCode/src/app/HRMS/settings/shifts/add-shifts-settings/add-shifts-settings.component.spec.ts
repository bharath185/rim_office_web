import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddShiftsSettingsComponent } from './add-shifts-settings.component';

describe('AddShiftsSettingsComponent', () => {
  let component: AddShiftsSettingsComponent;
  let fixture: ComponentFixture<AddShiftsSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddShiftsSettingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddShiftsSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
