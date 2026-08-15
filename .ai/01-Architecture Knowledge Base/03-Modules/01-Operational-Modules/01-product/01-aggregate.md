# Product Aggregate

## Purpose

The Product aggregate represents the commercial definition of a product.

It protects all business rules related to the product itself, independent of inventory, purchasing, sales, or medicine information.

The Product aggregate answers one question:

> What are we selling?

---

# Responsibilities

The Product aggregate is responsible for maintaining:

- Product identity
- Commercial information
- Trade name
- Product classification
- Packaging versions
- Selling units
- Unit conversion hierarchy
- Product barcodes
- Product lifecycle

---

# Business Boundary

The Product aggregate defines how a product is represented commercially.

It does not represent physical stock, medical information, purchasing, or sales.

---

# Owns

The Product aggregate owns:

- Product
- Packaging Versions
- Selling Units
- Unit Conversion Rules
- Product Barcodes

All modifications to these concepts must be performed through the Product aggregate.

---

# Does Not Own

The Product aggregate does not own:

- Inventory
- Batches
- Stock quantities
- Selling price
- Purchase documents
- Sales documents
- Medicine information
- Financial records

---

# Relationships

The Product aggregate is referenced by other modules.

Examples include:

- Medicine
- Inventory
- Purchase
- Sales

The Product aggregate never modifies those modules directly.

---

# Consistency Boundary

Every business rule related to the commercial definition of a product must remain consistent inside the Product aggregate.

Rules belonging to other business areas are enforced by their respective aggregates.

---

# Business Identity

A Product represents the commercial identity of an item offered by the pharmacy.

Its primary business identity includes:

- Trade Name
- Commercial Classification
- Packaging Definition
- Selling Units
- Commercial Barcodes

Medical characteristics are intentionally excluded from this aggregate.

Those belong to the Medicine module.

---

# Lifecycle

A Product may evolve over time.

Changes such as packaging updates or barcode changes are managed while preserving business correctness according to the Product module rules.
