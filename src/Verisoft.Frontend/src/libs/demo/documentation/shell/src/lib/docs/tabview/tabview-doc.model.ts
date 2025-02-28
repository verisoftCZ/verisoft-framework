import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzTabViewBasicExampleComponent,
  GovCzTabViewModelExampleComponent,
  GovCzTabViewRouterExampleComponent,
  PrimengTabViewBasicExampleComponent,
  PrimengTabViewModelExampleComponent,
  PrimengTabViewRouterExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { TabViewModule } from '@verisoft/ui-primeng';" : "import { TabViewModule } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The TabView component is a navigation interface where content is divided into multiple sections that can be accessed through tabs. Each tab panel can contain any content, and the component provides smooth transitions between different sections. It supports both template-driven and programmatic approaches for content management.'
          )
          .addCodeComponent(isPrimeng ? PrimengTabViewBasicExampleComponent : GovCzTabViewBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Model')
          .addText(
            'The TabView definition can be loaded from model.'
          )
          .addCodeComponent(isPrimeng ? PrimengTabViewModelExampleComponent : GovCzTabViewModelExampleComponent)
      )
      .addSection((s) =>
        s
          .title('With routing')
          .addText(
            'The TabView component can be used together wit router outlet for navigation. Configure router, set useRouter attribute or TabView and define tab urls.'
          )
          .addCodeComponent(isPrimeng ? PrimengTabViewRouterExampleComponent : GovCzTabViewRouterExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Style')
          .addText(
            'Following is the list of structural style classes. Use these classes for theming.'
          )
          .addTable((p) =>
            p.addItems([
              {
                name: '.verisoft-tab-view',
                description: 'Container element of TabView component',
              },
              {
                name: '.p-tabview',
                description: 'Container element',
              },
              {
                name: '.p-tabview-nav',
                description: 'Tab navigation container',
              },
              {
                name: '.p-tabview-panel',
                description: 'Tab panel container',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'items',
              type: 'TabViewItem[]',
              default: '[]',
              description: 'Array of tab items to display',
            },
            {
              name: 'useRouting',
              type: 'boolean',
              default: 'false',
              description: 'Enable routing integration for tabs',
            },
            {
              name: 'activeIndex',
              type: 'number',
              default: '0',
              description: 'Index of the active tab',
            },
            {
              name: 'activeIndexChange',
              type: 'EventEmitter<number>',
              default: 'null',
              description: 'Event emitted when active tab changes',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}