# Composition Root

## Purpose

The Composition Root is responsible for constructing the application's object graph.

It defines how implementations are connected to the contracts required by the Application layer.

---

## Architectural Responsibility

Dependency registration is centralized within the Composition Root.

No other part of the application is responsible for creating or wiring Infrastructure services.

---

## Dependency Ownership

Infrastructure registers implementations for contracts defined by the Application layer.

Typical registrations include:

- Repository implementations.
- Database services.
- Transaction services.
- Logging services.
- File storage services.
- External integrations.
- Background processing services.

---

## Lifetime Management

The Composition Root defines the appropriate lifetime for every registered service.

Service lifetime decisions must support correctness, resource management, and predictable execution.

---

## Dependency Direction

Dependencies always point toward the business.

Infrastructure depends on Application contracts rather than the opposite.

The Composition Root preserves this dependency direction while assembling the application.

---

## Framework Isolation

Dependency injection frameworks are implementation details.

The selected container must remain confined to the Composition Root.

Business layers remain independent of the dependency injection technology.

---

## Extensibility

New Infrastructure components should become available by registration rather than by modifying business code.

Replacing an implementation should require changes only within the Composition Root.

---

## Design Goals

The Composition Root is designed to provide:

- Centralized dependency registration.
- Consistent object creation.
- Technology isolation.
- Replaceable implementations.
- Predictable application startup.
