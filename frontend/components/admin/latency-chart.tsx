"use client";

import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import type { LatencyPointDto } from "@/lib/types";

interface LatencyChartProps {
  data: LatencyPointDto[];
}

export function LatencyChart({ data }: LatencyChartProps) {
  if (data.length === 0) {
    return (
      <Card className="col-span-full lg:col-span-2">
        <CardHeader>
          <CardTitle className="text-sm font-medium">API Data Pull Latency</CardTitle>
          <CardDescription>Last sync runs &middot; milliseconds per pull, games added overlay</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="h-48 flex items-center justify-center text-sm text-muted-foreground">
            No sync runs recorded yet.
          </div>
        </CardContent>
      </Card>
    );
  }

  const maxLatency = Math.max(...data.map((d) => d.latencyMs), 1);

  return (
    <Card className="col-span-full lg:col-span-2">
      <CardHeader>
        <CardTitle className="text-sm font-medium">API Data Pull Latency</CardTitle>
        <CardDescription>Last {data.length} sync runs &middot; milliseconds per pull, games added overlay</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex items-end gap-3 h-48 px-1">
          {data.map((d, i) => {
            const heightPct = Math.max((d.latencyMs / maxLatency) * 100, 4);
            const label = new Date(d.day).toLocaleDateString("en-US", { weekday: "short" });
            return (
              <div key={`${d.day}-${i}`} className="flex-1 flex flex-col items-center gap-2 group">
                <span className="text-[10px] text-muted-foreground opacity-0 group-hover:opacity-100 transition-opacity">
                  {d.latencyMs}ms
                </span>
                <div className="w-full flex items-end justify-center h-36">
                  <div
                    className="w-full max-w-8 rounded-t-sm bg-gradient-to-t from-primary/30 to-primary/80 transition-all group-hover:to-emerald-400"
                    style={{ height: `${heightPct}%` }}
                  />
                </div>
                <span className="text-[11px] text-muted-foreground">{label}</span>
              </div>
            );
          })}
        </div>
      </CardContent>
    </Card>
  );
}
