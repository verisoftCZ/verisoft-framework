import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'v-doc-header',
  standalone: true,
  imports: [],
  templateUrl: './documentation-header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DocumentationHeaderComponent {
  @Input() id?: string;

  @Input() title?: string;

  @Input() description?: string;

  @Input() level?: number;
}
