import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzBreadcrumbBasicExampleComponent,
  PrimengBreadcrumbBasicExampleComponent 
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { BreadcrumbComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Breadcrumb angular extension allows more control over the standard p-breadcrumb element. It allows to set custom paths or take the path from the URL by default and set the appropriate breadcrumb items. On top of that, it also allows to add path to the root of the project or any other path set by the developers.'
          )
          .addCodeComponent(isPrimeng ? PrimengBreadcrumbBasicExampleComponent: GovCzBreadcrumbBasicExampleComponent)
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
                name: '.v-breadcrumb',
                description: 'The container of the Breadcrumb component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'items',
              type: 'MenuItem[]',
              default: '[]',
              description: 'If set, overrides the path that the breadcrumb obtains through ActivatedRoute.',
            },
            {
              name: 'homeRoute',
              type: 'string',
              default: '"/"',
              description: 'Sets the home url, used only if useHomeRoute is set to true.',
            },
            {
              name: 'useHomeRoute',
              type: 'boolean',
              default: 'false',
              description: 'If set, adds home url alongside its icon before the rendered path in the breadcrumb.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
