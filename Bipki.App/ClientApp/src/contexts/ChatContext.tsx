import { createContext, useContext, useEffect, useRef, ReactNode } from "react";
import * as signalR from "@microsoft/signalr";
import { useQuery } from "@tanstack/react-query";
import { getUser } from "api/authApi";

interface ChatContextType {
  connection: signalR.HubConnection | null;
}

const ChatContext = createContext<ChatContextType>({ connection: null });

export const useChatContext = () => useContext(ChatContext);

interface ChatProviderProps {
  children: ReactNode;
}

export const ChatProvider = ({ children }: ChatProviderProps) => {
  const connection = useRef<signalR.HubConnection | null>(null);
  const { data: user } = useQuery({
    queryKey: ["user"],
    queryFn: getUser,
  });

  useEffect(() => {
    if (connection.current) return;
    if (!user) return;

    const hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("/api/chats-hub")
      .withAutomaticReconnect()
      .build();

    connection.current = hubConnection;

    hubConnection
      .start()
      .then(() => {
        console.log("SignalR Connected");
      })
      .catch((err: Error) => {
        console.error("SignalR Connection Error: ", err);
      });

    return () => {
      hubConnection.stop();
    };
  }, [user]);

  return (
    <ChatContext.Provider value={{ connection: connection.current }}>
      {children}
    </ChatContext.Provider>
  );
}; 