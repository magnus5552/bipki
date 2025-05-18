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

const mockChats: Chat[] = [
  {
    id: "1",
    title: "Конференция 1",
    messages: [
      {
        id: "1",
        timestamp: "2021-01-01T00:00:00Z",
        text: "Привет, как дела?",
        chatId: "1",
        senderId: "1",
        senderName: "Иван Иванов",
      },
      {
        id: "2",
        timestamp: "2021-01-01T00:00:00Z",
        text: "Привет, как дела?",
        chatId: "1",
        senderId: "2",
        senderName: "Петр Петров",
      },
    ]
  },
  {
    id: "2",
    title: "Конференция 2",
    messages: [],
  },
];

export const getChat = async (chatId: string): Promise<Chat> => {
  //const response = await api.get<Chat>(`/api/chats/${chatId}`);
  //return response.data;
  return mockChats.find(chat => chat.id === chatId)!;
};

export const sendMessage = async (request: SendMessageRequest): Promise<void> => {
  await api.post(`/api/chats/${request.chatId}/messages`, request);
}; 