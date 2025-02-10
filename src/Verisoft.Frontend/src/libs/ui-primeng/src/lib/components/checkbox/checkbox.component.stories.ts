import {
  ReactiveFormsModule,
  UntypedFormControl,
  Validators,
} from '@angular/forms';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { CheckboxComponent } from './checkbox.component';

const meta: Meta<CheckboxComponent> = {
  component: CheckboxComponent,
  title: 'CheckboxComponent',
  decorators: [
    moduleMetadata({
      imports: [ReactiveFormsModule],
    }),
  ],
};
export default meta;
type StoryOb = StoryObj<CheckboxComponent>;

export const Primary: StoryOb = {
  parameters: {
    controls: { expanded: true },
  },
  args: {
    label: 'Test checkbox',
    disabled: false,
    readonly: false,
    required: false,
  },
  argTypes: {
    label: {
      description:
        'Label shown on the right side of the `VerisoftCheckbox` input',
    },
    required: {
      description: 'Can be set via formControl `Validators.requiredTrue`',
      control: { disable: true },
    },
    disabled: {
      description: 'Sets `disabled` state',
    },
    readonly: {
      description: 'Sets `readonly` state',
    },
  },
};

export const CheckboxRequired: StoryOb = {
  parameters: {
    controls: { expanded: true },
  },
  args: {
    label: 'Agree to terms and conditions',
    formControl: new UntypedFormControl(false, Validators.requiredTrue),
    disabled: false,
    readonly: false,
  },
  argTypes: {
    label: {
      control: 'text',
      description:
        'Represents the `label` of the `CheckboxComponent`. Label is also being used as `placeholder` if the placeholder is not set',
    },
    formControl: {
      description: 'Form control with `Validators.requiredTrue` set',
      control: { disable: true },
    },
    disabled: { description: 'Sets `disabled` state' },
    readonly: { description: 'Sets `readonly` state' },
  },
};
