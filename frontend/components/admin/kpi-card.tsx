import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import type { LucideIcon } from "lucide-react";

interface KpiCardProps {
  label: string;
  value: string;
  hint?: string;
  icon: LucideIcon;
  pulse?: boolean;
  accent?: "default" | "emerald";
}

export function KpiCard({ label, value, hint, icon: Icon, pulse, accent = "default" }: KpiCardProps) {
  return (
    <Card className="gap-2 py-4">
      <CardHeader className="px-4 flex-row items-center justify-between space-y-0">
        <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
          {label}
        </p>
        <Icon
          className={cn(
            "size-4",
            accent === "emerald" ? "text-emerald-400" : "text-muted-foreground"
          )}
        />
      </CardHeader>
      <CardContent className="px-4">
        <div className="flex items-center gap-2">
          {pulse && (
            <span className="relative flex size-2.5">
              <span className="absolute inline-flex h-full w-full animate-ping rounded-full bg-emerald-400 opacity-75" />
              <span className="relative inline-flex size-2.5 rounded-full bg-emerald-500" />
            </span>
          )}
          <p className="text-2xl font-semibold tracking-tight">{value}</p>
        </div>
        {hint && <p className="text-xs text-muted-foreground mt-1">{hint}</p>}
      </CardContent>
    </Card>
  );
}
