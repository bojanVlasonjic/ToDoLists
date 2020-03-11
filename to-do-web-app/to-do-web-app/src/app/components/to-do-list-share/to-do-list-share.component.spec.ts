import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToDoListShareComponent } from './to-do-list-share.component';

describe('ToDoListShareComponent', () => {
  let component: ToDoListShareComponent;
  let fixture: ComponentFixture<ToDoListShareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToDoListShareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToDoListShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
