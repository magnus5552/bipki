import {
  Box,
  BottomNavigation,
  BottomNavigationAction,
  Paper,
} from "@mui/material";
import {
  useNavigate,
  useLocation,
  Outlet,
  matchPath,
  useParams,
} from "react-router-dom";
import Favorite from "@mui/icons-material/Favorite";
import { ConferenceHeader } from "../../conference/ConferenceHeader/ConferenceHeader";

export const AdminNavMenu = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { conferenceId } = useParams();

  const getActiveTab = () => {
    if (
      matchPath("/admin/conferences", location.pathname) ||
      matchPath("/admin/conferences/:conferenceId", location.pathname)
    ) {
      return "conferences";
    }
    if (
      matchPath(
        "/admin/conferences/:conferenceId/activities",
        location.pathname
      )
    ) {
      return "activities";
    }
    if (matchPath("/admin/conferences/:conferenceId/map", location.pathname)) {
      return "map";
    }
    return location.pathname;
  };

  const handleNavigation = (path: string) => {
    if (path === "conferences") {
      navigate(`/admin/conferences/${conferenceId}`);
    } else {
      navigate(`/admin/conferences/${conferenceId}/${path}`);
    }
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        minHeight: "100vh",
        alignItems: "center",
        width: "100vw",
        margin: "0 auto",
      }}
    >
      <ConferenceHeader />
      <Box component="main" sx={{ flexGrow: 1, pb: 7, width: "100vw" }}>
        <Outlet />
      </Box>
      <Paper
        sx={{ position: "fixed", bottom: 0, left: 0, right: 0 }}
        elevation={3}
      >
        <BottomNavigation
          value={getActiveTab()}
          onChange={(_, newValue) => {
            handleNavigation(newValue);
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
          <BottomNavigationAction
            value="conferences"
            icon={<Favorite />}
            label="О мероприятии"
          />
          <BottomNavigationAction
            value="activities"
            icon={<Favorite />}
            label="Программа"
          />
          <BottomNavigationAction
            value="map"
            icon={<Favorite />}
            label="Карта"
          />
        </BottomNavigation>
      </Paper>
    </Box>
  );
};
