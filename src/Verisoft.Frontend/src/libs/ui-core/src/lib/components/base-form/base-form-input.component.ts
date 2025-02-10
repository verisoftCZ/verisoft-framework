import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormControl,
  FormControlDirective,
  FormControlName,
  FormGroupDirective,
  NgControl,
  NgModel,
  ReactiveFormsModule,
} from '@angular/forms';
import { BaseInputControls } from './models/base-form-input.models';

const noop = () => {
  /* */
};

@Component({
  template: '',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class BaseFormInputComponent
  implements BaseInputControls<any>, ControlValueAccessor, OnInit
{
  readonly ngControl?: NgControl;

  formControl!: FormControl;

  constructor(private readonly control: NgControl) {
    this.ngControl = control;
    if (this.control) {
      this.ngControl.valueAccessor = this;
    }
  }

  @Input()
  label?: string;

  @Input()
  required = false;

  @Input()
  readonly!: boolean;

  @Input()
  tooltip!: string;

  @Input()
  formDisplay: 'flex' | 'block' = 'flex';

  @Input()
  clearable = true;

  @Input()
  placeholder?: string = '';

  @Input()
  testId?: string;

  inputId = Math.random().toString(36).substring(2);

  selectionChanged: any = noop;

  onTouch: any = noop;

  registerOnChange(fn: any): void {
    this.selectionChanged = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  writeValue(value: any): void {}

  ngOnInit(): void {
    if (this.ngControl) {
      if (this.ngControl instanceof FormControlName) {
        this.formControl =
          this.ngControl.control ||
          ((this.ngControl.formDirective as FormGroupDirective)?.form.controls[
            this.ngControl.name as string
          ] as FormControl);
      } else if (
        this.ngControl instanceof FormControlDirective ||
        this.ngControl instanceof NgModel
      ) {
        this.formControl = this.ngControl.control;
        if (this.ngControl instanceof NgModel) {
          this.formControl.valueChanges.subscribe(() =>
            this.ngControl?.viewToModelUpdate(this.control?.value)
          );
        }
      } else {
        this.formControl = new FormControl();
      }
    } else {
      this.formControl = new FormControl();
    }
  }

  isRequired() {
    if (this.ngControl) {
      const validator = this.ngControl?.control?.validator?.(
        {} as AbstractControl
      );
      return this.required || (validator && validator['required']);
    }
    return this.required;
  }
}
