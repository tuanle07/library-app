import PersonAddIcon from '@mui/icons-material/PersonAdd';
import {
  Autocomplete,
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  Stack,
  TextField,
  Tooltip,
} from '@mui/material';
import { FormEvent } from 'react';
import { CreateBookRequest, Employee } from '../../types';
import './AddBookDialog.css';

interface AddBookDialogProps {
  employees: Employee[];
  form: CreateBookRequest;
  formOwnerValue: Employee | null;
  open: boolean;
  onClose: () => void;
  onOpenEmployeeDialog: () => void;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
  onUpdateForm: (form: CreateBookRequest) => void;
}

function employeeLabel(employee: Employee) {
  return `${employee.firstName} ${employee.lastName} - ${employee.teamName}`;
}

export function AddBookDialog({
  employees,
  form,
  formOwnerValue,
  open,
  onClose,
  onOpenEmployeeDialog,
  onSubmit,
  onUpdateForm,
}: AddBookDialogProps) {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <Box component="form" onSubmit={onSubmit}>
        <DialogTitle>Add Book</DialogTitle>
        <DialogContent>
          <Stack spacing={2} pt={1}>
            <TextField
              required
              label="Title"
              value={form.title}
              onChange={(event) => onUpdateForm({ ...form, title: event.target.value })}
            />
            <TextField
              required
              label="Author"
              value={form.author}
              onChange={(event) => onUpdateForm({ ...form, author: event.target.value })}
            />
            <TextField
              label="ISBN"
              value={form.isbn}
              onChange={(event) => onUpdateForm({ ...form, isbn: event.target.value })}
            />
            <Stack direction="row" spacing={1} alignItems="flex-start">
              <Autocomplete
                fullWidth
                options={employees}
                value={formOwnerValue}
                getOptionLabel={employeeLabel}
                isOptionEqualToValue={(option, value) => option.id === value.id}
                onChange={(_, value) => onUpdateForm({ ...form, ownerId: value?.id ?? 0 })}
                renderInput={(params) => <TextField {...params} required label="Owner" />}
              />
              <Tooltip title="Add employee">
                <IconButton
                  className="add-book-dialog-employee-button"
                  color="primary"
                  onClick={onOpenEmployeeDialog}
                >
                  <PersonAddIcon />
                </IconButton>
              </Tooltip>
            </Stack>
            <TextField
              multiline
              minRows={3}
              label="Notes"
              value={form.notes}
              onChange={(event) => onUpdateForm({ ...form, notes: event.target.value })}
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
