import { NextResponse } from "next/server";

const LOCAL_API_BASE_URL = "http://localhost:5123";
const PRODUCTION_API_BASE_URL = "https://gamedaysync.onrender.com";
const schedules = new Set(["daily", "weekly"]);

function getApiBaseUrl() {
  return (
    process.env.API_BASE_URL ??
    process.env.NEXT_PUBLIC_API_BASE_URL ??
    (process.env.NODE_ENV === "production"
      ? PRODUCTION_API_BASE_URL
      : LOCAL_API_BASE_URL)
  );
}

export async function POST(
  _request: Request,
  { params }: { params: Promise<{ schedule: string }> }
) {
  const { schedule } = await params;

  if (!schedules.has(schedule)) {
    return NextResponse.json({ error: "Unknown pipeline schedule." }, { status: 404 });
  }

  const cronToken = process.env.CRON_SECRET_TOKEN;
  if (!cronToken) {
    return NextResponse.json(
      { error: "Pipeline triggers are not configured." },
      { status: 500 }
    );
  }

  try {
    const response = await fetch(`${getApiBaseUrl()}/api/cron/${schedule}`, {
      method: "POST",
      headers: { "X-Cron-Token": cronToken },
      cache: "no-store",
    });

    if (!response.ok) {
      return NextResponse.json(
        { error: "The pipeline could not be started." },
        { status: response.status >= 500 ? 502 : response.status }
      );
    }
  } catch {
    return NextResponse.json(
      { error: "Could not reach the pipeline API." },
      { status: 502 }
    );
  }

  return NextResponse.json({ schedule, status: "completed" });
}
