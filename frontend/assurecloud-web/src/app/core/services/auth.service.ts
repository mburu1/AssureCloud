import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresIn: number;
  tokenType: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = environment.apiUrl;
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  private tokenSubject = new BehaviorSubject<string | null>(null);

  isAuthenticated() {
    return this.isAuthenticatedSubject.value;
  }

  getToken() {
    return this.tokenSubject.value;
  }

  getIsAuthenticated$() {
    return this.isAuthenticatedSubject.asObservable();
  }

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/api/v1/authentication/login`, request).pipe(
      tap(response => {
        localStorage.setItem('access_token', response.token);
        localStorage.setItem('refresh_token', response.refreshToken);
        this.tokenSubject.next(response.token);
        this.isAuthenticatedSubject.next(true);
      })
    );
  }

  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    this.tokenSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  refreshToken() {
    const refreshToken = localStorage.getItem('refresh_token');
    if (!refreshToken) {
      this.logout();
      return;
    }
    this.http.post<AuthResponse>(`${this.baseUrl}/api/v1/authentication/refresh`, { refreshToken })
      .subscribe({
        next: response => {
          localStorage.setItem('access_token', response.token);
          localStorage.setItem('refresh_token', response.refreshToken);
          this.tokenSubject.next(response.token);
          this.isAuthenticatedSubject.next(true);
        },
        error: () => this.logout()
      });
  }

  constructor(private http: HttpClient) {}
}
