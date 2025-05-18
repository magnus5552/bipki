import React from 'react';
import { Box, Typography } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { getConference } from 'api/conferenceApi';
import { useParams } from 'react-router-dom';

export const AdminMap: React.FC = () => {
  const { conferenceId } = useParams();

  const { data: conference, isLoading } = useQuery({
    queryKey: ['conference', conferenceId],
    queryFn: () => getConference(conferenceId!),
    enabled: !!conferenceId,
  });

  if (isLoading) {
    return <Typography>Загрузка...</Typography>;
  }

  if (!conference?.plan) {
    return <Typography>План конференции не найден</Typography>;
  }

  return (
    <Box sx={{ width: '100%', height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
      <img 
        src={conference.plan} 
        alt="План конференции" 
        style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }}
      />
    </Box>
  );
}; 