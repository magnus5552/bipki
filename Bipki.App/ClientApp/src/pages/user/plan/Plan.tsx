import { useQuery } from "@tanstack/react-query";
import { getActivities } from "api/activityApi";
import { Loader } from "components/common/Loader/Loader";
import { CollapsedActivityCard } from "components/activities/ActivityCard/CollapsedActivityCard";
import { Box } from "@mui/material";

export function Plan() {
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
      {activities?.map((activity) => (
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
