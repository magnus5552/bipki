import { createBrowserRouter } from "react-router-dom";
import { ProtectedRoute } from "./components/auth/protectedRoute/ProtectedRoute";
import { Role } from "./types/User";
import { Login } from "./components/pages/login/Login";
import { GuestNavMenu } from 'components/navigation/GuestNavMenu/GuestNavMenu';
import { ActivityPage } from "./pages/user/ActivityPage/ActivityPage";
import { Plan } from 'components/pages/plan/Plan';

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
            path: "/activity/:activityId",
            element: <ActivityPage />,
          },
          {
            path: "/plan",
            element: <Plan />,
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
