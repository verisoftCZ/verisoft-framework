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
  RADIOBUTTON_COMPONENT_TOKEN,
  RadiobuttonCore,
  RadioButtonItem,
} from '@verisoft/ui-core';

@Component({
  selector: "v-radiobutton",
  standalone: true,
  styleUrls: [
    "./radiobutton.component.scss"
  ],
  templateUrl: './radiobutton.component.html',
  imports: [
    CommonModule, GovDesignSystemModule, RouterModule, ReactiveFormsModule, ErrorPipe
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

  @Input() size: 'xs' | 's' | 'm' | 'l' | 'xl' = 'm';

  @Input() errorSlot: 'top' | 'bottom' = 'top';

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
