# Medicine Business Rules

## Purpose

This document defines the business rules that apply to the Medicine aggregate as a whole.

Detailed pharmaceutical rules are documented in their respective documents.

---

# General Rules

## Product Dependency

A Medicine cannot exist independently.

Every Medicine must be associated with exactly one Product.

---

## Medical Ownership

The Medicine aggregate owns the pharmaceutical definition of a medicine.

No other module may modify that definition directly.

---

## Identity Separation

Medical identity and commercial identity are separate business concepts.

Neither replaces the other.

---

## Pharmaceutical Consistency

The Medicine aggregate always maintains a complete and consistent pharmaceutical definition.

Invalid intermediate states are never allowed.

---

## Reference Data

The Medicine aggregate references pharmaceutical reference data.

It does not own the lifecycle or management of those reference records.

---

## Module Independence

The Medicine module never manages:

- Product packaging
- Selling units
- Barcodes
- Inventory
- Purchasing
- Sales
- Financial information

Those responsibilities belong to their respective modules.

---

# Related Documentation

- medical-identity.md
- drug-compositions.md
