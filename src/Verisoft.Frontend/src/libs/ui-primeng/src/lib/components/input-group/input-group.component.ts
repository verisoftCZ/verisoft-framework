import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  Optional,
  Self,
} from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BaseFormInputComponent,
  BaseInputControls,
  INPUT_GROUP_COMPONENT_TOKEN,
  InputGroupCore,
  InputGroupItem,
} from '@verisoft/ui-core';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { FormFieldComponent } from '../form-field';

@Component({
  selector: 'v-input-group',
  standalone: true,
  imports: [
    CommonModule,
    InputGroupAddonModule,
    InputGroupModule,
    InputTextModule,
    ReactiveFormsModule,
    FormFieldComponent,
  ],
  templateUrl: './input-group.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: BaseInputControls,
      useExisting: forwardRef(() => InputGroupComponent),
    },
    {
      provide: INPUT_GROUP_COMPONENT_TOKEN,
      useExisting: InputGroupComponent,
    },
  ],
})
export class InputGroupComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, InputGroupCore
{
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }

  @Input() items!: InputGroupItem[];
}
