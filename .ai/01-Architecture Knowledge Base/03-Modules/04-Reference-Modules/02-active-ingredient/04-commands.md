# Commands

## Purpose

This document describes the business operations that may modify the Active Ingredient aggregate.

It focuses on business intent rather than implementation details.

---

# Create Active Ingredient

## Description

Creates a new Active Ingredient.

The ingredient becomes available for use by medicines.

---

## Business Rules

* The official name is required.
* Duplicate ingredients are not allowed.
* A new ingredient is created in the Active state unless specified otherwise.

---

# Rename Active Ingredient

## Description

Updates the official name of an existing Active Ingredient.

This operation is intended for corrections or official naming changes.

---

## Business Rules

* The new name must be valid.
* The new name must remain unique.
* Existing medicine references are unaffected.

---

# Activate Active Ingredient

## Description

Marks an inactive ingredient as available for future medicine definitions.

---

## Business Rules

* Only inactive ingredients can be activated.
* Historical data remains unchanged.

---

# Deactivate Active Ingredient

## Description

Prevents the ingredient from being selected for future medicines.

Existing medicines continue to reference the ingredient normally.

---

## Business Rules

* Existing references remain valid.
* Historical records are preserved.
* Physical deletion is not performed.

---

# Unsupported Operations

The following operations are intentionally not supported:

* Delete Active Ingredient
* Merge Active Ingredients
* Split Active Ingredient
* Manage Medicine Ingredients
* Manage Ingredient Strength
* Manage Dosage
* Manage Concentration

These responsibilities belong to other aggregates, primarily the Medicine aggregate.
