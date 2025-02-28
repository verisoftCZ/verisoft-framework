import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { TextareaComponent } from './textarea.component';

const meta: Meta<TextareaComponent> = {
  component: TextareaComponent,
  title: 'TextareaComponent',
  decorators: [
    moduleMetadata({
      imports: [ReactiveFormsModule],
    }),
  ],
};
export default meta;
type Story = StoryObj<TextareaComponent>;
const formGroup = new FormGroup({
  textarea: new FormControl(''),
});

export const Primary: Story = {
  render: (args: any) => ({
    props: {
      ...args,
      formGroup,
    },
    template: `
    <form [formGroup]="formGroup">
      <v-textarea
        formControlName="textarea"
        [label]="label"
        [rows]="rows"
        [cols]="cols"
        [disabled]="disabled"
        [readonly]="readonly"
        [required]="required"
        [tooltip]="tooltip"
        [placeholder]="placeholder"
        [testId]="testId"
        [formDisplay]="formDisplay"
        [autoResize]="autoResize"
      ></v-textarea>
    </form>
    `,
  }),
  args: {
    label: 'Description',
    rows: 5,
    cols: 30,
    formDisplay: 'block',
    autoResize: false,
    required: false,
    disabled: false,
    readonly: false,
    placeholder:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.',
    testId: 'testId',
    tooltip: 'This is a tooltip lorem ipsum',
  },
  argTypes: {
    formDisplay: {
      options: ['block', 'flex'],
      control: { type: 'radio' },
      description:
        'Defines if the **TextareaComponent** is in `flex` or `block` display mode',
    },
    label: {
      description:
        'Label shown on the (top)left side of the **TextareaComponent** input',
    },
    rows: {
      description: 'Number of `rows` of the **TextareaComponent** input',
    },
    cols: {
      description: 'Number of `columns` of the **TextareaComponent** input',
    },
    tooltip: { description: 'Tooltip text of the **TextareaComponent**' },
    required: {
      description: 'Defines if the **TextareaComponent** is `required`',
    },
    autoResize: {
      description:
        'Defines if the **TextareaComponent** should be auto-resized',
    },
    disabled: {
      description: 'Defines if the **TextareaComponent** is `disabled`',
    },
    readonly: {
      description: 'Defines if the **TextareaComponent** is `readonly`',
    },
    testId: { description: 'Test ID of the **TextareaComponent**' },
    placeholder: {
      description: 'Placeholder text for the **TextareaComponent**',
    },
  },
  parameters: {
    controls: { expanded: true },
  },
};
