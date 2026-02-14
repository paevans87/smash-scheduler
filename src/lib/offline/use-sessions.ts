"use client";

import { useState, useEffect } from "react";
import { createClient } from "@/lib/supabase/client";
import { useOnlineStatus } from "./online-status-provider";
import { getSessionsFromCache, syncClubData } from "./sync-service";

type Session = {
  id: string;
  club_id: string;
  slug: string;
  scheduled_date_time: string;
  court_count: number;
  state: number;
};

type UseSessionsResult = {
  sessions: Session[];
  isLoading: boolean;
  isStale: boolean;
};

export function useSessions(clubId: string): UseSessionsResult {
  const { isOnline } = useOnlineStatus();
  const [sessions, setSessions] = useState<Session[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isStale, setIsStale] = useState(false);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      setIsLoading(true);

      if (isOnline) {
        const supabase = createClient();
        const { data } = await supabase
          .from("sessions")
          .select("id, club_id, slug, scheduled_date_time, court_count, state")
          .eq("club_id", clubId)
          .order("scheduled_date_time", { ascending: true });

        if (!cancelled && data) {
          setSessions(data);
          setIsStale(false);
          await syncClubData(supabase, clubId);
        }
      } else {
        const cached = await getSessionsFromCache(clubId);
        if (!cancelled) {
          setSessions(cached);
          setIsStale(true);
        }
      }

      if (!cancelled) {
        setIsLoading(false);
      }
    }

    load();

    return () => {
      cancelled = true;
    };
  }, [clubId, isOnline]);

  return { sessions, isLoading, isStale };
}
