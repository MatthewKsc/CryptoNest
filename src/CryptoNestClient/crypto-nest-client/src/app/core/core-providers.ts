import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { PrependServiceUrlInterceptorService } from './interceptors/prepend-service-url-interceptor.service';
import { APP_INITIALIZER, Provider } from '@angular/core';
import { SystemInformationService } from './system-information/system-information.service';

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
]
