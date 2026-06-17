import { Routes } from '@angular/router';
import { AdminComponent } from './components/admin/admin';
import { LoginComponent } from './components/login/login';
import { HomeComponent } from './components/home/home';
import { adminGuard, authGuard } from './auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [authGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [adminGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
