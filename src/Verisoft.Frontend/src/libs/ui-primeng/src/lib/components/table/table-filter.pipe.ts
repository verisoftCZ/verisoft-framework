import { Pipe, PipeTransform } from '@angular/core';
import { FilterMetadata } from 'primeng/api';

type PrimeNgFilterDefinition = {
  [s: string]: FilterMetadata | FilterMetadata[];
};

@Pipe({
  name: 'primengFilter',
  standalone: true,
})
export class TableFilterPipe<T> implements PipeTransform {
  transform(value: Partial<T>): PrimeNgFilterDefinition | undefined {
    if (value == undefined) {
      return undefined;
    }

    return Object.entries(value)
      .filter((x) => x[0] != undefined)
      .reduce((currentFilter: PrimeNgFilterDefinition, [key, value]) => {
        const filterValue = this.convertToFilterMetadata(value);
        if (filterValue) {
          if (key === 'searchField') {
            key = 'global';
          }
          
          currentFilter[key] = filterValue;
        }

        return currentFilter;
      }, {});
  }

  private convertToFilterMetadata(
    value: unknown
  ): FilterMetadata | FilterMetadata[] | undefined {
    if (value == undefined) {
      return undefined;
    }

    if (typeof value == 'string') {
      return {
        matchMode: 'contains',
        value: value,
      };
    }

    if (Array.isArray(value)) {
      return value.map((x) =>
        this.convertToFilterMetadata(x)
      ) as FilterMetadata[];
    }

    return {
      matchMode: 'equals',
      value: value,
    };
  }
}
