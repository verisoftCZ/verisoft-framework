import { Pipe, PipeTransform } from '@angular/core';
import { GovFormMultiSelectItem } from '@gov-design-system-ce/components';

const valueDelimiter = '---';

@Pipe({
  name: 'govMultiselectOptions',
  standalone: true,
})
export class GovMultiselectOptionsPipe<T> implements PipeTransform {
  transform(
    options: T[],
    optionLabel?: string,
    optionValue?: string,
    addEmptyOption = false
  ): GovFormMultiSelectItem[] {
    if (!options.length) {
       return [];
    }

    if (this.alreadyConverted(options)) {
       return options as GovFormMultiSelectItem[];
    }

    return this.convertToGovOption(
      options,
      addEmptyOption,
      optionLabel,
      optionValue
    );
  }

  private alreadyConverted(options: any[]): boolean {
    return options.every(
      (o) => o?.value && isNaN(o.value) && o.value.includes(valueDelimiter)
    );
  }

  private convertToGovOption<T>(
    options: any[],
    addEmptyOption: boolean,
    optionLabel?: string,
    optionValue?: string
  ): GovFormMultiSelectItem[] {
    const convertedOptions: GovFormMultiSelectItem[] = [];
    options.forEach((obj) => {
      let stringKey = '';
      let numberKey = '';

      if (optionLabel && optionValue) {
        stringKey = optionLabel;
        numberKey = optionValue;
      } else {
        const keys = Object.keys(obj);

        keys.forEach((key) => {
          if (typeof obj[key] === 'string') {
            stringKey = key;
          } else if (typeof obj[key] === 'number') {
            numberKey = key;
          }
        });
      }

      convertedOptions.push({
        label: obj[stringKey] as string,
        value: obj[numberKey] as string,
      });
    });

    if (addEmptyOption) {
      convertedOptions.unshift({
        label: '',
        value: 'null',
      });
    }

    return convertedOptions;
  }
}
