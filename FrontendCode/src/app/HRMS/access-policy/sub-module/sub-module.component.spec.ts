import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubModuleComponent } from './sub-module.component';

describe('SubModuleComponent', () => {
  let component: SubModuleComponent;
  let fixture: ComponentFixture<SubModuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubModuleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SubModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
