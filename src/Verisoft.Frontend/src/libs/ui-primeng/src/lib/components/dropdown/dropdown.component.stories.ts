import { UntypedFormControl, Validators } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { faker } from '@faker-js/faker';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { DropdownComponent } from './dropdown.component';

const meta: Meta<DropdownComponent<any>> = {
  component: DropdownComponent,
  title: 'DropdownComponent',
  decorators: [
    moduleMetadata({
      imports: [BrowserAnimationsModule],
    }),
  ],
};
export default meta;
type Story = StoryObj<DropdownComponent<any>>;
const options = Array(5)
  .fill(0)
  .map((x, i) => {
    return {
      id: i,
      name: faker.airline.airline().name,
    };
  });
export const Primary: Story = {
  parameters: {
    controls: { expanded: true },
  },
  args: {
    options: options,
    optionLabel: 'name',
    optionValue: 'id',
  },
  argTypes: {
    optionLabel: {
      description:
        'String value that sets matching `T[property]` as a label value, where `property = optionLabel`',
    },
    optionValue: {
      description:
        'String value that sets matching `T[property]` as a binding value, where `property = optionValue`',
    },
    options: { description: 'Array of `T`' },
  },
};

export const DropdownWithFormControl: Story = {
  parameters: {
    controls: { expanded: true },
  },
  args: {
    options: options,
    optionLabel: 'name',
    optionValue: 'id',
    label: 'Airline names',
    placeholder: 'Select an option',
    clearable: true,
    formControl: new UntypedFormControl(undefined, Validators.required),
  },
  argTypes: {
    formControl: {
      control: false,
      description: 'Form control with `Validators.required` set',
    },
    clearable: {
      description: 'Defines if the `DropdownComponent` is clearable',
    },
    label: {
      description:
        'Represents the `label` of the `DropdownComponent`. Label is also being used as `placeholder` if the placeholder is not set',
    },
    optionLabel: {
      description:
        'String value that sets matching `T[property]` as a label value, where `property = optionLabel`',
    },
    optionValue: {
      description:
        'String value that sets matching `T[property]` as a binding value, where `property = optionValue`',
    },
    placeholder: { description: 'Placeholder for the `DropdownComponent`' },
    options: { description: 'Array of `T`' },
  },
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByTitle('Airline names')).toBeTruthy();
    expect(canvas.findByPlaceholderText('Select an option')).toBeTruthy();
  },
};
