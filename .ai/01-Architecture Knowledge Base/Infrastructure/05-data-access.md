# Data Access

## Purpose

This document defines how the Infrastructure layer accesses persistent data in Mawthooq.

The objective is to provide explicit, predictable, and maintainable data access while keeping business logic completely isolated from persistence technology.

---

## Architectural Ownership

All database interaction is owned by the Infrastructure layer.

No other layer may:

- Execute SQL.
- Access database connections.
- Depend on database-specific APIs.
- Read or write tables directly.

The Application layer communicates only through its defined contracts.

---

## Data Access Strategy

Mawthooq uses explicit data access.

Every database operation is intentionally implemented and directly traceable to its corresponding business operation.

Database behavior is never inferred automatically.

---

## Read and Write Separation

Read operations and write operations have different responsibilities.

### Read Operations

Read operations retrieve data required by application use cases.

They are optimized for data retrieval and response construction.

Read operations do not modify business state.

---

### Write Operations

Write operations persist changes produced by the Domain layer.

They are responsible only for storing business state and enforcing persistence consistency.

---

## SQL Execution

SQL statements are executed explicitly by Infrastructure.

Each query or command is implemented intentionally according to the requirements of the corresponding use case.

Automatic SQL generation is not part of the architectural approach.

---

## Mapping Responsibility

Infrastructure is responsible for translating between:

- Database representations.
- Domain models.
- Application read models.

Mapping logic belongs entirely to Infrastructure and must remain invisible to higher layers.

---

## Connection Usage

Database connections are treated as Infrastructure resources.

Their creation, usage, and disposal are managed entirely within the Infrastructure layer.

Business components remain unaware of connection management.

---

## Database Provider Isolation

The chosen database technology is an implementation detail.

Higher layers never depend on:

- Provider-specific APIs.
- SQL Server features.
- Driver implementations.

Infrastructure isolates these dependencies from the rest of the system.

---

## Performance

Data access should prioritize predictable performance.

Typical objectives include:

- Executing only the required operations.
- Minimizing unnecessary database round trips.
- Avoiding hidden database work.
- Keeping execution costs explicit.

Performance decisions should always remain observable.

---

## Error Handling

Infrastructure detects and reports technical failures related to data access.

Examples include:

- Connectivity failures.
- Command execution failures.
- Timeout conditions.
- Provider-specific errors.

Business validation remains outside the data access layer.

---

## Design Goals

The data access architecture is designed to provide:

- Explicit database interaction.
- Clear ownership.
- Technology isolation.
- Maintainable implementations.
- Predictable execution.
- Reliable operational behavior.

These goals ensure that persistence remains transparent, controllable, and independent from the business model.
