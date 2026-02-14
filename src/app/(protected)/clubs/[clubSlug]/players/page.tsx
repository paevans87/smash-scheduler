import { redirect } from "next/navigation";
import { createClient } from "@/lib/supabase/server";
import { PlayerListClient } from "@/components/player-list-client";

type PlayersPageProps = {
  params: Promise<{ clubSlug: string }>;
};

export default async function PlayersPage({ params }: PlayersPageProps) {
  const { clubSlug } = await params;
  const supabase = await createClient();

  const { data: club } = await supabase
    .from("clubs")
    .select("id")
    .eq("slug", clubSlug)
    .single();

  if (!club) {
    redirect("/clubs");
  }

  return <PlayerListClient clubId={club.id} clubSlug={clubSlug} />;
}
