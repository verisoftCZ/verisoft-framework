import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengCalendarBasicExampleComponent,
  PrimengFormFieldReactiveFormsExampleComponent,
  PrimengFormFieldExtraExampleComponent,
  GovCzCalendarBasicExampleComponent,
  GovCzFormFieldExtraExampleComponent,
  GovCzFormFieldReactiveFormsExampleComponent
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { CalendarComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Calendar angular component is a customizable wrapper field designed for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability.'
          )
          .addCodeComponent(isPrimeng ? PrimengCalendarBasicExampleComponent : GovCzCalendarBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Calendar component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(isPrimeng ? PrimengFormFieldReactiveFormsExampleComponent : GovCzFormFieldReactiveFormsExampleComponent)
      )
      .addSection((s) => 
        s
          .title('Calendar input modes')
          .addText('The Calendar component allows to use multiple input modes based on one\'s specified use case.')
          .addCodeComponent(isPrimeng ? PrimengFormFieldExtraExampleComponent : GovCzFormFieldExtraExampleComponent)
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
                name: '.v-calendar',
                description: 'The container of the Calendar component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'selectionMode',
              type: 'single | multiple | range | undefined',
              default: 'single',
              description: 'Sets the calendar\'s input mode.',
            },
            {
              name: 'label',
              type: 'string',
              default: 'null',
              description: 'The label of the Calendar component. Either this or the float label must be set.',
            },
            {
              name: 'required',
              type: 'boolean',
              default: 'false',
              description: 'When present, it specifies that the input field must be filled out before submitting the form.',
            },
            {
              name: 'inputId',
              type: 'string',
              default: 'random',
              description: 'The element\'s ID attribute.',
            },
            {
              name: 'tooltip',
              type: 'string',
              default: 'undefined',
              description: 'If present, shows the element\'s tooltip on hover.',
            },
            {
              name: 'formDisplay',
              type: 'flex | block',
              default: 'flex',
              description: 'Diplays the field as a block or flex.',
            },
            {
              name: 'icon',
              type: 'string',
              default: 'pi pi-calendar',
              description: 'Sets the input\'s icon inside of the input box.',
            },
            {
              name: 'maxDate',
              type: 'Date',
              default: 'undefined',
              description: 'Sets the input\'s maximum selectable date.',
            },
            {
              name: 'floatLabel',
              type: 'boolean',
              default: 'false',
              description: 'If input should have an floating label.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
