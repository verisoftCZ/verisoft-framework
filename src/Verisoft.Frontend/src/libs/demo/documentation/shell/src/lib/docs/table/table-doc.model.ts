import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzTableActionButtonsExampleComponent,
  GovCzTableBasicExampleComponent,
  GovCzTableBuilderExampleComponent,
  GovCzTableCellTemplateExampleComponent,
  GovCzTableDataSourceObservableExampleComponent,
  GovCzTableDataSourceUrlExampleComponent,
  GovCzTableLoadingExampleComponent,
  GovCzTableRowDetailExampleComponent,
  GovCzTableRowDoubleClickExampleComponent,
  GovCzTableSelectableExampleComponent,
  GovCzTableSortExampleComponent,
  GovCzTableDefaultSortExampleComponent,
  GovCzTableTemplateExampleComponent,
  PrimengTableActionButtonsExampleComponent,
  PrimengTableBasicExampleComponent,
  PrimengTableBuilderExampleComponent,
  PrimengTableCellTemplateExampleComponent,
  PrimengTableDataSourceObservableExampleComponent,
  PrimengTableDataSourceUrlExampleComponent,
  PrimengTableLoadingExampleComponent,
  PrimengTableRowDetailExampleComponent,
  PrimengTableRowDoubleClickExampleComponent,
  PrimengTableSelectableExampleComponent,
  PrimengTableSortExampleComponent,
  PrimengTableDefaultSortExampleComponent,
  PrimengTableTemplateExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { TableComponent } from '@verisoft/ui-primeng';" : "import { TableComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic Usage')
          .addText(
            `
            The table is for displaying of tabular data. The table support sorting, pagination. The pagination is enabled by default. The table is also resized to parent's height.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableBasicExampleComponent : GovCzTableBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Template driven')
          .addText(
            `
            The table can be defined in view by column templates. 
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableTemplateExampleComponent : GovCzTableTemplateExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Table builder')
          .addText(
            `
            The TableBuilder class provides a fluent and flexible API for dynamically creating table column definitions in Angular applications. It allows developers to construct complex table configurations with ease, leveraging type safety and customization options.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableBuilderExampleComponent : GovCzTableBuilderExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Loading')
          .addText(
            `
            The table has also loading state.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableLoadingExampleComponent : GovCzTableLoadingExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Selectable')
          .addText(
            `
            The table can be selectable. There is possibility to set selectable only signle item or multiple items. Single item is selectable by row click, for multiple items checkbox are displayed.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableSelectableExampleComponent : GovCzTableSelectableExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Sort')
          .addText(
            `The table support single sorting or multiple sorting. To enable multiple sort set property sortMultiple to true.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableSortExampleComponent : GovCzTableSortExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Default sort')
          .addText(
            `A column can define a default sort state via the sortDirection ('asc' | 'desc') and sortPriority properties. It is applied on the initial render, reflected in the table state (sort icons) without any user interaction, and reflected in the data - client-side rows are sorted immediately and, for lazy/datasource-bound tables, the initial request already includes the default sort. sortPriority controls the order of columns when multiple sort defaults are combined in multi-sort mode; in single-sort mode only the column with the lowest sortPriority is applied. Whenever the columns input changes and the resulting default sort differs, the table re-applies it and emits the sortChange output. These properties can be defined either in the columns model passed to the columns input, or directly on the v-table-column directive in the template.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableDefaultSortExampleComponent : GovCzTableDefaultSortExampleComponent)
      )
      .addSection((s) =>
        s
          .title('DataSource')
          .addText(
            `The Table component is bindable to data source. Data source could be url, array, paged data, observable array, observable paged data, class inherited from BaseHttpService and AllItemDataSource. Datasource is available after importing of TableDataSourceDirective.`
          )
          .addSection((s1) =>
            s1
              .title('Observable')
              .addText('This example shows table binded to rxjs observable. Import TableDatasourceDirective to component, add useDatasource attribute and set datasource attribute.')
              .addCodeComponent(isPrimeng ? PrimengTableDataSourceObservableExampleComponent : GovCzTableDataSourceObservableExampleComponent)
          )
          .addSection((s1) =>
            s1
              .title('Url')
              .addText(
                'This example shows table binded to url datasource by url.'
              )
              .addCodeComponent(isPrimeng ? PrimengTableDataSourceUrlExampleComponent : GovCzTableDataSourceUrlExampleComponent)
          )
      )
      .addSection((s) =>
        s
          .title('Row Detail')
          .addText(
            `
            The also support row detail mode. To create a row detail add ng-template with name of variable #rowDetail. Implicit context is row.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableRowDetailExampleComponent : GovCzTableRowDetailExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Row double click')
          .addText(
            `
            By default a double click on a row navigates to the row detail (relative route with the row id or entityKey). Bind the rowDoubleClick output to handle the double click yourself - the default navigation is then not performed. The output emits the row data.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableRowDoubleClickExampleComponent : GovCzTableRowDoubleClickExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Column cell template')
          .addText(
            `
            A column can define a custom cell template using ng-template. When a template is provided, it replaces the default raw value rendering.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableCellTemplateExampleComponent : GovCzTableCellTemplateExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Action buttons')
          .addText(
            `
            The action buttons can be simple added via action button group component.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengTableActionButtonsExampleComponent : GovCzTableActionButtonsExampleComponent)
      )
  );
}
