import { api } from "./api";

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
}

export interface SendMessageRequest {
  chatId: string;
  text: string;
}

export const getChat = async (chatId: string): Promise<Chat> => {
  const response = await api.get<Chat>(`/chats/${chatId}`);
  return response.data;
};

export const createChat = async (title: string): Promise<Chat> => {
  const response = await api.post<Chat>('/chats', { title });
  return response.data;
};

export const deleteChat = async (chatId: string): Promise<void> => {
  await api.delete(`/chats/${chatId}`);
}; 