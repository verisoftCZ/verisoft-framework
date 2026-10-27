import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { convertJWTToUser, SecurityConfig } from '../models';
import { SECURITY_CONFIG, SECURITY_CONTEXT_TOKEN_PROVIDER } from '../provider';
import { AuthContextService } from './auth-context.service';
import { LocalStorageTokenProvider } from './local-storage-token-provider';

@Injectable()
export class LoginService {
    private config = inject<SecurityConfig>(SECURITY_CONFIG);
    private tokenProvider = inject<LocalStorageTokenProvider>(SECURITY_CONTEXT_TOKEN_PROVIDER);
    private authService = inject(AuthContextService);
    private router = inject(Router);

    login(token?: string): void {
        if (token) {
            this.tokenProvider.setToken(token);
            this.authService.setUser(convertJWTToUser(token));
        } else {
            this.router.navigate([this.config.loginPage]);
        }
    }
}
