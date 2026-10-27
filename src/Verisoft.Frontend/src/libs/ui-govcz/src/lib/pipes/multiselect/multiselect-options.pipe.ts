import { Pipe, PipeTransform } from '@angular/core';
import { GovFormMultiSelectItem } from '@gov-design-system-ce/components';

@Pipe({
  name: 'govMultiselectOptions',
  standalone: true,
})
export class GovMultiselectOptionsPipe implements PipeTransform {
  transform(
    options: any[] | undefined,
    optionLabel?: string,
    optionValue?: string,
    addEmptyOption = false,
  ): GovFormMultiSelectItem[] {
    if (!options?.length) return [];

    return this.convertToGovOption(options, addEmptyOption, optionLabel, optionValue);
  }

  private convertToGovOption(
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
        value: (obj[numberKey] as string)?.toString() ?? 'VALUE NOT SET',
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
