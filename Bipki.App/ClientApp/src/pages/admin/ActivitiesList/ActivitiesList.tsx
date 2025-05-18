import { Box, Typography, Button, Stack } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { getConferenceActivities } from "api/activityApi";
import { Loader } from "components/common/Loader/Loader";
import { AdminActivityCard } from "components/activities/ActivityCard/AdminActivityCard";
import { useParams, useNavigate } from "react-router-dom";
import AddIcon from '@mui/icons-material/Add';

export const ActivitiesList = () => {
  const { conferenceId } = useParams();
  const navigate = useNavigate();

  const {
    data: activities,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["conferenceActivities", conferenceId],
    queryFn: () => getConferenceActivities(conferenceId!),
    enabled: !!conferenceId,
  });

  if (isLoading) {
    return <Loader />;
  }

  if (error) {
    return <div>Ошибка при загрузке активностей</div>;
  }

  return (
    <Stack direction="column" justifyContent="space-between" spacing={2} sx={{ paddingBottom: "16px" }}>  
      <Box sx={{ display: "flex", flexDirection: "column", gap: "10px" }}>
        {activities?.map((activity) => (
          <AdminActivityCard
            key={activity.id}
            activity={activity}
            conferenceId={conferenceId!}
          />
        ))}
      </Box>
      <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => navigate(`/admin/conferences/${conferenceId}/activities/create/edit`)}
        >
          Добавить активность
        </Button>
    </Stack>
  );
}; 