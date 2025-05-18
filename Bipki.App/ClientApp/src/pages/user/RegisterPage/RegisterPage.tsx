import React, { useState, useEffect } from 'react';
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

export const RegisterPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { conferenceId } = useParams();

  const [formData, setFormData] = useState<RegisterCredentials>({
    name: '',
    surname: '',
    telegram: '',
    password: '',
    conferenceId: conferenceId ?? '',
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: () => {
      // Сохраняем conferenceId при переходе на страницу логина
      if (formData.conferenceId) {
        navigate(`/login?conferenceId=${formData.conferenceId}`);
      } else {
        navigate('/login');
      }
    },
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();
    registerMutation.mutate(formData);
  };

  const goToLogin = () => {
    // Сохраняем conferenceId при переходе на страницу логина
    const searchParams = new URLSearchParams(location.search);
    const conferenceId = searchParams.get('conferenceId');
    
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