import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzLoaderBasicExampleComponent,
  PrimengLoaderBasicExampleComponent
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { LoaderComponent } from '@verisoft/ui-primeng';" : "import { LoaderComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Loader is angular component displays a loading indicator. An infinite spin animation is displayed by default.'
          )
          .addCodeComponent(isPrimeng ? PrimengLoaderBasicExampleComponent : GovCzLoaderBasicExampleComponent)
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
                name: '.v-loader',
                description: 'The container of the Loader component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
