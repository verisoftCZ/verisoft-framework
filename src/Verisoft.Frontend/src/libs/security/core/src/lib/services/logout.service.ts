import { inject, Injectable } from '@angular/core';
import { SECURITY_CONTEXT_TOKEN_PROVIDER } from '../provider';
import { AuthContextService } from './auth-context.service';
import { LocalStorageTokenProvider } from './local-storage-token-provider';

@Injectable()
export class LogoutService {
    private readonly tokenProvider = inject<LocalStorageTokenProvider>(SECURITY_CONTEXT_TOKEN_PROVIDER);
    private readonly authService = inject(AuthContextService);

    logout(): void {
        this.tokenProvider.removeToken();
        this.authService.setUser(undefined);
    }
}
