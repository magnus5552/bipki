export enum Role {
    Admin = 'Admin',
    User = 'User',
    Guest = 'Guest'
}

export interface User {
    id: string;
    username: string;
    email: string;
    role: Role;
    isAuthenticated: boolean;
} 