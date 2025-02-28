import { Sort } from '../models';
import { getValueByPath } from './object.utils';

type ArrayIntersectionType = (string | number)[];

export function isArrayIntersected(
  array1: ArrayIntersectionType,
  array2: ArrayIntersectionType
) {
  const filteredArray = array1.filter((value) => array2.includes(value));
  return filteredArray.length > 0;
}

export function multiSort<T>(values: T[], sorts: Sort[]): T[] {
  return [...values].sort((a, b) => {
    for (const sort of sorts) {
      const { field, direction } = sort;
      const valueA = getValueByPath(a, field);
      const valueB = getValueByPath(b, field);

      if (valueA !== valueB) {
        if (isComparable(valueA) && isComparable(valueB)) {
          if (valueA < valueB) {
            return direction === 'asc' ? -1 : 1;
          }
          if (valueA > valueB) {
            return direction === 'asc' ? 1 : -1;
          }
        }
      }
    }

    return 0;
  });
}

export function applyFilter<T>(data: T[], filter: Partial<T>): T[] {
  if (!filter || Object.keys(filter).length === 0) {
    return data;
  }

  return data.filter((item) => {
    return Object.entries(filter).every(([filterProperty, filterPropertyValue]) => {
      if (filterPropertyValue == undefined) {
        return true;
      }

      const itemValue = getValueByPath(item, filterProperty);
      return isSatisfiedBy(itemValue, filterPropertyValue);
    });
  });
}

export function isSatisfiedBy(value: unknown, filter: unknown): boolean {
  if (!value) {
    return !filter;
  }

  if (typeof filter === 'string') {
    return value?.toString().toLowerCase().includes(filter.toLowerCase());
  } else if (Array.isArray(filter)) {
    return filter.some((val) => isSatisfiedBy(value, val));
  } else {
    return filter === value;
  }

  return false;
}

function isComparable(value: unknown): value is string | number | Date {
  return (
    typeof value === 'string' ||
    typeof value === 'number' ||
    typeof value === 'boolean' ||
    value instanceof Date
  );
}
