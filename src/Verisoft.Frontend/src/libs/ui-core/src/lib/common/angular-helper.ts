import { OnChanges, SimpleChange, SimpleChanges } from '@angular/core';

export function setComponentProperties<TComponent>(
  component: TComponent,
  value: Partial<TComponent>,
  firstChange = false
) {
  if (!value || !component) {
    return;
  }

  const simpleChanges = Object.keys(value).reduce(
    (changes: SimpleChanges, property: string) => {
      const indexedComponent = component as unknown as {
        [key: string]: TComponent[keyof TComponent];
      };
      const indexedValue = value as unknown as {
        [key: string]: TComponent[keyof TComponent];
      };
      const previousValue = indexedComponent[property];
      const currentValue = indexedValue[
        property
      ] as TComponent[keyof TComponent];
      if (currentValue !== previousValue) {
        indexedComponent[property] = currentValue;
        const change = new SimpleChange(
          previousValue,
          currentValue,
          firstChange
        );

        return { ...changes, [property]: change };
      }

      return changes;
    },
    {}
  );

  const changeableComponent = component as unknown as OnChanges;
  if (changeableComponent['ngOnChanges']) {
    changeableComponent.ngOnChanges(simpleChanges);
  }
}
