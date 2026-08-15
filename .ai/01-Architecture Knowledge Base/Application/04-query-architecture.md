# Query Architecture

## Purpose

Queries retrieve information from the system without modifying its state.

The read side is optimized for data retrieval, reporting, searching, and presentation.

Queries never execute business behavior.

---

# Design Principles

Every query follows these principles:

- Read-only operation.
- Never modifies system state.
- Never invokes business behavior.
- Never raises Domain Events.
- Never starts a business transaction.
- Optimized for retrieval.
- Independent from Aggregate boundaries.

---

# Query Structure

A query consists of:

```
Query

↓

Query Handler

↓

Read Service

↓

Read Model

↓

DTO

↓

Response
```

Each component has a single responsibility.

---

# Query

A Query represents a request for information.

Examples:

- GetProductById
- SearchProducts
- GetInventoryBalance
- GetPurchaseHistory
- SearchCustomers
- GetSupplierStatement

Queries describe **what information is requested**, not how it is retrieved.

---

# Query Handler

The Query Handler orchestrates the execution of a query.

Responsibilities include:

- Validate request parameters.
- Invoke the appropriate read service.
- Return the requested data.
- Return an empty result when applicable.

A Query Handler never executes business logic.

---

# Read Service

Read Services are responsible for retrieving data from the persistence layer.

They may:

- Execute SQL.
- Join multiple tables.
- Perform projections.
- Apply filtering.
- Apply sorting.
- Apply pagination.

Read Services are optimized for performance rather than business consistency.

---

# Read Model

The Read Model represents data optimized for consumption.

Unlike Aggregates, Read Models:

- Do not enforce business rules.
- Do not contain behavior.
- May combine data from multiple modules.
- May flatten complex relationships.
- Exist only to support queries.

The structure of a Read Model is driven by consumer needs.

---

# DTO

Queries return DTOs (Data Transfer Objects).

DTOs contain only the information required by the caller.

They do not expose Domain behavior.

They are designed for transport rather than business logic.

---

# Response

Each query defines its own response contract.

The response contains only the data required by the caller.

Responses are use case specific and are independent from other queries.

Queries do not use a generic response wrapper.

A query response may contain:

- A single DTO.
- A collection of DTOs.
- A paged projection.
- A summary projection.
- An empty response when no data matches the request.

---

# Aggregate Independence

Queries are not required to respect Aggregate boundaries.

A single query may retrieve data from:

- Multiple tables.
- Multiple modules.
- Multiple aggregates.

This flexibility allows the read side to be optimized for reporting and user experience.

---

# Repository Usage

Repositories are part of the write model.

Queries should not use repositories.

Instead, they access dedicated read services or read data access components.

This separation keeps repositories focused on Aggregate persistence.

---

# Transactions

Queries do not define business transactions.

Read operations are independent from the transactional workflow of commands.

---

# Validation

Query validation focuses on:

- Required parameters.
- Paging constraints.
- Sorting options.
- Filtering rules.
- Identifier format.

Business validation belongs to the write side.

---

# Dependency Direction

```
Query

↓

Query Handler

↓

Read Service

↓

Infrastructure
```

The read side has no dependency on Domain behavior.

---

# Performance

The read side is optimized for performance.

Implementation techniques may include:

- Optimized SQL.
- Covering indexes.
- Projections.
- Paging.
- Filtering.
- Database views.
- Read-only models.

These optimizations are implementation details and remain outside the Application architecture.

---

# Summary

Queries retrieve information without modifying the system.

They use dedicated read services and read models to provide efficient data retrieval while remaining completely independent from business behavior implemented in the Domain Layer.
