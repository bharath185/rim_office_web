import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageModuleComponent } from './page-module.component';

describe('PageModuleComponent', () => {
  let component: PageModuleComponent;
  let fixture: ComponentFixture<PageModuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PageModuleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PageModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
