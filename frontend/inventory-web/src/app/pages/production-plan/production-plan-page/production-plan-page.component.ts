import { Component, OnInit } from '@angular/core';
import { ProductionPlanService } from '../../../core/services/production-plan.service';
import { ProductionPlanResult } from '../../../models/production-plan.model';

@Component({
  selector: 'app-production-plan-page',
  templateUrl: './production-plan-page.component.html',
})
export class ProductionPlanPageComponent implements OnInit {
  loading = false;
  error: string | null = null;
  data: ProductionPlanResult | null = null;

  constructor(private svc: ProductionPlanService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = null;

    this.svc.getPlan().subscribe({
      next: (res) => {
        this.data = res;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message ?? 'Failed to load production plan';
        this.loading = false;
      },
    });
  }
}
