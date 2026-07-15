import { Container, Paper, Typography } from '@mui/material';

export function NotFound() {
  return (
    <Container maxWidth="lg" className="content">
      <Paper className="empty-state" elevation={0}>
        <Typography>Page not found.</Typography>
      </Paper>
    </Container>
  );
}
