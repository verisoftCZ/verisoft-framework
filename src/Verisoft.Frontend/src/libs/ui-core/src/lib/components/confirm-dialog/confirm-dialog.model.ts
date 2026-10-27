import { InjectionToken, Type } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { ControlSeverityType } from '../../common';

export const CONFIRM_DIALOG_COMPONENT_TOKEN =
  new InjectionToken<ConfirmDialogCore>('ConfirmDialogComponentToken');

export type ConfirmDialogCore = object;

export interface DialogData {
  title?: string;
  headerIcon?: string;
  severity?: ControlSeverityType;
  showCancelButton?: boolean;
  buttonOrder?: 'confirm-cancel' | 'cancel-confirm';
  confirmButtonText?: string;
  confirmButtonDisabled?: boolean;
  confirmButtonFn?: () => void;
  cancelButtonFn?: () => void;
  cancelButtonText?: string;
  innerHTML?: string | SafeHtml;
  data?: unknown;
  componentType?: Type<unknown>;
  closable?: boolean;
  width?: string;
  height?: string;
}

export interface ExtendedDialogData<TInputs = unknown, TOutputs = unknown>
  extends DialogData {
  data?: TInputs & TOutputs;
}
