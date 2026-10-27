import { CommonModule } from '@angular/common';
import { Component, inject, Input, Optional, Self } from '@angular/core';
import { NgControl, ReactiveFormsModule } from '@angular/forms';
import { ERROR_PROVIDER_TOKEN } from '@verisoft/core';
import { BaseFormInputComponent, FormFieldCore, WarningPipe } from '@verisoft/ui-core';
import { MessageModule } from 'primeng/message';
import { TooltipModule } from 'primeng/tooltip';
import { Icons } from '../../icons';

@Component({
    selector: 'v-form-field',
    imports: [
        CommonModule,
        WarningPipe,
        ReactiveFormsModule,
        MessageModule,
        TooltipModule,
    ],
    templateUrl: './form-field.component.html',
    styleUrl: './form-field.component.scss'
})
export class FormFieldComponent
  extends BaseFormInputComponent
  implements FormFieldCore
{
  @Input() display: 'flex' | 'block' = 'block';
  
  @Input() variant!: 'simple';

  protected readonly errorProvider = inject(ERROR_PROVIDER_TOKEN)
  
  icons = Icons;

  constructor(@Optional() @Self() ngControl: NgControl) {
    super(ngControl);
  }
}
