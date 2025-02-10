import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzActionButtonGroupBasicExampleComponent, PrimengActionButtonGroupBasicExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(
            `import { ActionButtonGroupComponent } from '@verisoft/ui';
             import { ActionButtonComponent } from '@verisoft/ui';`
          )
      )
      .addSection((s) =>
        s
          .title('Overview')
          .addText(
            'The ActionButtonGroup component is designed to organize and manage multiple action buttons efficiently. It automatically handles responsive behavior by showing a specified number of buttons and collapsing extras into a dropdown menu.'
          )
      )
      .addSection((s) =>
        s
          .title('Features')
          .addText(`    
            - Responsive design with configurable breakpoints
            - Customizable number of visible buttons for desktop and mobile
            - Automatic overflow handling with dropdown menu
            - Support for various button sizes and styles
            - Right-aligned or left-aligned overflow menu
          `)
      )
      .addSection((s) =>
        s
          .title('API Documentation')
          .addText('ActionButtonGroup Component Properties:')
          .addText(`
            - @Input() maxActions: number = 3
              Maximum number of visible buttons on desktop screens
                      
            - @Input() maxActionsMobile: number = 0
              Maximum number of visible buttons on mobile screens
                      
            - @Input() menuIconPos: IconPositionType = IconPosition.right
              Position of the overflow menu icon
                      
            - @Input() icon: string = 'pi pi-ellipsis-v'
              Icon used for the overflow menu button
                      
            - @Input() label: string
              Label for the overflow menu button

            ActionButton Component Properties:
                      
            - @Input() enabled: boolean = true
              Enable/disable the button
                      
            - @Input() toolTip: string = ''
              Tooltip text for the button
                      
            - @Input() icon: string = ''
              Icon for the button
                      
            - @Input() outlined: boolean = false
              Outlined style button
                      
            - @Input() text: boolean = true
              Text style button
                      
            - @Input() severity: 'success' | 'info' | 'warning' | 'danger' | 'help' | 'primary' | 'secondary' | 'contrast'
              Button severity/color
                      
            - @Input() label: string = ''
              Button label text
                      
            - @Input() size: 'small' | 'large' | undefined
              Button size variant
                      
            - @Output() click: EventEmitter<MouseEvent>
              Emitted when button is clicked
          `)
      )
      .addSection((s) =>
        s
          .title('Usage')
          .addText('Basic usage with default configuration:')
          .addCodeComponent(isPrimeng ? PrimengActionButtonGroupBasicExampleComponent : GovCzActionButtonGroupBasicExampleComponent)
          .addText('Custom configuration with mobile breakpoints:')
          .addCode(`
            <v-action-button-group 
              label="Actions" 
              [maxActions]="2"
              [maxActionsMobile]="1"
              menuIconPos="left"
              icon="pi pi-bars">
              <v-action-button label="Action 1" />
              <v-action-button label="Action 2" />
              <v-action-button label="Action 3" />
            </v-action-button-group>`, 'html')
      )
  );
}