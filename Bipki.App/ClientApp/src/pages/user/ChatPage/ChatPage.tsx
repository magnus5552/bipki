import {
  Box,
  TextField,
  IconButton,
  Typography,
  styled,
  Chip,
  Stack,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import { useParams } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getChat, Message } from "api/chatApi";
import { getUser } from "api/authApi";
import SendIcon from "@mui/icons-material/Send";
import InsertChartOutlinedIcon from "@mui/icons-material/InsertChartOutlined";
import { useState, useEffect } from "react";
import { Loader } from "components/common/Loader/Loader";
import { useSignalR } from "hooks/useSignalR";
import { Poll } from "types/Poll";
import { Role } from "types/User";

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

const PollCard = styled(Box)({
  alignSelf: "flex-start",
  color: "#A980E0",
  backgroundColor: "#2D2D2D",
  boxSizing: "border-box",
  borderRadius: "4px",
  border: "1px solid #A980E0",
  maxWidth: "80%",
  minWidth: "200px",
  padding: "8px",
});

const PollOption = styled(Button)<{ selected?: boolean }>(({ selected }) => ({
  width: "100%",
  justifyContent: "flex-start",
  textTransform: "none",
  color: selected ? "#2D2D2D" : "#A980E0",
  backgroundColor: selected ? "#A980E0" : "transparent",
  "&:hover": {
    backgroundColor: selected ? "#A980E0" : "rgba(169, 128, 224, 0.1)",
  },
}));

export const ChatPage = () => {
  const { chatId } = useParams();
  const [messageText, setMessageText] = useState("");
  const [isPollDialogOpen, setIsPollDialogOpen] = useState(false);
  const [pollTitle, setPollTitle] = useState("");
  const [pollOptions, setPollOptions] = useState<string[]>([""]);
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

    signalR.send("EnterChat", { chatId });

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

    signalR.on("PollUpdated", (poll: Poll) => {
      if (poll.chatId === chatId) {
        queryClient.setQueryData<typeof chat>(["chat", chatId], (old) => {
          if (!old) return old;
          const oldPoll = old.polls.find(p => p.id === poll.id);
          return {
            ...old,
            polls: old.polls.map(p => 
              p.id === poll.id 
                ? { ...poll, selectedOptionId: poll.selectedOptionId || oldPoll?.selectedOptionId }
                : p
            ),
          };
        });
      }
    });

    return () => {
      signalR.send("ExitChat", { chatId });
      signalR.off("ReceiveMessage");
      signalR.off("PollUpdated");
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

  const handleCreatePoll = () => {
    if (!signalR || !chatId || !pollTitle.trim() || pollOptions.some(opt => !opt.trim())) return;
    
    signalR.send("CreatePoll", {
      chatId,
      title: pollTitle.trim(),
      options: pollOptions.map(opt => opt.trim()),
    });

    setIsPollDialogOpen(false);
    setPollTitle("");
    setPollOptions([""]);
  };

  const handleVote = (pollId: string, optionId: string) => {
    if (!signalR || user?.role === Role.Admin) return;
    signalR.send("Vote", { pollId, optionId });
  };

  const handleAddOption = () => {
    setPollOptions([...pollOptions, ""]);
  };

  const handleOptionChange = (index: number, value: string) => {
    const newOptions = [...pollOptions];
    newOptions[index] = value;
    setPollOptions(newOptions);
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    if (event.key === "Enter" && !event.shiftKey) {
      event.preventDefault();
      sendMessageMutation.mutate(messageText.trim());
    }
  };

  if (isUserLoading || isChatLoading) return <Loader />;

  if (!chat) return null;

  const allItems = [
    ...chat.messages.map(m => ({ ...m, type: 'message' as const })),
    ...chat.polls.map(p => ({ ...p, type: 'poll' as const })),
  ].sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());

  return (
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <ChatContainer>
        <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-around" }}>
          Чат {chat.title}
        </Box>
        <MessagesContainer>
          {allItems.map((item) => (
            item.type === 'message' ? (
              <MessageCard key={item.id} isown={item.senderId === user?.id}>
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
                      label={item.senderId === user?.id ? "Вы" : item.senderName}
                      variant="outlined"
                      color={
                        item.senderId === user?.id ? "secondary" : "primary"
                      }
                    />
                    <MessageTime>
                      {new Date(item.timestamp).toLocaleTimeString("ru-RU", {
                        hour: "2-digit",
                        minute: "2-digit",
                      })}
                    </MessageTime>
                  </Stack>
                  <MessageText>{item.text}</MessageText>
                </Stack>
              </MessageCard>
            ) : (
              <PollCard key={item.id}>
                <Stack spacing={1}>
                  <Typography variant="subtitle1">{item.title}</Typography>
                  {item.options.map((option) => (
                    <PollOption
                      key={option.id}
                      selected={option.id === item.selectedOptionId}
                      onClick={() => handleVote(item.id, option.id)}
                      disabled={user?.role === Role.Admin}
                    >
                      {option.text} ({option.votes})
                    </PollOption>
                  ))}
                </Stack>
              </PollCard>
            )
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
          {user?.role === Role.Admin && (
            <IconButton
              onClick={() => setIsPollDialogOpen(true)}
              color="primary"
            >
              <InsertChartOutlinedIcon />
            </IconButton>
          )}
          <IconButton
            onClick={() => sendMessageMutation.mutate(messageText.trim())}
            disabled={!messageText.trim() || sendMessageMutation.isPending}
            color="primary"
          >
            <SendIcon />
          </IconButton>
        </InputContainer>
      </ChatContainer>

      <Dialog open={isPollDialogOpen} onClose={() => setIsPollDialogOpen(false)}>
        <DialogTitle>Создать опрос</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1 }}>
            <TextField
              label="Вопрос"
              value={pollTitle}
              onChange={(e) => setPollTitle(e.target.value)}
              fullWidth
            />
            {pollOptions.map((option, index) => (
              <TextField
                key={index}
                label={`Вариант ${index + 1}`}
                value={option}
                onChange={(e) => handleOptionChange(index, e.target.value)}
                fullWidth
              />
            ))}
            <Button onClick={handleAddOption}>Добавить вариант</Button>
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setIsPollDialogOpen(false)}>Отмена</Button>
          <Button
            onClick={handleCreatePoll}
            disabled={!pollTitle.trim() || pollOptions.some(opt => !opt.trim())}
          >
            Создать
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};
