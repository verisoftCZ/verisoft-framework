import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzCheckboxBasicExampleComponent,
  PrimengCheckboxBasicExampleComponent,
  PrimengCheckboxReactFormsExampleComponent,
  GovCzCheckboxReactFormsExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { CheckboxComponent } from '@verisoft/ui-primeng';" : "import { CheckboxComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        {
          return s
            .title('Basic')
            .addText(
              'The Checkbox angular component is a customizable checkbox field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. It includes built-in validation messages, providing users with real-time feedback on their input. The text field also supports adjustable sizes (if allowed by input settings), allowing developers to fine-tune its appearance to match the design requirements. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
            )
            .addCodeComponent(isPrimeng ? PrimengCheckboxBasicExampleComponent : GovCzCheckboxBasicExampleComponent);
        }
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Checkbox component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengCheckboxReactFormsExampleComponent : GovCzCheckboxReactFormsExampleComponent)
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
                name: '.v-checkbox',
                description: 'The container of the Checkbox component.',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'label',
              type: 'string',
              default: 'null',
              description: 'The label of TristateCheckbox component.',
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
              name: 'disabled',
              type: 'boolean',
              default: 'null',
              description: 'When present, it specifies that the component should be disabled.',
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
      .addApiSection(
        (s) => s.title('Events')
          .addTable(
            (p) => p.addItems([])
          )
      )
  );
}
