import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation, useParams } from 'react-router-dom';
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
import { login } from '../../../api/authApi';
import { LoginCredentials } from '../../../types/Auth';
import { useUser } from '../../../hooks/useUser';

export const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { conferenceId } = useParams();
  const [formData, setFormData] = useState<LoginCredentials>({
    telegram: '',
    password: '',
    conferenceId: conferenceId ?? '',
  });

  const { user, isLoading } = useUser();

  const loginMutation = useMutation({
    mutationFn: login,
    onSuccess: () => {
      navigate('/activity');
    },
  });

  useEffect(() => {
    if (user && !isLoading) {
      navigate('/activity');
    }
  }, [user, isLoading, navigate]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    loginMutation.mutate(formData);
  };

  const goToRegister = () => {
    // Сохраняем conferenceId при переходе на страницу регистрации
    const searchParams = new URLSearchParams(location.search);
    const conferenceId = searchParams.get('conferenceId');
    
    if (conferenceId) {
      navigate(`/register?conferenceId=${conferenceId}`);
    } else {
      navigate('/register');
    }
  };

  const goToAdminLogin = () => {
    navigate('/admin/login');
  };

  return (
    <AuthLayout title="Войти как участник">
      <FormContainer>
        <Box component="form" onSubmit={handleLogin} sx={{ width: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          <StyledTextField
            label="Телеграм"
            name="telegram"
            value={formData.telegram}
            onChange={handleChange}
            variant="standard"
            fullWidth
          />
          <StyledTextField
            label="Пароль"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
            variant="standard"
            fullWidth
          />
          <StyledTextField
            label="ID конференции"
            name="conferenceId"
            value={formData.conferenceId}
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

        <FormText>Вы ещё не регистрировались? Зарегистрируйтесь!</FormText>
        <SecondaryButton 
          variant="contained" 
          onClick={goToRegister}
        >
          Зарегистрироваться
        </SecondaryButton>

        <FormText>Вы администратор? Для вас отдельная кнопочка!</FormText>
        <SecondaryButton 
          variant="contained" 
          onClick={goToAdminLogin}
        >
          Войти как администратор
        </SecondaryButton>
      </FormContainer>
    </AuthLayout>
  );
}; 