import { AsyncPipe } from '@angular/common';
import {
  AfterContentInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChildren,
  forwardRef,
  inject,
  Input,
  OnChanges,
  OnInit,
  QueryList,
  SimpleChanges,
} from '@angular/core';
import {
  FormGroup,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { BaseHttpService } from '@verisoft/core';
import {
  ButtonShortCutDirective,
  DEFAULT_DEBOUNCE_TIME,
  DialogService,
  FieldSize,
  FILTER_COMPONENT_TOKEN,
  FilterCore,
  GenericFieldDefinition,
  isFilterEmpty,
  ScreenSizeService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
import { BadgeModule } from 'primeng/badge';
import {
  BehaviorSubject,
  combineLatestWith,
  debounceTime,
  distinctUntilChanged,
  map,
  Observable,
  startWith,
  takeUntil,
} from 'rxjs';
import { ButtonComponent } from '../../button';
import { TextfieldComponent } from '../../textfield';
import { generateFormGroup, GenericFormComponent } from '../generic-form';
import { FilterFieldDirective } from './directives/filter-field.directive';
import { getFilledControlCount } from './filter.model';
import { Icons } from '../../../icons';

type FilterValueType = { [key: string]: unknown };

@Component({
  selector: 'v-filter',
  standalone: true,
  imports: [
    AsyncPipe,
    ReactiveFormsModule,
    BadgeModule,
    TextfieldComponent,
    GenericFormComponent,
    ButtonComponent,
    ButtonShortCutDirective,
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FilterComponent),
      multi: true,
    },
    {
      provide: FILTER_COMPONENT_TOKEN,
      useExisting: FilterComponent,
    },
  ],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterComponent<T extends object>
  extends UnsubscribeComponent
  implements OnChanges, OnInit, AfterContentInit, FilterCore
{
  @ContentChildren(FilterFieldDirective)
  fieldDeclarations!: QueryList<FilterFieldDirective>;

  @Input() fields: GenericFieldDefinition[] = [];

  @Input() filters: GenericFieldDefinition[] = [];

  @Input() title?: string;

  @Input() fulltextFieldName = 'searchField';

  @Input() showFulltext = true;

  @Input() showFilters = true;

  @Input() autoBind = true;

  @Input() debounceTime?: number = DEFAULT_DEBOUNCE_TIME;

  icons = Icons;

  fieldDefinitios$?: Observable<GenericFieldDefinition[]>;

  formGroup$?: Observable<FormGroup>;

  simpleFormFieldDefinitions$?: Observable<GenericFieldDefinition[]>;

  private searchField: GenericFieldDefinition = {
    name: this.fulltextFieldName,
  };

  private onTouch?: () => void;

  private onChange?: (value: FilterValueType | undefined) => void;

  private inputFields$ = new BehaviorSubject<
    GenericFieldDefinition[] | undefined
  >([...this.fields ?? [], ...this.filters ?? []]);

  FieldSize = FieldSize;

  protected screenSizeService = inject(ScreenSizeService);
  protected changeDetectorRef = inject(ChangeDetectorRef);
  protected dialogService = inject(DialogService);

  service!: BaseHttpService<T>;

  formGroup = new FormGroup({});

  filledFiltersCount$: Observable<string> = this.formGroup.valueChanges.pipe(
    startWith(this.formGroup),
    map(() => getFilledControlCount(this.formGroup).toString())
  );

  private lastFormFields: GenericFieldDefinition[] = [];

  ngOnInit(): void {
    this.formGroup.valueChanges
      .pipe(
        takeUntil(this.destroyed$),
        debounceTime(this.debounceTime ?? DEFAULT_DEBOUNCE_TIME),
        map((x) => this.convertFilter(x)),
        distinctUntilChanged()
      )
      .subscribe((value) => this.onChange?.(value));
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fields'] || changes['filters']) {
      this.inputFields$.next([...this.fields ?? [], ...this.filters ?? []]);
    }
  }

  ngAfterContentInit(): void {
    const fieldDeclaratios$: Observable<GenericFieldDefinition[]> =
      this.fieldDeclarations.changes.pipe(
        startWith({}),
        map(() => this.fieldDeclarations.toArray())
      );

    this.fieldDefinitios$ = this.inputFields$.pipe(
      combineLatestWith(fieldDeclaratios$),
      map(([inputs, views]) => {
        this.searchField.name = this.fulltextFieldName;
        this.lastFormFields = [this.searchField, ...(inputs ?? []), ...views];
        return this.lastFormFields;
      })
    );

    this.simpleFormFieldDefinitions$ = this.fieldDefinitios$.pipe(
      map((fields) => {
        return fields
          .filter((x) => x.name !== this.fulltextFieldName)
          .map((x) => ({ ...x, floatLabel: true, size: FieldSize.large }));
      })
    );

    this.formGroup$ = this.fieldDefinitios$.pipe(
      map((fields) =>
        generateFormGroup(fields, this.formGroup, undefined, false)
      )
    );

    this.changeDetectorRef.detectChanges();
  }

  protected openFilter() {
    const fields = this.lastFormFields.map((x) => ({
      ...x,
      label:
        x.name === this.fulltextFieldName ? 'Fulltext' : x.label ?? x.name,
    }));

    const formGroup = generateFormGroup(
      this.lastFormFields,
      undefined,
      undefined,
      false
    );
    formGroup.patchValue(this.formGroup.value);
    this.dialogService.showDialog({
      title: 'Set filters',
      headerIcon: this.icons.filter,
      severity: 'primary',
      componentType: GenericFormComponent,
      data: {
        formGroup,
        fields: fields,
        columns: 1,
      },
      confirmButtonFn: () => this.setFilterValues(formGroup),
      confirmButtonText: 'Apply',
      cancelButtonFn: () => this.clear(),
      cancelButtonText: 'Clear all',
      showCancelButton: true,
      closable: false,
    });
  }

  protected openSearch() {
    this.dialogService.showDialog({
      headerIcon: 'pi pi-search',
      severity: 'primary',
      innerHTML: '<p>Search</p>',
      confirmButtonFn: () => this.submitValue(),
      confirmButtonText: 'Apply',
      cancelButtonFn: () => this.clear(),
      cancelButtonText: 'Clear all',
      showCancelButton: true,
      closable: false,
    });
  }

  writeValue(data: FilterValueType): void {
    this.formGroup.patchValue(data);
  }

  registerOnChange(fn: (value: FilterValueType | undefined) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouch = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.formGroup.disable() : this.formGroup.enable();
  }

  submitValue() {
    if (!this.autoBind) {
      this.onChange?.(this.formGroup.value);
    }
  }

  setFilterValues(dialogFormGroup: FormGroup) {
    this.formGroup.setValue(dialogFormGroup.value);
    this.submitValue();
  }

  clear() {
    this.formGroup.reset();
    this.submitValue();
  }

  private convertFilter(
    value: FilterValueType | undefined
  ): FilterValueType | undefined {
    if (value == undefined) {
      return undefined;
    }

    const isEmpty = isFilterEmpty(value);
    if (isEmpty) {
      return undefined;
    }

    return value;
  }
}
