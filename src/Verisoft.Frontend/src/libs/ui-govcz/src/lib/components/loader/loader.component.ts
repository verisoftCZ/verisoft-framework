import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { GovDesignSystemModule } from '@gov-design-system-ce/angular';
import { 
    LOADER_COMPONENT_TOKEN, 
    LoaderCore,
    FieldSize,
    FieldSizeType,
} from '@verisoft/ui-core';
import { GovSizePipe } from '../../pipes';

@Component({
  selector: 'v-loader',
  standalone: true,
  imports: [
    CommonModule, GovDesignSystemModule, GovSizePipe
  ],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { 
      provide: LOADER_COMPONENT_TOKEN, 
      useExisting: LoaderComponent,
    }
  ],
})
export class LoaderComponent 
    implements LoaderCore 
{
  @Input() size: FieldSizeType = FieldSize.medium;
  @Input() message!: string;
}