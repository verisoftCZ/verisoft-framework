import type { Meta, StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { TristatecheckboxComponent } from './tristatecheckbox.component';

const meta: Meta<TristatecheckboxComponent> = {
  component: TristatecheckboxComponent,
  title: 'VerisoftTristatecheckboxComponent',
};
export default meta;
type Story = StoryObj<TristatecheckboxComponent>;

export const Primary: Story = {
  args: {},
};

export const Heading: Story = {
  args: {},
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByText(/verisoft-tristatecheckbox works!/gi)).toBeTruthy();
  },
};
