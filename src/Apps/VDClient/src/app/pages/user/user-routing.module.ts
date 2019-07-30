import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { UserComponent } from './user.component';
import { CreateworkplaceComponent } from './createworkplace/createworkplace.component';

const routes: Routes = [
  {
    path: '',
    component: UserComponent,
    children: [
      { path: 'index', component: IndexComponent },
      { path: 'createworkplace', component: CreateworkplaceComponent }
    ]
  },
  { path: '', redirectTo: 'index', pathMatch: 'full' },
  { path: '**', redirectTo: 'index' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
