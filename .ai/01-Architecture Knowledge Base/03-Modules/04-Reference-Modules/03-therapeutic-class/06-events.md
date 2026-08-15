# Events

## Purpose

This document describes the domain events that may be produced by the TherapeuticClass aggregate.

---

# Domain Events

The TherapeuticClass aggregate currently does not publish domain events.

This is an intentional architectural decision.

---

# Rationale

The TherapeuticClass aggregate represents a reference catalog.

Changes to a therapeutic class do not initiate or coordinate business workflows.

Typical operations such as:

* Creating a therapeutic class
* Renaming a therapeutic class
* Activating a therapeutic class
* Deactivating a therapeutic class

remain local to the aggregate and do not require reactions from other domain aggregates.

---

# Future Evolution

If future business requirements introduce workflows that depend on therapeutic class changes, domain events may be introduced.

Examples include:

* Therapeutic Class Deactivated
* Therapeutic Class Renamed

Such events should only be added when they represent meaningful business facts that another part of the domain must react to.

---

# Design Principle

Domain Events exist to communicate significant business events between aggregates.

Reference Catalog aggregates should remain event-free unless a business workflow explicitly requires cross-aggregate coordination.
