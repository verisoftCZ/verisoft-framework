import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzFilterBasicExampleComponent, PrimengFilterBasicExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(
            isPrimeng
              ? "import { FilterComponent } from '@verisoft/ui-primeng';"
              : "import { FilterComponent } from '@verisoft/ui-govcz';"
          )
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The FeatureList angular component displays a data in filterable table with filter.'
          )
          .addCodeComponent(isPrimeng ? PrimengFilterBasicExampleComponent: GovCzFilterBasicExampleComponent)
      )
  );
}
