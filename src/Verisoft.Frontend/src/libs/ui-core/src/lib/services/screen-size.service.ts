import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ScreenSizeService {
  isMobileBlock = new BehaviorSubject<boolean>(false);
  prevState = false;

  constructor() {
    this.checkScreenSize();
    window.addEventListener('resize', async () => {
      await this.checkScreenSize();
    });
  }

  async checkScreenSize() {
    const isMobile = window.matchMedia('(max-width: 768px)').matches;
    if (isMobile !== this.prevState) {
      this.prevState = isMobile;
      this.isMobileBlock.next(isMobile);
    }
  }
}
