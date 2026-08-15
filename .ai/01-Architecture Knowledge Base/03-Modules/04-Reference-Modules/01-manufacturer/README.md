# Manufacturer Module

## Purpose

The Manufacturer module manages pharmaceutical manufacturers used throughout the system.

A Manufacturer represents the organization responsible for producing a medicine.

This module serves as a shared reference catalog for the Medicine domain.

---

## Responsibilities

The module is responsible for:

* Managing pharmaceutical manufacturers.
* Maintaining official manufacturer names.
* Managing manufacturer lifecycle (Active / Inactive).
* Providing stable references for medicines.

The module is not responsible for managing products, inventory, purchasing, or sales.

---

## Aggregate

The module contains a single aggregate:

* Manufacturer

The aggregate is intentionally lightweight and acts as a reference catalog.

---

## Relationships

The Manufacturer aggregate is referenced by:

* Medicine

through:

* ManufacturerId

The Medicine aggregate owns the relationship.

Manufacturer has no knowledge of which medicines reference it.

---

## Ownership

The Manufacturer aggregate owns only its own data.

It never owns medicines or medicine-specific information.

---

## Business Scope

The module represents pharmaceutical manufacturers only.

It does not represent:

* Suppliers
* Customers
* Business Partners
* Branches

These concepts belong to their respective modules.

---

## Module Classification

Category:

Reference Catalog Module

Reference Catalog modules provide stable business references that may be shared across many operational aggregates.

---

## Design Principles

* Single Aggregate.
* Stable business identity.
* Reference by identifier only.
* No child entities.
* No ownership of Medicine data.
* No workflow orchestration.
* Small aggregate boundary.
