# Product Module

## Purpose

The Product module defines the commercial identity of everything that can be bought, stocked, and sold in Mawthooq.

It is responsible for describing products independently from inventory, purchasing, and sales.

The Product module answers one question:

> What do we sell?

---

# Responsibilities

The Product module owns the commercial definition of a product.

It defines:

- Product identity
- Trade name
- Product classification
- Packaging hierarchy
- Selling units
- Unit conversions
- Product barcodes
- Product lifecycle

---

# Out of Scope

The Product module does not manage:

- Stock quantities
- Inventory availability
- Batches
- Purchase history
- Sales history
- Financial balances
- Medicine information

These responsibilities belong to other modules.

---

# Relationships

Product is referenced by multiple business modules.

Examples include:

- Medicine
- Inventory
- Purchase
- Sales

The Product module does not own the business rules of those modules.

---

# Documentation

- aggregate.md
- business-rules.md
- lifecycle.md
- packaging.md
- barcodes.md
- commands.md
- queries.md
- events.md
