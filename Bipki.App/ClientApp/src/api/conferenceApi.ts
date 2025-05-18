import axios from "axios";
import { Conference, RegistrationStatus } from "types/Conference";

const mockConferences: Conference[] = [
  {
    id: "1",
    title: "Конференция 1",
    startDate: new Date("2024-01-01"),
    endDate: new Date("2024-01-02"),
    address: "Москва",
    description: "Описание конференции 1",
    participantsCount: 100,
    registrationStatus: RegistrationStatus.Registered,
  },
  {
    id: "2",
    title: "Конференция 2",
    startDate: new Date("2024-01-03"),
    endDate: new Date("2024-01-04"),
    address: "Санкт-Петербург",
    description: "Описание конференции 2",
    participantsCount: 200,
    registrationStatus: RegistrationStatus.Registered,
  },
];

export interface PatchConferenceRequest {
  name?: string;
  startDate?: Date;
  endDate?: Date;
  location?: string;
  description?: string;
}

export interface CreateConferenceRequest {
  name: string;
  startDate: Date;
  endDate: Date;
  location: string;
  description: string;
}

export const getConferences = async (): Promise<Conference[]> => {
  //const response = await axios.get<Conference[]>("/admin/conferences");
  //return response.data;
  return mockConferences;
};

export const getConference = async (conferenceId: string): Promise<Conference> => {
  //const response = await axios.get<Conference>(`/admin/conferences/${conferenceId}`);
  //return response.data;
  return mockConferences.find(c => c.id === conferenceId)!;
};

export const updateConference = async (conferenceId: string, data: PatchConferenceRequest): Promise<void> => {
  await axios.patch(`/admin/conferences/${conferenceId}`, data);
};

export const createConference = async (data: CreateConferenceRequest): Promise<void> => {
  await axios.put("/admin/conferences", data);
};

export const deleteConference = async (conferenceId: string): Promise<void> => {
  await axios.delete(`/admin/conferences/${conferenceId}`);
};