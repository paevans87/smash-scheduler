import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function OnboardingPage() {
  return (
    <div className="flex min-h-screen items-center justify-center px-4">
      <Card className="w-full max-w-md text-center">
        <CardHeader>
          <CardTitle>Join or Create a Club</CardTitle>
          <CardDescription>
            You need to be associated with a club before you can access the
            scheduler. Create your own club or ask an organiser to add you.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Button disabled className="w-full">
            Get Started
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}
