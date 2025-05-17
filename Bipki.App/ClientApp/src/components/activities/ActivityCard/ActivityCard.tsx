import {
  Box,
  Typography,
  Button,
  styled,
  Chip,
} from "@mui/material";
import {
  Activity,
  ActivityType,
} from "../../../types/Activity";


import { ActivityTitle } from './ActivityTitle';

const CardContainer = styled(Box)({
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
  color: "#CFCFCF",
  marginBottom: "8px",
  fontWeight: "bold",
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

export const SeatsInfo = styled(Typography)<{ isFull: boolean }>(
  ({ isFull }) => ({
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
  })
);

export const StatusChip = styled(Chip)({
  marginBottom: "8px",
  height: "16px",
  fontSize: "10px",
});

interface ActivityCardProps {
  activity: Activity;
  onAction: (activity: Activity) => void;
}

export const ActivityCard = ({ activity, onAction }: ActivityCardProps) => {
  function openChat(id: string): void {
    throw new Error("Function not implemented.");
  }

  return (
    <CardContainer>
      <ActivityTitle activity={activity} onAction={onAction} marginBottom="16px" />
      <Description>{activity.description}</Description>
      {activity.type === ActivityType.Workshop && (
        <Button
          onClick={() => openChat(activity.id)}
          variant="contained"
          fullWidth
          color="info"
        >
          Открыть чат
        </Button>
      )}
    </CardContainer>
  );
};
