import { api } from './api';

export const checkInGuest = async (userId: string): Promise<boolean> => {
  const { data } = await api.patch(`/admin/checkInGuest/${userId}`);
  return data;
}; 