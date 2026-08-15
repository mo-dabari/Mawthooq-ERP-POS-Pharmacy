# Error Handling

## Purpose

This document defines how errors are handled in the Mawthooq project.

The goal is to ensure consistent behavior across all architectural layers while preserving business integrity.

# Core Principle

Errors are handled by the layer that owns the responsibility.

Each layer is responsible only for the errors it can understand.

# Domain

The Domain is responsible for business rule violations.

Examples:

- Invalid business state.
- Business invariant violations.
- Invalid business operations.

The Domain never handles:

- SQL errors
- HTTP errors
- Infrastructure failures

# Application

The Application layer coordinates error handling.

It is responsible for:

- Executing Commands and Queries.
- Managing the Unit of Work.
- Rolling back transactions when necessary.
- Returning application-level results.

The Application does not implement business rules.

# Infrastructure

The Infrastructure is responsible for technical failures.

Examples:

- SQL exceptions.
- Connection failures.
- Timeout errors.
- External service failures.

Infrastructure never makes business decisions based on these failures.

# API

The API translates application results into HTTP responses.

Responsibilities include:

- Returning appropriate HTTP status codes.
- Formatting error responses.
- Hiding internal implementation details.

The API never exposes database or infrastructure exceptions directly.

# Transaction Failures

If an error occurs during a write operation before Commit:

- The Unit of Work rolls back the active transaction.
- No partial changes are persisted.
- Domain Events are not considered persisted.

This guarantees transactional consistency.

# Concurrency Conflicts

Concurrency conflicts are detected during persistence.

When a conflict occurs:

- The transaction is rolled back.
- No changes are committed.
- The conflict is returned to the caller.

The Domain remains unaware of concurrency handling.

# Logging

Technical failures should be logged by the Infrastructure.

Business rule violations are not treated as system failures.

Logging must never expose sensitive information.

# Design Goal

The project separates business failures from technical failures.

Business rules remain inside the Domain.

Technical concerns remain inside the Infrastructure.

The Application coordinates both without mixing their responsibilities.
