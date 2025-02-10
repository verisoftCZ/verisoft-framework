import {
  AfterViewInit,
  ChangeDetectorRef,
  Directive,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { cloneDeep } from 'lodash-es';
import { Subject, takeUntil, filter, map } from 'rxjs';
import { FormState, isFormStateEqual } from './models';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[v-baseForm]',
  standalone: true,
})
export abstract class BaseFormDirective<T extends object>
  implements OnInit, OnChanges, OnDestroy, AfterViewInit
{
  @Input() data!: T | any;
  @Output() dataChange = new EventEmitter<T>();
  @Output() statusChange = new EventEmitter<FormState>();
  @Output() formSubmit = new EventEmitter<T>();
  @Output() formClear = new EventEmitter<void>();

  formDestroyed$ = new Subject<void>();
  keys: { key: string; alias?: string; type?: string; readOnly: string }[] = [];
  formGroup!: FormGroup;
  cd = inject(ChangeDetectorRef);
  valueInitialization = false;
  lastState!: FormState;

  ngOnInit(): void {
    this.initializeFormGroup();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialData'] && this.formGroup) {
      this.valueInitialization = true;
      const dirty = this.formGroup.dirty;

      this.formGroup.patchValue(this.fromModel(this.data), {
        emitEvent: false,
        onlySelf: true,
      });

      this.formGroup.updateValueAndValidity();
      if (!dirty) {
        this.formGroup.markAsPristine();
      }

      this.valueInitialization = false;
      this.createAndEmitIfFormStateChanged();
    }
  }

  ngAfterViewInit() {
    this.cd.detectChanges();
  }

  ngOnDestroy(): void {
    this.formDestroyed$.next();
    this.formDestroyed$.complete();
  }

  abstract createFormGroup(): FormGroup;

  createCompleteData() {
    const change = this.toModel(this.formGroup?.value);
    this.recursiveObjectAttributesTransformation(change);
    const updatedData = this.applyChanges(
      cloneDeep(this.data),
      cloneDeep(change)
    );

    return updatedData;
  }

  submit() {
    this.formGroup.markAllAsTouched();
    if (!this.formGroup.invalid) {
      this.formSubmit.emit(this.createCompleteData());
    }
    this.formGroup.markAsUntouched();
  }

  clear() {
    this.formClear.emit();
  }

  private initializeFormGroup() {
    this.formGroup = this.createFormGroup();
    this.valueInitialization = true;
    this.formGroup.patchValue(this.fromModel(this.data), {
      emitEvent: false,
      onlySelf: true,
    });

    this.formGroup.markAsPristine();
    this.initValueChanges();
    this.initStatusChanges();
    this.valueInitialization = false;
  }

  private initValueChanges() {
    this.formGroup.valueChanges
      .pipe(
        takeUntil(this.formDestroyed$),
        filter(() => !this.valueInitialization),
        map((value) => {
          const change = this.toModel(value);
          this.recursiveObjectAttributesTransformation(change);
          const updatedData = this.applyChanges(
            cloneDeep(this.data),
            cloneDeep(change)
          );
          return updatedData;
        })
      )
      .subscribe((updatedData) => {
        this.dataChange.emit(updatedData);
      });
  }

  private initStatusChanges() {
    this.formGroup.statusChanges
      .pipe(takeUntil(this.formDestroyed$))
      .subscribe(() => {
        if (!this.valueInitialization) {
          this.createAndEmitIfFormStateChanged();
        }
      });

    this.createAndEmitIfFormStateChanged();
  }

  private createAndEmitIfFormStateChanged() {
    const formState: FormState = {
      valid: !this.formGroup.invalid,
      dirty: this.formGroup.dirty,
    };

    if (!isFormStateEqual(formState, this.lastState)) {
      this.lastState = formState;
      this.statusChange.emit(formState);
    }
  }

  protected applyChanges(data: T, changes: Partial<T>): T {
    return Object.assign(data || ({} as T), changes);
  }

  protected toModel(data: T): Partial<T> {
    return data || {};
  }

  protected fromModel(data: T): T {
    return { ...(data || {}) } as T;
  }

  private recursiveObjectAttributesTransformation(obj: any): void {
    this.recursiveObjectAttributesTraversal(
      obj,
      this.transformEmptyStringToNullStringFn
    );
  }

  private recursiveObjectAttributesTraversal(
    obj: any,
    transformationFn: (obj: any, key: string) => void
  ) {
    if (
      obj === null ||
      transformationFn === null ||
      typeof transformationFn !== 'function'
    ) {
      return;
    }

    const traverse = (obj: any) => {
      for (const key in obj) {
        // eslint-disable-next-line no-prototype-builtins
        if (obj.hasOwnProperty(key)) {
          transformationFn(obj, key);

          if (typeof obj[key] === 'object') {
            traverse(obj[key]);
          }
        }
      }
    };

    traverse(obj);
  }

  private transformEmptyStringToNullStringFn(obj: any, key: string) {
    // if empty string - transformation to null string
    if (typeof obj[key] === 'string' && obj[key] === '') {
      obj[key] = null;
    }
  }
}
