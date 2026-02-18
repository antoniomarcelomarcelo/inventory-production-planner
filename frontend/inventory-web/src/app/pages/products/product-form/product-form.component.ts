import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from '../../../core/services/products.service';
import { RawMaterialsService } from '../../../core/services/raw-materials.service';
import { RawMaterial } from '../../../models/raw-material.model';
import { UpdateBomRequest } from '../../../models/bom.model';
import { ToastService } from 'src/app/core/services/toast.service';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {

   loading = false;
  saving = false;
  isEdit = false;
  id: number | null = null;

  rawMaterials: RawMaterial[] = [];

  form: FormGroup = this.fb.group({
    id: [0],
    code: ['', [Validators.required, Validators.maxLength(50)]],
    name: ['', [Validators.required, Validators.maxLength(200)]],
    price: [0, [Validators.required, Validators.min(0)]],
    bom: this.fb.array([]),
  });

  get bom(): FormArray {
    return this.form.get('bom') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private products: ProductsService,
    private rawMaterialsSvc: RawMaterialsService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.loading = true;

    const idParam = this.route.snapshot.paramMap.get('id');
    this.id = idParam ? Number(idParam) : null;
    this.isEdit = !!this.id;

    this.rawMaterialsSvc.getAll().subscribe({
      next: (rms) => {
        this.rawMaterials = rms;

        if (this.isEdit && this.id) {
          this.loadProduct(this.id);
        } else {
          // new product starts with 1 empty bom row
          this.addBomRow();
          this.loading = false;
        }
      },
      error: (err) => {
        this.toast.show(err?.message ?? 'Failed to load raw materials', 'error');
        this.loading = false;
      },
    });
  }

  private loadProduct(id: number): void {
    this.products.getById(id).subscribe({
      next: (p) => {
        this.form.patchValue({
          id: p.id,
          code: p.code,
          name: p.name,
          price: p.price,
        });

        // load BOM from API
        this.products.getBom(id).subscribe({
          next: (bom) => {
            this.bom.clear();
            (bom.items || []).forEach((it) => this.bom.push(this.createBomRow(it.rawMaterialId, it.quantityPerUnit)));
            if (this.bom.length === 0) this.addBomRow();
            this.loading = false;
          },
          error: () => {
            // if bom endpoint not available or empty, keep at least one row
            this.bom.clear();
            this.addBomRow();
            this.loading = false;
          },
        });
      },
      error: (err) => {
        alert(err?.message ?? 'Product not found');
        this.router.navigate(['/products']);
      },
    });
  }

  createBomRow(rawMaterialId: number | null = null, quantityPerUnit: number | null = null): FormGroup {
    return this.fb.group({
      rawMaterialId: [rawMaterialId, [Validators.required]],
      quantityPerUnit: [quantityPerUnit ?? 1, [Validators.required, Validators.min(0.0001)]],
    });
  }

  addBomRow(): void {
    this.bom.push(this.createBomRow());
  }

  removeBomRow(index: number): void {
    this.bom.removeAt(index);
    if (this.bom.length === 0) this.addBomRow();
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;

    const productPayload = {
      id: this.form.value.id,
      code: this.form.value.code,
      name: this.form.value.name,
      price: Number(this.form.value.price),
    };

    const bomPayload: UpdateBomRequest = {
      items: (this.form.value.bom || []).map((x: any) => ({
        rawMaterialId: Number(x.rawMaterialId),
        quantityPerUnit: Number(x.quantityPerUnit),
      })),
    };

    const done = (message: string) => {
      this.saving = false;
      this.toast.show(message, 'success');
      this.router.navigate(['/products']);
    };

    if (!this.isEdit) {
  this.products
    .create({ code: productPayload.code, name: productPayload.name, price: productPayload.price } as any)
    .subscribe({
      next: (created) => {
        this.products.updateBom(created.id, bomPayload).subscribe({
          next: () => done('Product created successfully'),
          error: (err) => {
            this.toast.show(err?.message ?? 'Failed to save BOM', 'error');
            this.saving = false;
          },
        });
      },
      error: (err) => {
        this.toast.show(err?.message ?? 'Failed to create product', 'error');
        this.saving = false;
      },
    });
  }
 else {
      const id = productPayload.id;
      this.products.update(id, productPayload as any).subscribe({
        next: () => {
          this.products.updateBom(id, bomPayload).subscribe({
            next: () => done('Product updated successfully'),
            error: (err) => {
              this.toast.show(err?.message ?? 'Failed to save BOM', 'error');
              this.saving = false;
            },
          });
        },
        error: (err) => {
          this.toast.show(err?.message ?? 'Failed to update product', 'error');
          this.saving = false;
        },
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/products']);
  }
}
