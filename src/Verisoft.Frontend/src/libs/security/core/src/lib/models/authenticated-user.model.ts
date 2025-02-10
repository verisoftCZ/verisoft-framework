export interface AuthenticatedUser {
    userName: string;
    email: string;
    displayName: string;
    roles: string[] | undefined;
    permissions: string[] | undefined;
}