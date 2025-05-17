import axios from "axios";
import { Activity, ActivityType, RegistrationStatus } from "types/Activity";

const activities: Activity[] = [
  {
    id: "1",
    title: "Варим мыло ручной работы",
    startDateTime: new Date("2024-05-21T17:00:00"),
    endDateTime: new Date("2024-05-21T18:00:00"),
    description: "Научимся создавать красивое и полезное мыло своими руками. Все материалы предоставляются.",
    type: ActivityType.Workshop,
    registrationStatus: RegistrationStatus.Registered,
    totalSeats: 25,
    occupiedSeats: 25
  },
  {
    id: "2",
    title: "Создание сайта на React",
    startDateTime: new Date("2024-05-21T18:30:00"),
    endDateTime: new Date("2024-05-21T19:15:00"),
    description: "Практический мастер-класс по созданию современного веб-приложения с использованием React.",
    type: ActivityType.Workshop,
    registrationStatus: RegistrationStatus.WaitingList,
    totalSeats: 15,
    occupiedSeats: 15
  },
  {
    id: "3",
    title: "Основы TypeScript",
    startDateTime: new Date("2024-05-22T14:00:00"),
    endDateTime: new Date("2024-05-22T15:30:00"),
    description: "Введение в TypeScript: типы, интерфейсы, дженерики и другие возможности языка.",
    type: ActivityType.Workshop,
    registrationStatus: RegistrationStatus.PendingConfirmation,
    totalSeats: 20,
    occupiedSeats: 18,
    confirmationDeadline: new Date("2024-05-21T18:15:00")
  },
  {
    id: "4",
    title: "Почему лошади не летают?",
    startDateTime: new Date("2024-05-22T16:00:00"),
    endDateTime: new Date("2024-05-22T17:00:00"),
    description: "Увлекательная лекция о физиологии и эволюции лошадей.",
    type: ActivityType.Lecture
  },
  {
    id: "5",
    title: "Искусство фотографии",
    startDateTime: new Date("2024-05-23T15:00:00"),
    endDateTime: new Date("2024-05-23T16:30:00"),
    description: "Мастер-класс по основам композиции и работе со светом в фотографии.",
    type: ActivityType.Workshop,
    registrationStatus: RegistrationStatus.NotRegistered,
    totalSeats: 12,
    occupiedSeats: 8
  },
  {
    id: "6",
    title: "История искусственного интеллекта",
    startDateTime: new Date("2024-05-23T17:00:00"),
    endDateTime: new Date("2024-05-23T18:00:00"),
    description: "Лекция о развитии ИИ от первых нейронных сетей до современных языковых моделей.",
    type: ActivityType.Lecture
  },
  {
    id: "7",
    title: "Создание мобильного приложения",
    startDateTime: new Date("2024-05-24T13:00:00"),
    endDateTime: new Date("2024-05-24T14:30:00"),
    description: "Практический мастер-класс по разработке мобильных приложений с использованием React Native.",
    type: ActivityType.Workshop,
    registrationStatus: RegistrationStatus.PendingConfirmation,
    totalSeats: 15,
    occupiedSeats: 12,
    confirmationDeadline: new Date("2024-05-23T18:00:00")
  }
];

export async function getActivity(activityId: string): Promise<Activity> {
  return activities.find((activity) => activity.id === activityId)!;
  
  //const { data } = await axios.get<Activity>(`/api/activities/${activityId}`);
  //return data;
}

export async function getActivities(): Promise<Activity[]> {
  return activities;
}