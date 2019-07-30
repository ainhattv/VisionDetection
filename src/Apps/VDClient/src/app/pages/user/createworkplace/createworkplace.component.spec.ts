import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateworkplaceComponent } from './createworkplace.component';

describe('CreateworkplaceComponent', () => {
  let component: CreateworkplaceComponent;
  let fixture: ComponentFixture<CreateworkplaceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateworkplaceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateworkplaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
