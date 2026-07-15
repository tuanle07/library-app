import { Box } from '@mui/material';
import { Navigate, Route, Routes } from 'react-router-dom';
import { LibraryAppBar } from './components/LibraryAppBar';
import { Books } from './pages/Books';
import { NotFound } from './pages/NotFound';

export default function App() {
  return (
    <Box className="app-shell">
      <LibraryAppBar />
      <Routes>
        <Route path="/" element={<Navigate to="/books" replace />} />
        <Route path="/books" element={<Books />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Box>
  );
}
