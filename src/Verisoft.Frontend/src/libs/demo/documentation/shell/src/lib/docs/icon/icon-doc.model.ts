import {
  DocumentationBuilderService,
  DocumentationItem,
} from '@verisoft/demo-documentation-ui';
import {
  GovCzIconBasicExampleComponent,
  GovCzIconSizeExampleComponent,
  PrimengIconBasicExampleComponent,
  PrimengIconSizeExampleComponent,
} from './examples';

export function getDocumentation(
  builder: DocumentationBuilderService,
  isPrimeng?: boolean
): DocumentationItem[] {
  const basicExample = isPrimeng
    ? PrimengIconBasicExampleComponent
    : GovCzIconBasicExampleComponent;

  const sizeExample = isPrimeng
    ? PrimengIconSizeExampleComponent
    : GovCzIconSizeExampleComponent;

  const nativeLibraryLabel = isPrimeng
    ? 'PrimeIcons'
    : 'Gov Design System icons';

  const importStatement = isPrimeng
    ? "import { IconComponent } from '@verisoft/ui-primeng';"
    : "import { IconComponent } from '@verisoft/ui-govcz';";

  const librariesDescription = isPrimeng
    ? 'The Icon component supports two icon libraries: PrimeIcons (library="native") and Bootstrap Icons (library="bootstrap"). PrimeIcons are included with PrimeNG, while Bootstrap Icons use the Bootstrap Icons font. You can switch between libraries by changing the library property. Each library has its own set of icon names - refer to the respective documentation for available icons.'
    : 'The Icon component supports two icon libraries: Gov Design System icons (library="native") and Bootstrap Icons (library="bootstrap"). The Gov icons are provided by the @gov-design-system-ce package, while Bootstrap icons use the Bootstrap Icons font. You can switch between libraries by changing the library property. Each library has its own set of icon names - refer to the respective documentation for available icons.';

  const styleDescription = isPrimeng
    ? 'The Icon component renders PrimeIcons using i elements with "pi" classes, or Bootstrap Icons using "bi" classes. You can customise the appearance through the size and severity inputs, or provide additional CSS classes with the class input.'
    : 'The Icon component automatically adapts its styling based on the selected library and properties. Gov icons use the gov-icon web component, while Bootstrap icons use the i element with Bootstrap Icon classes. You can customize the appearance using the size and severity properties, or add custom CSS classes using the class input.';

  const basicDescription = `The Icon component is a versatile and customizable component that can be used to display icons from multiple icon libraries in an Angular application. It supports both ${nativeLibraryLabel} (library="native") and Bootstrap Icons (library="bootstrap"), allowing developers to choose the icon library that best fits their needs. The Icon component can be easily integrated into buttons, forms, navigation menus, and other components, providing a consistent and cohesive user interface. It supports various sizes and severities, making it adaptable to different design requirements.`;

  return builder.createDoc((x) =>
    x
      .addSection((s) =>
        s
          .title('Import')
          .addCode(importStatement)
      )
      .addSection((s) =>
        s
          .title('Basic')
          .addText(basicDescription)
          .addCodeComponent(basicExample)
      )
      .addSection((s) =>
        s
          .title('Size')
          .addText(
            'The Icon component supports different sizes: small, medium (default), large, and xl. The size property allows you to control the visual size of the icon to match your design requirements. Both native and Bootstrap icon libraries support all size variants.'
          )
          .addCodeComponent(sizeExample)
      )
      .addSection((s) =>
        s
          .title('Icon Libraries')
          .addText(librariesDescription)
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
              name: 'name',
              type: 'string',
              default: "''",
              description:
                'The name of the icon to display. Icon names vary by library - use native icon names for library="native" and Bootstrap icon names for library="bootstrap".',
            },
            {
              name: 'library',
              type: "'native' | 'bootstrap'",
              default: "'native'",
              description: `The icon library to use. Choose between ${nativeLibraryLabel} or Bootstrap Icons.`,
            },
            {
              name: 'size',
              type: "'small' | 'medium' | 'large' | 'xl' | undefined",
              default: 'undefined',
              description: 'The size of the icon. Controls the visual size of the icon.',
            },
            {
              name: 'severity',
              type: 'ControlSeverityType | undefined',
              default: 'undefined',
              description: 'Severity of the icon. Translated to the corresponding design-system colours.',
            },
            {
              name: 'class',
              type: 'string | undefined',
              default: 'undefined',
              description: 'Additional CSS classes to apply to the icon.',
            },
          ])
        )
      )
  );
}

