import { redirect } from "next/navigation";
import { createClient } from "@/lib/supabase/server";
import { AppBar } from "@/components/app-bar";

type ClubLayoutProps = {
  children: React.ReactNode;
  params: Promise<{ clubId: string }>;
};

export default async function ClubLayout({
  children,
  params,
}: ClubLayoutProps) {
  const { clubId } = await params;
  const supabase = await createClient();

  const {
    data: { user },
  } = await supabase.auth.getUser();

  if (!user) {
    redirect("/login");
  }

  const { data: membership } = await supabase
    .from("club_organisers")
    .select("club_id")
    .eq("club_id", clubId)
    .eq("user_id", user.id)
    .single();

  if (!membership) {
    redirect("/clubs");
  }

  const { data: subscription } = await supabase
    .from("subscriptions")
    .select("id")
    .eq("club_id", clubId)
    .in("status", ["active", "trialling"])
    .limit(1)
    .single();

  if (!subscription) {
    redirect("/clubs");
  }

  const { data: club } = await supabase
    .from("clubs")
    .select("name")
    .eq("id", clubId)
    .single();

  const clubName = club?.name ?? "Club";

  return (
    <div className="flex min-h-screen flex-col">
      <AppBar clubId={clubId} clubName={clubName} userEmail={user.email!} />
      <main className="flex-1">{children}</main>
    </div>
  );
}
