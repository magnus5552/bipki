import { Box, Typography, Button, styled } from "@mui/material";
import { useNavigate } from "react-router-dom";
import {
  Activity,
  ActivityType,
  RegistrationStatus,
} from "../../../types/Activity";
import { TypeLabel, Title, Time, SeatsInfo } from "./ActivityCard";

const CardContainer = styled(Box)({
  width: "311px",
  margin: "0 auto",
  padding: "10px",
  boxSizing: "border-box",
  borderRadius: "4px",
  border: "1px solid #A980E0",
});

const StatusLabel = styled(Typography)<{ isActive?: boolean }>(
  ({ isActive }) => ({
    fontSize: "10px",
    lineHeight: "16px",
    letterSpacing: "0.4px",
    color: isActive ? "#2D2D2D" : "#A980E0",
    backgroundColor: isActive ? "#A980E0" : "transparent",
    border: "1px solid #A980E0",
    borderRadius: "10px",
    padding: "0 8px",
    height: "15.59px",
    display: "inline-flex",
    alignItems: "center",
  })
);

interface CollapsedActivityCardProps {
  activity: Activity;
  onAction?: (activity: Activity) => void;
}

export const CollapsedActivityCard = ({
  activity,
  onAction,
}: CollapsedActivityCardProps) => {
  const navigate = useNavigate();

  const handleCardClick = (e: React.MouseEvent) => {
    if ((e.target as HTMLElement).closest('button')) {
      return;
    }
    navigate(`/activity/${activity.id}`);
  };

  const formatTime = (date: Date) => {
    return date.toLocaleTimeString("ru-RU", {
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getStatusLabel = () => {
    if (activity.type === ActivityType.Workshop) {
      switch (activity.registrationStatus) {
        case RegistrationStatus.Registered:
          return { text: "Вы записаны", isActive: true };
        case RegistrationStatus.WaitingList:
          return { text: "Вы в очереди", isActive: true };
        case RegistrationStatus.PendingConfirmation:
          return {
            text:
              "Ожидает подтверждения до " +
              formatTime(activity.confirmationDeadline!),
            isActive: true,
          };
        default:
          return { text: "", isActive: false };
      }
    }
    return { text: "", isActive: false };
  };

  const status = getStatusLabel();

  return (
    <CardContainer onClick={handleCardClick} sx={{ cursor: "pointer" }}>
      {activity.type === ActivityType.Workshop &&
        activity.registrationStatus ===
          RegistrationStatus.PendingConfirmation &&
        status.text && (
          <StatusLabel isActive width="100%" marginBottom="4px">{status.text}</StatusLabel>
        )}
      <Box sx={{ display: "flex", justifyContent: "space-between" }}>
        <Box sx={{ display: "flex", alignItems: "center", gap: "8px" }}>
          <TypeLabel>
            {activity.type === ActivityType.Workshop ? "Мастер-класс" : "Лекция"}
          </TypeLabel>
          {activity.type === ActivityType.Workshop &&
            activity.registrationStatus !==
              RegistrationStatus.PendingConfirmation &&
            status.text && (
              <StatusLabel isActive={status.isActive}>{status.text}</StatusLabel>
            )}
        </Box>
        {activity.type === ActivityType.Workshop && (
          <SeatsInfo isFull={activity.occupiedSeats === activity.totalSeats}>
            {`${activity.occupiedSeats}/${activity.totalSeats}`}
          </SeatsInfo>
        )}
      </Box>
      <Title sx={{ marginBottom: "8px" }}>{activity.title}</Title>
      <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <Time>{`${formatTime(activity.startDateTime)} - ${formatTime(
          activity.endDateTime
        )}`}</Time>
        {activity.type === ActivityType.Workshop &&
          activity.registrationStatus ===
            RegistrationStatus.PendingConfirmation &&
          onAction && (
            <Button 
              onClick={() => onAction(activity)} 
              variant="contained"
              sx={{ height: "16px", fontSize: "10px"}}
            >
              Подтвердить запись
            </Button>
          )}
      </Box>
    </CardContainer>
  );
};
