import { EventEmitter, SimpleChanges } from '@angular/core';
import { FilterEvent, LazyLoadEvent, RequestParams } from '@verisoft/core';

export interface DataSourceComponentModel<TEntity> {
  ngOnChanges?: (changes: SimpleChanges) => void;
  lazy: boolean;
  loading: boolean;
  filter: boolean;
  options: TEntity[] | undefined;
  optionValue: string | undefined;
  optionLabel: string | undefined;
  showed: EventEmitter<any>;
  cleared: EventEmitter<any>;
  filtered: EventEmitter<FilterEvent>;
  lazyLoad: EventEmitter<LazyLoadEvent>;
}

export function setDataToArray<T>(
  targetArray: (T | undefined)[] | undefined,
  data: T[],
  offset = 0,
  total: number | undefined = undefined,
  defaultItem: T | undefined = undefined
): (T | undefined)[] {
  const totalItems = total ?? data.length + offset;
  if (!targetArray) {
    targetArray = Array(totalItems).fill(defaultItem);
  }

  if (targetArray.length < totalItems) {
    targetArray = targetArray.concat(
      new Array(totalItems - targetArray.length).fill(defaultItem)
    );
  }

  for (let i = 0; i < data.length; i++) {
    targetArray[i + offset] = data[i] ?? defaultItem;
  }

  return targetArray;
}

export type ExtendedRequestType<T> = RequestParams<T> & { useNewData: boolean}