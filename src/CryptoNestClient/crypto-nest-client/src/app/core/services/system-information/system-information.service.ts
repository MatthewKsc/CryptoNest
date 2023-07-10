import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, tap } from 'rxjs';

import { ISystemInformation } from './system-information.module';
import { combineUrl } from '../../url-combine';
import { hostAddress } from '../../host-address';

@Injectable({
  providedIn: 'root'
})
export class SystemInformationService {
  get systemVersion(): string {
    return this.systemInformation.systemVersion;
  }

  private systemInformation: ISystemInformation = {
    systemVersion: ''
  }

  constructor(private httpClient: HttpClient) { }

  loadSystemVersionInformation(): Observable<ISystemInformation> {
    return this.httpClient.get<ISystemInformation>(combineUrl(hostAddress(), 'api/system-information'))
      .pipe(
        tap(systemInformation => this.systemInformation = systemInformation),
        catchError( error => {
          console.log(`Loading system information failed: ${error?.message}, using default development system information`)

          return of(this.systemInformation);
        })
      );
  }
}
