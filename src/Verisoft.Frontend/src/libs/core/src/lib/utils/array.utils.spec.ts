import { applyFilter } from './array.utils';
describe('array.utils', () => {
  describe('applyFilter', () => {
    interface TestData {
      id: number;
      name: string;
      tags: string[];
      age: number;
    }

    const data: TestData[] = [
      { id: 1, name: 'Alice', tags: ['developer', 'angular'], age: 25 },
      { id: 2, name: 'Bob', tags: ['designer', 'figma'], age: 30 },
      { id: 3, name: 'Charlie', tags: ['developer'], age: 25 },
      { id: 4, name: 'Dave', tags: ['developer', 'react'], age: 35 },
    ];

    const [alice, , charlie] = data;

    test('Should filter data by contains string value when a single string filter is applied', () => {
      const filter = { name: 'lice' };
      const result = applyFilter(data, filter);
      expect(result).toEqual([alice]);
    });

    test('Should filter data when filter contains an array value', () => {
      const filter = { tags: ['develop'] };
      const result = applyFilter(data, filter);
      expect(result).toHaveLength(3);
    });

    test('Should filter data with multiple conditions when filter properties use AND logic', () => {
      const filter = { age: 25, tags: ['developer'] };
      const result = applyFilter(data, filter);
      expect(result).toEqual([alice, charlie]);
    });

    test('Should return all data when the filter is empty', () => {
      const result = applyFilter(data, {});
      expect(result).toEqual(data);
    });

    test('Should return no data when no items match the filter', () => {
      const filter = { name: 'Zoe' };
      const result = applyFilter(data, filter);
      expect(result).toHaveLength(0);
    });
  });
});
