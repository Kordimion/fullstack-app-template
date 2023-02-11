import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './components/App';
import reportWebVitals from './reportWebVitals';
import { authority, redirectUri, clientId, clientSecret } from "./constants";

import { AuthProviderProps, AuthProvider } from "oidc-react";

const oidcConfig : AuthProviderProps = {
  authority: authority,
  clientId: clientId,
  scope: "catalog.api.read openid profile",
  clientSecret: clientSecret,
  redirectUri: redirectUri,
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
