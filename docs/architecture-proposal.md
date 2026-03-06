# Code311 Architecture Proposal (Final Pre-Implementation Refinement)

## Scope
Architecture-only artifact. No implementation code is included.

---

## Confirmed Foundations

### Strict dependency graph

```text
Code311.Ui.Abstractions
        ↓
Code311.Ui.Core
        ↓
Code311.Tabler.Core
        ↓
Code311.Tabler.Components
        ↓
Code311.Tabler.Dashboard + Code311.Tabler.Widgets.*
        ↓
Code311.Tabler.Mvc + Code311.Tabler.Razor
        ↓
Code311.Host
```

### Dependency rules
- `Code311.Ui.Abstractions`
  - No dependencies.
- `Code311.Ui.Core`
  - Depends only on `Code311.Ui.Abstractions`.
- `Code311.Tabler.*`
  - May depend on `Code311.Ui.*`; reverse forbidden.
- `Code311.Persistence.EFCore`
  - Depends primarily on `Code311.Ui.Abstractions`.
  - Provider-agnostic EF Core design; no SQL Server lock-in.
- `Code311.Licensing`
  - Independent package.
  - Startup validation required; runtime checks only at defined integration points.
- `Code311.Host`
  - Terminal application; never referenced by framework packages.
  - Hybrid by default (MVC + Razor Pages), demonstrative only.

### Tooling decision
- Bundler v1: **esbuild** (predictable, low-overhead, deterministic static asset output for RCL/NuGet distribution).

---

## Responsibility Separation (Locked)

| Area | `Code311.Tabler.Components` | `Code311.Tabler.Dashboard` | `Code311.Tabler.Widgets.*` |
|---|---|---|---|
| Core purpose | Reusable primitives + common composites | Dashboard shell and layout orchestration | Specialized widget integrations |
| Base domain rendering | Yes | No | No |
| Dashboard orchestration | No | Yes | No |
| Widget lifecycle | No | Placement coordination | Yes |
| Third-party JS adapters | Minimal/shared only | None/minimal | Primary ownership |
| Public API style | Semantic neutral parameters | Dashboard composition semantics | Widget-specific semantic options/builders |

---

## 1) Expanded Component Coverage Matrix (`Code311.Tabler.Components`)

> Parameter vocabulary is normalized to semantic terms (`tone`, `appearance`, `density`, `size`, `state`, `placement`, `layout`, `pinned`) and avoids CSS-framework tokens.

