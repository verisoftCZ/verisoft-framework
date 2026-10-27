import { Component, Input, ViewChild, TemplateRef } from '@angular/core';
import { TabViewItemCore } from '@verisoft/ui-core';

@Component({
  
  selector: 'v-tab-view-item',
  template: `<ng-template #contentTemplate>
    <ng-content></ng-content>
  </ng-template>`
  })
export class TabViewItemComponent 
implements TabViewItemCore {

  @Input() id?: string;
  @Input() index?: number;
  @Input() disabled = false;
  @Input() title!: string;
  @Input() content?: string;
  @Input() url?: string;

  @ViewChild('contentTemplate', { static: true })
  contentTemplate!: TemplateRef<any>;
}
