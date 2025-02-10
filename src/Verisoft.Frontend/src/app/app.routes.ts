import { Route } from '@angular/router';
import { GovCzAppComponent } from './GovCzAppComponent';
import { PrimengAppComponent } from './PrimengAppComponent';

export const appRoutes: Route[] = [
  {
    path: 'primeng',
    component: PrimengAppComponent,
    data: {
      breadcrumb: 'Primeng',
      breadcrumb_url: 'primeng',
    },
    children: [
      {
        path: 'docs',
        data: {
          breadcrumb: 'Documentation',
          breadcrumb_url: 'docs'
        },
        loadChildren: () =>
          import('@verisoft/demo-documentation-shell').then((m) => m.routes),
      },
      {
        path: '**',
        redirectTo: 'docs',
      },
    ]
  },
  {
    path: 'govcz',
    component: GovCzAppComponent,
    data: {
      breadcrumb: 'GovCz',
      breadcrumb_url: 'govcz'
    },
    children: [
      {
        path: 'docs',
        data: {
          breadcrumb: 'Documentation',
          breadcrumb_url: 'docs'
        },
        loadChildren: () =>
          import('@verisoft/demo-documentation-shell').then((m) => m.routes),
      },
      {
        path: '**',
        redirectTo: 'docs',
      },
    ]
  },
  {
    path: '**',
    redirectTo: 'primeng',
  },
];
