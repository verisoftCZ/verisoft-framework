import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzTagBasicExampleComponent,
  GovCzTagTypeExampleComponent,
  PrimengTagBasicExampleComponent,
  PrimengTagVariantExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService, isPrimeng?: boolean
): DocumentationItem[] {
  const basicExample = isPrimeng
    ? PrimengTagBasicExampleComponent
    : GovCzTagBasicExampleComponent;

  const variantExample = isPrimeng
    ? PrimengTagVariantExampleComponent
    : GovCzTagTypeExampleComponent;

  const styleDescription = isPrimeng
    ? 'The Tag component wraps the PrimeNG Tag component. You can customise it further by passing your own CSS classes or using PrimeNG utility classes. The component automatically adapts to the selected severity, variant, and size properties.'
    : 'The Tag component uses the gov-tag web component from @gov-design-system-ce. For custom styling, you can wrap the component in a container with your own CSS classes or use inline styles. The component automatically adapts to the selected severity, variant, and size properties.';

  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(
            isPrimeng
              ? "import { TagComponent } from '@verisoft/ui-primeng';"
              : "import { TagComponent } from '@verisoft/ui-govcz';"
          )
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(
            'The Tag component is a versatile and customizable component that can be used to display labels, statuses, categories, and other metadata in an Angular application. It supports various severities and sizes, allowing developers to create tags that match the design requirements of their application. The Tag component can be easily integrated into cards, lists, tables, and other components, providing a consistent and cohesive user interface. It automatically supports translation for labels and can display custom content through ng-content projection.'
          )
          .addCodeComponent(basicExample)
      )
      .addSection((s) =>
        s
          .title('Variants')
          .addText(
            'The tag component supports different variants: bold (default) and subtle. Bold variant displays the tag with strong emphasis, while the subtle variant provides a softer, less prominent appearance.'
          )
          .addCodeComponent(variantExample)
      )
      .addSection((s) =>
        s
          .title('Style')
          .addText(styleDescription)
      )
      .addApiSection((s) =>
        s.title('Properties').addTable((p) =>
          p.withType().addItems([
            {
              name: 'label',
              type: 'string | undefined',
              default: 'undefined',
              description: 'The label of the tag component. Will be automatically translated.',
            },
            {
              name: 'severity',
              type: 'ControlSeverityType | undefined',
              default: 'ControlSeverity.primary',
              description: 'Severity of the tag component. Maps to the underlying design system colours.',
            },
            {
              name: 'variant',
              type: '\'bold\' | \'subtle\'',
              default: '\'bold\'',
              description: 'The visual type of the tag component. Bold for strong emphasis, subtle for softer appearance.',
            },
            {
              name: 'size',
              type: '\'small\' | \'medium\' | \'large\'',
              default: '\'medium\'',
              description: 'The size of the tag component.',
            },
          ])
        )
      )
  );
}

