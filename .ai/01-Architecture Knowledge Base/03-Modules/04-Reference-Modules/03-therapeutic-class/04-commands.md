# Commands

## Purpose

This document describes the business operations that may modify the TherapeuticClass aggregate.

It focuses on business intent rather than implementation details.

---

# Create Therapeutic Class

## Description

Creates a new therapeutic class.

The therapeutic class becomes available for assignment to medicines.

---

## Business Rules

* The official name is required.
* Duplicate therapeutic classes are not allowed.
* A new therapeutic class is created in the Active state unless specified otherwise.

---

# Rename Therapeutic Class

## Description

Updates the official name of an existing therapeutic class.

This operation is intended for corrections or official naming changes.

---

## Business Rules

* The new name must be valid.
* The new name must remain unique.
* Existing medicine references are unaffected.

---

# Activate Therapeutic Class

## Description

Marks an inactive therapeutic class as available for future medicine definitions.

---

## Business Rules

* Only inactive therapeutic classes can be activated.
* Historical data remains unchanged.

---

# Deactivate Therapeutic Class

## Description

Prevents the therapeutic class from being assigned to new medicines.

Existing medicines continue to reference the therapeutic class normally.

---

## Business Rules

* Existing references remain valid.
* Historical records are preserved.
* Physical deletion is not performed.

---

# Unsupported Operations

The following operations are intentionally not supported:

* Delete Therapeutic Class
* Merge Therapeutic Classes
* Split Therapeutic Classes
* Assign Medicines
* Manage Active Ingredients
* Manage Manufacturers

These responsibilities belong to other aggregates.
