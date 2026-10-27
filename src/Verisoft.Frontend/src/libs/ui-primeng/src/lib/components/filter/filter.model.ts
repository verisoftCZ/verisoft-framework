import { FormGroup } from '@angular/forms';

export function getFilledControlCount(formGroup: FormGroup): number {
  let count = 0;
  Object.keys(formGroup.controls).forEach((key) => {
    const control = formGroup.get(key);
    if (control?.value) {
      if (typeof control.value === 'boolean') {
        count++;
      }
      if (typeof control.value === 'string' && control.value.trim() !== '') {
        count++;
      }
    }
  });

  return count;
}
