import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzActionButtonGroupBasicExampleComponent, PrimengActionButtonGroupBasicExampleComponent, GovCzActionButtonGroupLabelExampleComponent, PrimengActionButtonGroupLabelExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  const docs = builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ?
            "import { ActionButtonGroupComponent, ActionButtonComponent } from '@verisoft/ui-primeng';" :
            "import { ActionButtonGroupComponent, ActionButtonComponent } from '@verisoft/ui-govcz';"
          )
      )
      .addSection((s) =>
        s
          .title('Basic Usage')
          .addText(
            `
            The ActionButtonGroup component is designed to organize and manage multiple action buttons efficiently. It automatically handles responsive behavior by showing a specified number of buttons and collapsing extras into a dropdown menu when space is limited. This component is particularly useful for toolbars, action menus, and command interfaces where multiple actions need to be presented in a space-efficient manner.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengActionButtonGroupBasicExampleComponent : GovCzActionButtonGroupBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Label Visibility')
          .addText(
            `
            By default, action buttons display only icons without labels (showLabels = false). To show labels alongside icons, you must explicitly set the showLabels property to true.
          `
          )
          .addCodeComponent(isPrimeng ? PrimengActionButtonGroupLabelExampleComponent : GovCzActionButtonGroupLabelExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Responsive Behavior')
          .addText(
            `
            The component automatically adapts to different screen sizes by moving excess buttons into a dropdown menu. You can configure the maximum number of visible buttons for desktop and mobile screens using the maxItems and maxItemsMobile properties.
          `
          )
          .addCode(`
<v-action-button-group 
  label="More actions" 
  [maxItems]="2"
  [maxItemsMobile]="1">
  <v-action-button label="Edit" [icon]="icons.edit" severity="primary" (click)="handleEdit()" />
  <v-action-button label="Delete" [icon]="icons.delete" severity="danger" (click)="handleDelete()" />
  <v-action-button label="Save" [icon]="icons.save" severity="success" (click)="handleSave()" />
  <v-action-button label="Print" [icon]="icons.print" severity="help" (click)="handlePrint()" />
</v-action-button-group>`, 'html')
      )
      .addSection((s) =>
        s
          .title('Customization')
          .addText(
            `
            Each action button within the group can be fully customized with different severities, icons, tooltips, and states. The overflow menu icon position and label can also be customized.
          `
          )
          .addCode(`
<v-action-button-group 
  label="Actions" 
  menuIconPos="left"
  [menuIcon]="icons.menu">
  <v-action-button 
    label="Edit" 
    [icon]="icons.edit" 
    severity="primary" 
    toolTip="Edit this item"
    (click)="handleEdit()" />
  <v-action-button 
    label="Archive" 
    [icon]="icons.archive" 
    severity="secondary" 
    [disabled]="isArchived"
    (click)="handleArchive()" />
</v-action-button-group>`, 'html')
      )
  );

  const apiDocs = getApiDocumentation(builder);

  return [...docs, ...apiDocs];
}

export function getApiDocumentation(
  builder: DocumentationBuilderService
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addApiSection((s) =>
        s.title('ActionButtonGroup Properties').addTable((p) =>
          p.withType().withDefaultValue().addItems([
            {
              name: 'maxItems',
              type: 'number',
              default: '3',
              description: 'Maximum number of visible buttons on desktop screens.',
            },
            {
              name: 'maxItemsMobile',
              type: 'number',
              default: '0',
              description: 'Maximum number of visible buttons on mobile screens.',
            },
            {
              name: 'items',
              type: 'ActionButton[]',
              default: '[]',
              description: 'Array of action button items to be rendered.',
            },
            {
              name: 'menuIconPos',
              type: 'IconPositionType',
              default: '"right"',
              description: 'Position of the overflow menu icon (left or right).',
            },
            {
              name: 'menuIcon',
              type: 'string',
              default: 'Icons.action',
              description: 'Icon used for the overflow menu button.',
            },
            {
              name: 'label',
              type: 'string',
              default: 'undefined',
              description: 'Label for the overflow menu button.',
            },
            {
              name: 'icon',
              type: 'string',
              default: 'undefined',
              description: 'Icon for the overflow menu button (alternative to menuIcon).',
            },
            {
              name: 'showLabels',
              type: 'boolean',
              default: 'false',
              description: 'Whether to display labels on action buttons.',
            }
          ])
        )
      )
      .addApiSection((s) =>
        s.title('ActionButton Properties').addTable((p) =>
          p.withType().withDefaultValue().addItems([
            {
              name: 'disabled',
              type: 'boolean',
              default: 'false',
              description: 'Whether the button is disabled.',
            },
            {
              name: 'toolTip',
              type: 'string',
              default: 'undefined',
              description: 'Tooltip text displayed when hovering over the button.',
            },
            {
              name: 'id',
              type: 'string',
              default: 'undefined',
              description: 'Unique identifier for the button.',
            },
            {
              name: 'icon',
              type: 'string',
              default: 'undefined',
              description: 'Icon to display in the button.',
            },
            {
              name: 'outlined',
              type: 'boolean',
              default: 'false',
              description: 'Whether to display the button with outlined style.',
            },
            {
              name: 'raised',
              type: 'boolean',
              default: 'false',
              description: 'Whether to display the button with raised style.',
            },
            {
              name: 'severity',
              type: 'ControlSeverityType',
              default: 'undefined',
              description: 'Button color variant (primary, secondary, success, info, warning, danger, help, contrast).',
            },
            {
              name: 'label',
              type: 'string',
              default: 'undefined',
              description: 'Text label displayed in the button.',
            },
            {
              name: 'size',
              type: 'FieldSizeType',
              default: 'undefined',
              description: 'Size variant of the button (small, large).',
            }
          ])
        )
      )
      .addApiSection((s) =>
        s.title('ActionButton Events').addTable((p) =>
          p.addItems([
            {
              name: 'click',
              description: 'Emitted when the button is clicked. Returns MouseEvent.',
            }
          ])
        )
      )
  );
}