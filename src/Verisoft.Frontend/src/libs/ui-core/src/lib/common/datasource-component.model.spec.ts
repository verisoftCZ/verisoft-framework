import { setDataToArray } from './datasource-component.model';

describe('setDataToArray', () => {
  test('Should return data when target array is null and data is passed.', () => {
    const data = ['item 1', 'item 2'];
    const targetData = undefined;

    const actual = setDataToArray(targetData, data);

    expect(actual).toEqual(data);
  });

  test('Should return data when target array is null and data is passed.', () => {
    const data = ['item 1', 'item 2'];
    const targetData: string[] = [];

    const actual = setDataToArray(targetData, data);

    expect(actual).toEqual(data);
  });

  test('Should return extended target array when target array is empty and data is passed and offset is specified.', () => {
    const data = ['item 1', 'item 2'];
    const targetData: string[] = [];
    const offset = 1;

    const actual = setDataToArray(targetData, data, offset);

    expect(actual).toEqual([undefined, ...data]);
  });

  test('Should return target array extended to total items when data is passed and total is specified.', () => {
    const data = ['item 1', 'item 2'];
    const targetData: string[] = [];
    const total = 5;
    const offset = 0;

    const actual = setDataToArray(targetData, data, offset, total);

    expect(actual.length).toEqual(total);
  });
});
