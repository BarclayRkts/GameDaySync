"use client";

import { useEffect } from "react";
import { usePathname } from "next/navigation";
import { recordVisit } from "@/lib/api";
import { getVisitorId } from "@/lib/visitor";

/**
 * Silently records a page view every time the route changes. Mounted once in
 * the root layout so every page of the site is tracked.
 */
export function VisitTracker() {
  const pathname = usePathname();

  useEffect(() => {
    recordVisit({
      visitorId: getVisitorId(),
      pagePath: pathname,
      referrer: document.referrer || undefined,
    }).catch(() => {
      // Tracking failures should never disrupt the user experience.
    });
  }, [pathname]);

  return null;
}
