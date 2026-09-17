import { SidebarNav } from "@/components/admin/sidebar-nav";
import { ThemeToggle } from "@/components/admin/theme-toggle";

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="flex min-h-screen w-full">
      <SidebarNav />
      <div className="flex-1 flex flex-col min-w-0">
        <header className="h-14 border-b border-border flex items-center px-6 justify-between">
          <p className="text-sm text-muted-foreground">
            Administrative Console
          </p>
          <div className="flex items-center gap-4">
            <div className="flex items-center gap-2 text-xs text-muted-foreground">
              <span className="relative flex size-2">
                <span className="absolute inline-flex h-full w-full animate-ping rounded-full bg-emerald-400 opacity-75" />
                <span className="relative inline-flex size-2 rounded-full bg-emerald-500" />
              </span>
              Live
            </div>
            <ThemeToggle />
          </div>
        </header>
        <main className="flex-1 p-6 space-y-6">{children}</main>
      </div>
    </div>
  );
}
