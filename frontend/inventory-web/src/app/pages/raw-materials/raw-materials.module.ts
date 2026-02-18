import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { RawMaterialsRoutingModule } from './raw-materials-routing.module';
import { RawMaterialsListComponent } from './raw-materials-list/raw-materials-list.component';
import { RawMaterialFormComponent } from './raw-material-form/raw-material-form.component';

@NgModule({
  declarations: [RawMaterialsListComponent, RawMaterialFormComponent],
  imports: [CommonModule, ReactiveFormsModule, RawMaterialsRoutingModule],
})
export class RawMaterialsModule {}
