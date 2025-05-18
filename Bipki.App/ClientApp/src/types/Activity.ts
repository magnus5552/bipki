export enum ActivityType {
  Workshop = 'workshop',
  Lecture = 'lecture'
}

export enum RegistrationStatus {
  NotRegistered = 'not_registered',
  Registered = 'registered',
  WaitingList = 'waiting_list',
  PendingConfirmation = 'pending_confirmation'
}

export interface BaseActivity {
  id: string;
  title: string;
  startDateTime: Date;
  endDateTime: Date;
  description: string;
  type: ActivityType;
  typeLabel: string;
  registrationStatus: RegistrationStatus;
  waitingListCount: number;
  chatId: string;
}

export interface WorkshopActivity extends BaseActivity {
  type: ActivityType.Workshop;
  totalSeats: number;
  occupiedSeats: number;
  confirmationDeadline?: Date; // Только если status === PendingConfirmation
}

export interface LectureActivity extends BaseActivity {
  type: ActivityType.Lecture;
}

export type Activity = WorkshopActivity | LectureActivity;
