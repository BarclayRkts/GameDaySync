"use client";

import { useEffect, useState } from "react";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getGames } from "@/lib/api";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Search, ChevronLeft, ChevronRight, AlertTriangle } from "lucide-react";

const PAGE_SIZE = 10;
const LEAGUE_OPTIONS = ["All", "NBA", "MLB", "NFL", "English Premier League", "MLS", "NWSL"];

const LEAGUE_COLORS: Record<string, string> = {
  NBA: "bg-orange-500/10 text-orange-400 border-orange-500/20",
  MLB: "bg-blue-500/10 text-blue-400 border-blue-500/20",
  NFL: "bg-red-500/10 text-red-400 border-red-500/20",
  "English Premier League": "bg-purple-500/10 text-purple-400 border-purple-500/20",
  MLS: "bg-emerald-500/10 text-emerald-400 border-emerald-500/20",
  NWSL: "bg-pink-500/10 text-pink-400 border-pink-500/20",
};

function fmt(iso: string) {
  return new Date(iso).toLocaleString("en-US", {
    month: "short",
    day: "numeric",
    hour: "numeric",
    minute: "2-digit",
  });
}

function useDebouncedValue<T>(value: T, delayMs: number): T {
  const [debounced, setDebounced] = useState(value);
  useEffect(() => {
    const handle = setTimeout(() => setDebounced(value), delayMs);
    return () => clearTimeout(handle);
  }, [value, delayMs]);
  return debounced;
}

export function GamesTable() {
  const [searchInput, setSearchInput] = useState("");
  const [league, setLeague] = useState("All");
  const [page, setPage] = useState(1);

  const search = useDebouncedValue(searchInput, 350);

  const gamesQuery = useQuery({
    queryKey: ["gamesList", { page, pageSize: PAGE_SIZE, search, league }],
    queryFn: () => getGames({ page, pageSize: PAGE_SIZE, search, league }),
    placeholderData: keepPreviousData,
  });

  const totalPages = gamesQuery.data?.totalPages ?? 1;
  const items = gamesQuery.data?.items ?? [];

  return (
    <div className="space-y-4">
      <div className="flex flex-col sm:flex-row gap-3">
        <div className="relative flex-1">
          <Search className="absolute left-2.5 top-2.5 size-4 text-muted-foreground" />
          <Input
            placeholder="Search by event, team, opponent, or ID..."
            className="pl-8"
            value={searchInput}
            onChange={(e) => {
              setSearchInput(e.target.value);
              setPage(1);
            }}
          />
        </div>
        <Select
          value={league}
          onValueChange={(v) => {
            setLeague(v ?? "All");
            setPage(1);
          }}
        >
          <SelectTrigger className="w-full sm:w-56">
            <SelectValue placeholder="League" />
          </SelectTrigger>
          <SelectContent>
            {LEAGUE_OPTIONS.map((l) => (
              <SelectItem key={l} value={l}>
                {l}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {gamesQuery.isError && (
        <Alert variant="destructive">
          <AlertTriangle className="size-4" />
          <AlertTitle>Failed to load games</AlertTitle>
          <AlertDescription>
            {gamesQuery.error instanceof Error ? gamesQuery.error.message : "Unknown error"}
          </AlertDescription>
        </Alert>
      )}

      <div className="rounded-md border border-border overflow-hidden">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>ID</TableHead>
              <TableHead>Event</TableHead>
              <TableHead>League</TableHead>
              <TableHead>Game Time</TableHead>
              <TableHead>Team</TableHead>
              <TableHead>Opponent</TableHead>
              <TableHead>Created</TableHead>
              <TableHead>Updated</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {gamesQuery.isLoading &&
              Array.from({ length: PAGE_SIZE }).map((_, i) => (
                <TableRow key={i}>
                  {Array.from({ length: 8 }).map((__, j) => (
                    <TableCell key={j}>
                      <Skeleton className="h-4 w-full max-w-24" />
                    </TableCell>
                  ))}
                </TableRow>
              ))}

            {!gamesQuery.isLoading &&
              items.map((g) => (
                <TableRow key={g.id}>
                  <TableCell className="font-mono text-xs text-muted-foreground">{g.id}</TableCell>
                  <TableCell className="font-medium">{g.eventName}</TableCell>
                  <TableCell>
                    <Badge variant="outline" className={LEAGUE_COLORS[g.league] ?? ""}>
                      {g.league}
                    </Badge>
                  </TableCell>
                  <TableCell className="text-sm">{fmt(g.gameTime)}</TableCell>
                  <TableCell className="text-sm">{g.teamName}</TableCell>
                  <TableCell className="text-sm">{g.opponentName}</TableCell>
                  <TableCell className="text-xs text-muted-foreground">{fmt(g.createdAt)}</TableCell>
                  <TableCell className="text-xs text-muted-foreground">{fmt(g.updatedAt)}</TableCell>
                </TableRow>
              ))}

            {!gamesQuery.isLoading && !gamesQuery.isError && items.length === 0 && (
              <TableRow>
                <TableCell colSpan={8} className="text-center text-sm text-muted-foreground py-8">
                  No games match your filters.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      <div className="flex items-center justify-between text-sm text-muted-foreground">
        <span>
          {gamesQuery.data
            ? `Showing ${items.length === 0 ? 0 : (page - 1) * PAGE_SIZE + 1}–${
                (page - 1) * PAGE_SIZE + items.length
              } of ${gamesQuery.data.totalCount} games`
            : "Loading games…"}
        </span>
        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="icon"
            className="size-8"
            disabled={page <= 1}
            onClick={() => setPage((p) => p - 1)}
          >
            <ChevronLeft className="size-4" />
          </Button>
          <span className="text-xs">
            Page {page} / {totalPages}
          </span>
          <Button
            variant="outline"
            size="icon"
            className="size-8"
            disabled={page >= totalPages}
            onClick={() => setPage((p) => p + 1)}
          >
            <ChevronRight className="size-4" />
          </Button>
        </div>
      </div>
    </div>
  );
}
