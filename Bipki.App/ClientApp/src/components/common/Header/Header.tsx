import { Box, Typography, IconButton, styled, Stack } from "@mui/material";
import LogoutIcon from "@mui/icons-material/Logout";
import HomeIcon from "@mui/icons-material/Home";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getUser, logout } from "../../../api/authApi";
import { useNavigate, useParams } from "react-router-dom";
import { Role } from "types/User";
import SupportAgentIcon from "@mui/icons-material/SupportAgent";
import { getConference } from 'api/conferenceApi';
import { getActivity } from 'api/activityApi';

const HeaderContainer = styled(Stack)({
  height: "56px",
  padding: "0 24px",
  margin: "0 auto",
  gap: "8px",
  position: "relative",
});

const UserName = styled(Typography)({
  fontSize: "12px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: "#A980E0",
  textAlign: "right",
});

const IconButtonStyled = styled(IconButton)({
  color: "#A980E0",
  padding: 0,
  "&:hover": {
    backgroundColor: "transparent",
  },
});

const ChatButton = styled(IconButtonStyled)({
  position: "absolute",
  left: "50%",
  transform: "translateX(-50%)",
});

export const Header = () => {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const { conferenceId, activityId } = useParams();

  const { data: conference } = useQuery({
    queryKey: ["conference", conferenceId],
    queryFn: () => getConference(conferenceId!),
    enabled: !!conferenceId,
  });

  const { data: activity } = useQuery({
    queryKey: ["activity", activityId],
    queryFn: () => getActivity(activityId!),
    enabled: !!activityId,
  });

  const chatId = conference?.chatId ?? activity?.chatId;

  const { data: user, isLoading } = useQuery({
    queryKey: ["user"],
    queryFn: getUser,
  });

  const logoutMutation = useMutation({
    mutationFn: logout,
    onSuccess: () => {
      queryClient.setQueryData(["user"], null);
      navigate("/login");
    },
  });

  const handleLogout = () => {
    logoutMutation.mutate();
  };

  const handleHome = () => {
    navigate("/admin/conferences");
  };

  if (isLoading || !user) {
    return (
      <HeaderContainer>
        <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-around" }}>
          <Typography variant="h6">Bipki</Typography>
        </Box>
      </HeaderContainer>
    );
  }

  return (
    <HeaderContainer
      direction="row"
      justifyContent="space-between"
      alignItems="center"
    >
      <Box
        sx={{ display: "flex", alignItems: "center", gap: 1, minWidth: "80px" }}
      >
        {user.role === Role.Admin && (
          <IconButtonStyled onClick={handleHome}>
            <HomeIcon />
          </IconButtonStyled>
        )}
      </Box>
      {user && chatId && (
        <ChatButton
          onClick={() =>
            navigate(
              user.role === Role.Admin
                ? `/admin/conferences/${conferenceId}/chat/${chatId}`
                : `/chat/${chatId}`
            )
          }
        >
          <SupportAgentIcon />
        </ChatButton>
      )}
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          gap: 1,
          minWidth: "80px",
          justifyContent: "flex-end",
        }}
      >
        <UserName>{user.username}</UserName>
        <IconButtonStyled onClick={handleLogout}>
          <LogoutIcon />
        </IconButtonStyled>
      </Box>
    </HeaderContainer>
  );
};
