import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import {
  BaseFormInputComponent,
  ErrorPipe,
  FieldSize,
  FieldSizeType,
  RADIOBUTTON_COMPONENT_TOKEN,
  RadiobuttonCore,
  RadioButtonItem,
} from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { GovSizePipe } from '../../pipes';

@Component({
  selector: "v-radiobutton",
  standalone: true,
  styleUrls: [
    "./radiobutton.component.scss"
  ],
  templateUrl: './radiobutton.component.html',
  imports: [
    CommonModule, GovDesignSystemModule, RouterModule, ReactiveFormsModule, ErrorPipe, GovSizePipe
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: RADIOBUTTON_COMPONENT_TOKEN,
      useExisting: RadioButtonComponent,
    },
  ],
})

export class RadioButtonComponent<T>
  extends BaseFormInputComponent
  implements ControlValueAccessor, RadiobuttonCore<T>
{
  @Input() radioGroupName = Math.random().toString();
  @Input() items: RadioButtonItem<T>[] = [];
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() errorSlot: 'top' | 'bottom' = 'bottom';
  @Input() disabled = false;

  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
