import { RegistrationStatus } from 'types/Conference';
import { Conference } from 'types/Conference';

export const getConference = async (): Promise<Conference> => {
  // TODO: Заменить на реальный API запрос
  return {
    id: "1",
    title: "НАЗВАНИЕ МЕРОПРИЯТИЯ",
    startDate: new Date("2024-05-21T17:00:00"),
    endDate: new Date("2024-05-21T18:00:00"),
    address: "г. Екатеринбург, ул. Балбесная 32",
    registrationStatus: RegistrationStatus.Registered,
  };
};