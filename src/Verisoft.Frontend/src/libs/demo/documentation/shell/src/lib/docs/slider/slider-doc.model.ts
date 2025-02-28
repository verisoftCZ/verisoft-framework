import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengSliderBasicExampleComponent,
  PrimengSliderReactiveFormsExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { SliderModule } from '@verisoft/ui-primeng';" : "import { SliderModule } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Slider component allows users to select a value or range of values by dragging a handle along a track. It provides a user-friendly interface for selecting numeric values, with options for customizing the display and behavior of the slider.'
          )
          .addCodeComponent(PrimengSliderBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Reactive forms')
          .addText(
            'The Slider component supports reactive forms functionality. You can use bindings with directives such as formControl, formControlName, and formGroup, as well as control validation rules through the model. Additionally, this allows for dynamic form updates, improved form validation handling, and better integration with form state management.'
          )
          .addCodeComponent(PrimengSliderReactiveFormsExampleComponent)
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
                name: '.v-slider',
                description: 'The container of Slider component.',
              },
              {
                name: '.v-slider__track',
                description: 'The track of the Slider component.',
              },
              {
                name: '.v-slider__handle',
                description: 'The handle of the Slider component.',
              },
              {
                name: '.v-slider__range',
                description: 'The range selection area of the Slider component.',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'step',
              type: 'number',
              default: '1',
              description: 'The step size for the slider.',
            },
            {
              name: 'min',
              type: 'number',
              default: '0',
              description: 'The minimum value for the slider.',
            },
            {
              name: 'max',
              type: 'number',
              default: '100',
              description: 'The maximum value for the slider.',
            },
            {
              name: 'range',
              type: 'boolean',
              default: 'false',
              description: 'Whether the slider allows for range selection.',
            },
          ])
        )
      )
      .addApiSection((s) =>
        s.title('Events').addTable((p) =>
          p.addItems([
            {
              name: 'onChange',
              description: 'Emitted when the value changes.',
            },
            {
              name: 'onSlideEnd',
              description: 'Emitted when the user stops sliding.',
            },
          ])
        )
      )
  );
}