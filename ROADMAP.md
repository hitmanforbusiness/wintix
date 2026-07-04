# Roadmap

Planned milestones for Wintix. Timelines are approximate and may shift as priorities evolve.

## v0.1.0 — Foundation (current)

- [x] WinUI 3 project scaffold with MVVM structure
- [x] Navigation shell (Home, Events, My Tickets, Settings)
- [x] Sample in-memory event and ticket data
- [x] GitHub-ready repo (README, LICENSE, CI workflow)

## v0.2.0 — Core UX

- [ ] Event detail page with seat map placeholder
- [ ] Ticket purchase flow (mock checkout)
- [ ] Search and category filters on Events page
- [ ] Light/dark theme toggle wired to Settings
- [ ] Empty states and error UI polish

## v0.3.0 — Persistence & accounts

- [ ] Local ticket cache (SQLite or Windows.Storage)
- [ ] Microsoft account / Entra ID sign-in
- [ ] Sync tickets across devices
- [ ] Offline ticket viewing

## v0.4.0 — Backend integration

- [ ] REST or GraphQL API client layer
- [ ] Real event catalog from remote service
- [ ] Order history and refunds
- [ ] Push / toast notifications for event reminders

## v0.5.0 — Distribution

- [ ] MSIX packaging and auto-update
- [ ] Microsoft Store listing
- [ ] Crash reporting and analytics (opt-in)
- [ ] Accessibility audit (Narrator, keyboard nav, high contrast)

## Future ideas

- Wallet pass export (Apple/Google) via companion service
- QR / NFC check-in for venue staff mode
- Widgets and Live Tiles for upcoming events
- Multi-language support

## How to suggest features

Open a [GitHub issue](https://github.com/your-org/wintix/issues) with the **enhancement** label and describe the user scenario you want to solve.
