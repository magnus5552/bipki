import { Box, Button, styled } from "@mui/material";
import { useNavigate } from "react-router-dom";
import {
  Activity,
  ActivityType,
  RegistrationStatus,
} from "../../../types/Activity";
import { Title, Time, StatusChip } from "./ActivityCard";
import { ActivityStatus } from "../ActivityStatus/ActivityStatus";
import { ActivityTitle } from './ActivityTitle';

const CardContainer = styled(Box)({
  width: "311px",
  margin: "0 auto",
  padding: "10px",
  boxSizing: "border-box",
  borderRadius: "4px",
  border: "1px solid #A980E0",
});

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
    if ((e.target as HTMLElement).closest("button")) {
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
          return { text: "Вы записаны", isActive: true, isImportant: false };
        case RegistrationStatus.WaitingList:
          return { text: "Вы в очереди", isActive: true, isImportant: true };
        case RegistrationStatus.PendingConfirmation:
          return {
            text:
              "Ожидает подтверждения до " +
              formatTime(activity.confirmationDeadline!),
            isActive: true,
            isImportant: true,
          };
        default:
          return { text: "", isActive: false, isImportant: false };
      }
    }
    return { text: "", isActive: false, isImportant: false };
  };

  const status = getStatusLabel();

  return (
    <CardContainer onClick={handleCardClick} sx={{ cursor: "pointer" }}>
      {activity.type === ActivityType.Workshop &&
        activity.registrationStatus ===
          RegistrationStatus.PendingConfirmation &&
        status.text && (
          <StatusChip
            color="primary"
            variant="filled"
            size="small"
            label={status.text}
            sx={{ width: "100%", backgroundColor: "#CEAAFF" }}
          />
        )}
      <ActivityTitle activity={activity} onAction={() => onAction?.(activity)} />
    </CardContainer>
  );
};
