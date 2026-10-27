import { SortDirection, SortDirectionType } from '@verisoft/core';

export function getNextSortDirection(
  sortDirection: SortDirectionType | undefined,
  isNullable = false
): SortDirectionType | undefined {
  return isNullable && sortDirection === SortDirection.desc
    ? undefined
    : !sortDirection || sortDirection === SortDirection.desc
    ? SortDirection.asc
    : SortDirection.desc;
}
export function createCustomRoute<T>(row: T, entityKey?: string, customRoute?: string) {
  return customRoute && entityKey
    ? customRoute + (row as { [key: string]: string })[entityKey]
    : undefined;
}
