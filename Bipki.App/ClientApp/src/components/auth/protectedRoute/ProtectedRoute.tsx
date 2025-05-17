import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { Role } from '../../../types/User';
import { Loader } from '../../common/Loader/Loader';
import { getUser } from 'api/authApi';
import { Header } from '../../common/Header/Header';
import { Box } from '@mui/material';

interface ProtectedRouteProps {
    requiredRole: Role;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ requiredRole }) => {
    const { data: user, isLoading, error } = useQuery({
        queryKey: ['user'],
        queryFn: getUser
    });

    if (isLoading) {
        return <Loader />;
    }

    if (error || !user?.isAuthenticated || user.role !== requiredRole) {
        return <Navigate to="/login" replace />;
    }

    return (
        <Box>
            <Header />
            <Outlet />
        </Box>
    );
};