# Command Architecture

## Purpose

Commands represent business operations that modify the system.

Each command executes a single business use case by coordinating the Domain Layer, repositories, transactions, and event publishing.

Commands are the entry point of the write model.

---

# Design Principles

Every command follows these principles:

- Represents one business use case.
- Has a single responsibility.
- Executes one application workflow.
- Delegates business decisions to the Domain Layer.
- Persists changes through repositories.
- Executes within a transaction.
- May publish Domain Events after successful persistence.

Commands never contain business rules.

---

# Command Structure

A command consists of:

```
Command

↓

Command Handler

↓

Repository

↓

Aggregate

↓

Repository Save

↓

Commit

↓

Publish Events
```

Each component has a single responsibility.

---

# Command

A Command is a request to perform a business action.

Examples:

- CreateProductCommand
- UpdateProductCommand
- ReceivePurchaseCommand
- CompleteSaleCommand

A Command contains only the data required to execute the use case.

Commands do not contain business logic.

---

# Command Handler

A Command Handler orchestrates the execution of a single use case.

Responsibilities include:

- Validate request.
- Load Aggregates.
- Invoke Domain behavior.
- Persist changes.
- Commit transaction.
- Publish Domain Events.
- Return the use case response.

A handler coordinates the workflow but does not implement business rules.

---

# Repository

Repositories provide access to Aggregate Roots.

Responsibilities:

- Load Aggregates.
- Persist Aggregate state.

Repositories do not execute business workflows.

Repositories do not implement business rules.

---

# Aggregate

The Aggregate performs all business decisions.

Responsibilities include:

- Enforcing invariants.
- Validating business rules.
- Changing state.
- Raising Domain Events.

The Aggregate is the only place where business behavior is executed.

---

# Persistence

After successful execution of domain behavior, the handler requests persistence through the repository.

The persistence strategy is implementation-specific and may involve:

- Insert
- Update
- Delete
- Event-driven persistence
- Batch operations

The Application Layer remains independent of the persistence mechanism.

---

# Transaction

Commands execute inside an application transaction.

The transaction guarantees that all persistence operations succeed or fail together.

The transaction boundary belongs to the Application Layer.

---

# Domain Events

Aggregates may record Domain Events while executing business behavior.

These events are published only after the transaction has been successfully committed.

If persistence fails, no events are published.

---

# Validation

Validation occurs at multiple levels.

Request validation ensures the command contains valid input.

Business validation is enforced by the Domain Model.

The Application Layer never duplicates domain invariants.

---

# Response

- Every command defines its own response contract.
- Responses are specific to the use case and contain only the information required by the caller.
- There is no generic application-wide result wrapper.

---

# Dependency Direction

```
Command

↓

Command Handler

↓

Repository Interface

↓

Domain

↓

Infrastructure Implementation
```

Handlers depend only on abstractions.

Infrastructure provides concrete implementations.

---

# Lifetime

A command exists only for the duration of its execution.

After completion:

- Transaction ends.
- Events are published.
- Resources are released.
- Result is returned.

Commands are stateless and independent from previous executions.

---

# Summary

Commands are responsible for executing business use cases.

They coordinate repositories, transactions, and domain events while delegating all business decisions to the Domain Layer.

This separation keeps the write model explicit, maintainable, and aligned with the architectural principles of Mawthooq.
