import LocalLibraryIcon from '@mui/icons-material/LocalLibrary';
import { AppBar, Toolbar, Typography } from '@mui/material';

export function LibraryAppBar() {
  return (
    <AppBar position="static" color="primary" elevation={0}>
      <Toolbar>
        <LocalLibraryIcon sx={{ mr: 1.5 }} />
        <Typography variant="h1" component="h1" sx={{ flexGrow: 1 }}>
          Crumb-to-Crumb Library
        </Typography>
      </Toolbar>
    </AppBar>
  );
}
