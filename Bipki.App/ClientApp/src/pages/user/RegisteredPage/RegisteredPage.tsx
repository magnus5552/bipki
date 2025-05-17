import { Box } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { getActivities } from "api/activityApi";
import { CollapsedActivityCard } from "components/activities/ActivityCard/CollapsedActivityCard";
import { Loader } from "components/common/Loader/Loader";
import { RegistrationStatus } from "types/Activity";

export function RegisteredPage() {
  const {
    data: activities,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["activities"],
    queryFn: getActivities,
  });

  if (isLoading) {
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
            onAction={() => {
              throw new Error("Not implemented");
            }}
          />
        ))}
    </Box>
  );
}
