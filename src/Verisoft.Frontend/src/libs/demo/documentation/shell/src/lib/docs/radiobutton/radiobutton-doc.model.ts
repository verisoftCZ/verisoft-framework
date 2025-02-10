import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzRadiobuttonBasicExampleComponent,
  GovCzRadiobuttonReactiveFormsExampleComponent,
  PrimengRadiobuttonBasicExampleComponent,
  PrimengRadiobuttonReactiveFormsExampleComponent
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { RadiobuttonComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Radiobutton angular component is a customizable radio input field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. It includes built-in validation messages, providing users with real-time feedback on their input. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
          )
          .addCodeComponent(isPrimeng ? PrimengRadiobuttonBasicExampleComponent : GovCzRadiobuttonBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Radiobutton component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengRadiobuttonReactiveFormsExampleComponent : GovCzRadiobuttonReactiveFormsExampleComponent)
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
                name: '.v-radiobutton',
                description: 'The container of the Radiobutton component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'ngControl',
              type: 'ngControl',
              default: 'NgControl',
              description: 'NgControl which is provided from a form component.',
            },
            {
              name: 'items',
              type: 'RadioButtonItem<T>[]',
              default: '[]',
              description: 'Which group of radiobuttons appear in a selection.',
            },
            {
              name: 'radioGroupName',
              type: 'string',
              default: 'Math.random().toString()',
              description: 'The name of radiobutton group, by default is random.',
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
              name: 'tooltip',
              type: 'string',
              default: 'null' ,
              description: 'The text of the component\'s tooltip.',
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
