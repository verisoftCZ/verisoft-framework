import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Optional,
  Output,
  Self,
  SimpleChanges,
  inject,
  ChangeDetectorRef
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  FilterEvent,
  LazyLoadEvent,
} from '@verisoft/core';
import {
  BaseFormInputComponent,
  ErrorPipe,
  MULTISELECT_COMPONENT_TOKEN,
  MultiselectCore,
  FieldSize,
  FieldSizeType,
  SlotPositionType,
  SlotPosition
} from '@verisoft/ui-core';
import { GovMultiselectOptionsPipe, GovSizePipe } from '../../pipes';
import { Icons } from '../../icons';

@Component({
  selector: 'v-multiselect',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe,
    GovMultiselectOptionsPipe
  ],
  templateUrl: './multiselect.component.html',
  styleUrl: './multiselect.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MULTISELECT_COMPONENT_TOKEN,
      useExisting: MultiselectComponent,
    },
  ],
})
export class MultiselectComponent<T>
  extends BaseFormInputComponent
  implements ControlValueAccessor, OnChanges, MultiselectCore<T>
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
  
  @Input() options: T[] | undefined = [];
  @Input() optionLabel: string | undefined;
  @Input() optionValue: string | undefined;
  @Input() dropdownIcon: string | undefined;
  @Input() floatLabel: string | undefined;
  @Input() editable = true;
  @Input() display!: 'flex' | 'block';
  @Input() errorSlot: SlotPositionType = SlotPosition.top;
  @Input() lazy!: boolean;
  @Input() filter!: boolean;
  @Input() loading = false;
  @Input() size: FieldSizeType = FieldSize.medium;

  @Output() changed = new EventEmitter<any>();
  @Output() showed = new EventEmitter<any>();
  @Output() cleared = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() filtered = new EventEmitter<FilterEvent>();

  icons = Icons;

  lazyLoadOptions: LazyLoadEvent = {
    limit: 50,
    offset: 0,
  };

  cdRef = inject(ChangeDetectorRef);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['options'] && !changes['options'].firstChange) {
      this.cdRef.detectChanges();
    }
  }

  onLazyLoad(event: any): void {
    const optionsLength = this.options?.length ?? 0;
    if (optionsLength - event.last <= 0) {
      this.lazyLoadOptions.offset = event.last;
      this.lazyLoad.emit(this.lazyLoadOptions);
    }
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