import axios from 'axios';
import { User } from 'types/User';

export async function getUser() {
  const { data } = await axios.get<User>('/api/user');
  return data;
}