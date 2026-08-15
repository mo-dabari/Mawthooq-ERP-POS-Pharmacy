# Medicine Module

## Purpose

The Medicine module defines the medical identity of products classified as medicines.

It describes the pharmaceutical characteristics required for medical practice while remaining independent from commercial information.

The Medicine module answers one question:

> What medicine is this?

---

# Responsibilities

The Medicine module owns:

- Medicine identity
- Medicine name
- Dosage form
- Therapeutic classification
- Manufacturer
- Drug compositions
- Reference quantity
- Medicine lifecycle

---

# Out of Scope

The Medicine module does not manage:

- Trade names
- Packaging
- Selling units
- Barcodes
- Inventory
- Purchases
- Sales
- Financial records

Those responsibilities belong to other modules.

---

# Relationship with Product

Every Medicine is associated with exactly one Product.

The Product represents the commercial identity.

The Medicine represents the medical identity.

Together they describe a medicine from both business and pharmaceutical perspectives.

---

# Documentation

- aggregate.md
- business-rules.md
- medical-identity.md
- drug-compositions.md
- decision-rules.md
- commands.md
- queries.md
- events.md
