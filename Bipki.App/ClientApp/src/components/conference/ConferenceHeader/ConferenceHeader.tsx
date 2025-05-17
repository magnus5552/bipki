import { Box, Typography, Button, styled } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { Conference, RegistrationStatus } from "../../../types/Conference";

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
  const month = startDate.toLocaleString('ru-RU', { month: 'long', day: 'numeric' }).split(' ')[1];
  const startTime = startDate.toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' });
  const endTime = endDate.toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' });
  return `${day} ${month}, ${startTime} - ${endTime}`;
};

const getConference = async (): Promise<Conference> => {
  // TODO: Заменить на реальный API запрос
  return {
    id: "1",
    title: "НАЗВАНИЕ МЕРОПРИЯТИЯ",
    startDate: new Date("2024-05-21T17:00:00"),
    endDate: new Date("2024-05-21T18:00:00"),
    address: "г. Екатеринбург, ул. Балбесная 32",
    registrationStatus: RegistrationStatus.Registered,
  };
};

export const ConferenceHeader = () => {
  const { data: conference, isLoading } = useQuery({
    queryKey: ["conference"],
    queryFn: getConference,
  });

  if (isLoading || !conference) {
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
      {conference.registrationStatus === RegistrationStatus.NotRegistered ? (
        <Button
          variant="contained"
          fullWidth
        >
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