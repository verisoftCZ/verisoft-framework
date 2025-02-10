import { ContentChild, Directive, Input, TemplateRef } from '@angular/core';
import { Params } from '@angular/router';
import { ColumnDefinition, ColumnProvider, TABLE_COLUMN_PROVIDER } from './table.models';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'v-table-column',
  standalone: true,
  providers: [
    {
      provide: TABLE_COLUMN_PROVIDER,
      useExisting: TableColumnDirective,
      multi: true,
    },
  ],
})
export class TableColumnDirective<T> implements ColumnProvider<T> {
  @ContentChild(TemplateRef) template!: TemplateRef<{ $implicit: T }>;

  @Input() index = 0;

  @Input() id!: string;

  @Input() columnClass!: string;

  @Input() sortable!: boolean;

  @Input() routerLink!: (row: T) => string;

  @Input() queryParams!: Params;

  @Input() headerName!: ((column: string, index?: number) => string) | string;

  @Input() width: string | number | undefined;

  getDefinition(): ColumnDefinition<T> {
    return {
      id: this.id,
      columnClass: this.columnClass,
      template: this.template,
      headerName: this.headerName,
      routerLink: this.routerLink,
      queryParams: this.queryParams,
      sortable: this.sortable,
      width: this.width,
    };
  }
}
