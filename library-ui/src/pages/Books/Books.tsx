import AddIcon from '@mui/icons-material/Add';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import LoginIcon from '@mui/icons-material/Login';
import LogoutIcon from '@mui/icons-material/Logout';
import SearchIcon from '@mui/icons-material/Search';
import {
  Alert,
  Box,
  Button,
  Chip,
  Container,
  FormControl,
  IconButton,
  InputAdornment,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Tooltip,
  Typography,
} from '@mui/material';
import { FormEvent, useEffect, useMemo, useState } from 'react';
import {
  borrowBook,
  createBook,
  createEmployee,
  deleteBook,
  getBooks,
  getEmployees,
  returnBook,
} from '../../api';
import { AddBookDialog } from '../../components/AddBookDialog';
import { AddEmployeeDialog } from '../../components/AddEmployeeDialog';
import { BorrowBookDialog } from '../../components/BorrowBookDialog';
import {
  Book,
  BookStatus,
  BorrowBookRequest,
  CreateBookRequest,
  CreateEmployeeRequest,
  Employee,
  PagedResponse,
} from '../../types';
import './Books.css';

const statusOptions: Array<BookStatus | 'All'> = ['All', BookStatus.Available, BookStatus.Borrowed];

const emptyNewBookForm: CreateBookRequest = {
  title: '',
  author: '',
  isbn: '',
  ownerId: 0,
  notes: '',
};

const emptyBorrower: BorrowBookRequest = {
  borrowerId: 0,
};

const emptyEmployee: CreateEmployeeRequest = {
  firstName: '',
  lastName: '',
  teamName: '',
};

const emptyBookPage: PagedResponse<Book> = {
  items: [],
  page: 1,
  pageSize: 20,
  totalCount: 0,
  totalPages: 0,
};

