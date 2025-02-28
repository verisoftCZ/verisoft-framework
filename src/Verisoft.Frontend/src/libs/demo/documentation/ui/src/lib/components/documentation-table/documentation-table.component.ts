import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { sortBy } from '@verisoft/core';
import { DocumentationTable, DocumentationTableItem } from '../../models';

@Component({
  selector: 'v-doc-table',
  standalone: true,
  imports: [],
  templateUrl: './documentation-table.component.html',
  styleUrl: './documentation-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DocumentationTableComponent implements OnChanges {
  @Input() table?: DocumentationTable;

  orderedItems: DocumentationTableItem[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    this.orderedItems = sortBy(this.table?.items ?? [], (x) => x.name);
  }
}
