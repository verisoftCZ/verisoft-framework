import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  constructor(readonly messageService: MessageService) {}

  showSuccess(message: string, icon?: string) {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: message,
      icon: icon,
    });
  }

  showInfo(message: string, icon?: string) {
    this.messageService.add({
      severity: 'info',
      summary: 'Info',
      detail: message,
      icon: icon,
    });
  }

  showWarn(message: string, icon?: string) {
    this.messageService.add({
      severity: 'warn',
      summary: 'Warn',
      detail: message,
      icon: icon,
    });
  }

  showError(message: string, icon?: string) {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail: message,
      icon: icon,
    });
  }
}
