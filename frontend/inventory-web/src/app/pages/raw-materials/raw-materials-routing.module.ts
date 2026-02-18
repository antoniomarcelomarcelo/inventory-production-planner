import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RawMaterialsListComponent } from './raw-materials-list/raw-materials-list.component';
import { RawMaterialFormComponent } from './raw-material-form/raw-material-form.component';

const routes: Routes = [
  { path: '', component: RawMaterialsListComponent },
  { path: 'new', component: RawMaterialFormComponent },
  { path: ':id/edit', component: RawMaterialFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RawMaterialsRoutingModule {}
