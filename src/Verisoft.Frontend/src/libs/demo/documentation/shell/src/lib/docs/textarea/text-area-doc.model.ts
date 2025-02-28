import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengTextAreaBasicExampleComponent,
  PrimengTextAreaReactFormsExampleComponent,
  PrimengTextAreaSizeExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { TextAreaComponent } from '@verisoft/ui-primeng';" : "import { TextAreaComponent } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The TextArea angular component is a customizable text area field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability. It includes built-in validation messages, providing users with real-time feedback on their input. The text field also supports adjustable sizes (if allowed by input settings), allowing developers to fine-tune its appearance to match the design requirements. Additionally, it comes with support for testing attributes, making it easier to integrate into automated testing frameworks.'
          )
          .addCodeComponent(PrimengTextAreaBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The TextArea component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(PrimengTextAreaReactFormsExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Size')
          .addText(
            'The TextArea component allows you to set different sizes by specifying the <strong>rows</strong> and <strong>cols</strong> attribute. You can customize these settings with a number value.'
          )
          .addCodeComponent(PrimengTextAreaSizeExampleComponent)
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
                name: '.v-textarea',
                description: 'The container of TextArea component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'floatLabel',
              type: 'string | undefined',
              default: 'undefined',
              description: 'If the textarea should have a floating label and show its value.',
            },
            {
              name: 'rows',
              type: 'number',
              default: '5',
              description: 'The ammount of rows the elements takes up vertically.',
            },
            {
              name: 'cols',
              type: 'number',
              default: '30',
              description: 'The ammount of columns the elements takes up horizontally.',
            },
            {
              name: 'autoResize',
              type: 'boolean',
              default: 'false',
              description: 'The ammount of rows the elements takes up.',
            },
            {
              name: 'label',
              type: 'string',
              default: 'null',
              description: 'The label of TextArea component.',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that an input field must be filled out before submitting the form.',
            },
            {
              name: 'readonly',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the component cannot be edited.',
            },
            {
              name: 'tooltip',
              type: 'string',
              default: 'null' ,
              description: 'The text of the component\'s tooltip.',
            },
            {
              name: 'disabled',
              type: 'boolean',
              default: 'null',
              description: 'When present, it specifies that the component should be disabled.',
            },
            {
              name: 'formDisplay',
              type: 'flex | block',
              default: 'flex',
              description: 'Diplays the field as a block or flex.',
            },
            {
              name: 'placeholder',
              type: 'string',
              default: 'null',
              description: 'Default text to display when no text is filled.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
