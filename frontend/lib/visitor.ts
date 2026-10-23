const VISITOR_ID_KEY = "gds_visitor_id";

/**
 * Returns a stable anonymous visitor id persisted in localStorage, creating
 * one on first use. This lets us count unique visitors without auth/cookies.
 */
export function getVisitorId(): string {
  if (typeof window === "undefined") return "";

  let visitorId = window.localStorage.getItem(VISITOR_ID_KEY);
  if (!visitorId) {
    visitorId = crypto.randomUUID();
    window.localStorage.setItem(VISITOR_ID_KEY, visitorId);
  }
  return visitorId;
}
