import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Organization {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
  updatedAt?: string;
}

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Organization[]> {
    return this.http.get<Organization[]>(`${this.baseUrl}/api/v1/organizations`);
  }

  getById(id: string): Observable<Organization> {
    return this.http.get<Organization>(`${this.baseUrl}/api/v1/organizations/${id}`);
  }

  create(data: Organization): Observable<Organization> {
    return this.http.post<Organization>(`${this.baseUrl}/api/v1/organizations`, data);
  }

  update(id: string, data: Organization): Observable<Organization> {
    return this.http.put<Organization>(`${this.baseUrl}/api/v1/organizations/${id}`, data);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/v1/organizations/${id}`);
  }
}
