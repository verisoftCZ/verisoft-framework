import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzMultiselectBasicExampleComponent,
  GovCzMultiSelectDatasourceExampleComponent,
  GovCzMultiSelectDatasourceUrlExampleComponent,
  GovCzMultiselectReactiveFormsExampleComponent,
  PrimengMultiselectBasicExampleComponent,
  PrimengMultiSelectDatasourceExampleComponent,
  PrimengMultiSelectDatasourceUrlExampleComponent,
  PrimengMultiselectReactiveFormsExampleComponent} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { MultiselectComponent } from '@verisoft/ui-primeng';" : "import { MultiselectComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Multiselect component allows users to select multiple options from a dropdown list. It provides a user-friendly interface for selecting multiple items, with options for customizing the display and behavior of the dropdown.'
          )
          .addCodeComponent(isPrimeng ? PrimengMultiselectBasicExampleComponent : GovCzMultiselectBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Multiselect component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengMultiselectReactiveFormsExampleComponent : GovCzMultiselectReactiveFormsExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Data source')
          .addText(
            'The Multiselect component is bindable to data source. Data source should be url, Observable and AllItemDataSource. Datasource is available after importing of DataSourceDirective'
          )
          .addSection((s1) =>
            s1
              .title('Observable')
              .addText('This example shows dropdown binded to rxjs observable. Import DatasourceDirective to component, add useDatasource attribute and set datasource attribute.')
              .addCodeComponent(isPrimeng ? PrimengMultiSelectDatasourceExampleComponent : GovCzMultiSelectDatasourceExampleComponent)
          )
          .addSection((s1) =>
            s1
              .title('Url')
              .addText(
                'This example shows dropdown binded to url datasource by url.'
              )
              .addCodeComponent(isPrimeng ? PrimengMultiSelectDatasourceUrlExampleComponent : GovCzMultiSelectDatasourceUrlExampleComponent)
          )
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
                name: '.v-multiselect',
                description: 'The container of Multiselect component.',
              },
              {
                name: '.v-multiselect__icon',
                description: 'The icon of Multiselect component.',
              },
              {
                name: '.v-multiselect__label',
                description: 'The label of Multiselect component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'options',
              type: 'T[]',
              default: '[]',
              description: 'The options available for selection.',
            },
            {
              name: 'optionLabel',
              type: 'string',
              default: 'undefined',
              description: 'The label to display for each option.',
            },
            {
              name: 'optionValue',
              type: 'string',
              default: 'undefined',
              description: 'The value to bind for each option.',
            },
            {
              name: 'dropdownIcon',
              type: 'string',
              default: 'undefined',
              description: 'The icon to display for the dropdown.',
            },
            {
              name: 'floatLabel',
              type: 'string',
              default: 'undefined',
              description: 'The floating label for the input.',
            },
            {
              name: 'editable',
              type: 'boolean',
              default: 'true',
              description: 'Whether the input is editable.',
            },
          ])
        )
      )
      .addApiSection((s) => 
        s.title('Events').addTable((p) =>
          p.addItems([
            {
              name: 'changeEvent',
              description: 'Emitted when the selection changes.',
            },
            {
              name: 'showEvent',
              description: 'Emitted when the dropdown is shown.',
            },
            {
              name: 'clearEvent',
              description: 'Emitted when the selection is cleared.',
            },
            {
              name: 'lazyLoadEvent',
              description: 'Emitted when more options need to be loaded.',
            },
          ])
        )
      )
  );
}