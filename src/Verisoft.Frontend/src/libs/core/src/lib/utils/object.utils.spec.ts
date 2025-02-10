import { getValueByPath, sortBy } from './object.utils';

describe('object.utils', () => {
  describe('sortBy', () => {
    const items = [
      { name: undefined, age: 60 },
      { name: 'Martin', age: 40 },
      { name: 'Alena', age: 28 },
      { name: 'Karel', age: 15 },
    ];

    test('Should sort items by name ascending.', () => {
      const sortedItems = sortBy(items, (x) => x.name);

      expect(items).not.toBe(sortedItems);
      expect(sortedItems).toBeDefined();
      expect(sortedItems?.[0].name).toBeUndefined();
      expect(sortedItems?.[1].name).toBe('Alena');
      expect(sortedItems?.[2].name).toBe('Karel');
      expect(sortedItems?.[3].name).toBe('Martin');
    });

    test('Should sort items by name descending.', () => {
      const sortedItems = sortBy(items, (x) => x.name, false);

      expect(items).not.toBe(sortedItems);
      expect(sortedItems).toBeDefined();
      expect(sortedItems?.[0].name).toBe('Martin');
      expect(sortedItems?.[1].name).toBe('Karel');
      expect(sortedItems?.[2].name).toBe('Alena');
      expect(sortedItems?.[3].name).toBeUndefined();
    });
  });

  describe('getValueByPath', () => {
    const complexObject = {
      name: 'Martin',
      age: 40,
      address: {
        city: 'Brno',
        street: 'Kralova',
      },
    };

    test('Should return property value when name of property is provided.', () => {
      const value = getValueByPath(complexObject, 'name');

      expect(value).toBe('Martin');
    });

    test('Should return undefined when name is nonexistent.', () => {
      const value = getValueByPath(complexObject, 'nonexistentProperty');

      expect(value).toBeUndefined();
    });

    test('Should return property value when name of property is complex path.', () => {
      const value = getValueByPath(complexObject, 'address.city');

      expect(value).toBe('Brno');
    });

    test('Should return undefined when some object on complex path missing.', () => {
      const value = getValueByPath(complexObject, 'car.model');

      expect(value).toBeUndefined();
    });
  });
});
