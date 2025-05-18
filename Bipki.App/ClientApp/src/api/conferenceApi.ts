import { Conference } from "types/Conference";
import { api } from './api';


export interface PatchConferenceRequest {
  name?: string;
  startDate?: Date;
  endDate?: Date;
  location?: string;
  description?: string;
  plan?: string;
}

export interface CreateConferenceRequest {
  name: string;
  startDate: Date;
  endDate: Date;
  location: string;
  description: string;
  plan?: string;
}

export const getConferences = async (): Promise<Conference[]> => {
  const response = await api.get<Conference[]>("/conferences");
  return response.data;
};

export const getConference = async (conferenceId: string): Promise<Conference> => {
  const response = await api.get<Conference>(`/conferences/${conferenceId}`);
  return response.data;
};

export const updateConference = async (conferenceId: string, data: PatchConferenceRequest): Promise<void> => {
  await api.patch(`/conferences/${conferenceId}`, data);
};

export const createConference = async (data: CreateConferenceRequest): Promise<void> => {
  await api.put("/conferences", data);
};

export const deleteConference = async (conferenceId: string): Promise<void> => {
  await api.delete(`/conferences/${conferenceId}`);
};

export const getConferenceQRCode = async (conferenceId: string): Promise<string> => {
  const response = await api.get<string>(`/conferences/${conferenceId}/qrcode`);
  return response.data;
};

export const checkInGuest = async (userId: string): Promise<void> => {
  await api.post(`/checkInGuest/${userId}`);
};