import { AsyncPipe, CommonModule } from "@angular/common";
import { 
  ChangeDetectionStrategy,
  Component,
  inject,
  Input
} from "@angular/core";
import { RouterModule } from "@angular/router";
import { GovDesignSystemModule } from "@gov-design-system-ce/angular";
import {
  HEADER_COMPONENT_TOKEN,
  HeaderCore,
  MenuItem,
  SideMenuService
} from "@verisoft/ui-core";
import { Icons } from "../../icons";

@Component({
  selector: "v-header",
  standalone: true,
  styleUrls: [
    "./header.component.scss"
  ],
  templateUrl: './header.component.html',
  imports: [
    CommonModule,
    GovDesignSystemModule,
    AsyncPipe,
    RouterModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: HEADER_COMPONENT_TOKEN,
      useExisting: HeaderComponent
    }
  ],
})
export class HeaderComponent
  implements HeaderCore 
{
  @Input() title!: string;
  @Input() userName!: string;
  @Input() logoUrl!: string;
  @Input() userRole!: any;
  @Input() menuRef!: HTMLDivElement;
  @Input() items: MenuItem[] = [];

  icons = Icons;

  protected menuVisible = false;

  tabsService = inject(SideMenuService);

  toggleMenu() {
    this.menuVisible = !this.menuVisible;
  }
}