export function Books() {
  const [books, setBooks] = useState<Book[]>([]);
  const [bookPage, setBookPage] = useState<PagedResponse<Book>>(emptyBookPage);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [search, setSearch] = useState('');
  const [status, setStatus] = useState<BookStatus | 'All'>('All');
  const [page, setPage] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [borrowTarget, setBorrowTarget] = useState<Book | null>(null);
  const [borrower, setBorrower] = useState<BorrowBookRequest>(emptyBorrower);
  const [newBookForm, setNewBookForm] = useState<CreateBookRequest>(emptyNewBookForm);
  const [isEmployeeOpen, setIsEmployeeOpen] = useState(false);
  const [employeeForm, setEmployeeForm] = useState<CreateEmployeeRequest>(emptyEmployee);
  const [selectNewEmployeeFor, setSelectNewEmployeeFor] = useState<'owner' | 'borrower'>('owner');

  const formOwnerValue = useMemo(
    () => employees.find((employee) => employee.id === newBookForm.ownerId) ?? null,
    [newBookForm.ownerId, employees],
  );

  const borrowerValue = useMemo(
    () => employees.find((employee) => employee.id === borrower.borrowerId) ?? null,
    [borrower.borrowerId, employees],
  );

  async function loadBooks(pageToLoad = page) {
    setIsLoading(true);
    setError(null);

    try {
      const nextPage = await getBooks(search, status, pageToLoad);
      setBookPage(nextPage);
      setBooks(nextPage.items);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to load books.');
    } finally {
      setIsLoading(false);
    }
  }

  async function loadEmployees() {
    try {
      setEmployees(await getEmployees());
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to load employees.');
    }
  }

  useEffect(() => {
    void loadBooks();
  }, [search, status, page]);

  useEffect(() => {
    void loadEmployees();
  }, []);

  const displayPage = bookPage.totalPages === 0 ? 1 : bookPage.page;
  const displayTotalPages = Math.max(bookPage.totalPages, 1);
  const canGoPrevious = page > 1 && !isLoading;
  const canGoNext = bookPage.totalPages > 0 && page < bookPage.totalPages && !isLoading;

  async function handleCreate(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError(null);

    if (!newBookForm.ownerId) {
      setError('Select an owner for the book.');
      return;
    }

    try {
      await createBook(newBookForm);
      setNewBookForm(emptyNewBookForm);
      setIsAddOpen(false);
      await Promise.all([loadBooks(), loadEmployees()]);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to add book.');
    }
  }

  async function handleBorrow(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!borrowTarget || !borrower.borrowerId) {
      return;
    }

    try {
      await borrowBook(borrowTarget.id, borrower);
      setBorrower(emptyBorrower);
      setBorrowTarget(null);
      await Promise.all([loadBooks(), loadEmployees()]);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to borrow book.');
    }
  }

  async function handleReturn(book: Book) {
    try {
      await returnBook(book.id);
      await loadBooks();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to return book.');
    }
  }

  async function handleDelete(book: Book) {
    if (!window.confirm(`Delete "${book.title}"?`)) {
      return;
    }

    try {
      await deleteBook(book.id);
      await loadBooks();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to delete book.');
    }
  }

  async function handleCreateEmployee(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    try {
      const employee = await createEmployee(employeeForm);
      await loadEmployees();
      setEmployeeForm(emptyEmployee);
      setIsEmployeeOpen(false);

      if (selectNewEmployeeFor === 'owner') {
        setNewBookForm({ ...newBookForm, ownerId: employee.id });
      } else {
        setBorrower({ borrowerId: employee.id });
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to add employee.');
    }
  }

  function openEmployeeDialog(target: 'owner' | 'borrower') {
    setSelectNewEmployeeFor(target);
    setEmployeeForm(emptyEmployee);
    setIsEmployeeOpen(true);
  }

  return (
    <>
      <Container maxWidth="lg" className="content">
        <Stack spacing={3}>
          <Paper className="toolbar-panel" elevation={0}>
            <Box className="filter-grid">
              <TextField
                fullWidth
                label="Search books, authors, owners, teams"
                value={search}
                onChange={(event) => {
                  setPage(1);
                  setSearch(event.target.value);
                }}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <SearchIcon />
                    </InputAdornment>
                  ),
                }}
              />
              <FormControl fullWidth>
                <InputLabel>Status</InputLabel>
                <Select
                  label="Status"
                  value={status}
                  onChange={(event) => {
                    setPage(1);
                    setStatus(event.target.value as BookStatus | 'All');
                  }}
                >
                  {statusOptions.map((option) => (
                    <MenuItem value={option} key={option}>
                      {option}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
              <Box className="toolbar-action">
                <Button
                  fullWidth
                  startIcon={<AddIcon />}
                  variant="contained"
                  onClick={() => setIsAddOpen(true)}
                >
                  Add a book
                </Button>
              </Box>
            </Box>
          </Paper>
          {error && <Alert severity="error">{error}</Alert>}
          <Box className="pagination-bar">
            <Typography fontWeight={600}>
              Page {displayPage} of {displayTotalPages}
            </Typography>
            <Stack direction="row" spacing={1}>
              <Tooltip title="Previous page">
                <span>
                  <IconButton
                    aria-label="Previous page"
                    disabled={!canGoPrevious}
                    onClick={() => setPage((currentPage) => Math.max(1, currentPage - 1))}
                  >
                    <ChevronLeftIcon />
                  </IconButton>
                </span>
              </Tooltip>
              <Tooltip title="Next page">
                <span>
                  <IconButton
                    aria-label="Next page"
                    disabled={!canGoNext}
                    onClick={() => setPage((currentPage) => currentPage + 1)}
                  >
                    <ChevronRightIcon />
                  </IconButton>
                </span>
              </Tooltip>
            </Stack>
          </Box>
          {/** Book grid view for mobile screens */}
          <Box className="book-grid">
            {books.map((book) => (
              <Paper className="book-card" elevation={0} key={book.id}>
                <Stack spacing={2} height="100%">
                  <Stack
                    direction="row"
                    spacing={1}
                    justifyContent="space-between"
                    alignItems="flex-start"
                  >
                    <Box>
                      <Typography variant="h2">{book.title}</Typography>
                      <Typography color="text.secondary">{book.author}</Typography>
                    </Box>
                    <Chip
                      size="small"
                      label={book.status}
                      color={book.status === BookStatus.Available ? 'success' : 'warning'}
                    />
                  </Stack>

                  <Box className="book-meta">
                    <Typography variant="body2">Owner</Typography>
                    <Typography fontWeight={600}>
                      {book.ownerFirstName} {book.ownerLastName} - {book.ownerTeam}
                    </Typography>
                  </Box>

                  {book.notes && <Typography color="text.secondary">{book.notes}</Typography>}

                  <Box flexGrow={1} />

                  <Stack direction="row" spacing={1} justifyContent="space-between">
                    {book.status === BookStatus.Available ? (
                      <Button
                        startIcon={<LogoutIcon />}
                        variant="contained"
                        onClick={() => setBorrowTarget(book)}
                      >
                        Borrow
                      </Button>
                    ) : (
                      <Button
                        startIcon={<LoginIcon />}
                        variant="outlined"
                        onClick={() => handleReturn(book)}
                      >
                        Return
                      </Button>
                    )}
                    <Tooltip title="Delete book">
                      <IconButton
                        aria-label={`Delete ${book.title}`}
                        color="error"
                        onClick={() => handleDelete(book)}
                      >
                        <DeleteOutlineIcon />
                      </IconButton>
                    </Tooltip>
                  </Stack>
                </Stack>
              </Paper>
            ))}
          </Box>
          {/** Book table view for larger screens */}
          <TableContainer component={Paper} className="book-table" elevation={0}>
            <Table size="small" aria-label="Books">
              <TableHead>
                <TableRow>
                  <TableCell>Book</TableCell>
                  <TableCell>Owner</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell align="center">Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {books.map((book) => (
                  <TableRow key={book.id} hover>
                    <TableCell>
                      <Typography fontWeight={600}>{book.title}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        {book.author}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Typography fontWeight={600}>
                        {book.ownerFirstName} {book.ownerLastName}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {book.ownerTeam}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Chip
                        size="small"
                        label={book.status}
                        color={book.status === BookStatus.Available ? 'success' : 'warning'}
                      />
                    </TableCell>
                    <TableCell align="right">
                      <Stack direction="row" spacing={1} justifyContent="flex-end">
                        {book.status === BookStatus.Available ? (
                          <Tooltip title="Borrow book">
                            <IconButton
                              aria-label={`Borrow ${book.title}`}
                              color="primary"
                              size="small"
                              sx={{ transform: 'rotate(-90deg)' }}
                              onClick={() => setBorrowTarget(book)}
                            >
                              <LogoutIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                        ) : (
                          <Tooltip title="Return book">
                            <IconButton
                              aria-label={`Return ${book.title}`}
                              color="primary"
                              size="small"
                              sx={{ transform: 'rotate(90deg)' }}
                              onClick={() => handleReturn(book)}
                            >
                              <LoginIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                        )}
                        <Tooltip title="Delete book">
                          <IconButton
                            aria-label={`Delete ${book.title}`}
                            color="error"
                            size="small"
                            onClick={() => handleDelete(book)}
                          >
                            <DeleteOutlineIcon fontSize="small" />
                          </IconButton>
                        </Tooltip>
                      </Stack>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          {!isLoading && books.length === 0 && (
            <Paper className="empty-state" elevation={0}>
              <Typography>No books match the current filters.</Typography>
            </Paper>
          )}
        </Stack>
      </Container>

      <AddBookDialog
        employees={employees}
        form={newBookForm}
        formOwnerValue={formOwnerValue}
        open={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        onOpenEmployeeDialog={() => openEmployeeDialog('owner')}
        onSubmit={handleCreate}
        onUpdateForm={setNewBookForm}
      />

      <AddEmployeeDialog
        form={employeeForm}
        open={isEmployeeOpen}
        onClose={() => setIsEmployeeOpen(false)}
        onSubmit={handleCreateEmployee}
        onUpdateForm={setEmployeeForm}
      />

      <BorrowBookDialog
        book={borrowTarget}
        borrowerValue={borrowerValue}
        employees={employees}
        onClose={() => setBorrowTarget(null)}
        onOpenEmployeeDialog={() => openEmployeeDialog('borrower')}
        onSubmit={handleBorrow}
        onUpdateBorrower={setBorrower}
      />
    </>
  );
}
