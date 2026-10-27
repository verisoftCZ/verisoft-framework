import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzDropdownButtonBasicExampleComponent,
  PrimengDropdownButtonBasicExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { DropdownButtonComponent } from '@verisoft/ui-primeng';" : "import { DropdownButtonComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic Usage')
          .addText(
            `
            The DropdownButton component is a versatile button that displays a dropdown menu when clicked. It combines the functionality of a button with a menu, allowing users to choose from multiple actions or options. The component supports various styles, sizes, and severities to match your application's design.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengDropdownButtonBasicExampleComponent : GovCzDropdownButtonBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Style')
          .addText(
            'Following is the list of structural style classes. Use these classes for theming.'
          )
          .addTable((p) =>
            p.addItems([
              {
                name: '.v-dropdown-button',
                description: 'The container of DropdownButton component.',
              },
              {
                name: '.v-dropdown-button__icon',
                description: 'The icon of DropdownButton component.',
              },
              {
                name: '.v-dropdown-button__label',
                description: 'The label of DropdownButton component.',
              },
              {
                name: '.v-dropdown-button__menu',
                description: 'The dropdown menu container.',
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
              description: 'The label of the dropdown button component.',
            },
            {
              name: 'icon',
              type: 'string | undefined',
              default: 'undefined',
              description: 'The icon of the dropdown button component.',
            },
            {
              name: 'rounded',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the dropdown button component should be rounded.',
            },
            {
              name: 'outlined',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the dropdown button component should be outlined.',
            },
            {
              name: 'raised',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the dropdown button component should be raised.',
            },
            {
              name: 'severity',
              type: '\'primary\' | \'secondary\' | \'success\' | \'info\' | \'warning\' | \'danger\' | \'help\' | \'contrast\'',
              default: '\'primary\'',
              description: 'The severity of the dropdown button component.',
            },
            {
              name: 'size',
              type: '\'small\' | \'medium\' | \'large\' | undefined',
              default: 'undefined',
              description: 'The size of the dropdown button component.',
            },
            {
              name: 'items',
              type: 'MenuItem[] | undefined',
              default: 'undefined',
              description: 'Array of menu items to display in the dropdown. Each item can have label, icon, and command properties.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Menu Item Interface').addTable((p) => p.withType().addItems([
        {
          name: 'id',
          type: 'string',
          default: 'undefined',
          description: 'Unique identifier for the menu item.',
        },
        {
          name: 'label',
          type: 'string | undefined',
          default: 'undefined',
          description: 'The label text of the menu item.',
        },
        {
          name: 'icon',
          type: 'string',
          default: 'undefined',
          description: 'The icon of the menu item.',
        },
        {
          name: 'command',
          type: 'Function',
          default: 'undefined',
          description: 'Callback function to execute when the menu item is clicked.',
        },
        {
          name: 'visible',
          type: 'boolean | Observable<boolean>',
          default: 'true',
          description: 'Controls the visibility of the menu item.',
        },
        {
          name: 'routerLink',
          type: 'any',
          default: 'undefined',
          description: 'Router link for navigation when the menu item is clicked.',
        },
      ])))
  );
}
