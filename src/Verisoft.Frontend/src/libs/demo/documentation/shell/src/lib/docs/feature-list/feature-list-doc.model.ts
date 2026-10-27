import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzFeatureListAutobindExampleComponent, GovCzFeatureListBasicExampleComponent, GovCzFeatureListServerExampleComponent, GovCzFeatureListTemplateExampleComponent, PrimengFeatureListAutobindExampleComponent, PrimengFeatureListBasicExampleComponent, PrimengFeatureListServerExampleComponent, PrimengFeatureListTemplateExampleComponent } from './examples';

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
          s.title('Server data source')
          .addText(
            `The datasource can also point directly to a backend endpoint by name - the FeatureList then fetches, filters and paginates the data itself. This example uses the demo API endpoint GET v2/User/list (see Verisoft.DemoApi.Host/Controllers/V2/UserController.cs), which returns a paged list (UserListData: data + total) mapped from UserListItem (firstName, surname, email). Note that this endpoint requires the DemoEdit or Admin role, so the demo user must be logged in for data to load.`
          )
          .addCodeComponent(isPrimeng ? PrimengFeatureListServerExampleComponent : GovCzFeatureListServerExampleComponent)
      )
      .addSection((s) =>
          s.title('Manual load (autoBind)')
          .addText(
            `The FeatureList (and the underlying v-table) supports [autoBind]="false" to skip the automatic data fetch on init - data is only loaded once you explicitly call reload() on the FeatureList (e.g. from a template reference, as in this example) or once the user interacts with paging/sorting/filtering.`
          )
          .addCodeComponent(isPrimeng ? PrimengFeatureListAutobindExampleComponent : GovCzFeatureListAutobindExampleComponent)
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
