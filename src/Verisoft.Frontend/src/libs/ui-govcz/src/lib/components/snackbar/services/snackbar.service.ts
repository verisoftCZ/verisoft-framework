import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import * as portal from '@angular/cdk/portal';
import { Injectable, Injector } from '@angular/core';
import { SnackbarComponent } from '../snackbar.component';
import { SnackbarConfig } from '../snackbar.model';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  constructor(private overlay: Overlay, private injector: Injector) {}

  showSuccess(message: string, icon?: string) {
    this.showToast({
      message: message,
      icon: icon,
      color: 'success',
    });
  }

  showInfo(message: string, icon?: string) {
    this.showToast({
      message: message,
      icon: icon,
      color: 'primary',
    });
  }

  showWarn(message: string, icon?: string) {
    this.showToast({
      message: message,
      icon: icon,
      color: 'warning',
    });
  }

  showError(message: string, icon?: string) {
    this.showToast({
      message: message,
      icon: icon,
      color: 'error',
    });
  }

  private showToast(config: SnackbarConfig) {
    const positionStrategy = this.overlay
      .position()
      .global();

    const overlayRef: OverlayRef = this.overlay.create({
      positionStrategy,
      hasBackdrop: false,
      scrollStrategy: this.overlay.scrollStrategies.noop(),
    });

    const toastPortal = new portal.ComponentPortal(SnackbarComponent, null, this.createInjector(config));
    overlayRef.attach(toastPortal);
  }

  private createInjector(config: any) {
    return Injector.create({
      providers: [
        { provide: 'TOAST_CONFIG', useValue: config },  
      ],
      parent: this.injector,
    });
  }
}