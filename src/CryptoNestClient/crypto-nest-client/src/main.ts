import { bootstrapApplication, provideClientHydration } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';

import { AppComponent } from './app/app.component';
import { routes } from './app/routes';
import { coreProviders } from './app/core/core-providers';
import { provideHttpClient } from '@angular/common/http';

bootstrapApplication(AppComponent, {
  providers: [
    provideClientHydration(),
    provideHttpClient(),
    provideRouter(routes),
    provideAnimations(),
    coreProviders
  ]
});
