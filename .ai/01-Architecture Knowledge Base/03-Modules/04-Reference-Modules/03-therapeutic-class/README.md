# Therapeutic Class Module

## Purpose

The Therapeutic Class module manages therapeutic classifications used throughout the system.

A Therapeutic Class represents the primary therapeutic category assigned to a medicine.

This module serves as a shared reference catalog for the Medicine domain.

---

## Responsibilities

The module is responsible for:

* Managing therapeutic classes.
* Maintaining official therapeutic class names.
* Managing lifecycle status (Active / Inactive).
* Providing stable references for medicines.

The module is not responsible for managing medicines, active ingredients, manufacturers, inventory, purchasing, or sales.

---

## Aggregate

The module contains a single aggregate:

* TherapeuticClass

The aggregate is intentionally lightweight and acts as a reference catalog.

---

## Relationships

The TherapeuticClass aggregate is referenced by:

* Medicine

through:

* TherapeuticClassId

Each Medicine belongs to one primary Therapeutic Class.

The Medicine aggregate owns the relationship.

TherapeuticClass has no knowledge of which medicines reference it.

---

## Ownership

The TherapeuticClass aggregate owns only its own data.

It never owns medicines or medicine-specific information.

---

## Business Scope

The module represents therapeutic classifications only.

It does not represent:

* Active Ingredients
* Manufacturers
* Products
* Suppliers
* Medical indications
* Clinical treatment guidelines

These concepts belong to their respective modules or external medical knowledge.

---

## Module Classification

Category:

Reference Catalog Module

Architectural Pattern:

Identity Module

Reference Catalog modules provide stable business references that may be shared across many operational aggregates.

---

## Design Principles

* Single Aggregate.
* Stable business identity.
* Primary therapeutic classification.
* Reference by identifier only.
* No child entities.
* No ownership of Medicine data.
* No workflow orchestration.
* Small aggregate boundary.
