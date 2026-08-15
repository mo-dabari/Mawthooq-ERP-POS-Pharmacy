# Application Architecture

## Purpose

This folder documents the Application Layer architecture of Mawthooq.

The goal is to describe how business use cases are executed while remaining independent from infrastructure and presentation technologies.

The documentation focuses on application behavior, orchestration, CQRS, transactions, validation, event publishing, and dependency rules.

This is an architectural reference rather than implementation documentation.

---

# Responsibilities

The Application Layer is responsible for orchestrating business use cases while delegating all business decisions to the Domain Layer.

For the complete responsibility breakdown, architectural decisions, and design rationale, see `architecture-review.md`.

---

# Folder Structure
application/

├── README.md
├── 01-Application-overview.md
├── 02-Application-cqrs.md
├── 03-Application-command-architecture.md
├── 04-Application-query-architecture.md
├── 05-Application-request-response-contracts.md
├── 06-Application-validation.md
├── 07-Application-transactions.md
├── 08-Application-domain-events.md
├── 09-Application-error-handling.md
├── 10-Application-anti-duplicate-execution.md
├── 11-Application-concurrency.md
├── 12-Application-dependency-rules.md
└── Application-architecture-review.md

---

# Reading Order

For first-time readers, the recommended order is:

1. Overview
2. CQRS
3. Command Architecture
4. Query Architecture
5. Request & Response Contracts
6. Validation
7. Transactions
8. Domain Events
9. Error Handling
10. Anti-Duplicate Execution (Idempotency)
11. Concurrency
12. Dependency Rules
13. Architecture Review (summary)

---

# Design Goals

The documentation is intended to help both developers and AI assistants understand:

- How use cases are executed.
- How commands differ from queries.
- How transactions are managed.
- How repositories are used.
- How validation is organized.
- How domain events are published.
- How dependencies are enforced.

Every document focuses on architecture rather than implementation details.

---

# Scope

This folder documents only the Application Layer.

Business concepts are documented under:


.ai/domain/


Infrastructure implementation details are documented under:

.ai/infrastructure/


API behavior is documented under:
