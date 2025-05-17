import { Box, Typography, IconButton, styled } from "@mui/material";
import LogoutIcon from "@mui/icons-material/Logout";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getUser, logout } from "../../../api/authApi";
import { useNavigate } from "react-router-dom";

const HeaderContainer = styled(Box)({
  height: "56px",
  maxWidth: "311px",
  margin: "0 auto",
  display: "flex",
  justifyContent: "flex-end",
  alignItems: "center",
  gap: "8px",
});

const UserName = styled(Typography)({
  fontSize: "12px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: "#A980E0",
  textAlign: "right",
});

const LogoutButton = styled(IconButton)({
  color: "#A980E0",
  padding: 0,
  "&:hover": {
    backgroundColor: "transparent",
  },
});

export const Header = () => {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  
  const { data: user } = useQuery({
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

  if (!user?.isAuthenticated) {
    return <HeaderContainer />;
  }

  return (
    <HeaderContainer>
      <UserName>{user.username}</UserName>
      <LogoutButton onClick={handleLogout}>
        <LogoutIcon />
      </LogoutButton>
    </HeaderContainer>
  );
}; 