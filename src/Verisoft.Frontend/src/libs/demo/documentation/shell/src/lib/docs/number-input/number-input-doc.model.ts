import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengNumberInputBasicExampleComponent,
  PrimengNumberInputReactFormsExampleComponent,
  PrimengNumberInputSizeExampleComponent,
  GovCzNumberInputBasicExampleComponent,
  GovCzNumberInputReactFormsExampleComponent,
  GovCzNumberInputSizeExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { NumberInputComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The NumberInput component allows users to input numeric values easily. It provides a user-friendly interface for entering numbers, with options for setting minimum and maximum values, as well as step increments for precise adjustments.'
          )
          .addCodeComponent(isPrimeng ? PrimengNumberInputBasicExampleComponent : GovCzNumberInputBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The NumberInput component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengNumberInputReactFormsExampleComponent : GovCzNumberInputReactFormsExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Size')
          .addText(
            'The NumberInput component allows you to set different sizes by specifying the size attribute. You can customize the size of the NumberInput to be small, medium, or large by setting the size property accordingly.'
          )
          .addCodeComponent(isPrimeng ? PrimengNumberInputSizeExampleComponent : GovCzNumberInputSizeExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Style')
          .addText(
            'Following is the list of structural style classes. Use this classes for theming.'
          )
          .addTable((p) =>
            p.addItems([
              {
                name: '.v-number-input',
                description: 'The container of NumberInput component.',
              },
              {
                name: '.v-number-input__icon',
                description: 'The icon of NumberInput component.',
              },
              {
                name: '.v-number-input__label',
                description: 'The label of NumberInput component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'mode',
              type: 'string',
              default: 'null',
              description: 'The mode of the number input component.',
            },
            {
              name: 'currency',
              type: 'string',
              default: 'null',
              description: 'The currency symbol for the number input.',
            },
            {
              name: 'min',
              type: 'number',
              default: 'null',
              description: 'The minimum value for the number input.',
            },
            {
              name: 'max',
              type: 'number',
              default: 'null',
              description: 'The maximum value for the number input.',
            },
            {
              name: 'step',
              type: 'number',
              default: '1',
              description: 'The step value for incrementing/decrementing the number.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
