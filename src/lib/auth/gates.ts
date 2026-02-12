import { redirect } from "next/navigation";
import { createClient } from "@/lib/supabase/server";

export async function checkAssociationGate(): Promise<string[]> {
  const supabase = await createClient();
  const {
    data: { user },
  } = await supabase.auth.getUser();

  if (!user) {
    redirect("/login");
  }

  const { data: memberships } = await supabase
    .from("club_organisers")
    .select("club_id")
    .eq("user_id", user.id);

  const clubIds = memberships?.map((m) => m.club_id) ?? [];

  if (clubIds.length === 0) {
    redirect("/onboarding");
  }

  return clubIds;
}

export async function checkSubscriptionGate(clubIds: string[]): Promise<void> {
  const supabase = await createClient();

  const { data: subscriptions } = await supabase
    .from("subscriptions")
    .select("id")
    .in("club_id", clubIds)
    .in("status", ["active", "trialling"]);

  if (!subscriptions || subscriptions.length === 0) {
    redirect("/pricing");
  }
}
