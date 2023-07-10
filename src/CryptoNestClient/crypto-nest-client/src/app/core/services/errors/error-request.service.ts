import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { hostAddress } from '../../host-address';
import { combineUrl } from '../../url-combine';

@Injectable({
  providedIn: 'root'
})
export class ErrorRequestService {
  constructor(private httpClient: HttpClient) { }

  logErrorOnServer(errorMessage: string): Observable<void> {
    console.error(errorMessage);

    return this.httpClient.post<void>(combineUrl(hostAddress(), 'api/error'), { errorMessage });
  }
}
