import { createBrowserRouter } from "react-router-dom";
import { ProtectedRoute } from "./components/auth/protectedRoute/ProtectedRoute";
import { Role } from "./types/User";
import { GuestNavMenu } from "components/navigation/GuestNavMenu/GuestNavMenu";
import { ActivityPage } from "./pages/user/ActivityPage/ActivityPage";
import { Plan } from "components/pages/plan/Plan";
import { LoginPage } from "./pages/user/LoginPage/LoginPage";
import { RegisterPage } from "./pages/user/RegisterPage/RegisterPage";
import { AdminLoginPage } from "./pages/user/AdminLoginPage/AdminLoginPage";
import { Navigate } from "react-router-dom";

export const createRouter = (basename: string) =>
  createBrowserRouter(
    [
      {
        path: "/login",
        element: <LoginPage />,
      },
      {
        path: "/register",
        element: <RegisterPage />,
      },
      {
        path: "/admin/login",
        element: <AdminLoginPage />,
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
        element: <ProtectedRoute requiredRole={Role.User} />,
        children: [
          {
            element: <GuestNavMenu />,
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
      },
      {
        path: "*",
        element: <Navigate to="/login" />
      }
    ],
    { basename }
  );
