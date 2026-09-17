export interface GameDto {
  id: string;
  eventName: string;
  league: string;
  gameTime: string;
  teamName: string;
  opponentName: string;
  createdAt: string;
  updatedAt: string;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface TrackedTeamDto {
  id: number;
  sportsDbId: number;
  name: string;
  category: "Team" | "League";
  dateAdded: string;
  status: "Active" | "Paused";
}

export interface AddTrackedTeamRequest {
  name: string;
}

export interface SyncLogDto {
  id: number;
  runAt: string;
  status: "Success" | "Partial" | "Failed";
  gamesAdded: number;
  durationMs: number;
  message: string;
}

export interface LatencyPointDto {
  day: string;
  latencyMs: number;
  gamesAdded: number;
}

export interface DashboardSummaryDto {
  totalSyncedGames: number;
  systemStatus: "Active" | "Degraded" | "Down";
  lastRunAt: string | null;
  nextRunAt: string;
  activeTrackedTeams: number;
  latencySeries: LatencyPointDto[];
}
