import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { NgControl, ReactiveFormsModule } from '@angular/forms';
import { ErrorPipe, FormFieldCore, WarningPipe } from '@verisoft/ui-core';
import { MessageModule } from 'primeng/message';
import { TooltipModule } from 'primeng/tooltip';
import { Icons } from '../../icons';

@Component({
  selector: 'v-form-field',
  standalone: true,
  imports: [
    CommonModule,
    ErrorPipe,
    WarningPipe,
    ReactiveFormsModule,
    MessageModule,
    TooltipModule,
  ],
  templateUrl: './form-field.component.html',
  styleUrl: './form-field.component.scss',
})
export class FormFieldComponent implements FormFieldCore {
  @Input() ngControl?: NgControl;

  @Input() label?: string;

  @Input() tooltip?: string;

  @Input() required = false;

  @Input() testId?: string;

  @Input() display: 'flex' | 'block' = 'flex';

  icons = Icons;
}
