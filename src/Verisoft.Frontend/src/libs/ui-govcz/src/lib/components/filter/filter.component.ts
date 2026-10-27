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
  FormControl,
  FormGroup,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BaseHttpService } from '@verisoft/core';
import {
  ButtonShortCutDirective,
  DEFAULT_DEBOUNCE_TIME,
  DialogService,
  FieldSize,
  FILTER_COMPONENT_TOKEN,
  FilterCore,
  FilterFieldDirective,
  GenericFieldDefinition,
  getFilledControlCount,
  GovButtonType,
  GovButtonTypeType,
  isFilterEmpty,
  ScreenSizeService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
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
import { Icons } from '../../icons';
import { ButtonComponent } from '../button';
import { generateFormGroup, GenericFormComponent } from '../shared-components/generic-form';
import { TextfieldComponent } from '../textfield';

type FilterValueType = { [key: string]: unknown };

@Component({
    selector: 'v-filter',
    imports: [
        AsyncPipe,
        ReactiveFormsModule,
        TranslateModule,
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
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FilterComponent<T extends object>
  extends UnsubscribeComponent
  implements OnChanges, OnInit, AfterContentInit, FilterCore
{
  @ContentChildren(FilterFieldDirective)
  fieldDeclarations!: QueryList<FilterFieldDirective>;

  @Input() filterDetailTitle = 'Set filters';

  @Input() applyButtonLabel = 'Apply';

  @Input() clearButtonLabel = 'Clear all';

  @Input() fields: GenericFieldDefinition[] = [];

  @Input() filters: GenericFieldDefinition[] = [];

  @Input() title?: string;

  @Input() total?: number;

  @Input() fulltextFieldName = 'searchField';

  @Input() showFulltext = true;

  @Input() filterDetailShowFulltext = true;

  @Input() showFilters = true;

  @Input() autoBind = true;

  @Input() buttonType: GovButtonTypeType = GovButtonType.solid;

  @Input() buttonOutlined = false;

  @Input() debounceTime?: number = DEFAULT_DEBOUNCE_TIME;

  @Input() defaultExpanded = false;

  icons = Icons;

  protected isExpanded = false;

  fieldDefinitios$?: Observable<GenericFieldDefinition[]>;

  formGroup$?: Observable<FormGroup>;

  simpleFormFieldDefinitions$?: Observable<GenericFieldDefinition[]>;

  private get searchField(): GenericFieldDefinition {
    return {
      name: this.fulltextFieldName,
    };
  }

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
    this.isExpanded = this.defaultExpanded;
    this.formGroup.valueChanges
      .pipe(
        takeUntil(this.destroyed$),
        debounceTime(this.debounceTime ?? DEFAULT_DEBOUNCE_TIME),
        map((x) => this.convertFilter(x)),
        distinctUntilChanged()
      )
      .subscribe((value) => {
        this.onChange?.(value);
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['total']) {
      this.changeDetectorRef.markForCheck();
    }
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
        const filter = [...(inputs ?? []), ...views]
        this.lastFormFields = this.filterDetailShowFulltext ? [this.searchField, ...filter] : filter;
        return this.lastFormFields;
      })
    );

    this.simpleFormFieldDefinitions$ = this.fieldDefinitios$.pipe(
      map((fields) => {
        return fields
          .filter((x) => x.name !== this.fulltextFieldName)
          .map((x) => ({ ...x, floatLabel: false, size: x.size ?? FieldSize.medium }));
      })
    );

    this.formGroup$ = this.fieldDefinitios$.pipe(
      map((fields) => {
        const group = generateFormGroup(fields, this.formGroup, undefined, false);
        if (this.showFulltext && !group.get(this.fulltextFieldName)) {
          group.addControl(this.fulltextFieldName, new FormControl(''));
        }
        return group;
      })
    );

    this.changeDetectorRef.detectChanges();
  }

  protected toggleFilters() {
    this.isExpanded = !this.isExpanded;
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
