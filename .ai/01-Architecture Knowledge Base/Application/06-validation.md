# Validation Strategy

## Purpose

Validation ensures that application requests satisfy the minimum requirements before business execution begins.

The validation strategy separates application-level validation from business validation, allowing each layer to enforce the rules it owns.

---

# Validation Layers

Validation is performed at multiple layers.

```
Client

↓

Application Validation

↓

Application Use Case

↓

Domain Validation

↓

Persistence
```

Each layer validates different concerns.

---

# Application Validation

The Application Layer validates the incoming request.

Typical validation includes:

- Required values.
- Invalid identifiers.
- Invalid ranges.
- Invalid formats.
- Missing mandatory fields.
- Invalid paging parameters.
- Invalid sorting parameters.

Application validation does not evaluate business rules.

---

# Domain Validation

Business validation belongs exclusively to the Domain Layer.

Examples include:

- Business invariants.
- State transitions.
- Aggregate consistency.
- Business policies.
- Domain rules.

The Application Layer must never duplicate these validations.

---

# Validation Responsibilities

Application Layer validates:

- Request completeness.
- Request format.
- Basic request correctness.

Domain Layer validates:

- Business rules.
- Aggregate invariants.
- Domain consistency.
- State validity.

Infrastructure validates:

- Technical constraints imposed by external systems.

Each layer owns its own validation responsibilities.

---

# Fail Fast

Validation follows the Fail Fast principle.

If validation fails, execution stops immediately.

The use case is not executed.

The Domain Layer is not invoked.

No transaction is started.

No persistence occurs.

---

# Command Validation

Commands validate whether the request is structurally correct before business execution begins.

Examples include:

- Required identifiers.
- Required values.
- Invalid numeric ranges.
- Invalid request structure.

Business decisions remain inside the Domain.

---

# Query Validation

Queries validate retrieval parameters.

Examples include:

- Page number.
- Page size.
- Search filters.
- Sorting fields.
- Required identifiers.

Queries do not perform business validation.

---

# Validation Order

Validation occurs before the use case begins executing.

```
Request

↓

Validate Request

↓

Execute Use Case
```

Invalid requests never reach the Domain Layer.

---

# Dependency Rules

Validation belongs to the Application Layer.

Business validation belongs to the Domain Layer.

Neither layer should duplicate the responsibilities of the other.

---

# Design Principles

The validation strategy follows these principles:

- Validate early.
- Keep responsibilities separated.
- Avoid duplicated business rules.
- Fail fast.
- Keep validation independent from infrastructure.

---

# Summary

Validation protects the Application Layer from invalid requests while allowing the Domain Layer to remain the single source of truth for business rules.

Each layer validates only the concerns it owns, resulting in a clear separation of responsibilities and a predictable execution flow.
