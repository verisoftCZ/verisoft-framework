import { AsyncPipe, CommonModule } from "@angular/common";
import {
  ChangeDetectionStrategy,
  Component,
  ContentChild,
  inject,
  Input,
  TemplateRef,
} from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import { HasPermissionDirective } from "@verisoft/security-core";
import {
  HEADER_COMPONENT_TOKEN,
  HeaderCore,
  MenuItem,
  SideMenuService,
} from "@verisoft/ui-core";
import { Icons } from "../../icons";

@Component({
    selector: "v-header",
    styleUrl: "./header.component.scss",
    templateUrl: './header.component.html',
    imports: [
        CommonModule,
        GovDesignSystemModule,
        AsyncPipe,
        RouterModule,
        HasPermissionDirective,
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: HEADER_COMPONENT_TOKEN,
            useExisting: HeaderComponent
        }
    ]
})
export class HeaderComponent
  implements HeaderCore {

  @Input() title!: string;

  @Input() userName!: string;

  @Input() userAvatar!: string;

  @Input() logoUrl!: string;

  @Input() userRole!: any;

  @Input() menuRef!: HTMLDivElement;

  @Input() items: MenuItem[] = [];

  @Input() fixed = true;

  @ContentChild('action', { static: false })
  actionTemplate!: TemplateRef<{ $implicit: any }>;

  protected icons = Icons;
  protected menuVisible = false;
  private openedIndexes: number[] = [];

  protected readonly tabsService = inject(SideMenuService);
  private readonly router = inject(Router);

  toggleMenu() {
    this.menuVisible = !this.menuVisible;
  }

  protected canRedirect(url?: string, canRedirect?: boolean) {
    if (url && canRedirect) {
      this.router.navigateByUrl(url);
    }
  }

  protected showOrHide(index: number, children?: number): void {
    if (children === 0) {
      return;
    }

    if (!this.openedIndexes.includes(index)) {
      this.openedIndexes = [...[], index];
    } else if (this.openedIndexes.includes(index)) {
      this.openedIndexes = this.openedIndexes.filter(x => x !== index);
    }
  }

  protected canBeShown(index: number) {
    return this.openedIndexes.includes(index);
  }
}
