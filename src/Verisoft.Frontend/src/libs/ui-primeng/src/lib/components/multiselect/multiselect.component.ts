import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  forwardRef,
  Input,
  OnChanges,
  Optional,
  Output,
  Self,
  SimpleChanges,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  FilterEvent,
  LazyLoadEvent,
} from '@verisoft/core';
import {
  BaseFormInputComponent,
  BaseInputControls,
  MULTISELECT_COMPONENT_TOKEN,
  MultiselectCore,
  FieldSize,
  FieldSizeType,
} from '@verisoft/ui-core';
import { ScrollerOptions } from 'primeng/api';
import { FloatLabelModule } from 'primeng/floatlabel';
import { MultiSelectModule } from 'primeng/multiselect';
import { ScrollerLazyLoadEvent } from 'primeng/scroller';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-multiselect',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FloatLabelModule,
    MultiSelectModule,
    FormFieldComponent,
  ],
  templateUrl: './multiselect.component.html',
  styleUrl: './multiselect.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => MultiselectComponent),
      multi: true,
    },
    {
      provide: MULTISELECT_COMPONENT_TOKEN,
      useExisting: MultiselectComponent,
    },
  ],
})
export class MultiselectComponent<T>
  extends BaseFormInputComponent
  implements ControlValueAccessor, MultiselectCore<T>, OnChanges
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() options: T[] | undefined = [];
  @Input() optionLabel: string | undefined;
  @Input() optionValue: string | undefined;
  @Input() dropdownIcon?: string;
  @Input() floatLabel?: string;
  @Input() editable = true;
  @Input() loading = false;
  @Input() lazy = false;
  @Input() filter = true;
  @Input() size: FieldSizeType = FieldSize.medium;

  @Output() changed = new EventEmitter<any>();
  @Output() showed = new EventEmitter<any>();
  @Output() cleared = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() filtered = new EventEmitter<FilterEvent>();

  virtualScrollOptions!: ScrollerOptions | undefined;
  virtualOptionSize = 37.5;  

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['lazy'] || changes['loading']) {
      this.virtualScrollOptions = this.lazy
        ? {
            showLoader: this.loading,
            lazy: true,
            onLazyLoad: this.onLazyLoad.bind(this),
          }
        : undefined;
    }
  }

  onLazyLoad(event: ScrollerLazyLoadEvent): void {
    this.lazyLoad.emit({
      offset: event.first,
      limit: event.last - event.first,
    });
  }

  selectionChange(event: any): void {
    this.cleared.emit(event);
  }

  onDropdownShow(): void {
    this.showed.emit();
  }

  onDropdownClear(): void {
    this.cleared.emit();
  }
}
