import { redirect } from "next/navigation";
import Link from "next/link";
import { createClient } from "@/lib/supabase/server";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

type ClubRow = {
  club_id: string;
  clubs: {
    id: string;
    name: string;
    subscriptions: { status: string; plan_type: string }[];
  };
};

function subscriptionLabel(status: string, planType: string): string {
  if (status === "trialling") return "Trial";
  if (planType === "free") return "Free";
  return "Pro";
}

export default async function ClubsPage() {
  const supabase = await createClient();
  const {
    data: { user },
  } = await supabase.auth.getUser();

  if (!user) {
    redirect("/login");
  }

  const { data } = await supabase
    .from("club_organisers")
    .select("club_id, clubs:club_id(id, name, subscriptions(status, plan_type))")
    .eq("user_id", user.id);

  const clubs = (data as unknown as ClubRow[]) ?? [];

  const activeClubs = clubs.filter((row) =>
    row.clubs.subscriptions.some(
      (s) => s.status === "active" || s.status === "trialling"
    )
  );

  if (activeClubs.length === 0) {
    redirect("/onboarding");
  }

  if (activeClubs.length === 1) {
    redirect(`/clubs/${activeClubs[0].club_id}`);
  }

  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-6 px-4">
      <h1 className="text-3xl font-bold">Select a Club</h1>
      <div className="grid w-full max-w-2xl gap-4 sm:grid-cols-2">
        {activeClubs.map((row) => {
          const activeSub = row.clubs.subscriptions.find(
            (s) => s.status === "active" || s.status === "trialling"
          );

          return (
            <Link key={row.club_id} href={`/clubs/${row.club_id}`}>
              <Card className="transition-colors hover:border-primary">
                <CardHeader>
                  <CardTitle>{row.clubs.name}</CardTitle>
                </CardHeader>
                <CardContent>
                  {activeSub && (
                    <span className="inline-block rounded-full bg-muted px-2.5 py-0.5 text-xs font-medium">
                      {subscriptionLabel(activeSub.status, activeSub.plan_type)}
                    </span>
                  )}
                </CardContent>
              </Card>
            </Link>
          );
        })}
      </div>
    </div>
  );
}
