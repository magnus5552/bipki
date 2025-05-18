import { Box, Stack } from "@mui/material";
import { Activity, ActivityType, RegistrationStatus } from "../../../types/Activity";
import { StatusChip } from "../ActivityCard/ActivityCard";
import { useUser } from 'hooks/useUser';
import { Role } from 'types/User';

interface ActivityStatusProps {
  activity: Activity;
}

export const ActivityStatus = ({ activity }: ActivityStatusProps) => {
  const { user } = useUser();

  const getStatusLabel = () => {
    if (activity.type === ActivityType.Workshop) {
      switch (activity.registrationStatus) {
        case RegistrationStatus.Registered:
          return { text: "Вы записаны", isActive: true, isImportant: false };
        case RegistrationStatus.WaitingList:
          return { text: "Вы в очереди", isActive: true, isImportant: true };
        case RegistrationStatus.PendingConfirmation:
          return {
            text: "Ожидает подтверждения до " + formatTime(activity.confirmationDeadline!),
            isActive: true,
            isImportant: true,
          };
        default:
          return { text: "", isActive: false, isImportant: false };
      }
    }
    return { text: "", isActive: false, isImportant: false };
  };

  const formatTime = (date: Date) => {
    return date.toLocaleTimeString("ru-RU", {
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const status = getStatusLabel();

  return (
    <Box sx={{ display: "flex", justifyContent: "space-between" }}>
      <Stack
        spacing={{ xs: 1, sm: 1 }}
        direction="row"
        useFlexGap
        sx={{ flexWrap: "wrap" }}
      >
        <StatusChip
          color="primary"
          variant="outlined"
          size="small"
          label={activity.typeLabel}
        />
        {user?.role !== Role.Admin &&
        activity.type === ActivityType.Workshop &&
          activity.registrationStatus !== RegistrationStatus.PendingConfirmation &&
          status.text && (
            <StatusChip
              color="primary"
              variant={status.isActive ? "filled" : "outlined"}
              size="small"
              label={status.text}
              sx={{
                backgroundColor: status.isImportant ? "#CEAAFF" : "#A980E0",
              }}
            />
          )}
      </Stack>
      {activity.type === ActivityType.Workshop && (
        <StatusChip
          color="primary"
          variant={
            activity.occupiedSeats === activity.totalSeats
              ? "filled"
              : "outlined"
          }
          size="small"
          label={`${activity.occupiedSeats}/${activity.totalSeats}`}
        />
      )}
    </Box>
  );
}; 