# Error Handling

## Purpose

Error handling ensures that application failures are detected, communicated, and handled consistently while preserving business consistency.

The strategy separates business failures from technical failures and assigns each layer responsibility for handling its own errors.

---

# Error Categories

Application errors fall into three categories.

```
Application Errors

├── Validation Errors
├── Business Errors
└── Technical Errors
```

Each category originates from a different layer and has a different handling strategy.

---

# Validation Errors

Validation errors occur before business execution begins.

Examples include:

- Missing required fields.
- Invalid identifier format.
- Invalid numeric ranges.
- Invalid paging parameters.

Validation errors prevent the use case from executing.

---

# Business Errors

Business errors occur during domain execution.

Examples include:

- Product is discontinued.
- Inventory is insufficient.
- Purchase order is already closed.
- Active shift is required.
- Branch isolation rule is violated.

Business errors are produced by the Domain Layer.

---

# Technical Errors

Technical errors originate outside the business model.

Examples include:

- Database connection failure.
- Transaction failure.
- Concurrency conflict.
- External service unavailable.
- Unexpected infrastructure exception.

These errors are implementation concerns rather than business concerns.

---

# Responsibility

Application Layer

- Handles request validation.
- Coordinates error propagation.
- Returns the appropriate response contract.

Domain Layer

- Detects business rule violations.
- Protects aggregate consistency.

Infrastructure

- Detects technical failures.
- Reports persistence and external system failures.

Each layer handles only the errors it owns.

---

# Transaction Safety

If an error occurs during persistence:

- The transaction is rolled back.
- No partial changes are committed.
- No Domain Events are published.

The system remains in a consistent state.

---

# Error Propagation

Errors propagate upward through the architecture.

```
Infrastructure

↑

Application

↑

Presentation
```

Lower layers never depend on higher layers when reporting failures.

---

# User Communication

The Application Layer is responsible for returning a response that allows the caller to understand the outcome of the use case.

The exact representation of failures is defined by the response contract of each use case.

---

# Logging

Logging is an operational concern.

The Application architecture does not define how logging is implemented.

Logging mechanisms belong to the infrastructure or presentation layers.

---

# Design Principles

Error handling follows these principles:

- Fail fast.
- Preserve business consistency.
- Keep responsibilities separated.
- Never expose infrastructure details to the Domain Layer.
- Roll back failed business operations.

---

# Summary

Each architectural layer is responsible for handling its own failures.

Validation errors prevent execution.

Business errors protect domain consistency.

Technical errors are managed by infrastructure.

This separation keeps error handling predictable, maintainable, and aligned with the architecture of Mawthooq.
