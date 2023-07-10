import { ErrorHandler, Injectable, Injector } from '@angular/core';

import { ErrorRequestService } from './error-request.service';
import { NotificationService } from '../notifications/notification.service';

@Injectable()
export class GlobalErrorHandlerService implements ErrorHandler {
  constructor(private injector: Injector) { }

  handleError(error: any): void {
    const errorRequestService = this.injector.get(ErrorRequestService);
    const notificationService = this.injector.get(NotificationService);

    errorRequestService.logErrorOnServer(error.toString())
      .subscribe({
        error: e => console.error(e),
      });

    notificationService.notifyError(error?.message || error, 'Error');
  }
}
