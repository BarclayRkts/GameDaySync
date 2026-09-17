"use client";

import { useQuery } from "@tanstack/react-query";
import { KpiCard } from "@/components/admin/kpi-card";
import { LatencyChart } from "@/components/admin/latency-chart";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { getDashboardSummary, getSyncLogs } from "@/lib/api";
import { Database, Activity, Clock, CalendarClock, AlertTriangle } from "lucide-react";

function formatRelative(iso: string | null) {
  if (!iso) return "No runs yet";
  const date = new Date(iso);
  const today = new Date();
  const isToday = date.toDateString() === today.toDateString();
  const tomorrow = new Date(today);
  tomorrow.setDate(today.getDate() + 1);
  const isTomorrow = date.toDateString() === tomorrow.toDateString();

  const time = date.toLocaleTimeString("en-US", { hour: "numeric", minute: "2-digit" });
  if (isToday) return `Today, ${time}`;
  if (isTomorrow) return `Tomorrow, ${time}`;
  return `${date.toLocaleDateString("en-US", { month: "short", day: "numeric" })}, ${time}`;
}

const STATUS_ACCENT: Record<string, "emerald" | "default"> = {
  Active: "emerald",
  Degraded: "default",
  Down: "default",
};

function KpiSkeletons() {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      {Array.from({ length: 4 }).map((_, i) => (
        <Card key={i} className="gap-2 py-4">
          <CardHeader className="px-4">
            <Skeleton className="h-3 w-24" />
          </CardHeader>
          <CardContent className="px-4 space-y-2">
            <Skeleton className="h-7 w-20" />
            <Skeleton className="h-3 w-32" />
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

export default function AdminOverviewPage() {
  const summaryQuery = useQuery({
    queryKey: ["dashboardSummary"],
    queryFn: getDashboardSummary,
  });

  const syncLogsQuery = useQuery({
    queryKey: ["syncLogs", 5],
    queryFn: () => getSyncLogs(5),
  });

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-xl font-semibold tracking-tight">Dashboard Overview</h1>
        <p className="text-sm text-muted-foreground">
          Live snapshot of the GameDay-Sync ingestion pipeline.
        </p>
      </div>

      {summaryQuery.isLoading && <KpiSkeletons />}

      {summaryQuery.isError && (
        <Alert variant="destructive">
          <AlertTriangle className="size-4" />
          <AlertTitle>Failed to load dashboard summary</AlertTitle>
          <AlertDescription>
            {summaryQuery.error instanceof Error ? summaryQuery.error.message : "Unknown error"}
          </AlertDescription>
        </Alert>
      )}

      {summaryQuery.isSuccess && (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
          <KpiCard
            label="Total Synced Games"
            value={summaryQuery.data.totalSyncedGames.toString()}
            hint={`${summaryQuery.data.activeTrackedTeams} tracked teams`}
            icon={Database}
          />
          <KpiCard
            label="System Status"
            value={summaryQuery.data.systemStatus}
            hint={summaryQuery.data.systemStatus === "Active" ? "No incidents detected" : "Check recent sync logs"}
            icon={Activity}
            pulse
            accent={STATUS_ACCENT[summaryQuery.data.systemStatus] ?? "default"}
          />
          <KpiCard
            label="Last Run Time"
            value={formatRelative(summaryQuery.data.lastRunAt)}
            hint="GitHub Actions cron"
            icon={Clock}
          />
          <KpiCard
            label="Next Scheduled Run"
            value={formatRelative(summaryQuery.data.nextRunAt)}
            hint="Daily @ 8:00 AM CT"
            icon={CalendarClock}
          />
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
        {summaryQuery.isLoading ? (
          <Card className="col-span-full lg:col-span-2">
            <CardContent className="pt-6">
              <Skeleton className="h-48 w-full" />
            </CardContent>
          </Card>
        ) : (
          summaryQuery.isSuccess && <LatencyChart data={summaryQuery.data.latencySeries} />
        )}

        <Card>
          <CardHeader>
            <CardTitle className="text-sm font-medium">Recent Sync Runs</CardTitle>
            <CardDescription>Latest pipeline executions</CardDescription>
          </CardHeader>
          <CardContent className="space-y-3">
            {syncLogsQuery.isLoading &&
              Array.from({ length: 5 }).map((_, i) => (
                <div key={i} className="flex items-center justify-between">
                  <Skeleton className="h-8 w-24" />
                  <Skeleton className="h-5 w-16 rounded-full" />
                </div>
              ))}

            {syncLogsQuery.isError && (
              <p className="text-sm text-destructive">Could not load sync logs.</p>
            )}

            {syncLogsQuery.isSuccess && syncLogsQuery.data.length === 0 && (
              <p className="text-sm text-muted-foreground">No sync runs recorded yet.</p>
            )}

            {syncLogsQuery.isSuccess &&
              syncLogsQuery.data.map((log) => (
                <div key={log.id} className="flex items-center justify-between text-sm">
                  <div className="flex flex-col">
                    <span className="font-medium">
                      {new Date(log.runAt).toLocaleDateString("en-US", { month: "short", day: "numeric" })}
                    </span>
                    <span className="text-xs text-muted-foreground">{log.gamesAdded} games added</span>
                  </div>
                  <span
                    className={
                      log.status === "Success"
                        ? "text-xs rounded-full bg-emerald-500/10 text-emerald-400 px-2 py-0.5"
                        : log.status === "Partial"
                          ? "text-xs rounded-full bg-amber-500/10 text-amber-400 px-2 py-0.5"
                          : "text-xs rounded-full bg-red-500/10 text-red-400 px-2 py-0.5"
                    }
                  >
                    {log.status}
                  </span>
                </div>
              ))}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
