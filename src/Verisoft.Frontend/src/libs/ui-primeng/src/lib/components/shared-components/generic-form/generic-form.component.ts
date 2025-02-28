import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { ReactiveFormsModule, UntypedFormGroup } from '@angular/forms';
import { GenericFieldDefinition } from '@verisoft/ui-core';
import { GenericFieldComponent } from '../generic-field';
import {
  generateFormGroup,
  getColumnClass,
} from './generic-form.model';

@Component({
  selector: 'v-generic-form',
  standalone: true,
  imports: [GenericFieldComponent, ReactiveFormsModule],
  templateUrl: './generic-form.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GenericFormComponent implements OnChanges {
  @Input() formGroup?: UntypedFormGroup;

  @Input() fields?: GenericFieldDefinition[];

  @Input() columns?: number;

  formGroupComputed!: UntypedFormGroup;

  columnClass?: string;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fields'] || changes['formGroup']) {
      this.formGroupComputed = generateFormGroup(
        this.fields,
        this.formGroupComputed,
        this.formGroup,
        !!changes['formGroup']
      );
    }

    if (changes['columns']) {
      this.columnClass = getColumnClass(this.columns);
    }
  }
}
