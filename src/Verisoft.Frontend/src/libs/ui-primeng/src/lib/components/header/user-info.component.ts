import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { AvatarModule } from 'primeng/avatar';

@Component({
    selector: 'v-user-info',
    standalone: true,
    imports: [CommonModule, AvatarModule],
    template: `
        <div class="v-user-info">
            <div class="d-flex align-items-center">
                @if (userAvatar) {
                    <p-avatar [image]="userAvatar" styleClass="me-2" size="large" shape="circle" />
                }
                <div class="card align-content-center pe-3">
                    @if (userName) {
                        <p class="text-primary m-0 header-username">{{ userName }}</p>
                    }
                    @if (userRole) {
                        <p class="text-default m-0 header-userrole">{{ userRole }}</p>
                    }
                </div>
            </div>
        </div>
    `
})
export class UserInfoComponent {
    @Input() userAvatar?: string;
    @Input() userName = '';
    @Input() userRole = '';
}