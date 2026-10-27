
export interface SecurityConfig {
    tokenStorageKey: string;
    contextTokenStorageKey?: string;
    loginPage?: string;
    logoutPage?: string;
    notAuthorizedPage?: string;
    sendTokenHeader?: boolean;
}
