export enum RegistrationStatus {
  Registered = 'registered',
  NotRegistered = 'not_registered'
}

export interface Conference {
  id: string;
  title: string;
  startDate: Date;
  endDate: Date;
  address: string;
  description: string;
  mapLink: string;
  registrationStatus: RegistrationStatus;
  participantsCount: number;
}
