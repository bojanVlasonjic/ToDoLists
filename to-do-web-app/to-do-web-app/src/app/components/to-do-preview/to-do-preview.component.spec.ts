import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToDoPreviewComponent } from './to-do-preview.component';

describe('ToDoPreviewComponent', () => {
  let component: ToDoPreviewComponent;
  let fixture: ComponentFixture<ToDoPreviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToDoPreviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToDoPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
