import { GamesTable } from "@/components/admin/games-table";

export default function GamesDatabasePage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-xl font-semibold tracking-tight">Games Database</h1>
        <p className="text-sm text-muted-foreground">
          Inspect every row currently persisted in the <code className="text-xs bg-muted px-1 py-0.5 rounded">games</code> table.
        </p>
      </div>
      <GamesTable />
    </div>
  );
}
