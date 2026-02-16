"use client";

import React, { useState, useEffect } from "react";
import { X, Undo2, UserPlus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";

type BlacklistEntry = {
  id: string;
  blacklisted_player_id: string;
  blacklisted_player_name: string;
};

type OtherPlayer = {
  id: string;
  name: string;
};

type BlacklistManagerProps = {
  partnerBlacklist: BlacklistEntry[];
  opponentBlacklist: BlacklistEntry[];
  otherPlayers: OtherPlayer[];
  onPendingChange?: (changes: {
    adds: Array<{ id: string; type: number }>;
    removals: string[];
  }) => void;
};

function BlacklistSection({
  description,
  blacklistType,
  entries,
  otherPlayers,
  pendingAdds,
  pendingRemovals,
  onAddPlayer,
  onRemoveEntry,
  onUndoRemoval,
  onRemovePendingAdd,
}: {
  description: string;
  blacklistType: number;
  entries: BlacklistEntry[];
  otherPlayers: OtherPlayer[];
  pendingAdds: Array<{ id: string; type: number }>;
  pendingRemovals: string[];
  onAddPlayer: (playerId: string) => void;
  onRemoveEntry: (entryId: string) => void;
  onUndoRemoval: (entryId: string) => void;
  onRemovePendingAdd: (playerId: string) => void;
}) {
  const [query, setQuery] = useState("");

  const blacklistedIds = new Set([
    ...entries.map((e) => e.blacklisted_player_id),
    ...pendingAdds.filter((a) => a.type === blacklistType).map((a) => a.id),
  ]);
  const available = otherPlayers.filter((p) => !blacklistedIds.has(p.id));
  const matched =
    query.length > 0
      ? available.filter((p) =>
          (p.name ?? "").toLowerCase().includes(query.toLowerCase())
        )
      : [];

  const myPendingAdds = pendingAdds.filter((a) => a.type === blacklistType);

  return (
    <div className="space-y-3">
      <p className="text-xs text-muted-foreground">{description}</p>

      {/* Search to add players */}
      <div className="relative">
        <Input
          placeholder="Search players to add..."
          value={query}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setQuery(e.target.value)
          }
          className="pr-8"
        />
        {query.length > 0 && (
          <button
            type="button"
            className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
            onClick={() => setQuery("")}
          >
            <X className="size-4" />
          </button>
        )}
      </div>

      {query.length > 0 && (
        <div className="rounded-md border bg-popover shadow-sm overflow-hidden">
          {matched.length > 0 ? (
            <ul className="max-h-40 overflow-auto divide-y">
              {matched.map((p) => (
                <li
                  key={p.id}
                  role="button"
                  className="flex items-center justify-between px-3 py-2 hover:bg-accent transition-colors cursor-pointer"
                  onClick={() => {
                    onAddPlayer(p.id);
                    setQuery("");
                  }}
                >
                  <span className="text-sm truncate">{p.name}</span>
                  <UserPlus className="size-3.5 text-muted-foreground shrink-0" />
                </li>
              ))}
            </ul>
          ) : (
            <p className="px-3 py-2 text-sm text-muted-foreground">
              No players found
            </p>
          )}
        </div>
      )}

      {available.length === 0 && query.length === 0 && (
        <p className="text-xs text-muted-foreground italic">
          All players are already in this list
        </p>
      )}

      {/* Unified entry list */}
      <div className="space-y-1.5">
        {entries.map((entry) => {
          const isRemoved = pendingRemovals.includes(entry.id);
          return (
            <div
              key={entry.id}
              className={`flex items-center justify-between rounded-md border px-3 py-2 text-sm transition-colors ${
                isRemoved ? "opacity-50 bg-muted" : ""
              }`}
            >
              <span className={isRemoved ? "line-through" : ""}>
                {entry.blacklisted_player_name}
              </span>
              {isRemoved ? (
                <Button
                  type="button"
                  variant="ghost"
                  size="sm"
                  className="h-7 gap-1 text-xs"
                  onClick={() => onUndoRemoval(entry.id)}
                >
                  <Undo2 className="size-3.5" />
                  Undo
                </Button>
              ) : (
                <Button
                  type="button"
                  variant="ghost"
                  size="icon"
                  className="size-7 text-muted-foreground hover:text-destructive"
                  onClick={() => onRemoveEntry(entry.id)}
                >
                  <X className="size-4" />
                </Button>
              )}
            </div>
          );
        })}

        {/* Pending additions in the same list style */}
        {myPendingAdds.map((a) => (
          <div
            key={`pending-${a.id}`}
            className="flex items-center justify-between rounded-md border border-dashed px-3 py-2 text-sm"
          >
            <span className="flex items-center gap-2">
              {otherPlayers.find((p) => p.id === a.id)?.name ?? a.id}
              <Badge variant="secondary" className="text-[10px] px-1.5 py-0">
                New
              </Badge>
            </span>
            <Button
              type="button"
              variant="ghost"
              size="icon"
              className="size-7 text-muted-foreground hover:text-destructive"
              onClick={() => onRemovePendingAdd(a.id)}
            >
              <X className="size-4" />
            </Button>
          </div>
        ))}

        {entries.length === 0 && myPendingAdds.length === 0 && (
          <p className="text-sm text-muted-foreground py-2">
            No players blacklisted yet
          </p>
        )}
      </div>
    </div>
  );
}