| Component | Domain | Implementation Type | Primary Semantic Parameters | Internal Dependency Requirements | Phase | Classification |
|---|---|---|---|---|---|---|
| `Cd311Input` | Forms | TagHelper | `field`, `label`, `state`, `size`, `density`, `appearance`, `hint` | `IFieldMetadataProvider`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311TextArea` | Forms | TagHelper | `field`, `state`, `size`, `density`, `appearance`, `hint`, `rows` | `IFieldMetadataProvider`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311Select` | Forms | TagHelper | `field`, `state`, `size`, `density`, `appearance`, `placement`, `options` | `ISelectOptionSource`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311Checkbox` | Forms | TagHelper | `field`, `state`, `size`, `appearance`, `label` | `IFieldMetadataProvider`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311RadioGroup` | Forms | TagHelper | `field`, `state`, `size`, `density`, `layout`, `options` | `IChoiceOptionSource`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311Switch` | Forms | TagHelper | `field`, `state`, `size`, `tone`, `appearance`, `label` | `IFieldMetadataProvider`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311InputGroup` | Forms | TagHelper | `field`, `appearance`, `size`, `density`, `placement`, `state` | `IFieldMetadataProvider`, `IInputAdornmentResolver`, `ITablerFormClassMapper` | Phase 2 | Composite |
| `Cd311DateInput` | Forms | TagHelper | `field`, `state`, `size`, `density`, `appearance`, `placement` | `IDateFormatProvider`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Primitive |
| `Cd311FileUpload` | Forms | ViewComponent | `field`, `state`, `size`, `appearance`, `layout`, `placement` | `IFileUploadPolicy`, `IValidationStateResolver`, `ITablerMediaClassMapper` | Phase 3 | Composite |
| `Cd311SearchBox` | Forms | TagHelper | `field`, `state`, `size`, `appearance`, `placement`, `pinned` | `ISearchQueryBinder`, `IValidationStateResolver`, `ITablerFormClassMapper` | Phase 2 | Pattern helper |
| `Cd311FieldGroup` | Forms | ViewComponent | `layout`, `density`, `appearance`, `state`, `placement`, `legend` | `IFormLayoutPolicy`, `IValidationSummaryComposer`, `ITablerLayoutClassMapper` | Phase 2 | Composite |
| `Cd311ValidationSummary` | Forms | TagHelper | `tone`, `appearance`, `density`, `state`, `placement`, `scope` | `IValidationSummaryComposer`, `IValidationStateResolver`, `ITablerFeedbackClassMapper` | Phase 3 | Pattern helper |
| `Cd311FormActionsBar` | Forms | ViewComponent | `layout`, `density`, `appearance`, `placement`, `pinned`, `state` | `IFormActionModelBuilder`, `ICommandPolicyResolver`, `ITablerLayoutClassMapper` | Phase 2 | Pattern helper |
| `Cd311TopNav` | Navigation | ViewComponent | `layout`, `appearance`, `density`, `state`, `pinned`, `placement` | `INavigationModelProvider`, `INavStateResolver`, `ITablerNavigationClassMapper` | Phase 2 | Composite |
| `Cd311SidebarNav` | Navigation | ViewComponent | `layout`, `appearance`, `density`, `state`, `placement`, `pinned` | `INavigationModelProvider`, `INavStateResolver`, `ITablerNavigationClassMapper` | Phase 2 | Composite |
| `Cd311Breadcrumb` | Navigation | TagHelper | `appearance`, `density`, `size`, `placement`, `state` | `IBreadcrumbProvider`, `IRouteContextAccessor`, `ITablerNavigationClassMapper` | Phase 2 | Primitive |
| `Cd311Pagination` | Navigation | TagHelper | `size`, `appearance`, `density`, `state`, `placement`, `layout` | `IPagingModelFactory`, `IPagingStateResolver`, `ITablerDataClassMapper` | Phase 2 | Primitive |
| `Cd311Tabs` | Navigation | TagHelper | `layout`, `appearance`, `density`, `state`, `placement`, `pinned` | `ITabSetModelProvider`, `ITabStateResolver`, `ITablerNavigationClassMapper` | Phase 2 | Composite |
| `Cd311DropdownMenu` | Navigation | ViewComponent | `placement`, `appearance`, `size`, `state`, `layout`, `pinned` | `IMenuModelProvider`, `ICommandPolicyResolver`, `ITablerNavigationClassMapper` | Phase 2 | Composite |
| `Cd311CommandBar` | Navigation | ViewComponent | `layout`, `density`, `appearance`, `placement`, `pinned`, `state` | `ICommandModelProvider`, `ICommandPolicyResolver`, `ITablerNavigationClassMapper` | Phase 3 | Pattern helper |
| `Cd311MenuGroup` | Navigation | TagHelper | `layout`, `appearance`, `density`, `state`, `placement` | `IMenuModelProvider`, `INavStateResolver`, `ITablerNavigationClassMapper` | Phase 2 | Composite |
| `Cd311Container` | Layout | TagHelper | `layout`, `density`, `appearance`, `size`, `placement`, `pinned` | `ILayoutProfileResolver`, `IThemeProfileResolver`, `ITablerLayoutClassMapper` | Phase 2 | Primitive |
| `Cd311Section` | Layout | TagHelper | `layout`, `density`, `appearance`, `state`, `placement`, `pinned` | `ILayoutProfileResolver`, `IThemeProfileResolver`, `ITablerLayoutClassMapper` | Phase 2 | Primitive |
| `Cd311PageHeader` | Layout | ViewComponent | `layout`, `appearance`, `density`, `state`, `placement`, `pinned` | `IPageHeaderModelProvider`, `ICommandPolicyResolver`, `ITablerLayoutClassMapper` | Phase 2 | Composite |
| `Cd311Card` | Layout | TagHelper | `tone`, `appearance`, `density`, `size`, `state`, `layout` | `ICardModelPolicy`, `IThemeProfileResolver`, `ITablerLayoutClassMapper` | Phase 2 | Primitive |
| `Cd311Grid` | Layout | TagHelper | `layout`, `density`, `appearance`, `state`, `placement`, `size` | `IGridLayoutPolicy`, `ILayoutProfileResolver`, `ITablerLayoutClassMapper` | Phase 2 | Primitive |
| `Cd311Stack` | Layout | TagHelper | `layout`, `density`, `appearance`, `state`, `placement`, `size` | `IStackLayoutPolicy`, `ILayoutProfileResolver`, `ITablerLayoutClassMapper` | Phase 2 | Primitive |
| `Cd311Accordion` | Layout | ViewComponent | `layout`, `appearance`, `density`, `state`, `placement`, `pinned` | `IAccordionStateResolver`, `ILayoutProfileResolver`, `ITablerLayoutClassMapper` | Phase 2 | Composite |
| `Cd311Drawer` | Layout | ViewComponent | `placement`, `layout`, `appearance`, `density`, `state`, `pinned` | `IDrawerStateCoordinator`, `ICommandPolicyResolver`, `ITablerLayoutClassMapper` | Phase 3 | Composite |
| `Cd311TabsContainer` | Layout | TagHelper | `layout`, `appearance`, `density`, `state`, `placement`, `pinned` | `ITabSetModelProvider`, `ILayoutProfileResolver`, `ITablerLayoutClassMapper` | Phase 3 | Pattern helper |
| `Cd311PanelLayout` | Layout | Service + render pattern | `layout`, `density`, `appearance`, `state`, `placement`, `pinned` | `IPanelLayoutComposer`, `ILayoutProfileResolver`, `ITablerLayoutClassMapper` | Phase 3 | Pattern helper |
| `Cd311Alert` | Feedback | TagHelper | `tone`, `appearance`, `density`, `size`, `state`, `placement` | `IAlertQueue`, `IFeedbackSerializer`, `ITablerFeedbackClassMapper` | Phase 2 | Primitive |
| `Cd311ToastHost` | Feedback | ViewComponent | `placement`, `appearance`, `density`, `state`, `size`, `pinned` | `IToastQueue`, `IToastRenderModelFactory`, `ITablerFeedbackClassMapper` | Phase 3 | Composite |
| `Cd311Modal` | Feedback | ViewComponent | `placement`, `appearance`, `size`, `state`, `layout`, `pinned` | `IDialogOrchestrator`, `IDialogStateResolver`, `ITablerFeedbackClassMapper` | Phase 3 | Composite |
| `Cd311ConfirmDialog` | Feedback | ViewComponent | `tone`, `appearance`, `size`, `state`, `placement`, `layout` | `IConfirmInteractionService`, `IDialogOrchestrator`, `ITablerFeedbackClassMapper` | Phase 3 | Pattern helper |
| `Cd311PageAlertHost` | Feedback | Service + render pattern | `placement`, `appearance`, `density`, `state`, `layout`, `pinned` | `IPageAlertPipeline`, `IFeedbackSerializer`, `ITablerFeedbackClassMapper` | Phase 3 | Pattern helper |
| `Cd311BusyOverlay` | Feedback | TagHelper | `appearance`, `density`, `state`, `placement`, `layout`, `pinned` | `IBusyStateService`, `ILoaderRenderModelFactory`, `ITablerFeedbackClassMapper` | Phase 3 | Pattern helper |
| `Cd311BusyButton` | Feedback | TagHelper | `tone`, `appearance`, `size`, `state`, `placement`, `layout` | `IButtonStateCoordinator`, `IBusyStateService`, `ITablerFeedbackClassMapper` | Phase 3 | Pattern helper |
| `Cd311LockScreen` | Feedback | ViewComponent | `layout`, `appearance`, `density`, `state`, `placement`, `pinned` | `ILockScreenPolicy`, `IThemeProfileResolver`, `ITablerLayoutClassMapper` | Phase 4 | Composite |
| `Cd311PagePreloader` | Feedback | Service + render pattern | `appearance`, `density`, `state`, `placement`, `layout`, `pinned` | `IPreloadOrchestrator`, `ILoaderRenderModelFactory`, `ITablerFeedbackClassMapper` | Phase 3 | Pattern helper |
| `Cd311Progress` | Feedback | TagHelper | `tone`, `appearance`, `size`, `state`, `placement`, `density` | `IProgressModelFactory`, `IFeedbackSerializer`, `ITablerFeedbackClassMapper` | Phase 2 | Primitive |
| `Cd311Table` | Data | TagHelper | `layout`, `density`, `appearance`, `state`, `size`, `placement` | `ITableModelPolicy`, `ISortStateResolver`, `ITablerDataClassMapper` | Phase 2 | Primitive |
| `Cd311DataGridShell` | Data | ViewComponent | `layout`, `density`, `appearance`, `state`, `placement`, `pinned` | `IDataGridModelBuilder`, `IFilterStateResolver`, `ITablerDataClassMapper` | Phase 3 | Composite |
| `Cd311FilterToolbar` | Data | ViewComponent | `layout`, `density`, `appearance`, `state`, `placement`, `pinned` | `IFilterModelProvider`, `IQueryStateBinder`, `ITablerDataClassMapper` | Phase 3 | Pattern helper |
| `Cd311Badge` | Data | TagHelper | `tone`, `appearance`, `size`, `state`, `placement`, `density` | `IBadgeStylePolicy`, `IThemeProfileResolver`, `ITablerDataClassMapper` | Phase 2 | Primitive |
| `Cd311StatKpi` | Data | ViewComponent | `tone`, `appearance`, `density`, `size`, `state`, `layout` | `IStatModelProvider`, `ITrendIndicatorResolver`, `ITablerDataClassMapper` | Phase 2 | Composite |
| `Cd311ListGroup` | Data | TagHelper | `layout`, `density`, `appearance`, `state`, `placement`, `size` | `IListModelProvider`, `ISelectionStateResolver`, `ITablerDataClassMapper` | Phase 2 | Primitive |
| `Cd311KeyValueList` | Data | TagHelper | `layout`, `density`, `appearance`, `state`, `size`, `placement` | `IMetadataListFormatter`, `ILayoutProfileResolver`, `ITablerDataClassMapper` | Phase 2 | Primitive |
| `Cd311EmptyState` | Data | TagHelper | `tone`, `appearance`, `density`, `state`, `placement`, `layout` | `IEmptyStatePolicy`, `ICommandPolicyResolver`, `ITablerDataClassMapper` | Phase 2 | Pattern helper |
| `Cd311Avatar` | Media | TagHelper | `appearance`, `size`, `state`, `placement`, `density`, `tone` | `IAvatarModelPolicy`, `IMediaUrlResolver`, `ITablerMediaClassMapper` | Phase 2 | Primitive |
| `Cd311Icon` | Media | TagHelper | `appearance`, `size`, `state`, `placement`, `tone`, `density` | `IIconRegistry`, `IIconResolver`, `ITablerMediaClassMapper` | Phase 2 | Primitive |
| `Cd311Image` | Media | TagHelper | `appearance`, `size`, `state`, `placement`, `layout`, `density` | `IMediaUrlResolver`, `IMediaOptimizationPolicy`, `ITablerMediaClassMapper` | Phase 2 | Primitive |
| `Cd311FilePreview` | Media | ViewComponent | `layout`, `appearance`, `size`, `state`, `placement`, `density` | `IFilePreviewModelBuilder`, `IMediaTypeClassifier`, `ITablerMediaClassMapper` | Phase 3 | Composite |
| `Cd311ProfileBlock` | Media | ViewComponent | `layout`, `appearance`, `density`, `state`, `size`, `placement` | `IProfileSummaryProvider`, `IAvatarModelPolicy`, `ITablerMediaClassMapper` | Phase 3 | Composite |
| `Cd311Banner` | Media | ViewComponent | `tone`, `appearance`, `density`, `state`, `placement`, `pinned` | `IBannerContentProvider`, `IThemeProfileResolver`, `ITablerMediaClassMapper` | Phase 3 | Pattern helper |

