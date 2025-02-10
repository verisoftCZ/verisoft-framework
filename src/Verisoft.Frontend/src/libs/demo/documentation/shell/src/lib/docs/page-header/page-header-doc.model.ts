import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import { GovCzPageHeaderBasicExampleComponent, PrimengPageHeaderBasicExampleComponent } from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { PageHeaderComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The PageHeader component provides a consistent header for pages, including a title, subtitle, and optional action buttons. It helps in maintaining a uniform look and feel across different sections of the application.'
          )
          .addCodeComponent(isPrimeng ? PrimengPageHeaderBasicExampleComponent : GovCzPageHeaderBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Service Usage')
          .addText(
            'The PageHeaderService can be used to update the header dynamically from any component:'
          )
          .addCode(`
            // Inject the service
            constructor(private headerService: PageHeaderService) {}

            // Update the header
            this.headerService.pageHeader.emit({
              title: 'New Title',
              subtitle: 'New Subtitle',
              showBackButton: true
            });`, 'typescript')
      )
      .addSection((s) =>
        s
          .title('API')
          .addText('The PageHeader component exposes the following inputs:')
          .addText(`
            - \`@Input() title: string;\`: The title of the page header.
            - \`@Input() subtitle?: string;\`: The subtitle of the page header.
            - \`@Input() showBackButton = false;\`: Whether to show the back button.
          `)
      )
  );
}