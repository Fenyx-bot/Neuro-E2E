# Neuro-E2E (E2EChat)

A small end-to-end chat app with:

- **Backend:** ASP.NET Core (net8.0) Web API + SignalR + EF Core (SQLite)
- **Frontend:** React + TypeScript (Vite) UI that talks to the API and connects to the SignalR hub

The goal of the project is a simple real-time chat experience: users can sign up / log in, create chats, load message history, and receive new messages live over SignalR.

## Repo structure

- `WebApplication1/Messaging/` — ASP.NET Core backend
  - REST API controllers: `Controllers/`
  - SignalR hub: `Hubs/ChatHub.cs`
  - EF Core + SQLite: `DAL/ApplicationDbContext.cs` (db file is `./messaging.db`)
  - Swagger (dev): `/swagger`
- `chat-ui/` — React + Vite frontend
  - API client: `src/api/api.ts` (base URL currently `http://localhost:5299`)
  - SignalR connection: `src/pages/ChatPage.tsx` (hub URL `http://localhost:5299/chathub`)

## Prerequisites

- **.NET SDK 8**
- **Node.js** (recommended: latest LTS)
- **pnpm** (the frontend uses `pnpm-lock.yaml`)

## Run locally

### 1) Start the backend

From the repo root:

```bash
cd WebApplication1/Messaging

dotnet restore
# Use the launch profile that binds to http://localhost:5299

dotnet run --launch-profile http
```

- API base URL: `http://localhost:5299`
- Swagger (Development): `http://localhost:5299/swagger`
- SignalR hub: `http://localhost:5299/chathub`

The backend uses SQLite with this connection string:

- `Data Source = ./messaging.db`

If you want a clean local database, stop the backend and delete `WebApplication1/Messaging/messaging.db`.

### 2) Start the frontend

In a separate terminal:

```bash
cd chat-ui

pnpm install
pnpm dev
```

Vite dev server runs on `http://localhost:5173` by default.

## Configuration notes

- **CORS:** the backend CORS policy allows `http://localhost:5173` (see `Program.cs`). If your frontend runs on a different origin/port, update the backend CORS origin.
- **Frontend API URL:** `chat-ui/src/api/api.ts` is currently hardcoded to `http://localhost:5299`.
- **SignalR URL:** `chat-ui/src/pages/ChatPage.tsx` connects to `http://localhost:5299/chathub`.

## Security note (dev/demo)

The UI encrypts/decrypts message content client-side for display. The encryption key is currently present in the frontend code (see `ChatPage.tsx`), which means this is **not** secure key management and should be treated as a demo/prototype approach.

## Common issues

- **Frontend can’t call the API:** confirm backend is running on `http://localhost:5299` and CORS allows `http://localhost:5173`.
- **SignalR connect fails:** ensure the hub URL is `http://localhost:5299/chathub` and you’re logged in (the client sends the JWT as an access token).
