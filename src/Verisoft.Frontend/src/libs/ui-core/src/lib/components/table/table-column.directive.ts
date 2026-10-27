import { ContentChild, Directive, Input, TemplateRef } from '@angular/core';
import { Params } from '@angular/router';
import { SortDirectionType } from '@verisoft/core';
import { FieldAlign, FieldAlignType } from '../../common';
import { ColumnDefinition, ColumnProvider, ColumnVisibility, ColumnVisibilityType, TABLE_COLUMN_PROVIDER } from './table.models';

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
  @ContentChild(TemplateRef) template!:
  TemplateRef<{ $implicit: T }>;

  @Input() index = 0;

  @Input() id!: string;

  @Input() columnClass!: string;

  @Input() headerClass!: string;

  @Input() sortable!: boolean;

  @Input() sortDirection?: SortDirectionType;

  @Input() sortPriority?: number;

  @Input() routerLink!: (row: T) => string;

  @Input() queryParams!: Params;

  @Input() headerName!: ((column: string, index?: number) => string) | string;

  @Input() width: string | number | undefined;

  @Input() textAlign: FieldAlignType = FieldAlign.left;

  @Input() format!: (row: T) => string

  @Input() forceVisibility: ColumnVisibilityType = ColumnVisibility.default;

  @Input() visible = true;

  getDefinition(): ColumnDefinition<T> {
    return {
      id: this.id,
      columnClass: this.columnClass,
      headerClass: this.headerClass,
      template: this.template,
      headerName: this.headerName,
      routerLink: this.routerLink,
      queryParams: this.queryParams,
      sortable: this.sortable,
      sortDirection: this.sortDirection,
      sortPriority: this.sortPriority,
      width: this.width,
      format: this.format,
      textAlign: this.textAlign,
      forceVisibility: this.forceVisibility,
      visible: this.visible,
    };
  }
}
