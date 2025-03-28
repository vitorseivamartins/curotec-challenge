import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TodoListDto } from '../models/todo-list.model';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private apiUrl = 'https://localhost:7225/api/todo';

  constructor(private http: HttpClient) { }

  getTodoLists(userId: string): Observable<TodoListDto[]> {
    return this.http.get<TodoListDto[]>(`${this.apiUrl}/${userId}`);
  }

  getTodoList(userId: string, listId: string): Observable<TodoListDto> {
    return this.http.get<TodoListDto>(`${this.apiUrl}/${userId}/${listId}`);
  }

  createTodoList(list: Omit<TodoListDto, 'id'>): Observable<TodoListDto> {
    return this.http.post<TodoListDto>(this.apiUrl, list);
  }
}