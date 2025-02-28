import { ColumnDefinition } from "./table.models";

export class ColumnConfiguration<TEntity> {
    private column: Partial<ColumnDefinition<TEntity>> = {};
  
    constructor(private id: string | keyof TEntity) {
      this.column.id = id as string;
    }
  
    headerName(headerName: string | ((columnId: string, index?: number) => string)): this {
      this.column.headerName = headerName;
      return this;
    }
  
    sortable(sortable: boolean): this {
      this.column.sortable = sortable;
      return this;
    }
  
    columnClass(columnClass: string): this {
      this.column.columnClass = columnClass;
      return this;
    }
  
    valueFunction(value: (row: TEntity, index?: number) => string): this {
      this.column.value = value;
      return this;
    }
  
    type(type: string): this {
      this.column.type = type;
      return this;
    }
  
    build(): ColumnDefinition<TEntity> {
      return this.column as ColumnDefinition<TEntity>;
    }
  }