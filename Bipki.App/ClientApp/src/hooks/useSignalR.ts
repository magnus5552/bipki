import { useChatContext } from "contexts/ChatContext";

export const useSignalR = () => {
  const { connection } = useChatContext();
  return connection;
}; 