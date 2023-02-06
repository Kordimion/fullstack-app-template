import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './components/App';
import reportWebVitals from './reportWebVitals';

import { AuthProviderProps, AuthProvider } from "oidc-react";

const oidcConfig : AuthProviderProps = {
  authority: "https://localhost/identity/realms/shop",
  clientId: 'mainpage.spa.client',
  scope: "catalog.api.read openid profile",
  clientSecret: "AGVacqRHpG5PXbUXpaQ0kKiXartkh5LB",
  redirectUri: "https://localhost/redirect/",
  autoSignIn: false,
};

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <AuthProvider {...oidcConfig}>
      <App />
    </AuthProvider>    
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
