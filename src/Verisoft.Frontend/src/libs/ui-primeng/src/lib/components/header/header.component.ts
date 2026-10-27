import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { RouterModule } from '@angular/router';
import {
  HEADER_COMPONENT_TOKEN,
  HeaderCore,
  ScreenSizeService,
  UnsubscribeComponent,
} from '@verisoft/ui-core';
import { AvatarModule } from 'primeng/avatar';
import { Icons } from '../../icons';
import { ButtonComponent } from '../button';
import { HeaderProviderService } from './services/header-provider.service';
import { UserInfoComponent } from "./user-info.component";

@Component({
    selector: 'v-header',
    imports: [CommonModule, AvatarModule, ButtonComponent, RouterModule, UserInfoComponent],
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        { provide: HEADER_COMPONENT_TOKEN, useExisting: HeaderComponent },
    ]
})
export class HeaderComponent
  extends UnsubscribeComponent
  implements OnInit, HeaderCore
{
  constructor(
    readonly providerService: HeaderProviderService,
    readonly screenSizeService: ScreenSizeService,
    readonly cdr: ChangeDetectorRef
  ) {
    super();
  }

  @Input() title!: string;
  @Input() logoUrl!: string;
  @Input() userName!: string;
  @Input() userAvatar!: string;
  @Input() userRole!: any | any[] | undefined;
  @Input() menuRef!: HTMLDivElement;
  @Input() fixed = true;

  icons = Icons;
  protected menuVisible = false;
  protected logoRouterLink!: string;

  ngOnInit(): void {
    this.providerService.init();
  }

  toggleMenu() {
    if (this.menuRef) {
      this.menuVisible = !this.menuVisible;
      this.menuRef.classList.toggle('menu-visible');
      this.cdr.detectChanges();
    }
  }
}
