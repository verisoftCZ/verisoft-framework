import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengStepperBasicExampleComponent,
  GovCzStepperBasicExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { StepperComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Stepper component is a versatile and customizable component that can be used to guide users through a series of steps in an Angular application. It supports various styles, sizes, and states, allowing developers to create steppers that match the design requirements of their application. The Stepper component can be easily integrated into forms, navigation menus, and other components, providing a consistent and cohesive user interface. It also includes support for testing attributes, making it easier to integrate into automated testing frameworks. The Stepper component is highly configurable and can be customized to meet the specific needs of the application, including support for custom icons, tooltips, and keyboard shortcuts. Overall, the Stepper component is a powerful and flexible tool that can be used to enhance the user experience and functionality of an Angular application.'
          )
          .addCodeComponent(isPrimeng ? PrimengStepperBasicExampleComponent : GovCzStepperBasicExampleComponent)
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
                name: '.v-stepper',
                description: 'The container of Stepper component.',
              },
              {
                name: '.v-stepper__icon',
                description: 'The icon of Stepper component.',
              },
              {
                name: '.v-stepper__label',
                description: 'The label of Stepper component.',
              },
              {
                name: '.v-stepper__panel',
                description: 'The panel of each step in the Stepper component.',
              },
              {
                name: '.v-stepper__content',
                description: 'The content area of each step in the Stepper component.',
              },
              {
                name: '.v-stepper__button',
                description: 'The button used for navigation in the Stepper component.',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {

              name: 'items',

              type: 'StepperItem[]',

              default: 'undefined',

              description: 'An array of StepperItem objects representing the steps in the stepper component.',

            }
          ])
        )
      )
      .addApiSection((s) => s.title('StepperItem').addTable((p) => p.addItems([
        {

          name: 'header',
          type: 'string',
          description: 'The header text to be displayed.',
        },

        {

          name: 'template',
          type: 'TemplateRef',
          description: 'A reference to an Angular template that defines the content.',

        },
      ])))
  );
}
