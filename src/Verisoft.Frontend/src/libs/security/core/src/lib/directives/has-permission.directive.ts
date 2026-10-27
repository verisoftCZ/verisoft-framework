import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  OnDestroy,
} from '@angular/core';
import { distinctUntilChanged, Subscription } from 'rxjs';
import { AuthContextService } from '../services';

@Directive({
  standalone: true,
  selector: '[hasPermission]',
})
export class HasPermissionDirective<T> implements OnDestroy {
  private requiredPermissions: string | string[] = '';
  private sub?: Subscription;

  constructor(
    private templateRef: TemplateRef<T>,
    private viewContainer: ViewContainerRef,
    private authContext: AuthContextService
  ) {}

  @Input()
  set hasPermission(value: string | string[]) {
    this.requiredPermissions = value;
    this.updateView();
  }

  ngOnDestroy(): void {
    this.unregister();
  }

  private unregister() {
    this.sub?.unsubscribe();
  }

  private updateView(): void {
    this.unregister();
    if (this.requiredPermissions || this.requiredPermissions.length) {
      this.sub = this.authContext
        .hasRequiredPermission(this.requiredPermissions)
        .pipe(distinctUntilChanged())
        .subscribe((hasPerm) => {
          if (hasPerm) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          } else {
            this.viewContainer.clear();
          }
        });
    }
  }
}
