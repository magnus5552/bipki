import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { Box } from '@mui/material';
import { AuthLayout } from '../../../components/auth/AuthLayout/AuthLayout';
import {
  FormContainer,
  FormText,
  PrimaryButton,
  SecondaryButton,
  StyledTextField
} from '../../../components/auth/AuthComponents/AuthComponents';
import { loginAdmin } from '../../../api/authApi';
import { AdminLoginCredentials } from '../../../types/Auth';
import { useUser } from 'hooks/useUser';
import { Role } from 'types/User';

export const AdminLoginPage: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<AdminLoginCredentials>({
    token: '',
  });

  const { user, isLoading } = useUser();

  const loginMutation = useMutation({
    mutationFn: loginAdmin,
    onSuccess: (data) => {
      if (data.role === Role.Admin) {
        navigate('/admin/conferences');
      } else {
        navigate('/activity');
      }
    },
  });

  if (user && !isLoading) {
    if (user.role === Role.Admin) {
      navigate('/admin/conferences');
    } else {
      navigate('/activity');
    }
    return null;
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    loginMutation.mutate(formData);
  };

  const goToUserLogin = () => {
    navigate('/login');
  };

  return (
    <AuthLayout title="Войти как администратор">
      <FormContainer>
        <Box component="form" onSubmit={handleLogin} sx={{ width: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          <StyledTextField
            label="Токен доступа"
            name="token"
            value={formData.token}
            onChange={handleChange}
            variant="standard"
            fullWidth
          />

          <PrimaryButton 
            type="submit"
            variant="contained"
            disabled={loginMutation.isPending}
          >
            Войти
          </PrimaryButton>
        </Box>

        <FormText>Вы не администратор? Зарегистрироваться как участник!</FormText>
        <SecondaryButton 
          variant="contained" 
          onClick={goToUserLogin}
        >
          Вход для участников
        </SecondaryButton>
      </FormContainer>
    </AuthLayout>
  );
}; 