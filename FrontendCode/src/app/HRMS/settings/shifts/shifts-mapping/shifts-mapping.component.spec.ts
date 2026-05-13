import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShiftsMappingComponent } from './shifts-mapping.component';

describe('ShiftsMappingComponent', () => {
  let component: ShiftsMappingComponent;
  let fixture: ComponentFixture<ShiftsMappingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShiftsMappingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ShiftsMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
