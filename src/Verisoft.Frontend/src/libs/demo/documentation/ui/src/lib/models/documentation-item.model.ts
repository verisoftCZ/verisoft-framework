import { Type } from '@angular/core';
import { DOCS_BLOCK_TYPE, DOCS_CODE_LANG } from './constants.model';

export interface DocumentationItem {
  type: DOCS_BLOCK_TYPE;
  title?: string;
  description?: string;
  text?: string;
  level?: number;
  items?: DocumentationItem[];
  code?: DocumentationCode;
  table?: DocumentationTable;
}

export interface DocumentationCode {
  componentType?: Type<unknown>;
  files: DocumentationCodeFile[];
}

export interface DocumentationCodeFile {
  language: DOCS_CODE_LANG;
  content: string;
  fileName: string;
  visible: boolean;
}

export interface IDocumentationCodeComponent {
  code?: DocumentationCode;
}

export interface DocumentationNavigationItem {
  title?: string;
  url: string;
}

export interface DocumentationTableItem {
  name?: string;
  description?: string;
  parameters?: string;
  type?: string;
  default?: string;
}

export interface DocumentationTable {
  orderItems: boolean;
  nameColumnHeader: string;
  showParameters: boolean;
  showDefault: boolean;
  showType: boolean;
  items: DocumentationTableItem[];
}

export const DOC_PREFIX = 'doc';
export const API_PREFIX = 'api';
