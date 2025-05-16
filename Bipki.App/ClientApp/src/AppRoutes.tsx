import { ReactElement } from 'react';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { ProtectedRoute } from './components/auth/protectedRoute/ProtectedRoute';
import { Role } from './types/User';

interface AppRoute {
  index?: boolean;
  path?: string;
  element: ReactElement;
}

const AppRoutes: AppRoute[] = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/admin',
    element: <ProtectedRoute requiredRole={Role.Admin} />
  },
  {
    path: '/user',
    element: <ProtectedRoute requiredRole={Role.User} />
  }
];

export default AppRoutes; 