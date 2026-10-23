"use client";

import { useQuery } from "@tanstack/react-query";
import { getVisitStats } from "@/lib/api";
import { KpiCard } from "@/components/admin/kpi-card";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Eye, Users, CalendarDays, AlertTriangle } from "lucide-react";

function fmt(iso: string) {
  return new Date(iso).toLocaleString("en-US", {
    month: "short",
    day: "numeric",
    hour: "numeric",
    minute: "2-digit",
  });
}

export function VisitsOverview() {
  const statsQuery = useQuery({
    queryKey: ["visitStats"],
    queryFn: getVisitStats,
    refetchInterval: 30_000,
  });

  if (statsQuery.isError) {
    return (
      <Alert variant="destructive">
        <AlertTriangle className="size-4" />
        <AlertTitle>Failed to load visit stats</AlertTitle>
        <AlertDescription>
          {statsQuery.error instanceof Error ? statsQuery.error.message : "Unknown error"}
        </AlertDescription>
      </Alert>
    );
  }

  return (
    <div className="space-y-6">
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        {statsQuery.isLoading ? (
          Array.from({ length: 3 }).map((_, i) => (
            <Card key={i} className="gap-2 py-4">
              <CardHeader className="px-4">
                <Skeleton className="h-3 w-24" />
              </CardHeader>
              <CardContent className="px-4">
                <Skeleton className="h-7 w-20" />
              </CardContent>
            </Card>
          ))
        ) : (
          statsQuery.isSuccess && (
            <>
              <KpiCard
                label="Total Page Views"
                value={statsQuery.data.totalVisits.toLocaleString()}
                icon={Eye}
              />
              <KpiCard
                label="Unique Visitors"
                value={statsQuery.data.uniqueVisitors.toLocaleString()}
                icon={Users}
              />
              <KpiCard
                label="Views Today"
                value={statsQuery.data.visitsToday.toLocaleString()}
                icon={CalendarDays}
              />
            </>
          )
        )}
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="text-sm font-medium">Recent Visits</CardTitle>
          <CardDescription>Most recent page views across the site</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="rounded-md border border-border overflow-hidden">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Time</TableHead>
                  <TableHead>Page</TableHead>
                  <TableHead>Visitor</TableHead>
                  <TableHead>IP Address</TableHead>
                  <TableHead>Referrer</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {statsQuery.isLoading &&
                  Array.from({ length: 6 }).map((_, i) => (
                    <TableRow key={i}>
                      {Array.from({ length: 5 }).map((__, j) => (
                        <TableCell key={j}>
                          <Skeleton className="h-4 w-full max-w-28" />
                        </TableCell>
                      ))}
                    </TableRow>
                  ))}

                {!statsQuery.isLoading &&
                  statsQuery.data?.recentVisits.map((v) => (
                    <TableRow key={v.id}>
                      <TableCell className="text-sm">{fmt(v.visitedAt)}</TableCell>
                      <TableCell className="font-mono text-xs">{v.pagePath}</TableCell>
                      <TableCell className="font-mono text-xs text-muted-foreground">
                        {v.visitorId.slice(0, 8)}
                      </TableCell>
                      <TableCell className="text-sm text-muted-foreground">
                        {v.ipAddress ?? "—"}
                      </TableCell>
                      <TableCell className="text-sm text-muted-foreground truncate max-w-48">
                        {v.referrer ?? "Direct"}
                      </TableCell>
                    </TableRow>
                  ))}

                {!statsQuery.isLoading && statsQuery.data?.recentVisits.length === 0 && (
                  <TableRow>
                    <TableCell colSpan={5} className="text-center text-sm text-muted-foreground py-8">
                      No visits recorded yet.
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
