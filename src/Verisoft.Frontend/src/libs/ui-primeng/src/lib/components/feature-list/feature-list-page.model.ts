import { Route } from '@angular/router';
import { FEATURE_LIST_PAGE_CONFIG_PROPERTY, FeatureListPageConfig } from '@verisoft/ui-core';
import { FeatureListPageComponent } from './feature-list-page.component';

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