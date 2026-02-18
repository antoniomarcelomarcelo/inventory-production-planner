import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RawMaterialsService } from '../../../core/services/raw-materials.service';
import { RawMaterial } from '../../../models/raw-material.model';

@Component({
  selector: 'app-raw-materials-list',
  templateUrl: './raw-materials-list.component.html',
  styleUrls: ['./raw-materials-list.component.css']
})
export class RawMaterialsListComponent implements OnInit {

  loading = false;
  error: string | null = null;
  items: RawMaterial[] = [];

  // Delete confirmation modal state
  deleteModalOpen = false;
  deleting = false;
  deleteError: string | null = null;
  selected: RawMaterial | null = null;

  constructor(private svc: RawMaterialsService, private router: Router) {}

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
        this.error = err?.message ?? 'Failed to load raw materials';
        this.loading = false;
      },
    });
  }

  create(): void {
    this.router.navigate(['/raw-materials/new']);
  }

  edit(id: number): void {
    this.router.navigate(['/raw-materials', id, 'edit']);
  }

  openDelete(item: RawMaterial): void {
    this.selected = item;
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
