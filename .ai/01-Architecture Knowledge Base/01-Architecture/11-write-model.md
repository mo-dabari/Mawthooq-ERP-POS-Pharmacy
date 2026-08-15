# Write Model

## Purpose

This document describes how the write side is implemented in the Mawthooq project.

The write model is responsible for protecting business rules and changing system state.

# Core Principle

State changes happen only through business behavior.

Data is never modified directly.

The Aggregate is the only component allowed to change business state.

# Write Flow

text id="4q5xxq" Client │ ▼ API │ ▼ Command │ ▼ Command Handler │ ▼ Load Aggregate │ ▼ Execute Domain Behavior │ ▼ Aggregate Records Domain Events │ ▼ Repository.Update() │ ▼ Event-Based Persistence │ ▼ Commit Transaction

# Commands

Commands represent an intention to change business state.

Examples:

- CreateProduct
- CreatePackagingVersion
- AddCompanyBarcodeToVersion
- RemoveCompanyBarcodeFromVersion
- ActivatePackagingVersions
- DeactivatePackagingVersions
- Rename
- ChangeCategory
- AllowReturnIfOpened
- DisallowReturnIfOpened
- Activate
- Discontinue
- ChangeNearExpiryThreshold
- ChangeImageName

Commands:

- contain input data
- do not contain business rules
- do not execute SQL
- do not make business decisions

# Command Handlers

Command Handlers coordinate the use case.

Responsibilities:

- validate command data
- load Aggregates
- execute domain behavior
- persist changes
- return results

Command Handlers must not:

- contain business rules
- modify Aggregate state directly
- execute SQL

# Aggregates

Aggregates are the center of the write model.

Responsibilities:

- protect invariants
- enforce business rules
- change state
- record Domain Events

All state changes occur through Aggregate behavior.

# Persistence

After domain behavior completes:

- The Aggregate contains the new state.
- Domain Events describe what happened.
- The Repository persists the changes.
- Concurrency is validated.
- The transaction is committed.

Persistence is driven by Domain Events.

Object comparison is not used.

# Business Rules

Business rules belong only to the Domain.

They never belong in:

- API
- Commands
- Command Handlers
- Repositories
- Infrastructure

Handlers coordinate.

Aggregates decide.

# Transaction Boundary

A write operation executes inside a transaction.

The transaction includes:

- Aggregate changes
- SQL operations
- concurrency validation
- event persistence

Either everything succeeds or everything is rolled back.

# Design Goal

The Write Model exists to ensure that business state changes are explicit, protected, and consistent.

The Domain expresses business behavior.

Infrastructure persists the results.
