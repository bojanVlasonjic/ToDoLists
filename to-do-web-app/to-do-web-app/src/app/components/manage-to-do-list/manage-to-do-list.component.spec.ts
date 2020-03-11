import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageToDoListComponent } from './manage-to-do-list.component';

describe('ManageToDoListComponent', () => {
  let component: ManageToDoListComponent;
  let fixture: ComponentFixture<ManageToDoListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageToDoListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageToDoListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
