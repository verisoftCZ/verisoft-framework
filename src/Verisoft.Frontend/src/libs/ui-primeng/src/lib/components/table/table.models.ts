import { FilterMetadata } from 'primeng/api';

export function convertToFilter<T>(
  value:
    | {
        [s: string]: FilterMetadata | FilterMetadata[] | undefined;
      }
    | Partial<T>
    | undefined
): Partial<T> | undefined {
  if (value == undefined) {
    return undefined;
  }

  const filter: { [key: string]: unknown } = {};
  Object.keys(value).map((key) => {
    const filterValue = (value as { [s: string]: unknown })[key];
    const originalFilterKey = key?.startsWith('row.')
        ? key.slice(4)
        : key;
    if (filterValue != undefined && typeof filterValue === 'object') {
      filter[originalFilterKey] = (filterValue as FilterMetadata).value;
    } else {
      filter[originalFilterKey] = filterValue;
    }
  });

  return filter as Partial<T>;
}
