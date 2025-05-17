export type LoginCredentials = {
  telegram: string;
  password: string;
  conferenceId?: string;
};

export type AdminLoginCredentials = {
  token: string;
};

export type RegisterCredentials = {
  username: string;
  telegram: string;
  password: string;
  confirmPassword: string;
  conferenceId?: string;
};