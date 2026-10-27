import { Component, Input } from "@angular/core";

@Component({
  standalone: true,
  selector: "v-table-pagination-info",
  templateUrl: "./table-pagination-info.component.html",
})
export class TablePaginationInfoComponent {
  @Input() showingText = 'Showing'

  @Input() toText = 'to';

  @Input() ofText = 'of';

  @Input() entriesText = 'entries'

  @Input() currentPage = 0;

  @Input() pageSize = 0;

  @Input() total = 0;
}
