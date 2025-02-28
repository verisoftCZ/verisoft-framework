import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzConfirmDialogBasicExampleComponent, PrimengConfirmDialogBasicExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(isPrimeng ? "import { DialogService } from '@verisoft/ui-primeng';" : "import { DialogService } from '@verisoft/ui-govcz';")
      )
      .addSection((s) =>
        {
          return s
            .title('Basic')
            .addText(
              'The Dialog confirm angular component is custom-made wrapper around ng-prime\'s p-dialog. It supports customization through data input in its Dialog service, which handles the initial logic via injection. It allows to add a title and an icon, the severity of the message, the body of the Dialog, which can utilize HTML markdown. Finally, it supports adding custom functions for both the accept and cancel buttons, ensuring responsive and seamless user experience.'
            )
         //   .addCodeComponent(isPrimeng ? PrimengConfirmDialogBasicExampleComponent : GovCzConfirmDialogBasicExampleComponent);
        }
      )
      .addSection((s) =>
        s
          .title('Style')
          .addText(
            'Following is the list of structural style classes. Use this classes for theming.'
          )
          .addTable((p) =>
            p.addItems([
              {
                name: '.v-dialog',
                description: 'The container of the Confirm dialog component.',
              },
              {
                name: '.v-dialog-header',
                description: 'The header of the Confirm dialog component.',
              },
              {
                name: '.v-dialog-content',
                description: 'The body of the Confirm dialog component.',
              },
              {
                name: '.v-dialog-footer',
                description: 'The footer of the Confirm dialog component.',
              },
            ])
          )
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'data',
              type: 'DialogData',
              default: 'undefined',
              description: 'The input data for the Confirm dialog service, which in hand uses the Confirm dialog component.',
            },
          ])
        )
      )
      .addApiSection(
        (s) => s.title('Events')
          .addTable(
            (p) => p.addItems([
              {
                name: 'showEvent',
                type: 'EventEmitter<DialogData>',
                default: 'undefined',
                description: 'Emits when the Side menu is minimalized or maximalized back.'
              }
            ])
          )
      )
  );
}
