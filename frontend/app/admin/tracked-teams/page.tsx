import { TrackedTeamsTable } from "@/components/admin/tracked-teams-table";

export default function TrackedTeamsPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-xl font-semibold tracking-tight">Tracked Teams</h1>
        <p className="text-sm text-muted-foreground">
          Teams whose upcoming schedules are synchronized by the pipeline.
        </p>
      </div>
      <TrackedTeamsTable />
    </div>
  );
}
