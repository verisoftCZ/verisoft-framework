import { InjectionToken, Type } from '@angular/core';
import { SafeHtml } from "@angular/platform-browser";
import { ControlSeverityType } from '../../common';

export const CONFIRM_DIALOG_COMPONENT_TOKEN = new InjectionToken<ConfirmDialogCore>(
    'ConfirmDialogComponentToken'
);

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface ConfirmDialogCore {}

export interface DialogData {
    title?: string;
    headerIcon?: string;
    severity?: ControlSeverityType;
    showCancelButton?: boolean;
    buttonOrder?: 'confirm-cancel' | 'cancel-confirm';
    confirmButtonText?: string;
    confirmButtonFn?: () => void;
    cancelButtonFn?: () => void;
    cancelButtonText?: string;
    innerHTML?: string | SafeHtml;
    data?: unknown;
    componentType?: Type<unknown>;
    closable?: boolean;
}