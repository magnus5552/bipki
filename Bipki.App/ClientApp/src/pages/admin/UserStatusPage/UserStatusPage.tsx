import { Box, Typography, CircularProgress } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useSearchParams } from 'react-router-dom';
import { checkInGuest } from '../../../api/adminApi';

export function UserStatusPage() {
    const [searchParams] = useSearchParams();
    const userId = searchParams.get('userId');

    const { data, isLoading, isError } = useQuery({
        queryKey: ['userStatus', userId],
        queryFn: () => checkInGuest(userId!),
        enabled: !!userId,
    });

    if (!userId) {
        return (
            <Box sx={{ p: 2 }}>
                <Typography variant="h6" color="error">
                    Не указан ID пользователя
                </Typography>
            </Box>
        );
    }

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
                <CircularProgress />
            </Box>
        );
    }

    if (isError || !data) {
        return (
            <Box sx={{ p: 2 }}>
                <Typography variant="h6" color="error">
                    Пользователь не найден
                </Typography>
            </Box>
        );
    }

    return (
        <Box sx={{ p: 2 }}>
            <Typography variant="h6" color="success.main">
                Подтвержден
            </Typography>
        </Box>
    );
} 