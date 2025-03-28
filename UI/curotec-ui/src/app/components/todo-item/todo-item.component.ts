import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-item',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss']
})
export class TodoItemComponent {
  @Input() description: string = '';
  @Input() isCompleted: boolean = false;
  @Input() isEditing: boolean = false;
  @Output() descriptionChange = new EventEmitter<string>();
  @Output() completedChange = new EventEmitter<boolean>();
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onDescriptionChange(newDesc: string) {
    this.descriptionChange.emit(newDesc);
  }

  onCompletedChange(isChecked: boolean) {
    this.completedChange.emit(isChecked);
  }
}