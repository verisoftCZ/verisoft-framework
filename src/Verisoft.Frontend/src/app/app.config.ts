import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
} from '@angular/common/http';
import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {
  provideRouter,
  withEnabledBlockingInitialNavigation,
} from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { provideStore, StoreModule } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import {
  provideTranslateService,
  TranslateService,
} from '@ngx-translate/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { BASE_URL_PATH, ERROR_PROVIDER_TOKEN, LocalizableErrorProviderService, LocalStorageService } from '@verisoft/core';
import { SecurityModule } from '@verisoft/security-core';
import { MenuItem, LOGO_ROUTER_ROUTE, SETTINGS_MENU } from '@verisoft/ui-core';
import {
  SnackbarService,
  SideMenuModule,
  HttpErrorMessageInterceptor,
  VerisoftTheme,
} from '@verisoft/ui-primeng';
import { MessageService } from 'primeng/api';
import { providePrimeNG } from 'primeng/config';
import { environment } from '../environments/environment.prod';
import { appRoutes } from './app.routes';

export function getMenuItems(isPrimeng: boolean): MenuItem[] {
  const uiLibraryPath = isPrimeng ? 'primeng' : 'govcz';
  return [
    {
      label: 'Dokumentace',
      icon: 'pi pi-cog',
      url: 'docs',
      children: [
        {
          label: 'Action button group',
          url: '/' + uiLibraryPath + '/docs/action-button-group',
        },
        { label: 'Base form', url: '/' + uiLibraryPath + '/docs/base-form' },
        { label: 'Breadcrumb', url: '/' + uiLibraryPath + '/docs/breadcrumb' },
        { label: 'Button', url: '/' + uiLibraryPath + '/docs/button' },
        { label: 'Calendar', url: '/' + uiLibraryPath + '/docs/calendar' },
        { label: 'Checkbox', url: '/' + uiLibraryPath + '/docs/checkbox' },
        {
          label: 'Confirm dialog',
          url: '/' + uiLibraryPath + '/docs/confirm-dialog',
        },
        { label: 'Dropdown', url: '/' + uiLibraryPath + '/docs/dropdown' },
        { label: 'Dropdown Button', url: '/' + uiLibraryPath + '/docs/dropdown-button' },
        { label: 'Form field', url: '/' + uiLibraryPath + '/docs/form-field' },
        {
          label: 'Feature List',
          url: '/' + uiLibraryPath + '/docs/feature-list',
        },
        { label: 'Filter', url: '/' + uiLibraryPath + '/docs/filter' },
        { label: 'Header', url: '/' + uiLibraryPath + '/docs/header' },
        { label: 'Icon', url: '/' + uiLibraryPath + '/docs/icon' },
        {
          label: 'Input group',
          url: '/' + uiLibraryPath + '/docs/input-group',
        },
        { label: 'Loader', url: '/' + uiLibraryPath + '/docs/loader' },
        {
          label: 'Multiselect',
          url: '/' + uiLibraryPath + '/docs/multiselect',
        },
        {
          label: 'Number input',
          url: '/' + uiLibraryPath + '/docs/number-input',
        },
        { label: 'Password', url: '/' + uiLibraryPath + '/docs/password' },
        {
          label: 'Radiobutton',
          url: '/' + uiLibraryPath + '/docs/radiobutton',
        },
        { label: 'Section', url: '/' + uiLibraryPath + '/docs/section' },
        { label: 'Side menu', url: '/' + uiLibraryPath + '/docs/side-menu' },
        { label: 'Slider', url: '/' + uiLibraryPath + '/docs/slider' },
        { label: 'Snackbar', url: '/' + uiLibraryPath + '/docs/snackbar' },
        { label: 'Stepper', url: '/' + uiLibraryPath + '/docs/stepper' },
        { label: 'Switch', url: '/' + uiLibraryPath + '/docs/switch' },
        { label: 'Table', url: '/' + uiLibraryPath + '/docs/table' },
        { label: 'TabView', url: '/' + uiLibraryPath + '/docs/tabview' },
        { label: 'Tag', url: '/' + uiLibraryPath + '/docs/tag' },
        { label: 'TextArea', url: '/' + uiLibraryPath + '/docs/text-area' },
        { label: 'TextField', url: '/' + uiLibraryPath + '/docs/text-field' },
      ],
    },
  ];
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideStore(),
    provideStoreDevtools(),
    provideHttpClient(),
    {
      provide: ERROR_PROVIDER_TOKEN,
      useClass: LocalizableErrorProviderService
    },
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: VerisoftTheme,
        options: {
          prefix: 'p',
          darkModeSelector: 'system',
          cssLayer: false
        }
      },
      ripple: true
    }),
    provideRouter(appRoutes, withEnabledBlockingInitialNavigation()),
    provideTranslateService({
      loader: provideTranslateHttpLoader({
        prefix: '/assets/i18n/',
        suffix: '.json'
      }),
      fallbackLang: 'cs',
      lang: 'cs'
    }),
    importProvidersFrom(
      SecurityModule.forRoot({
        sendTokenHeader: true,
      }),
      StoreModule.forRoot({}),
      SideMenuModule.forRoot(),
      EffectsModule.forRoot([]),
    ),
    MessageService,
    SnackbarService,
    TranslateService,
    LocalStorageService,
    {
      provide: BASE_URL_PATH,
      useValue: environment.apiUrl,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorMessageInterceptor,
    },
    {
      provide: SETTINGS_MENU,
      useValue: [{ key: 'SETTINGS_MENU' }],
    },
    {
      provide: LOGO_ROUTER_ROUTE,
      useValue: [{ key: 'LOGO_ROUTER_ROUTE' }],
    },
  ],
};
