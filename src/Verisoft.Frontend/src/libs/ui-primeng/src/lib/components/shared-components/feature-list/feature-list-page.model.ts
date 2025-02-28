import { InjectionToken } from '@angular/core';
import { Route } from '@angular/router';
import { ColumnDefinition } from '@verisoft/ui-core';
import { FeatureListPageComponent } from './feature-list-page.component';

export interface FeatureListPageConfig {
  title: string;
  datasource: string;
  tableName: string;
  showAdd?: boolean;
  showDownload?: boolean;
  showDelete?: boolean;
}

export const FEATURE_LIST_PAGE_CONFIG_PROPERTY = 'feature_list_config';

export function addFeatureListPage(
  value: Partial<Route> & { config: FeatureListPageConfig }
): Route {
  const { config, ...route } = value;
  return {
    component: FeatureListPageComponent,
    ...route,
    data: {
      [FEATURE_LIST_PAGE_CONFIG_PROPERTY]: config,
      ...(route.data ?? {}),
    },
  };
}

export interface FeatureListColumnDefinition<T> extends ColumnDefinition<T> {
  filter?: boolean;
}

export const FEATURE_LIST_COLUMN_PROVIDER = new InjectionToken(
  'FEATURE_LIST_COLUMN_PROVIDER'
);

export interface FeatureListColumnProvider<T> {
  getDefinition: () => FeatureListColumnDefinition<T>;
}
