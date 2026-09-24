import type {
  AddTrackedTeamRequest,
  DashboardSummaryDto,
  GameDto,
  PagedResult,
  SyncLogDto,
  TrackedTeamDto,
} from "./types";

const LOCAL_API_BASE_URL = "http://localhost:5123";
const PRODUCTION_API_BASE_URL = "https://gamedaysync.onrender.com";

export const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_BASE_URL ??
  (process.env.NODE_ENV === "production"
    ? PRODUCTION_API_BASE_URL
    : LOCAL_API_BASE_URL);

export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number
  ) {
    super(message);
    this.name = "ApiError";
  }
}

async function apiFetch<T>(path: string, init?: RequestInit): Promise<T> {
  let res: Response;
  try {
    res = await fetch(`${API_BASE_URL}${path}`, {
      ...init,
      headers: {
        "Content-Type": "application/json",
        ...init?.headers,
      },
      cache: "no-store",
    });
  } catch {
    throw new ApiError(
      `Could not reach the GameDay-Sync API at ${API_BASE_URL}${path}. Is the backend running?`,
      0
    );
  }

  if (!res.ok) {
    const body = await res.text().catch(() => "");
    throw new ApiError(body || `Request failed with status ${res.status}`, res.status);
  }

  if (res.status === 204) {
    return undefined as T;
  }

  return (await res.json()) as T;
}

export interface GamesListParams {
  page?: number;
  pageSize?: number;
  search?: string;
  league?: string;
}

export function getGames(params: GamesListParams = {}): Promise<PagedResult<GameDto>> {
  const query = new URLSearchParams();
  query.set("page", String(params.page ?? 1));
  query.set("pageSize", String(params.pageSize ?? 10));
  if (params.search) query.set("search", params.search);
  if (params.league && params.league !== "All") query.set("league", params.league);

  return apiFetch<PagedResult<GameDto>>(`/api/games?${query.toString()}`);
}

export function getDashboardSummary(): Promise<DashboardSummaryDto> {
  return apiFetch<DashboardSummaryDto>("/api/dashboard/summary");
}

export function getTrackedTeams(): Promise<TrackedTeamDto[]> {
  return apiFetch<TrackedTeamDto[]>("/api/tracked-teams");
}

export function addTrackedTeam(payload: AddTrackedTeamRequest): Promise<TrackedTeamDto> {
  return apiFetch<TrackedTeamDto>("/api/tracked-teams", {
    method: "POST",
    body: JSON.stringify(payload),
  });
}

export function deleteTrackedTeam(id: number): Promise<void> {
  return apiFetch<void>(`/api/tracked-teams/${id}`, { method: "DELETE" });
}

export function getSyncLogs(take = 30): Promise<SyncLogDto[]> {
  return apiFetch<SyncLogDto[]>(`/api/sync-logs?take=${take}`);
}

export type PipelineSchedule = "daily" | "weekly";

export async function triggerPipeline(schedule: PipelineSchedule): Promise<void> {
  let res: Response;
  try {
    res = await fetch(`/api/pipeline/${schedule}`, {
      method: "POST",
      cache: "no-store",
    });
  } catch {
    throw new ApiError("Could not reach the dashboard server.", 0);
  }

  if (!res.ok) {
    const body = (await res.json().catch(() => null)) as { error?: string } | null;
    throw new ApiError(body?.error || "The pipeline could not be started.", res.status);
  }
}
