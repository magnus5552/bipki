import { createBrowserRouter } from "react-router-dom";
import { ProtectedRoute } from "./components/auth/protectedRoute/ProtectedRoute";
import { Role } from "./types/User";
import { Login } from "./components/pages/login/Login";
import { GuestNavMenu } from 'components/navigation/GuestNavMenu/GuestNavMenu';
export const createRouter = (basename: string) =>
  createBrowserRouter(
    [
      {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/admin",
        element: <ProtectedRoute requiredRole={Role.Admin} />,
        children: [
          {
            index: true,
            element: <div>Admin Dashboard</div>,
          },
          {
            path: "users",
            element: <div>User Management</div>,
          },
          {
            path: "settings",
            element: <div>Admin Settings</div>,
          },
        ],
      },
      {
        element: /*<ProtectedRoute requiredRole={Role.User} /> */ <GuestNavMenu />,
        children: [
          {
            path: "/conference",
            element: <div>User Dashboard</div>,
          },
          {
            path: "/plan",
            element: <div>User Plan</div>,
          },
          {
            path: "/map",
            element: <div>User Map</div>,
          },
        ],
      },
    ],
    { basename }
  );
