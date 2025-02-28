import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  DOCS_CODE_LANG,
  DocumentationCodeBuilder,
} from '@verisoft/demo-documentation-ui';
import { CalendarComponent } from '@verisoft/ui-govcz';

const template = `
<div class="row" [formGroup]="formGroup">
  <div class="col col-md-6">
    <v-calendar label="Calendar" formControlName="calendar"/>
  </div>
  <div class="col col-md-6">
    <v-calendar label="Required" formControlName="required" [required]="true"/>
  </div>
  <div class="col col-md-6">
    <v-calendar label="Disabled" formControlName="disabled" [disabled]="true"/>
  </div>
</div>
`;

const typescript = `
formGroup = new FormGroup({
    calendar: new FormControl<string | null>(null),
    required: new FormControl<string | null>(null, Validators.required),
    disabled: new FormControl<string | null>(null),
});

ngOnInit(): void {
  this.formGroup.get('disabled')?.disable();
}
`;

@Component({
  selector: 'v-doc-govcz-calendar-reactive-forms-example',
  imports: [ReactiveFormsModule, CalendarComponent],
  standalone: true,
  template: template,
})
export class GovCzFormFieldReactiveFormsExampleComponent implements OnInit {
  code = new DocumentationCodeBuilder()
    .addCode(template, DOCS_CODE_LANG.HTML)
    .addCode(typescript, DOCS_CODE_LANG.TS)
    .buildCode();

  formGroup = new FormGroup({
    calendar: new FormControl<string | null>(null),
    required: new FormControl<string | null>(null, Validators.required),
    disabled: new FormControl<string | null>(null),
  });

  ngOnInit(): void {
    this.formGroup.get('disabled')?.disable();
  }
}
