# Dependency Rules

## Purpose

This document defines the dependency rules of Mawthooq.

The objective is to ensure that each architectural layer depends only on stable abstractions and that business knowledge remains isolated from technical implementation.

---

# Dependency Direction

Dependencies always point inward.

```
Presentation
        │
        ▼
Application
        │
        ▼
Domain
        ▲
        │
Infrastructure
```

The Domain Layer is the architectural center of the system.

---

# Domain Layer

The Domain Layer defines the business model.

The Domain Layer must not depend on:

- Application
- Infrastructure
- Presentation
- Database technologies
- External frameworks

The Domain Layer has no knowledge of how the application is executed.

---

# Application Layer

The Application Layer coordinates business execution.

The Application Layer may depend on:

- Domain

The Application Layer must not depend directly on:

- SQL Server
- ADO.NET
- ASP.NET Core
- External services
- Infrastructure implementations

The Application Layer depends only on abstractions.

---

# Infrastructure Layer

The Infrastructure Layer implements technical concerns.

The Infrastructure Layer may depend on:

- Domain
- Application

Infrastructure provides implementations for contracts defined by the Application Layer.

Examples include:

- Repository implementations.
- Transaction management.
- Persistence.
- External integrations.

---

# Presentation Layer

The Presentation Layer is responsible for communication with external clients.

It may depend on:

- Application

It must not contain:

- Business rules.
- Persistence logic.
- Domain behavior.

---

# Dependency Inversion

Stable layers define contracts.

Technical layers implement them.

Typical examples include:

- Repository interfaces.
- External service abstractions.
- Unit of Work abstractions.

This allows the business architecture to remain independent from implementation details.

---

# Cross-Layer Communication

Communication follows the architectural boundaries.

```
Presentation

↓

Application

↓

Domain

↓

Infrastructure (implementation)
```

No layer may bypass its architectural responsibility.

---

# Architectural Independence

Each layer owns a distinct responsibility.

Business rules remain independent from:

- Database technology.
- Messaging technology.
- Web framework.
- User interface.

Changing infrastructure should not require changes to business logic.

---

# Design Principles

Dependency management follows these principles:

- Dependencies point inward.
- Stable layers own abstractions.
- Technical layers provide implementations.
- Business logic remains isolated.
- Architectural boundaries are respected.

---

# Summary

Mawthooq follows strict dependency rules to preserve architectural integrity.

The Domain Layer remains independent, the Application Layer orchestrates business execution, Infrastructure provides technical implementations, and Presentation communicates with external clients.

Respecting these boundaries keeps the system maintainable, testable, and adaptable to future technical changes.
