import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzFeatureListBasicExampleComponent, GovCzFeatureListTemplateExampleComponent, PrimengFeatureListBasicExampleComponent, PrimengFeatureListTemplateExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(
            isPrimeng ? "import { FeatureListComponent } from '@verisoft/ui-primeng';" : "import { FeatureListComponent } from '@verisoft/ui-govcz';"
          )
      )
      .addSection((s) =>
          s.title('Basic')
          .addText(
            'The FeatureList angular component displays a data in filterable table with filter.'
          )
          .addCodeComponent(isPrimeng ? PrimengFeatureListBasicExampleComponent : GovCzFeatureListBasicExampleComponent)
      )
      .addSection((s) =>
          s.title('Template')
          .addText(
            'The Feature List component allows you to configure its behavior and display by specifying various attributes in the template. You can customize the table to include features such as filtering, adding, downloading, and deleting by setting the corresponding properties.'
          )
          .addCodeComponent(isPrimeng ? PrimengFeatureListTemplateExampleComponent : GovCzFeatureListTemplateExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Router')
          .addText(
            'The FeatureList list page can be defined only via routing without creating any component. Add method addFeatureListPage to Route definition.'
          ).addCode(`{ path: 'feature-list', component: .... },
addFeatureListPage({
  path: 'feature-list-page-example',
  config: {
    title: 'User List',
    datasource: 'v2/users',
    tableName: 'USER_LIST',
  },
}),
{ path: 'contracts', ...}`)
      )
  );
}
