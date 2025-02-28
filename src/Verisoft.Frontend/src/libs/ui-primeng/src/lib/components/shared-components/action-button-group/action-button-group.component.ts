import {
  AfterContentInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChildren,
  Input,
  QueryList,
} from '@angular/core';
import {
  ACTION_BUTTON_GROUP_COMPONENT_TOKEN,
  ActionButton,
  ActionButtonGroupCore,
  IconPositionType,
  queryListChanged,
  ScreenSizeService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
import { MenuItem } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import {
  combineLatestWith,
  takeUntil,
} from 'rxjs';
import { ButtonComponent } from '../../button';
import { ActionButtonComponent } from './components/action-button/action-button.component';
import { Icons } from '../../../icons';

@Component({
  selector: 'v-action-button-group',
  standalone: true,
  imports: [MenuModule, ButtonComponent, ActionButtonComponent],
  templateUrl: './action-button-group.component.html',
  styleUrl: './action-button-group.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: ACTION_BUTTON_GROUP_COMPONENT_TOKEN,
      useExisting: ActionButtonGroupComponent,
    },
  ],
})
export class ActionButtonGroupComponent
  extends UnsubscribeComponent
  implements AfterContentInit, ActionButtonGroupCore
{
  @ContentChildren(ActionButtonComponent)
  actions!: QueryList<ActionButtonComponent>;

  @Input() maxItems = 3;

  @Input() maxItemsMobile = 0;

  @Input() items: ActionButton[] = [];

  @Input() menuIconPos: IconPositionType = 'right';

  @Input() menuIcon = Icons.action;

  @Input() label?: string;

  @Input() icon?: string;

  icons = Icons;
  allItems: ActionButton[] = [];
  visibleActions: ActionButton[] = [];
  menuItems: MenuItem[] = [];

  constructor(
    private readonly changeDetectorRef: ChangeDetectorRef,
    readonly screenSizeService: ScreenSizeService
  ) {
    super();
  }

  ngAfterContentInit(): void {
    this.subscribeItemChange();
  }

  fireClick(item: ActionButton, event: MouseEvent) {
    event.stopPropagation();
    item.click.emit();
  }

  private subscribeItemChange(): void {
    const screenResize$ = this.screenSizeService.isMobileBlock;

    const actions$ = queryListChanged(this.actions);

    screenResize$
      .pipe(takeUntil(this.destroyed$), combineLatestWith(actions$))
      .subscribe(([isMobile, actions]) => {
        this.computeItems(actions, isMobile);
      });
  }

  private computeItems(actions: ActionButton[], isMobile: boolean) {
    const allItems = [...(this.items ?? []), ...actions];
    const maxItems = isMobile ? this.maxItemsMobile : this.maxItems;
    this.visibleActions = allItems.slice(0, maxItems);
    this.menuItems = allItems.slice(maxItems).map(this.convertToMenuItem);
    this.changeDetectorRef.detectChanges();
  }

  private convertToMenuItem(item: ActionButton): MenuItem {
    return {
      label: item.label,
      icon: item.icon,
      command: () => {
        item.click.emit();
      },
    };
  }
}
