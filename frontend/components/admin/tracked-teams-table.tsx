"use client";

import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { addTrackedTeam, deleteTrackedTeam, getTrackedTeams } from "@/lib/api";
import type { TrackedTeamDto } from "@/lib/types";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Plus, Trash2, AlertTriangle, Loader2 } from "lucide-react";

function fmt(iso: string) {
  return new Date(iso).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" });
}

export function TrackedTeamsTable() {
  const queryClient = useQueryClient();

  const trackedTeamsQuery = useQuery({
    queryKey: ["trackedTeamsList"],
    queryFn: getTrackedTeams,
  });

  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [formError, setFormError] = useState<string | null>(null);

  const createMutation = useMutation({
    mutationFn: addTrackedTeam,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["trackedTeamsList"] });
      setName("");
      setFormError(null);
      setOpen(false);
    },
    onError: (err: unknown) => {
      setFormError(err instanceof Error ? err.message : "Failed to add tracked team.");
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteTrackedTeam,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["trackedTeamsList"] });
    },
  });

  function handleAdd() {
    if (!name.trim()) return;
    createMutation.mutate({ name: name.trim() });
  }

  return (
    <div className="space-y-4">
      <div className="flex justify-end">
        <Dialog
          open={open}
          onOpenChange={(next) => {
            setOpen(next);
            if (!next) setFormError(null);
          }}
        >
          <DialogTrigger
            render={
              <Button size="sm">
                <Plus className="size-4" />
                Add Tracked Team
              </Button>
            }
          />
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Add Tracked Team</DialogTitle>
              <DialogDescription>
                Enter a team name. GameDay-Sync finds its SportsDB ID and imports its upcoming schedule.
              </DialogDescription>
            </DialogHeader>
            <div className="space-y-4 py-2">
              <div className="space-y-1.5">
                <Label htmlFor="target-name">Team name</Label>
                <Input
                  id="target-name"
                  placeholder="e.g. Dallas Cowboys"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                />
              </div>
              {formError && <p className="text-sm text-destructive">{formError}</p>}
            </div>
            <DialogFooter>
              <Button variant="outline" onClick={() => setOpen(false)}>
                Cancel
              </Button>
              <Button
                onClick={handleAdd}
                disabled={!name.trim() || createMutation.isPending}
              >
                {createMutation.isPending && <Loader2 className="size-4 animate-spin" />}
                Add Target
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      {trackedTeamsQuery.isError && (
        <Alert variant="destructive">
          <AlertTriangle className="size-4" />
          <AlertTitle>Failed to load tracked teams</AlertTitle>
          <AlertDescription>
            {trackedTeamsQuery.error instanceof Error ? trackedTeamsQuery.error.message : "Unknown error"}
          </AlertDescription>
        </Alert>
      )}

      <div className="rounded-md border border-border overflow-hidden">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>ID</TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Date Added</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="w-10" />
            </TableRow>
          </TableHeader>
          <TableBody>
            {trackedTeamsQuery.isLoading &&
              Array.from({ length: 5 }).map((_, i) => (
                <TableRow key={i}>
                  {Array.from({ length: 6 }).map((__, j) => (
                    <TableCell key={j}>
                      <Skeleton className="h-4 w-full max-w-20" />
                    </TableCell>
                  ))}
                </TableRow>
              ))}

            {!trackedTeamsQuery.isLoading &&
              trackedTeamsQuery.data?.map((t: TrackedTeamDto) => (
                <TableRow key={t.id}>
                  <TableCell className="font-mono text-xs text-muted-foreground">{t.sportsDbId}</TableCell>
                  <TableCell className="font-medium">{t.name}</TableCell>
                  <TableCell>
                    <Badge variant="secondary">{t.category}</Badge>
                  </TableCell>
                  <TableCell className="text-sm">{fmt(t.dateAdded)}</TableCell>
                  <TableCell>
                    <Badge
                      variant="outline"
                      className={
                        t.status === "Active"
                          ? "bg-emerald-500/10 text-emerald-400 border-emerald-500/20"
                          : "bg-muted text-muted-foreground"
                      }
                    >
                      {t.status}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="ghost"
                      size="icon"
                      className="size-8 text-muted-foreground hover:text-destructive"
                      disabled={deleteMutation.isPending && deleteMutation.variables === t.id}
                      onClick={() => deleteMutation.mutate(t.id)}
                    >
                      {deleteMutation.isPending && deleteMutation.variables === t.id ? (
                        <Loader2 className="size-4 animate-spin" />
                      ) : (
                        <Trash2 className="size-4" />
                      )}
                    </Button>
                  </TableCell>
                </TableRow>
              ))}

            {!trackedTeamsQuery.isLoading && !trackedTeamsQuery.isError && trackedTeamsQuery.data?.length === 0 && (
              <TableRow>
                <TableCell colSpan={6} className="text-center text-sm text-muted-foreground py-8">
                  No teams are being tracked yet.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
