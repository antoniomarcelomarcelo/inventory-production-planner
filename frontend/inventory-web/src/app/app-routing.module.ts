import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'production-plan' },

  {
    path: 'products',
    loadChildren: () => import('./pages/products/products.module').then(m => m.ProductsModule),
  },
  {
    path: 'raw-materials',
    loadChildren: () => import('./pages/raw-materials/raw-materials.module').then(m => m.RawMaterialsModule),
  },
  {
    path: 'production-plan',
    loadChildren: () => import('./pages/production-plan/production-plan.module').then(m => m.ProductionPlanModule),
  },

  { path: '**', redirectTo: 'production-plan' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
