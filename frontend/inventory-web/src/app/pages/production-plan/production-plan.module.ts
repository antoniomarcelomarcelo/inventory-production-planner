import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductionPlanRoutingModule } from './production-plan-routing.module';
import { ProductionPlanPageComponent } from './production-plan-page/production-plan-page.component';

@NgModule({
  declarations: [ProductionPlanPageComponent],
  imports: [CommonModule, ProductionPlanRoutingModule],
})
export class ProductionPlanModule {}
