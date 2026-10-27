import { ActivatedRoute, Route, Router } from '@angular/router';
import { RowModel } from './table.models';

export function hasRowDetailRoute(route: ActivatedRoute, id: string): boolean {
  const config = route.routeConfig;
  const candidates: Route[] = [...(config?.children ?? [])];

  if (!config?.path) {
    candidates.push(...(route.parent?.routeConfig?.children ?? []));
  }

  return candidates.some(
    (x) =>
      x.path === id.split('/')[0] ||
      x.path?.startsWith(':') ||
      (x.path === '' && !!x.loadChildren)
  );
}

export function createRowDetailUrl<T>(row: RowModel<T>, entityKey?: string): string {
  if (row.customRoute) {
    return row.customRoute;
  }

  const key = (row.row as Record<string, unknown>)[entityKey ?? 'id'];
  return String(key ?? row.id);
}

export function navigateToRowDetail<T>(
  router: Router,
  route: ActivatedRoute,
  row: RowModel<T>,
  entityKey?: string,
  event?: MouseEvent
): void {
  const targetUrl = createRowDetailUrl(row, entityKey);

  if (event?.ctrlKey || event?.metaKey) {
    openRowDetailInNewWindow(router, route, row, entityKey);
    return;
  }

  router.navigate([targetUrl], { relativeTo: route });
}

export function openRowDetailInNewWindow<T>(
  router: Router,
  route: ActivatedRoute,
  row: RowModel<T>,
  entityKey?: string
): void {
  const url = router.serializeUrl(
    router.createUrlTree([createRowDetailUrl(row, entityKey)], { relativeTo: route })
  );

  window.open(url, '_blank');
}
