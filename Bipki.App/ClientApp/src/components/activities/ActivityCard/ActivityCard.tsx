import { Box, Typography, Button, styled } from "@mui/material";
import { Activity, ActivityType } from "../../../types/Activity";

const CardContainer = styled(Box)({
  position: "relative",
  width: "294px",
  height: "323.28px",
  margin: "0 auto",
  padding: "16px",
  boxSizing: "border-box",
  borderRadius: "4px",
});

export const TypeLabel = styled(Typography)({
  fontSize: "10px",
  lineHeight: "16px",
  color: "#A980E0",
  marginBottom: "4px",
  boxSizing: "border-box",
  padding: "0 8px",
  height: "15.59px",
  display: "inline-flex",
  alignItems: "center",
  border: "1px solid #A980E0",
  borderRadius: "10px",
});

export const Title = styled(Typography)({
  fontSize: "14px",
  lineHeight: "20px",
  letterSpacing: "0.25px",
  color: "#A980E0",
  marginBottom: "4px",
});

export const Time = styled(Typography)({
  fontSize: "10px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: "#A980E0",
});

export const Description = styled(Typography)({
  fontSize: "12px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: "#A980E0",
  marginBottom: "20px",
  minHeight: "199.8px",
  overflow: "hidden",
});

export const SeatsInfo = styled(Typography)<{ isFull: boolean }>(({ isFull }) => ({
  fontSize: "10px",
  lineHeight: "16px",
  letterSpacing: "0.4px",
  color: isFull ? "#2D2D2D" : "#A980E0",
  backgroundColor: isFull ? "#A980E0" : "transparent",
  border: "1px solid #A980E0",
  borderRadius: "10px",
  padding: "0 8px",
  height: "15.59px",
  display: "inline-flex",
  alignItems: "center",
}));

interface ActivityCardProps {
  activity: Activity;
  onAction: (activity: Activity) => void;
}

export const ActivityCard = ({ activity, onAction }: ActivityCardProps) => {
  const formatTime = (date: Date) => {
    return date.toLocaleTimeString("ru-RU", {
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getActionButtonText = () => {
    if (activity.type === "workshop") {
      switch (activity.registrationStatus) {
        case "not_registered":
          return "Записаться";
        case "registered":
          return "Отменить";
        case "waiting_list":
          return "В листе ожидания";
        case "pending_confirmation":
          return "Подтвердить запись";
      }
    }
    return null;
  };

  return (
    <CardContainer>
      <Box sx={{ display: "flex", justifyContent: "space-between" }}>
        <TypeLabel>
          {activity.type === ActivityType.Workshop ? "Мастер-класс" : "Лекция"}
        </TypeLabel>
        {activity.type === ActivityType.Workshop && (
          <SeatsInfo isFull={activity.occupiedSeats === activity.totalSeats}>
            {`${activity.occupiedSeats}/${activity.totalSeats}`}
          </SeatsInfo>
        )}
      </Box>
      <Title>{activity.title}</Title>
      <Time sx={{ marginBottom: "20px" }}>{`${formatTime(activity.startDateTime)} - ${formatTime(
        activity.endDateTime
      )}`}</Time>
      <Description>{activity.description}</Description>
      {activity.type === ActivityType.Workshop && (
        <Button
          onClick={() => onAction(activity)}
          variant="contained"
          fullWidth
        >
          {getActionButtonText()}
        </Button>
      )}
    </CardContainer>
  );
};
