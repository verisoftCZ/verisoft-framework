import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DocumentationCode } from '../../models';
import { CodeHighlighterComponent } from '../code-highlighter/code-highlighter.component';

@Component({
  selector: 'v-doc-code',
  standalone: true,
  imports: [CodeHighlighterComponent, CommonModule],
  templateUrl: './documentation-code.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrl: './documentation-code.component.scss',
})
export class DocumentationCodeComponent {
  @Input() code?: DocumentationCode;
}
