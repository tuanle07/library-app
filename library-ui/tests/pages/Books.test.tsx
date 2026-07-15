import { cleanup, render, waitFor } from '@testing-library/react';
import { afterEach, describe, expect, it, vi } from 'vitest';

import { Books } from '../../src/pages/Books';

vi.mock('../../src/api', () => ({
  borrowBook: vi.fn(),
  createBook: vi.fn(),
  createEmployee: vi.fn(),
  deleteBook: vi.fn(),
  getBooks: vi.fn().mockResolvedValue({
    items: [
      {
        id: 1,
        title: 'Clean Code',
        author: 'Robert C. Martin',
        isbn: '9780132350884',
        ownerId: 7,
        ownerFirstName: 'Ada',
        ownerLastName: 'Lovelace',
        ownerTeam: 'Engineering',
        status: 'Available',
        borrowerId: null,
        borrowerFirstName: null,
        borrowerLastName: null,
        borrowerTeam: null,
        borrowedOn: null,
        notes: 'Keep near the pairing station.',
      },
      {
        id: 2,
        title: 'Refactoring',
        author: 'Martin Fowler',
        isbn: '9780201485677',
        ownerId: 7,
        ownerFirstName: 'Ada',
        ownerLastName: 'Lovelace',
        ownerTeam: 'Engineering',
        status: 'Available',
        borrowerId: null,
        borrowerFirstName: null,
        borrowerLastName: null,
        borrowerTeam: null,
        borrowedOn: null,
        notes: null,
      },
    ],
    page: 1,
    pageSize: 20,
    totalCount: 2,
    totalPages: 1,
  }),
  getEmployees: vi.fn().mockResolvedValue([
    {
      id: 7,
      firstName: 'Ada',
      lastName: 'Lovelace',
      teamName: 'Engineering',
    },
  ]),
  returnBook: vi.fn(),
}));

afterEach(() => {
  cleanup();
});

describe('Books', () => {
  it('matches the snapshot after loading books', async () => {
    const { asFragment, getAllByText } = render(<Books />);

    await waitFor(() => {
      expect(getAllByText('Clean Code').length).toBeGreaterThan(0);
    });

    expect(asFragment()).toMatchSnapshot();
  });
});
