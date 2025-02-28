import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzSideMenuBasicExampleComponent,
  PrimengSideMenuBasicExampleComponent} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { SideMenuComponent } from '@verisoft/ui-primeng';" : "import { SideMenuComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Side menu angular component sports multiple services, which for example handle screen sizes and tell the component when to switch to a mobile layout. Furthermore, all the menu items are rendered in a tree-style layout.'
          )
          .addCodeComponent(isPrimeng ? PrimengSideMenuBasicExampleComponent : GovCzSideMenuBasicExampleComponent)
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
                name: '.v-side-menu',
                description: 'The parent of the Side menu\'s container.',
              },
              {
                name: '.v-side-menu-container',
                description: 'The container of the Side menu component.',
              },
              {
                name: '.v-side-menu-container__mobile',
                description: 'If in a mobile layout, the container of the Side menu component.',
              },
              {
                name: '.v-side-menu-body',
                description: 'The body of the Side menu\'s container.',
              },
              {
                name: '.v-side-menu-body__mobile',
                description: 'If in a mobile layout, the body of the Side menu\'s container.',
              },
              {
                name: '.user-info-container',
                description: 'If in a mobile layout, the container containing user informations.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'item',
              type: 'MenuItem[]',
              default: '[]',
              description: 'The items to render in a tree-style fashion inside of the Side menu.',
            },
            {
              name: 'logoUrl',
              type: 'string',
              default: '',
              description: 'The path to a picture which should serve as a logo for the Side menu.',
            },
            {
              name: 'userName',
              type: 'string',
              default: 'undefined',
              description: 'If in a mobile layout, serves as an alternative place to show the current user\'s name.',
            },
            {
              name: 'userRole',
              type: 'string',
              default: 'undefined',
              description: 'If in a mobile layout, serves as an alternative place to show the current user\'s role.',
            },
          ])
        )
      )
      .addApiSection((s) => 
        s.title('Events').addTable((p) => 
          p.addItems([
            {
              name: 'minimalized',
              type: 'EventEmitter<boolean>',
              default: 'undefined',
              description: 'Emits when the Side menu is minimalized or maximalized back.',
            },
            {
              name: 'itemSelected',
              type: 'EventEmitter<MenuItem>',
              default: 'undefined',
              description: 'Emits the selected (clicked) MenuItem from the Side menu.',
            },
          ])
        )
      )
  );
}
