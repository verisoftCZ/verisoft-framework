import {
  AfterViewChecked,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { highlightElement } from 'prismjs';
import 'prismjs/components/prism-typescript';
import 'prismjs/components/prism-csharp';

@Component({
  selector: 'v-doc-code-highlighter',
  standalone: true,
  templateUrl: './code-highlighter.component.html',
  styleUrls: ['./code-highlighter.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CodeHighlighterComponent implements AfterViewChecked {
  @Input() code?: string;

  @Input() language?: string;

  @ViewChild('codeElement') codeElement?: ElementRef;

  ngAfterViewChecked(): void {
    if (this.codeElement) {
      highlightElement(this.codeElement.nativeElement);
    }
  }
}
