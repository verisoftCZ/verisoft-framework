import { ChangeDetectorRef, Directive, Input, inject, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, takeUntil } from 'rxjs';
import { MenuItem } from '../side-menu';
import { UnsubscribeComponent } from '../unsubscribe.component';
import { BreadcrumbCore } from './breadcrumb.model';
import { BreadcrumbService } from './breadcrumb.service';

@Directive({})
export class BreadcrumbCoreComponent
  extends UnsubscribeComponent
  implements BreadcrumbCore, OnInit
{
  @Input() items: MenuItem[] = [];
  @Input() homeRoute = '/';
  @Input() useHomeRoute = false;

  static readonly ROUTE_DATA_BREADCRUMB = 'breadcrumb';
  static readonly ROUTE_DATA_BREADCRUMB_URL = 'breadcrumb_url';
  readonly home = { icon: 'pi pi-home', routerLink: this.homeRoute };

  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);
  breadcrumbService = inject(BreadcrumbService);
  cdr = inject(ChangeDetectorRef);

  ngOnInit(): void {
    this.initBreadcrumbsCreation();
    this.initRouteChangeListen();
  }

  private initBreadcrumbsCreation() {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(
        () => (this.items = this.createBreadcrumbs(this.activatedRoute.root))
      );
  }

  private initRouteChangeListen() {
    this.breadcrumbService.routeChange
    .pipe(
      filter((x) => x.label !== 'Undefined'),
      takeUntil(this.destroyed$)
    )
    .subscribe((x) => {
      this.items = [
        ...this.items,
        {
          label: x.label,
          routerLink: x.routerLink,
          url: x.url,
          class: 'breadcrumb',
        },
      ];
      this.cdr.detectChanges();
    });
  }

  private createBreadcrumbs(
    route: ActivatedRoute,
    routerLink = '',
    breadcrumbs: MenuItem[] = []
  ): any {
    const children: ActivatedRoute[] = route.children;

    if (children.length === 0) {
      return breadcrumbs;
    }

    for (const child of children) {
      const routeURL: string = child.snapshot.url
        .map((segment) => segment.path)
        .join('/');

      if (BreadcrumbCoreComponent.ROUTE_DATA_BREADCRUMB_URL !== undefined) {
        const route =
          child.snapshot.data[BreadcrumbCoreComponent.ROUTE_DATA_BREADCRUMB_URL];
        routerLink += `/${route}`;
      }

      if (!BreadcrumbCoreComponent.ROUTE_DATA_BREADCRUMB_URL && routeURL !== '') {
        routerLink += `/${routeURL}`;
      }

      const label = child.snapshot.data[BreadcrumbCoreComponent.ROUTE_DATA_BREADCRUMB];
      if (label && child.snapshot.routeConfig?.path !== '') {
        breadcrumbs.push({ label, routerLink: routerLink });
      }

      return this.createBreadcrumbs(child, routerLink, breadcrumbs);
    }
  }
}
