import {
  Box,
  BottomNavigation,
  BottomNavigationAction,
  Paper,
} from "@mui/material";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import Info from "@mui/icons-material/Info";
import Assignment from "@mui/icons-material/Assignment";
import Star from "@mui/icons-material/Star";
import Map from "@mui/icons-material/Map";
import { ConferenceHeader } from "../../conference/ConferenceHeader/ConferenceHeader";

export const GuestNavMenu = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const isActivityPage = location.pathname.startsWith("/activity");

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        minHeight: "100vh",
        alignItems: "center",
        maxWidth: "311px",
        margin: "0 auto",
      }}
    >
      <ConferenceHeader />
      <Box component="main" sx={{ flexGrow: 1, pb: 7 }}>
        <Outlet />
      </Box>
      <Paper
        sx={{ position: "fixed", bottom: 0, left: 0, right: 0 }}
        elevation={3}
      >
        <BottomNavigation
          value={isActivityPage ? "/activity" : location.pathname}
          onChange={(_, newValue) => {
            navigate(newValue);
          }}
          sx={{
            backgroundColor: "#2D2D2D",
            "& .MuiBottomNavigationAction-root": {
              color: "#FFFFFF99",
              "&.Mui-selected": {
                color: "#A980E0",
              },
            },
          }}
        >
          <BottomNavigationAction value="/activity" icon={<Info />} />
          <BottomNavigationAction value="/plan" icon={<Assignment />} />
          <BottomNavigationAction value="/registered" icon={<Star />} />
          <BottomNavigationAction value="/map" icon={<Map />} />
        </BottomNavigation>
      </Paper>
    </Box>
  );
};
