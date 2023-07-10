import { importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { bootstrapApplication, provideClientHydration } from '@angular/platform-browser';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app/app.component';
import { routes } from './app/routes';
import { coreProviders } from './app/core/core-providers';

bootstrapApplication(AppComponent, {
  providers: [
    provideClientHydration(),
    provideHttpClient(
      withInterceptorsFromDi(),
    ),
    provideRouter(routes),
    provideAnimations(),
    importProvidersFrom(
      ToastrModule.forRoot(),
      BrowserAnimationsModule,
    ),
    coreProviders
  ],
});
