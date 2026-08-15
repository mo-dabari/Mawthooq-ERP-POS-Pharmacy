# Mawthooq ERP - AI Knowledge Base

## Purpose

This directory is the official knowledge base for the Mawthooq ERP project.

It is designed primarily for AI assistants and secondarily for human developers.

The purpose of this knowledge base is to ensure that every architectural decision, business rule, coding standard, and implementation guideline remains consistent throughout the lifetime of the project.

The AI must always prefer this knowledge base over assumptions or generic best practices whenever project-specific rules exist.

---

# How AI Must Use This Knowledge Base

Before generating code, reviewing code, refactoring, or answering implementation questions, the AI must:

1. Read this README first.
2. Identify the area of the project involved.
3. Read the corresponding documentation.
4. Follow project-specific rules.
5. Never invent architectural decisions.
6. Never contradict an Architecture Decision Record (ADR).

If project documentation conflicts with generic software recommendations, the project documentation always wins.

---

# Knowledge Structure

The knowledge base is divided into independent sections.

Each section has a single responsibility.

## Constitution

Defines the identity of the project.

Contains:

- Vision
- Mission
- Design Philosophy
- Core Principles
- Non-goals
- AI Operating Rules

---

## Architecture

Defines the architectural foundation.

Contains:

- Clean Architecture
- Modular Monolith
- Domain-Driven Design
- CQRS
- Repository Pattern
- Event-Based Persistence
- Transaction Strategy
- Concurrency Strategy
- Dependency Rules

---

## Domain

Defines the business model.

Contains:

- Bounded Contexts
- Aggregates
- Entities
- Value Objects
- Domain Events
- Business Rules
- Invariants
- Domain Services
- Factories
- Specifications

This is the source of truth for business behavior.

---

## Application

Defines application workflows.

Contains:

- Commands
- Queries
- Use Cases
- Validation
- Authorization
- Transactions
- DTOs
- Mapping Strategy

---

## Infrastructure

Defines technical implementation.

Contains:

- ADO.NET
- SQL Server
- Repository Implementations
- Persistence
- Event Dispatching
- Connection Management
- Performance Strategy

---

## API

Defines the HTTP layer.

Contains:

- Controllers
- Endpoints
- Middleware
- Authentication
- Authorization
- Error Handling
- Problem Details

---

## SQL

Defines database standards.

Contains:

- Naming Conventions
- Tables
- Indexes
- Constraints
- Performance Rules
- Query Guidelines

---

## Testing

Defines testing standards.

Contains:

- Unit Testing
- Integration Testing
- Naming Conventions
- Test Strategy
- Test Architecture

---

## ADR

Architecture Decision Records.

Every architectural decision made during the project must be documented here.

Nothing is considered an official architectural decision unless an ADR exists.

---

## Templates

Reusable templates used to generate consistent project artifacts.

Examples:

- Aggregate Template
- Command Template
- Repository Template
- SQL Template
- Test Template

---

## Prompts

Reusable prompts for AI.

Examples:

- Create Feature
- Review Feature
- Generate SQL
- Explain Code
- Refactor
- Create Tests

---

## Checklists

Verification checklists used before accepting changes.

Examples:

- Feature Checklist
- Pull Request Checklist
- Architecture Checklist
- Performance Checklist

---

# Updating This Knowledge Base

Whenever a new decision is made:

1. Update the appropriate documentation.
2. Create or update an ADR if the decision is architectural.
3. Update templates if necessary.
4. Update prompts if necessary.
5. Never duplicate information.
6. Keep one source of truth for every concept.

---

# AI Operating Principle

The objective is not simply to generate code.

The objective is to preserve the architectural integrity of Mawthooq ERP throughout its evolution.

Consistency is more important than speed.

Correctness is more important than convenience.

Explicit design is preferred over hidden behavior.

Project-specific rules always take precedence over generic recommendations.
