import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DocumentationNavigationItem } from '../../models';

@Component({
  selector: 'v-doc-navigation',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './documentation-navigation.component.html',
  styleUrl: './documentation-navigation.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DocumentationNavigationComponent {
  @Input() items!: DocumentationNavigationItem[];
}
