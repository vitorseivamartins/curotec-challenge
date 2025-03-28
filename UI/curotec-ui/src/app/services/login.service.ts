import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { UserDto } from '../dtos/user.dto';
import { LoginDto } from '../dtos/login.dto';


@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = 'https://localhost:7225/api/user';

  constructor(private http: HttpClient) { }

  login(user: UserDto): Observable<LoginDto> {
    return this.http.post<LoginDto>(`${this.apiUrl}/login`, user).pipe(
      tap(result => {
        localStorage.setItem('authToken', result.token);
        localStorage.setItem('userId', result.userId);
      })
    );
  }
}
