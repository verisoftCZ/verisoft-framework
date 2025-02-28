import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  CONFIRM_DIALOG_COMPONENT_TOKEN,
  ConfirmDialogCore,
  DialogData,
  DialogService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
import { takeUntil } from 'rxjs';
import { GovColorPipe } from '../../pipes';
import { DynamicComponent } from '../shared-components';

@Component({
  selector: 'v-confirm-dialog',
  standalone: true,
  imports: [
    CommonModule,
    GovDesignSystemModule,
    GovColorPipe,
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
  data: DialogData = { severity: 'primary', headerIcon: 'info-circle' };

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
