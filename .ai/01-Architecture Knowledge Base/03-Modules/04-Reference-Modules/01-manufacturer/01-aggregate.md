# Manufacturer Aggregate

## Purpose

The Manufacturer aggregate represents the organization responsible for producing pharmaceutical medicines.

It acts as a shared reference catalog for the Medicine domain.

The aggregate contains only information intrinsic to the manufacturer itself.

Medicine-specific information does not belong here.

---

## Responsibilities

The Manufacturer aggregate is responsible for:

* Maintaining the identity of a manufacturer.
* Maintaining the official manufacturer name.
* Managing lifecycle status (Active / Inactive).
* Preventing duplicate manufacturer definitions.
* Serving as a stable reference for medicines.

---

## What Does NOT Belong Here

The aggregate does **not** own:

* Medicines
* Medicine registration data
* Inventory
* Purchase information
* Sales information
* Pricing
* Suppliers

These concepts belong to their respective aggregates.

---

## Aggregate Boundary

Manufacturer

The aggregate has no child entities.

It is intentionally designed as a lightweight reference aggregate.

---

## Relationships

The Manufacturer aggregate is referenced by:

* Medicine

through:

ManufacturerId

The relationship is:

Medicine
─── references ───► ManufacturerId

Ownership always remains inside the Medicine aggregate.

---

## Identity

Each Manufacturer has its own unique identifier.

The identifier is stable and may safely be referenced by other aggregates.

---

## Lifecycle

Typical lifecycle:

Created

↓

Updated

↓

Activated / Deactivated

Deletion is generally not allowed once referenced by medicines.

Inactive manufacturers remain available for historical consistency.

---

## Design Principles

* Small aggregate.
* Reference catalog with business meaning.
* Stable identifier.
* No child entities.
* No ownership of Medicine data.
* Safe for cross-aggregate references.
* Independent from purchasing and inventory.
