# Event-Based Persistence

## Purpose

This document describes how the Mawthooq project persists Aggregate changes.

This strategy is specific to Mawthooq and should not be confused with Event Sourcing.

# Core Idea

Aggregates do not generate SQL.

Aggregates generate **Domain Events** that describe what business changes occurred.

The Infrastructure translates those events into SQL operations.

# Why

The project intentionally does not use:

- Entity Framework
- Change Tracking
- Aggregate Differ
- Snapshot Comparison

Instead, persistence is driven by the business actions that actually occurred.

# Persistence Flow

Load Aggregate
│
▼
Execute Domain Behavior
│
▼
Aggregate Records Domain Events
│
▼
Repository.Update(aggregate)
│
▼
Infrastructure Reads Domain Events
│
▼
Execute SQL Statements
│
▼
Commit Transaction

# Aggregate Responsibility

The Aggregate is responsible for:

- enforcing business rules
- protecting invariants
- recording Domain Events

The Aggregate never knows how data is stored.

# Repository Responsibility

The Write Repository is responsible for:

- starting the persistence process
- opening the transaction
- invoking the persistence strategy
- handling concurrency
- committing or rolling back the transaction

# Persistence Strategy

The persistence layer examines the recorded Domain Events.

Each event is mapped to one or more SQL operations.

Example:

ProductCreated
→ INSERT Product
<br/>PackagingVersionAdded
→ INSERT PackagingVersion
<br/>CompanyBarcodeAdded
→ INSERT ProductBarcode
<br/>ProductDiscontinued
→ UPDATE Product

The mapping belongs entirely to the Infrastructure layer.

# Benefits

This approach provides:

- explicit persistence behavior
- no object comparison
- no hidden tracking
- SQL generated from business actions
- clear separation between Domain and Infrastructure

# What This Is Not

This strategy is **not**:

- Event Sourcing
- Event Store
- Change Tracking
- Aggregate Diffing
- Snapshot Comparison

Domain Events exist to describe business behavior.

They are used internally to determine persistence actions.

They are not the system's source of truth.

# Concurrency

Optimistic concurrency is enforced during persistence.

The Repository validates the Aggregate's RowVersion before committing changes.

If the expected RowVersion does not match, the operation fails and no changes are committed.

# Transaction Boundary

All SQL operations generated from a single Aggregate update execute within the same database transaction.

Either all persistence operations succeed or all are rolled back.

# Design Goal

Event-Based Persistence allows the Domain to describe **what happened** while allowing the Infrastructure to decide **how it is persisted**.

This keeps business behavior independent from persistence implementation while avoiding the complexity of object comparison techniques.
