import type { Meta, StoryObj } from '@storybook/angular';
import { FilterComponent } from './filter.component';

const meta: Meta<FilterComponent<any>> = {
  component: FilterComponent,
  title: 'VerisoftTableFilterComponent',
};
export default meta;
type Story = StoryObj<FilterComponent<any>>;

export const Primary: Story = {
  args: {
    filterDefinitions: [],
    debounceTime: 0,
  },
};

export const Heading: Story = {
  args: {
    filterDefinitions: [],
    debounceTime: 0,
  },
};
