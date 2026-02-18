import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { ProductionPlanResult } from '../../models/production-plan.model';

@Injectable({ providedIn: 'root' })
export class ProductionPlanService {
  constructor(private http: HttpClient, private api: ApiService) {}

  getPlan(): Observable<ProductionPlanResult> {
    return this.http.get<ProductionPlanResult>(`/api/production-plan`);
  }
}
