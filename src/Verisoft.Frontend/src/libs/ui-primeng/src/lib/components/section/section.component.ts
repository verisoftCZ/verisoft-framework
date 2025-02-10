import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { SECTION_COMPONENT_TOKEN, SectionCore } from '@verisoft/ui-core';
import { ButtonComponent } from '../button';

@Component({
  selector: 'v-section',
  standalone: true,
  imports: [CommonModule, ButtonComponent],
  templateUrl: './section.component.html',
  styleUrl: './section.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: SECTION_COMPONENT_TOKEN, useExisting: SectionComponent },
  ],
})
export class SectionComponent 
  implements SectionCore
{
  @Input() title!: string;
  @Input() showContent = true;
  @Input() backgroundColor!: string;

  protected icon = 'pi-angle-down';

  protected toggle(): void {
    this.showContent = !this.showContent;
    this.icon = this.showContent ? 'pi-angle-down' : 'pi-angle-up';
  }
}
