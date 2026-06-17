import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { UserRole } from '../models/user-role.enum';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = 'https://localhost:7144/api/auth';

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<{ token: string; username: string; role: UserRole }> {
    return this.http.post<{ token: string; username: string; role: UserRole }>(`${this.api}/login`, { username, password }).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('username', res.username);
        localStorage.setItem('role', res.role.toString());
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserRole(): UserRole | null {
    const role = localStorage.getItem('role');
    return role !== null ? (Number(role) as UserRole) : null;
  }

  getUsername(): string | null {
    return localStorage.getItem('username');
  }

  isAdmin(): boolean {
    return this.getUserRole() === UserRole.Admin;
  }
}
