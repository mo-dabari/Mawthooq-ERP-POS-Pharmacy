# Medicine Aggregate

## Purpose

The Medicine aggregate represents the medical identity of a product.

It protects the pharmaceutical information required to correctly identify, classify, and describe a medicine.

The Medicine aggregate answers one question:

> What medicine is this?

---

# Responsibilities

The Medicine aggregate is responsible for maintaining:

- Medicine identity
- Medicine name
- Dosage form
- Reference quantity
- Therapeutic classification
- Manufacturer
- Drug compositions
- Medicine lifecycle

---

# Business Boundary

The Medicine aggregate defines the pharmaceutical characteristics of a medicine.

It does not manage commercial information, inventory, purchasing, or sales.

---

# Owns

The Medicine aggregate owns:

- Medicine
- Drug compositions
- Pharmaceutical identity
- Therapeutic classification
- Dosage form
- Reference quantity

All modifications to these concepts must be performed through the Medicine aggregate.

---

# Does Not Own

The Medicine aggregate does not own:

- Trade name
- Product packaging
- Selling units
- Product barcodes
- Inventory
- Batches
- Purchase documents
- Sales documents
- Financial records

---

# Relationship with Product

Every Medicine belongs to exactly one Product.

The Product defines the commercial identity.

The Medicine defines the pharmaceutical identity.

Neither aggregate replaces the other.

---

# Consistency Boundary

Every pharmaceutical rule must remain consistent within the Medicine aggregate.

Business rules owned by other modules are enforced by their respective aggregates.

---

# Business Identity

The Medicine aggregate represents the medical identity of a product.

Its primary business identity includes:

- Medicine Name
- Dosage Form
- Active Ingredients
- Ingredient Amounts
- Therapeutic Classification
- Manufacturer
- Reference Quantity

Commercial information is intentionally excluded from this aggregate.

That responsibility belongs to the Product module.
