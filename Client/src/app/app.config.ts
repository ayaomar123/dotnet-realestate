import { APP_BOOTSTRAP_LISTENER, ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { JwtModule } from "@auth0/angular-jwt";
import { routes } from './app.routes';
import { AuthService } from './core/auth/auth.service';
import { provideHttpClient, withInterceptors, withFetch } from '@angular/common/http';
import { authInterceptor } from './core/auth/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor]), withFetch()),
    importProvidersFrom([
      BrowserAnimationsModule,
      ToastrModule.forRoot({
        closeButton: true,
        progressBar: true,
        timeOut: 5000,
        positionClass: 'toast-top-right',
        newestOnTop: true,
        preventDuplicates: true,
      }),
      JwtModule.forRoot({
        config: {
          tokenGetter: () => localStorage.getItem('token')
        }
      })
    ]),
    {
      provide: APP_BOOTSTRAP_LISTENER,
      multi: true,
      useFactory: initializerFactory,
      deps: [AuthService]
    }
  ]
};

export function initializerFactory(authService: AuthService) {
  return () => authService.refreshToken();
}
