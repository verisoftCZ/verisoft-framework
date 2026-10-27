
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ControlSeverity, ControlSeverityType, IconsComponent } from '@verisoft/ui-core';

@Component({
  selector: 'v-icon',
  imports: [],
  templateUrl: './icon.component.html',
  styleUrl: './icon.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    '[class]': 'hostClasses',
  },
})
export class IconComponent extends IconsComponent {
  get primeIconClasses(): string {
    return ['pi', `pi-${this.name()}`].join(' ');
  }

  get hostClasses(): string {
    const classes = ['v-primeng-icon'];
    const sizeClass = this.primeSizeClass;
    const colorClass = this.primeColorClass;
    const customClass = this.class() ?? '';

    if (sizeClass) {
      classes.push(sizeClass);
    }

    if (colorClass) {
      classes.push(colorClass);
    }

    if (customClass) {
      classes.push(customClass);
    }

    return classes.join(' ');
  }

  private get primeSizeClass(): string {
    switch (this.size()) {
      case 'small':
        return 'v-primeng-icon--small';
      case 'medium':
        return 'v-primeng-icon--medium';
      case 'large':
        return 'v-primeng-icon--large';
      case 'xl':
        return 'v-primeng-icon--xl';
      default:
        return '';
    }
  }

  private get primeColorClass(): string {
    const color = this.mapSeverityToPrimeClass(this.severity());
    return color ? `v-primeng-icon--${color}` : '';
  }

  private mapSeverityToPrimeClass(severity: ControlSeverityType | undefined): string | undefined {
    if (!severity) {
      return undefined;
    }

    switch (severity) {
      case ControlSeverity.success:
        return 'success';
      case ControlSeverity.danger:
        return 'danger';
      case ControlSeverity.warning:
        return 'warning';
      case ControlSeverity.info:
        return 'info';
      case ControlSeverity.secondary:
        return 'secondary';
      case ControlSeverity.help:
      case ControlSeverity.primary:
      case ControlSeverity.contrast:
        return 'primary';
      default:
        return severity;
    }
  }
}

