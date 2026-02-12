# SmashScheduler Aesthetics Guide

UI/UX reference derived from the archived Blazor implementation. All future UI generation must follow these conventions.

---

## 1. Colour Palette

### Primary Brand

| Token                  | Hex       | Usage                          |
| ---------------------- | --------- | ------------------------------ |
| `--smash-primary-500`  | `#2ECC71` | Main brand, CTAs, highlights   |
| `--smash-primary-600`  | `#27AE60` | Hover / success                |
| `--smash-primary-700`  | `#229954` | Active / pressed               |
| Primary Lighter        | `#58D68D` | Light accents                  |
| Primary Darkest        | `#1E8449` | Deep emphasis                  |

### Secondary / Accent

| Token                 | Hex       | Usage                |
| --------------------- | --------- | -------------------- |
| `--smash-accent-500`  | `#3498DB` | Links, info, accents |
| `--smash-accent-600`  | `#2980B9` | Hover                |
| `--smash-accent-700`  | `#2471A3` | Active               |
| Accent Lighter        | `#5DADE2` | Light accents        |

### Semantic

| Role    | Default   | Dark      | Lighter   |
| ------- | --------- | --------- | --------- |
| Success | `#27AE60` | `#1E8449` | `#52BE80` |
| Warning | `#F39C12` | `#D68910` | `#F8C471` |
| Error   | `#E74C3C` | `#C0392B` | `#EC7063` |
| Info    | `#3498DB` | `#2471A3` | `#5DADE2` |

### Tertiary

| Token              | Hex       |
| ------------------ | --------- |
| `--smash-tertiary` | `#9B59B6` |
| Tertiary Dark      | `#7D3C98` |
| Tertiary Light     | `#AF7AC5` |

### Gender-Coded

| Token                     | Hex       | Usage          |
| ------------------------- | --------- | -------------- |
| `--smash-gender-female`   | `#E91E63` | Female avatars |
| `--smash-gender-male`     | `#3498DB` | Male avatars   |
| `--smash-gender-other`    | `#9E9E9E` | Other/unknown  |

### Neutrals (Light Mode)

| Token                     | Hex       | Usage                    |
| ------------------------- | --------- | ------------------------ |
| `--smash-background`      | `#F8F9FA` | Page background          |
| Background Grey           | `#ECF0F1` | Subtle section fills     |
| `--smash-surface`         | `#FFFFFF` | Card / paper surfaces    |
| `--smash-text-primary`    | `#2C3E50` | Headings, body text      |
| `--smash-text-secondary`  | `#7F8C8D` | Subtitles, labels        |
| `--smash-text-tertiary`   | `#BDC3C7` | Disabled / placeholder   |
| `--smash-border`          | `#E8ECEF` | Borders, dividers        |
| Lines Inputs              | `#BDC3C7` | Form field borders       |
| Divider Light             | `#F4F6F7` | Subtle dividers          |

### Neutrals (Dark Mode)

| Token            | Hex       |
| ---------------- | --------- |
| Background       | `#121212` |
| Background Grey  | `#1E1E1E` |
| Surface          | `#1E1E1E` |
| Drawer / Appbar  | `#1A1A1A` |
| Text Primary     | `#E8E8E8` |
| Text Secondary   | `#A0A0A0` |
| Text Disabled    | `#5C5C5C` |
| Lines Default    | `#2D2D2D` |
| Lines Inputs     | `#3D3D3D` |
| Divider          | `#2D2D2D` |
| Divider Light    | `#252525` |

### Skill Level Gradients

| Level                  | Gradient                                           |
| ---------------------- | -------------------------------------------------- |
| Beginner (1-3)         | `linear-gradient(135deg, #E74C3C, #EC7063)` (Red)  |
| Intermediate (4-6)     | `linear-gradient(135deg, #F39C12, #F8C471)` (Gold) |
| Advanced (7+)          | `linear-gradient(135deg, #27AE60, #52BE80)` (Green)|

### Team Identification

| Team   | Background                                                          |
| ------ | ------------------------------------------------------------------- |
| Team 1 | `linear-gradient(135deg, rgba(46,204,113,0.08), rgba(46,204,113,0.04))` |
| Team 2 | `linear-gradient(135deg, rgba(52,152,219,0.08), rgba(52,152,219,0.04))` |

---

## 2. Typography

### Font Family

**Roboto** (Google Fonts) with sans-serif fallback.

Weights loaded: 300 (Light), 400 (Regular), 500 (Medium), 700 (Bold).

### Scale

| Element              | Size          | Weight | Notes                              |
| -------------------- | ------------- | ------ | ---------------------------------- |
| Hero Title           | h4            | 700    |                                    |
| App Bar Title        | h6            | 700    | Gradient text fill (green to blue) |
| Page Title           | h4            | 700    |                                    |
| Section Header       | h6            | 600    |                                    |
| Stat Value           | 2.5rem (40px) | 700    |                                    |
| Body / Default       | 1rem (16px)   | 400    |                                    |
| Player Name          | 1rem          | 500    |                                    |
| Session Badge        | inherit       | 600    | Uppercase, letter-spacing 0.5px    |
| Stat Label           | 0.875rem      | 400    | Uppercase, letter-spacing 0.5px    |
| Small / Caption      | 0.75rem       | 400    |                                    |
| Skill Badge          | 13px          | 700    | White text on gradient             |
| Tiny (loading text)  | 0.625rem      | 400    |                                    |

