import type { Meta, StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { HeaderComponent } from './header.component';

const meta: Meta<HeaderComponent> = {
  component: HeaderComponent,
  title: 'HeaderComponent',
};
export default meta;
type Story = StoryObj<HeaderComponent>;

export const Primary: Story = {
  args: {
    title: 'verisoft',
    userName: 'Elon Musk',
    userRole: 'Mars Owner',
  },
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByText('verisoft')).toBeTruthy();
    expect(canvas.findByText('Elon Musk')).toBeTruthy();
    expect(canvas.findByText('Mars Owner')).toBeTruthy();
  },
};
