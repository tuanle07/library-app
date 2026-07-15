export enum BookStatus {
  Available = 'Available',
  Borrowed = 'Borrowed',
}

export interface Book {
  id: number;
  title: string;
  author: string;
  isbn?: string | null;
  ownerId: number;
  ownerFirstName: string;
  ownerLastName: string;
  ownerTeam: string;
  status: BookStatus;
  borrowerId?: number | null;
  borrowerFirstName?: string | null;
  borrowerLastName?: string | null;
  borrowerTeam?: string | null;
  borrowedOn?: string | null;
  notes?: string | null;
}

export interface CreateBookRequest {
  title: string;
  author: string;
  isbn?: string;
  ownerId: number;
  notes?: string;
}

export interface BorrowBookRequest {
  borrowerId: number;
}

export interface UpdateBookRequest {
  title: string;
  author: string;
  isbn?: string;
  ownerId: number;
  status: BookStatus;
  borrowerId?: number | null;
  borrowedOn?: string | null;
  notes?: string;
}

export interface Employee {
  id: number;
  firstName: string;
  lastName: string;
  teamName: string;
}

export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  teamName: string;
}

export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