### Text Transforms

Uppercase is used for: session state badges, stat labels, team labels ("TEAM 1" / "TEAM 2"), VS dividers.

---

## 3. Spacing & Layout

### Spacing Tokens

| Token     | Value |
| --------- | ----- |
| `--xxs`   | 4px   |
| `--xs`    | 8px   |
| `--sm`    | 12px  |
| `--md`    | 16px  |
| `--lg`    | 24px  |
| `--xl`    | 32px  |
| `--xxl`   | 48px  |

### Border Radius

| Token          | Value  | Usage                         |
| -------------- | ------ | ----------------------------- |
| `--radius-sm`  | 8px    | Badges, small elements        |
| `--radius-md`  | 12px   | Default (cards, inputs)       |
| `--radius-lg`  | 16px   | Feature cards, hero sections  |
| `--radius-xl`  | 24px   | Hero containers               |
| `--radius-full`| 9999px | Circles, pills                |

### Shadows

| Token       | Value                                               |
| ----------- | --------------------------------------------------- |
| `--shadow-sm` | `0 2px 4px rgba(0,0,0,0.08)`                      |
| `--shadow-md` | `0 4px 12px rgba(0,0,0,0.1)`                      |
| `--shadow-lg` | `0 8px 24px rgba(0,0,0,0.12)`                     |
| Hover         | `0 8px 24px rgba(46,204,113,0.2)` (green-tinted)  |
| FAB           | `var(--shadow-lg), 0 4px 20px rgba(46,204,113,0.3)` |
| Button Hover  | `0 4px 12px rgba(46,204,113,0.3)`                 |

### Responsive Breakpoints

| Name    | Width   | Notes                        |
| ------- | ------- | ---------------------------- |
| Mobile  | < 600px | Stack layouts, compact       |
| Tablet  | >= 600px | Side-by-side grids begin     |
| Desktop | >= 960px | Full layouts, wider padding  |

### Container

- Max width: large (1200px)
- Page padding: 16px (mobile), 24px (desktop)
- Top offset: 64px (app bar height)

---

## 4. Component Patterns

### Cards

- Border-radius: `--radius-lg` (16px)
- Transition: transform 150ms, box-shadow 150ms
- Hover: `translateY(-2px)` + `--shadow-lg`
- Active: `scale(0.98)`

**Session cards** have a 4px left border colour-coded by state:

| State    | Border Colour |
| -------- | ------------- |
| Draft    | `#F39C12`     |
| Active   | `#27AE60`     |
| Complete | `#BDC3C7`     |

**Match cards** have a 4px left border in primary green; completed matches use grey + 0.85 opacity.

**Stat cards** have a 4px top gradient border (primary to accent), centred text, padding `--lg`.

### Buttons

**Primary**: `linear-gradient(135deg, #2ECC71, #27AE60)`, green shadow on hover, `scale(0.98)` on active.

**FAB** (Floating Action Button): Fixed bottom-right (24px desktop, 16px mobile), gradient green, enhanced green-glow shadow, pulse animation when loading.

### Badges & Indicators

**Skill badge**: 32x32px circle, 2px white border, gradient background by skill tier, white bold text.

**Session badge**: Font-weight 600, uppercase, letter-spacing 0.5px, semantic colour.

**Status dot**: 8x8px circle, success green, animated pulse (opacity 1-0.6, scale 1-1.2, 2s loop).

### Avatars

Gender-coded background colour. Display uppercase initials (max 3 characters). Available in Small / Medium / Large.

### Empty States

Centre-aligned. Padding: `--xxl` vertical, `--lg` horizontal. Icon: 80px, tertiary colour, 60% opacity. Description: max-width 320px, secondary colour. CTA button below.

### Courts

**Empty court**: 2px dashed border (`--smash-border`), radius `--radius-lg`, min-height 180px. Icon container: 64x64px circle with gradient green/blue overlay. Hover: translateY(-2px), green border.

**Court grid**: xs=12, sm=6, md=4.

### Navigation

**App bar**: Elevation 0 (flat), 1px solid bottom border, white background (light) / `#1A1A1A` (dark). Logo as avatar (192px icon). Title with gradient text.

### Dialogs / Modals

Overlay: `rgba(0,0,0,0.7)` + 8px backdrop-blur. Close on Escape. Width varies: ExtraSmall to Medium.

### Forms

Valid input: 1px solid `#26B050`. Invalid input: 1px solid red. Standard 12px radius on input fields.

### Tables

Compact padding on mobile (4px 8px). Sortable columns with arrow indicators. Emoji indicators for state (shuttlecock for playing, chair for benched).

---

## 5. Animations & Transitions

### Transition Speeds

