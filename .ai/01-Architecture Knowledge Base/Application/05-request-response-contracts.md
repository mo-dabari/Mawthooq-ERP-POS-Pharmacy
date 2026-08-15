# Request & Response Contracts

## Purpose

Every application use case defines its own request and response contracts.

These contracts represent the public interface of a use case and define how external clients interact with the Application Layer.

Each use case owns its contracts.

---

# Design Principles

Request and Response contracts follow these principles:

- One request per use case.
- One response per use case.
- Immutable data structures.
- No business behavior.
- No infrastructure concerns.
- Independent from other use cases.

They represent application contracts rather than domain concepts.

---

# Request

A Request contains all information required to execute a single use case.

It describes **what the caller wants to do**.

A request:

- Contains input data only.
- Contains no behavior.
- Contains no business rules.
- Is immutable.
- Is specific to one use case.

Every command and every query defines its own request.

---

# Response

A Response contains the outcome of a successfully executed use case.

It describes **what the caller receives**.

A response:

- Contains output data only.
- Contains no behavior.
- Contains no business logic.
- Is immutable.
- Is specific to one use case.

Responses are designed for the needs of the caller.

---

# One Contract per Use Case

Each use case owns its contracts.

Examples:

- CreateProductRequest
- CreateProductResponse

- UpdateProductRequest
- UpdateProductResponse

- GetProductRequest
- GetProductResponse

Contracts are not shared between unrelated use cases.

---

# No Generic Response Wrapper

Mawthooq does not use a generic application-wide response wrapper.

Examples of intentionally avoided patterns include:

- Result<T>
- OperationResult<T>
- ErrorOr<T>
- OneOf<T>
- Generic Response<T>

Every use case explicitly defines its own response contract.

This keeps use case contracts simple, explicit, and self-descriptive.

---

# Relationship with DTOs

Response contracts may contain DTOs.

For example:

```
GetProductResponse
    ↓
ProductDto
```

DTOs represent transported data.

The Response represents the complete contract of the use case.

A DTO is not a replacement for a Response.

---

# Commands

Command requests contain the information required to execute a business operation.

Command responses contain the information returned after successful execution.

Typical examples include:

- Created identifier.
- Generated number.
- Updated values.
- Confirmation information.

---

# Queries

Query requests define retrieval criteria.

Examples include:

- Identifier.
- Search text.
- Filters.
- Sorting.
- Paging.

Query responses contain the requested projection.

---

# Validation

Requests are validated before use case execution.

Responses are never validated because they are produced by the application itself.

---

# Dependency Rules

Request and Response contracts belong to the Application Layer.

They must not depend on:

- Domain entities.
- Infrastructure.
- Database models.
- API models.

They define stable contracts between the Application Layer and its callers.

---

# Naming Convention

Requests:

- CreateProductRequest
- UpdateProductRequest
- GetProductRequest

Responses:

- CreateProductResponse
- UpdateProductResponse
- GetProductResponse

The naming convention should remain consistent throughout the solution.

---

# Summary

Every use case owns its Request and Response contracts.

These contracts define the public interface of the Application Layer.

Keeping contracts specific to each use case makes the architecture explicit, predictable, and easy to evolve without introducing generic wrappers or shared response abstractions.