---

## 2) Host Integration Matrix

### 2.1 Package matrix

| Aspect | Shared Responsibilities (`Code311.Tabler.Mvc` + `Code311.Tabler.Razor`) | MVC-only (`Code311.Tabler.Mvc`) | Razor-only (`Code311.Tabler.Razor`) |
|---|---|---|---|
| Core registration | Register `Ui.Core`, `Tabler.Core`, `Tabler.Components`, `Tabler.Dashboard`, selected `Tabler.Widgets.*` | Same shared registration for MVC apps | Same shared registration for Razor Pages apps |
| Feedback services integration | Wire alert/toast/dialog/busy pipelines to request lifecycle | Action-result feedback adapters and MVC model-state bridge | Handler-result feedback adapters and Razor handler-state bridge |
| Loader/preloader integration | Register loader orchestration and preloader policy services | MVC filter hooks for action-start/action-complete loader transitions | Razor handler filters for page-handler loader transitions |
| Theme system integration | Request-time theme resolution and preference application | Controller/view-context theme helpers | PageModel/view-data theme helpers |
| Asset pipeline integration | Asset contributor registration, ordered manifests, deduplication | MVC view helper hooks for scoped asset injection | Razor layout/page helper hooks for scoped asset injection |
| Licensing integration | Startup validation + bounded runtime checks at integration points | MVC filter/convention integration points | Razor filter/convention integration points |
| Design neutrality guard | Keep semantic API surface; no Tabler token exposure outside internals | Same | Same |

