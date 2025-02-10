import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { ButtonComponent } from '../button';
import { ConfirmDialogComponent } from './confirm-dialog.component';

const meta: Meta<ConfirmDialogComponent> = {
  component: ConfirmDialogComponent,
  title: 'ConfirmDialogComponent',
  decorators: [
    moduleMetadata({
      imports: [BrowserAnimationsModule, ButtonComponent],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: ActivatedRoute,
        },
      ],
    }),
  ],
};
export default meta;
type Story = StoryObj<ConfirmDialogComponent>;
const dataOptions = [
  {
    headerIcon: 'pi pi-times',
    severity: 'danger',
    confirmButtonText: 'Agreed',
    innerHtml:
      'You are <b class="text-danger">not allowed</b> to do that. Please contact <b>admin</b>.',
    label: 'Danger',
  },
  {
    headerIcon: 'pi pi-check',
    severity: 'success',
    confirmButtonText: 'Agreed',
    innerHtml: 'Item <b class="text-success">successfully</b> saved.',
    label: 'Success',
  },
  {
    headerIcon: 'pi pi-exclamation-triangle',
    severity: 'primary',
    confirmButtonText: 'Agreed',
    innerHtml:
      'You might be <b class="text-primary">not allowed</b> to do that. Please contact <b>admin</b>.',
    label: 'Warn',
  },
];

const dataOptionsMap = {
  Danger: dataOptions[0],
  Success: dataOptions[1],
  Warn: dataOptions[2],
};

export const Primary = {
  args: {
    visible: true,
    data: dataOptions[0],
  },
  argTypes: {
    data: {
      options: dataOptionsMap,
      control: {
        type: 'select',
        labels: {
          danger: 'Danger',
          success: 'Success',
          warn: 'Warn',
        },
      },
    },
  },
};
