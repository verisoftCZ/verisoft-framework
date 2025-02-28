import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { distinctUntilChanged, Subscription } from 'rxjs';
import { AuthContextService } from '../services';

@Directive({
  selector: '[hasPermission]',
})
export class HasPermissionDirective<T> implements OnInit, OnDestroy {
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

  ngOnInit(): void {
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
    if (!this.requiredPermissions) {
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
