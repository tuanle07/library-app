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
  Typography,
} from '@mui/material';
import { FormEvent } from 'react';
import { Book, BorrowBookRequest, Employee } from '../../types';
import './BorrowBookDialog.css';

interface BorrowBookDialogProps {
  book: Book | null;
  borrowerValue: Employee | null;
  employees: Employee[];
  onClose: () => void;
  onOpenEmployeeDialog: () => void;
  onSubmit: (event: FormEvent<HTMLFormElement>) => void;
  onUpdateBorrower: (borrower: BorrowBookRequest) => void;
}

function employeeLabel(employee: Employee) {
  return `${employee.firstName} ${employee.lastName} - ${employee.teamName}`;
}

export function BorrowBookDialog({
  book,
  borrowerValue,
  employees,
  onClose,
  onOpenEmployeeDialog,
  onSubmit,
  onUpdateBorrower,
}: BorrowBookDialogProps) {
  return (
    <Dialog open={Boolean(book)} onClose={onClose} maxWidth="xs" fullWidth>
      <Box component="form" onSubmit={onSubmit}>
        <DialogTitle>Borrow Book</DialogTitle>
        <DialogContent>
          <Stack spacing={2} pt={1}>
            <Typography>{book?.title}</Typography>
            <Stack direction="row" spacing={1} alignItems="flex-start">
              <Autocomplete
                fullWidth
                options={employees.filter((e) => e.id !== book?.ownerId)}
                value={borrowerValue}
                getOptionLabel={employeeLabel}
                isOptionEqualToValue={(option, value) => option.id === value.id}
                onChange={(_, value) => onUpdateBorrower({ borrowerId: value?.id ?? 0 })}
                renderInput={(params) => (
                  <TextField {...params} autoFocus required label="Borrower" />
                )}
              />
              <Tooltip title="Add employee">
                <IconButton
                  className="borrow-book-dialog-employee-button"
                  color="primary"
                  onClick={onOpenEmployeeDialog}
                >
                  <PersonAddIcon />
                </IconButton>
              </Tooltip>
            </Stack>
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" variant="contained">
            Borrow
          </Button>
        </DialogActions>
      </Box>
    </Dialog>
  );
}
