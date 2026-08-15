# Product Decision Rules

## Purpose

This document defines how the Product aggregate classifies incoming business changes.

Every requested change must be evaluated before it is applied.

The Product aggregate decides how the change should be handled.

---

# Decision Categories

Every change belongs to one of the following categories:

- Business Correction
- Commercial Evolution
- Lifecycle Change

Each category follows different business rules.

---

# Business Correction

A Business Correction fixes existing information without changing the commercial meaning of the product.

Typical examples include:

- Correcting a trade name
- Correcting descriptive information
- Correcting invalid metadata

Business Corrections update the existing Product.

---

# Commercial Evolution

Commercial Evolution changes the commercial definition of the product.

Typical examples include:

- Packaging structure changes
- Selling unit changes
- Unit hierarchy changes
- Conversion rule changes

Commercial Evolution creates a new Packaging Version.

Existing packaging definitions remain unchanged.

---

# Lifecycle Change

Lifecycle Changes affect product availability.

Typical examples include:

- Discontinuing a Product
- Re-activating a Product (if supported)

Lifecycle Changes do not alter the commercial definition of the product.

---

# Aggregate Decision

The Product aggregate is responsible for classifying every requested change.

External modules never decide how a Product should evolve.

---

# Related Documentation

- evolution.md
- packaging.md
- lifecycle.md
