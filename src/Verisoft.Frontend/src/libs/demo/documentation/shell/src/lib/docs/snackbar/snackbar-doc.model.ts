import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzSnackbarAdvancedExampleComponent,
  GovCzSnackbarBasicExampleComponent,
  PrimengSnackbarAdvancedExampleComponent,
  PrimengSnackbarBasicExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(
            isPrimeng ? "import { SnackbarComponent } from '@verisoft/ui-primeng';" : "import { SnackbarComponent } from '@verisoft/ui-govcz';"
          )
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Snackbar angular component is a customizable toast element designed for flexible integration anywhere. The element is rendered on top of the existing layout, ensuring seamless layout adaptability. It includes 4 styles of the toast - success, info, warning and danger.'
          )
       //   .addCodeComponent(isPrimeng ? PrimengSnackbarBasicExampleComponent : GovCzSnackbarBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Custom messages and icons')
          .addText(
            'This element also supports custom messages alongside setting icons into the toast, as shown in the example below.'
          )
      //    .addCodeComponent(isPrimeng ? PrimengSnackbarAdvancedExampleComponent : GovCzSnackbarAdvancedExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Style')
          .addText(
            'Following is the list of structural style classes. Use this class for theming.'
          )
          .addTable((p) =>
            p.addItems([
              {
                name: '.p-toast',
                description: 'The container of Snackbar component.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'type',
              type: 'success | info | warn | error',
              default: 'N/A',
              description: 'What type of toast should appear.',
            },
            {
              name: 'message',
              type: 'string',
              default: '""',
              description: 'The text of the toast element.',
            },
            {
              name: 'icon',
              type: 'string',
              default: 'undefined',
              description: 'What prime-ng icon should appear next to the message.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
