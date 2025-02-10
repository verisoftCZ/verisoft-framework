import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import {
  CONFIRM_DIALOG_COMPONENT_TOKEN,
  ConfirmDialogCore,
  DialogData,
  DialogService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
import { AvatarModule } from 'primeng/avatar';
import { DialogModule } from 'primeng/dialog';
import { takeUntil } from 'rxjs';
import { ButtonComponent } from '../button';
import { DynamicComponent } from '../shared-components';

@Component({
  selector: 'v-confirm-dialog',
  standalone: true,
  imports: [
    CommonModule,
    DialogModule,
    ButtonComponent,
    AvatarModule,
    DynamicComponent,
  ],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.scss',
  providers: [
    {
      provide: CONFIRM_DIALOG_COMPONENT_TOKEN,
      useExisting: ConfirmDialogComponent,
    },
  ],
})
export class ConfirmDialogComponent
  extends UnsubscribeComponent
  implements OnInit, ConfirmDialogCore
{
  constructor(
    private readonly dialogService: DialogService,
    private readonly cdr: ChangeDetectorRef
  ) {
    super();
  }

  visible = false;
  protected closable = false;
  data!: DialogData;

  ngOnInit(): void {
    this.dialogService.showEvent
      .pipe(takeUntil(this.destroyed$))
      .subscribe((x: DialogData) => {
        this.data = x;
        this.visible = true;
        this.cdr.detectChanges();
      });
  }

  protected dialogClick(confirm: boolean) {
    const { confirmButtonFn, cancelButtonFn } = this.data;

    if (confirm && confirmButtonFn) {
      confirmButtonFn();
    }

    if (!confirm && cancelButtonFn) {
      cancelButtonFn();
    }

    this.visible = false;
    this.cdr.detectChanges();
  }
}
