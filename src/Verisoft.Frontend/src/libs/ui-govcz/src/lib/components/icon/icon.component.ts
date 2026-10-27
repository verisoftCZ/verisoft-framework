
import { ChangeDetectionStrategy, Component, CUSTOM_ELEMENTS_SCHEMA, input } from '@angular/core';
import { ControlSeverity, ControlSeverityType, IconLibraryType, IconsComponent } from '@verisoft/ui-core';

@Component({
  selector: 'v-icon',
  imports: [],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './icon.component.html',
  styleUrl: './icon.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class IconComponent extends IconsComponent {
  get govSize(): 's' | 'm' | 'l' | 'xl' {
    switch (this.size()) {
      case 'small':
        return 's';
      case 'large':
        return 'l';
      case 'medium':
      default:
        return 'm';
    }
  }

  get govColorClass(): string {
    const severity = this.mapSeverityToGovColor(this.severity());
    return severity ? `text-${severity}` : '';
  }

  get govClasses(): string {
    const colorClass = this.govColorClass;
    const customClass = this.class() || '';
    return `${colorClass} ${customClass}`.trim();
  }

  get bootstrapSizeClass(): string {
    switch (this.size()) {
      case 'small':
        return 'fs-6';
      case 'medium':
        return 'fs-4';
      case 'large':
        return 'fs-2';
      case 'xl':
        return 'fs-1';
      default:
        return '';
    }
  }
  
  get bootstrapClasses(): string {
    const baseClass = `bi bi-${this.name()}`;
    const sizeClass = this.bootstrapSizeClass;
    const colorClass = this.mapSeverityToBootstrapClass(this.severity());
    const customClass = this.class() || '';
    return `${baseClass} ${sizeClass} ${colorClass} ${customClass}`.trim();
  }

  private mapSeverityToGovColor(severity: ControlSeverityType | undefined): string | undefined {
    if (!severity) {
      return undefined;
    }

    switch (severity) {
      case ControlSeverity.danger:
        return 'error';
      case ControlSeverity.warning:
        return 'warning';
      case ControlSeverity.success:
        return 'success';
      case ControlSeverity.secondary:
        return 'secondary';
      case ControlSeverity.info:
        return 'neutral';
      case ControlSeverity.help:
      case ControlSeverity.primary:
      case ControlSeverity.contrast:
        return 'primary';
      default:
        return severity as string;
    }
  }

  private mapSeverityToBootstrapClass(severity: ControlSeverityType | undefined): string {
    if (!severity) {
      return '';
    }

    switch (severity) {
      case ControlSeverity.success:
        return 'text-success';
      case ControlSeverity.danger:
        return 'text-danger';
      case ControlSeverity.warning:
        return 'text-warning';
      case ControlSeverity.info:
        return 'text-info';
      case ControlSeverity.secondary:
        return 'text-secondary';
      case ControlSeverity.help:
      case ControlSeverity.primary:
      case ControlSeverity.contrast:
        return 'text-primary';
      default:
        return `text-${severity}`;
    }
  }
}

