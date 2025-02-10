import { EventEmitter, InjectionToken, TemplateRef } from '@angular/core';
import { Params } from '@angular/router';
import { LazyLoadEvent, Sort, SortDirectionType } from '@verisoft/core';
import { v4 } from 'uuid';

export const TABLE_COMPONENT_TOKEN = new InjectionToken<TableCore<any>>(
  'TabVIewComponentToken'
);

export const TABLE_COLUMN_PROVIDER = new InjectionToken(
  'TABLE_COLUMN_PROVIDER'
);
export enum TableSelectionMode {
  single = 'single',
  multiple = 'multiple',
}

export type TableSelectionModeType = keyof typeof TableSelectionMode;

export interface TableCore<T> {
  sorters: Sort[];
  columns: ColumnDefinition<T>[];
  data: T[];
  total: number;
  filter: Partial<T> | undefined;
  loading: boolean;
  scrollable: boolean;
  pageSize: number;
  currentPage: number;
  showPaginator: boolean;
  sortMultiple: boolean;
  lazy: boolean;
  selection: T[];
  selectionMode: TableSelectionModeType | undefined;
  showPageSizePicker: boolean;
  entityKey: string | undefined;
  selectionChange: EventEmitter<T[]>;
  lazyLoad: EventEmitter<LazyLoadEvent>;
}

export interface ActionColumnsDefinition<T> {
  severity?: TableButtonSeverity;
  title?: string;
  icon?: string;
  tooltip?: string;
  rounded?: boolean;
  outlined?: boolean;
  raised?: boolean;
  badge?: string;
  visible?: (row: T) => boolean;
  routerLink?: (row: T) => string;
  queryParams?: Params;
  disabled?: boolean | ((row: T) => boolean);
  onClick?: (row: T, event?: MouseEvent) => void;
}

export interface ColumnProvider<T> {
  getDefinition(): ColumnDefinition<T>;
  index: number;
}

export enum TableButtonSeverity {
  SUCCESS = 'success',
  INFO = 'info',
  WARNING = 'warning',
  DANGER = 'danger',
  HELP = 'help',
  PRIMARY = 'primary',
  SECONDARY = 'secondary',
  CONTRAST = 'contrast',
}

export interface ColumnDefinition<T, _KEY = keyof T> {
  id: string;
  value?: (row: T, index?: number) => string;
  headerName?: ((columnId: string, index?: number) => string) | string;
  icon?: string | ((row?: T | undefined) => string);
  type?: string;
  sortable?: boolean;
  // eslint-disable-next-line @typescript-eslint/ban-types
  format?: Function;
  routerLink?: (row: T) => string | string;
  queryParams?: Params;
  columnClass?: string;
  template?: TemplateRef<{ $implicit: T }>;
  actions?: ActionColumnsDefinition<T>[];
  width?: string | number;
}

export function LinkRenderer<T>(text: string, href: string) {
  return (row: T, index: number | undefined) => {
    return '<a href="' + href + '">' + text + '</a>';
  };
}

export function routerRenderer<T>(link: string, text: string) {
  return (row: T, index?: number | undefined) => {
    return { text: text, link: link };
  };
}

export function Renderer<T>(fnc: (row: T) => string) {
  return (row: T) => {
    return fnc(row);
  };
}

export class ColumnModel<T> {
  sortDirection: SortDirectionType | undefined = undefined;
  columnClass?: string;
  queryParams?: Params;
  routerLink?: (row: T) => string | string;
  valueGetter!: (row: T, index: number) => string;
  headerGetter!: (columnId: string, index: number) => string | string;
  template?: TemplateRef<{ $implicit: T }>;
  actions?: ActionColumnsDefinition<T>[];
  sortable?: boolean;
  id: string;
  // eslint-disable-next-line @typescript-eslint/ban-types
  format!: Function;
  width: string | undefined;

  constructor(readonly configuration: ColumnDefinition<T>) {
    this.id = this.configuration.id;

    if (this.configuration.format) {
      this.format = this.configuration.format;
    }

    if (this.configuration.value) {
      this.valueGetter = (row, index) => {
        const value = this.configuration.value?.(row, index) ?? '-';
        return this.format ? this.format(value) : value;
      };
    } else {
      this.valueGetter = (row) => {
        const value = ((row as { [key: string]: string })?.[this.configuration.id]) ?? '';
        return this.format ? this.format(value) : value;
      };
    }

    if (this.configuration.actions) {
      this.actions = this.configuration.actions;
    }

    if (this.configuration.routerLink) {
      this.routerLink = this.configuration.routerLink;
    }

    if (this.configuration.columnClass) {
      this.columnClass = this.configuration.columnClass;
    }

    if (this.configuration.template) {
      this.template = this.configuration.template;
    }

    if (this.configuration.headerName) {
      this.headerGetter = (
        typeof this.configuration.headerName === 'string'
          ? () => this.configuration.headerName ?? ''
          : (columnId: string, index: number) =>
              (<(columnId: string, index?: number) => string>(
                this.configuration.headerName
              ))?.(columnId, index) ?? this.id
      ) as (columnId: string, index: number) => string;
    } else {
      this.headerGetter = () => "";
    }

    if (this.configuration.queryParams) {
      this.queryParams = this.configuration.queryParams;
    }

    if (this.routerLink) {
      this.columnClass += ' ' + 'link';
    }

    if (this.configuration.sortable !== undefined) {
      this.sortable = this.configuration.sortable;
    } else {
      this.sortable = true;
    }

    if (this.configuration.width){
      this.width = typeof this.configuration.width === "number" ? this.configuration.width + 'px' : this.configuration.width;
    }
  }
}

export class RowModel<T> {
  row: T;
  index?: number;
  id: number | string;
  selected: boolean;
  marked?: boolean;
  focused?: boolean;
  expanded: boolean;
  fnc?: void;

  constructor(
    row: T,
    selected: boolean,
    expanded: boolean,
    marked?: boolean,
    index?: number,
    fnc?: void,
  ) {
    this.row = row;
    this.id = (row as { [key: string]: string })['id'] ?? v4();
    this.index = index;
    this.selected = selected;
    this.expanded = expanded;
    this.marked = marked;
    this.fnc = fnc;
  }
}
