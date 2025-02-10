import { Route } from '@angular/router';
import { addFeatureListPage } from '@verisoft/ui-primeng';

export const routes: Route[] = [
  {
    path: 'filter',
    loadChildren: () =>
      import('./lib/docs/filter/filter-doc.routes').then((m) => m.routes),
  },
  {
    path: 'action-button-group',
    data: {
      breadcrumb: 'Action Button Group',
      breadcrumb_url: 'action-button-group'
    },
    loadChildren: () =>
      import('./lib/docs/action-button-group/action-button-group-doc.routes').then(
        (m) => m.routes
      ),
  },
  {
    path: 'base-form',
    data: {
      breadcrumb: 'Base Form',
      breadcrumb_url: 'base-form'
    },
    loadChildren: () =>
      import('./lib/docs/base-form/base-form-doc.routes').then((m) => m.routes),
  },
  {
    path: 'breadcrumb',
    data: {
      breadcrumb: 'Breadcrumb',
      breadcrumb_url: 'breadcrumb'
    },
    loadChildren: () => 
    import('./lib/docs/breadcrumb/breadcrumb-doc.routes').then((x) => x.routes),
  },
  {
    path: 'button',
    data: {
      breadcrumb: 'Button',
      breadcrumb_url: 'button'
    },
    loadChildren: () =>
      import('./lib/docs/button/button-doc.routes').then((m) => m.routes),
  },
  {
    path: 'calendar',
    data: {
      breadcrumb: 'Calendar',
      breadcrumb_url: 'calendar'
    },
    loadChildren: () => 
    import('./lib/docs/calendar/calendar-doc.routes').then((x) => x.routes),
  },
  {
    path: 'checkbox',
    data: {
      breadcrumb: 'Checkbox',
      breadcrumb_url: 'checkbox'
    },
    loadChildren: () => 
    import('./lib/docs/checkbox/checkbox-doc.routes').then((x) => x.routes),
  },
  {
    path: 'confirm-dialog',
    data: {
      breadcrumb: 'Confirm Dialog',
      breadcrumb_url: 'confirm-dialog'
    },
    loadChildren: () => 
    import('./lib/docs/confirm-dialog/confirm-dialog-doc.routes').then((x) => x.routes),
  },
  {
    path: 'dropdown',
    data: {
      breadcrumb: 'Dropdown',
      breadcrumb_url: 'dropdown'
    },
    loadChildren: () => 
    import('./lib/docs/dropdown/dropdown-doc.routes').then((x) => x.routes),
  },
  {
    path: 'form-field',
    data: {
      breadcrumb: 'Form Field',
      breadcrumb_url: 'form-field'
    },
    loadChildren: () => 
    import('./lib/docs/form-field/form-field-doc.routes').then((x) => x.routes),
  },
  {
    path: 'header',
    data: {
      breadcrumb: 'Header',
      breadcrumb_url: 'header'
    },
    loadChildren: () => 
    import('./lib/docs/header/header-doc.routes').then((x) => x.routes),
  },
  {
    path: 'input-group',
    data: {
      breadcrumb: 'Input Group',
      breadcrumb_url: 'input-group'
    },
    loadChildren: () =>
      import('./lib/docs/input-group/input-group-doc.routes').then((m) => m.routes),
  },
  {
    path: 'loader',
    data: {
      breadcrumb: 'Loader',
      breadcrumb_url: 'loader'
    },
    loadChildren: () => 
      import('./lib/docs/loader/loader-doc.routes').then((x) => x.routes),
  },
  {
    path: 'multiselect',
    data: {
      breadcrumb: 'Multiselect',
      breadcrumb_url: 'multiselect'
    },
    loadChildren: () =>
      import('./lib/docs/multiselect/multiselect-doc.routes').then((m) => m.routes),
  },
  {
    path: 'number-input',
    data: {
      breadcrumb: 'Number Input',
      breadcrumb_url: 'number-input'
    },
    loadChildren: () =>
      import('./lib/docs/number-input/number-input-doc.routes').then(
        (m) => m.routes
      ),
  },
  {
    path: 'page-header',
    data: {
      breadcrumb: 'Page Header',
      breadcrumb_url: 'page-header'
    },
    loadChildren: () =>
      import('./lib/docs/page-header/page-header-doc.routes').then((m) => m.routes),
  },
  {
    path: 'password',
    data: {
      breadcrumb: 'Password',
      breadcrumb_url: 'password'
    },
    loadChildren: () => 
      import('./lib/docs/password/password-doc.routes').then((x) => x.routes),
  },
  {
    path: 'radiobutton',
    data: {
      breadcrumb: 'Radiobutton',
      breadcrumb_url: 'radiobutton'
    },
    loadChildren: () => 
      import('./lib/docs/radiobutton/radiobutton-doc.routes').then((x) => x.routes),
  },
  {
    path: 'section',
    data: {
      breadcrumb: 'Section',
      breadcrumb_url: 'section'
    },
    loadChildren: () => 
      import('./lib/docs/section/section-doc.routes').then((x) => x.routes),
  },
  {
    path: 'side-menu',
    data: {
      breadcrumb: 'Side Menu',
      breadcrumb_url: 'side-menu'
    },
    loadChildren: () => 
      import('./lib/docs/side-menu/side-menu-doc.routes').then((x) => x.routes),
  },
  {
    path: 'slider',
    data: {
      breadcrumb: 'Slider',
      breadcrumb_url: 'slider'
    },
    loadChildren: () =>
      import('./lib/docs/slider/slider-doc.routes').then((m) => m.routes),
  },
  {
    path: 'snackbar',
    data: {
      breadcrumb: 'Snackbar',
      breadcrumb_url: 'snackbar'
    },
    loadChildren: () => 
      import('./lib/docs/snackbar/snackbar-doc.routes').then((x) => x.routes),
  },
  {
    path: 'stepper',
    data: {
      breadcrumb: 'Stepper',
      breadcrumb_url: 'stepper'
    },
    loadChildren: () =>
      import('./lib/docs/stepper/stepper-doc.routes').then((m) => m.routes),
  },
  {
    path: 'switch',
    data: {
      breadcrumb: 'Switch',
      breadcrumb_url: 'switch'
    },
    loadChildren: () => 
      import('./lib/docs/switch/switch-doc.routes').then((x) => x.routes),
  },
  {
    path: 'table',
    data: {
      breadcrumb: 'Table',
      breadcrumb_url: 'table'
    },
    loadChildren: () =>
      import('./lib/docs/table/table-doc.routes').then(
        (m) => m.routes
      ),
  },
  {
    path: 'tabview',
    data: {
      breadcrumb: 'Tabview',
      breadcrumb_url: 'tabview'
    },
    loadChildren: () =>
      import('./lib/docs/tabview/tabview-doc.routes').then((m) => m.routes),
  },
  {
    path: 'text-area',
    data: {
      breadcrumb: 'Text Area',
      breadcrumb_url: 'text-area'
    },
    loadChildren: () => 
      import('./lib/docs/textarea/text-area-doc.routes').then((x) => x.routes),
  },
  {
    path: 'text-field',
    data: {
      breadcrumb: 'Text Field',
      breadcrumb_url: 'text-field'
    },
    loadChildren: () =>
      import('./lib/docs/textfield/text-field-doc.routes').then((m) => m.routes),
  },
  {
    path: 'feature-list',
    loadChildren: () =>
      import('./lib/docs/feature-list/feature-list-doc.routes').then(
        (m) => m.routes
      ),
  },
  addFeatureListPage({
    path: 'feature-list-page-example',
    config: {
      title: 'User List',
      datasource: 'v2/users',
      tableName: 'USER_LIST',
    },
  }),
  {
    path: 'tristate-checkbox',
    data: {
      breadcrumb: 'Tristate Checkbox',
      breadcrumb_url: 'tristate-checkbox'
    },
    loadChildren: () => 
      import('./lib/docs/tristate-checkbox/tristate-checkbox-doc.routes').then((x) => x.routes),
  },
  {
    path: '**',
    redirectTo: 'text-field',
  },
];
