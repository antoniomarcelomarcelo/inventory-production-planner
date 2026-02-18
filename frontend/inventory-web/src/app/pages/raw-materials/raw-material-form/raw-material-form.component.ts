import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RawMaterialsService } from '../../../core/services/raw-materials.service';
import { RawMaterial } from '../../../models/raw-material.model';

@Component({
  selector: 'app-raw-material-form',
  templateUrl: './raw-material-form.component.html',
  styleUrls: ['./raw-material-form.component.css']
})
export class RawMaterialFormComponent implements OnInit {

   loading = false;
  saving = false;
  isEdit = false;
  id: number | null = null;

  form: FormGroup = this.fb.group({
    id: [0],
    code: ['', [Validators.required, Validators.maxLength(50)]],
    name: ['', [Validators.required, Validators.maxLength(200)]],
    stockQuantity: [0, [Validators.required, Validators.min(0)]],
  });

  constructor(
    private fb: FormBuilder,
    private svc: RawMaterialsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loading = true;

    const idParam = this.route.snapshot.paramMap.get('id');
    this.id = idParam ? Number(idParam) : null;
    this.isEdit = !!this.id;

    if (this.isEdit && this.id) {
      this.svc.getById(this.id).subscribe({
        next: (rm) => {
          this.form.patchValue({
            id: rm.id,
            code: rm.code,
            name: rm.name,
            stockQuantity: rm.stockQuantity,
          });
          this.loading = false;
        },
        error: (err) => {
          alert(err?.message ?? 'Raw material not found');
          this.router.navigate(['/raw-materials']);
        },
      });
    } else {
      this.loading = false;
    }
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;

    const payload: RawMaterial = {
      id: Number(this.form.value.id),
      code: String(this.form.value.code),
      name: String(this.form.value.name),
      stockQuantity: Number(this.form.value.stockQuantity),
    };

    const done = () => {
      this.saving = false;
      this.router.navigate(['/raw-materials']);
    };

    if (!this.isEdit) {
      this.svc.create({ code: payload.code, name: payload.name, stockQuantity: payload.stockQuantity } as any).subscribe({
        next: () => done(),
        error: (err) => {
          alert(err?.message ?? 'Failed to create raw material');
          this.saving = false;
        },
      });
    } else {
      this.svc.update(payload.id, payload).subscribe({
        next: () => done(),
        error: (err) => {
          alert(err?.message ?? 'Failed to update raw material');
          this.saving = false;
        },
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/raw-materials']);
  }
}
