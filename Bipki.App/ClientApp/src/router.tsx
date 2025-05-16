import { createBrowserRouter } from "react-router-dom";
import { ProtectedRoute } from "./components/auth/protectedRoute/ProtectedRoute";
import { Role } from "./types/User";
import { Login } from "./components/pages/login/Login";
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
        path: "/user",
        element: <ProtectedRoute requiredRole={Role.User} />,
        children: [
          {
            index: true,
            element: <div>User Dashboard</div>,
          },
          {
            path: "profile",
            element: <div>User Profile</div>,
          },
        ],
      },
    ],
    { basename }
  );
