import { useMemo } from "react";
import { RouterProvider } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { createRouter } from "./router";
import { ThemeProvider, createTheme, CssBaseline } from "@mui/material";
import "./custom.css";
import { ChatProvider } from "contexts/ChatContext";

interface AppProps {
  basename: string;
}

const queryClient = new QueryClient();

const theme = createTheme({
  palette: {
    mode: 'dark',
    background: {
      default: '#2D2D2D',
      paper: '#2D2D2D',
    },
    primary: {
      main: '#A980E0',
      contrastText: '#2D2D2D',
    },
    secondary: {
      main: '#2D2D2D',
      contrastText: '#A980E0',
    },
    info: {
      main: "#FFFFFF",
      contrastText: "#2D2D2D",
    },
  },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          backgroundColor: '#2D2D2D',
          color: '#A980E0',
        },
        fontFamily: "Static/Body Small/Font",
      },
    },
    MuiTypography: {
      styleOverrides: {
        root: {
          fontFamily: "Static/Body Small/Font",
        },
      },
    },
  },
  breakpoints: {
    values: {
      xs: 0,
      sm: 600,
      md: 960,
      lg: 1280,
      xl: 1920,
    },
  },
});

export default function App({ basename }: AppProps) {
  const router = useMemo(() => createRouter(basename), [basename]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <QueryClientProvider client={queryClient}>
        <ChatProvider>
          <RouterProvider router={router} />
        </ChatProvider>
      </QueryClientProvider>
    </ThemeProvider>
  );
}
