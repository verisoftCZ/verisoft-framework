import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MenuItem, ScreenSizeService, SideMenuCore, SideMenuService, UnsubscribeComponent } from '@verisoft/ui-core';
import { AvatarModule } from 'primeng/avatar';
import { TreeModule, TreeNodeSelectEvent } from 'primeng/tree';
import { ButtonComponent } from '../button';
import { Icons } from '../../icons';

@Component({
  selector: 'v-side-menu',
  standalone: true,
  imports: [
    NgClass,
    AsyncPipe,
    NgIf,
    TreeModule,
    ButtonComponent,
    TranslateModule,
    AvatarModule,
    RouterModule,
  ],
  templateUrl: './side-menu.component.html',
  styleUrl: './side-menu.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideMenuComponent
  extends UnsubscribeComponent
  implements SideMenuCore, AfterContentInit
{
  @Input() items: MenuItem[] = [];
  @Input() logoUrl = '';
  @Input() userName!: string;
  @Input() userRole!: any | any[] | undefined;
  @Output() minimalized = new EventEmitter<boolean>();
  @Output() itemSelected = new EventEmitter<MenuItem>();
  
  @ViewChild ('sidemenu', { static: true }) menu!: ElementRef<HTMLDivElement>;

  menuService = inject(SideMenuService);
  router = inject(Router);
  icons = Icons;

  protected screenSizeService = inject(ScreenSizeService);
  protected logoRouterLink!: string;
  protected isMinimalized = false;

  ngAfterContentInit(): void {
    if (this.menuService.menuMinimalized) {
      this.setMenuMinimalized(this.menu);
    }
  }
  
  protected menuMinimalize(menu: HTMLDivElement): void {
    const audit = document.getElementsByClassName('detail-audit');
    if (audit.length > 0) {
      audit.item(0)?.classList.toggle('menu-close__audit');
    }
    if (menu) {
      menu.classList.toggle('menu-closed');
      this.isMinimalized = menu.classList.contains('menu-closed');
      this.menuService.saveMinimalizedState(this.isMinimalized);
      this.minimalized.emit(this.isMinimalized);
    }
  }

  onSelectionChange(event: any) {
    const item = event.node;
    if (item.label) {
      this.menuService.saveExpandedState(item);
    }
  }

  onNodeSelect(event: TreeNodeSelectEvent): void {
    const item = event.node as MenuItem;
    if (item.url) {
      this.router.navigateByUrl(item.url);
    }
  }

  private setMenuMinimalized(menu: ElementRef<HTMLDivElement>): void {
    if(menu) {
      menu.nativeElement.classList.add("menu-closed");
      this.minimalized.emit(true);
    }
  }
}
