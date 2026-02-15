import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Calendar,
  Users,
  Trophy,
  Zap,
  Clock,
  Shield,
  ChevronRight,
  Check,
} from "lucide-react";

const features = [
  {
    icon: Calendar,
    title: "Effortless Scheduling",
    description: "Create and manage badminton sessions with intuitive drag-and-drop scheduling. Never double-book courts again."
  },
  {
    icon: Users,
    title: "Player Management",
    description: "Track player skill levels, availability, and match history. Build balanced teams automatically."
  },
  {
    icon: Trophy,
    title: "Smart Matchmaking",
    description: "Intelligent matchmaking creates fair and competitive games. Balance skill levels and minimise wait times."
  },
  {
    icon: Zap,
    title: "Offline First",
    description: "Work without internet. All changes sync automatically when you are back online. Perfect for venues with poor connectivity."
  },
  {
    icon: Clock,
    title: "Time Off Tracking",
    description: "Automatically track time off court for each player. Ensures fair rotation and keeps everyone active."
  },
  {
    icon: Shield,
    title: "Secure & Reliable",
    description: "Enterprise-grade security with RLS policies. Your club data is protected and backed up automatically."
  }
];

const pricingFeatures = {
  free: [
    "1 club",
    "Up to 16 players",
    "7-day advance scheduling",
    "Basic matchmaking",
    "3 session history",
  ],
  pro: [
    "Unlimited clubs",
    "Unlimited players",
    "Unlimited scheduling",
    "Advanced matchmaking",
    "Unlimited session history",
    "Custom matchmaking profiles",
    "Guest player support",
    "CSV export",
    "Club branding",
  ],
};

