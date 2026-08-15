# Transaction Management

## Purpose

Transactions ensure that a command is executed as a single atomic operation.

All persistence operations performed during a command execution either succeed together or fail together.

Transaction management is coordinated by the Application Layer.

---

# Transaction Scope

Transactions exist only for commands.

Commands modify the system state and therefore require transactional consistency.

Queries do not modify data and do not require application transactions.

---

# Transaction Lifecycle

A typical command follows this execution flow.

```
Receive Request

↓

Validate Request

↓

Load Aggregate

↓

Execute Domain Behavior

↓

Begin Transaction

↓

Persist Changes

↓

Commit Transaction

↓

Publish Domain Events

↓

Return Response
```

If any persistence operation fails, the transaction is rolled back.

---

# Transaction Boundary

The Application Layer defines the transaction boundary.

The boundary starts immediately before persistence begins.

The boundary ends after either:

- Commit
- Rollback

Business execution occurs before persistence.

---

# Atomicity

A command is treated as a single unit of work.

Either:

- Every persistence operation succeeds.

or

- Every persistence operation is rolled back.

Partial persistence is not allowed.

---

# Repository Coordination

A command may require one or more repositories.

The transaction guarantees consistency across all persistence operations executed during the command.

Repositories participate in the same transaction.

---

# Domain Behavior

Business behavior is executed before persistence begins.

The Domain Layer:

- Makes business decisions.
- Changes Aggregate state.
- Records Domain Events.

The Domain Layer does not manage transactions.

---

# Persistence

Persistence is executed inside the transaction.

The persistence mechanism is implementation-specific.

Examples include:

- Insert
- Update
- Delete
- Event-based persistence

The Application Layer remains independent from these implementation details.

---

# Commit

The transaction is committed only after all persistence operations complete successfully.

A successful commit makes the business operation durable.

Only after a successful commit may the workflow continue to event publishing.

---

# Rollback

If persistence fails:

- The transaction is rolled back.
- No changes are persisted.
- No Domain Events are published.

The system returns to its previous consistent state.

---

# Domain Events

Domain Events are recorded during business execution.

They are published only after a successful transaction commit.

This guarantees that published events always represent committed business facts.

---

# Queries

Queries do not define transaction boundaries.

Their responsibility is data retrieval only.

---

# Design Principles

Transaction management follows these principles:

- Commands are atomic.
- Queries are non-transactional.
- Business logic is transaction-independent.
- Persistence is transactional.
- Events are published only after commit.

---

# Responsibilities

Application Layer

- Defines transaction boundaries.
- Coordinates repositories.
- Commits transactions.
- Rolls back transactions.
- Publishes Domain Events.

Domain Layer

- Executes business behavior.
- Records Domain Events.

Infrastructure

- Implements the transaction mechanism.

---

# Summary

Transactions guarantee that every command is executed atomically.

The Application Layer coordinates transactional consistency while the Domain Layer remains focused solely on business behavior.
