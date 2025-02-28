import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { createUrl, DOCS_BLOCK_TYPE, DocumentationItem } from '../../models';
import { DocumentationCodeComponent } from '../documentation-code/documentation-code.component';
import { DocumentationHeaderComponent } from '../documentation-header/documentation-header.component';
import { DocumentationTableComponent } from '../documentation-table/documentation-table.component';

@Component({
  selector: 'v-doc-section',
  standalone: true,
  imports: [
    DocumentationHeaderComponent,
    DocumentationCodeComponent,
    DocumentationTableComponent,
  ],
  templateUrl: './documentation-section.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DocumentationSectionComponent {
  @Input() items?: DocumentationItem[];

  createUrl = createUrl;

  sectionType = DOCS_BLOCK_TYPE;
}
