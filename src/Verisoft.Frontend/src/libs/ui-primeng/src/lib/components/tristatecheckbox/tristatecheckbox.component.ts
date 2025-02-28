import { CommonModule } from '@angular/common';
import { Component, Optional, Self } from '@angular/core';
import {
  ControlValueAccessor,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BaseFormInputComponent,
  BaseInputControls,
  TRISTATE_CHECKBOX_COMPONENT_TOKEN,
  TristateCheckboxCore,
} from '@verisoft/ui-core';
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';
import { FormFieldComponent } from '../form-field';
import { Icons } from '../../icons';

@Component({
  selector: 'v-tristatecheckbox',
  standalone: true,
  imports: [
    CommonModule,
    TriStateCheckboxModule,
    ReactiveFormsModule,
    FormFieldComponent,
  ],
  templateUrl: './tristatecheckbox.component.html',
  styleUrl: './tristatecheckbox.component.scss',
  providers: [
    {
      provide: BaseInputControls,
      useExisting: TristatecheckboxComponent,
    },
    {
      provide: TRISTATE_CHECKBOX_COMPONENT_TOKEN,
      useExisting: TristatecheckboxComponent,
    },
  ],
})
export class TristatecheckboxComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, TristateCheckboxCore
{
  icons = Icons;
  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
