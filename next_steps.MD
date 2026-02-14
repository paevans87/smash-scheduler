# General

## offline & sync
It is imperitive that this app works offline so we need to add the required safeguards in place to handle that. 

Subscription, club management, etc can work online only for simplicity but the session management needs to work offline.

To do this we will use Indexdb with a sync mechanism 

## Breadcrumbs
I'd like to add breadcrumbs to make sure we are managing things correctly. 

## urls & slugs
Rather than using the ID of a club / session, I'd like to use slugs. For instance `/clubs/{club-slug}`, `/sessions/{session-slug}` (session slug will likely be the date). 

## Patterns
Update CLAUDE.md with the key patterns we are using so these are adhered to

# Dashboard Display
We'll add some simple stats to show on the dashboard display

- Club Name
- Active Member count
- Upcoming session