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

  // Delete confirmation modal state
  deleteModalOpen = false;
  deleting = false;
  deleteError: string | null = null;
  selected: Product | null = null;

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

  openDelete(product: Product): void {
    this.selected = product;
    this.deleteError = null;
    this.deleteModalOpen = true;
  }

  closeDeleteModal(): void {
    if (this.deleting) return;
    this.deleteModalOpen = false;
    this.selected = null;
    this.deleteError = null;
  }

  confirmDelete(): void {
    if (!this.selected || this.deleting) return;

    this.deleting = true;
    this.deleteError = null;

    this.svc.delete(this.selected.id).subscribe({
      next: () => {
        this.deleting = false;
        this.closeDeleteModal();
        this.load();
      },
      error: (err) => {
        this.deleting = false;
        this.deleteError = err?.message ?? 'Failed to delete';
      },
    });
  }
}
