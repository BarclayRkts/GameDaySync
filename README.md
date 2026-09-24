# GameDay-Sync

GameDay-Sync tracks upcoming sports events for selected teams, persists them to PostgreSQL, and sends scheduled Discord notifications. It includes a .NET API and ingestion pipeline plus a Next.js admin dashboard for monitoring sync health, games, teams, and run history.

[View the live dashboard](https://gamedaysync.vercel.app/)

## Live services

| Service | URL |
|---|---|
| Admin dashboard | [https://gamedaysync.vercel.app/](https://gamedaysync.vercel.app/) |
| API | https://gamedaysync.onrender.com |

> **Note:** The API runs on Render's free tier and may take a short time to respond after a period of inactivity.

## Architecture

- **Backend:** ASP.NET Core on .NET 10, Entity Framework Core, and PostgreSQL via Npgsql.
- **Frontend:** Next.js 16, React 19, TypeScript, Tailwind CSS, and TanStack Query.
- **Sports data:** TheSportsDB API supplies team and upcoming-event data.
- **Notifications:** Discord webhooks deliver daily and weekly game alerts.
- **Automation:** GitHub Actions runs the ingestion and notification pipeline daily and weekly.

```text
GameDay-Sync/
|-- GameDay-Sync/       # .NET API, data pipeline, EF Core migrations
|-- frontend/           # Next.js admin dashboard
`-- .github/workflows/  # Scheduled pipeline workflows
```

## Local development

### Prerequisites

- .NET SDK 10
- Node.js 20 or later
- PostgreSQL

### 1. Configure the backend

Set the PostgreSQL connection string before starting the API. PowerShell example:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=gameday_sync;Username=postgres;Password=your-password"
```

Apply the existing Entity Framework Core migrations:

```powershell
Set-Location .\GameDay-Sync
dotnet ef database update
```

Start the API on `http://localhost:5123`:

```powershell
dotnet run --launch-profile http
```

### 2. Configure and start the frontend

The frontend defaults to the local API in development. To explicitly set or override it, create `frontend\.env.local` from the committed example:

```powershell
Copy-Item .\frontend\.env.example .\frontend\.env.local
```

Install dependencies and start the Next.js development server:

```powershell
Set-Location .\frontend
npm ci
npm run dev
```

Open http://localhost:3000. The API allows this local origin through CORS.

## Configuration

| Setting | Purpose | Local value |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | Required |
| `NotificationSettings__DiscordWebhookUrl` | Discord webhook used by alert runs | Required for `--daily` and `--weekly` |
| `CRON_SECRET_TOKEN` | Token required by the cron webhook endpoints | Required in Render and cron-job.org |
| `NEXT_PUBLIC_API_BASE_URL` | Optional frontend API origin override | `http://localhost:5123` |
| Frontend `CRON_SECRET_TOKEN` | Server-only token used by the dashboard trigger route; must match the backend token | Required for dashboard triggers |

The frontend selects `http://localhost:5123` outside production and `https://gamedaysync.onrender.com` for production builds. Do not set `NEXT_PUBLIC_API_BASE_URL` in Vercel unless intentionally overriding the production API.

## API

All endpoints are prefixed by the local or production API base URL.

| Method | Path | Description |
|---|---|---|
| `GET` | `/api/games` | Lists stored games. Supports `page`, `pageSize`, `search`, and `league` query parameters. |
| `GET` | `/api/dashboard/summary` | Returns dashboard metrics, current system status, and latency history. |
| `GET` | `/api/tracked-teams` | Lists tracked teams. |
| `POST` | `/api/tracked-teams` | Adds a tracked team. Body: `{ "name": "Houston Astros" }`. |
| `DELETE` | `/api/tracked-teams/{id}` | Removes a tracked team by numeric ID. |
| `GET` | `/api/sync-logs` | Lists paginated pipeline executions. Supports `page` and `pageSize`; defaults to page `1` with `20` entries. |
| `POST` | `/api/cron/daily` | Runs the same ingestion and notification routine as `--daily`. Requires `X-Cron-Token`. |
| `POST` | `/api/cron/weekly` | Runs the same ingestion and notification routine as `--weekly`. Requires `X-Cron-Token`. |

Set `CRON_SECRET_TOKEN` as an environment variable in Render and configure cron-job.org to send the same value in the `X-Cron-Token` request header. Requests with a missing, duplicate, or invalid token receive `401 Unauthorized`.

To enable the **Run Daily** and **Run Weekly** dashboard buttons, configure the same `CRON_SECRET_TOKEN` in the frontend deployment. The Next.js server sends it to the protected API endpoint; the token is never exposed to the browser.

## Running the ingestion pipeline

The pipeline fetches future events for active tracked teams, stores new games, records the run, and optionally sends Discord alerts.

```powershell
# Synchronize events only
dotnet run --project .\GameDay-Sync\GameDay-Sync.csproj -- --sync-only

# Synchronize and send daily alerts
dotnet run --project .\GameDay-Sync\GameDay-Sync.csproj -- --daily

# Synchronize and send weekly alerts
dotnet run --project .\GameDay-Sync\GameDay-Sync.csproj -- --weekly
```

## Available commands

| Location | Command | Purpose |
|---|---|---|
| Repository root | `dotnet build .\GameDay-Sync\GameDay-Sync.csproj` | Builds the backend. |
| `frontend` | `npm run dev` | Starts the frontend development server. |
| `frontend` | `npm run build` | Creates a production frontend build. |
| `frontend` | `npm run lint` | Runs frontend linting. |
