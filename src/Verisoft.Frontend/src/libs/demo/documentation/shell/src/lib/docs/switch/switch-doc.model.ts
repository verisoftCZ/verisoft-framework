import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzSwitchBasicExampleComponent,
  GovCzSwitchReactFormsExampleComponent,
  PrimengSwitchBasicExampleComponent,
  PrimengSwitchReactFormsExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(
            isPrimeng ? "import { SwitchComponent } from '@verisoft/ui-primeng';" : "import { SwitchComponent } from '@verisoft/ui-govcz';"
          )
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The TextArea angular component is a customizable text area field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. It includes built-in validation messages, providing users with real-time feedback on their input. The text field also supports adjustable sizes (if allowed by input settings), allowing developers to fine-tune its appearance to match the design requirements. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
          )
          .addCodeComponent(isPrimeng ? PrimengSwitchBasicExampleComponent : GovCzSwitchBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The TextArea component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengSwitchReactFormsExampleComponent : GovCzSwitchReactFormsExampleComponent)
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
                name: '.v-switch',
                description: 'The container of Switch component.',
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
              description: 'The label of Switch component.',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that an input field must be filled out before submitting the form.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
