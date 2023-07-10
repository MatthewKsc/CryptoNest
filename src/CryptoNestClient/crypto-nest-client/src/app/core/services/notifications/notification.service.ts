import { Injectable } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly defaultToastrNofiricationTimeOutInMilliseconds: number = 3000;

  constructor(private toastrService: ToastrService) { }

  notifyError(message: string, title: string): void {
    this.toastrService.error(message, title, { timeOut: this.defaultToastrNofiricationTimeOutInMilliseconds, onActivateTick: true });
  }

  notifySuccess(message: string, title: string): void {
    this.toastrService.success(message, title, { timeOut: this.defaultToastrNofiricationTimeOutInMilliseconds });
  }
}
