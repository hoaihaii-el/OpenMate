import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: 'OpenMate',
        loadChildren: () => import('./layout/layout.module').then(m => m.LayoutModule)
    },
    {
        path: 'Login',
        loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
    },
    {
        path: '',
        redirectTo: 'OpenMate',
        pathMatch: 'full'
    }
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }