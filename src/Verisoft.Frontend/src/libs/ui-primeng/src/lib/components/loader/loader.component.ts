import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { LOADER_COMPONENT_TOKEN, LoaderCore } from '@verisoft/ui-core';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'v-loader',
  standalone: true,
  imports: [CommonModule, ProgressSpinnerModule],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: LOADER_COMPONENT_TOKEN, useExisting: LoaderComponent },
  ],
})
export class LoaderComponent implements LoaderCore {}
