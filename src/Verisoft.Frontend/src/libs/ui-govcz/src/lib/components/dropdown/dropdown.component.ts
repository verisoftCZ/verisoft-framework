import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Optional,
  Output,
  Self,
  OnInit,
  OnDestroy,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { FilterEvent, LazyLoadEvent } from '@verisoft/core';
import {
  BaseFormInputComponent,
  ErrorPipe,
  FieldSize,
  FieldSizeType,
  SlotPositionType,
  SlotPosition,
  DropdownCore,
  DROPDOWN_COMPONENT_TOKEN,
} from '@verisoft/ui-core';
import { Subscription } from 'rxjs';
import { GovMultiselectOptionsPipe, GovSizePipe } from '../../pipes';
import { TooltipComponent } from '../tooltip';
import { Icons } from '../../icons';

@Component({
  selector: 'v-dropdown',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GovDesignSystemModule,
    ErrorPipe,
    GovSizePipe,
    GovMultiselectOptionsPipe,
    TooltipComponent,
  ],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: DROPDOWN_COMPONENT_TOKEN,
      useExisting: DropdownComponent,
    },
  ],
})
export class DropdownComponent<T>
  extends BaseFormInputComponent
  implements ControlValueAccessor, OnInit, OnDestroy, DropdownCore<T>
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() options: T[] | undefined = [];
  @Input() optionLabel: string | undefined;
  @Input() optionValue: string | undefined;
  @Input() optionValueType: 'string' | 'number' = 'string';
  @Input() addEmptyOption = false;
  @Input() dropdownIcon: string | undefined;
  @Input() floatLabel: string | undefined;
  @Input() editable = true;
  @Input() display!: 'flex' | 'block';
  @Input() errorSlot: SlotPositionType = SlotPosition.bottom;
  @Input() labelSlot: SlotPositionType = SlotPosition.top;
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

  private nullOptionValue = this.optionValueType === 'number' ? -1 : 'null';
  private sub!: Subscription;

  override ngOnInit() {
    super.ngOnInit();
    this.sub = this.formControl.valueChanges.subscribe((val) => {
      if (val === this.nullOptionValue) {
        this.formControl.setValue(null, { emitEvent: false });
      }
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  onLazyLoad(event: any): void {
    if ((this.options?.length ?? 0) - event.last <= 0) {
      this.lazyLoadOptions.offset = event.last;
      this.lazyLoad.emit(this.lazyLoadOptions);
    }
  }

  selectionChange(event: any): void {
    if (event.detail.value === this.nullOptionValue) {
      event.detail.value = null;
    }

    this.changed.emit(event);
  }

  onDropdownShow(): void {
    this.showed.emit();
  }

  onDropdownClear(): void {
    this.cleared.emit();
  }
}
