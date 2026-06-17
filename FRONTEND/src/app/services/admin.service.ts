import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { User } from '../models/user.model';
import { Property } from '../models/property.model';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private api = 'https://localhost:7144/api/admin';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private headers(): HttpHeaders {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.api}/users`, { headers: this.headers() });
  }

  addUser(user: User): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.api}/users`, user, { headers: this.headers() });
  }

  getProperties(): Observable<Property[]> {
    return this.http.get<Property[]>(`${this.api}/properties`, { headers: this.headers() });
  }

  assignProperty(userId: number, propertyId: number): Observable<void> {
    return this.http.post<void>(`${this.api}/assign?userId=${userId}&propertyId=${propertyId}`, {}, { headers: this.headers() });
  }

  unassignProperty(userId: number, propertyId: number): Observable<void> {
    return this.http.post<void>(`${this.api}/unassign?userId=${userId}&propertyId=${propertyId}`, {}, { headers: this.headers() });
  }
}
