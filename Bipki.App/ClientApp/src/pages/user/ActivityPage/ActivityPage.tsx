import { useQuery } from "@tanstack/react-query";
import { getActivity } from "api/activityApi";
import { Loader } from "components/common/Loader/Loader";
import { useParams, useNavigate } from "react-router-dom";
import { ActivityCard } from "components/activities/ActivityCard/ActivityCard";
import { Box, Typography, Fab } from "@mui/material";
import { useConference } from 'hooks/useConference';
import EditIcon from '@mui/icons-material/Edit';

export function ActivityPage() {
  const { activityId } = useParams();
  const navigate = useNavigate();
  const {
    data: activity,
    isLoading: isActivityLoading,
    error: activityError,
  } = useQuery({
    queryKey: ["activity", activityId],
    queryFn: () => getActivity(activityId ?? ""),
    enabled: !!activityId,
  });

  const { user, conference, isLoading } = useConference();

  if (isLoading || isActivityLoading) {
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
      <Box sx={{ padding: "16px", position: "relative" }}>
        <Typography variant="body1">{conference.description}</Typography>
        {user?.role === "Admin" && (
          <Fab
            color="primary"
            size="large"
            sx={{
              position: "fixed",
              bottom: "80px",
              right: "40px",
            }}
            onClick={() => navigate(`/admin/conferences/${conference.id}/edit`)}
          >
            <EditIcon />
          </Fab>
        )}
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
