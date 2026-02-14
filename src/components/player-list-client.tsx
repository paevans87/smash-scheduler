"use client";

import { useCallback } from "react";
import { usePlayers } from "@/lib/offline/use-players";
import { PlayerCard } from "@/components/player-card";

type PlayerListClientProps = {
  clubId: string;
  clubSlug: string;
};

export function PlayerListClient({ clubId, clubSlug }: PlayerListClientProps) {
  const { players, isLoading, isStale, mutate } = usePlayers(clubId);

  const handleDeleted = useCallback(() => {
    mutate();
  }, [mutate]);

  if (isLoading) {
    return (
      <div className="space-y-6 px-4 py-6 md:px-6">
        <h1 className="text-3xl font-bold">Players</h1>
        <p className="text-muted-foreground">Loading players...</p>
      </div>
    );
  }

  return (
    <div className="space-y-6 px-4 py-6 md:px-6">
      <h1 className="text-3xl font-bold">Players</h1>

      {isStale && (
        <p className="text-sm text-muted-foreground italic">
          Showing cached data — changes will sync when you are back online.
        </p>
      )}

      {players.length === 0 ? (
        <p className="text-muted-foreground">
          No players yet. Add your first player to get started.
        </p>
      ) : (
        <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
          {players.map((player) => (
            <PlayerCard
              key={player.id}
              id={player.id}
              name={player.name}
              skillLevel={player.skill_level}
              gender={player.gender}
              clubSlug={clubSlug}
              onDeleted={handleDeleted}
            />
          ))}
        </div>
      )}
    </div>
  );
}
