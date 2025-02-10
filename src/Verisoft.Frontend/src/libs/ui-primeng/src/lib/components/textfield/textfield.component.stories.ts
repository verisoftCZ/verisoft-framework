import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { FieldSize, FieldType } from '@verisoft/ui-core';
import { TextfieldComponent } from './textfield.component';

const meta: Meta<TextfieldComponent> = {
  component: TextfieldComponent,
  title: 'TextfieldComponent',
  decorators: [
    moduleMetadata({
      imports: [ReactiveFormsModule],
    }),
  ],
};
export default meta;
type Story = StoryObj<TextfieldComponent>;
const formGroup = new FormGroup({
  formField: new FormControl(''),
});
export const Primary: Story = {
  render: (args: any) => ({
    props: {
      ...args,
      formGroup,
    },
    template: `
    <form [formGroup]="formGroup">
      <v-textfield
        formControlName="formField"
        [label]="label"
        [size]="size"
        [type]="type"
        [disabled]="disabled"
        [readonly]="readonly"
        [required]="required"
        [tooltip]="tooltip"
        [placeholder]="placeholder"
        [testId]="testId"
      ></v-textfield>
    </form>
    `,
  }),
  args: {
    label: 'Description',
    size: FieldSize.medium,
    type: FieldType.text,
    required: false,
    disabled: false,
    readonly: false,
    placeholder:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
    testId: 'testId',
    tooltip: 'This is a tooltip lorem ipsum',
  },
  argTypes: {
    size: {
      options: ['small', 'medium', 'large'],
      control: { type: 'radio' },
      description: 'Defines the size of the `TextfieldComponent`',
    },
    type: {
      options: ['text', 'number', 'password', 'search'],
      control: { type: 'radio' },
      description: 'Defines the type of the `TextfieldComponent`',
    },
    label: {
      description:
        'Label shown on the left side of the `TextfieldComponent` input',
    },
    tooltip: { description: 'Tooltip text of the `TextfieldComponent`' },
    required: {
      description: 'Defines if the `TextfieldComponent` is `required`',
    },
    disabled: {
      description: 'Defines if the `TextfieldComponent` is `disabled`',
    },
    readonly: {
      description: 'Defines if the `TextfieldComponent` is `readonly`',
    },
    testId: { description: 'Test ID of the `TextfieldComponent`' },
  },
  parameters: {
    controls: { expanded: true },
  },
};
