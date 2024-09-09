import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: '', redirectTo: 'home', pathMatch: 'full'
            },
            {
                path: 'home',
                loadChildren: () => import('./home/home.module').then(m => m.HomeModule)
            },
            {
                path: 'devices',
                loadChildren: () => import('./device/device.module').then(m => m.DeviceModule)
            },
            {
                path: 'new-post',
                loadChildren: () => import('./new-post/new-post.module').then(m => m.NewPostModule)
            },
            {
                path: 'posts',
                loadChildren: () => import('./post/post.module').then(m => m.PostModule)
            },
            {
                path: 'profile',
                loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule)
            },
            {
                path: 'requests',
                loadChildren: () => import('./request/request.module').then(m => m.RequestModule)
            },
            {
                path: 'time-sheet',
                loadChildren: () => import('./time-sheet/time-sheet.module').then(m => m.TimeSheetModule)
            },
            {
                path: 'user-info',
                loadChildren: () => import('./user-info/user-info.module').then(m => m.UserInfoModule)
            },
        ]
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule { }