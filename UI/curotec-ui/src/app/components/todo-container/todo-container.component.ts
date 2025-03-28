import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TodoListComponent } from '../todo-list/todo-list.component';
import { TodoItemComponent } from '../todo-item/todo-item.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TodoService } from '../../services/todo.service';
import { TodoListDto } from '../../models/todo-list.model';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

interface TodoList {
  id?: number | null;
  name: string;
  items: TodoItem[];
  expanded: boolean;
  isEditing: boolean;
}

interface TodoItem {
  id?: number | null;
  description: string;
  isCompleted: boolean;
  isEditing: boolean;
}

@Component({
  selector: 'app-todo-container',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TodoListComponent,
    TodoItemComponent,
    MatExpansionModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  templateUrl: './todo-container.component.html',
  styleUrls: ['./todo-container.component.scss']
})
export class TodoContainerComponent implements OnInit {
  todoLists: TodoList[] = [];

  newListName: string = '';

  userId = localStorage.getItem('userId')!;

  constructor(private todoService: TodoService, private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.fetchTodoLists();
  }

  fetchTodoLists() {
    this.todoService.getTodoLists(this.userId).subscribe((response: TodoListDto[]) => {
      this.todoLists = response.map(todoList => ({
        ...todoList,
        expanded: false,
        isEditing: false,
        items:
          todoList.items.map(item => ({
              ...item, 
              isEditing: false
            }))
        }));
    }, error => {
      console.error('Error fetching todo lists:', error);
    });
  }

  toggleList(list: TodoList) {
    list.expanded = !list.expanded;
  }

  addNewList() {
    if (this.newListName.trim()) {
      const newList: TodoList = {
        name: this.newListName,
        expanded: true,
        isEditing: false,
        items: []
      };
      this.todoLists.push(newList);
      this.newListName = '';
    }
  }

  addNewItem(list: TodoList) {
    list.items.push({
      description: '',
      isCompleted: false,
      isEditing: true
    });
  }

  saveItem(item: TodoItem) {
    item.isEditing = false;
  }

  saveList(list: TodoList) {
    list.isEditing = false;

    this.todoService.createTodoList({
      name: list.name,
      items: list.items.map(item => ({ description: item.description, isCompleted: item.isCompleted })),
      userId: this.userId
    }).subscribe(response => {
      this.fetchTodoLists();
      this.snackBar.open('Todo list created successfully!', undefined, { duration: 3000 });
    }, error => {
      this.snackBar.open('Error creating todo list. Please try again.', undefined, { duration: 3000 });
    });
  }
}