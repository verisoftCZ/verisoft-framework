import { NgControl } from '@angular/forms';
import type { Meta, StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { FormFieldComponent } from './form-field.component';

const meta: Meta<FormFieldComponent> = {
  component: FormFieldComponent,
  title: 'FormFieldComponent',
};
export default meta;
type Story = StoryObj<FormFieldComponent>;

export const Primary: Story = {
  parameters: {
    controls: { expanded: true },
  },
  args: {
    label: 'Label showcase',
    tooltip: 'This is a tooltip lorem ipsum',
    required: false,
    testId: '123',
    display: 'flex',
    ngControl: {} as NgControl,
  },
  argTypes: {
    ngControl: {
      control: false,
      description: 'NgControl which is provided from a form component',
    },
    display: {
      options: ['flex', 'block'],
      control: {
        type: 'radio',
      },
      description:
        'Defines if the **FormFieldComponent** is in `flex` or `block` display mode',
    },
    required: {
      control: {
        type: 'boolean',
      },
      description: 'Defines if the **FormFieldComponent** is `required`',
    },
    testId: {
      control: {
        type: 'text',
      },
      description: 'Sets the **FormFieldComponent** `test id`',
    },
    label: {
      control: {
        type: 'text',
      },
      description: 'Sets the **FormFieldComponent** `label`',
    },
    tooltip: {
      control: {
        type: 'text',
      },
      description: 'Sets the **FormFieldComponent** `tooltip`',
    },
  },
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByText('Label showcase')).toBeTruthy();
    expect(canvas.queryAllByText('This is a tooltip lorem ipsum')).toBeTruthy();
    expect(canvas.findByTestId('123')).toBeTruthy();
  },
};
