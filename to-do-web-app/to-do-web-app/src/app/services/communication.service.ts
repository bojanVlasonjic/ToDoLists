import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

import { ToDoListService } from './to-do-list.service';
import { ToDoList } from '../models/to-do-list.model';

import { ToDoItem } from '../models/to-do-item.model';
import { ToDoItemService } from './to-do-item.service';

@Injectable({
  providedIn: 'root'
})

/**
 * Used for communication between sibling and parent to child components
 */
export class CommunicationService {

  private createSubject = new Subject<any>();
  private searchSubject = new Subject<any>();

  constructor(private toDoListService: ToDoListService, private toDoItemService: ToDoItemService) { }


  createEmptyList() {

    let listData = new ToDoList();
    listData.endDate = null;
    listData.title = "";

    this.toDoListService.createToDoList(listData).subscribe(
      data => {
        this.createSubject.next(data);
      }
    );
  }

  getToDoList(): Observable<any> {
    return this.createSubject.asObservable();
  }

  updatePosition(listId: string, itemId: string, position: number): Observable<ToDoItem> {
    return this.toDoItemService.updateItemPosition(listId, itemId, position);
  }


  searchToDoLists(title: string) {
    return this.toDoListService.searchToDoLists(title).subscribe(
      data => {
        this.searchSubject.next(data);
      }
    )
  }

  getToDoLists() {
    return this.toDoListService.getToDoLists().subscribe(
      data => {
        this.searchSubject.next(data);
      }
    )
  }

  getSearchedLists(): Observable<any> {
    return this.searchSubject.asObservable();
  }


}
