import React from 'react';
import { CircularProgress, Box } from '@mui/material';

export const Loader: React.FC = () => {
    return (
        <Box
            sx={{
                position: 'fixed',
                top: 0,
                left: 0,
                width: '100%',
                height: '100%',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                color: '#A980E0',
                zIndex: 9999
            }}
        >
            <CircularProgress />
        </Box>
    );
}; 