### 2.2 DI/service registration extensions

- `AddCode311TablerMvc(...)`
- `AddCode311TablerRazor(...)`
- `AddCode311TablerDashboard(...)`
- `AddCode311TablerWidgetsDataTables(...)`
- `AddCode311TablerWidgetsCalendar(...)`
- `AddCode311TablerWidgetsCharts(...)`
- `AddCode311PersistenceEfCore(...)` (provider-agnostic configuration surface)
- `AddCode311Licensing(...)`

### 2.3 Filters / conventions / helpers / base types

| Package | Filters | Conventions | Helpers | Base Types |
|---|---|---|---|---|
| `Code311.Tabler.Mvc` | `Code311FeedbackActionFilter`, `Code311BusyTransitionFilter`, `Code311ThemeContextFilter` | Controller CRUD UX convention set, model-state to feedback convention, view asset convention | `IHtmlHelper` semantic component helpers, feedback helper facade, theme helper facade | `Code311ControllerBase`, `Code311ViewModelBase` |
| `Code311.Tabler.Razor` | `Code311PageFeedbackFilter`, `Code311BusyTransitionPageFilter`, `Code311ThemePageFilter` | Page folder UX conventions, handler feedback convention, page asset convention | `PageModel` semantic helpers, feedback helper facade, theme helper facade | `Code311PageModelBase`, `Code311RazorViewModelBase` |

