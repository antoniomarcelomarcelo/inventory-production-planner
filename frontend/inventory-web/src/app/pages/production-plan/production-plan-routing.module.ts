import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionPlanPageComponent } from './production-plan-page/production-plan-page.component';

const routes: Routes = [{ path: '', component: ProductionPlanPageComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductionPlanRoutingModule {}
