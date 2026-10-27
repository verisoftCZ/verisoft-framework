import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  ControlSeverity,
  ControlSeverityType,
  FieldSize,
  FieldSizeType,
  TagCore,
  TAG_COMPONENT_TOKEN,
  TagVariant,
} from '@verisoft/ui-core';
import { TagModule } from 'primeng/tag';
import { convertToMessagePrimeSeverity, PrimeMessageSeverity } from '../../models';

@Component({
  selector: 'v-tag',
  imports: [CommonModule, TagModule, TranslateModule],
  templateUrl: './tag.component.html',
  styleUrl: './tag.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: TAG_COMPONENT_TOKEN,
      useExisting: TagComponent,
    },
  ],
})
export class TagComponent implements TagCore {
  label = input<string | undefined>();
  severity = input<ControlSeverityType | undefined>(ControlSeverity.primary);
  variant = input<TagVariant>('bold');
  size = input<FieldSizeType>(FieldSize.medium);

  protected readonly primeSeverity = computed<PrimeMessageSeverity>(() => {
    const severity = this.severity();
    if (!severity) {
      return null;
    }

    return convertToMessagePrimeSeverity(severity);
  });

  protected readonly tagClasses = computed(() => {
    const classes: string[] = ['v-primeng-tag'];
    const severity = this.severity();
    const variant = this.variant();
    const size = this.size();

    if (severity) {
      classes.push(`v-primeng-tag--${severity}`);
    }

    if (variant === 'subtle') {
      classes.push('v-primeng-tag--subtle');
    }

    if (size) {
      classes.push(`v-primeng-tag--${size}`);
    }

    return classes.join(' ');
  });

  protected readonly rounded = computed(() => this.variant() === 'subtle');
}

