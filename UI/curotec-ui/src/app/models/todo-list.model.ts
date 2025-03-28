import { TodoItemDto } from "./todo-item.model";

export interface TodoListDto {
    name: string;
    items: TodoItemDto[];
    userId: string;
  }