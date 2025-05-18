import { Box, Button, styled } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Activity, ActivityType } from "../../../types/Activity";
import { ActivityTitle } from './ActivityTitle';
import { Chip } from "@mui/material";

const CardContainer = styled(Box)({
  width: "311px",
  margin: "0 auto",
  padding: "10px",
  boxSizing: "border-box",
  borderRadius: "4px",
  border: "1px solid #A980E0",
});

interface AdminActivityCardProps {
  activity: Activity;
  conferenceId: string;
}

export const AdminActivityCard = ({
  activity,
  conferenceId,
}: AdminActivityCardProps) => {
  const navigate = useNavigate();

  const handleCardClick = (e: React.MouseEvent) => {
    if ((e.target as HTMLElement).closest("button")) {
      return;
    }
    navigate(`/admin/conferences/${conferenceId}/activities/${activity.id}/edit`);
  };

  const getWaitingListText = (count: number) => {
    if (count === 0) return "";
    const lastDigit = count % 10;
    const lastTwoDigits = count % 100;
    
    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) {
      return `Лист ожидания: ${count} человек`;
    }
    
    if (lastDigit === 1) {
      return `Лист ожидания: ${count} человек`;
    }
    
    if (lastDigit >= 2 && lastDigit <= 4) {
      return `Лист ожидания: ${count} человека`;
    }
    
    return `Лист ожидания: ${count} человек`;
  };

  return (
    <CardContainer onClick={handleCardClick} sx={{ cursor: "pointer" }}>
      {activity.waitingListCount > 0 && (
        <Chip
          color="primary"
          variant="filled"
          size="small"
          label={getWaitingListText(activity.waitingListCount)}
          sx={{ 
            width: "100%", 
            backgroundColor: "#CEAAFF",
            marginBottom: "8px"
          }}
        />
      )}
      <ActivityTitle 
        activity={activity} 
        onAction={() => navigate(`/admin/conferences/${conferenceId}/activities/${activity.id}/edit`)}
        showRegisterButton={false}
        showEditButton={true}
      />
    </CardContainer>
  );
}; 