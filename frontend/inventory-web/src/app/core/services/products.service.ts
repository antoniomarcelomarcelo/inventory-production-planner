import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api.service';
import { Product } from '../../models/product.model';
import { Observable } from 'rxjs';
import { UpdateBomRequest } from '../../models/bom.model';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  constructor(private http: HttpClient, private api: ApiService) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.api.baseUrl}/api/products`);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.api.baseUrl}/api/products/${id}`);
  }

  create(payload: Omit<Product, 'id'>): Observable<Product> {
    return this.http.post<Product>(`${this.api.baseUrl}/api/products`, payload);
  }

  update(id: number, payload: Product): Observable<void> {
    return this.http.put<void>(`${this.api.baseUrl}/api/products/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api.baseUrl}/api/products/${id}`);
  }

  getBom(id: number): Observable<UpdateBomRequest> {
    return this.http.get<UpdateBomRequest>(`${this.api.baseUrl}/api/products/${id}/bom`);
  }

  updateBom(id: number, payload: UpdateBomRequest): Observable<void> {
    return this.http.put<void>(`/api/products/${id}/bom`, payload);
  }
}
