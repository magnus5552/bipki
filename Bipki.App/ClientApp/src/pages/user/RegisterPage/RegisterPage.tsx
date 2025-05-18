import React, { useState } from 'react';
import { useNavigate, useLocation, useParams } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { AuthLayout } from '../../../components/auth/AuthLayout/AuthLayout';
import {
  FormContainer,
  FormText,
  PrimaryButton,
  SecondaryButton,
  StyledTextField
} from '../../../components/auth/AuthComponents/AuthComponents';
import { register } from '../../../api/authApi';
import { RegisterCredentials } from '../../../types/Auth';
import { Box } from '@mui/material';
import { useUser } from '../../../hooks/useUser';
import { Role } from '../../../types/User';

export const RegisterPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { conferenceId } = useParams();

  const { user, isLoading } = useUser();

  const [formData, setFormData] = useState<RegisterCredentials>({
    name: '',
    surname: '',
    telegram: '',
    password: '',
    conferenceId: conferenceId ?? '',
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: (data) => {
      if (data.role === Role.Admin) {
        navigate('/admin/conferences');
      } else {
        if (formData.conferenceId) {
          navigate(`/login?conferenceId=${formData.conferenceId}`);
        } else {
          navigate('/login');
        }
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

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();
    registerMutation.mutate(formData);
  };

  const goToLogin = () => {    
    if (conferenceId) {
      navigate(`/login?conferenceId=${conferenceId}`);
    } else {
      navigate('/login');
    }
  };

  const goToAdminLogin = () => {
    navigate('/admin/login');
  };

  return (
    <AuthLayout title="Зарегистрироваться">
      <FormContainer>
        <Box component="form" onSubmit={handleRegister} sx={{ width: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          <StyledTextField
            label="Имя"
            name="name"
            value={formData.name}
            onChange={handleChange}
            variant="standard"
            fullWidth
          />
          <StyledTextField
            label="Фамилия"
            name="surname"
            value={formData.surname}
            onChange={handleChange}
            variant="standard"
            fullWidth
          />
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
            label="Id конференции"
            name="conferenceId"
            value={formData.conferenceId}
            onChange={handleChange}
            variant="standard"
            fullWidth
          />

          <PrimaryButton 
            type="submit"
            variant="contained"
            disabled={registerMutation.isPending}
          >
            Зарегистрироваться
          </PrimaryButton>
        </Box>

        <FormText>Уже записаны на мероприятие? Войдите!</FormText>
        <SecondaryButton 
          variant="contained" 
          onClick={goToLogin}
        >
          Войти
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