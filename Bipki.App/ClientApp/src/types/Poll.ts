export interface PollOption {
  id: string;
  text: string;
  votes: number;
}

export interface Poll {
  id: string;
  chatId: string;
  title: string;
  timestamp: string;
  options: PollOption[];
  selectedOptionId?: string;
}

export interface CreatePollRequest {
  chatId: string;
  title: string;
  options: string[];
}

export interface VoteRequest {
  pollId: string;
  optionId: string;
} 