# Dependency Rules

## Purpose

This document defines the allowed dependencies between architectural layers.

These rules preserve the project's architecture and prevent unintended coupling.

# Allowed Dependencies

| Layer          | May Depend On       |
| -------------- | ------------------- |
| API            | Application         |
| Application    | Domain              |
| Infrastructure | Application, Domain |
| Domain         | None                |

# Dependency Direction

Dependencies always point toward the center of the architecture.

API
↓
Application
↓
Domain
<br/>Infrastructure ─────► Application
Infrastructure ─────► Domain

The Domain never depends on another project layer.

# Forbidden Dependencies

The following dependencies are not allowed:

- Domain → Application
- Domain → Infrastructure
- Domain → API
- Application → Infrastructure
- Application → API
- API → Infrastructure

If a dependency violates these rules, the design should be reconsidered.

# Dependency Inversion

Outer layers implement contracts defined by inner layers.

Example:

- Repository interface → Application
- Repository implementation → Infrastructure

The business logic depends on abstractions, not implementations.

# Cross-Layer Communication

Communication between layers should happen only through well-defined contracts.

Layers must not access the internal implementation details of other layers.

# Architecture Review Checklist

Before introducing a new dependency, verify:

- Does it follow the allowed dependency direction?
- Does it increase coupling unnecessarily?
- Can the dependency be replaced with an abstraction?
- Does it preserve Domain independence?

If any answer is **No**, revisit the design before implementation.
