import { createBrowserRouter } from "react-router-dom";
import { ProtectedRoute } from "./components/auth/protectedRoute/ProtectedRoute";
import { Role } from "./types/User";
import { GuestNavMenu } from "components/navigation/GuestNavMenu/GuestNavMenu";
import { ActivityPage } from "./pages/user/ActivityPage/ActivityPage";
import { Plan } from "pages/user/plan/Plan";
import { LoginPage } from "./pages/user/LoginPage/LoginPage";
import { RegisterPage } from "./pages/user/RegisterPage/RegisterPage";
import { AdminLoginPage } from "./pages/user/AdminLoginPage/AdminLoginPage";
import { Navigate } from "react-router-dom";
import { RegisteredPage } from "pages/user/RegisteredPage/RegisteredPage";
import { AdminNavMenu } from "components/navigation/AdminNavMenu/AdminNavMenu";
import { ConferencesList } from "pages/admin/ConferencesList/ConferencesList";
import { ActivitiesList } from "pages/admin/ActivitiesList/ActivitiesList";
import { ActivityEdit } from "pages/admin/ActivityEdit/ActivityEdit";
import { ConferenceEdit } from "pages/admin/ConferenceEdit/ConferenceEdit";
import { ChatPage } from "pages/user/ChatPage/ChatPage";

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
            path: "conferences",
            element: <ConferencesList />,
          },
          {
            path: "conferences/:conferenceId/edit",
            element: <ConferenceEdit />,
          },
          {
            path: "conferences/:conferenceId/activities/:activityId/edit",
            element: <ActivityEdit />,
          },
          {
            element: <AdminNavMenu />,
            path: "conferences/:conferenceId",
            children: [
              {
                path: "activities",
                element: <ActivitiesList />,
              },
              {
                index: true,
                element: <ActivityPage />,
              },
              {
                path: "map",
                element: <div>Admin Map</div>,
              },
              {
                path: "chat/:chatId",
                element: <ChatPage />,
              },
            ],
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
                path: "/activity",
                element: <ActivityPage />,
              },
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
              {
                path: "/registered",
                element: <RegisteredPage />,
              },
              {
                path: "/chat/:chatId",
                element: <ChatPage />,
              },
            ],
          },
        ],
      },
      {
        path: "*",
        element: <Navigate to="/activity" />,
      },
    ],
    { basename }
  );
