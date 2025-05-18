import { useQuery } from '@tanstack/react-query';
import { getUser } from 'api/authApi';

import { User } from 'types/User';
import { getConference } from 'api/conferenceApi';
import { Role } from 'types/User';
import { useParams } from 'react-router-dom';

function getConferenceId(user: User | undefined, conferenceId: string | undefined) {
  if (user?.role !== Role.User) {
    return conferenceId;
  }

  return user?.conferenceId;
}

export function useConference() {
  const { data: user, isLoading: isUserLoading, error: userError } = useQuery({
    queryKey: ["user"],
    queryFn: getUser,
  });

  const { conferenceId: paramsConferenceId } = useParams();

  const conferenceId = getConferenceId(user, paramsConferenceId);

  const { data: conference, isLoading, error: conferenceError } = useQuery({
    queryKey: ["conference", conferenceId],
    queryFn: () => getConference(conferenceId!),
    enabled: !!conferenceId,
  });

  if (user && !conferenceId) {
    return { user, conference: undefined, isLoading: false };
  }

  if (isUserLoading || (conferenceId && isLoading)) {
    return { user: undefined, conference: undefined, isLoading: true };
  }

  if (!conference || userError || conferenceError) {
    return { user: undefined, conference: undefined, isLoading: false };
  }
  return { user, conference, isLoading: false };
}