export default function LandingPage() {
  return (
    <div className="min-h-screen bg-gradient-to-b from-white to-green-50/30">
      {/* Navigation */}
      <header className="sticky top-0 z-50 w-full border-b bg-white/80 backdrop-blur-sm">
        <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4 sm:px-6 lg:px-8">
          <Link href="/" className="flex items-center gap-2">
            <div className="flex size-8 items-center justify-center rounded-lg bg-primary">
              <span className="text-lg font-bold text-primary-foreground">S</span>
            </div>
            <span className="text-xl font-bold tracking-tight">SmashScheduler</span>
          </Link>
          <nav className="hidden md:flex items-center gap-6">
            <Link href="#features" className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">
              Features
            </Link>
            <Link href="#how-it-works" className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">
              How It Works
            </Link>
            <Link href="#pricing" className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">
              Pricing
            </Link>
          </nav>
          <div className="flex items-center gap-3">
            <Button variant="ghost" asChild>
              <Link href="https://app.smashscheduler.com/login">Sign In</Link>
            </Button>
            <Button asChild>
              <Link href="https://app.smashscheduler.com/pricing">Get Started</Link>
            </Button>
          </div>
        </div>
      </header>

      {/* Hero Section */}
      <section className="relative overflow-hidden px-4 pt-16 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <div className="grid gap-12 lg:grid-cols-2 lg:gap-8 items-center">
            <div className="text-center lg:text-left">
              <Badge variant="secondary" className="mb-4">
                Now with Offline Support
              </Badge>
              <h1 className="text-4xl font-bold tracking-tight text-foreground sm:text-5xl lg:text-6xl">
                Schedule Badminton Sessions{" "}
                <span className="text-primary">Effortlessly</span>
              </h1>
              <p className="mt-6 text-lg leading-8 text-muted-foreground max-w-2xl mx-auto lg:mx-0">
                The ultimate badminton club management app. Create sessions, manage players, and let our intelligent matchmaking create balanced games every time. Works offline, syncs automatically.
              </p>
              <div className="mt-8 flex flex-col sm:flex-row gap-4 justify-center lg:justify-start">
                <Button size="lg" asChild>
                  <Link href="https://app.smashscheduler.com/pricing">
                    Start Free Trial
                    <ChevronRight className="ml-2 size-4" />
                  </Link>
                </Button>
                <Button size="lg" variant="outline" asChild>
                  <Link href="#features">Learn More</Link>
                </Button>
              </div>
              <div className="mt-8 flex items-center justify-center lg:justify-start gap-4 text-sm text-muted-foreground">
                <div className="flex items-center gap-1">
                  <Check className="size-4 text-primary" />
                  <span>No credit card required</span>
                </div>
                <div className="flex items-center gap-1">
                  <Check className="size-4 text-primary" />
                  <span>14-day free trial</span>
                </div>
              </div>
            </div>
            <div className="relative lg:h-[500px]">
              <div className="relative rounded-2xl bg-gradient-to-br from-primary/20 to-primary/5 p-8 shadow-2xl">
                <div className="space-y-4">
                  <div className="rounded-xl bg-white p-4 shadow-lg">
                    <div className="flex items-center justify-between mb-3">
                      <div className="flex items-center gap-2">
                        <div className="size-3 rounded-full bg-red-400" />
                        <div className="size-3 rounded-full bg-amber-400" />
                        <div className="size-3 rounded-full bg-green-400" />
                      </div>
                      <span className="text-xs text-muted-foreground">Session Overview</span>
                    </div>
                    <div className="space-y-2">
                      <div className="flex justify-between items-center p-2 bg-muted/50 rounded-lg">
                        <span className="text-sm font-medium">Court 1</span>
                        <Badge variant="secondary" className="text-xs">Active</Badge>
                      </div>
                      <div className="flex justify-between items-center p-2 bg-muted/50 rounded-lg">
                        <span className="text-sm font-medium">Court 2</span>
                        <Badge variant="secondary" className="text-xs">Active</Badge>
                      </div>
                      <div className="flex justify-between items-center p-2 bg-muted/50 rounded-lg">
                        <span className="text-sm font-medium">Court 3</span>
                        <Badge className="text-xs">Waiting</Badge>
                      </div>
                    </div>
                  </div>
                  <div className="rounded-xl bg-white p-4 shadow-lg">
                    <h4 className="text-sm font-semibold mb-3">Up Next</h4>
                    <div className="space-y-2">
                      <div className="flex items-center gap-3 p-2 bg-green-50 rounded-lg border border-green-100">
                        <div className="size-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold">JD</div>
                        <div className="flex-1">
                          <p className="text-sm font-medium">John Doe</p>
                          <p className="text-xs text-muted-foreground">Skill: 7/10</p>
                        </div>
                        <span className="text-xs text-green-600 font-medium">Next up</span>
                      </div>
                      <div className="flex items-center gap-3 p-2 bg-muted/30 rounded-lg">
                        <div className="size-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold">AS</div>
                        <div className="flex-1">
                          <p className="text-sm font-medium">Alice Smith</p>
                          <p className="text-xs text-muted-foreground">Skill: 8/10</p>
                        </div>
                        <span className="text-xs text-muted-foreground">In 2 mins</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section id="features" className="py-24 px-4 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <div className="text-center max-w-3xl mx-auto mb-16">
            <h2 className="text-3xl font-bold tracking-tight sm:text-4xl">
              Everything You Need to{" "}
              <span className="text-primary">Run Your Club</span>
            </h2>
            <p className="mt-4 text-lg text-muted-foreground">
              From scheduling to matchmaking, we have got you covered. Focus on playing, we will handle the logistics.
            </p>
          </div>
          <div className="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
            {features.map((feature) => (
              <Card key={feature.title} className="group border-0 shadow-lg bg-white/50 backdrop-blur-sm hover:shadow-xl transition-all duration-300">
                <CardHeader>
                  <div className="size-12 rounded-lg bg-primary/10 flex items-center justify-center mb-4 group-hover:bg-primary/20 transition-colors">
                    <feature.icon className="size-6 text-primary" />
                  </div>
                  <CardTitle>{feature.title}</CardTitle>
                </CardHeader>
                <CardContent>
                  <CardDescription className="text-base">{feature.description}</CardDescription>
                </CardContent>
              </Card>
            ))}
          </div>
        </div>
      </section>

      {/* How It Works */}
      <section id="how-it-works" className="py-24 px-4 sm:px-6 lg:px-8 bg-white">
        <div className="mx-auto max-w-7xl">
          <div className="text-center max-w-3xl mx-auto mb-16">
            <h2 className="text-3xl font-bold tracking-tight sm:text-4xl">
              How It <span className="text-primary">Works</span>
            </h2>
            <p className="mt-4 text-lg text-muted-foreground">
              Get started in minutes. Our intuitive interface makes managing your club a breeze.
            </p>
          </div>
          <div className="grid gap-8 md:grid-cols-3">
            {[
              {
                step: "1",
                title: "Create Your Club",
                description: "Sign up and create your club in seconds. Set your preferences and court count."
              },
              {
                step: "2",
                title: "Add Players",
                description: "Import your player list or add them individually. Track skill levels and preferences."
              },
              {
                step: "3",
                title: "Start Scheduling",
                description: "Create sessions and let our intelligent matchmaking handle the rest. Enjoy perfectly balanced games."
              }
            ].map((item) => (
              <div key={item.step} className="relative">
                <div className="flex flex-col items-center text-center">
                  <div className="size-12 rounded-full bg-primary flex items-center justify-center text-primary-foreground text-xl font-bold mb-4">
                    {item.step}
                  </div>
                  <h3 className="text-xl font-semibold mb-2">{item.title}</h3>
                  <p className="text-muted-foreground">{item.description}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Pricing Section */}
      <section id="pricing" className="py-24 px-4 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <div className="text-center max-w-3xl mx-auto mb-16">
            <h2 className="text-3xl font-bold tracking-tight sm:text-4xl">
              Simple, <span className="text-primary">Transparent</span> Pricing
            </h2>
            <p className="mt-4 text-lg text-muted-foreground">
              Start free and upgrade when you are ready. No hidden fees, cancel anytime.
            </p>
          </div>
          <div className="grid gap-8 md:grid-cols-2 max-w-4xl mx-auto">
            {/* Free Plan */}
            <Card className="border-0 shadow-lg relative overflow-hidden">
              <CardHeader className="text-center pb-8">
                <CardTitle className="text-2xl">Free</CardTitle>
                <div className="mt-4">
                  <span className="text-4xl font-bold">£0</span>
                  <span className="text-muted-foreground">/month</span>
                </div>
                <CardDescription className="mt-2">
                  Perfect for small clubs just getting started
                </CardDescription>
              </CardHeader>
              <CardContent>
                <ul className="space-y-3 mb-8">
                  {pricingFeatures.free.map((feature) => (
                    <li key={feature} className="flex items-center gap-3">
                      <Check className="size-4 text-primary flex-shrink-0" />
                      <span className="text-sm">{feature}</span>
                    </li>
                  ))}
                </ul>
                <Button variant="outline" className="w-full" asChild>
                  <Link href="https://app.smashscheduler.com/pricing">Get Started Free</Link>
                </Button>
              </CardContent>
            </Card>

            {/* Pro Plan */}
            <Card className="border-2 border-primary shadow-xl relative overflow-hidden">
              <div className="absolute top-0 right-0 bg-primary text-primary-foreground text-xs font-bold px-3 py-1 rounded-bl-lg">
                POPULAR
              </div>
              <CardHeader className="text-center pb-8">
                <CardTitle className="text-2xl">Pro</CardTitle>
                <div className="mt-4">
                  <span className="text-4xl font-bold">£9</span>
                  <span className="text-muted-foreground">/month</span>
                </div>
                <CardDescription className="mt-2">
                  For serious clubs that want the best experience
                </CardDescription>
              </CardHeader>
              <CardContent>
                <ul className="space-y-3 mb-8">
                  {pricingFeatures.pro.map((feature) => (
                    <li key={feature} className="flex items-center gap-3">
                      <Check className="size-4 text-primary flex-shrink-0" />
                      <span className="text-sm">{feature}</span>
                    </li>
                  ))}
                </ul>
                <Button className="w-full" asChild>
                  <Link href="https://app.smashscheduler.com/pricing">Start Free Trial</Link>
                </Button>
              </CardContent>
            </Card>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-24 px-4 sm:px-6 lg:px-8 bg-primary">
        <div className="mx-auto max-w-4xl text-center">
          <h2 className="text-3xl font-bold tracking-tight text-primary-foreground sm:text-4xl">
            Ready to Transform Your Club?
          </h2>
          <p className="mt-4 text-lg text-primary-foreground/80 max-w-2xl mx-auto">
            Join hundreds of badminton clubs already using SmashScheduler to run better sessions. Start your free trial today.
          </p>
          <div className="mt-8 flex flex-col sm:flex-row gap-4 justify-center">
            <Button size="lg" variant="secondary" asChild>
              <Link href="https://app.smashscheduler.com/pricing">Start Free Trial</Link>
            </Button>
            <Button size="lg" variant="ghost" className="text-primary-foreground hover:bg-primary-foreground hover:text-primary" asChild>
              <Link href="https://app.smashscheduler.com/login">Sign In</Link>
            </Button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t bg-muted/30 py-12 px-4 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <div className="flex flex-col md:flex-row justify-between items-center gap-4">
            <div className="flex items-center gap-2">
              <div className="flex size-8 items-center justify-center rounded-lg bg-primary">
                <span className="text-lg font-bold text-primary-foreground">S</span>
              </div>
              <span className="text-lg font-bold">SmashScheduler</span>
            </div>
            <p className="text-sm text-muted-foreground">
              © 2026 SmashScheduler. All rights reserved.
            </p>
            <div className="flex gap-6">
              <Link href="/privacy" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                Privacy
              </Link>
              <Link href="/terms" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                Terms
              </Link>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}
