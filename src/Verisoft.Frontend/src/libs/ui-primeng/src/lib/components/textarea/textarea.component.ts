
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
import { BaseFormInputComponent, BaseInputControls, TEXTAREA_COMPONENT_TOKEN, TextareaCore } from '@verisoft/ui-core';
import { FloatLabelModule } from 'primeng/floatlabel';
import { Textarea } from 'primeng/textarea';
import { ErrorComponent } from '../errors';
import { FormFieldComponent } from '../form-field/form-field.component';

@Component({
    selector: 'v-textarea',
    imports: [
    Textarea,
    ReactiveFormsModule,
    FormFieldComponent,
    FloatLabelModule,
    ErrorComponent
],
    templateUrl: './textarea.component.html',
    styleUrl: './textarea.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: BaseInputControls,
            useExisting: TextareaComponent,
        },
        {
            provide: TEXTAREA_COMPONENT_TOKEN,
            useExisting: TextareaComponent,
        },
    ]
})
export class TextareaComponent
  extends BaseFormInputComponent
  implements ControlValueAccessor, TextareaCore
{
  @Input()
  rows = 5;

  @Input()
  cols = 30;

  @Input()
  autoResize = false;

  @Input()
  floatLabel: string | undefined;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
