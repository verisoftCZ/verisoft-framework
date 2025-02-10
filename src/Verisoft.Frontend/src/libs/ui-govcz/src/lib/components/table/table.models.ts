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
