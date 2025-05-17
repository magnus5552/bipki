import React from 'react';
import { Box, Container, Typography, styled } from '@mui/material';

const AuthContainer = styled(Container)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  padding: theme.spacing(2),
  maxWidth: '100%',
  minHeight: '100vh',
}));

const ContentContainer = styled(Box)({
  width: '100%',
  maxWidth: '311px',
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
});

const TitleText = styled(Typography)(({ theme }) => ({
  fontSize: '22px',
  lineHeight: '28px',
  fontWeight: 500,
  textAlign: 'center',
  color: '#A980E0',
  marginTop: theme.spacing(8),
  marginBottom: theme.spacing(4),
  width: '100%',
}));

interface AuthLayoutProps {
  title: string;
  children: React.ReactNode;
}

export const AuthLayout: React.FC<AuthLayoutProps> = ({ title, children }) => {
  return (
    <AuthContainer disableGutters>
      <ContentContainer>
        <TitleText variant="h1">{title}</TitleText>
        {children}
      </ContentContainer>
    </AuthContainer>
  );
}; 