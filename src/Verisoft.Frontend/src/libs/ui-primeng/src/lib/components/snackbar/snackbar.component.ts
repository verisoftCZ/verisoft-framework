import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SNACKBAR_COMPONENT_TOKEN, SnackbarCore } from '@verisoft/ui-core';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { SnackbarService } from './services/snackbar.service';

@Component({
  selector: 'v-snackbar',
  standalone: true,
  imports: [CommonModule, ToastModule, RippleModule],
  templateUrl: './snackbar.component.html',
  styleUrl: './snackbar.component.scss',
  providers: [
    SnackbarService,
    {
      provide: SNACKBAR_COMPONENT_TOKEN,
      useExisting: SnackbarComponent,
    },
  ],
})
export class SnackbarComponent 
  implements SnackbarCore
{
  constructor(private snackbarService: SnackbarService) {}
}
