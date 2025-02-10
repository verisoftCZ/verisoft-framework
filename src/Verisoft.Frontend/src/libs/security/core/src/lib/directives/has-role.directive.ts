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
  selector: '[hasRole]',
})
export class HasRoleDirective<T> implements OnInit, OnDestroy {
  private requiredRoles: string | string[] | undefined;
  private sub?: Subscription;

  constructor(
    private templateRef: TemplateRef<T>,
    private viewContainer: ViewContainerRef,
    private authContext: AuthContextService
  ) {}

  @Input()
  set hasRole(value: string | string[]) {
    this.requiredRoles = value;
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
    if (this.requiredRoles) {
      this.sub = this.authContext
        .hasRequiredRole(this.requiredRoles)
        .pipe(distinctUntilChanged())
        .subscribe((hasRole) => {
          if (hasRole) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          } else {
            this.viewContainer.clear();
          }
        });
    }
  }
}
