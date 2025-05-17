import axios from 'axios';
import { Role, User } from 'types/User';

export async function getUser(): Promise<User> {
  return {
    id: "1",
    username: "John Doe",
    email: "john.doe@example.com",
    role: Role.User,
    isAuthenticated: true,
  };
  //const { data } = await axios.get<User>('/api/user');
  //return data;
}

export const logout = async (): Promise<void> => {
  await axios.post('/api/auth/logout', {
    method: 'POST',
    credentials: 'include',
  });
};