export function BlacklistManager({
  partnerBlacklist,
  opponentBlacklist,
  otherPlayers,
  onPendingChange,
}: BlacklistManagerProps) {
  const [pendingAdds, setPendingAdds] = useState<
    Array<{ id: string; type: number }>
  >([]);
  const [pendingRemovals, setPendingRemovals] = useState<string[]>([]);

  // Propagate pending changes to parent
  useEffect(() => {
    onPendingChange?.({ adds: pendingAdds, removals: pendingRemovals });
  }, [pendingAdds, pendingRemovals]);

  function handleAdd(blacklistedPlayerId: string, blacklistType: number) {
    const allEntries =
      blacklistType === 0 ? partnerBlacklist : opponentBlacklist;
    const alreadyExists = allEntries.some(
      (e) => e.blacklisted_player_id === blacklistedPlayerId
    );
    const alreadyQueued = pendingAdds.some(
      (a) => a.id === blacklistedPlayerId && a.type === blacklistType
    );
    if (alreadyExists || alreadyQueued) return;
    setPendingAdds((prev) => [
      ...prev,
      { id: blacklistedPlayerId, type: blacklistType },
    ]);
  }

  function handleRemove(entryId: string) {
    if (!pendingRemovals.includes(entryId)) {
      setPendingRemovals((prev) => [...prev, entryId]);
    }
  }

  function handleUndoRemoval(entryId: string) {
    setPendingRemovals((prev) => prev.filter((id) => id !== entryId));
  }

  function handleRemovePendingAdd(playerId: string, blacklistType: number) {
    setPendingAdds((prev) =>
      prev.filter((a) => !(a.id === playerId && a.type === blacklistType))
    );
  }

  return (
    <div className="space-y-4">
      <Separator />
      <div>
        <h2 className="text-lg font-semibold">Blacklists</h2>
        <p className="text-xs text-muted-foreground mt-1">
          Control who this player can be paired with or matched against
        </p>
      </div>

      <Tabs defaultValue="partner">
        <TabsList className="w-full">
          <TabsTrigger value="partner" className="flex-1">
            Partners
            {(partnerBlacklist.length > 0 ||
              pendingAdds.some((a) => a.type === 0)) && (
              <Badge variant="secondary" className="ml-1.5 text-[10px] px-1.5 py-0">
                {partnerBlacklist.length +
                  pendingAdds.filter((a) => a.type === 0).length}
              </Badge>
            )}
          </TabsTrigger>
          <TabsTrigger value="opponent" className="flex-1">
            Opponents
            {(opponentBlacklist.length > 0 ||
              pendingAdds.some((a) => a.type === 1)) && (
              <Badge variant="secondary" className="ml-1.5 text-[10px] px-1.5 py-0">
                {opponentBlacklist.length +
                  pendingAdds.filter((a) => a.type === 1).length}
              </Badge>
            )}
          </TabsTrigger>
        </TabsList>

        <TabsContent value="partner">
          <BlacklistSection
            description="Players who should not be paired as partners with this player"
            blacklistType={0}
            entries={partnerBlacklist}
            otherPlayers={otherPlayers}
            pendingAdds={pendingAdds}
            pendingRemovals={pendingRemovals}
            onAddPlayer={(id) => handleAdd(id, 0)}
            onRemoveEntry={handleRemove}
            onUndoRemoval={handleUndoRemoval}
            onRemovePendingAdd={(id) => handleRemovePendingAdd(id, 0)}
          />
        </TabsContent>

        <TabsContent value="opponent">
          <BlacklistSection
            description="Players who should not be matched as opponents against this player"
            blacklistType={1}
            entries={opponentBlacklist}
            otherPlayers={otherPlayers}
            pendingAdds={pendingAdds}
            pendingRemovals={pendingRemovals}
            onAddPlayer={(id) => handleAdd(id, 1)}
            onRemoveEntry={handleRemove}
            onUndoRemoval={handleUndoRemoval}
            onRemovePendingAdd={(id) => handleRemovePendingAdd(id, 1)}
          />
        </TabsContent>
      </Tabs>
    </div>
  );
}
