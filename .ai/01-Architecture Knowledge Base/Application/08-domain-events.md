# Domain Events

## Purpose

Domain Events represent business facts that occurred during the execution of a business operation.

They allow the Domain Layer to communicate significant business changes without depending on infrastructure or external systems.

Domain Events describe **what happened**, not **what should happen next**.

---

# Design Principles

Domain Events follow these principles:

- Represent completed business facts.
- Are raised by Aggregates.
- Contain business information only.
- Do not contain business behavior.
- Do not access infrastructure.
- Do not execute side effects.

A Domain Event is a record of something that has already happened inside the Domain.

---

# Ownership

Only Aggregate Roots may record Domain Events.

Entities and Value Objects may participate in business behavior, but the Aggregate Root is responsible for collecting and exposing the events produced during execution.

---

# Lifecycle

The lifecycle of a Domain Event is:

```
Execute Business Behavior

↓

Business Fact Occurs

↓

Aggregate Records Domain Event

↓

Application Persists Changes

↓

Transaction Commits

↓

Application Publishes Domain Events

↓

Lifecycle Ends
```

---

# Recording Events

Domain Events are recorded during business execution.

Recording an event does not publish it.

Events remain internal to the Aggregate until the Application Layer completes the persistence process successfully.

---

# Publishing Events

The Application Layer is responsible for publishing recorded Domain Events.

Events are published only after:

- Persistence succeeds.
- The transaction commits successfully.

Events must never be published before the business operation becomes durable.

---

# Failed Commands

If command execution fails:

- Persistence is rolled back.
- Recorded Domain Events are discarded.
- Nothing is published.

Only committed business operations produce published Domain Events.

---

# Event Responsibilities

A Domain Event is responsible for describing:

- What happened.
- Which business object was affected.
- Business data required by downstream consumers.

A Domain Event is not responsible for:

- Executing business logic.
- Calling services.
- Updating the database.
- Sending notifications.
- Publishing itself.

---

# Dependency Rules

Domain Events belong to the Domain Layer.

They must not depend on:

- Infrastructure.
- Database technologies.
- Messaging frameworks.
- External services.

They remain pure business concepts.

---

# Relationship with the Application Layer

The Application Layer:

- Collects recorded Domain Events.
- Completes persistence.
- Commits the transaction.
- Publishes the events.

The Domain Layer remains unaware of how events are published.

---

# Relationship with Infrastructure

Infrastructure provides the technical implementation required to publish Domain Events.

The Domain Layer has no knowledge of:

- Message brokers.
- Event buses.
- Databases.
- External integrations.

This separation preserves Domain independence.

---

# Event-Based Persistence

Mawthooq uses an Event-Based Persistence strategy within the Infrastructure Layer.

This strategy determines how Aggregate changes are persisted.

It is an infrastructure concern and is independent from the concept of Domain Events documented here.

The existence of Event-Based Persistence does not change the responsibilities or lifecycle of Domain Events.

---

# Design Principles

Domain Events follow these architectural principles:

- Business-first.
- Infrastructure-independent.
- Immutable.
- Recorded during business execution.
- Published after successful commit.
- Never published on failure.

---

# Summary

Domain Events capture significant business facts that occur during Aggregate execution.

They are recorded by the Domain Layer, published by the Application Layer after a successful transaction, and implemented technically by the Infrastructure Layer.

Their purpose is to communicate business facts while preserving the independence of the Domain Model.
