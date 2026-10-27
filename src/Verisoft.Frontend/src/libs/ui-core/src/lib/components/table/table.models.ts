import { EventEmitter, InjectionToken, TemplateRef } from '@angular/core';
import { Params } from '@angular/router';
import { LazyLoadEvent, Sort, SortDirectionType } from '@verisoft/core';
import { v4 } from 'uuid';
import { FieldAlignType } from '../../common';

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

export enum TableButtonSeverity {
  success = 'success',
  info = 'info',
  warning = 'warning',
  danger = 'danger',
  help = 'help',
  primary = 'primary',
  secondary = 'secondary',
  contrast = 'contrast',
}

export enum ColumnVisibility {
  visible = 'visible',
  hidden = 'hidden',
  default = 'default',
}

export type TableSelectionModeType = keyof typeof TableSelectionMode;
export type TableButtonSeverityType = keyof typeof TableButtonSeverity;
export type ColumnVisibilityType = keyof typeof ColumnVisibility;

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
  rowDoubleClick: EventEmitter<T>;
  maximumColumnLength: number;
  disableCustomClicks: boolean;
  rowClass: (data: RowModel<T>) => string;
}

export interface TableSignal {
  name: string,
  symbol: symbol
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

export interface ColumnDefinition<T, _KEY = keyof T> {
  id: string;
  value?: (row: T, index?: number) => string;
  headerName?: ((columnId: string, index?: number) => string) | string;
  icon?: string | ((row?: T | undefined) => string);
  type?: string;
  sortable?: boolean;
  sortDirection?: SortDirectionType;
  sortPriority?: number;
  format?: (row: T) => string
  routerLink?: (row: T) => string | string;
  queryParams?: Params;
  columnClass?: string;
  headerClass?: string;
  template?: TemplateRef<{ $implicit: T }>;
  actions?: ActionColumnsDefinition<T>[];
  textAlign?: FieldAlignType;
  width?: string | number;
  forceVisibility?: ColumnVisibilityType;
  visible?: boolean;
}

export function LinkRenderer<T>(text: string, href: string) {
  return (row: T, index: number | undefined) => {
    return '<a href="' + href + '">' + text + '</a>';
  };
}

export function getDefaultColumnSorts<T>(columns: ColumnDefinition<T>[] | undefined): Sort[] | undefined {
  const sorts = (columns ?? [])
    .filter((x) => x.sortable !== false && x.sortDirection)
    .sort((a, b) => (a.sortPriority ?? Number.MAX_SAFE_INTEGER) - (b.sortPriority ?? Number.MAX_SAFE_INTEGER))
    .map((x) => ({ field: x.id, direction: x.sortDirection as SortDirectionType }));

  return sorts.length ? sorts : undefined;
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
  sortPriority?: number;
  columnClass?: string;
  headerClass?: string;
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
  textAlign?: FieldAlignType;
  width: string | undefined;
  forceVisibility: ColumnVisibilityType = ColumnVisibility.default;
  visible = true;

  constructor(readonly configuration: ColumnDefinition<T>) {
    this.id = this.configuration.id;

    if (this.configuration.format) {
      this.format = this.configuration.format;
    }

    if (this.configuration.value) {
      this.valueGetter = (row, index) => {
        const value = this.configuration.value?.(row, index) ?? '-';
        return this.format ? this.format(value, row) : value;
      };
    } else {
      this.valueGetter = (row) => {
        const value = ((row as { [key: string]: string })?.[this.configuration.id]) ?? '';
        return this.format ? this.format(value, row) : value;
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

    if (this.configuration.headerClass) {
      this.headerClass = this.configuration.headerClass;
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

    if (this.configuration.sortDirection !== undefined) {
      this.sortDirection = this.configuration.sortDirection;
    }

    if (this.configuration.sortPriority !== undefined) {
      this.sortPriority = this.configuration.sortPriority;
    }

    if (this.configuration.width){
      this.width = typeof this.configuration.width === "number" ? this.configuration.width + 'px' : this.configuration.width;
    }

    if (this.configuration.textAlign !== undefined) {
      this.textAlign = this.configuration.textAlign;
    }

    if (this.configuration.forceVisibility) {
      this.forceVisibility = this.configuration.forceVisibility;
    }

    if (this.configuration.visible !== undefined) {
      this.visible = this.configuration.visible;
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
  customRoute: string | undefined;

  constructor(
    row: T,
    selected: boolean,
    expanded: boolean,
    marked?: boolean,
    index?: number,
    fnc?: void,
    customRoute?: string,
    entityKey?: string
  ) {
    this.row = row;
    this.id = (row as { [key: string]: string })['id'] ?? v4();
    this.index = index;
    this.selected = selected;
    this.expanded = expanded;
    this.marked = marked;
    this.fnc = fnc;
    this.customRoute = createCustomRoute<T>(row, entityKey, customRoute);
  }
}

function createCustomRoute<T>(row: T, entityKey?: string, customRoute?: string) {
  return customRoute && entityKey
    ? `${customRoute}/` + (row as { [key: string]: string })[entityKey]
    : undefined;
}
