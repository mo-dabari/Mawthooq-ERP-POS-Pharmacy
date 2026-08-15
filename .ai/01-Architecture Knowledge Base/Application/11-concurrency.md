# Concurrency Control

## Purpose

Concurrency control protects the system from conflicting modifications performed by multiple users or processes.

Its goal is to preserve business consistency while allowing concurrent access to the system.

---

# Design Principles

Concurrency control follows these principles:

- Preserve business consistency.
- Detect conflicting modifications.
- Prevent accidental data loss.
- Keep the Domain Layer independent from persistence technology.
- Keep concurrency implementation inside the Infrastructure Layer.

---

# Concurrency Strategy

Mawthooq adopts an Optimistic Concurrency strategy.

The system assumes that conflicting updates are uncommon.

Instead of preventing concurrent access, conflicts are detected when changes are persisted.

---

# Optimistic Concurrency

A command executes using the current state of the Aggregate.

When persistence occurs, the system verifies that the Aggregate has not been modified by another transaction.

If a conflict is detected, persistence fails and the command is not committed.

---

# Conflict Detection

Concurrency conflicts are detected during persistence.

Typical examples include:

- Another user updates the same Aggregate.
- Another process modifies the Aggregate before the current transaction commits.

Conflict detection is a persistence concern.

The Application Layer coordinates the workflow but does not implement the detection mechanism.

---

# Conflict Handling

When a concurrency conflict occurs:

- Persistence fails.
- The transaction is rolled back.
- Domain Events are not published.
- The use case returns its response indicating that the operation could not be completed because the data has changed.

The caller may choose to reload the latest state and retry the operation.

---

# Aggregate Consistency

Concurrency is enforced at the Aggregate boundary.

Each Aggregate is persisted as a single consistent business unit.

Partial Aggregate updates are not allowed.

---

# Infrastructure Responsibility

The Infrastructure Layer is responsible for:

- Detecting concurrent modifications.
- Comparing persistence versions.
- Implementing the chosen concurrency mechanism.

The Application Layer remains independent from the implementation details.

---

# Domain Responsibility

The Domain Layer is unaware of concurrency mechanisms.

Its responsibility is to enforce business rules and maintain Aggregate consistency.

---

# Design Principles

Concurrency control follows these architectural principles:

- Optimistic by default.
- Detect conflicts during persistence.
- Never overwrite newer business state silently.
- Keep concurrency independent from business rules.
- Preserve Aggregate consistency.

---

# Summary

Mawthooq uses Optimistic Concurrency to protect business data from conflicting updates.

The Application Layer coordinates the workflow, the Infrastructure Layer detects conflicts, and the Domain Layer remains focused solely on business behavior.
