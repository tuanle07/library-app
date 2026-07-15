# Crumb-to-Crumb Library

A full-stack library lending app for managing an internal engineering book collection. Employees can add books, search the catalogue, filter by availability, borrow available books, return borrowed books, and add new employees during the lending flow.

## Stack

- Backend: ASP.NET Core 9, EF Core in-memory database, Swagger UI
- Frontend: React 18, TypeScript, Vite, Material UI
- Solution: `LibraryApp.sln`

## Project Structure

```text
library-app/
  library-api/LibraryApp.Api/   ASP.NET Core API
  library-ui/                   React/Vite frontend
  LibraryApp.sln                .NET solution
```

## Prerequisites

- .NET 9 SDK
- Node.js and npm

## Run the API

From the repository root:

```powershell
$env:DOTNET_CLI_HOME="$PWD\.dotnet-home"
dotnet restore .\LibraryApp.sln
dotnet run --project .\library-api\LibraryApp.Api\LibraryApp.Api.csproj
```

The API starts on the URLs configured in `library-api/LibraryApp.Api/Properties/launchSettings.json`:

- `https://localhost:7055`
- `http://localhost:5133`

Swagger UI is available in development at `/swagger`, for example `https://localhost:7055/swagger`.

The API uses an in-memory database and seeds sample employees and books on startup. Data resets when the API process restarts.

## Run the Frontend

In a second terminal:

```powershell
cd .\library-ui
npm install
```

Create a local environment file:

```powershell
"VITE_API_BASE_URL=https://localhost:7055" | Out-File -Encoding utf8 .env
```

Then start Vite:

```powershell
npm run dev
```

The frontend runs at `http://localhost:5173`.

If you run the API over HTTP instead of HTTPS, set `VITE_API_BASE_URL=http://localhost:5133`.

## Available Scripts

From `library-ui/`:

```powershell
npm run dev
npm run build
npm run preview
npm run format
npm run format:check
```

From the repository root:

```powershell
dotnet build .\LibraryApp.sln
```

## API Overview

Books:

- `GET /api/books?search=&status=&page=&pageSize=`
- `GET /api/books/{id}`
- `POST /api/books`
- `PUT /api/books/{id}`
- `POST /api/books/{id}/borrow`
- `POST /api/books/{id}/return`
- `DELETE /api/books/{id}`

Employees:

- `GET /api/employees`
- `GET /api/employees/{id}`
- `POST /api/employees`

## Notes

- CORS is configured for the Vite dev server at `http://localhost:5173`.
- Book statuses are `Available` and `Borrowed`.
- The frontend expects API responses to use string enum values, which the API configures with `JsonStringEnumConverter`.

## AI Usage

I used AI assistance during this project to help plan, implement, debug, and document the full-stack library lending application. The AI was used as a development assistant, while I reviewed the generated suggestions and kept the implementation aligned with the assignment requirements.

Important prompts used during the project included:

- "Create a boilerplate full-stack app with an ASP.NET Core backend and a React TypeScript frontend using mui."
- "Use Entity Framework Core with an in-memory database and seed the application with sample books and employees where an employee can own or borrow a book."
- "Create simple endpoints to add, update, delete, borrow, and return books."
- "Add global exception handling middleware."
- "Add a theme for the UI with primary colour = #1b2b1d and secondary colour = #ff7230 and use Rethink Sans as font-family".
- "Add api.ts file that corresponds to the library-api endpoints responses and use axios"
- "Use MUI table component to display the list of books including title, author name, owner, status and actions (including borrow, return and delete icons) on desktop. On mobile devices please use card component instead."
- "Open add a book modal upon clicking on Add a book button."
- "Open borrow a book modal upon clicking on Borrow button."
- "Open add employee modal upon clicking on add employee button"
- "Connect the frontend to the ASP.NET Core API using a configurable Vite environment variable for the API base URL."
- "Add validation and user-friendly feedback for book and employee forms."
- "Update the README with setup instructions, available scripts, API endpoints, and notes about how the app works."

AI assistance was also used to check formatting, improve code organization, and identify small issues during development. Final decisions about the implementation, project structure, and submitted code were reviewed manually.
