import {
  Box,
  TextField,
  IconButton,
  Typography,
  styled,
  Chip,
  Stack,
} from "@mui/material";
import { useParams } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getChat, sendMessage, Message } from "api/chatApi";
import { getUser } from "api/authApi";
import SendIcon from "@mui/icons-material/Send";
import { useState, useEffect } from "react";
import { Loader } from "components/common/Loader/Loader";
import { useSignalR } from "hooks/useSignalR";

const ChatContainer = styled(Box)({
  display: "flex",
  flexDirection: "column",
  height: "calc(100vh - 160px)",
  width: "100%",
  padding: "0 24px",
  gap: "16px",
});

const MessagesContainer = styled(Box)({
  display: "flex",
  flexDirection: "column",
  gap: "8px",
  flex: 1,
  overflowY: "auto",
  width: "100%",
});

const MessageCard = styled(Box)<{ isown: boolean }>(({ isown: isOwn }) => ({
  alignSelf: isOwn ? "flex-end" : "flex-start",
  color: isOwn ? "#2D2D2D" : "#A980E0",
  backgroundColor: isOwn ? "#A980E0" : "#2D2D2D",
  boxSizing: "border-box",
  borderRadius: "4px",
  border: "1px solid #A980E0",
  maxWidth: "80%",
  minWidth: "200px",
}));

const SenderChip = styled(Chip)({
  fontSize: "12px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  height: "16px",
  "& .MuiChip-label": {
    padding: "0 8px",
  },
});

const MessageTime = styled(Typography)({
  fontSize: "10px",
  lineHeight: "14px",
  letterSpacing: "0.4px",
  opacity: 0.7,
});

const MessageText = styled(Typography)({
  fontSize: "14px",
  lineHeight: "20px",
  letterSpacing: "0.25px",
  wordBreak: "break-word",
});

const InputContainer = styled(Box)({
  display: "flex",
  gap: "8px",
  alignItems: "center",
});

export const ChatPage = () => {
  const { chatId } = useParams();
  const [messageText, setMessageText] = useState("");
  const queryClient = useQueryClient();
  const signalR = useSignalR();

  const { data: user, isLoading: isUserLoading } = useQuery({
    queryKey: ["user"],
    queryFn: getUser,
  });

  const { data: chat, isLoading: isChatLoading } = useQuery({
    queryKey: ["chat", chatId],
    queryFn: () => getChat(chatId!),
    enabled: !!chatId,
  });

  useEffect(() => {
    if (!signalR || !chatId) return;

    signalR.on("ReceiveMessage", (message: Message) => {
      if (message.chatId === chatId) {
        queryClient.setQueryData<typeof chat>(["chat", chatId], (old) => {
          if (!old) return old;
          return {
            ...old,
            messages: [...old.messages, message],
          };
        });
      }
    });

    return () => {
      signalR.off("ReceiveMessage");
    };
  }, [signalR, chatId, queryClient]);

  const sendMessageMutation = useMutation({
    mutationFn: async (text: string) => {
      if (!signalR) throw new Error("SignalR not connected");
      await signalR.send("SendMessage", {
        chatId,
        text,
      });
    },
    onSuccess: () => {
      setMessageText("");
    },
  });

  const handleSendMessage = () => {
    if (!messageText.trim() || !chatId) return;

    sendMessageMutation.mutate(messageText.trim());
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    if (event.key === "Enter" && !event.shiftKey) {
      event.preventDefault();
      handleSendMessage();
    }
  };

  if (isUserLoading || isChatLoading) return <Loader />;

  if (!chat) return null;

  return (
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <ChatContainer>
        <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-around" }}>
          Чат {chat.title}
        </Box>
        <MessagesContainer>
          {chat.messages.map((message) => (
            <MessageCard key={message.id} isown={message.senderId === user?.id}>
              <Stack
                direction="column"
                justifyContent="space-between"
                sx={{ padding: "8px" }}
                spacing={1}
              >
                <Stack
                  direction="row"
                  justifyContent="space-between"
                  alignItems="center"
                >
                  <SenderChip
                    label={message.senderId === user?.id ? "Вы" : message.senderName}
                    variant="outlined"
                    color={
                      message.senderId === user?.id ? "secondary" : "primary"
                    }
                  />
                  <MessageTime>
                    {new Date(message.timestamp).toLocaleTimeString("ru-RU", {
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </MessageTime>
                </Stack>
                <MessageText>{message.text}</MessageText>
              </Stack>
            </MessageCard>
          ))}
        </MessagesContainer>
        <InputContainer>
          <TextField
            fullWidth
            multiline
            maxRows={4}
            placeholder="Введите сообщение..."
            value={messageText}
            onChange={(e) => setMessageText(e.target.value)}
            onKeyDown={handleKeyPress}
            size="small"
            variant="standard"
          />
          <IconButton
            onClick={handleSendMessage}
            disabled={!messageText.trim() || sendMessageMutation.isPending}
            color="primary"
          >
            <SendIcon />
          </IconButton>
        </InputContainer>
      </ChatContainer>
    </Box>
  );
};
