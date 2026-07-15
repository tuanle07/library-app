import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Stack,
  TextField,
} from '@mui/material';
import { FormEvent } from 'react';
import { CreateEmployeeRequest } from '../types';

interface AddEmployeeDialogProps {
  form: CreateEmployeeRequest;
  open: boolean;
  onClose: () => void;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
  onUpdateForm: (form: CreateEmployeeRequest) => void;
}

export function AddEmployeeDialog({
  form,
  open,
  onClose,
  onSubmit,
  onUpdateForm,
}: AddEmployeeDialogProps) {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <Box component="form" onSubmit={onSubmit}>
        <DialogTitle>Add Employee</DialogTitle>
        <DialogContent>
          <Stack spacing={2} pt={1}>
            <TextField
              autoFocus
              required
              label="First name"
              value={form.firstName}
              onChange={(event) => onUpdateForm({ ...form, firstName: event.target.value })}
            />
            <TextField
              required
              label="Last name"
              value={form.lastName}
              onChange={(event) => onUpdateForm({ ...form, lastName: event.target.value })}
            />
            <TextField
              required
              label="Team"
              value={form.teamName}
              onChange={(event) => onUpdateForm({ ...form, teamName: event.target.value })}
            />
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" variant="contained">
            Add
          </Button>
        </DialogActions>
      </Box>
    </Dialog>
  );
}
