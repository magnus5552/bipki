import { useQuery } from '@tanstack/react-query';
import { getActivity } from 'api/activityApi';
import { Loader } from 'components/common/Loader/Loader';
import { useParams } from 'react-router-dom';
import { ActivityCard } from 'components/activities/ActivityCard/ActivityCard';

export function ActivityPage() {
  const { activityId } = useParams();
  const { data: activity, isLoading, error } = useQuery({
    queryKey: ['activity', activityId],
    queryFn: () => getActivity(activityId ?? ""),
  });

  if (isLoading) {
    return <Loader />;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  if (!activity) {
    return <div>Activity not found</div>;
  }

  return <ActivityCard activity={activity} onAction={() => {}} />;
}
