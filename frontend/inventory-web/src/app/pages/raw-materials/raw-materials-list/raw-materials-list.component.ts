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

  remove(id: number): void {
    if (!confirm('Delete this raw material?')) return;

    this.svc.delete(id).subscribe({
      next: () => this.load(),
      error: (err) => alert(err?.message ?? 'Failed to delete'),
    });
  }
}
