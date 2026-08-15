# Database Architecture

## Purpose

This document defines the architectural role of the database within Mawthooq.

The database is responsible for durable data storage and transactional consistency. It is not responsible for implementing business rules or business workflows.

---

## Architectural Role

The database serves as the persistent storage mechanism for the system.

Its responsibilities include:

- Persisting business data.
- Maintaining relational integrity.
- Supporting transactional consistency.
- Providing efficient data retrieval.

Business behavior remains outside the database.

---

## Business Ownership

The Domain model is the source of business truth.

The database stores the business state produced by the Domain but does not determine how the business behaves.

Business rules must never depend on database-side logic.

---

## Data Ownership

Each business module owns its persistent data.

Infrastructure persists that data according to the contracts defined by the Application layer.

Direct modification of another module's persistent data is not permitted outside its architectural boundaries.

---

## Transactional Consistency

The database guarantees atomic persistence for each business operation.

A transaction either commits completely or leaves the persistent state unchanged.

Consistency is maintained at the persistence boundary.

---

## Integrity

Data integrity is achieved through cooperation between:

- Domain invariants.
- Application coordination.
- Database constraints.

Each layer protects the integrity that belongs to its responsibility.

---

## Database Independence

The business architecture is independent of the selected database engine.

Database-specific capabilities may be used inside Infrastructure when they improve implementation quality without leaking into higher layers.

---

## Schema Evolution

The database schema evolves together with the application architecture.

Schema changes must preserve business correctness and maintain compatibility with the current application version.

Infrastructure is responsible for applying schema changes in a controlled manner.

---

## Design Goals

The database architecture aims to provide:

- Reliable persistence.
- Transactional safety.
- Predictable performance.
- Long-term maintainability.
- Clear separation between storage and business behavior.
