import { cloneDeep, getValueByPath, sortBy } from './object.utils';

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

  describe('cloneDeep', () => {
    test('Should return null if input is null', () => {
      const result = cloneDeep(null);
      expect(result).toBeNull();
    });

    test('Should return primitive value if input is primitive', () => {
      expect(cloneDeep(1)).toBe(1);
      expect(cloneDeep('string')).toBe('string');
      expect(cloneDeep(true)).toBe(true);
    });

    test('Should clone Date object', () => {
      const date = new Date('2023-01-01');
      const clonedDate = cloneDeep(date);
      expect(clonedDate).toEqual(date);
      expect(clonedDate).not.toBe(date);
    });

    test('Should clone Array', () => {
      const array = [1, { a: 2 }, [3]];
      const clonedArray = cloneDeep(array);
      expect(clonedArray).toEqual(array);
      expect(clonedArray).not.toBe(array);
      expect(clonedArray[1]).not.toBe(array[1]);
      expect(clonedArray[2]).not.toBe(array[2]);
    });

    test('Should clone Map', () => {
      const map = new Map<any, any>([['key', { a: 1 }]]);
      const clonedMap = cloneDeep(map);
      expect(clonedMap).toEqual(map);
      expect(clonedMap).not.toBe(map);
      expect(clonedMap.get('key')).not.toBe(map.get('key'));
    });

    test('Should clone Set', () => {
      const set = new Set([{ a: 1 }]);
      const clonedSet = cloneDeep(set);
      expect(clonedSet).toEqual(set);
      expect(clonedSet).not.toBe(set);
      // Check if the object inside the set is a different instance
      const originalObj = Array.from(set)[0];
      const clonedObj = Array.from(clonedSet)[0];
      expect(clonedObj).toEqual(originalObj);
      expect(clonedObj).not.toBe(originalObj);
    });

    test('Should deep clone object', () => {
      const obj = {
        a: 1,
        b: { c: 2 },
        d: new Date(),
        e: [1, 2],
      };
      const clonedObj = cloneDeep(obj);
      expect(clonedObj).toEqual(obj);
      expect(clonedObj).not.toBe(obj);
      expect(clonedObj.b).not.toBe(obj.b);
      expect(clonedObj.d).not.toBe(obj.d);
      expect(clonedObj.e).not.toBe(obj.e);
    });
  });
});
