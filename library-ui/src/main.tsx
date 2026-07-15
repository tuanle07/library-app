import React from 'react';
import ReactDOM from 'react-dom/client';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './styles.css';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1b2b1d',
    },
    secondary: {
      main: '#ff7230',
    },
    background: {
      default: '#f7f8f5',
    },
  },
  shape: {
    borderRadius: 8,
  },
  typography: {
    fontFamily: '"Rethink Sans", "Inter", "Segoe UI", Arial, sans-serif',
    h1: {
      fontSize: '1.75rem',
      fontWeight: 600,
    },
    h2: {
      fontSize: '1.25rem',
      fontWeight: 600,
    },
  },
});

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </ThemeProvider>
  </React.StrictMode>,
);
