# Commands

## Purpose

This document describes the business operations that may modify the Manufacturer aggregate.

It focuses on business intent rather than implementation details.

---

# Create Manufacturer

## Description

Creates a new pharmaceutical manufacturer.

The manufacturer becomes available for use by medicines.

---

## Business Rules

* The official name is required.
* Duplicate manufacturers are not allowed.
* A new manufacturer is created in the Active state unless specified otherwise.

---

# Rename Manufacturer

## Description

Updates the official name of an existing manufacturer.

This operation is intended for corrections or official naming changes.

---

## Business Rules

* The new name must be valid.
* The new name must remain unique.
* Existing medicine references are unaffected.

---

# Activate Manufacturer

## Description

Marks an inactive manufacturer as available for future medicine definitions.

---

## Business Rules

* Only inactive manufacturers can be activated.
* Historical data remains unchanged.

---

# Deactivate Manufacturer

## Description

Prevents the manufacturer from being selected for future medicine definitions.

Existing medicines continue to reference the manufacturer normally.

---

## Business Rules

* Existing references remain valid.
* Historical records are preserved.
* Physical deletion is not performed.

---

# Unsupported Operations

The following operations are intentionally not supported:

* Delete Manufacturer
* Merge Manufacturers
* Split Manufacturer
* Manage Medicines
* Manage Inventory
* Manage Purchases
* Manage Sales

These responsibilities belong to other aggregates.
