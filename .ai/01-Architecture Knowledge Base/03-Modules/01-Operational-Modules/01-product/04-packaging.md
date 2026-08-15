# Packaging

## Purpose

This document describes how Mawthooq models product packaging.

Packaging defines how a product is organized, stored, and sold.

It also defines the available selling units and the relationships between them.

---

# Business Philosophy

Packaging is part of the commercial definition of a product.

Changes to packaging affect how the product is represented throughout the business.

For this reason, packaging is treated as business history rather than mutable data.

---

# Packaging Version

A Product may have multiple Packaging Versions during its lifetime.

Each version represents a complete commercial definition of the product's packaging at a specific point in time.

Only one Packaging Version may be active.

Historical versions are preserved for business correctness.

---

# Why Versioning Exists

Changing packaging is not considered editing existing data.

From the business perspective, changing the packaging creates a new commercial definition.

Historical business records must continue referencing the packaging definition that existed when those records were created.

Creating a new Packaging Version preserves this relationship.

---

# Packaging Definition

A Packaging Version defines:

- Selling units
- Unit hierarchy
- Conversion hierarchy
- Unit ordering
- Commercial barcodes

Together these represent the complete packaging structure of a product.

---

# Selling Units

A Packaging Version contains one or more selling units.

Each selling unit represents a physical unit that can participate in business operations.

Examples include:

- Box
- Strip
- Bottle
- Tablet
- Capsule
- Ampoule
- Tube

The available units depend on the product.

---

# Unit Hierarchy

Selling units are organized from the largest unit to the smallest unit.

Each unit has a fixed position within the hierarchy.

The hierarchy defines how quantities are converted between units.

---

# Conversion Rules

Each selling unit defines its relationship to the next lower unit.

Conversion rules are immutable within a Packaging Version.

Changing any conversion requires creating a new Packaging Version.

---

# Commercial Barcodes

Barcodes belong to the Packaging Version.

Each selling unit may contain one or more commercial barcodes.

Barcode management is documented separately.

---

# Historical Integrity

Business documents continue referencing the Packaging Version that was active when the business event occurred.

Historical packaging information is never rewritten.

---

# Aggregate Ownership

Packaging belongs entirely to the Product aggregate.

Other modules may reference packaging information but never modify it.

---

# Related Documentation

- aggregate.md
- lifecycle.md
- barcodes.md
