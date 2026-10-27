import { EventEmitter, InjectionToken } from '@angular/core';
import { DatasourceType } from '@verisoft/core';
import { GenericFieldDefinition, GenericFieldTypeType } from '../generic-form';
import { ColumnDefinition, TableSelectionModeType } from '../table';


export interface FeatureListPageConfig {
  title: string;
  datasource: string;
  tableName: string;
  showAdd?: boolean;
  showDownload?: boolean;
  showDelete?: boolean;
}

export const FEATURE_LIST_PAGE_CONFIG_PROPERTY = 'feature_list_config';

export interface FeatureListColumnDefinition<T> extends ColumnDefinition<T>  {
  filter?: boolean;
  type?: GenericFieldTypeType;
  filterOptions?: FeatureListColumnFilterOptions;
}

export type FeatureListColumnFilterOptions = Omit<GenericFieldDefinition, 'type' | 'label' | 'name' | 'showFilter'>;

export const FEATURE_LIST_COLUMN_PROVIDER = new InjectionToken(
  'FEATURE_LIST_COLUMN_PROVIDER'
);

export const FEATURE_LIST_COMPONENT_TOKEN = new InjectionToken<FeatureListCore<any>>(
  'FeatureListComponentToken'
);

export interface FeatureListColumnProvider<T> {
  getDefinition: () => FeatureListColumnDefinition<T>;
}

export interface FeatureListCore<T> {
  autoBind: boolean;
  title: string;
  columns: FeatureListColumnDefinition<T>[];
  filters: GenericFieldDefinition[];
  maxVisibleActions: number;
  tableName: string;
  ngrxFeatureKey?: string;
  maxVisibleMobileActions: number;
  showExtendedFilter: boolean;
  showDownload: boolean;
  showDelete: boolean;
  showAdd: boolean;
  canDownload: boolean;
  canDelete: boolean;
  canAdd: boolean;
  useRouterFilter: boolean;
  fulltextFieldName: string;
  showFulltext: boolean;
  filterDialogShowFulltext: boolean;
  applyButtonLabel: string;
  clearButtonLabel: string;
  filterDetailTitle: string;
  deleteConfirmMessage: string | undefined;
  autoDeleteEnabled: boolean;
  autoDownloadEnabled: boolean;
  downloadFileName: string;
  datasource: DatasourceType<T>;
  extraFilter?: Partial<T>;
  disableCustomClicks: boolean;
  maximumColumnLength: number;
  selectionMode: TableSelectionModeType | undefined;
  pageSize: number;
  showPaginator: boolean;
  datasourceDebounceTime: number;
  addClick: EventEmitter<void>;
  downloadClick: EventEmitter<void>;
  deleteClick: EventEmitter<T[]>;
  selectionChange: EventEmitter<unknown>;
  reload(): void;
}
