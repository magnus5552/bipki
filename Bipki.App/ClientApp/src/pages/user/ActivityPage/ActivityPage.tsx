import { useQuery } from "@tanstack/react-query";
import { getActivity } from "api/activityApi";
import { getConference } from "api/conferenceApi";
import { Loader } from "components/common/Loader/Loader";
import { useParams } from "react-router-dom";
import { ActivityCard } from "components/activities/ActivityCard/ActivityCard";
import { Box, Typography } from "@mui/material";

export function ActivityPage() {
  const { activityId } = useParams();

  const {
    data: activity,
    isLoading: isActivityLoading,
    error: activityError,
  } = useQuery({
    queryKey: ["activity", activityId],
    queryFn: () => getActivity(activityId ?? ""),
    enabled: !!activityId,
  });

  const { data: conference, isLoading: isConferenceLoading } = useQuery({
    queryKey: ["conference"],
    queryFn: getConference,
    enabled: !activityId,
  });

  if (isActivityLoading || isConferenceLoading) {
    return <Loader />;
  }

  if (activityError) {
    return <div>Error: {activityError.message}</div>;
  }

  if (activityId && !activity) {
    return <div>Activity not found</div>;
  }

  if (!activityId && conference) {
    return (
      <Box sx={{ padding: "16px" }}>
        <Typography variant="body1">{conference.description}</Typography>
      </Box>
    );
  }

  if (!activity) {
    return null;
  }

  return (
    <ActivityCard
      activity={activity}
      onAction={() => {
        throw new Error("Not implemented");
      }}
    />
  );
}
