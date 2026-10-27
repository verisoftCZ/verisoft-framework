import { Injectable, inject } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subject } from 'rxjs';
import { PreventUnsavedChangesCore, CommonIcons } from '../common';
import { DialogService } from './confirm-dialog.service';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesDirective implements CanDeactivate<PreventUnsavedChangesCore> {
  private dialogService = inject(DialogService);
  private translateService = inject(TranslateService);
  private readonly Icons!: CommonIcons;

  canDeactivate(component: PreventUnsavedChangesCore): Observable<boolean> | Promise<boolean> | boolean {
    if (!component || !component.canDeactivate) {
      return true;
    }
    const result = component.canDeactivate();
    return result;
  }

  showConfirmationDialog(): Observable<boolean> {
    const resultSubject = new Subject<boolean>();

    const title = this.translateService.instant('VALIDATIONS.UNSAVED_CHANGES');
    const message = this.translateService.instant('VALIDATIONS.LEAVING_UNSAVED_FORM');
    const leaveButton = this.translateService.instant('BUTTONS.LEAVE');
    const stayButton = this.translateService.instant('BUTTONS.STAY');

    this.dialogService.showDialog({
      title: title,
      headerIcon: this.Icons.infoCircle,
      innerHTML: `<p>${message}</p>`,
      showCancelButton: true,
      confirmButtonText: stayButton,
      cancelButtonText: leaveButton,
      confirmButtonFn: () => {
        resultSubject.next(false);
        resultSubject.complete();
        this.dialogService.closeModal();
      },
      cancelButtonFn: () => {
        resultSubject.next(true);
        resultSubject.complete();
        this.dialogService.closeModal();
      },
    });

    return resultSubject.asObservable();
  }
}
