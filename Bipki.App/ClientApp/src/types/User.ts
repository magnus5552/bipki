export enum Role {
    Admin = 'Admin',
    User = 'User',
}

export interface User {
    id: string;
    username: string;
    telegram: string;
    role: Role;
    isAuthenticated: boolean;
} 