import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function PricingPage() {
  return (
    <div className="flex min-h-screen items-center justify-center px-4">
      <Card className="w-full max-w-md text-center">
        <CardHeader>
          <CardTitle>Choose a Plan</CardTitle>
          <CardDescription>
            Your club needs an active subscription to use the scheduler. Choose a
            plan to get started.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Button disabled className="w-full">
            View Plans
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}
