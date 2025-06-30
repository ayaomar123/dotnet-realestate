import { CategoryComponent } from './dashboard/pages/category/category.component';
import { Routes } from '@angular/router';
import { authGuard, accountGuard } from './core/auth/guards';
import { LayoutComponent } from './dashboard/components/layout/layout.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'admin/home',
        pathMatch: 'full'
    },
    {
        path: 'admin',
        component: LayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: 'home',
                loadComponent: () => import('./dashboard/pages/home/home.component').then(mod => mod.HomeComponent),
            },
            {
                path: 'categories',
                loadComponent: () => import('./dashboard/pages/category/category.component').then(mod => mod.CategoryComponent),
            },
            {
                path: 'cities',
                loadComponent: () => import('./dashboard/pages/city/city.component').then(mod => mod.CityComponent),
            },
            {
                path: 'districts',
                loadComponent: () => import('./dashboard/pages/district/district.component').then(mod => mod.DistrictComponent),
            },
            {
                path: 'types',
                loadComponent: () => import('./dashboard/pages/type/type.component').then(mod => mod.TypeComponent),
            },
            {
                path: 'property_types',
                loadComponent: () => import('./dashboard/pages/property-type/property-type.component').then(mod => mod.PropertyTypeComponent),
            },
            {
                path: 'statuses',
                loadComponent: () => import('./dashboard/pages/status/status.component').then(mod => mod.StatusComponent),
            },

            {
                path: 'items',
                loadComponent: () => import('./dashboard/pages/item/item.component').then(mod => mod.ItemComponent),
            },
            {
                path: 'items/create',
                loadComponent: () => import('./dashboard/pages/item/create-item/create-item.component').then(mod => mod.CreateItemComponent),
            },
            {
                path: 'profile',
                loadComponent: () => import('./dashboard/pages/profile/profile.component').then(mod => mod.ProfileComponent),
            },
        ]
    },
    {
        path: 'login',
        loadComponent: () => import('./core/auth/login/pages/login-page/login-page.component').then(mod => mod.LoginPageComponent),
        canActivate: [accountGuard]
    }
];
