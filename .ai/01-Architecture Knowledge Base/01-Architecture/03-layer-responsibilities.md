# Layer Responsibilities

## Purpose

This document defines the responsibility of each architectural layer.

Every piece of code should belong to the layer that owns its responsibility.

# Domain

## Responsibility

Represents the business itself.

Contains:

- Entities
- Value Objects
- Aggregates
- Domain Services
- Domain Events
- Business Rules

## Must Not

- Access databases
- Call external APIs
- Read configuration
- Perform logging
- Know about HTTP
- Depend on Infrastructure

# Application

## Responsibility

Coordinates business use cases.

Contains:

- Commands
- Queries
- Handlers
- Application Services
- DTOs
- Repository Interfaces

## Must Not

- Contain business rules
- Execute SQL
- Depend on HTTP
- Contain infrastructure logic

# Infrastructure

## Responsibility

Implements technical details.

Contains:

- Repository implementations
- SQL access
- File storage
- Email
- External services
- Logging
- Configuration

## Must Not

- Contain business rules
- Change domain behavior

# API

## Responsibility

Receives external requests.

Responsible for:

- Routing
- Validation
- Authentication
- Authorization
- Mapping requests
- Returning responses

## Must Not

- Contain business logic
- Execute SQL directly
- Implement business rules

# Dependency Rule

Each layer may depend only on the layers inside it.

API
↓
Application
↓
Domain
<br/>Infrastructure ─────► Application
Infrastructure ─────► Domain

The Domain is always the innermost layer.

# Where Should This Code Go?

When adding new code, ask:

- Is it a business rule? → Domain
- Is it coordinating a use case? → Application
- Is it a technical implementation? → Infrastructure
- Is it handling HTTP? → API

If none of these clearly apply, reconsider the design before writing code.
