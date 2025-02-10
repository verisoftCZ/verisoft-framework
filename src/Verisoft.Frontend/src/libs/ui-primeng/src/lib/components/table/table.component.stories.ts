import type { Meta, StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { DEFAULT_SEARCH_LIMIT } from '@verisoft/core';
import { TableComponent } from './table.component';

const meta: Meta<TableComponent<any>> = {
  component: TableComponent,
  title: 'TableComponent',
};
export default meta;
type Story = StoryObj<TableComponent<any>>;

export const Primary: Story = {
  args: {
    tableRepository: '',
    total: 0,
    loading: false,
    scrollHeight: '100%',
    scrollable: true,
    pageSize: DEFAULT_SEARCH_LIMIT,
    currentPage: 0,
    showPaginator: true,
    sortMultiple: false,
    lazy: true,
    multipleSelect: false,
    autoIndex: false,
    showPageSizePicker: true,
    entityId: '',
    columnDefinitions: [],
  },
};

export const Heading: Story = {
  args: {
    tableRepository: '',
    total: 0,
    loading: false,
    scrollHeight: '100%',
    scrollable: true,
    pageSize: DEFAULT_SEARCH_LIMIT,
    currentPage: 0,
    showPaginator: true,
    sortMultiple: false,
    lazy: true,
    multipleSelect: false,
    autoIndex: false,
    showPageSizePicker: true,
    entityId: '',
    columnDefinitions: [],
  },
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByText(/verisoft-table works!/gi)).toBeTruthy();
  },
};
