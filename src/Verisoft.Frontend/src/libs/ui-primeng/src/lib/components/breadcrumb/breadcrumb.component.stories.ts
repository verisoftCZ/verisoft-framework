import { ActivatedRoute } from '@angular/router';
import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { within, expect } from '@storybook/test';
import { BreadcrumbComponent } from './breadcrumb.component';

const meta: Meta<BreadcrumbComponent> = {
  component: BreadcrumbComponent,
  title: 'BreadcrumbComponent',
  decorators: [
    moduleMetadata({
      providers: [
        {
          provide: ActivatedRoute,
          useValue: ActivatedRoute,
        },
      ],
    }),
  ],
};
export default meta;
type Story = StoryObj<BreadcrumbComponent>;
export const Primary: Story = {
  args: {
    homeRoute: '123',
    useHomeRoute: true,
    items: [
      { label: 'Contract', routerLink: '123' },
      { label: 'List', routerLink: './list' },
    ],
  },
  play: async ({ canvasElement }: any) => {
    const canvas = within(canvasElement);
    expect(canvas.getByText('Contract')).toBeTruthy();
    expect(canvas.getByText('List')).toBeTruthy();
  },
};
