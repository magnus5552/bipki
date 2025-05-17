import { TextField, Button, Typography, Box, styled } from '@mui/material';

export const StyledTextField = styled(TextField)({
  width: '265px',
  marginBottom: '12px',
  '& .MuiInput-underline:before': {
    borderBottomColor: '#A980E0',
  },
  '& .MuiInput-underline:after': {
    borderBottomColor: '#A980E0',
  },
  '& .MuiInputBase-input': {
    color: '#A980E0',
  },
  '& .MuiInputLabel-root': {
    color: '#FFFFFF',
    fontSize: '12px',
    lineHeight: '12px',
  },
});

export const PrimaryButton = styled(Button)(({ theme }) => ({
  width: '265px',
  height: '36px',
  marginTop: theme.spacing(1),
  marginBottom: theme.spacing(2),
  backgroundColor: '#A980E0',
  color: theme.palette.common.black,
  '&:hover': {
    backgroundColor: '#8a68b8',
  },
}));

export const SecondaryButton = styled(Button)(({ theme }) => ({
  width: '265px',
  height: '36px',
  marginTop: theme.spacing(1),
  marginBottom: theme.spacing(2),
  backgroundColor: theme.palette.common.white,
  color: theme.palette.grey[900],
  '&:hover': {
    backgroundColor: theme.palette.common.white,
  },
}));

export const FormText = styled(Typography)(({ theme }) => ({
  fontSize: '12px',
  lineHeight: '16px',
  letterSpacing: '0.4px',
  textAlign: 'center',
  color: '#A980E0',
  marginTop: theme.spacing(2),
  marginBottom: theme.spacing(1),
  width: '100%',
}));

export const FormContainer = styled(Box)({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  width: '100%',
}); 