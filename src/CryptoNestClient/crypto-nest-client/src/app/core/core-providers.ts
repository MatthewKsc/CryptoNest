import { APP_INITIALIZER, ErrorHandler, Provider } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { PrependServiceUrlInterceptorService } from './interceptors/prepend-service-url-interceptor.service';
import { SystemInformationService } from './services/system-information/system-information.service';
import { GlobalErrorHandlerService } from './services/errors/global-error-handler.service';

export const coreProviders: Provider[] = [
  {
    provide: APP_INITIALIZER,
    deps: [SystemInformationService],
    useFactory: (systemInformationService: SystemInformationService) => () => systemInformationService.loadSystemVersionInformation(),
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: PrependServiceUrlInterceptorService,
    multi: true
  },
  {
    provide: ErrorHandler,
    useClass: GlobalErrorHandlerService
  }
]
