import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ToDoItem } from '../models/to-do-item.model';

import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class ToDoItemService {

  constructor(private http: HttpClient) { }

  getItemsForList(listId: string): Observable<Array<ToDoItem>> {
    return this.http.get<Array<ToDoItem>>(`${environment.baseUrl}/${listId}/to-do-item`);
  }

  updateToDoItem(listId: string, itemData: ToDoItem): Observable<ToDoItem> {
      return this.http.put<ToDoItem>(`${environment.baseUrl}/${listId}/to-do-item`, itemData);
  }

  removeToDoItem(listId: string, itemId: string): Observable<any> {
    return this.http.delete(`${environment.baseUrl}/${listId}/to-do-item/${itemId}`);
  }

  createToDoItem(listId: string, itemData: ToDoItem): Observable<ToDoItem> {
    return this.http.post<ToDoItem>(`${environment.baseUrl}/${listId}/to-do-item`, itemData);
  }

  updateItemPosition(listId: string, itemId: string, position: number) : Observable<ToDoItem> {
    return this.http.put<ToDoItem>(`${environment.baseUrl}/${listId}/to-do-item/${itemId}/${position}`, null);
  }


}
