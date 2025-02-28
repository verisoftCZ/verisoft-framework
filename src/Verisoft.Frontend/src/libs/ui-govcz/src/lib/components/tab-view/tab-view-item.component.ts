import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { TabViewItemCore } from '@verisoft/ui-core';
import { Icons } from '../../icons';

@Component({
  standalone: false,
  selector: 'v-tab-view-item',
  template: `<ng-template #contentTemplate>
    <ng-content></ng-content>
  </ng-template>`
  })
export class TabViewItemComponent 
  implements TabViewItemCore
{
  @ViewChild('contentTemplate', { static: true })
  contentTemplate!: TemplateRef<any>;

  @Input() title!: string;

  @Input() url?: string;

  @Input() icon?: string;

  @Input() disabled?: boolean;

  @Input() content?: string;

  icons = Icons;
}
