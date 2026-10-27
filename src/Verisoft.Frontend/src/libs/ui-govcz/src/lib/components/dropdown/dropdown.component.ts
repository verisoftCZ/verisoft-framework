import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  NO_ERRORS_SCHEMA,
  OnChanges,
  OnDestroy,
  OnInit,
  Optional,
  Output,
  Self,
  SimpleChanges,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { TranslateModule } from '@ngx-translate/core';
import { DEFAULT_PAGE_SIZE, FilterEvent, getValueByPath, LazyLoadEvent } from '@verisoft/core';
import {
  BaseFormInputComponent,
  DropdownCore,
  DROPDOWN_COMPONENT_TOKEN,
  FieldSizeType,
  FieldSize,
} from '@verisoft/ui-core';
import { debounceTime, distinctUntilChanged, Subscription } from 'rxjs';
import { Icons } from '../../icons';
import { ErrorComponent } from '../errors';
import { TextfieldComponent } from '../textfield';
import { DropdownItemComponent } from './dropdown-item.component';
import { DropDownItemData } from './dropdown.model';

@Component({
    selector: 'v-dropdown',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        GovDesignSystemModule,
        ErrorComponent,
        DropdownItemComponent,
        TextfieldComponent,
        ScrollingModule,
        TranslateModule,
    ],
    templateUrl: './dropdown.component.html',
    styleUrl: './dropdown.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: DROPDOWN_COMPONENT_TOKEN,
            useExisting: DropdownComponent,
        },
    ],
    schemas: [
        NO_ERRORS_SCHEMA
    ]
})
export class DropdownComponent<T>
  extends BaseFormInputComponent
  implements ControlValueAccessor, DropdownCore<T>, OnInit, OnChanges, OnDestroy {

  @ViewChild('filterTextField', { static: false }) filterTextField!: ElementRef;

  @Input() options: T[] | undefined = [];
  @Input() optionLabel: string | undefined;
  @Input() optionValue: string | undefined;
  @Input() dropdownIcon: string | undefined;
  @Input() floatLabel: string | undefined;
  @Input() lazy!: boolean;
  @Input() filter!: boolean;
  @Input() editable!: boolean;
  @Input() loading!: boolean;
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() showFilter = true;
  @Input() localSearch = true;
  @Input() forceMinWidth = false;

  @Output() changed = new EventEmitter<any>();
  @Output() showed = new EventEmitter<any>();
  @Output() cleared = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() filtered = new EventEmitter<FilterEvent>();

  opened = false;
  icons = Icons;
  selectedItem: DropDownItemData<T> | undefined;
  filterControl = new FormControl('');
  filterSub: Subscription | undefined
  dropdownOptions: DropDownItemData<T>[] | undefined;
  private readonly pageSize = DEFAULT_PAGE_SIZE;
  private lastPage: number | undefined;

  get labelClasses() {
    return {
      disabled: this.ngControl?.disabled,
      error: this.ngControl?.invalid
    };
  }

  get inputClasses() {
    return {
      'disabled-input': this.ngControl?.disabled,
      'error-input': this.ngControl?.invalid
    };
  }

  constructor(@Optional() @Self() ngControl: NgControl,
    private changeDetectorRef: ChangeDetectorRef) {
    super(ngControl);
  }

  override ngOnInit() {
    super.ngOnInit();
    this.filterSub = this.filterControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((value) => {
      this.filterOptions(value);
    });
  }

  override writeValue(value: any): void {
    const selectedItem = (value == undefined || value === "") ? undefined : (this.dropdownOptions?.find(x => x.value === value)
      ?? { value, label: '', option: undefined as T, selected: false } as unknown as DropDownItemData<T>)
    this.selectItem(selectedItem, false);

    this.changeDetectorRef.detectChanges();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['options'] || changes['loading']) {
      this.dropdownOptions = this.convertToOptions();
      if (this.selectedItem && !this.selectedItem?.item) {
        const item = this.dropdownOptions?.find(x => this.selectedItem?.value === x.value);
        if (item) {
          this.selectItem(item, false);
        }
      }
    }

    this.changeDetectorRef.detectChanges();
  }

  private convertToOptions(): DropDownItemData<T>[] | undefined {
    return this.options?.map((option) => ({
      label: (this.optionLabel ? getValueByPath(option, this.optionLabel) : option) as unknown as string,
      value: this.optionValue ? getValueByPath(option, this.optionValue) : option as unknown as string,
      item: option,
      selected: false,
    } as DropDownItemData<T>)
    );
  }

  ngOnDestroy() {
    this.filterSub?.unsubscribe();
  }

  toggleDropdown() {
    if (this.ngControl?.disabled) {
      return;
    }

    if (!this.opened) {
      this.openDropdown();
    } else {
      this.closeDropdown();
    }
  }

  protected clearSelection(event: MouseEvent) {
    event.stopPropagation();
    this.changed.emit(null);
    this.dropdownOptions = this.dropdownOptions?.map(x => ({ ...x, selected: false })) ?? [];
    this.ngControl?.control?.setValue(null);
  }

  private openDropdown() {
    this.opened = true;
    this.setFilterFocus();
    this.showed.emit();
  }

  selectItem(option: DropDownItemData<T> | undefined, emitChange: boolean) {
    if (option == undefined) {
      this.selectedItem = undefined;
      this.closeDropdown();
      return;
    }

    if (this.selectedItem && this.selectedItem != option) {
      this.selectedItem.selected = false;
    }

    option.selected = true;
    this.selectedItem = option;

    if (emitChange) {
      if (this.ngControl?.control?.value !== option.value) {
        this.changed.emit(option.value);
        this.ngControl?.control?.setValue(option?.value);
      }
    }

    this.closeDropdown();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('v-dropdown')) {
      this.opened = false;
    }
  }

  onScroll(index: number) {
    this.loadDataForIndex(index);
  }

  onKeyDown(event: KeyboardEvent) {
    if (this.formControl.disabled || this.readonly || this.loading || !this.opened) {
      return;
    }

    switch (event.code) {
      case 'ArrowDown':
        break;

      case 'ArrowUp':
        break;

      case 'ArrowLeft':
      case 'ArrowRight':
        break;
    }
  }

  private loadDataForIndex(index: number) {
    if (this.lazy) {
      const currentPage = Math.floor(index / this.pageSize);
      const shouldLazyLoad = currentPage !== this.lastPage;
      if (shouldLazyLoad) {
        this.lastPage = currentPage;
        this.lazyLoad.emit({
          filter: this.createFilterObject(this.filterControl.value),
          offset: currentPage * this.pageSize,
          limit: this.pageSize,
        });
      }
    }
  }

  private setFilterFocus() {
    setTimeout(() => {
      this.filterTextField?.nativeElement?.querySelector("input").focus();
    }, 200);
  }

  private closeDropdown() {
    this.opened = false;
  }

  private filterOptions(value: string | null) {
    if (!this.localSearch) {
      this.filtered.emit({
        filter: !value ? undefined : value,
      });
    }

    if (this.lazy) {
      this.loadDataForIndex(0);
    } else {
      const loweredFilterValue = value?.toLocaleLowerCase();
      this.dropdownOptions = !loweredFilterValue ?
        this.convertToOptions()
        : this.convertToOptions()?.filter(x => x.label?.toLocaleLowerCase()?.indexOf(loweredFilterValue) !== -1);
      this.changeDetectorRef.detectChanges();
    }
  }

  private createFilterObject(filterValue: string | null): Partial<T> {
    return (this.optionLabel ? { [this.optionLabel]: filterValue } : filterValue) as unknown as Partial<T>
  }
}
