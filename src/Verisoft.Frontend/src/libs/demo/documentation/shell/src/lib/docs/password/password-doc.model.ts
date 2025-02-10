import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzPasswordBasicExampleComponent,
  GovCzPasswordExtraExampleComponent,
  GovCzPasswordReactiveFormsExampleComponent,
  PrimengPasswordBasicExampleComponent,
  PrimengPasswordReactiveFormsExampleComponent} from './examples';
import { PrimengPasswordExtraExampleComponent } from './examples/primeng/primeng-password-extra-example.component';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { PasswordComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Password angular component is a customizable password input field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability.'
          )
          .addCodeComponent(isPrimeng ? PrimengPasswordBasicExampleComponent : GovCzPasswordBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Password component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengPasswordReactiveFormsExampleComponent : GovCzPasswordReactiveFormsExampleComponent)
      )
      .addSection((s) => 
        s
          .title('Extra example')
          .addText('The Password component allows to have the password reveal button disabled and also to show the password\'s strength.')
          .addCodeComponent(isPrimeng ? PrimengPasswordExtraExampleComponent : GovCzPasswordExtraExampleComponent)
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
                name: '.v-password',
                description: 'The container of the Password component.',
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
              description: 'The label of the Password component.',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the input field must be filled out before submitting the form.',
            },
            {
              name: 'inputId',
              type: 'string',
              default: 'random',
              description: 'The element\'s ID attribute.',
            },
            {
              name: 'toggleMask',
              type: 'boolean',
              default: 'true',
              description: 'Button to reveal plaintext password.',
            },
            {
              name: 'feedback',
              type: 'boolean',
              default: 'false',
              description: 'Show the given password\'s strength.',
            },
            {
              name: 'placeholder',
              type: 'string',
              default: '',
              description: 'The input\'s placeholder value.',
            },
            {
              name: 'clearable',
              type: 'boolean',
              default: 'true',
              description: 'If the password can be deleted using the "X" button.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
