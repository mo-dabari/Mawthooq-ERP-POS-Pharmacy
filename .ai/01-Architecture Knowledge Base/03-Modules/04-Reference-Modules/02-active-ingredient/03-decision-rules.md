# Decision Rules

## Purpose

This document explains the architectural decisions behind the Active Ingredient aggregate.

It focuses on *why* the aggregate is designed this way rather than *how* it is implemented.

---

# Why is Active Ingredient a Separate Aggregate?

An Active Ingredient has its own business identity.

It may be referenced by many medicines and should not be duplicated inside each Medicine aggregate.

Keeping it as a separate aggregate ensures a single source of truth for pharmaceutical ingredients.

---

# Why Does Medicine Reference Only ActiveIngredientId?

Aggregate references are made only through identifiers.

This keeps aggregate boundaries independent and prevents direct object references across aggregates.

The Medicine aggregate only needs to know which ingredient is used.

It does not require the Active Ingredient object itself.

---

# Why Does Medicine Own MedicineIngredient?

MedicineIngredient exists only because a Medicine exists.

It has no meaning outside the Medicine aggregate.

A MedicineIngredient cannot be shared between medicines.

For this reason, ownership belongs entirely to the Medicine aggregate.

---

# Why Doesn't Active Ingredient Store Strength?

Strength is not an intrinsic property of an Active Ingredient.

The same ingredient may appear in different medicines with different quantities.

Example:

* Paracetamol 500 mg
* Paracetamol 650 mg
* Paracetamol 1000 mg

The ingredient remains the same.

Only the relationship between the Medicine and the ingredient changes.

Therefore, strength belongs to MedicineIngredient.

---

# Why Is IngredientAmount a Value Object?

IngredientAmount represents a measurable value.

It has no identity.

Two amounts with the same value and unit are considered equal.

Its lifecycle is entirely controlled by MedicineIngredient.

---

# Why Doesn't Active Ingredient Know Which Medicines Use It?

Relationships are intentionally one-way.

Medicine references ActiveIngredient.

ActiveIngredient has no knowledge of the medicines that reference it.

This prevents unnecessary coupling and keeps the aggregate lightweight.

Queries that need this information should be handled by the query model.

---

# Why Is Deactivation Preferred Over Deletion?

Medicines may already reference an Active Ingredient.

Deleting it would break historical consistency and create invalid references.

Marking the ingredient as inactive preserves existing data while preventing future use.

---

# Why Is This Aggregate Intentionally Small?

The aggregate represents a reference catalog.

Its responsibility is to define pharmaceutical substances—not medicine formulations.

Keeping the aggregate small improves maintainability, scalability, and transactional consistency.

---

# Design Summary

The Active Ingredient aggregate is designed around a single principle:

**Describe the pharmaceutical substance itself.**

Everything related to how that substance is used inside a medicine belongs to the Medicine aggregate through MedicineIngredient.
