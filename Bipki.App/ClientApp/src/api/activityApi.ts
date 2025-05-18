import { Activity, ActivityType } from "types/Activity";
import { api } from './api';

export interface PatchActivityRequest {
  title?: string;
  startDateTime?: Date;
  endDateTime?: Date;
  description?: string;
  type?: ActivityType;
  typeLabel?: string;
  totalSeats?: number;
}

export interface CreateActivityRequest {
  title: string;
  startDateTime: Date;
  endDateTime: Date;
  description: string;
  type: ActivityType;
  typeLabel: string;
  totalSeats?: number;
}

export async function getActivity(activityId: string): Promise<Activity> { 
  const { data } = await api.get<Activity>(`/activities/${activityId}`);
  return data;
}

export const getConferenceActivities = async (conferenceId: string): Promise<Activity[]> => {
  const response = await api.get<Activity[]>(`/conferences/${conferenceId}/activities`);
  return response.data;
};

export const updateActivity = async (conferenceId: string, activityId: string, data: PatchActivityRequest): Promise<void> => {
  await api.patch(`/conferences/${conferenceId}/activities/${activityId}`, data);
};

export const createActivity = async (conferenceId: string, data: CreateActivityRequest): Promise<void> => {
  await api.post(`/conferences/${conferenceId}/activities`, data);
};

export const deleteActivity = async (conferenceId: string, activityId: string): Promise<void> => {
  await api.delete(`/conferences/${conferenceId}/activities/${activityId}`);
};

export const registerForActivity = async (activityId: string): Promise<void> => {
  await api.post(`/activities/${activityId}/register`);
};

export const unregisterFromActivity = async (activityId: string): Promise<void> => {
  await api.delete(`/activities/${activityId}/register`);
};

export const confirmRegistration = async (activityId: string): Promise<void> => {
  await api.post(`/activities/${activityId}/confirm`);
};