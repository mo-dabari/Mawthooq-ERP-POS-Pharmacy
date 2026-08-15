# Business Rules

## Purpose

This document defines the business rules governing the TherapeuticClass aggregate.

These rules apply only to the TherapeuticClass itself and do not include medicine-specific behavior.

---

# Identity Rules

## Unique Identity

Each Therapeutic Class has a unique identifier.

The identifier is immutable and may safely be referenced by other aggregates.

---

# Naming Rules

## Official Name

Every Therapeutic Class must have an official name.

The name is required.

---

## Unique Name

Two Therapeutic Class records cannot represent the same business classification.

Duplicate therapeutic class definitions are not allowed.

The uniqueness policy is enforced by the application.

---

## Name Updates

The official therapeutic class name may be corrected or updated when necessary.

Updating the name does not affect existing medicine records because medicines reference the therapeutic class by its identifier.

---

# Lifecycle Rules

## Active

An active Therapeutic Class may be selected when defining new medicines.

---

## Inactive

An inactive Therapeutic Class cannot be selected for new medicine definitions.

Existing medicines that already reference the therapeutic class remain valid.

Historical records are never modified.

---

# Reference Rules

The TherapeuticClass aggregate may be referenced by many medicines.

The aggregate never owns those relationships.

Ownership always belongs to the Medicine aggregate.

---

# Classification Rules

Each Medicine belongs to one primary Therapeutic Class.

The TherapeuticClass aggregate defines the available classifications only.

It does not manage medicine classification decisions.

---

# Deletion Rules

A Therapeutic Class that is referenced by one or more medicines should not be physically deleted.

Instead, it should be marked as inactive.

This preserves historical consistency and prevents broken references.

---

# Business Invariants

The following invariants must always hold:

* Every Therapeutic Class has a valid identity.
* Every Therapeutic Class has an official name.
* Duplicate therapeutic classes are not allowed.
* Medicine-specific information never belongs to this aggregate.
* Historical references must remain valid.
