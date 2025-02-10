import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  forwardRef,
  Input,
  OnChanges,
  OnDestroy,
  Optional,
  Output,
  Self,
  SimpleChanges,
  ViewEncapsulation,
  OnInit,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { FilterEvent, LazyLoadEvent } from '@verisoft/core';
import {
  BaseFormInputComponent,
  BaseInputControls,
  DROPDOWN_COMPONENT_TOKEN,
  DropdownCore
} from '@verisoft/ui-core';
import { ScrollerOptions } from 'primeng/api';
import {
  DropdownFilterEvent,
  DropdownModule,
} from 'primeng/dropdown';
import { FloatLabelModule } from 'primeng/floatlabel';
import { MessageModule } from 'primeng/message';
import { ScrollerLazyLoadEvent } from 'primeng/scroller';
import { TooltipModule } from 'primeng/tooltip';
import { Subject } from 'rxjs';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-dropdown',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DropdownModule,
    TooltipModule,
    FormFieldComponent,
    MessageModule,
    FloatLabelModule,
  ],
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => DropdownComponent),
      multi: true,
    },
    {
      provide: DROPDOWN_COMPONENT_TOKEN,
      useExisting: DropdownComponent,
    },
  ],
})
export class DropdownComponent<T>
  extends BaseFormInputComponent
  implements
    ControlValueAccessor,
    OnChanges,
    OnDestroy,
    OnInit,
    DropdownCore<T>
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() options: T[] | undefined = [];
  @Input() optionLabel: string | undefined;
  @Input() optionValue: string | undefined;
  @Input() dropdownIcon?: string;
  @Input() floatLabel?: string;
  @Input() editable = false;
  @Input() loading = false;
  @Input() lazy = false;
  @Input() filter = true;

  @Output() showed = new EventEmitter<any>();
  @Output() cleared = new EventEmitter<any>();
  @Output() lazyLoad = new EventEmitter<LazyLoadEvent>();
  @Output() filtered = new EventEmitter<FilterEvent>();

  private destroy$ = new Subject<void>();

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

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onLazyLoad(event: ScrollerLazyLoadEvent): void {
    this.lazyLoad.emit({
      offset: event.first,
      limit: event.last - event.first,
    });
  }

  filterChange(event: DropdownFilterEvent): void {
    this.filtered.emit({
      filter: event.filter,
    });
  }

  onDropdownShow(): void {
    this.showed.emit();
  }

  onDropdownClear(): void {
    this.cleared.emit();
  }
}
