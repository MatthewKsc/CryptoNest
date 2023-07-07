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
  },
]
