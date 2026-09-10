import { Routes } from '@angular/router';
import { ShortUrlsTableComponent } from './components/short-urls-table.component';
import { LoginComponent } from './components/login.component';
import { ShortUrlDetailComponent } from './components/short-url-detail.component';
import { AboutComponent } from './components/about.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: ShortUrlsTableComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'urls/:id',
    component: ShortUrlDetailComponent,
    canActivate: [authGuard]
  },
  {
    path: 'about',
    component: AboutComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];
