import { InjectionToken } from "@angular/core";

export interface DocumentationCofig {
    isPrimeng: boolean
}

export const DOCS_CONFIG = new InjectionToken<DocumentationCofig>('DOCS_CONFIG');