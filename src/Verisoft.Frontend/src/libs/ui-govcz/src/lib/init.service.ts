import { Injectable } from '@angular/core';
import { defineCustomElements } from "@gov-design-system-ce/components/loader";

@Injectable({
    providedIn: 'root',
})
export class GovInitService {
    constructor() {
        defineCustomElements(window);
    }
}
