import { redirect } from "next/navigation";
import Link from "next/link";
import { createClient } from "@/lib/supabase/server";
import { SignOutButton } from "@/components/sign-out-button";

type ClubDashboardProps = {
  params: Promise<{ clubId: string }>;
};

export default async function ClubDashboardPage({ params }: ClubDashboardProps) {
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

  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-6 px-4">
      <h1 className="text-3xl font-bold">{club?.name ?? "Club"}</h1>
      <p className="text-muted-foreground">{user.email}</p>
      <p className="text-muted-foreground">Dashboard coming soon</p>
      <div className="flex gap-4">
        <Link
          href="/clubs"
          className="text-sm text-muted-foreground underline hover:text-foreground"
        >
          Switch clubs
        </Link>
        <SignOutButton />
      </div>
    </div>
  );
}
