import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { SwitchComponent } from './switch.component';

const meta: Meta<SwitchComponent> = {
  component: SwitchComponent,
  title: 'SwitchComponent',
  decorators: [
    moduleMetadata({
      imports: [ReactiveFormsModule],
    }),
  ],
};
export default meta;
type Story = StoryObj<SwitchComponent>;
const formGroup = new FormGroup({
  switch: new FormControl(null),
  switchRequired: new FormControl(null, [Validators.requiredTrue]),
});
export const Primary: Story = {
  render: (args: any) => ({
    props: {
      ...args,
      formGroup: formGroup,
    },
    template: `
    <form [formGroup]="formGroup">
      <v-switch
      [disabled]="disabled"
      [readonly]="readonly"
      formControlName="switch"
      [required]="required"/>
    </form>`,
  }),
  args: {
    disabled: false,
    readonly: false,
  },
};

export const SwitchRequired: Story = {
  render: (args: any) => ({
    props: {
      ...args,
      formGroup: formGroup,
    },
    template: `
    <form [formGroup]="formGroup">
      <v-switch
      [disabled]="disabled"
      [readonly]="readonly"
      formControlName="switchRequired"
      [required]="required"/>
    </form>`,
  }),
  args: {
    disabled: false,
    readonly: false,
  },
};
