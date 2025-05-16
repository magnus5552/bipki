import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';

interface AppProps {}

interface AppState {}

const queryClient = new QueryClient();

export default class App extends Component<AppProps, AppState> {
  static displayName = App.name;

  render() {
    return (
      <QueryClientProvider client={queryClient}>
        <Layout>
          <Routes>
            {AppRoutes.map((route, index) => {
              const { element, ...rest } = route;
              return <Route key={index} {...rest} element={element} />;
            })}
          </Routes>
        </Layout>
      </QueryClientProvider>
    );
  }
} 