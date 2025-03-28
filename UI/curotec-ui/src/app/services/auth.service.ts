import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserDto } from '../dtos/user.dto';

interface AuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = 'https://localhost:7225/api/user';
  private tokenSubject = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) {
    // Initialize with token from localStorage if available
    const storedToken = localStorage.getItem('authToken');
    if (storedToken) {
      this.tokenSubject.next(storedToken);
    }
  }

  get token$(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  get currentToken(): string | null {
    return this.tokenSubject.value;
  }

  login(email: string, password: string): Observable<AuthResponse> {
    const request: UserDto = { email, password };
    return this.http.post<AuthResponse>(`${this.API_URL}/login`, request).pipe(
      tap(response => {
        this.storeToken(response.token);
      })
    );
  }

  register(email: string, password: string): Observable<UserDto> {
    const request: UserDto = { email, password };
    return this.http.post<UserDto>(`${this.API_URL}/create`, request);
  }

  logout(): void {
    this.removeToken();
  }

  isAuthenticated(): boolean {
    return !!this.currentToken;
  }

  private storeToken(token: string): void {
    localStorage.setItem('authToken', token);
    this.tokenSubject.next(token);
  }

  private removeToken(): void {
    localStorage.removeItem('authToken');
    this.tokenSubject.next(null);
  }
}