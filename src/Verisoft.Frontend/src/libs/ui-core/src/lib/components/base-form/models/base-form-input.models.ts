import { NgControl } from '@angular/forms';

export abstract class BaseInputControls<T> {
  value?: T | null;

  readonly ngControl?: NgControl;

  readonly label?: string;

  readonly required!: boolean;
}
