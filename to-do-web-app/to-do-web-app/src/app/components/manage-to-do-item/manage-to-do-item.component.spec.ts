import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageToDoItemComponent } from './manage-to-do-item.component';

describe('ManageToDoItemComponent', () => {
  let component: ManageToDoItemComponent;
  let fixture: ComponentFixture<ManageToDoItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageToDoItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageToDoItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
