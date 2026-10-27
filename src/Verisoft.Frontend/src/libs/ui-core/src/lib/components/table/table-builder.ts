import { Injectable } from '@angular/core';
import { ColumnConfiguration } from './column-configuration';
import { ColumnDefinition } from './table.models';

@Injectable({
  providedIn: 'root',
})
export class TableBuilder<TEntity> {
  private columns: ColumnDefinition<TEntity>[] = [];

  addColumn(
    id: string | keyof TEntity,
    config?: (v: ColumnConfiguration<TEntity>) => void
  ): TableBuilder<TEntity> {
    const columnConfig = new ColumnConfiguration<TEntity>(id);
    config?.(columnConfig);
    this.columns.push(columnConfig.build());
    return this;
  }

  addTextColumn(
    id: string | keyof TEntity,
    config?: (v: ColumnConfiguration<TEntity>) => void
  ): TableBuilder<TEntity> {
    return this.addColumn(id, (x) => {
      config?.(x);
      x.type('text');
    });
  }

  addNumberColumn(
    id: string | keyof TEntity,
    config?: (v: ColumnConfiguration<TEntity>) => void
  ): TableBuilder<TEntity> {
    return this.addColumn(id, (x) => {
      config?.(x);
      x.type('number').columnClass('text-end');
    });
  }

  addDateColumn(
    id: string | keyof TEntity,
    config?: (v: ColumnConfiguration<TEntity>) => void
  ): TableBuilder<TEntity> {
    return this.addColumn(id, (x) => {
      config?.(x);
      x.type('date').valueFunction((row) => covertFromDateToUserLocale(row, id as string));
    });
  }

  addBooleanColumn(
    id: string,
    config?: (v: ColumnConfiguration<TEntity>) => void
  ): TableBuilder<TEntity> {
    return this.addColumn(id, (x) => {
      config?.(x);
      x.type('boolean');
    });
  }

  addEnumColumn(
    id: string | keyof TEntity,
    config?: (v: ColumnConfiguration<TEntity>) => void
  ): TableBuilder<TEntity> {
    return this.addColumn(id, (x) => {
      config?.(x);
      x.type('enum');
    });
  }

  build(): ColumnDefinition<TEntity>[] {
    return this.columns;
  }
}

function covertFromDateToUserLocale<TEntity>(row: TEntity, id: string) {
  const value = (row as { [key: string]: undefined | string | Date })[id];
  if (value === undefined) {
    return '';
  }

  const locale = navigator.language;

  if (value instanceof Date) {
    return value.toLocaleDateString(locale);
  }

  if (typeof value === 'string' && !isNaN(Date.parse(value))) {
    return new Date(value).toLocaleDateString(locale);
  }

  return value;
}
