# Application Layer Overview

## Purpose

The Application Layer coordinates the execution of business use cases.

It acts as the bridge between external clients (API, UI, background jobs, integrations) and the Domain Layer.

Its responsibility is orchestration, not business decision making.

---

# Responsibilities

The Application Layer is responsible for:

- Executing use cases.
- Coordinating domain objects.
- Loading Aggregates.
- Invoking domain behavior.
- Persisting changes.
- Managing transactions.
- Publishing domain events.
- Returning application results.

---

# What the Application Layer Does NOT Do

The Application Layer does not:

- Implement business rules.
- Enforce domain invariants.
- Contain SQL.
- Access the database directly.
- Know infrastructure implementation details.
- Perform object-relational mapping.
- Contain UI logic.

These responsibilities belong to other layers.

---

# Position in the Architecture

```
Presentation

↓

Application

↓

Domain

↓

Infrastructure
```

The Application Layer depends only on abstractions.

Infrastructure depends on the Application Layer—not the opposite.

---

# Use Case Orchestration

Each use case represents one application action.

Examples:

- Create Product
- Update Product
- Receive Purchase
- Complete Sale
- Create Supplier

Each use case coordinates the execution flow but delegates business decisions to the Domain Model.

---

# High-Level Execution Flow

```
Client

↓

Application Use Case

↓

Repository

↓

Aggregate

↓

Domain Behavior

↓

Repository Save

↓

Commit Transaction

↓

Publish Events

↓

Return Result
```

The Application Layer controls the workflow but does not implement the business logic executed by the Aggregate.

---

# CQRS

The Application Layer separates read and write operations.

Commands modify the system.

Queries read the system.

Each has its own execution model.

---

# Transactions

The Application Layer defines transaction boundaries.

A transaction starts before persistence and completes only after all required changes are successfully saved.

If persistence fails, the transaction is rolled back.

---

# Domain Events

Aggregates raise Domain Events during business execution.

The Application Layer is responsible for ensuring these events are dispatched only after the transaction has been successfully committed.

---

# Dependency Rule

Application depends on:

- Domain

Application does not depend on:

- SQL Server
- ADO.NET
- ASP.NET Core
- External services

Concrete implementations are provided by the Infrastructure Layer.

---

# Summary

The Application Layer is an orchestration layer.

Its primary responsibility is to execute use cases by coordinating repositories, aggregates, transactions, and domain events while keeping all business rules inside the Domain Layer.
