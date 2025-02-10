import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { BaseFormInputComponent } from '@verisoft/ui-core';
import { FormFieldComponent } from '../form-field';
import { RadioButtonComponent } from './radiobutton.component';

const meta: Meta<RadioButtonComponent<any>> = {
  component: RadioButtonComponent,
  title: 'RadioButtonComponent',
  decorators: [
    moduleMetadata({
      imports: [
        FormFieldComponent,
        ReactiveFormsModule,
        FormsModule,
        BaseFormInputComponent,
      ],
    }),
  ],
};
export default meta;
type Story = StoryObj<RadioButtonComponent<any>>;
const formGroup: FormGroup = new FormGroup({
  radio: new FormControl(null),
});
export const Primary: Story = {
  render: (args: any) => ({
    props: {
      ...args,
      formGroup,
    },
    template: `
    <form [formGroup]="formGroup">
      <v-radiobutton
      formControlName="radio"
      [radioGroupName]="radioGroupName"
      [items]="items"
      [testId]="testId"
      [label]="label"
      [required]="required"
      [tooltip]="tooltip"
      />
    </form>
    `,
  }),
  args: {
    radioGroupName: 'test',
    items: [
      { id: 'true', value: 'Yes' },
      { id: 'false', value: 'No' },
    ],
    label: 'Save item?',
    required: true,
    tooltip: 'This is a tooltip lorem ipsum',
    testId: 'radioButtonTestId',
  },
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByText('Save item?')).toBeTruthy();
    expect(canvas.getByText('Yes')).toBeTruthy();
    expect(canvas.getByText('No')).toBeTruthy();
    expect(canvas.queryAllByText('This is a tooltip lorem ipsum')).toBeTruthy();
    expect(canvas.findByTestId('radioButtonTestId')).toBeTruthy();
  },
};
