# Events

## Purpose

This document describes the domain events that may be produced by the Active Ingredient aggregate.

---

# Domain Events

The Active Ingredient aggregate currently does not publish domain events.

This is an intentional architectural decision.

The aggregate represents a reference catalog and does not coordinate business workflows across other aggregates.

---

# Why No Domain Events?

Changes to an Active Ingredient do not trigger business behavior in other aggregates.

For example:

* Renaming an ingredient does not modify existing medicines.
* Activating an ingredient does not affect existing data.
* Deactivating an ingredient only prevents future selection.

These operations remain local to the aggregate.

---

# Future Evolution

If future business requirements introduce workflows that depend on Active Ingredient changes, domain events may be added.

Examples could include:

* Active Ingredient Deactivated
* Active Ingredient Renamed

Such events should only be introduced when another business capability genuinely depends on them.

Events should never be created solely for completeness.

---

# Design Principle

Domain Events are introduced only when they represent meaningful business facts that other parts of the domain need to react to.

Reference catalog aggregates should remain simple unless business requirements evolve.
