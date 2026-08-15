# Repository Strategy

## Purpose

This document describes how repositories are designed and used in the Mawthooq project.

It defines project-specific conventions for loading and persisting Aggregates.

# Core Principle

A Repository represents the persistence boundary of an Aggregate.

Its responsibility is to reconstruct Aggregates from storage and persist Aggregate changes.

Repositories do not contain business rules.

# Read and Write Separation

Mawthooq follows CQRS.

Write operations and read operations use different repositories.

| Operation | Repository       |
| --------- | ---------------- |
| Commands  | Write Repository |
| Queries   | Read Repository  |

A Write Repository must never be used to serve queries.

A Read Repository must never modify business state.

# Write Repository

The Write Repository is responsible for:

- Loading Aggregates
- Rehydrating the complete Aggregate
- Persisting Aggregate changes
- Enforcing optimistic concurrency
- Executing the Event-Based Persistence strategy

The Write Repository owns the Aggregate lifecycle.

# Read Repository

The Read Repository exists only to retrieve data for queries.

It:

- Returns DTOs or read models
- Uses SQL optimized for reading
- Does not construct Aggregates
- Does not execute business behavior

Performance is prioritized over object reconstruction.

# Aggregate Rehydration

Before executing a Command, the complete Aggregate is loaded from storage.

Business behavior is always executed on a fully rehydrated Aggregate.

The Domain never operates on partially loaded Aggregates.

# Saving Changes

After business behavior completes successfully:

- The Aggregate contains its new state.
- The Aggregate contains recorded Domain Events.
- The Write Repository persists those changes.
- The transaction is committed.

Persistence is driven by the recorded Domain Events, not by object comparison.

# Repository Interface

Repository interfaces belong to the Application layer.

Repository implementations belong to the Infrastructure layer.

The Domain does not know how persistence is implemented.

# Optimistic Concurrency

Every Aggregate includes a RowVersion.

The expected RowVersion is supplied when persisting changes.

If the stored RowVersion differs from the expected value:

- the update is rejected
- the transaction is rolled back
- the caller handles the concurrency conflict

# Repository Boundaries

A Repository manages exactly one Aggregate type.

Repositories do not coordinate multiple Aggregates.

Cross-Aggregate workflows belong to the Application layer.

# SQL Responsibility

Repositories may execute:

- INSERT
- UPDATE
- DELETE
- SELECT

The Aggregate never generates SQL.

Infrastructure decides how persistence is implemented.

# What Repositories Must Not Do

Repositories must not:

- contain business rules
- make business decisions
- validate domain invariants
- expose database tables
- return partially valid Aggregates

Those responsibilities belong elsewhere.

# Design Goal

Repositories provide a clear boundary between the Domain model and persistence.

The Domain expresses business intent.

The Repository ensures that business intent is stored correctly while remaining independent of the underlying database implementation.
