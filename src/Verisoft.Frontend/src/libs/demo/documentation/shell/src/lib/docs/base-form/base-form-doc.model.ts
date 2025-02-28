import { JsonPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { DocumentationBuilderService, DocumentationPageComponent, DocumentationItem} from '@verisoft/demo-documentation-ui';

@Component({
  template: `<v-doc-page
    title="Base Form"
    description="The BaseForm directive provides a foundation for creating form components with built-in state management, validation, and data transformation. It offers a structured approach to handling forms with automatic dirty checking, validation state management, and consistent form submission handling."
    [items]="items"
  ></v-doc-page>`,
  imports: [DocumentationPageComponent, JsonPipe],
  standalone: true,
})
export class BaseFormDocComponent {
  builder = inject(DocumentationBuilderService);
  items = getDocumentation(this.builder);
}

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? 
            `import { BaseFormDirective,
                      DetailStoreDirective,
                      BaseFormInputComponent,
                      BaseInputControls
                    } from '@verisoft/ui-primeng';` : 
            `import { 
                      BaseFormDirective,
                      DetailStoreDirective,
                      BaseFormInputComponent,
                      BaseInputControls
                    } from '@verisoft/ui-govcz';`)
      )
      .addSection((s) =>
        s
          .title('System Architecture')
          .addText(`
            The form system consists of four main components working together:
            
            1. BaseFormDirective: Core abstract class providing form management
            2. DetailStoreDirective: NgRx store integration for data persistence
            3. BaseFormInputComponent: Base class for form control components
            4. Form Models: Interfaces and utilities for form state management

            These components form a complete system for handling complex forms with:
            • Reactive form integration
            • State management
            • Backend integration
            • Input component standardization
            • Validation handling
            • Data transformation
          `)
      )
      .addSection((s) =>
        s
          .title('BaseFormDirective Deep Dive')
          .addText(`
            Core form management directive that handles:

            1. Form Lifecycle Management
               • Initialization through createFormGroup()
               • Value changes tracking
               • Status changes monitoring
               • Resource cleanup

            2. Data Transformation
               • Model to form data (fromModel)
               • Form to model data (toModel)
               • Empty string to null conversion
               • Deep object traversal

            3. State Management
               • Dirty checking
               • Validation state
               • Form submission
               • Change detection

            4. Event Handling
               • Value change events
               • Status change events
               • Submit events
               • Clear events

            The directive uses generic type T to ensure type safety throughout the form lifecycle.
          `)
      )
      .addSection((s) =>
        s
          .title('DetailStoreDirective Integration')
          .addText(`
            NgRx store integration directive that provides:

            1. Store Integration
               • Automatic store updates
               • Form state synchronization
               • Data persistence
               • Error handling

            2. Validation Integration
               • Server-side validation
               • Error message handling
               • Warning message handling
               • Control state management

            3. Lifecycle Management
               • Automatic initialization
               • Data loading
               • State cleanup
               • Change detection

            4. Configuration Options
               • Repository specification
               • Auto-start behavior
               • Detail ID handling
               • Store feature key

            The directive seamlessly integrates with BaseFormDirective through DI.
          `)
      )
      .addSection((s) =>
        s
          .title('BaseFormInputComponent System')
          .addText(`
            Base component for form controls providing:

            1. Control Integration
               • ControlValueAccessor implementation
               • NgControl integration
               • Form control creation
               • Value propagation

            2. Common Input Features
               • Labels
               • Required state
               • Readonly state
               • Tooltips
               • Display modes

            3. State Management
               • Value changes
               • Touch state
               • Validation state

            4. Customization
               • Display options
               • Clear functionality
               • Placeholder text
               • Test IDs
          `)
      )
      .addSection((s) =>
        s
          .title('Form Models and Interfaces')
          .addText(`
            Core form system interfaces and utilities:

            1. FormState Interface
               • Dirty state tracking
               • Validation state tracking
               • State comparison utilities

            2. BaseInputControls Interface
               • Common input properties
               • Type safety
               • Control integration

            3. Validation Models
               • Error handling
               • Warning handling
               • Message formatting

            4. Store Models
               • Detail state
               • Action creation
               • State updates
          `)
      )
      .addApiSection((s) =>
        s.title('BaseFormDirective API').addTable((p) =>
          p.withType().withDefaultValue().addItems([
            {
              name: 'initialData',
              type: 'T',
              default: 'undefined',
              description: 'Initial form data of type T. Used to populate form on initialization and for change tracking.'
            },
            {
              name: 'formGroup',
              type: 'FormGroup',
              default: 'undefined',
              description: 'Angular FormGroup instance. Created by createFormGroup(). Manages form controls and validation.'
            },
            {
              name: 'dataChange',
              type: 'EventEmitter<T>',
              default: 'new EventEmitter()',
              description: 'Emits when form data changes. Provides transformed model data after validation and transformation.'
            },
            {
              name: 'statusChange',
              type: 'EventEmitter<FormState>',
              default: 'new EventEmitter()',
              description: 'Emits form state changes including validation and dirty state. Used for form status tracking.'
            },
            {
              name: 'formSubmit',
              type: 'EventEmitter<T>',
              default: 'new EventEmitter()',
              description: 'Emits on form submission after validation. Provides complete transformed model data.'
            },
            {
              name: 'formClear',
              type: 'EventEmitter<void>',
              default: 'new EventEmitter()',
              description: 'Emits when form is cleared. Triggers cleanup operations.'
            },
            {
              name: 'formDestroyed$',
              type: 'Subject<void>',
              default: 'new Subject()',
              description: 'Subject for cleanup. Used to unsubscribe from observables on component destruction.'
            },
            {
              name: 'valueInitialization',
              type: 'boolean',
              default: 'false',
              description: 'Flag indicating form initialization state. Prevents unnecessary emissions during setup.'
            }
          ])
        )
      )
      .addApiSection((s) =>
        s.title('DetailStoreDirective API').addTable((p) =>
          p.withType().withDefaultValue().addItems([
            {
              name: 'form',
              type: 'BaseFormDirective<any>',
              default: 'required',
              description: 'Reference to BaseFormDirective instance. Used for form integration and state management.'
            },
            {
              name: 'detailsRepository',
              type: 'string',
              default: 'required',
              description: 'Repository identifier for store integration. Used for action creation and state selection.'
            },
            {
              name: 'autostart',
              type: 'boolean',
              default: 'true',
              description: 'Controls automatic form initialization. When true, loads data on component initialization.'
            },
            {
              name: 'detailId',
              type: 'string | number',
              default: 'undefined',
              description: 'Identifier for detail record. Used for loading specific data from backend.'
            },
            {
              name: 'ngrxFeatureKey',
              type: 'string',
              default: 'required',
              description: 'NgRx feature state key. Used for store selector creation and state management.'
            },
            {
              name: 'destroyForm',
              type: 'boolean',
              default: 'false',
              description: 'Flag indicating whether to destroy form on component destruction. Ensures cleanup.'
            },
            {
              name: 'readonly',
              type: 'boolean',
              default: 'false',
              description: 'Readonly state flag. Prevents user input while allowing form submission.'
            },
            {
              name: 'tooltip',
              description: 'Tooltip text. Provides additional information about the input.'
            },
            {
              name: 'formDisplay',
              type: "'flex' | 'block'",
              default: "'flex'",
              description: 'Display mode for form layout. Controls input component layout behavior.'
            },
            {
              name: 'clearable',
              type: 'boolean',
              default: 'true',
              description: 'Enables clear functionality. When true, allows clearing input value.'
            }
          ])
        )
      )
      .addApiSection((s) =>
        s.title('Form Models').addTable((p) =>
          p.withType().addItems([
            {
              name: 'FormState',
              type: 'interface',
              description: 'Interface defining form state with dirty and valid flags. Used for state tracking and comparison.'
            },
            {
              name: 'BaseInputControls',
              type: 'abstract class',
              description: 'Abstract class defining common input control properties and behavior. Base for input components.'
            },
            {
              name: 'DetailState',
              type: 'interface',
              description: 'Interface for store state including item data, loading state, and validation messages.'
            }
          ])
        )
      )
      .addSection((s) =>
        s
          .title('Implementation Details')
          .addText(
            `Advanced implementation features:

            1. Change Detection
               • OnPush strategy support
               • Manual change detection
               • Optimization techniques

            2. Data Transformation
               • Deep cloning
               • Immutable updates
               • Type conversion

            3. Validation
               • Sync validation
               • Async validation
               • Cross-field validation
               • Server validation

            4. Performance
               • Subscription management
               • Memory leak prevention
               • Efficient updates

            5. Error Handling
               • Validation errors
               • Runtime errors
               • Store errors

            6. Testing
               • Unit testing support
               • Integration testing
               • E2E testing`
          )
      )
  );
}