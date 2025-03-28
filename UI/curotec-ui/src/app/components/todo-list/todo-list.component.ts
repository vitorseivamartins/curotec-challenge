import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent {
  @Input() name: string = '';
  @Input() isEditing: boolean = false;
  @Output() nameChange = new EventEmitter<string>();
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onNameChange(newName: string) {
    this.nameChange.emit(newName);
  }
}