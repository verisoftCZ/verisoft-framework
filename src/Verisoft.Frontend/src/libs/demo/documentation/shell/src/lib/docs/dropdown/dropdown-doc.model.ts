import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzDropdownBasicExampleComponent,
  GovCzDropdownDatasourceExampleComponent,
  GovCzDropdownDatasourceUrlExampleComponent,
  GovCzDropdownReactiveFormsExampleComponent,
  PrimengDropdownBasicExampleComponent,
  PrimengDropdownDatasourceExampleComponent,
  PrimengDropdownDatasourceUrlExampleComponent,
  PrimengDropdownReactiveFormsExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { DropdownComponent } from '@verisoft/ui-primeng';" : "import { DropdownComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Dropdown angular component is a customizable text input field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. It includes built-in validation messages, providing users with real-time feedback on their input. The text field also supports adjustable sizes, allowing developers to fine-tune its appearance to match the design requirements. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
          )
          .addCodeComponent(isPrimeng ? PrimengDropdownBasicExampleComponent : GovCzDropdownBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Dropdown component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengDropdownReactiveFormsExampleComponent : GovCzDropdownReactiveFormsExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Data source')
          .addText(
            'The Dropdown component is bindable to data source. Data source should be url, Observable and AllItemDataSource. Datasource is available after importing of DataSourceDirective'
          )
          .addSection((s1) =>
            s1
              .title('Observable')
              .addText('This example shows dropdown binded to rxjs observable. Import DatasourceDirective to component, add useDatasource attribute and set datasource attribute.')
              .addCodeComponent(isPrimeng ? PrimengDropdownDatasourceExampleComponent : GovCzDropdownDatasourceExampleComponent)
          )
          .addSection((s1) =>
            s1
              .title('Url')
              .addText(
                'This example shows dropdown binded to url datasource by url.'
              )
              .addCodeComponent(isPrimeng ? PrimengDropdownDatasourceUrlExampleComponent : GovCzDropdownDatasourceUrlExampleComponent)
          )
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
                name: '.v-dropdown',
                description: 'The container of the Dropdown component.',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'label',
              type: 'string',
              default: 'null',
              description:
                'The label of the Calendar component. Either this or the float label must be set.',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description:
                'When present, it specifies that the input field must be filled out before submitting the form.',
            },
            {
              name: 'inputId',
              type: 'string',
              default: 'random',
              description: "The element's ID attribute.",
            },
            {
              name: 'options',
              type: 'array<T>',
              default: '[]',
              description: 'All the selectable options inside of the dropdown.',
            },
            {
              name: 'optionLabel',
              type: 'string',
              default: 'undefined',
              description:
                'String value that sets matching `T[property]` as a label value, where `property = optionLabel`',
            },
            {
              name: 'optionValue',
              type: 'string',
              default: 'undefined',
              description:
                'String value that sets matching `T[property]` as a binding value, where `property = optionValue`',
            },
            {
              name: 'editable',
              type: 'boolean',
              default: 'true',
              description:
                'If true, allows the user to type into the dropdown in order to search the options.',
            },
            {
              name: 'tooltip',
              type: 'string',
              default: 'undefined',
              description: "If present, shows the element's tooltip on hover.",
            },
            {
              name: 'formDisplay',
              type: 'flex | block',
              default: 'flex',
              description: 'Diplays the field as a block or flex.',
            },
            {
              name: 'floatLabel',
              type: 'boolean',
              default: 'false',
              description: 'If input should have an floating label.',
            },
            {
              name: 'testId',
              type: 'string',
              default: 'null',
              description: 'The test id attribute.',
            },
          ])
        )
      )
      .addApiSection((s) =>
        s.title('Events').addTable((p) =>
          p.addItems([
            {
              name: 'changeEvent',
              type: 'EventEmitter<any>',
              default: 'null',
              description: 'Emits when a value is chosen in the dropdown.',
            },
            {
              name: 'showEvent',
              type: 'EventEmitter<any>',
              default: 'null',
              description: 'Emits when the dropdown is opened.',
            },
            {
              name: 'clearEvent',
              type: 'EventEmitter<any>',
              default: 'null',
              description:
                'Emits when the chosen value is cleared via the clear button.',
            },
            {
              name: 'lazyLoadEvent',
              type: 'EventEmitter<LazyLoad>',
              default: 'null',
              description: 'Emits when the element used Lazy load.',
            },
          ])
        )
      )
  );
}
