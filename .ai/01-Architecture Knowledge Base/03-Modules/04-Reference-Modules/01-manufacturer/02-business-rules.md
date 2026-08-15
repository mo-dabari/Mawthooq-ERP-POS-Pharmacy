# Business Rules

## Purpose

This document defines the business rules governing the Manufacturer aggregate.

These rules apply only to the Manufacturer itself and do not include medicine-specific behavior.

---

# Identity Rules

## Unique Identity

Each Manufacturer has a unique identifier.

The identifier is immutable and may safely be referenced by other aggregates.

---

# Naming Rules

## Official Name

Every Manufacturer must have an official name.

The name is required.

---

## Unique Name

Two Manufacturer records cannot represent the same organization.

Duplicate manufacturer definitions are not allowed.

The uniqueness policy is enforced by the application.

---

## Name Updates

The official manufacturer name may be corrected or updated when necessary.

Updating the name does not affect existing medicine records because medicines reference the manufacturer by its identifier.

---

# Lifecycle Rules

## Active

An active Manufacturer may be selected when defining new medicines.

---

## Inactive

An inactive Manufacturer cannot be selected for new medicine definitions.

Existing medicines that already reference the manufacturer remain valid.

Historical records are never modified.

---

# Reference Rules

The Manufacturer aggregate may be referenced by many medicines.

The aggregate never owns those relationships.

Ownership always belongs to the Medicine aggregate.

---

# Medicine Rules

The Manufacturer aggregate does not store:

* Medicine information
* Product information
* Pricing
* Inventory data
* Purchase data
* Sales data

These responsibilities belong to their respective aggregates.

---

# Deletion Rules

A Manufacturer that is referenced by one or more medicines should not be physically deleted.

Instead, it should be marked as inactive.

This preserves historical consistency and prevents broken references.

---

# Business Invariants

The following invariants must always hold:

* Every Manufacturer has a valid identity.
* Every Manufacturer has an official name.
* Duplicate manufacturers are not allowed.
* Medicine-specific information never belongs to this aggregate.
* Historical references must remain valid.
