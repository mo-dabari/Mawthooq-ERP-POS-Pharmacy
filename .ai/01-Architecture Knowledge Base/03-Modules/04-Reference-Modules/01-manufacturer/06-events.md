# Events

## Purpose

This document describes the domain events that may be produced by the Manufacturer aggregate.

---

# Domain Events

The Manufacturer aggregate currently does not publish domain events.

This is an intentional architectural decision.

---

# Rationale

The Manufacturer aggregate represents a reference catalog.

Changes to a manufacturer do not initiate or coordinate business workflows.

Typical operations such as:

* Creating a manufacturer
* Renaming a manufacturer
* Activating a manufacturer
* Deactivating a manufacturer

remain local to the aggregate and do not require reactions from other domain aggregates.

---

# Future Evolution

If future business requirements introduce workflows that depend on manufacturer changes, domain events may be introduced.

Examples include:

* Manufacturer Deactivated
* Manufacturer Renamed

Such events should only be added when they represent meaningful business facts that another part of the domain must react to.

---

# Design Principle

Domain Events are driven by business workflows—not by CRUD operations.

Reference Catalog aggregates should remain event-free unless a real business process requires cross-aggregate coordination.
