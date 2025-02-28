import { Pipe, PipeTransform } from '@angular/core';
import { TabViewItemCore } from '@verisoft/ui-core';
import { MenuItem } from 'primeng/api';

@Pipe({
  name: 'tabMenu',
  pure: true,
})
export class TabMenuPipe implements PipeTransform {
  transform(value: TabViewItemCore[] | undefined): MenuItem[] | undefined {
    if (!value) {
      return undefined;
    }

    return value.map((x) => ({
      label: x.title,
      icon: x.icon,
      disabled: x.disabled,
      routerLink: x.url,
    }));
  }
}
