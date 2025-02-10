import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  PrimengSectionBasicExampleComponent,
  PrimengSectionAdvancedExampleComponent,
  GovCzSectionBasicExampleComponent,
  GovCzSectionAdvancedExampleComponent
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode("import { SectionComponent } from '@verisoft/ui';")
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Section angular component is a custom wrapper around elements designed to hide it. It was created for flexible integration in both template-driven and reactive forms. The component automatically adjusts to fill 100% of the width of its parent container, ensuring responsive and seamless layout adaptability.'
          )
          .addCodeComponent(isPrimeng ? PrimengSectionBasicExampleComponent : GovCzSectionBasicExampleComponent)
      )
      .addSection((s) =>
        s
          .title('Advanced')
          .addText(
            'The Section component supports hiding its content by default and adjusting the part of the header to a set background colour.'
          )
          .addCodeComponent(isPrimeng ? PrimengSectionAdvancedExampleComponent : GovCzSectionAdvancedExampleComponent)
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
                name: '.v-section',
                description: 'The container of the Section component.',
              },
              {
                name: '.v-section-header',
                description: 'The header container of the Section component.',
              },
              {
                name: '.v-section-content',
                description: 'The content of the component under its header.',
              },
            ])
          )
      )
      .addApiSection((s) => 
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'title',
              type: 'string',
              default: 'undefined',
              description: 'The title of the Section component.',
            },
            {
              name: 'showContent',
              type: 'boolean',
              default: 'true',
              description: 'If the element should hide its content.',
            },
            {
              name: 'backgroundColor',
              type: 'string',
              default: 'undefined',
              description: 'The background colour of the header part of the element, should be specified in a CSS style.',
            },
          ])
        )
      )
      .addApiSection((s) => s.title('Events').addTable((p) => p.addItems([])))
  );
}