| Token               | Value    | Usage                        |
| -------------------- | -------- | ---------------------------- |
| `--transition-fast`  | 150ms ease | Hovers, button presses      |
| `--transition-normal`| 250ms ease | Default transitions         |
| `--transition-slow`  | 400ms ease | Complex state changes       |

### Keyframe Animations

| Name               | Duration | Effect                                  |
| ------------------ | -------- | --------------------------------------- |
| fab-pulse          | 1.5s     | Scale 1-1.05, green glow intensifies    |
| pulse-dot          | 2s       | Opacity 1-0.6, scale 1-1.2             |
| loading-breathe    | 2.4s     | Scale 1-1.04                            |
| loading-glow-pulse | 2.4s     | Opacity 0.5-0.9, scale 1-1.08          |
| loading-shimmer    | 1.8s     | translateX -100% to 100%               |

### Transform Effects

| Element             | Hover              | Active        |
| ------------------- | ------------------ | ------------- |
| Card                | translateY(-2px)   | scale(0.98)   |
| FAB                 | translateY(-2px) scale(1.02) | scale(0.95) |
| Filter Chip         | translateY(-1px)   | -             |
| Team Section        | scale(1.02)        | scale(0.98)   |
| Empty Court         | translateY(-2px)   | -             |
| Player (selectable) | translateX(4px)    | -             |

---

## 6. Iconography

**Library**: Material Symbols Outlined (Google Fonts / Lucide equivalent in Next.js).

No custom imagery, photographs, or background textures. The design relies entirely on iconography and colour.

### Common Icons

| Context            | Icon              |
| ------------------ | ----------------- |
| Badminton / Sport  | SportsTennis      |
| Groups / Players   | Groups            |
| AI / Matchmaking   | AutoAwesome       |
| Analytics          | Timeline          |
| Bench / Waiting    | Weekend           |
| Match / Game       | SportsScore       |
| Complete           | CheckCircle       |
| Add                | Add               |
| Edit               | Edit              |
| Delete             | Delete            |
| Navigate Back      | ArrowBack         |
| Start Match        | PlayArrow         |
| Queue              | Drafts            |
| Winner / Trophy    | EmojiEvents       |
| Chevron            | ChevronRight      |
| Settings           | Settings          |

### App Icon

Green (`#2ECC71`) background with white badminton shuttlecock graphic. Rounded corners (64px radius at 512px). PWA theme colour: `#2ECC71`.

---

## 7. Design Philosophy

### Principles

1. **Card-based layout** - Every content block lives in an elevated card or paper surface.
2. **Gradient accents** - Strategic green-to-blue gradients for visual interest (app title, stat borders, icon containers).
3. **Semantic colour coding** - Warning = draft/pending, success = active/complete, error = problems. Consistent everywhere.
4. **Micro-interactions** - Every interactive element has hover transforms, shadow elevations, and scale effects.
5. **Mobile-first** - Stack on small screens, expand on larger. Touch-friendly targets (min 64px height).
6. **Gender coding** - Visual differentiation via avatar background colours.
7. **Skill visualisation** - Colour-gradient badges for quick skill assessment.
8. **Friendly empty states** - Centred layout with large icon, clear message, and CTA.

### Light and Dark Mode

- Default: follow system preference.
- User toggle available: System / Light / Dark.
- Dark mode uses OLED-friendly deep blacks (`#121212`) with the same structural design.

### Accessibility

- High-contrast text colours.
- Consistent semantic colour meanings.
- Never reliant on colour alone (always paired with icons, labels, or state indicators).
- Keyboard navigation support.
- Focus states on all interactive elements.

---

## 8. Feature-Specific Patterns

### Session Management

- Session cards with left-border state coding.
- Date/time format: `dd MMM yyyy HH:mm` with bullet separator.
- Court count shown as badge/chip.

### Club Management

- Club card: gradient green icon container (56x56px), name, court count chip, game type chip, chevron right.
- Empty state with large icon and CTA.

### Matchmaking

- Dialog-based workflow with preview step.
- Two-team layout with VS divider (flanked by horizontal lines).
- Draft matches in separate section with queue UI.
- Court grid: xs=12, sm=6, md=4.
- Bench section: surface background, 16px radius, 1px border, player list with games-played badges.

### Player Management

- Player card: avatar (gender-coded), name, skill badge, optional actions.
- Skill scale: 1-10, grouped beginner/intermediate/advanced.
- Initials auto-extracted from name (max 3 characters, uppercase).

### Analytics

- Stat cards in 2x2 or 4-column grid.
- Collapsible expansion panels for real-time stats.
- Sortable player tables with arrow indicators.
- Shuttlecock emoji for playing, chair emoji for benched.

### Live Session

- Status bar: horizontal with animated status dot, playing count, completed count.
- Prominent "Complete Match" (filled primary), "Edit Players" (outlined).
- Pulse animations on active elements.

### Loading Screen

- Full viewport, centred with bottom offset (20vh).
- Animated breathing icon (64x64px, 14px radius).
- Radial glow effect with pulse.
- Progress bar: 160px width (max 60vw), 3px height, green gradient with shimmer.
