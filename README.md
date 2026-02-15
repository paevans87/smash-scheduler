# SmashScheduler Monorepo

This is a monorepo containing two applications:

## Structure

```
/apps
├── marketing/          # Static marketing site (landing page)
│   ├── src/
│   │   ├── app/       # Next.js pages
│   │   ├── components/ui/  # shadcn/ui components
│   │   └── lib/       # Utilities
│   └── package.json
│
└── web-app/           # Main application
    ├── src/
    │   ├── app/       # Next.js app router
    │   ├── components/ui/  # shadcn/ui components
    │   └── lib/       # Utilities, hooks, etc.
    └── package.json
```

## Apps

### Marketing Site (`/apps/marketing`)
- **Purpose**: Static marketing landing page
- **Port**: 3001 (development)
- **Features**: 
  - Landing page with features, pricing, and CTA
  - Static export for deployment
  - Links to web-app for sign up/login
- **Start**: `npm run dev:marketing`

### Web App (`/apps/web-app`)
- **Purpose**: Main application for club management
- **Port**: 3000 (development)
- **Features**:
  - Authentication (login/signup)
  - Club management
  - Player management
  - Session scheduling
  - Offline support
- **Start**: `npm run dev`

## Development

### Install dependencies
```bash
npm install
```

### Run web app (main app)
```bash
npm run dev
```

### Run marketing site
```bash
npm run dev:marketing
```

### Build all apps
```bash
npm run build
```

### Build specific app
```bash
npm run build:web-app
npm run build:marketing
```

## Deployment

### Marketing Site
The marketing app is configured for static export. Build outputs to `apps/marketing/dist/`.

### Web App
The web app is a full Next.js application with server-side rendering. Deploy to Vercel, Netlify, or your preferred platform.

## Notes

- Each app has its own `package.json` and dependencies
- UI components are duplicated in both apps (can be shared via a packages directory in the future)
- Marketing site links to `https://app.smashscheduler.com` for the web app (update as needed)
