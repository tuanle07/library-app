import axios from 'axios';

import {
  Book,
  BookStatus,
  BorrowBookRequest,
  CreateBookRequest,
  CreateEmployeeRequest,
  Employee,
  PagedResponse,
  UpdateBookRequest,
} from './types';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});

export async function getBooks(
  search: string,
  status: BookStatus | 'All',
  page = 1,
): Promise<PagedResponse<Book>> {
  const params = new URLSearchParams();
  params.set('page', page.toString());

  if (search.trim()) {
    params.set('search', search.trim());
  }

  if (status !== 'All') {
    params.set('status', status);
  }

  const { data } = await api.get<PagedResponse<Book>>('/api/books', { params });
  return data;
}

export async function createBook(payload: CreateBookRequest): Promise<Book> {
  const { data } = await api.post<Book>('/api/books', payload);
  return data;
}

export async function deleteBook(id: number): Promise<void> {
  await api.delete(`/api/books/${id}`);
}

export async function updateBook(id: number, payload: UpdateBookRequest): Promise<Book> {
  const { data } = await api.put<Book>(`/api/books/${id}`, payload);
  return data;
}

export async function borrowBook(id: number, payload: BorrowBookRequest): Promise<Book> {
  const { data } = await api.post<Book>(`/api/books/${id}/borrow`, payload);
  return data;
}

export async function returnBook(id: number): Promise<Book> {
  const { data } = await api.post<Book>(`/api/books/${id}/return`);
  return data;
}

export async function getEmployees(): Promise<Employee[]> {
  const { data } = await api.get<Employee[]>('/api/employees');
  return data;
}

export async function createEmployee(payload: CreateEmployeeRequest): Promise<Employee> {
  const { data } = await api.post<Employee>('/api/employees', payload);
  return data;
}
