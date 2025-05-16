import { useMemo } from "react";
import { RouterProvider } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { createRouter } from "./router";
import "./custom.css";

interface AppProps {
  basename: string;
}

const queryClient = new QueryClient();

export default function App({ basename }: AppProps) {
  const router = useMemo(() => createRouter(basename), [basename]);

  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  );
}
