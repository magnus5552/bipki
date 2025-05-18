import { Box } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { getActivities } from "api/activityApi";
import { CollapsedActivityCard } from "components/activities/ActivityCard/CollapsedActivityCard";
import { Loader } from "components/common/Loader/Loader";
import { RegistrationStatus } from "types/Activity";
import { useActivityRegistration } from "hooks/useActivityRegistration";

export function RegisteredPage() {
  const {
    data: activities,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["activities"],
    queryFn: getActivities,
  });

  const { handleAction, isLoading: isActionLoading } = useActivityRegistration("");

  if (isLoading || isActionLoading) {
    return <Loader />;
  }

  if (error) {
    return <div>Error</div>;
  }

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: "10px" }}>
      {activities
        ?.filter(
          (activity) =>
            activity.registrationStatus !== RegistrationStatus.NotRegistered
        )
        .map((activity) => (
          <CollapsedActivityCard
            key={activity.id}
            activity={activity}
            onAction={() => handleAction(activity.registrationStatus)}
          />
        ))}
    </Box>
  );
}
