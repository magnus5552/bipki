import { api } from "./api";
import { User } from "types/User";
import {
  LoginCredentials,
  RegisterCredentials,
  AdminLoginCredentials,
} from "types/Auth";

export async function getUser(): Promise<User> {
  const { data } = await api.get<User>("/user");
  return data;
}

export const login = async (credentials: LoginCredentials): Promise<User> => {
  const { data } = await api.post<User>("/auth/login", credentials);
  return data;
};

export const loginAdmin = async (
  credentials: AdminLoginCredentials
): Promise<User> => {
  const { data } = await api.post<User>("/auth/login/admin", credentials.token);
  return data;
};

export const register = async (
  credentials: RegisterCredentials
): Promise<User> => {
  const { data } = await api.post<User>("/auth/register", credentials);
  return data;
};

export const logout = async (): Promise<void> => {
  await api.post("/auth/logout", {
    method: "POST",
    credentials: "include",
  });
};
