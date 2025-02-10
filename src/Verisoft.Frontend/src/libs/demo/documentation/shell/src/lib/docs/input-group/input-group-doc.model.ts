import {
  DocumentationBuilderService,
  DocumentationItem,
  DOCS_CODE_LANG,
} from '@verisoft/demo-documentation-ui';
import { PrimengInputGroupBasicExampleComponent, GovCzInputGroupBasicExampleComponent } from './examples';
export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Overview')
          .addText(
            'The InputGroup component enhances input fields by allowing the addition of icons or text elements on either side of the input. It is built on top of PrimeNG\'s InputGroup and provides seamless integration with Angular forms.'
          )
      )
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { InputGroupComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic Usage')
          .addText(
            'Use the InputGroup component to add icons or text addons to your input fields. The component supports both left and right positioned addons.'
          )
          .addCodeComponent(isPrimeng ? PrimengInputGroupBasicExampleComponent : GovCzInputGroupBasicExampleComponent)
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'items',
              type: 'InputGroupItem[]',
              default: '[]',
              description: 'Array of addon items to display around the input',
            },
            {
              name: 'label',
              type: 'string',
              default: 'undefined',
              description: 'Label text for the input field',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description: 'Whether the input field is required',
            },
            {
              name: 'tooltip',
              type: 'string',
              default: 'undefined',
              description: 'Tooltip text to display on hover',
            },
            {
              name: 'testId',
              type: 'string',
              default: 'undefined',
              description: 'Data test id for testing purposes',
            },
          ])
        )
      )
      .addApiSection((s) =>
        s.title('Events').addTable((p) =>
          p.withType().addItems([
            {
              name: 'onChange',
              type: 'EventEmitter<any>',
              description: 'Emitted when the input value changes',
            },
            {
              name: 'onBlur',
              type: 'EventEmitter<void>',
              description: 'Emitted when the input loses focus',
            },
          ])
        )
      )
      .addSection((s) =>
        s
          .title('Interfaces')
          .addCode(
            `interface InputGroupItem {
              icon?: string;
              text?: string;
              position: IconPositionType;
            }`,
            DOCS_CODE_LANG.TS
          )
      )
      .addSection((s) =>
        s
          .title('Form Support')
          .addText(
            'The InputGroup component extends BaseFormInputComponent and supports:'
          )
      )
  );
}