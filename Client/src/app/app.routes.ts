import { Routes } from '@angular/router';
import { authGuard, accountGuard } from './core/auth/guards';
import { LayoutComponent } from './dashboard/components/layout/layout.component';

export const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: 'home',
                loadComponent: () => import('./dashboard/pages/home/home.component').then(mod => mod.HomeComponent),
                canActivate: [authGuard]
            }
        ]
    },
    {
        path: 'login',
        loadComponent: () => import('./core/auth/login/pages/login-page/login-page.component').then(mod => mod.LoginPageComponent),
        canActivate: [accountGuard]
    }
];
