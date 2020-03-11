import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ToDoList } from '../models/to-do-list.model';

import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class ToDoListService {

  constructor(private http: HttpClient) { }

  getToDoLists(): Observable<Array<ToDoList>> {
    return this.http.get<Array<ToDoList>>(environment.baseUrl);
  }

  getToDoListById(listId: string): Observable<ToDoList> {
    return this.http.get<ToDoList>(`${environment.baseUrl}/${listId}`);
  }

  removeToDoList(listId: string): Observable<any> {
    return this.http.delete(`${environment.baseUrl}/${listId}`);
  }

  updateToDoList(listData: ToDoList): Observable<ToDoList> {
    return this.http.put<ToDoList>(`${environment.baseUrl}/${listData.id}`, listData);
  }

  createToDoList(listData: ToDoList): Observable<ToDoList> {
    return this.http.post<ToDoList>(environment.baseUrl, listData);
  }

  updatePosition(listId: string, position: number): Observable<ToDoList> {
    return this.http.put<ToDoList>(`${environment.baseUrl}/${listId}/${position}`, null);
  }

  searchToDoLists(title: string): Observable<Array<ToDoList>>{
    return this.http.get<Array<ToDoList>>(`${environment.baseUrl}/search?title=${title}`);
  }

  createSharedList(listId: string): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}/${listId}`, null);
  }

  getSharedList(shareId: string): Observable<ToDoList> {
    return this.http.get<ToDoList>(`${environment.baseUrl}/share/${shareId}`);
  }



}
