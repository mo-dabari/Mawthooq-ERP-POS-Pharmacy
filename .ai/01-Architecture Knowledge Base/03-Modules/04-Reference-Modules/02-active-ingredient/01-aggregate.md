# Active Ingredient Aggregate

## Purpose

The Active Ingredient aggregate represents a pharmaceutical substance that can be used in one or many medicines.

It acts as a reference catalog shared across the Medicine domain.

The aggregate is intentionally small and contains only information intrinsic to the active ingredient itself.

Medicine-specific information such as strength, dosage, concentration, or quantity does not belong here.

---

## Responsibilities

The Active Ingredient aggregate is responsible for:

- Maintaining the identity of an active ingredient.
- Maintaining the official ingredient name.
- Managing lifecycle status (Active / Inactive).
- Preventing duplicate ingredient definitions.
- Serving as a reference for MedicineIngredient relationships.

---

## What Does NOT Belong Here

The aggregate does **not** own:

- Medicine strength
- Dosage
- Ingredient quantity
- Concentration
- Units
- Medicine composition

Those concepts belong to the relationship between a Medicine and its ingredients.

---

## Aggregate Boundary

ActiveIngredient

The aggregate has no child entities.

It is intentionally designed as a lightweight reference aggregate.

---

## Relationships

ActiveIngredient is referenced by:

- Medicine

through:

MedicineIngredient

The relationship is:

Medicine
    ─── owns ───► MedicineIngredient
                           │
                           ▼
                   ActiveIngredientId

Ownership always remains inside the Medicine aggregate.

---

## Identity

Each Active Ingredient has its own unique identifier.

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

Inactive ingredients remain available for historical consistency.

---

## Design Principles

- Small aggregate.
- Reference data with business meaning.
- No business rules related to medicines.
- No ownership of dosage information.
- Stable identifier.
- Safe for cross-aggregate references.
