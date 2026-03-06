# ADR-0001: Phase 0 governance baseline

## Status
Accepted

## Context
Phase 0 requires repository-level governance and build defaults for a commercial multi-package ecosystem.

## Decision
Adopt central package management, repository-wide nullable + XML docs + analyzer defaults, SDK pinning via global.json, and deterministic build settings.

## Consequences
All projects inherit consistent quality gates and package governance from repository root.
