# Code311

Code311 is a modular, design-system-neutral UI framework ecosystem for ASP.NET Core (.NET 10), with a Tabler-backed implementation and an official hybrid MVC + Razor demo host.

## Repository status

This repository now includes:

- **Foundation and governance**: solution layout, SDK pinning, centralized package versions, analyzer/nullability/doc enforcement.
- **Framework-neutral contracts and core orchestration**:
  - `Code311.Ui.Abstractions`
  - `Code311.Ui.Core`
- **Tabler implementation packages**:
  - `Code311.Tabler.Core`
  - `Code311.Tabler.Components`
  - `Code311.Tabler.Dashboard`
  - `Code311.Tabler.Widgets.DataTables`
  - `Code311.Tabler.Widgets.Calendar`
  - `Code311.Tabler.Widgets.Charts`
  - `Code311.Tabler.Mvc`
  - `Code311.Tabler.Razor`
- **Infrastructure packages**:
  - `Code311.Persistence.EFCore`
  - `Code311.Licensing`
- **Hybrid demo/integration portal**:
  - `Code311.Host`

## High-level architecture

Code311 is intentionally layered:

1. **Abstractions layer** (`Ui.Abstractions`) defines semantics/contracts with no design-system coupling.
2. **Core layer** (`Ui.Core`) provides neutral orchestration services (theme/feedback/loading).
3. **Design-system layer** (`Tabler.*`) maps abstractions into concrete UI behavior and rendering for MVC/Razor, dashboard, and widgets.
4. **Cross-cutting infrastructure** (`Persistence.EFCore`, `Licensing`) remains independent from `Ui.Core` and Tabler component primitives.
5. **Host layer** (`Code311.Host`) composes the ecosystem and demonstrates real integration flows without pushing host concerns down into framework packages.

## Run the host demo (`Code311.Host`)

> Prerequisites
>
> - .NET SDK 10 (preview)

From repository root:

```bash
dotnet restore Code311.sln
dotnet build Code311.sln
dotnet run --project src/Code311.Host/Code311.Host.csproj
```

Default host behavior:

- Uses **SQLite** demo persistence at `src/Code311.Host/App_Data/code311-host-demo.db`.
- Runs explicit startup licensing validation (demo in-memory license configured in host startup).
- Hosts both MVC and Razor Pages routes.

## Key demo sections/routes

Base URL assumes local launch profile default (`https://localhost:5001` or similar):

- Home: `/`
- Components:
  - Forms: `/ComponentsDemo/Forms`
  - Navigation: `/ComponentsDemo/Navigation`
  - Layout: `/ComponentsDemo/Layout`
  - Feedback: `/ComponentsDemo/Feedback`
  - Data: `/ComponentsDemo/Data`
  - Media: `/ComponentsDemo/Media`
- Dashboard: `/Dashboard`
- Widgets:
  - DataTables: `/Widgets/DataTables`
  - Calendar: `/Widgets/Calendar`
  - Charts: `/Widgets/Charts`
- Preferences / Theme: `/Preferences`
- Licensing Diagnostics (Razor Page): `/Diagnostics/Licensing`
- Architecture / About: `/Architecture/About`

## Test and validation

Run the full test matrix:

```bash
dotnet test Code311.sln
```

## Repository layout

- `src/` — product/framework/host packages
- `tests/` — unit and integration test projects
- `docs/` — ADRs and architecture proposal
