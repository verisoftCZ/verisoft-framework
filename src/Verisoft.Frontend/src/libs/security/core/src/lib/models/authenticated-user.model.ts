export interface AuthenticatedUser {
    userId?: string;
    userName: string;
    email: string;
    displayName?: string;
    roles: string[] | undefined;
    permissions: string[] | undefined;
}
