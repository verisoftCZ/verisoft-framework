import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzTextFieldBasicExampleComponent,
  GovCzTextFieldReactFormsExampleComponent,
  GovCzTextFieldSizeExampleComponent,
  PrimengTextFieldBasicExampleComponent,
  PrimengTextFieldReactFormsExampleComponent,
  PrimengTextFieldSizeExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { VerisoftFormFieldComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The TextField angular component is a customizable text input field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. It includes built-in validation messages, providing users with real-time feedback on their input. The text field also supports adjustable sizes, allowing developers to fine-tune its appearance to match the design requirements. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
          )
          .addCodeComponent(isPrimeng ? PrimengTextFieldBasicExampleComponent : GovCzTextFieldBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The TextField component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengTextFieldReactFormsExampleComponent : GovCzTextFieldReactFormsExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Size')
          .addText(
            'The textfield component allows you to set different sizes by specifying the size attribute. You can customize the size of the textfield to be small, medium, or large by setting the size property accordingly.'
          )
          .addCodeComponent(isPrimeng ? PrimengTextFieldSizeExampleComponent : GovCzTextFieldSizeExampleComponent)
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
                name: '.v-text-field',
                description: 'The container of TextField component.',
              },
              {
                name: '.v-text-field__icon',
                description: 'The icon of TextField component.',
              },
              {
                name: '.v-text-field__label',
                description: 'The label of TextField component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'floatLabel',
              type: 'boolean',
              default: 'false',
              description: 'If input should have an floating label.',
            },
            {
              name: 'type',
              type: 'FieldType',
              default: 'FieldType.Text',
              description: 'The type of input component (text, password, number, search).',
            },
            {
              name: 'size',
              type: 'FieldSize',
              default: 'FieldSize.Medium',
              description: 'The size of component (small, medium, large).',
            },
            {
              name: 'label',
              type: 'string',
              default: 'null',
              description: 'The label of TextField component.',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that an input field must be filled out before submitting the form.',
            },
            {
              name: 'readonly',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the component cannot be edited.',
            },
            {
              name: 'tooltip',
              type: 'string',
              default: 'null' ,
              description: 'The text of the component\'s tooltip.',
            },
            {
              name: 'formDisplay',
              type: 'flex | block',
              default: 'flex',
              description: 'Diplays the field as a block or flex.',
            },
            {
              name: 'clearable',
              type: 'boolean',
              default: 'true',
              description: 'When enabled, a clear icon is displayed to clear the value.',
            },
            {
              name: 'placeholder',
              type: 'string',
              default: 'null',
              description: 'Default text to display when no text is filled.',
            },
            {
              name: 'testId',
              type: 'string',
              default: 'null',
              description: 'The test id attribute.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
