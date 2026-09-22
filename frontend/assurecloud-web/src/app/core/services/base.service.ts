import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseService<T> {
  protected baseUrl = environment.apiUrl;

  constructor(protected http: HttpClient, protected resource: string) {}

  getAll(params?: Record<string, unknown>): Observable<T[]> {
    return this.http.get<T[]>(`${this.baseUrl}/api/v1/${this.resource}`, { params });
  }

  getById(id: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/api/v1/${this.resource}/${id}`);
  }

  create(data: T): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/api/v1/${this.resource}`, data);
  }

  update(id: string, data: T): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/api/v1/${this.resource}/${id}`, data);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/v1/${this.resource}/${id}`);
  }
}
