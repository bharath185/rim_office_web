import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VariableHistroyComponent } from './variable-histroy.component';

describe('VariableHistroyComponent', () => {
  let component: VariableHistroyComponent;
  let fixture: ComponentFixture<VariableHistroyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VariableHistroyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VariableHistroyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
