# Request Lifecycle

## Purpose

This document describes how a request is executed inside Mawthooq after it reaches the Application layer.

The lifecycle defines the responsibilities of each architectural layer and the sequence in which technical operations are performed.

The goal is to provide a predictable execution flow while preserving the separation between business logic and technical implementation.

---

## Execution Flow

Every request follows the same high-level lifecycle:

```text
Client
    │
    ▼
Presentation
    │
    ▼
Application
    │
    ▼
Domain
    │
    ▼
Infrastructure
    │
    ▼
Database / External Systems
    │
    ▼
Application
    │
    ▼
Presentation
    │
    ▼
Client
```

Each layer is responsible only for its own concerns.

---

## Step 1 — Request Reception

The Presentation layer receives the incoming request.

Its responsibilities include:

- Receiving transport-specific requests.
- Authentication and authorization.
- Model binding.
- Request validation.
- Invoking the appropriate Application use case.

No business logic is executed in this layer.

---

## Step 2 — Use Case Execution

The Application layer coordinates the execution of the requested business operation.

Typical responsibilities include:

- Loading required aggregates.
- Calling domain behaviors.
- Coordinating repositories.
- Managing transactional boundaries.
- Returning the response model.

The Application layer orchestrates the workflow but does not implement business rules.

---

## Step 3 — Domain Execution

The Domain layer executes the business behavior.

Typical activities include:

- Validating invariants.
- Applying business policies.
- Updating aggregate state.
- Producing domain events.

The Domain remains completely unaware of persistence and external technologies.

---

## Step 4 — Persistence

After successful domain execution, the Application layer delegates persistence to Infrastructure.

Infrastructure is responsible for:

- Executing SQL commands.
- Persisting aggregate changes.
- Managing optimistic concurrency.
- Performing transactional database operations.

Persistence is an implementation concern and remains isolated from the business model.

---

## Step 5 — External Operations

If the use case requires communication with external systems, Infrastructure performs those operations after the business behavior has been completed.

Examples include:

- File storage
- External APIs
- Message brokers
- Email services

External integrations must never contain business decision logic.

---

## Step 6 — Transaction Completion

The transaction is committed only after all required persistence operations have completed successfully.

If any operation fails before completion, the transaction is rolled back.

The transaction boundary guarantees consistency for the current business operation.

---

## Step 7 — Response Generation

After successful execution:

- Required data is returned to the Application layer.
- The response model is constructed.
- Presentation formats the response for the client.

Infrastructure never constructs API responses.

---

## Failure Handling

If execution fails:

- The current operation stops immediately.
- The active transaction is rolled back when applicable.
- The failure is propagated back through the Application layer.
- Presentation returns the appropriate client response.

Business consistency always takes priority over partial execution.

---

## Architectural Responsibilities

| Layer | Primary Responsibility |
|--------|------------------------|
| Presentation | Receive and return requests |
| Application | Coordinate business workflow |
| Domain | Execute business rules |
| Infrastructure | Execute technical operations |
| Database / External Systems | Persist and provide external capabilities |

Each layer remains responsible only for its own architectural role.

---

## Design Goals

The request lifecycle is designed to ensure:

- Predictable execution.
- Clear ownership.
- Explicit persistence.
- Reliable transaction handling.
- Complete separation between business logic and technical implementation.

Every use case in Mawthooq follows this lifecycle regardless of the business module being executed.
