"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { cn } from "@/lib/utils";
import {
  LayoutDashboard,
  Database,
  Crosshair,
  ScrollText,
  Radio,
} from "lucide-react";

const NAV_ITEMS = [
  { href: "/admin", label: "Dashboard Overview", icon: LayoutDashboard },
  { href: "/admin/games", label: "Games Database", icon: Database },
  { href: "/admin/tracked-teams", label: "Tracked Teams", icon: Crosshair },
  { href: "/admin/logs", label: "Sync Logs", icon: ScrollText },
];

export function SidebarNav() {
  const pathname = usePathname();

  return (
    <aside className="hidden md:flex w-64 shrink-0 flex-col border-r border-border bg-card/40">
      <div className="flex items-center gap-2 px-5 h-14 border-b border-border">
        <Radio className="size-4 text-emerald-400" />
        <span className="font-semibold tracking-tight text-sm">
          GameDay-Sync
        </span>
        <span className="ml-auto rounded-full border border-border px-1.5 py-0.5 text-[10px] text-muted-foreground">
          admin
        </span>
      </div>
      <nav className="flex-1 px-3 py-4 space-y-1">
        {NAV_ITEMS.map(({ href, label, icon: Icon }) => {
          const active =
            href === "/admin" ? pathname === href : pathname?.startsWith(href);
          return (
            <Link
              key={href}
              href={href}
              className={cn(
                "flex items-center gap-2.5 rounded-md px-3 py-2 text-sm transition-colors",
                active
                  ? "bg-primary text-primary-foreground font-medium"
                  : "text-muted-foreground hover:bg-accent hover:text-foreground"
              )}
            >
              <Icon className="size-4" />
              {label}
            </Link>
          );
        })}
      </nav>
      <div className="px-5 py-4 border-t border-border text-[11px] text-muted-foreground leading-relaxed">
        Neon PostgreSQL &middot; GitHub Actions
        <br />
        Cron: 08:00 America/Chicago
      </div>
    </aside>
  );
}
