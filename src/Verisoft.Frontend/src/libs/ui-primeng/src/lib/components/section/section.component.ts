import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { SECTION_COMPONENT_TOKEN, SectionCore } from '@verisoft/ui-core';
import { Icons } from '../../icons';
import { ButtonComponent } from '../button';

@Component({
    selector: 'v-section',
    imports: [CommonModule, ButtonComponent],
    templateUrl: './section.component.html',
    styleUrl: './section.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        { provide: SECTION_COMPONENT_TOKEN, useExisting: SectionComponent },
    ]
})
export class SectionComponent 
  implements SectionCore
{
  @Input() title!: string;
  @Input() showContent = true;
  @Input() backgroundColor!: string;

  icons = Icons;
  protected icon = Icons.chevronDown;

  protected toggle(): void {
    this.showContent = !this.showContent;
    this.icon = this.showContent ? Icons.chevronDown : Icons.chevronUp;
  }
}
