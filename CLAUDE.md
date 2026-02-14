# Project Protocol: Agentic Dev Lab

## Core Constraints
- **Language**: English
- **Spelling**: Use English (UK) exclusively (e.g., 'optimise', 'colour', 'centre').
- **Comments**: NEVER include comments in generated code. Code must be self-documenting and apply SOLID principals.
- **Verification**: Always run tests after any code change and ensure new code & edge cases are covered by tests.
- **Git Workflow**: ALWAYS commit and push changes when work is complete. Never leave uncommitted or unpushed changes.

## Supabase Clients
- **Server** (RSC/route handlers): `const supabase = await createClient()` from `@/lib/supabase/server` — async, cookie-based
- **Browser** (client components): `const supabase = createClient()` from `@/lib/supabase/client` — synchronous
- **Service role** (webhooks/admin): `createServiceClient()` from `@/lib/supabase/server` — bypasses RLS

## Auth
- Always use `supabase.auth.getUser()`, never `getSession()` — `getUser()` validates the JWT server-side
- Middleware (`src/middleware.ts`) refreshes sessions and redirects unauthenticated users to `/login`
- Auth gates in `src/lib/auth/gates.ts` check club association and active subscription before rendering protected pages

## Route Structure
- `src/app/(protected)/` — route group guarded by association + subscription gates
- `src/app/(protected)/clubs/[clubSlug]/` — club-scoped pages; layout validates membership, subscription, resolves club by slug
- `[clubSlug]` param is the human-readable slug (e.g., `shuttle-stars`), not the UUID

## Components
- Server components by default; use `"use client"` only when interactivity is required
- UI primitives from shadcn/ui in `src/components/ui/`
- App-level components (nav, breadcrumbs, FAB) in `src/components/`

## Database
- RPCs with `SECURITY DEFINER` for multi-table transactional operations (e.g., `create_club_with_subscription`)
- Nested PostgREST joins for related data (e.g., `clubs:club_id(id, name, slug, subscriptions(...))`)
- RLS policies gate all tables via `club_organisers` membership
- Slugs: `clubs.slug` is globally unique; `sessions.slug` is unique per club (`club_id, slug`)

## Tailwind v4
- ONLY put colour mappings, font-family, and border-radius in `@theme inline`
- NEVER put `--spacing-*`, `--shadow-*`, or `--transition-*` in `@theme inline` — this overrides the entire default namespace
- Custom design tokens (spacing, shadow, transition) belong in `:root` as CSS custom properties
