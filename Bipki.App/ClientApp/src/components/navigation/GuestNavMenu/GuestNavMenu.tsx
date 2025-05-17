import { Box, BottomNavigation, BottomNavigationAction, Paper } from '@mui/material';
import { useNavigate, useLocation, Outlet } from 'react-router-dom';
import Favorite from '@mui/icons-material/Favorite';

export const GuestNavMenu = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const isActivityPage = location.pathname.startsWith('/activity/');

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <Box component="main" sx={{ flexGrow: 1, pb: 7 }}>
        <Outlet />
      </Box>  
      <Paper sx={{ position: 'fixed', bottom: 0, left: 0, right: 0 }} elevation={3}>
        <BottomNavigation
          value={isActivityPage ? '/activity/1' : location.pathname}
          onChange={(_, newValue) => {
            if (newValue !== '/activity/1') {
              navigate(newValue);
            }
          }}
          showLabels
          sx={{
            backgroundColor: '#2D2D2D',
            '& .MuiBottomNavigationAction-root': {
              color: '#FFFFFF99',
              '&.Mui-selected': {
                color: '#A980E0'
              }
            }
          }}
        >
          <BottomNavigationAction
            label="О мероприятии"
            value="/activity/1"
            icon={<Favorite />}
            disabled
          />
          <BottomNavigationAction
            label="Программа"
            value="/plan"
            icon={<Favorite />}
          />
          <BottomNavigationAction
            label="Карта"
            value="/map"
            icon={<Favorite />}
          />
        </BottomNavigation>
      </Paper>
    </Box>
  );
}; 