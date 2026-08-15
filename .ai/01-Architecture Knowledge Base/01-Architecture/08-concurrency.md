# Concurrency Strategy

## Purpose

This document describes how Mawthooq handles concurrent updates.

It defines the project's concurrency strategy for protecting Aggregate consistency.

# Concurrency Model

Mawthooq uses **Optimistic Concurrency**.

The system assumes that concurrent modifications are uncommon and verifies consistency when persisting changes.

No pessimistic locking is used.

# Why Optimistic Concurrency

The project favors:

- Better scalability
- Short-lived database transactions
- Reduced database locking
- Higher throughput

Concurrency conflicts are detected instead of prevented.

# Aggregate Version

Every Aggregate has a RowVersion.

The RowVersion represents the version of the Aggregate at the moment it was loaded.

The Aggregate itself does not interpret or modify the RowVersion.

# Update Flow

A typical update follows these steps:

Load Aggregate
│
▼
Read RowVersion
│
▼
Execute Domain Behavior
│
▼
Repository.Update(aggregate, expectedRowVersion)
│
▼
UPDATE ... WHERE RowVersion = @ExpectedRowVersion
│
▼
Success or Concurrency Conflict

# Conflict Detection

During persistence, the Infrastructure verifies that the stored RowVersion matches the expected RowVersion.

If no rows are affected, the update is treated as a concurrency conflict.

The Aggregate is not persisted.

# Responsibility

## Domain

The Domain:

- does not detect concurrency conflicts
- does not compare RowVersions
- does not know how concurrency is implemented

Business rules remain independent of persistence concerns.

## Repository

The Write Repository is responsible for:

- validating the expected RowVersion
- detecting concurrency conflicts
- completing or rolling back the transaction

## Application

The Application layer is responsible for deciding how to react to a concurrency conflict.

Possible actions include:

- notifying the user
- retrying the operation
- reloading the Aggregate

The chosen strategy depends on the use case.

# Transaction Boundary

Concurrency validation occurs inside the same transaction that persists the Aggregate changes.

This guarantees that validation and persistence are performed atomically.

# Design Goal

Concurrency protection should be invisible to the Domain while ensuring that business decisions are never persisted over stale data.

The Domain focuses on business behavior.

The Infrastructure protects data consistency.
