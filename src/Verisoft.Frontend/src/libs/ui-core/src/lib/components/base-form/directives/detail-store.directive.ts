import {
  Directive,
  AfterViewInit,
  inject,
  Input,
  OnInit,
  ChangeDetectorRef,
  OnDestroy,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { createFeatureSelector, createSelector, Store } from '@ngrx/store';
import {
  createInitDetailAction,
  createResetStateAction,
  createUpdateDetailAction,
  createUpdateFormStateAction,
  DetailState,
} from '@verisoft/store';
import { takeUntil } from 'rxjs';
import { UnsubscribeComponent } from '../../unsubscribe.component';
import { BaseFormDirective } from '../base-form.component';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[v-useDetailStore]',
  exportAs: 'useDetailStore',
  standalone: true,
})
export class DetailStoreDirective
  extends UnsubscribeComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  store = inject(Store);
  cdr = inject(ChangeDetectorRef);
  route = inject(ActivatedRoute);
  @Input({ required: true })
  form!: BaseFormDirective<any>;
  @Input({ required: true }) detailsRepository!: string;
  @Input() autoBind = true;
  @Input() detailId!: string | number | undefined;
  @Input({ required: true }) ngrxFeatureKey!: string;
  @Input() destroyForm = true;

  private loaded!: boolean;

  ngOnInit(): void {
    if (!this.loaded) {
      this.listenFormState();
    }
    this.listenFormChange();
    this.listenFormStatusChange();
  }

  ngAfterViewInit(): void {
    if (this.autoBind && !this.loaded) {
      this.initForm();
    }
  }

  override ngOnDestroy(): void {
    super.ngOnDestroy();
    if (this.destroyForm) {
      this.store.dispatch(createResetStateAction(this.detailsRepository)());
    }
  }

  private initForm() {
    this.store.dispatch(
      createInitDetailAction(this.detailsRepository)({ obj: this.detailId })
    );
  }

  private listenFormState() {
    const selectIncomeData = createSelector(
      createFeatureSelector<any>(this.ngrxFeatureKey),
      (state: any) => state?.[this.detailsRepository] as DetailState<any>
    );
    this.store
      .select(selectIncomeData)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(({ item, loaded } = { item: undefined, loaded: false }) => {
        if (
          item &&
          (item.validationErrors || item.validationWarnings) &&
          !this.form.formGroup.dirty
        ) {
          this.handleValidation(
            'propertyName',
            item.validationWarnings || [],
            'validationWarning',
            'warningMessage'
          );
          this.handleValidation(
            'propertyName',
            item.validationErrors || [],
            'validationError',
            'errorMessage'
          );
        }

        this.loaded = loaded;
        this.cdr.detectChanges();
      });
  }

  private listenFormChange() {
    this.form.dataChange.pipe(takeUntil(this.destroyed$)).subscribe((item) => {
      this.store.dispatch(
        createUpdateDetailAction(this.detailsRepository)({ item })
      );
    });
  }

  private listenFormStatusChange() {
    this.form.statusChange
      .pipe(takeUntil(this.destroyed$))
      .subscribe((formState) => {
        this.store.dispatch(
          createUpdateFormStateAction(this.detailsRepository)({ formState })
        );
      });
  }

  private handleValidation = (
    controlName: string,
    errors: any[],
    errorKey: string,
    messageKey: string
  ) => {
    if (errors.length > 0) return;
    errors.forEach((error: any) => {
      const control = this.form.formGroup.get(error[controlName]);
      if (control) {
        control.disabled
          ? control.disable({ emitEvent: false, onlySelf: true })
          : control.enable({ emitEvent: false, onlySelf: true });
        control.setErrors(
          { [errorKey]: error[messageKey] },
          { emitEvent: true }
        );
        control.markAsDirty();
      }
    });
    this.cdr.markForCheck();
  };
}
