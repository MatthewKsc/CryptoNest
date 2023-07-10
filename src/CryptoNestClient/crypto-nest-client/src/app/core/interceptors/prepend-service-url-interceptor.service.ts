import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { hostAddress } from '../host-address';
import { combineUrl } from '../url-combine';

@Injectable()
export class PrependServiceUrlInterceptorService implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (request.url.startsWith('api') && environment.apiUrl) {
      const updatedRequest = request.clone({ url: combineUrl(hostAddress(), request.url) });

      return next.handle(updatedRequest);
    }

    return next.handle(request);
  }
}
