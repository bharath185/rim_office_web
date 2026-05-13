import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewWorktypeComponent } from './view-worktype.component';

describe('ViewWorktypeComponent', () => {
  let component: ViewWorktypeComponent;
  let fixture: ComponentFixture<ViewWorktypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewWorktypeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewWorktypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
