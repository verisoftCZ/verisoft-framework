import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzHeaderBasicExampleComponent, PrimengHeaderBasicExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { HeaderComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Header component is a custom-made element used for providing information about the Company (its name) and some information about the logged in user. For the user, it shows his username, user role and an avatar assigned to the account. The logo is handled by the side menu and is not meant to be used inside of the header.'
          )
          .addCodeComponent(isPrimeng ? PrimengHeaderBasicExampleComponent : GovCzHeaderBasicExampleComponent)
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
                name: '.v-header',
                description: 'The container of the Header component.',
              },
              {
                name: '.v-header-title',
                description: 'The title of the Header component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'title',
              type: 'string',
              default: 'undefined',
              description: 'If set, shows the title of the Header element',
            },
            {
              name: 'userName',
              type: 'string',
              default: 'undefined',
              description: 'When present, shows the logged in user\'s username',
            },
            {
              name: 'userRole',
              type: 'string',
              default: 'undefined',
              description: 'When present, shows the logged in user\'s user role',
            },
            {
              name: 'menuRef',
              type: 'HTMLDivElement',
              default: 'undefined',
              description: 'The underlying main HTML element.',
            },
            {
              name: 'exampleHeader',
              type: 'boolean',
              default: 'false',
              description: 'If set true, forces the position of the header to be relative, meaning it can be used the way it is showcased in this example. Otherwise stays at the top of the page.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
