# Therapeutic Class Aggregate

## Purpose

The TherapeuticClass aggregate represents the primary therapeutic classification of medicines.

It acts as a shared reference catalog for the Medicine domain.

The aggregate contains only information intrinsic to the therapeutic class itself.

Medicine-specific information does not belong here.

---

## Responsibilities

The TherapeuticClass aggregate is responsible for:

* Maintaining the identity of a therapeutic class.
* Maintaining the official therapeutic class name.
* Managing lifecycle status (Active / Inactive).
* Preventing duplicate therapeutic class definitions.
* Serving as a stable reference for medicines.

---

## What Does NOT Belong Here

The aggregate does **not** own:

* Medicines
* Active ingredients
* Manufacturers
* Inventory
* Purchases
* Sales
* Clinical treatment protocols
* Medical indications

These concepts belong to their respective modules.

---

## Aggregate Boundary

TherapeuticClass

The aggregate has no child entities.

It is intentionally designed as a lightweight reference aggregate.

---

## Relationships

The TherapeuticClass aggregate is referenced by:

* Medicine

through:

TherapeuticClassId

The relationship is:

Medicine
─── references ───► TherapeuticClassId

Each Medicine belongs to one primary therapeutic class.

Ownership always remains inside the Medicine aggregate.

---

## Identity

Each Therapeutic Class has its own unique identifier.

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

Inactive therapeutic classes remain available for historical consistency.

---

## Design Principles

* Small aggregate.
* Reference catalog with business meaning.
* Stable identifier.
* Primary business classification.
* No child entities.
* No ownership of Medicine data.
* Safe for cross-aggregate references.
