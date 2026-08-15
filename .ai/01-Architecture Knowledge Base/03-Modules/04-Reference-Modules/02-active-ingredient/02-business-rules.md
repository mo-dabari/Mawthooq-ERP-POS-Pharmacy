# Business Rules

## Purpose

This document defines the business rules governing the Active Ingredient aggregate.

These rules apply only to the Active Ingredient itself and do not include medicine-specific behavior.

---

# Identity Rules

## Unique Identity

Each Active Ingredient has a unique identifier.

The identifier is immutable and is used by other aggregates to reference the ingredient.

---

# Naming Rules

## Official Name

Every Active Ingredient must have an official name.

The name is required.

---

## Unique Name

Two Active Ingredients cannot represent the same pharmaceutical substance.

Duplicate ingredient definitions are not allowed.

The uniqueness policy is enforced by the application.

---

## Name Updates

The official name may be corrected or updated when necessary.

Updating the name does not affect historical medicine records because medicines reference the ingredient by its identifier.

---

# Lifecycle Rules

## Active

An active ingredient may be used when creating or updating medicines.

---

## Inactive

An inactive ingredient cannot be selected for new medicine definitions.

Existing medicines that already reference the ingredient remain valid.

Historical records are never modified.

---

# Reference Rules

The Active Ingredient aggregate may be referenced by many medicines.

The aggregate never owns those relationships.

Ownership always belongs to the Medicine aggregate through MedicineIngredient.

---

# Medicine Rules

The aggregate does not store:

* Strength
* Quantity
* Dosage
* Concentration
* Units

These values belong to MedicineIngredient.

---

# Deletion Rules

An Active Ingredient that is referenced by one or more medicines should not be physically deleted.

Instead, it should be marked as inactive.

This preserves historical consistency and prevents broken references.

---

# Business Invariants

The following invariants must always hold:

* Every Active Ingredient has a valid identity.
* Every Active Ingredient has an official name.
* Duplicate ingredients are not allowed.
* Medicine-specific information never belongs to this aggregate.
* Historical references must remain valid.
