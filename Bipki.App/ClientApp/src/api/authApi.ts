import axios from "axios";
import { Role, User } from "types/User";
import { LoginCredentials, RegisterCredentials, AdminLoginCredentials } from "types/Auth";


export async function getUser(): Promise<User> {
  return {
    id: "1",
    username: "John Doe",
    telegram: "john_doe",
    role: Role.Admin,
    conferenceId: "1",
    checkedIn: false,
    isAuthenticated: true,
  };
  //const { data } = await axios.get<User>('/api/user');
  //return data;
}

export const login = async (credentials: LoginCredentials): Promise<User> => {
  const { data } = await axios.post<User>("/api/auth/login", credentials);
  return data;
};

export const loginAdmin = async (
  credentials: AdminLoginCredentials
): Promise<User> => {
  const { data } = await axios.post<User>("/api/auth/admin/login", credentials);
  return data;
};

export const register = async (
  credentials: RegisterCredentials
): Promise<User> => {
  const { data } = await axios.post<User>("/api/auth/register", credentials);
  return data;
};

export const logout = async (): Promise<void> => {
  await axios.post("/api/auth/logout", {
    method: "POST",
    credentials: "include",
  });
};
