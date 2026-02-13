import Image from "next/image";
import Link from "next/link";
import { ProfileMenu } from "@/components/profile-menu";

type AppBarProps = {
  clubId: string;
  clubName: string;
  userEmail: string;
};

export function AppBar({ clubId, clubName, userEmail }: AppBarProps) {
  return (
    <header className="sticky top-0 z-50 border-b bg-background">
      <div className="flex h-14 items-center gap-6 px-4">
        <Link
          href={`/clubs/${clubId}`}
          className="flex items-center gap-2 font-semibold"
        >
          <Image
            src="/icon-192.png"
            alt="SmashScheduler"
            width={32}
            height={32}
          />
          <span className="hidden sm:inline">SmashScheduler</span>
        </Link>

        <nav className="flex items-center gap-4 text-sm">
          <Link
            href="#"
            className="text-muted-foreground transition-colors hover:text-foreground"
          >
            Sessions
          </Link>
          <Link
            href="#"
            className="text-muted-foreground transition-colors hover:text-foreground"
          >
            Club Management
          </Link>
        </nav>

        <div className="ml-auto">
          <ProfileMenu userEmail={userEmail} />
        </div>
      </div>
    </header>
  );
}
