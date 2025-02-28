import { Injectable } from '@angular/core';
import { defineCustomElements } from "@gov-design-system-ce/components/loader";

@Injectable({
    providedIn: 'root',
})
export class GovInitService {
    /* the rest of settings is set in `config.d.ts` */
    constructor() {
        defineCustomElements(window);
    }
}