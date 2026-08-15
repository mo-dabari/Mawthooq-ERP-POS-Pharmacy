# Architecture

## Purpose

This directory documents the architecture of the Mawthooq project.

Its goal is to explain **how Mawthooq is designed**, **why architectural decisions were made**, and **how the different parts of the system work together**.

This is not a reference for Clean Architecture or CQRS in general.

It documents **how Mawthooq applies these concepts**.

# Objectives

The Architecture documentation helps contributors and AI agents:

- Understand the overall system design.
- Follow the project's architectural rules.
- Make consistent implementation decisions.
- Understand the reasoning behind important design choices.
- Avoid introducing architectural inconsistencies.

# Reading Order

Read the documents in the following order:

- system-overview.md
- architectural-style.md
- layer-responsibilities.md
- dependency-rules.md
- request-lifecycle.md
- cqrs.md
- event-based-persistence.md
- repository-strategy.md

Each document builds upon the previous one.

# Scope

This directory focuses on architectural concepts specific to Mawthooq, including:

- System structure
- Layer responsibilities
- Dependency rules
- Request lifecycle
- CQRS implementation
- Event-Based Persistence
- Repository strategy

Detailed business rules belong in the **Domain** documentation.

Implementation details belong in the corresponding project layers.

# Guiding Principles

The architecture is guided by the following principles:

- Business rules drive the design.
- Architecture is explicit.
- Responsibilities are clearly separated.
- Dependencies remain intentional.
- Knowledge is documented before it becomes tribal knowledge.

# Audience

This documentation is intended for:

- Project contributors
- Software architects
- Backend developers
- AI agents working on the repository

No prior knowledge of Mawthooq is assumed.

# Keeping This Documentation Updated

Whenever a significant architectural decision changes, the relevant document in this directory should be updated.

Major architectural changes should also be accompanied by an Architecture Decision Record (ADR).

Architecture documentation should always reflect the current design of the project.
