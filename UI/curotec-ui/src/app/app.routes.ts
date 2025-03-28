import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { TodoContainerComponent } from './components/todo-container/todo-container.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'todo', component: TodoContainerComponent },
    { path: '**', redirectTo: 'login'},
];
