import { Component, Input, TemplateRef, ViewChild } from '@angular/core';

@Component({
  selector: 'v-tab-view-item',
  template: `<ng-template #contentTemplate>
    <ng-content></ng-content>
  </ng-template>`,
})
export class TabViewItemComponent {
  @ViewChild('contentTemplate', { static: true })
  contentTemplate!: TemplateRef<any>;

  @Input() title!: string;

  @Input() url?: string;

  @Input() icon?: string;

  @Input() disabled?: boolean;

  @Input() content?: string;
}
