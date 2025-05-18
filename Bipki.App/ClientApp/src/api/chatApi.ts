import { api } from "./api";
import { Poll } from "types/Poll";

export interface Message {
  id: string;
  timestamp: string;
  text: string;
  chatId: string;
  senderId: string;
  senderName: string;
}

export interface Chat {
  id: string;
  title: string;
  messages: Message[];
  polls: Poll[];
}

export interface SendMessageRequest {
  chatId: string;
  text: string;
}

export const getChat = async (chatId: string): Promise<Chat> => {
  const response = await api.get<Chat>(`/chats/${chatId}`);
  return response.data;
};