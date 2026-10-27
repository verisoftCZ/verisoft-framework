import { EventEmitter, Injectable, Output } from '@angular/core';
import { ExtendedDialogData } from '../components';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  @Output() showEvent: EventEmitter<ExtendedDialogData> =
    new EventEmitter<ExtendedDialogData>();

  @Output() closeEvent: EventEmitter<null> = new EventEmitter();

  private currentDialogData: ExtendedDialogData | null = null;

  showDialog<TInputs, TOutputs>(
    data: ExtendedDialogData<TInputs, TOutputs>
  ): void {
    const mappedData = this.mapInputAndOutpus(data);
    const dialogData = {
      ...data,
      data: mappedData,
    };

    this.currentDialogData = dialogData;
    this.showEvent.emit(dialogData);
  }

  updateDialog<TInputs = unknown, TOutputs = unknown>(
    patch: Partial<ExtendedDialogData<TInputs, TOutputs>>
  ): void {
    if (!this.currentDialogData) {
      return;
    }

    const mergedDialogData: ExtendedDialogData = {
      ...this.currentDialogData,
      ...patch,
      data: this.currentDialogData.data,
      componentType: this.currentDialogData.componentType,
    };

    this.currentDialogData = mergedDialogData;
    this.showEvent.emit(mergedDialogData);
  }

  private mapInputAndOutpus<TInputs, TOutputs>(
    data: ExtendedDialogData<TInputs, TOutputs>
  ): ExtendedDialogData<TInputs, TOutputs> {
    const inputsAndOutputs = data.data as TInputs & TOutputs;
    return {
      ...data.data,
      ...Object.keys(inputsAndOutputs ?? {})
        .filter(
          (key) => typeof inputsAndOutputs[key as keyof TOutputs] === 'function'
        )
        .reduce((acc, key) => {
          acc[key] = (value: unknown) => {
            if (typeof inputsAndOutputs[key as keyof TOutputs] === 'function') {
              (
                inputsAndOutputs[key as keyof TOutputs] as (
                  value: unknown
                ) => void
              )(value);
            }
          };
          return acc;
        }, {} as Record<string, unknown>),
    };
  }

  closeModal(): void {
    this.currentDialogData = null;
    this.closeEvent.emit();
  }
}
