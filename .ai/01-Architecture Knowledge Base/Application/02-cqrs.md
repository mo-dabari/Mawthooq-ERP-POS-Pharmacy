# CQRS Architecture

## Purpose

Mawthooq adopts the Command Query Responsibility Segregation (CQRS) architectural pattern.

CQRS separates operations that modify the system from operations that read the system.

This separation keeps business workflows explicit, improves maintainability, and allows the read and write sides to evolve independently.

---

# Design Goals

CQRS is used to achieve the following goals:

- Clear separation of responsibilities.
- Simpler business workflows.
- Independent optimization of reads and writes.
- Explicit application use cases.
- Better scalability.
- Reduced coupling between business logic and data retrieval.

CQRS in Mawthooq is an architectural pattern, not a distributed architecture.

---

# Two Execution Models

Every application request belongs to one of two categories.

```
                Request
                   │
        ┌──────────┴──────────┐
        │                     │
    Command                Query
        │                     │
 Changes System         Reads System
```

Commands and Queries follow different execution models.

---

# Commands

Commands represent business actions that modify the system.

Examples:

- Create Product
- Update Product
- Discontinue Product
- Receive Purchase
- Complete Sale
- Transfer Inventory

Commands execute business rules through the Domain Model.

A Command always represents an intention to change the system.

---

# Query

Queries retrieve information without modifying the system.

Examples:

- Get Product Details
- Search Medicines
- Get Inventory Balance
- Get Customer Statement
- Search Suppliers

Queries never execute business behavior.

Queries never modify aggregates.

---

# Command Characteristics

Commands:

- Execute a business use case.
- Load one or more Aggregates.
- Invoke Domain behavior.
- Persist changes.
- Execute inside a transaction.
- May publish Domain Events.
- Return operation results.

Commands are write operations.

---

# Query Characteristics

Queries:

- Read data only.
- Never modify the system.
- Never execute business rules.
- Never publish events.
- Never start transactions.
- May read from optimized read models.

Queries are read operations.

---

# Command Flow

```
Client

↓

Command

↓

Command Handler

↓

Repository

↓

Aggregate

↓

Business Behavior

↓

Repository Save

↓

Commit

↓

Publish Events

↓

Result
```

---

# Query Flow

```
Client

↓

Query

↓

Query Handler

↓

Read Model

↓

DTO

↓

Result
```

Queries bypass the Domain Model because no business behavior is executed.

---

# Read Model

Queries use dedicated read models.

Read models are optimized for retrieval rather than business consistency.

They may:

- Join multiple tables.
- Return DTOs.
- Flatten complex structures.
- Include calculated fields.
- Ignore Aggregate boundaries when appropriate.

Read models are not part of the Domain.

---

# Write Model

Commands always operate on the Domain Model.

The write model is responsible for:

- Business rules.
- Invariants.
- Aggregate consistency.
- Domain Events.
- State transitions.

All business decisions are executed inside the Domain.

---

# Repository Usage

Repositories belong to the write side.

Repositories are responsible for:

- Loading Aggregates.
- Persisting Aggregates.

Repositories are not used for reporting or search operations.

Queries use dedicated read components instead.

---

# Transactions

Transactions exist only on the command side.

Commands define transaction boundaries.

Queries execute without transactions unless required by the underlying data store.

---

# Validation

Validation differs between commands and queries.

Commands validate:

- Request correctness.
- Business rules.
- Domain invariants.

Queries validate only the request structure and filtering parameters.

---

# Dependency Direction

```
Command

↓

Application

↓

Domain

↓

Infrastructure
```

```
Query

↓

Application

↓

Read Model

↓

Infrastructure
```

The read side never depends on the Domain Model.

---

# Benefits

Using CQRS provides:

- Clear use case boundaries.
- Smaller application services.
- Better separation of concerns.
- Simpler maintenance.
- Independent optimization of reads and writes.
- Easier testing.
- Better scalability.

---

# Summary

CQRS separates business execution from data retrieval.

Commands execute business behavior through the Domain Model and persist changes.

Queries retrieve information through dedicated read models without invoking business logic.

This separation keeps the Application Layer simple, explicit, and aligned with the architecture of Mawthooq.
