import { Routes } from '@angular/router';

import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { LoginComponent } from './layouts/login/login.component';

export const AppRoutes: Routes = [
  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'home', component: AdminLayoutComponent
  }
]
