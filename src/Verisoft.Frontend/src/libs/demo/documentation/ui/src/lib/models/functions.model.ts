import { DOCS_BLOCK_TYPE, DOCS_CODE_LANG } from './constants.model';
import {
  API_PREFIX,
  DOC_PREFIX,
  DocumentationItem,
  DocumentationNavigationItem,
} from './documentation-item.model';

export function getApiDocs(items?: DocumentationItem[]) {
  return items?.filter((x) => x.type === DOCS_BLOCK_TYPE.API_SECTION) ?? [];
}

export function getMainDocs(items?: DocumentationItem[]) {
  return items?.filter((x) => x.type === DOCS_BLOCK_TYPE.SECTION) ?? [];
}

export function getLangByFileName(fileName: string): DOCS_CODE_LANG {
  const ext = fileName?.split('.').pop()?.toLocaleLowerCase();
  switch (ext) {
    case 'ts':
      return DOCS_CODE_LANG.TS;
    case 'js':
      return DOCS_CODE_LANG.JS;
    case 'cs':
      return DOCS_CODE_LANG.CSHARP;
    case 'html':
    case 'htm':
      return DOCS_CODE_LANG.HTML;
    case 'json':
      return DOCS_CODE_LANG.HTML;
  }

  return DOCS_CODE_LANG.UNKNOWN;
}

export function getMenuItems(
  items?: DocumentationItem[]
): DocumentationNavigationItem[] {
  return (items ?? [])
    .filter(
      (x) =>
        x.type === DOCS_BLOCK_TYPE.SECTION ||
        x.type === DOCS_BLOCK_TYPE.API_ITEM
    )
    .map((x) => ({ title: x.title ?? '', url: createUrl(x) }));
}

export function createUrl(items: DocumentationItem): string {
  const prefix = items.type === DOCS_BLOCK_TYPE.SECTION ? DOC_PREFIX : API_PREFIX;
  return (
    prefix + '_' + (items?.title?.toLocaleLowerCase().replace(' ', '-') ?? '')
  );
}
