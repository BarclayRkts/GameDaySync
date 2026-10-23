import { VisitsOverview } from "@/components/visits-overview";

export default function AnalyticsPage() {
  return (
    <div className="mx-auto max-w-5xl p-6 space-y-6">
      <div>
        <h1 className="text-xl font-semibold tracking-tight">Site Analytics</h1>
        <p className="text-sm text-muted-foreground">
          Who is visiting the site and how often.
        </p>
      </div>
      <VisitsOverview />
    </div>
  );
}
