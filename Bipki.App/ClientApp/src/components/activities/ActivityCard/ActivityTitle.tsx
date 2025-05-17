import { Stack } from "@mui/material";
import { Time } from "./ActivityCard";
import { Activity, RegistrationStatus } from "types/Activity";
import { Title } from "./ActivityCard";
import { ActivityStatus } from "../ActivityStatus/ActivityStatus";
import PersonAddAltIcon from "@mui/icons-material/PersonAddAlt";
import PersonOffIcon from "@mui/icons-material/PersonOff";
import HowToRegIcon from "@mui/icons-material/HowToReg";
import IconButton from "@mui/material/IconButton";

export function ActivityTitle({
  activity,
  onAction,
  marginBottom,
}: {
  activity: Activity;
  onAction: (activity: Activity) => void;
  marginBottom?: string;
}) {
  const formatTime = (date: Date) => {
    return date.toLocaleTimeString("ru-RU", {
      hour: "2-digit",
      minute: "2-digit",
    });
  };
  return (
    <>
      <ActivityStatus activity={activity} />
      <Stack
        direction="row"
        justifyContent="space-between"
        alignItems="center"
        marginBottom={marginBottom}
      >
        <Stack
          direction="column"
          justifyContent="space-between"
          alignItems="flex-start"
        >
          <Title>{activity.title}</Title>
          <Time>{`${formatTime(activity.startDateTime)} - ${formatTime(
            activity.endDateTime
          )}`}</Time>
        </Stack>
        <RegisterButton activity={activity} onAction={onAction} />
      </Stack>
    </>
  );
}

function RegisterButton({
  activity,
  onAction,
}: {
  activity: Activity;
  onAction: (activity: Activity) => void;
}) {
  const registrationStatus = activity.registrationStatus;
  const isRegistered =
    registrationStatus === RegistrationStatus.Registered ||
    registrationStatus === RegistrationStatus.WaitingList;
  const isNotRegistered =
    registrationStatus === RegistrationStatus.NotRegistered;
  const isPendingConfirmation =
    registrationStatus === RegistrationStatus.PendingConfirmation;
  return (
    <IconButton
      onClick={() => onAction(activity)}
      color={
        isNotRegistered ? "info" : isPendingConfirmation ? "success" : "error"
      }
    >
      {isNotRegistered && <PersonAddAltIcon />}
      {isPendingConfirmation && <HowToRegIcon />}
      {isRegistered && <PersonOffIcon />}
    </IconButton>
  );
}
