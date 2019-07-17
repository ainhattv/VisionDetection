import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import { AuthGuard } from './core/_guards/auth.guard';
import { RoleGuard } from './core/_guards/role.guard';
import { LoginComponent } from './pages/shared/login/login.component';
import { RegisterComponent } from './pages/shared/register/register.component';

const routes: Routes = [
  {
    path: 'admin',
    loadChildren: () => import('./pages/admin/admin.module')
      .then(m => m.AdminModule),
    canActivate: [RoleGuard],
    data: {
      expectedRole: 'Admin'
    }
  },
  {
    path: 'user',
    loadChildren: () => import('./pages/user/user.module')
      .then(m => m.UserModule),
    canActivate: [AuthGuard]
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: 'user', pathMatch: 'full' },
  { path: '**', redirectTo: 'user' },
];

const config: ExtraOptions = {
  useHash: false,
};

@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
