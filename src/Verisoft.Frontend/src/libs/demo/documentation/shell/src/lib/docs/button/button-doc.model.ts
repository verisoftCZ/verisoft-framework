import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengButtonBasicExampleComponent,
  PrimengButtonSizeExampleComponent,
  GovCzButtonBasicExampleComponent,
  GovCzButtonSizeExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { ButtonComponent } from '@verisoft/ui-primeng';" : "import { ButtonComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Button angular component is a versatile and customizable component that can be used to trigger actions in an Angular application. It supports various styles, sizes, and states, allowing developers to create buttons that match the design requirements of their application. The Button component can be easily integrated into forms, navigation menus, and other components, providing a consistent and cohesive user interface. It also includes support for testing attributes, making it easier to integrate into automated testing frameworks. The Button component is highly configurable and can be customized to meet the specific needs of the application, including support for custom icons, tooltips, and keyboard shortcuts. Overall, the Button component is a powerful and flexible tool that can be used to enhance the user experience and functionality of an Angular application.'
          )
          .addCodeComponent(isPrimeng ? PrimengButtonBasicExampleComponent : GovCzButtonBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Size')
          .addText(
            'The button component allows you to set different sizes by specifying the size attribute. You can customize the size of the textfield to be small, medium, or large by setting the size property accordingly.'
          )
          .addCodeComponent(isPrimeng ? PrimengButtonSizeExampleComponent : GovCzButtonSizeExampleComponent)
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
                name: '.v-button',
                description: 'The container of Button component.',
              },
              {
                name: '.v-button__icon',
                description: 'The icon of Button component.',
              },
              {
                name: '.v-text-field__label',
                description: 'The label of Button component.',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([

            {

              name: 'label',

              type: 'string | undefined',

              default: 'undefined',

              description: 'The label of the button component.',

            },

            {

              name: 'icon',

              type: 'string | undefined',

              default: 'undefined',

              description: 'The icon of the button component.',

            },

            {

              name: 'badge',

              type: 'string',

              default: 'N/A',

              description: 'The badge of the button component.',

            },

            {

              name: 'iconPos',

              type: '\'left\' | \'right\'',

              default: '\'left\'',

              description: 'The position of the icon in the button component.',

            },

            {

              name: 'disabled',

              type: 'boolean',

              default: 'false',

              description: 'When present, it specifies that the button component should be disabled.',

            },

            {

              name: 'rounded',

              type: 'boolean',

              default: 'false',

              description: 'When present, it specifies that the button component should be rounded.',

            },

            {

              name: 'text',

              type: 'boolean',

              default: 'false',

              description: 'When present, it specifies that the button component should be text-based.',

            },

            {

              name: 'outlined',

              type: 'boolean',

              default: 'false',

              description: 'When present, it specifies that the button component should be outlined.',

            },

            {

              name: 'raised',

              type: 'boolean',

              default: 'false',

              description: 'When present, it specifies that the button component should be raised.',

            },

            {

              name: 'severity',

              type: '\'secondary\' | \'success\' | \'info\' | \'warning\' | \'danger\' | \'help\'',

              default: '\'primary\'',

              description: 'The severity of the button component.',

            },

            {

              name: 'routerLink',

              type: 'any[]',

              default: 'N/A',

              description: 'The router link of the button component.',

            },

            {

              name: 'size',

              type: '\'small\' | \'large\' | undefined',

              default: 'undefined',

              description: 'The size of the button component.',

            },

            {

              name: 'queryParams',

              type: 'Params',

              default: 'N/A',

              description: 'The query parameters of the button component.',

            },

          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([
        {

          name: 'click',

          default: 'undefined',

          description: 'The method called after clicking.',

        },
      ])))
      .addApiSection((s) => s.title('Directives').addTable((p) => p.addItems([
        {

          name: 'CustomRouterLinkDirective',

          default: 'undefined',

          description: 'If button is clicked when holding CTRL, link is opened in new tab.'

        },
      ])))
  );
}