---

## 3) New / Updated Architecture Decisions

| Decision | Context | Chosen Option | Reasoning | Consequence |
|---|---|---|---|---|
| DR-0010: Semantic vocabulary normalization | Matrix drift risk from inconsistent parameter names | Standardize component vocabulary around `tone`, `appearance`, `density`, `size`, `state`, `placement`, `layout`, `pinned` | Improves API predictability and cross-design-system portability | API review checklist must enforce vocabulary |
| DR-0011: Explicit internal dependency contracts | Prior matrix used vague dependency labels | Use named internal contracts/services per component line item | Clarifies package responsibilities and test seams | Requires maintaining contract catalog documentation |
| DR-0012: Explicit implementation type taxonomy | Ambiguous type descriptions reduced precision | Restrict to: TagHelper, ViewComponent, HTML helper extension, Service + render pattern | Better implementation planning and ownership clarity | Architecture reviews must reject vague type labels |
| DR-0013: Host adapter parity as governance rule | Risk of capability drift between MVC and Razor adapters | Define parity matrix for shared behaviors and stack-specific deltas | Preserves consistent developer experience | Requires parity tests and release gate checks |

---

## 4) Approval Gate

Architecture deliverables finalized for this phase:
1. Expanded Component Coverage Matrix
2. Host Integration Matrix
3. New architecture decisions

Awaiting approval before any implementation work.
