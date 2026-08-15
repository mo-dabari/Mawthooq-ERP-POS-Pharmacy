# Unit of Work

## Purpose

This document describes how the Unit of Work is implemented in the Mawthooq project.

It defines the transaction boundary for every write operation.

# Core Principle

In Mawthooq, the Unit of Work is the only component responsible for managing database transactions.

Repositories never create, commit, or rollback transactions.

# Responsibilities

The Unit of Work is responsible for:

- Opening the database connection.
- Beginning the database transaction.
- Providing the active connection and transaction to repositories.
- Committing all changes.
- Rolling back all changes when a failure occurs.
- Disposing transaction resources.

The Unit of Work contains no business rules.

# Repository Responsibilities

Repositories participate in the active Unit of Work.

A Repository is responsible only for:

- Creating SQL commands.
- Binding parameters.
- Executing SQL.
- Mapping data.

Repositories never:

- Open transactions.
- Commit transactions.
- Rollback transactions.
- Own the database connection lifecycle.

# Transaction Flow

Command Handler
│
▼
BeginTransaction()
│
▼
Load Aggregate
│
▼
Execute Domain Behavior
│
▼
Aggregate Records Domain Events
│
▼
Repository.Update(...)
│
▼
Infrastructure Translates Events into SQL
│
▼
Execute SQL
│
▼
Commit()
│
▼
Clear Domain Events
│
▼
Dispose Transaction

# Shared Transaction

All Write Repositories participating in the same Command share:

- the same database connection
- the same database transaction

This guarantees that all persistence operations succeed or fail together.

# Failure Handling

If any operation fails before Commit:

- the transaction is rolled back
- no SQL changes are persisted
- recorded Domain Events are not considered persisted

The operation fails as a single atomic unit.

# Event-Based Persistence

The Unit of Work coordinates the persistence process.

During a write operation:

- Aggregates execute business behavior.
- Aggregates record Domain Events.
- Repositories translate those events into SQL operations.
- SQL commands execute within the active transaction.
- Commit completes successfully.
- Domain Events are cleared.

# Design Goal

The Unit of Work guarantees transactional consistency while allowing repositories to focus exclusively on data access.

Business behavior, persistence, and transaction management remain clearly separated.
