"use client";

import { useQuery } from "@tanstack/react-query";
import { getSyncLogs } from "@/lib/api";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AlertTriangle } from "lucide-react";

function fmt(iso: string) {
  return new Date(iso).toLocaleString("en-US", {
    month: "short",
    day: "numeric",
    hour: "numeric",
    minute: "2-digit",
  });
}

const STATUS_STYLES: Record<string, string> = {
  Success: "bg-emerald-500/10 text-emerald-400 border-emerald-500/20",
  Partial: "bg-amber-500/10 text-amber-400 border-amber-500/20",
  Failed: "bg-red-500/10 text-red-400 border-red-500/20",
};

export default function SyncLogsPage() {
  const syncLogsQuery = useQuery({
    queryKey: ["syncLogs", 30],
    queryFn: () => getSyncLogs(30),
  });

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-xl font-semibold tracking-tight">Sync Logs</h1>
        <p className="text-sm text-muted-foreground">
          Execution history of the daily GitHub Actions cron pipeline.
        </p>
      </div>

      {syncLogsQuery.isError && (
        <Alert variant="destructive">
          <AlertTriangle className="size-4" />
          <AlertTitle>Failed to load sync logs</AlertTitle>
          <AlertDescription>
            {syncLogsQuery.error instanceof Error ? syncLogsQuery.error.message : "Unknown error"}
          </AlertDescription>
        </Alert>
      )}

      <div className="rounded-md border border-border overflow-hidden">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Run At</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Games Added</TableHead>
              <TableHead>Duration</TableHead>
              <TableHead>Message</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {syncLogsQuery.isLoading &&
              Array.from({ length: 8 }).map((_, i) => (
                <TableRow key={i}>
                  {Array.from({ length: 5 }).map((__, j) => (
                    <TableCell key={j}>
                      <Skeleton className="h-4 w-full max-w-32" />
                    </TableCell>
                  ))}
                </TableRow>
              ))}

            {!syncLogsQuery.isLoading &&
              syncLogsQuery.data?.map((log) => (
                <TableRow key={log.id}>
                  <TableCell className="text-sm">{fmt(log.runAt)}</TableCell>
                  <TableCell>
                    <Badge variant="outline" className={STATUS_STYLES[log.status]}>
                      {log.status}
                    </Badge>
                  </TableCell>
                  <TableCell className="text-sm">{log.gamesAdded}</TableCell>
                  <TableCell className="text-sm">{(log.durationMs / 1000).toFixed(1)}s</TableCell>
                  <TableCell className="text-sm text-muted-foreground">{log.message}</TableCell>
                </TableRow>
              ))}

            {!syncLogsQuery.isLoading && !syncLogsQuery.isError && syncLogsQuery.data?.length === 0 && (
              <TableRow>
                <TableCell colSpan={5} className="text-center text-sm text-muted-foreground py-8">
                  No sync runs recorded yet.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
