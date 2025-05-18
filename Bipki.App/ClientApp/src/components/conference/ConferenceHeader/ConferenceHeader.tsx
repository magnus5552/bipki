import { Box, Typography, Button, styled } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { RegistrationStatus } from "../../../types/Conference";
import { getConference } from "../../../api/conferenceApi";
import { getUser } from "api/authApi";
import { useParams } from "react-router-dom";
import { Loader } from "components/common/Loader/Loader";
import { Role, User } from "types/User";
import { useConference } from "hooks/useConference";

const Time = styled(Typography)({
  fontSize: "12px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: "#A980E0",
});

const Location = styled(Typography)({
  fontSize: "12px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: "#A980E0",
});

const formatDate = (startDate: Date, endDate: Date) => {
  const day = startDate.getDate();
  const month = startDate
    .toLocaleString("ru-RU", { month: "long", day: "numeric" })
    .split(" ")[1];
  const startTime = startDate.toLocaleTimeString("ru-RU", {
    hour: "2-digit",
    minute: "2-digit",
  });
  const endTime = endDate.toLocaleTimeString("ru-RU", {
    hour: "2-digit",
    minute: "2-digit",
  });
  return `${day} ${month}, ${startTime} - ${endTime}`;
};

export const ConferenceHeader = () => {
  const { user, conference, isLoading } = useConference();

  if (isLoading) {
    return <Loader />;
  }

  if (!conference) {
    return null;
  }

  return (
    <Box sx={{ padding: "16px 0", margin: "0 auto" }}>
      <Typography
        variant="h6"
        sx={{
          color: "#A980E0",
          marginBottom: "8px",
        }}
      >
        {conference.title}
      </Typography>
      <Time
        sx={{
          color: "#A980E0",
          marginBottom: "4px",
        }}
      >
        {formatDate(conference.startDate, conference.endDate)}
      </Time>
      <Location
        sx={{
          color: "#A980E0",
          marginBottom: "16px",
        }}
      >
        {conference.address}
      </Location>
      {user?.role === Role.Admin ? undefined : conference.registrationStatus ===
        RegistrationStatus.NotRegistered ? (
        <Button variant="contained" fullWidth sx={{ color: "#FFFFFF" }}>
          Записаться на мероприятие
        </Button>
      ) : (
        <Location
          sx={{
            color: "#A980E0",
          }}
        >
          Вы записаны
        </Location>
      )}
    </Box>
  );
};

