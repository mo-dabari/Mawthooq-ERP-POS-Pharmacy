# Domain Events

## Purpose

This document describes how Domain Events are used in the Mawthooq project.

It defines their lifecycle, ownership, and role within the architecture.

# Core Principle

In Mawthooq, Domain Events represent business actions that have already occurred.

They are produced by the Aggregate during business behavior.

They are **not** used for Event Sourcing.

# Event Lifecycle

Load Aggregate
│
▼
Execute Business Behavior
│
▼
Aggregate Records Domain Events
│
▼
Repository.Update()
│
▼
Infrastructure Reads Events
│
▼
Execute Event-Based Persistence
│
▼
Commit Transaction
│
▼
Clear Domain Events

# Event Ownership

Domain Events belong to the Aggregate.

Only the Aggregate may create and record Domain Events.

No other layer is allowed to create Domain Events on behalf of the Aggregate.

# When Events Are Created

A Domain Event is recorded immediately after a business action succeeds.

Examples:

- CreateProduct
- CreatePackagingVersion
- AddCompanyBarcodeToVersion
- RemoveCompanyBarcodeFromVersion
- ActivatePackagingVersions
- DeactivatePackagingVersions

Events describe completed business actions.

They never describe requested actions.

# Aggregate Responsibility

The Aggregate is responsible for:

- recording Domain Events
- exposing recorded Domain Events
- remaining unaware of how those events are processed

The Aggregate never dispatches events.

# Infrastructure Responsibility

The Infrastructure is responsible for:

- reading recorded Domain Events
- translating them into SQL operations
- executing persistence
- clearing events after a successful commit

The Infrastructure decides how an event is persisted.

# Failed Transactions

If the transaction fails:

- no changes are committed
- no events are considered persisted
- the operation fails as a single unit

# Event Scope

Domain Events are internal to the write side.

They exist to support business consistency and Event-Based Persistence.

They are not API contracts.

They are not integration messages.

# Design Goal

Domain Events allow the Domain to describe **what happened** without knowing **how it will be persisted**.

This keeps business behavior independent from persistence while providing explicit information for the Infrastructure to execute the correct persistence actions.
