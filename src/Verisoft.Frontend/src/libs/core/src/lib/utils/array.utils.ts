import { Sort } from "../models";
import { getValueByPath } from "./object.utils";

type ArrayIntersectionType = (string | number)[]

export function isArrayIntersected(
  array1: ArrayIntersectionType,
  array2: ArrayIntersectionType,
) {
  const filteredArray = array1.filter(value => array2.includes(value));
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
          if (typeof valueA === 'string' && typeof valueB === 'string') {
            const baseCompare = valueA.localeCompare(valueB, undefined, {
              sensitivity: 'base',
            });

            if (baseCompare === 0) {
              return valueA < valueB ? -1 : valueA > valueB ? 1 : 0;
            }

            return direction === 'asc' ? baseCompare : -baseCompare;
          } else {
            if (valueA < valueB) {
              return direction === 'asc' ? -1 : 1;
            }
            if (valueA > valueB) {
              return direction === 'asc' ? 1 : -1;
            }
          }
        }
      }
    }

    return 0;
  });
}

function isComparable(value: unknown): value is string | number | Date {
  return typeof value === 'string' || typeof value === 'number' || value instanceof Date;
}

export function applyFilter<T>(data: T[], filter: any): T[] {
  if (!filter || Object.keys(filter).length === 0) {
    return data;
  }

  return data.filter((item) => {
    return Object.keys(filter).every((key) => {
      const filterValue = filter[key];
      const itemValue = getValueByPath(item, key);

      if (itemValue === undefined || itemValue === null) {
        return false;
      }

      if (Array.isArray(itemValue)) {
        if (Array.isArray(filterValue)) {
             return itemValue.some(iv => 
                filterValue.some(fv => 
                    typeof iv === 'string' && typeof fv === 'string' 
                    ? iv.toLowerCase().includes(fv.toLowerCase())
                    : iv === fv
                )
             );
        }
        return itemValue.includes(filterValue);
      }

      if (Array.isArray(filterValue)) {
          return filterValue.includes(itemValue);
      }

      if (typeof filterValue === 'string' && typeof itemValue === 'string') {
        return itemValue.toLowerCase().includes(filterValue.toLowerCase());
      }

      return itemValue == filterValue;
    });
  });
}

