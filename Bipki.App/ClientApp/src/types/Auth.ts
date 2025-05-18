export type LoginCredentials = {
  telegram: string;
  password: string;
  conferenceId?: string;
};

export type AdminLoginCredentials = {
  token: string;
};

export type RegisterCredentials = {
  name: string;
  surname: string;
  telegram: string;
  password: string;
  conferenceId?: string;
};