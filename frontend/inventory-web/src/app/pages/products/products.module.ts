import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { ProductsRoutingModule } from './products-routing.module';
import { ProductsListComponent } from './products-list/products-list.component';
import { ProductFormComponent } from './product-form/product-form.component';

@NgModule({
  declarations: [ProductsListComponent, ProductFormComponent],
  imports: [CommonModule, ReactiveFormsModule, ProductsRoutingModule],
})
export class ProductsModule {}
