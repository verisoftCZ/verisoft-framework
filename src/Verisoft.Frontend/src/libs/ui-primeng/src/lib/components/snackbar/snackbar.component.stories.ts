import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ButtonComponent } from '../button/button.component';
import { SnackbarService } from './services/snackbar.service';
import { SnackbarComponent } from './snackbar.component';

const meta: Meta<SnackbarComponent> = {
  component: SnackbarComponent,
  title: 'SnackbarComponent',
  decorators: [
    moduleMetadata({
      imports: [ButtonComponent, BrowserAnimationsModule, ToastModule],
      providers: [
        MessageService,
        SnackbarService,
        {
          provide: ActivatedRoute,
          useValue: {},
        },
      ],
    }),
  ],
};

export default meta;
type Story = StoryObj<SnackbarComponent>;
const messageService = new MessageService();
const snackbarService = new SnackbarService(messageService);

export const Primary: Story = {
  render: () => {
    return {
      props: {
        showSuccess: () => snackbarService.showSuccess('Snackbar works!'),
        showInfo: () => snackbarService.showInfo('Snackbar works!'),
        showWarn: () => snackbarService.showWarn('Snackbar works!'),
        showError: () => snackbarService.showError('Snackbar works!'),
      },
      template: `
      <v-snackbar class="mt-2" />
        <div class="d-flex gap-3 mt-2">
          <v-button
            label="Show Success"
            severity="success"
            (click)='showSuccess()'
          ></v-button>
          <v-button
            label="Show Info"
            severity="info"
            (click)='showInfo()'
          ></v-button>
          <v-button
            label="Show Warn"
            severity="warn"
            (click)='showWarn()'
          ></v-button>
          <v-button
            label="Show Error"
            severity="danger"
            (click)='showError()'
          ></v-button>
        </div>
      `,
    };
  },
  args: {},
};
