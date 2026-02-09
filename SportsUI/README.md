# SportsUI

A Vue 3 single-page application for managing sports leagues, teams, players, and matches. Built with Vite, Tailwind CSS, and a .NET backend API.

## Features

- **CRUD Operations** — Full create, read, update, and delete for Leagues, Teams, Players, and Matches
- **Virtualized Lists** — All list views use `@tanstack/vue-virtual` for smooth scrolling performance, even with large datasets
- **Smart Server-State Management** — Matches module powered by TanStack Vue Query with automatic cache invalidation, optimistic seeding, and configurable stale times
- **Match Simulation Polling** — Simulate all matches with one click; smart polling detects when results stabilize and stops automatically
- **Custom Dropdown Component** — Styled, accessible `<CustomSelect>` replacing all native `<select>` elements, with keyboard navigation, separators, and animated transitions
- **Page Transitions** — Smooth fade transitions between routes and between loading/error/content states
- **Animated Data Updates** — Total passes badges pop-in with scale + fade when simulation results arrive
- **Accessibility** — Skip-to-content link, ARIA labels on all interactive elements, `aria-expanded`/`role="listbox"` on custom dropdowns, keyboard-navigable
- **Error Handling** — Friendly error states with retry buttons on all list views; toast notifications for success and failure
- **Disabled State UX** — Form selects gracefully disable with visual feedback when dependent data hasn't loaded
- **Responsive Dashboard** — Grid-based home page adapts from 1 to 4 columns across breakpoints

## Tech Stack

| Layer               | Technology                                            |
| ------------------- | ----------------------------------------------------- |
| Framework           | Vue 3.5 (Composition API, `<script setup>`)           |
| Build Tool          | Vite 7                                                |
| Styling             | Tailwind CSS v4                                       |
| Routing             | Vue Router 4                                          |
| Server State        | TanStack Vue Query                                    |
| List Virtualization | TanStack Vue Virtual                                  |
| HTTP                | Axios (leagues/teams/players), native fetch (matches) |
| Notifications       | vue-toastification                                    |

## Project Setup

```sh
npm install
```

### Development

```sh
npm run dev
```

> The Vite dev server proxies `/api` requests to `https://localhost:7253` (the .NET backend).

### Production Build

```sh
npm run build
```

### Lint

```sh
npm run lint
```
