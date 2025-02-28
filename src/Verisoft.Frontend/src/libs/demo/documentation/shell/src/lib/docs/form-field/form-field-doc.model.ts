import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengFormFieldBasicExampleComponent,
  PrimengFormFieldReactiveFormsExampleComponent,
  GovCzFormFieldBasicExampleComponent,
  GovCzFormFieldReactiveFormsExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { FormfieldComponent } from '@verisoft/ui-primeng';" : "import { FormfieldComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Formfield angular component is a customizable wrapper field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
          )
          .addCodeComponent(isPrimeng ? PrimengFormFieldBasicExampleComponent : GovCzFormFieldBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Formfield component supports reactive forms functionality. Through the use of ngControl it allows to show an display various errors coming from the elements which are actually tied to a Formcontrol, providing users with real-time feedback on their input. Furthermore, it unifies the styling of the various input elements, making them appear consistent through-out the application.'
          )
          .addCodeComponent(isPrimeng ? PrimengFormFieldReactiveFormsExampleComponent : GovCzFormFieldReactiveFormsExampleComponent)
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
                name: '.v-form-field',
                description: 'The container of the Formfield component.',
              },
              {
                name: '.v-formfield-label',
                description: 'The label for the Formfield\'s content container.',
              },
              {
                name: '.v-tooltip',
                description: 'The tooltip for the Formfield\'s content container.',
              },
              {
                name: '.v-form-field-content',
                description: 'The container of the Formfield\'s content.',
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
              name: 'tooltip',
              type: 'string',
              default: '',
              description: 'Sets the tooltip for the wrapped element inside of Formfield',
            },
            {
              name: 'display',
              type: 'flex | block',
              default: 'flex',
              description: 'Diplays the field as a block or flex.',
            },
            {
              name: 'testId',
              type: 'string',
              default: 'null',
              description: 'The test id attribute.',
            },
            {
              name: 'ngControl',
              type: 'ngControl',
              default: 'NgControl',
              description: 'NgControl which is provided from a form component.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
