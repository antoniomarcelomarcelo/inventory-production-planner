import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api.service';
import { RawMaterial } from '../../models/raw-material.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RawMaterialsService {
  constructor(private http: HttpClient, private api: ApiService) {}

  getAll(): Observable<RawMaterial[]> {
    return this.http.get<RawMaterial[]>(`${this.api.baseUrl}/api/raw-materials`);
  }

  getById(id: number): Observable<RawMaterial> {
    return this.http.get<RawMaterial>(`${this.api.baseUrl}/api/raw-materials/${id}`);
  }

  create(payload: Omit<RawMaterial, 'id'>): Observable<RawMaterial> {
    return this.http.post<RawMaterial>(`${this.api.baseUrl}/api/raw-materials`, payload);
  }

  update(id: number, payload: RawMaterial): Observable<void> {
    return this.http.put<void>(`${this.api.baseUrl}/api/raw-materials/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`/api/raw-materials/${id}`);
  }
}
