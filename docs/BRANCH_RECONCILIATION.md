# Branch Reconciliation

**Audit date:** 2026-11-21  
**Repository:** `TrentBest/TheSingularityWorkshop.Ontology`  
**Policy:** exactly `master` and `development`; the owner performs all branch deletions.

This is an evidence snapshot, not permission to merge, close pull requests, delete branches, rewrite refs, publish packages, or deploy.

## Inventory snapshot

At the start of this audit, the GitHub branch endpoint returned four branches:

| Branch | Head at audit start | Role / finding |
|---|---|---|
| `development` | `c2f80d3141de9b05a50942df958f0ec9dea6cbdc` | Active development branch. .NET 10 target. |
| `master` | `395c2eb5e82752ef372a248de49d27998c213f72` | Default branch. .NET 8 target. Contains generated Sith-policy evidence commits. Release-source alignment is unresolved; see below. |
| `chore/standardize-ci-ontology` | `f8b4111174d0b51f12e6fdb855679d35ad17c042` | One unique commit relative to development; old CI workflow change. PR #2 remains open. |
| `docs/readme-standard` | `61bb7667081b23133f95d9aceb95432c5b8fe948` | Two unique commits relative to development; README quick-start/integration guide plus workflow change. PR #3 remains open. |

The repository is **NON-COMPLIANT** with the two-branch policy until the owner resolves the two extra branches.

### Follow-up commits made during this audit

- `f903b50b9e18f92ca130abfe64b925007e3252c4` — added this reconciliation record.
- `0a6ee33975ff517f33734e15c86665f5c43f5af3` — added a live Sith branch-inventory prerequisite to the NuGet publish workflow.
- The current `development` head after those commits is `0a6ee33975ff517f33734e15c86665f5c43f5af3`. The noncanonical branch heads and `master` were not changed.

## Noncanonical branch review

### `chore/standardize-ci-ontology`

- Exact head: `f8b4111174d0b51f12e6fdb855679d35ad17c042`.
- Comparison against development at audit start: 1 commit ahead, 16 behind; the only changed file is `.github/workflows/build.yml`.
- Its workflow targets .NET 8 and has a hard-disabled publish job.
- Current development has superseded this workflow with .NET 10, Codecov coverage, package artifacts, source-integrated MicroBundleDomain validation, and a separately hard-disabled publish job.
- Open PR: [#2 — chore: standardize .NET CI](https://github.com/TrentBest/TheSingularityWorkshop.Ontology/pull/2), base `development`, head `chore/standardize-ci-ontology`. It remains open and unmerged.

**Disposition:** the branch's CI implementation appears superseded by current development, but the open PR is an unresolved repository artifact. Do not delete this branch until the owner decides the PR's disposition. It is not currently marked READY FOR OWNER DELETION.

### `docs/readme-standard`

- Exact head: `61bb7667081b23133f95d9aceb95432c5b8fe948`.
- Comparison against development at audit start: 2 commits ahead, 16 behind; changed files are `README.md` and `.github/workflows/build.yml`.
- The README adds a Visual Studio quick start and existing-project integration guidance. Those additions are not present in current development.
- The workflow change hard-disables NuGet publication; current development also has a hard-disabled publish job.
- Open PR: [#3 — docs: add Ontology quick start and integration guide](https://github.com/TrentBest/TheSingularityWorkshop.Ontology/pull/3), base `development`, head `docs/readme-standard`. It remains open and unmerged.

**Disposition:** preserve the branch and its unique README work. It is not READY FOR OWNER DELETION. If this work is later integrated, verify the final README and workflow on development before considering deletion.

## Pull-request inventory

| PR | State | Head branch | Unique work / action |
|---|---|---|---|
| [#2](https://github.com/TrentBest/TheSingularityWorkshop.Ontology/pull/2) | Open, unmerged | `chore/standardize-ci-ontology` | CI change superseded by current development; owner must decide PR disposition. |
| [#3](https://github.com/TrentBest/TheSingularityWorkshop.Ontology/pull/3) | Open, unmerged | `docs/readme-standard` | Quick-start and integration documentation not yet present on development; preserve. |

No PR was merged or closed during this audit.

## Master / NuGet alignment

The NuGet profile currently lists `TheSingularityWorkshop.Ontology` version `0.1.0-alpha.2` as the latest published prerelease. Both the `master` and `development` project files inspected during this audit declare `0.1.0-alpha.3`. The `master` project targets .NET 8; `development` targets .NET 10.

Therefore this audit cannot identify either current branch head as the exact source commit for the published `0.1.0-alpha.2` package. Do not move, reset, or replace `master` based on branch equality or CI status. Before assigning released-baseline meaning to `master`, recover the exact successful alpha.2 publication commit from authoritative package/publish evidence and compare it to the current history.

## CI and Sith evidence

- The exact development head at audit start, `c2f80d3141de9b05a50942df958f0ec9dea6cbdc`, has a successful Build, test, and package workflow run: [run 43799255261](https://github.com/TrentBest/TheSingularityWorkshop.Ontology/actions/runs/43799255261), created 2026-11-16.
- The Sith-policy workflow fails on the four-branch inventory, as intended. The latest inspected default-branch evaluation is [run 44741634153](https://github.com/TrentBest/TheSingularityWorkshop.Ontology/actions/runs/44741634153), created 2026-11-20.
- The new publish prerequisite queries the live remote branch endpoint during an explicitly requested publish workflow dispatch. API failure or any inventory other than exactly `development` and `master` fails the prerequisite. `publish_nuget` depends on that job.
- The publish job remains explicitly hard-disabled with `&& false`; this change does not authorize or enable publication.
- A green build does not override branch policy or authorize publication.

## Owner deletion readiness

**READY FOR OWNER DELETION:** none.

| Branch | Why not ready |
|---|---|
| `chore/standardize-ci-ontology` | Its workflow is superseded, but PR #2 remains open; owner must decide whether to close it before deleting its head branch. |
| `docs/readme-standard` | PR #3 contains unique README guidance not yet present in development. Preserve or explicitly disposition that work first. |

After the owner resolves both PRs and any required documentation integration, refresh this audit from live GitHub refs and verify the inventory again. Do not infer completion from this historical snapshot.
