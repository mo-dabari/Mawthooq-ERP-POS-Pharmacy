# Request Lifecycle

## Purpose

This document describes how a request flows through the system.

Understanding this lifecycle helps contributors and AI agents place logic in the correct layer.

# Request Flow

Client
│
▼
API
│
▼
Application
│
▼
Domain
│
▼
Repository Interface
│
▼
Infrastructure
│
▼
Database / External Services
│
▲
Infrastructure
│
▲
Application
│
▲
API
│
▲
Client

# Step 1 - API

The API receives the incoming request.

Responsibilities:

- Validate the request
- Authenticate and authorize if required
- Convert the request into an application command or query

The API does not contain business rules.

# Step 2 - Application

The Application layer executes the requested use case.

Responsibilities:

- Coordinate the workflow
- Load required data
- Invoke the Domain model
- Persist changes
- Return the result

# Step 3 - Domain

The Domain performs the business behavior.

Responsibilities:

- Enforce business rules
- Protect invariants
- Execute domain behavior
- Produce domain events when necessary

The Domain never performs infrastructure operations.

# Step 4 - Infrastructure

Infrastructure executes technical operations.

Examples:

- Database access
- External APIs
- File storage
- Email
- Logging

Infrastructure implements interfaces defined by the inner layers.

# Step 5 - Response

The result flows back through the same layers until the API returns the response to the client.

Business decisions should already be complete before the response leaves the Application layer.

# Lifecycle Rules

- Business rules belong in the Domain.
- Use case coordination belongs in the Application layer.
- Technical implementation belongs in Infrastructure.
- HTTP concerns belong in the API.
- Each layer communicates only through its defined boundaries.

# Summary

Every request follows the same high-level path:

Request
↓
API
↓
Application
↓
Domain
↓
Infrastructure
↓
Database / External Service
↑
Response

Keeping this flow consistent makes the system predictable, testable, and easier for both humans and AI systems to understand.
