import { Injectable } from '@angular/core';
import { DocumentationBuilder, DocumentationItem } from '../models';

@Injectable({ providedIn: 'root' })
export class DocumentationBuilderService {
  createDoc(config: (x: DocumentationBuilder) => void): DocumentationItem[] {
    const builder = new DocumentationBuilder();
    config?.(builder);
    return builder.build();
  }
}
