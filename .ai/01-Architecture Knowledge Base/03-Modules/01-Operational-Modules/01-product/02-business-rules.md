# Product Business Rules

## Purpose

This document defines the business rules that apply to the Product aggregate as a whole.

Detailed rules for specific business concepts are documented in their respective files.

---

# General Rules

## Product Independence

A Product exists independently from inventory, purchasing, sales, and medicine information.

---

## Commercial Ownership

The Product aggregate owns all rules related to the commercial definition of a product.

No other module may modify those rules directly.

---

## Business Consistency

Every modification to a Product must preserve the consistency of its commercial identity.

Invalid intermediate states are never allowed.

---

## Controlled Evolution

A Product may evolve during its lifetime.

Business changes must preserve historical correctness according to the Product module rules.

---

## Separation of Responsibilities

The Product module never manages:

- Stock
- Inventory availability
- Medicine information
- Purchases
- Sales
- Financial records

Those responsibilities belong to their respective modules.

---

# Related Documentation

Packaging rules:

- packaging.md

Barcode rules:

- barcodes.md

Lifecycle rules:

- lifecycle.md
