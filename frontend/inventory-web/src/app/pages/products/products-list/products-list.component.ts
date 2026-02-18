import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductsService } from '../../../core/services/products.service';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent implements OnInit {

   loading = false;
  error: string | null = null;
  items: Product[] = [];

  constructor(private svc: ProductsService, private router: Router) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = null;

    this.svc.getAll().subscribe({
      next: (res) => {
        this.items = res;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message ?? 'Failed to load products';
        this.loading = false;
      },
    });
  }

  create(): void {
    this.router.navigate(['/products/new']);
  }

  edit(id: number): void {
    this.router.navigate(['/products', id, 'edit']);
  }

  remove(id: number): void {
    if (!confirm('Delete this product?')) return;

    this.svc.delete(id).subscribe({
      next: () => this.load(),
      error: (err) => alert(err?.message ?? 'Failed to delete'),
    });
  }
}
