import { AfterViewInit, ChangeDetectorRef, Directive, inject } from "@angular/core";
import { takeUntil } from "rxjs";
import { UnsubscribeComponent } from "../../unsubscribe.component";
import { SideMenuService } from "../services/side-menu.service";
import { SIDE_MENU_COMPONENT_TOKEN } from "../side-menu.model";

@Directive({
    // eslint-disable-next-line @angular-eslint/directive-selector
    selector: 'v-side-menu[useMenuService]',
    exportAs: 'useMenuService',
    standalone: true,
})
export class MenuServiceDirective
    extends UnsubscribeComponent
    implements AfterViewInit
{
    private readonly menuService = inject(SideMenuService);
    private readonly cdr = inject(ChangeDetectorRef);
    private readonly sideMenu = inject(SIDE_MENU_COMPONENT_TOKEN);

    ngAfterViewInit() {
        this.menuService.menuItems$.pipe(takeUntil(this.destroyed$)).subscribe((items) => {

            if (this.sideMenu) {
                this.sideMenu.items = items;
            }
        });

        this.cdr.detectChanges();
    }
}