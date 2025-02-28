import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { generateFormGroup, GenericFieldDefinition } from './generic-form.model';

describe('GenericFormModel', () => {
  describe('generateFormGroup', () => {
    const fields = [{ name: 'name' }, { name: 'age' }];

    const containsField = (formGroup: UntypedFormGroup, field: GenericFieldDefinition) => expect(formGroup.get(field.name)).toBeDefined();

    test('Should create a new group when no group was provided', () => {
      const formGroup = generateFormGroup([], undefined, undefined, false);

      expect(formGroup).toBeDefined();
    });

    test('Should create a new group when no fields are provided', () => {
      const formGroup = generateFormGroup(
        undefined,
        undefined,
        undefined,
        false
      );

      expect(formGroup).toBeDefined();
    });

    test('Should create a new group with defined control when fields are provided', () => {
      const formGroup = generateFormGroup(fields, undefined, undefined, false);

      expect(formGroup).toBeDefined();
      containsField(formGroup, fields[0]);
      containsField(formGroup, fields[1]);
    });

    test('Should used input group and creates control when fields are provided and formgroup does not have a controls', () => {
      const inputGroup = new UntypedFormGroup({});

      const formGroup = generateFormGroup(fields, undefined, inputGroup, false);

      expect(formGroup).toBe(inputGroup);
      containsField(formGroup, fields[0]);
    });

    test('Should not remove control from input group when input group is provided and fields does not contain control definition.', () => {
      const inputGroup = new UntypedFormGroup({
        student: new UntypedFormControl(),
      });

      const formGroup = generateFormGroup(fields, undefined, inputGroup, false);

      expect(formGroup.get('student')).toBeDefined();
    });

    test('Should use existing group when input group is provided but have already defined group.', () => {
      const inputGroup = new UntypedFormGroup({
        student: new UntypedFormControl(),
      });

      const lastGroup = new UntypedFormGroup({
      });

      const formGroup = generateFormGroup(fields, lastGroup, inputGroup, false);

      expect(formGroup).toBe(lastGroup);
      containsField(formGroup, fields[0]);
    });

    test('Should use existing a new input group when last form group exists, but input group was changed.', () => {
      const inputGroup = new UntypedFormGroup({
        student: new UntypedFormControl(),
      });

      const lastGroup = new UntypedFormGroup({
      });

      const formGroup = generateFormGroup(fields, lastGroup, inputGroup, true);

      expect(formGroup).toBe(inputGroup);
      containsField(formGroup, fields[0]);
    });
  });
});
